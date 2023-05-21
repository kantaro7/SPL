
//Variables
let viewModel;
let settingsToDisplayPCIReportsDTO;
let btnRequest = document.getElementById("btnRequest");
let btnInfo = document.querySelector('#btnInfo');
let btnClear = document.getElementById("btnClear");
let btnLoadTemplate = document.getElementById("btnLoadTemplate");
let noSerieInput = document.getElementById("NoSerie");
let CommentsInput = document.getElementById("Comments");
let noPruebaInput = document.getElementById("NoPrueba");
let claveIdiomaInput = document.getElementById("ClaveIdioma");
let incluyeTerciarioInput = document.getElementById("Low");
let conexionInput = document.getElementById("Connections");
let pruebaInput = document.getElementById("ClavePrueba");


let spreadsheetElement;
let treeViewKendoElement;

let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");
let panelExcel = document.getElementById("spreadsheet");

//Evento
$("#NoSerie").focus();

$("#Prueba").change(function(){

   
})

//Mostrar info necesaria para consultar el reporte
document.querySelector('#btnInfo').addEventListener('click', () => {
    var text =
        'Considerar que para este reporte:<br><br>'
        + 'El “No. De Serie” del Aparato se debe de encontrar registrado en SPL.<br>'
        + 'Se debe contar con Información en la pantalla: “Diseño del Aparato”.<br>'
        + 'Se deben de contar con las plantillas cargadas para las pruebas correspondientes del reporte:<br>'
        + '<ul>'
        + '<li>Única (Ambos Idiomas)</li>'
        + '</ul>'
        + '<br>'
        + 'Debe de existir información en la pantalla: “Tensión de la Placa”.';
    ShowWarningMessageHtmlContent(text);
});

$("#NoSerie").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $("#btnRequest").click();
    }
});


//Requests
btnClear.addEventListener("click", function () {

    noSerieInput.disabled = false
    noSerieInput.value = '';
    //noPruebaInput.value = '';
    CommentsInput.value = '';
    claveIdiomaInput.value = '';
    btnRequest.disabled = false;
    btnLoadTemplate.disabled = true;
   
    if (treeViewKendoElement !== undefined) {
        kendo.destroy(treeViewKendoElement);
        $("#treeview-kendo").empty();
    }

    $("#Connections option[value!='']").each(function() {
        $(this).remove();
    });

    $(".k-upload-files").remove()
    listaConexiones = [];
    pruebaInput.value = '';
    incluyeTerciarioInput.value = 'No';

});

btnLoadTemplate.addEventListener("click", function () {
var error = false
    if(error){
        ShowFailedMessage("Se han encontrados errores los archivos.")
        return
    }

    if (ValidateForm()) {
        MapToViewModel();
        btnRequest.disabled = true;
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
    var path = path = "/Pim/GetFilter/";

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
var path = path = "/Pim/GetTemplate/";
var url = new URL(domain + path),
    params = { 
        noSerie: noSerieInput.value,
        clavePrueba: pruebaInput.value,
        claveIdioma: claveIdiomaInput.value,
        connections: cons,
        applyLow : incluyeTerciarioInput.value,
        comment: CommentsInput.value
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
    console.log(response)
    claveIdiomaInput.value = response.ClaveIdioma;
    //noPruebaInput.value = response.NoPrueba;
    pruebas = response.CharacteristicsArtifact;

    $("#Connections option[value!='']").each(function() {
        $(this).remove();
    });

    $.each(viewModel.ConnectionsList,function(index,value){
        $("#Connections").append("<option value='"+value+"'>"+value+"</option>")
    })
}
var ResultValidations;
var cons;
function ValidateForm() {
    ResultValidations = $("#form_menu_sup").data("kendoValidator").validate();
    if (ResultValidations) {
        cons = $('#Connections').val();

        if (cons.length < 1) {
            ShowFailedMessage('Se requiere mínimo una Conexión.');
            return false;
        }

        if (cons[0] === 'Todas') {
            cons = viewModel.ConnectionsList.slice(1);
        }

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
    //viewModel.NoPrueba = noPruebaInput.value;
    viewModel.Comments = CommentsInput.value;
    viewModel.NoSerie = noSerieInput.value;
    viewModel.Low = incluyeTerciarioInput.value;
    viewModel.Connection = conexionInput.value
    if (loadWorkbook)
        viewModel.Workbook.sheets = $("#spreadsheet").getKendoSpreadsheet().toJSON().sheets;
}

function onSelect(e) {
    var text = this.text(e.node);

    if (text.split('.').length > 1) {
        var id = text.split('.')[0].split('-')[2].split('_')[1];
        console.log("Selecting: " + this.text(e.node));
        console.log(id);


        GetPDFJSON(id, "PIM").then(
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
    var path = path = "/Pim/GetPDFReport/";

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


/*MANEJO DE CARGA DE ARCHIVOS */

/************************************/
function onError(e){
    console.log(e)
    console.log( e.sender)
}


//Functions