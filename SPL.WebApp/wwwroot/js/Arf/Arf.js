
//Variables
let viewModel;
let settingsToDisplayPCIReportsDTO;
let btnRequest = document.getElementById("btnRequest");
let btnClear = document.getElementById("btnClear");
let btnLoadTemplate = document.getElementById("btnLoadTemplate");
let noSerieInput = document.getElementById("NoSerie");
let CommentsInput = document.getElementById("Comments");
let pruebaInput = document.getElementById("Pruebas");
let noPruebaInput = document.getElementById("NoPrueba");
let claveIdiomaInput = document.getElementById("ClaveIdioma");

let equipoInput = document.getElementById("Equipo");
let terSegundaInput = document.getElementById("TerciarioOSegunda");
let terciarioDisponibleInput = document.getElementById("TerciarioDisponible");
let nivelesTensionInput = document.getElementById("NivelesTension");
let nivelAceiteLabInput = document.getElementById("NivelAceiteLab");
let nivelAceiteEmpInput = document.getElementById("NivelAceiteEmp");
let boquillasLabInput = document.getElementById("BoquillasLab");
let boquillasEmpInput = document.getElementById("BoquillasEmp");
let nucleosLabInput = document.getElementById("NucleosLab");
let nucleosEmpInput = document.getElementById("NucleosEmp");
let terciarioLabInput = document.getElementById("TerciarioLab");
let terciarioEmpInput = document.getElementById("TerciarioEmp");


let spreadsheetElement;
let treeViewKendoElement;

let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");
let panelExcel = document.getElementById("spreadsheet");

nivelAceiteEmpInput.disabled = true
boquillasEmpInput.disabled = true
terciarioEmpInput.disabled = true
nucleosEmpInput.disabled = true

nivelAceiteLabInput.disabled = true
boquillasLabInput.disabled = true
terciarioLabInput.disabled = true
nucleosLabInput.disabled = true

//Evento
$("#NoSerie").focus();

//Mostrar info necesaria para consultar el reporte
document.querySelector('#btnInfo').addEventListener('click', () => {
    var text =
        'Considerar que para este reporte:<br><br>'
        + 'El “No. De Serie” del Aparato se debe de encontrar registrado en SPL.<br>'
        + 'Se debe contar con Información en la pantalla: “Diseño del Aparato”.<br>'
        + 'Se deben de contar con las plantillas cargadas para las pruebas correspondientes del reporte:<br>'
        + '<ul>'
        + '<li>En Laboratorio (Ambos Idiomas)</li>'
        + '<li>En Laboratorio y Plataforma (Ambos Idiomas)</li>'
        + '<li>En Plataforma (Ambos Idiomas)</li>'
        + '</ul>';
    ShowWarningMessageHtmlContent(text);
});

$("#NroCiclosSelect").change(function () {
    if (this.value === "Otro") {
        nroCiclosInput.disabled = false
        nroCiclosInput.value = ""
        $("#NroCiclosText_validationMessage").remove();
    } else {
        nroCiclosInput.disabled = true
        nroCiclosInput.value = ""
        $("#NroCiclosText_validationMessage").remove();
    }
})

$("#TerciarioOSegunda").change(function () {

    if (this.value !== "2B" && this.value !== "CT") {
        terciarioLabInput.disabled = true
        terciarioEmpInput.disabled = true
    } else {
        if (pruebaInput.value === "LAB") {
            terciarioLabInput.disabled = false
            terciarioEmpInput.disabled = true
        }

        if (pruebaInput.value === "PLA") {
            terciarioEmpInput.disabled = false
            terciarioLabInput.disabled = true
        }

        if (pruebaInput.value === "LYP") {
            terciarioLabInput.disabled = false
            terciarioEmpInput.disabled = false
        }

    }

    if (this.value === "CT") {
        terciarioDisponibleInput.value = 'Si'
    }
})

$("#Pruebas").change(function () {
    if (this.value === "LAB") {
        nivelAceiteEmpInput.disabled = true
        boquillasEmpInput.disabled = true
        terciarioEmpInput.disabled = true
        nucleosEmpInput.disabled = true

        nivelAceiteLabInput.disabled = false
        boquillasLabInput.disabled = false
        terciarioLabInput.disabled = false
        nucleosLabInput.disabled = false

        if (terSegundaInput.value !== "2B" && terSegundaInput.value !== "CT") {
            terciarioLabInput.disabled = true
            terciarioEmpInput.disabled = true
        } else {
            terciarioLabInput.disabled = false
        }

    }
    if (this.value === "LYP") {
        nivelAceiteEmpInput.disabled = false
        boquillasEmpInput.disabled = false
        terciarioEmpInput.disabled = false
        nucleosEmpInput.disabled = false

        nivelAceiteLabInput.disabled = false
        boquillasLabInput.disabled = false
        terciarioLabInput.disabled = false
        nucleosLabInput.disabled = false

        if (terSegundaInput.value !== "2B" && terSegundaInput.value !== "CT") {
            terciarioLabInput.disabled = true
            terciarioEmpInput.disabled = true
        } else {
            terciarioLabInput.disabled = false
            terciarioEmpInput.disabled = false
        }
    }
    if (this.value === "PLA") {

        nivelAceiteEmpInput.disabled = false
        boquillasEmpInput.disabled = false
        terciarioEmpInput.disabled = false
        nucleosEmpInput.disabled = false

        nivelAceiteLabInput.disabled = true
        boquillasLabInput.disabled = true
        terciarioLabInput.disabled = true
        nucleosLabInput.disabled = true

        if (terSegundaInput.value !== "2B" && terSegundaInput.value !== "CT") {
            terciarioEmpInput.disabled = true
            terciarioEmpInput.disabled = true
        } else {
            terciarioEmpInput.disabled = false
        }
    }

})

