
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
let nroCiclosInput = document.getElementById("NroCiclosText");
let nroCiclosSelectInput = document.getElementById("NroCiclosSelect");
let tiempoTotalInput = document.getElementById("TiempoTotal");
let tiempoIntervaloInput = document.getElementById("TiempoIntervalo");
let nivelHoraInput = document.getElementById("NivelHora");
let nivelRealceInput = document.getElementById("NivelRealce");
let descargaPCInput = document.getElementById("DescargaPC");
let descargaUVInput = document.getElementById("DescargaUV");
let incrementoMaxInput = document.getElementById("IncrementoMaxPC");
let nivelesTensionInput = document.getElementById("NivelesTension");
let tipoMedicionInput = document.getElementById("TipoMedicion");
let terminalesProbarInput = document.getElementById("TerminalesProbar");
//noPruebaInput.value = '';

let claveIdiomaInput = document.getElementById("ClaveIdioma");
let spreadsheetElement;
let treeViewKendoElement;

let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");
let panelExcel = document.getElementById("spreadsheet");

//Mostrar info necesaria para consultar el reporte
document.querySelector('#btnInfo').addEventListener('click', () => {
    var text = 'Considerar que para este reporte:<br><br>'
        + 'El “No. De Serie” del Aparato se debe de encontrar registrado en SPL.<br>'
        + 'Contar con Información en la pantalla: “Diseño del Aparato”.<br>'
        + 'Se deben de contar con las plantillas cargadas para las pruebas correspondientes del reporte.';
    ShowWarningMessage(text);
});

//Evento
$("#NoSerie").focus();

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
    nroCiclosSelectInput.value = '';
    nroCiclosInput.value = '';
    nroCiclosInput.disabled = true;
    tiempoIntervaloInput.value = '5';
    tiempoTotalInput.value = '60';
    descargaPCInput.value = '250'
    descargaUVInput.value = '100'
    nivelRealceInput.value = ''
    nivelHoraInput.value = ''
    incrementoMaxInput.value = '50'
    nivelesTensionInput.value = ''
    tipoMedicionInput.value = 'Picolumns'
    terminalesProbarInput.value = ''
    btnRequest.disabled = false;
    btnClear.disabled = false;
    btnLoadTemplate.disabled = true;
    noSerieInput.disabled = false;

    $("#NivelesTension option[value!='']").each(function () {
        $(this).remove();
    });

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
    var path = path = "/Tdp/GetFilter/";

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

var path = path = "/Tdp/GetTemplate/";
var url = new URL(domain + path),
    params = { 
        nroSerie: noSerieInput.value,
        keyTest: pruebaInput.value,
        lenguage: claveIdiomaInput.value,
        noCycles: nroCiclosSelectInput.value === "Otro" ? nroCiclosInput.value : nroCiclosSelectInput.value  ,
        totalTime : tiempoTotalInput.value,
        interval : tiempoIntervaloInput.value,
        timeLevel : nivelHoraInput.value,
        outputLevel : nivelRealceInput.value,
        descMayPc : descargaPCInput.value,
        descMayMv : descargaUVInput.value,
        incMaxPc: incrementoMaxInput.value,
        voltageLevels: nivelesTensionInput.value,
        measurementType : tipoMedicionInput.value,
        terminalsTest: terminalesProbarInput.value,
        comments: CommentsInput.value
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


        GetPDFJSON(id, "TDP").then(
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

async function GetPDFJSON(code, tyoeReport) {
    var path = path = "/Tdp/GetPDFReport/";

    var url = new URL(domain + path),
        params = { code: code, typeReport: tyoeReport }
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