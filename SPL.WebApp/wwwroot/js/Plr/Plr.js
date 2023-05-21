
//Variables
let viewModel;
let btnRequest = document.getElementById("btnRequest");
let btnClear = document.getElementById("btnClear");
let btnLoadTemplate = document.getElementById("btnLoadTemplate");
let noSerieInput = document.getElementById("NoSerie");
let CommentsInput = document.getElementById("Comments");
let noPruebaInput = document.getElementById("NoPrueba");
let claveIdiomaInput = document.getElementById("ClaveIdioma");
let clavePruebaInput = document.getElementById("ClavePrueba");
let cantidadTiempoInput = document.getElementById("CantidadDeTiempo");
let cantidadTensionInput = document.getElementById("CantidadDeTension");
let reactanciaLinealInput = document.getElementById("ReactanciaLinealDeDiseno");
let tensionNominalInput = document.getElementById("TensionNominal");
let spreadsheetElement;
let treeViewKendoElement;
let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");
let panelExcel = document.getElementById("spreadsheet");

//Mostrar info necesaria para consultar el reporte
document.querySelector('#btnInfo').addEventListener('click', () => {
    var text =
        'Considerar que para este reporte:<br><br>'
        + 'El “No. De Serie” del Aparato se debe de encontrar registrado en SPL.<br>'
        + 'Se debe contar con Información en la pantalla: “Diseño del Aparato”.<br>'
        + 'Se deben de contar con las plantillas cargadas para las pruebas correspondientes del reporte:<br>'
        + '<ul>'
        + '<li>Reactancia (Ambos Idiomas)</li>'
        + '<li>Tiempo (Ambos Idiomas)</li>'
        + '</ul>';
    ShowWarningMessageHtmlContent(text);
});

//Evento

$("#ClavePrueba").change(function(){
    if(clavePruebaInput.value === "TIE"){
        reactanciaLinealInput.disabled = true;
        tensionNominalInput.disabled = true ;
        cantidadTensionInput.disabled = true ;
        cantidadTiempoInput.disabled = false ;

        $("#ReactanciaLinealDeDiseno_validationMessage").remove()
        $("#CantidadDeTension_validationMessage").remove()

        reactanciaLinealInput.value = ''
        tensionNominalInput.value = ''
        cantidadTensionInput.value = ''

    }else{
        reactanciaLinealInput.disabled = false;
        tensionNominalInput.disabled = false ;
        cantidadTensionInput.disabled = false ;
        cantidadTiempoInput.disabled = true ;

        $("#CantidadDeTiempo_validationMessage").remove()
        reactanciaLinealInput.value = ''
        cantidadTiempoInput.value = ''

    }
});

$("#NoSerie").focus();

$("#NoSerie").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $("#btnRequest").click();
    }
});

btnClear.addEventListener("click", function () {

    noSerieInput.value = '';
    //noPruebaInput.value = '';
    CommentsInput.value = '';
    claveIdiomaInput.value = '';
    clavePruebaInput.value = '';
    reactanciaLinealInput.value = ''
    tensionNominalInput.value = ''
    cantidadTensionInput.value = ''
    cantidadTiempoInput.value = ''

    reactanciaLinealInput.disabled = true;
    tensionNominalInput.disabled = true ;
    cantidadTensionInput.disabled = true ;
    cantidadTiempoInput.disabled = true ;

    $("#ReactanciaLinealDeDiseno_validationMessage").remove()
    $("#CantidadDeTension_validationMessage").remove()
    $("#CantidadDeTiempo_validationMessage").remove()
    
    btnRequest.disabled = false;
    btnClear.disabled = false;
    btnLoadTemplate.disabled = true;
    noSerieInput.disabled = false;

     $("#TensionNominal option[value!='']").each(function() {
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
        $("#loader").css("display", "block");
        btnRequest.disabled = true;
        btnClear.disabled = false;
        GetTemplateJSON();
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

//Functions

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
        $.each(response.TensionesNominales , function(i,val){
            $("#TensionNominal").append("<option value='"+val +"'>"+val+"</option>");
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


async function GetFilterJSON() {
    var path = path = "/Plr/GetFilter/";

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
    var path = path = "/Plr/GetTemplate/";

    var url = new URL(domain + path),
        params = { 
            nroSerie: noSerieInput.value,
            keyTest: clavePruebaInput.value,
            lenguage: claveIdiomaInput.value,
            rldnt : reactanciaLinealInput.value,
            nominalVoltage  : tensionNominalInput.value,
            amountOfTensions : cantidadTensionInput.value,
            amountOfTime: cantidadTiempoInput.value,
            comment: CommentsInput.value
            }

    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))
    $("#loader").css("display", "none");
    var win = window.open(url.toString(), '_blank');
    win.focus();
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
    viewModel.ClavePrueba = clavePruebaInput.value;
    //viewModel.NoPrueba = noPruebaInput.value;
    viewModel.Comments = CommentsInput.value;
    viewModel.NoSerie = noSerieInput.value;

    viewModel.TensionNominal = tensionNominalInput.value;
    viewModel.CantidadDeTiempo = cantidadTiempoInput.value ;
    viewModel.CantidadDeTension = cantidadTensionInput.value;
    viewModel.ReactanciaLinealDeDiseno = reactanciaLinealInput.value

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

        GetPDFJSON(id, "PLR").then(
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
    var path = path = "/Plr/GetPDFReport/";

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
