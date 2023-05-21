
//Variables
let viewModel;
let settingsToDisplayPCIReportsDTO;
let claveIdiomaInput = document.getElementById("ClaveIdioma");
let btnRequest = document.getElementById("btnRequest");
let btnClear = document.getElementById("btnClear");
let btnLoadTemplate = document.getElementById("btnLoadTemplate");
let noSerieInput = document.getElementById("NoSerie");
let CommentsInput = document.getElementById("Comments");
let noPruebaInput = document.getElementById("NoPrueba");
let pruebaInput = document.getElementById("Pruebas");
let enfriamiento = document.getElementById("Enfriamiento");
let selectCargaInput = document.getElementById("SelectCargaInformacion");
let fechaInput = document.getElementById("Fecha");
let normaInput = document.getElementById("Norma");
let cantMedicionesInput = document.getElementById("CantMediciones");
let alimentacionInput = document.getElementById("Alimentacion");
let alimentacionValueInput = document.getElementById("ValorAlimentacion");
let laboratorioInput = document.getElementById("Laboratorio");
let alturasInput = document.getElementById("Altura");
let spreadsheetElement;
let treeViewKendoElement;

let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");
let panelExcel = document.getElementById("spreadsheet");
let cantidadColumnas;
let cantidadMaximadeMediciones;
let altura;

let infoAntes;
let infoEnfriamiento;
let infoDespues;

//fechaInput.value = '2014-02-24'
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


$("#NoSerie").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $("#btnRequest").click();
    }
});

$("#Norma").change(function () {
    changeTipoCarga()
})

$("#CheckBox").change(function () {
    if (!$(this).prop("checked")) {

        alturasInput.disabled = false
        selectCargaInput.disabled = false
        fechaInput.disabled = true
    } else {
        selectCargaInput.disabled = true
        fechaInput.disabled = false
        alturasInput.disabled = true


    }

    changeTipoCarga()

})