$("#NoSerie").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $("#btnRequest").click();
    }
});
//Evento

//Requests
btnClear.addEventListener("click", function () {

    noSerieInput.value = '';
    //noPruebaInput.value = '';
    CommentsInput.value = '';
    claveIdiomaInput.value = '';
    pruebaInput.value = '';
    nivelesTensionInput.value = ''
    btnRequest.disabled = false;
    btnClear.disabled = false;
    btnLoadTemplate.disabled = true;
    noSerieInput.disabled = false;
    equipoInput.value = '';
    terSegundaInput.value = ''
    terciarioDisponibleInput.value = 'Si'
    nivelesTensionInput.value = ''
    nivelAceiteEmpInput.value = 'Lleno'
    nivelAceiteLabInput.value = 'Lleno'

    boquillasEmpInput.value = 'Operación'
    boquillasLabInput.value = 'Operación'

    nucleosEmpInput.value = 'Aterrizado'
    nucleosLabInput.value = 'Aterrizado'

    terciarioEmpInput.value ='Aterrizado'
    terciarioLabInput.value ='Aterrizado'


    $("#NivelesTension option[value!='']").each(function () {
        $(this).remove();
    });

    $("#TerciarioOSegunda option[value!='']").each(function () {
        $(this).remove();
    });

    $("#TerciarioOSegunda").append("<option value='ST'>Sin Terciario</option>")
    $("#TerciarioOSegunda").append("<option value='CT'>Con Terciario</option>")
    $("#TerciarioOSegunda").append("<option value='2B'>2da. Baja</option>")

    if (treeViewKendoElement !== undefined) {
        kendo.destroy(treeViewKendoElement);
        $("#treeview-kendo").empty();
    }

});

btnLoadTemplate.addEventListener("click", function () {
    if (ValidateForm()) {
        MapToViewModel();
        btnRequest.disabled = true;
        btnLoadTemplate.disabled = true;
        btnClear.disabled = false;
        $("#loader").css("display", "block");
        GetTemplateJSON()
    }
    else {
        ShowFailedMessage("Faltan campos por llenar")
        $("#loader").css("display", "none");
    }
});

btnRequest.addEventListener("click", function () {
 
    
    if (noSerieInput.value === undefined || noSerieInput.value === "" || noSerieInput.value === null) {
        $(`#NoSerieSpand`).text("Requerido");
        return;
    }
    else {
        $(`#NoSerieSpand`).text("");
    }

    if (!(/^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ0-9-@]*$/.test(noSerieInput.value))) {
        $(`#NoSerieSpand`).text("Character(es) no permitido.");
        return;
    }
    else {
        $(`#NoSerieSpand`).text("");
    }


    $("#loader").css("display", "block");
    if (noSerieInput.value) {
        GetFilterJSON(null).then(
            data => {
                if (data.response.Code !== -1) {
                    btnSave.disabled = false;
                    noSerieInput.disabled = true;

                    viewModel = data.response.Structure;


                    console.log(data.response.Structure );
                    LoadTreeView(viewModel.TreeViewItem);
                    LoadForm(viewModel);

                    btnRequest.disabled = true;
                    btnClear.disabled = false;
                    btnLoadTemplate.disabled = false;
                }
                else {
                    ShowFailedMessage(data.response.Description);
                }
                $("#loader").css("display", "none");
            }
        );
    }
    else {
        $("#loader").css("display", "none");

        ShowSuccessMessage('Por favor ingrese un No. Serie.');
        btnSave.disabled = true;
        btnLoadDataSidco.disabled = true;
    }
});

//Requests


//Functions
async function GetFilterJSON() {
    var path = path = "/Arf/GetFilter/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const result = await response.json();
        return result;
    }

    else {
        ShowFailedMessage('Error, por favor contacte al administrador del sistema.');
        return null;
    }
}


