
//Variables
let viewModel;
let settingsToDisplayPCIReportsDTO;
let btnRequest = document.getElementById("btnRequest");
let btnClear = document.getElementById("btnClear");
let btnLoadTemplate = document.getElementById("btnLoadTemplate");
let noSerieInput = document.getElementById("NoSerie");
let CommentsInput = document.getElementById("Comments");
let noPruebaInput = document.getElementById("NoPrueba");
let claveIdiomaInput = document.getElementById("ClaveIdioma");
let incluyeTerciarioInput = document.getElementById("IncluyeTerciario");
let conexionInput = document.getElementById("Connections");
let pruebaInput = document.getElementById("Pruebas");


let spreadsheetElement;
let treeViewKendoElement;

let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");
let panelExcel = document.getElementById("spreadsheet");

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
        + '<li>Única (Ambos Idiomas)</li>'
        + '</ul>'
        + '<br>'
        + 'Debe de existir información en la pantalla: “Tensión de la Placa”.';
    ShowWarningMessageHtmlContent(text);
});

$("#Prueba").change(function(){

   
})

$("#NoSerie").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $("#btnRequest").click();
    }
});

//Requests
btnClear.addEventListener("click", function () {

    noSerieInput.disabled = false
    noSerieInput.value = '';
    CommentsInput.value = '';
    claveIdiomaInput.value = '';
    btnRequest.disabled = false;
    btnLoadTemplate.disabled = true;
    pruebaInput.value = ''
    incluyeTerciarioInput.value = 'No'
   
    if (treeViewKendoElement !== undefined) {
        kendo.destroy(treeViewKendoElement);
        $("#treeview-kendo").empty();
    }
   
    $("#Connections option[value!='']").each(function() {
        $(this).remove();
    });

    $(".k-upload-files").remove()
    listaConexiones = [];
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
    var path = path = "/Pir/GetFilter/";

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
var path = path = "/Pir/GetTemplate/";
var url = new URL(domain + path),
    params = { 
        nroSerie: noSerieInput.value,
        keyTest: pruebaInput.value,
        lenguage: claveIdiomaInput.value,
        connection : cons,
        includesTertiary : incluyeTerciarioInput.value,
/*        voltageLevel : nivelTensionInput.value,*/
        pruebasPir: viewModel.PirPruebasDTO,
        comments: CommentsInput.value
    }

    btnLoadTemplate.disabled = false;

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

var listaConexiones = [];

function LoadForm(response) {
    console.log(response)
    claveIdiomaInput.value = response.ClaveIdioma;
    pruebas = response.CharacteristicsArtifact;

    $("#Connections option[value!='']").each(function() {
        $(this).remove();
    });

    $.each(viewModel.PirPruebasDTO, function (index, value) {
        listaConexiones.push(value.Nombre);
        if (value.Tensiones.Tension1 != 0 && value.Tensiones.Tension2 != 0) {
            listaConexiones.push(value.Nombre + " - " + value.Tensiones.Tension1);
            listaConexiones.push(value.Nombre + " - " + value.Tensiones.Tension2);
        }
    })

    if (listaConexiones.length > 1) {
        listaConexiones.unshift('Todas');
    }

    $.each(listaConexiones, function (index, value) {
        $("#Connections").append("<option value='" + value + "'>" + value + "</option>")
    })

    if (response.VoltageKV.TensionKvTerciario1 !== null && response.VoltageKV.TensionKvTerciario1 !== 0){
        incluyeTerciarioInput.value = 'Si'
    }else{
        incluyeTerciarioInput.value = 'No'
    }


}
var ResultValidations;
var cons;
function ValidateForm() {

    ResultValidations = $("#form_menu_sup").data("kendoValidator").validate();
    if (ResultValidations) {
        cons = $('#Connections').val();

        if (cons == undefined) {
            ShowFailedMessage('Se requiere mínimo una Conexión');
            return false;
        }

        if (cons.length < 1) {
            ShowFailedMessage('Se requiere mínimo una Conexión');
            return false;
        }

        if (cons[0] == 'Todas') {
            cons = listaConexiones.slice(1);
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
    viewModel.IncluyeTerciario = incluyeTerciarioInput.value;
/*    viewModel.NivelTension = nivelTensionInput.value;*/
    viewModel.Conexion = conexionInput.value
    console.log(viewModel)
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

        GetPDFJSON(id, "PIR").then(
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
    var path = path = "/Pir/GetPDFReport/";

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