$("#NoSerie").change(async function () {
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

    var split = noSerieInput.value.split('-')
    if (split.length === 1) {
        $(`#NoSerieSpand`).text("Formato invalido");
        return
    }
    else {
        if (split[1] === '') {
            $(`#NoSerieSpand`).text("Formato invalido");
            return
        }
    }


    $("#loader").css("display", "block");
    if (noSerieInput.value) {
        GetInfo(noSerieInput.value).then(
            data => {
                if (data.response.Code !== -1) {
                    noSerieInput.disabled = true;
                    viewModel = data.response.Structure;
                    enfriamiento.disabled = false
                    btnRequest.disabled = false
                    LoadTreeView(viewModel.TreeViewItem);


                    $("#Enfriamiento option[value!='']").each(function () {
                        $(this).remove();
                    });

                    $("#Norma option[value!='']").each(function () {
                        $(this).remove();
                    });

                    $.each(viewModel.Enfriamientos, function (i, val) {
                        $("#Enfriamiento").append("<option value='" + val.Value + "'>" + val.Text + "</option>");
                    });

                    $.each(viewModel.Normas, function (i, val) {
                        $("#Norma").append("<option value='" + val.Value + "'>" + val.Text + "</option>");
                    });

                    if (data.response.Structure.Norma !== null && data.response.Structure.Norma !== "") {

                        normaInput.value = data.response.Structure.Norma

                      
                    }
                    claveIdiomaInput.value = data.response.Structure.ClaveIdioma


                    if (normaInput.value === '') {
                        normaInput.value = ''
                    }
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
    }
})

$("#Pruebas").change(function () {
    if ($(this).val() === "RUI") {
        $("#Alturas").prop("disabled", true)
        $("#div-altura").css("display", "initial")
        $("#CheckBox").css("opacity", "initial")
        $("#CheckBox").prop("disabled", false)
        $("#Altura").prop("selectedIndex", 1)

 
    } else {
        $("#Alturas").prop("disabled", true)
        $("#div-altura").css("display", "none")
        $("#CheckBox").css("opacity", 0.5)
        $("#CheckBox").prop("disabled", true)

        $("#CheckBox").prop("checked", true)
        $("#SelectCargaInformacion").prop("disabled", true)

        fechaInput.disabled = false
        fechaInput.value = ""

    }

    changeTipoCarga();
})

function changeTipoCarga() {
    if (!$(this).prop('checked') && pruebaInput.value === 'RUI') {
        if (normaInput.value === '001' || normaInput.value === '003') {
            selectCargaInput.value = '10 MEDICIONES ANTES Y DESPUÉS DE AMBIENTE'
        }

        if (normaInput.value === '002' || normaInput.value === '005') {
            selectCargaInput.value = '4 MEDICIONES ANTES Y DESPUÉS DE AMBIENTE'
        }
    }
}

//Evento


//Requests
btnClear.addEventListener("click", function () {

    noSerieInput.disabled = false
    noSerieInput.value = '';
    CommentsInput.value = '';
    claveIdiomaInput.value = '';
    btnRequest.disabled = true;
    btnLoadTemplate.disabled = true;
    pruebaInput.value = ''
    normaInput.value = ''
    normaInput.laboratorio = ''
    alimentacionInput.value = 'Tensión'
    alimentacionValueInput.value = ''
    cantMedicionesInput.value = ''
    enfriamiento.value =''
    laboratorioInput.value = 'Potencia'
    fechaInput.value = ''
    $("#CheckBox").prop("checked", true);
    selectCargaInput.value ='4 MEDICIONES ANTES Y DESPUÉS DE AMBIENTE'
    fechaInput.disabled = false

    btnRequest.disabled = true;
    btnLoadTemplate.disabled = true;

    bloquearCampos(false)
    selectCargaInput.disabled = true
    alturasInput.disabled = true
    $("#div-altura").css("display","none")
    $("#Norma option[value!='']").each(function () {
        $(this).remove();
    });
    $("#Enfriamiento option[value!='']").each(function () {
        $(this).remove();
    });


    if (treeViewKendoElement !== undefined) {
        kendo.destroy(treeViewKendoElement);
        $("#treeview-kendo").empty();
    }
    $(".k-upload-files").remove()
    $("#Altura").prop("selectedIndex", 1)

});

btnLoadTemplate.addEventListener("click", function () {

    if (ValidateForm()) {

        MapToViewModel();

        btnRequest.disabled = true;
        btnLoadTemplate.disabled = true;
        btnClear.disabled = false;
        $("#loader").css("display", "block");
        GetTemplateJSON().then(data => {
        })
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

    if ($("#CheckBox").is(":checked") && fechaInput.value === '') {
        ShowFailedMessage("Debe seleccionar la fecha para realizar la busqueda de información")
        return
    }

   /* if (enfriamiento.value === '') {
        ShowFailedMessage("Debe seleccionar un tipo de enfriamiento para realizar la busqueda de información")
        return
    }*/

    $("#loader").css("display", "block");
    if (ValidateForm()) {

        if (noSerieInput.value) {
            CheckInfo().then(
                data => {
                    if (data.response.Code !== -1) {
                        noSerieInput.disabled = true;
                        viewModel = data.response.Structure;
                        //LoadTreeView(viewModel.TreeViewItem);
                        //LoadForm(viewModel);
                        cantidadColumnas = data.response.Structure.CantidadColumnas
                        cantidadMaximadeMediciones = data.response.Structure.CantidadMaximadeMediciones
                        altura = data.response.Structure.Altura
                        btnRequest.disabled = true;
                        btnClear.disabled = false;
                        btnLoadTemplate.disabled = false;

                        bloquearCampos(true)
                    }
                    else {
                        ShowFailedMessage(data.response.Description);
                        bloquearCampos(false)
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
    } else {
        $("#loader").css("display", "none");

        ShowFailedMessage('Debe llenar todos los campos');
    }
});

//Requests

function bloquearCampos(valor) {
    normaInput.disabled = valor;
    pruebaInput.disabled = valor;
    laboratorioInput.disabled = valor;
    alimentacionInput.disabled = valor;
    enfriamiento.disabled = valor;
    cantMedicionesInput.disabled = valor;
    fechaInput.disabled = valor;
    alimentacionValueInput.disabled = valor;
    selectCargaInput.disabled = valor;
    alturasInput.disabled = valor;
    $("#CheckBox").prop("disabled", valor)

}

//Functions
async function GetFilterJSON() {
    var path = path = "/Nra/GetFilter/";

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

async function CheckInfo() {
    var path = path = "/Nra/CheckInfo/";

    var url = new URL(domain + path),
        params =
        {
            noSerie: noSerieInput.value,
            tipoInformacion: enfriamiento.value,
            fecha: fechaInput.value,
            keyTest: pruebaInput.value,
            language: claveIdiomaInput.value,
            esCargaData: $("#CheckBox").is(":checked"),
            valorSelectCargaData: selectCargaInput.value,
            enfriamiento: enfriamiento.value,
            dataDate: fechaInput.value,
            cantidadMediciones: cantMedicionesInput.value,
            alturaSelected : alturasInput.value
        }
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
    var path = path = "/Nra/GetTemplate/";
    console.log(viewModel)
    var url = new URL(domain + path),
        params = {
            nroSerie: noSerieInput.value,
            keyTest: pruebaInput.value,
            language: claveIdiomaInput.value,
            esCargaData: $("#CheckBox").is(":checked"),
            valorSelectCargaData: selectCargaInput.value,
            laboratorio: laboratorioInput.value,
            norma: normaInput.value,
            alimentacion: alimentacionInput.value,
            valorAlimentacion: alimentacionValueInput.value,
            alimentacionRespaldo: alimentacionInput.value,
            enfriamiento: enfriamiento.value,
            dataDate: fechaInput.value,
            cantidadMaximadeMediciones:cantidadMaximadeMediciones,
            cantidadColumnas: cantidadColumnas,
            alutraDefault: altura,
            comentarios : CommentsInput.value


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
        select: onSelect,
        checkboxes: false,
        loadOnDemand: true
    });

}

function LoadForm(response) {
    console.log(response)
    claveIdiomaInput.value = response.ClaveIdioma;
    //noPruebaInput.value = response.NoPrueba;
    pruebas = response.CharacteristicsArtifact;

}
var ResultValidations;

function ValidateForm() {

    ResultValidations = $("#form_menu_sup").data("kendoValidator").validate();
    if (ResultValidations) {
        return true;
    } else {
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

        GetPDFJSON(id, "NRA", text.split('.')).then(
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
    var path = path = "/Nra/GetPDFReport/";

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



async function GetInfo(value) {
    var path = path = "/Nra/GetInfo/";

    var url = new URL(domain + path),
        params = { noSerie: value }
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
function onError(e) {
    console.log(e)
    console.log(e.sender)
}


//Functions This is likely because the managed PInvoke signature does not match the unmanaged target signature. Check that the calling convention and parameters of the PInvoke signature match the target unmanaged signature.