async function GetTemplateJSON() {

var path = path = "/Arf/GetTemplate/";
var url = new URL(domain + path),
    params = { 
        nroSerie: noSerieInput.value,
        keyTest: pruebaInput.value,
        lenguage: claveIdiomaInput.value,
        levelsVoltage: nivelesTensionInput.value,
        team: equipoInput.value,
        tertiary2Low: terSegundaInput.value,
        tertiaryDisp: terciarioDisponibleInput.value,
        comments: CommentsInput.value,
        nivelAceiteLab : nivelAceiteLabInput.value,
        nivelAceitePla :  nivelAceiteEmpInput.value,
        boquillasLab :  boquillasLabInput.value,
        boquillasPla: boquillasEmpInput.value,
        nucleoLab: nucleosLabInput.value,
        nucleoPla: nucleosEmpInput.value,
        terLab: terciarioLabInput.value,
        terPla: terciarioEmpInput.value,
    }

Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

$("#loader").css("display", "none");
var win = window.open(url.toString(), '_blank');
win.focus();
}


function LoadTreeView(treeViewModel) {
    treeViewKendoElement = $("#treeview-kendo").kendoTreeView({
        template: kendo.template($("#treeview").html()),
        dataSource: treeViewModel,
        dragAndDrop: false,
        select : onSelect,
        checkboxes: false,
        loadOnDemand: true
    });

}

function LoadForm(response) {
    claveIdiomaInput.value = response.ClaveIdioma;
    //noPruebaInput.value = response.NoPrueba;

    pruebas = response.CharacteristicsArtifact;

    $.each(response.VoltageLevels, function (i, val) {
        if (val.Value !== "") {
            $("#NivelesTension").append("<option value='" + val.Value + "'>" + val.Text + "</option>");
        }
    });

    $("#TerciarioOSegunda option[value!='']").each(function () {
        $(this).remove();
    });

    $.each(response.Terciarios, function (i, val) {
         $("#TerciarioOSegunda").append("<option value='" + val.Value + "'>" + val.Text + "</option>");
    });

    if (response.Terciarios.length === 1) {
        $("#TerciarioOSegunda").prop("selectedIndex", 1)
    }

    if (terSegundaInput.value === "ST") {
        terciarioDisponibleInput.value = "No"
    }

    if (terSegundaInput.value === "CT" || terSegundaInput.value === "2B") {
        if (pruebaInput.value === "LYP") {
            terciarioLabInput.disabled = false
            terciarioEmpInput.disabled = false
        } else if (pruebaInput.value === "LAB") {
            terciarioLabInput.disabled = false
            terciarioEmpInput.disabled = true
        } else if (pruebaInput.value === "PLA") {
            terciarioLabInput.disabled = true
            terciarioEmpInput.disabled = false
        } else {
            terciarioLabInput.disabled = true
            terciarioEmpInput.disabled = true
        }
    }


}
var ResultValidations;

function ValidateForm() {

    ResultValidations = $("#form_menu_sup").data("kendoValidator").validate();
    if (ResultValidations) {
        return true;
    }else {
        return false;
    }
}

function LoadPDF(nameFile, File) {    
    var byteCharacters = atob(File);
    var byteNumbers = new Array(byteCharacters.length);
    for (var i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    var byteArray = new Uint8Array(byteNumbers);
    var file = new Blob([byteArray], { type: 'application/pdf;base64' });
    var fileURL = URL.createObjectURL(file);
    window.open(fileURL);
}

function showPdfInNewTab(base64Data, fileName) {

    let pdfWindow = window.open("");
    pdfWindow.document.write("<html<head><title>" + fileName + "</title><style>body{margin: 0px;}iframe{border-width: 0px;}</style></head>");
    pdfWindow.document.write("<body><embed width='100%' height='100%' src='data:application/pdf;base64, " + encodeURI(base64Data) + "#toolbar=0&navpanes=0&scrollbar=0'></embed></body></html>");
}

function MapToViewModel(loadWorkbook = false, loadOfficial = false) {
    viewModel.ClaveIdioma = claveIdiomaInput.value;
    viewModel.Prueba = pruebaInput.value;
    //viewModel.NoPrueba = noPruebaInput.value;
    viewModel.Comments = CommentsInput.value;
    viewModel.NoSerie = noSerieInput.value;


    if (loadWorkbook)
        viewModel.Workbook.sheets = $("#spreadsheet").getKendoSpreadsheet().toJSON().sheets;
    if (loadOfficial)
        viewModel.OfficialWorkbook = officialWorkbook;
}

function onSelect(e) {
    var text = this.text(e.node);

    if (text.split('.').length > 1) {
        var id = text.split('.')[0].split('-')[2].split('_')[1];
        console.log("Selecting: " + this.text(e.node));
        console.log(id);
        GetPDFJSON(id, "ARF", text.split('.')).then(
            data => {
                if (data != null) {

                    LoadPDF(this.text(e.node), data.data);
                }
                else {
                    ShowFailedMessage(data.response.Description);
                }
                $("#loader").css("display", "none");
            }
        );
    }

}

async function GetPDFJSON(code, tyoeReport, reportName) {
    var path = path = "/Arf/GetPDFReport/";

    var url = new URL(domain + path),
        params = { code: code, typeReport: tyoeReport, reportName: reportName }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const result = await response.json();
        return result;
    }

    else {
        ShowFailedMessage('Error, por favor contacte al administrador del sistema.');
        return null;
    }
}

function clearPosiciones(){
   
}
//Functions