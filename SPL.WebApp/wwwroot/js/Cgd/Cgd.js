
//Variables
let viewModel;
let settingsToDisplayCGDReportsDTO;
let btnRequest = document.getElementById("btnRequest");
let btnClear = document.getElementById("btnClear");
let btnLoadTemplate = document.getElementById("btnLoadTemplate");
let noSerieInput = document.getElementById("NoSerie");
let CommentsInput = document.getElementById("Comments");
let noPruebaInput = document.getElementById("NoPrueba");
let claveIdiomaInput = document.getElementById("ClaveIdioma");
let clavePruebaInput = document.getElementById("ClavePrueba");
let temperatureHour1 = document.getElementById("TemperatureHour1");
let temperatureHour2 = document.getElementById("TemperatureHour2");
let temperatureHour3 = document.getElementById("TemperatureHour3");
let oilType = document.getElementById("OilType");
let spreadsheetElement;
let treeViewKendoElement;

let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");

let DDT = false;

//Evento
$("#NoSerie").focus();
$("#NoSerie").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $("#btnRequest").click();
    }
});

//Mostrar info necesaria para consultar el reporte
document.querySelector('#btnInfo').addEventListener('click', () => {
    var text =
        'Considerar que para este reporte:<br><br>'
        + 'El “No. De Serie” del Aparato se debe de encontrar registrado en SPL.<br>'
        + 'Se debe contar con Información en la pantalla: “Diseño del Aparato”.<br>'
        + 'Se deben de contar con las plantillas cargadas para las pruebas correspondientes del reporte:<br>'
        + '<ul>'
        + '<li>Antes de Pruebas (Ambos Idiomas)</li>'
        + '<li>Después de Pruebas Dieléctricas (Ambos Idiomas)</li>'
        + '<li>Después de Excitación (Ambos Idiomas)</li>'
        + '<li>Después de Temperatura (Ambos Idiomas)</li>'
        + '<li>Después de Sobrecarga (Ambos Idiomas)</li>'
        + '</ul>'
        + '<br>'
        + 'En caso de que la prueba sea: “Después de Temperatura” el sistema habilitará los 3 campos del apartado: “Horas de Temperatura” el cual incluye una validación que consta que el 1er campo debe de ser menor al segundo y el segundo debe de ser menor al tercero, con el fin de que los datos vayan en aumento, de tipo numérico.';
    ShowWarningMessageHtmlContent(text);
});

$("#ClavePrueba").on("change", function () {
    if (clavePruebaInput.value == 'DDT') {
        temperatureHour1.disabled = false;
        temperatureHour2.disabled = false;
        temperatureHour3.disabled = false;
        DDT = true;
    } else {
        temperatureHour1.disabled = true;
        temperatureHour2.disabled = true;
        temperatureHour3.disabled = true;
        temperatureHour1.value = 0;
        temperatureHour2.value = 0;
        temperatureHour3.value = 0;
        DDT = false;
    }

    //if (clavePruebaInput.value != '') {
    //    $("#OilType option[value!='']").each(function () {
    //        $(this).remove();
    //    });

    //    for (var i = 0; i < viewModel.OilTypes.length; i++) {
    //        if (viewModel.OilTypes[i].ClavePrueba == clavePruebaInput.value) {
    //            $("#OilType").append("<option value='" + viewModel.OilTypes[i].TipoAceite + "'>" + viewModel.OilTypes[i].TipoAceite + "</option>");
    //        }
    //    }
    //} else {
    //    $("#OilType option[value!='']").each(function () {
    //        $(this).remove();
    //    });
    //}
});

btnClear.addEventListener("click", function () {
    noSerieInput.value = '';
    //noPruebaInput.value = '';
    CommentsInput.value = '';
    claveIdiomaInput.value = '';
    clavePruebaInput.value = '';
    oilType.value = '';
    temperatureHour1.value = 0;
    temperatureHour2.value = 0;
    temperatureHour3.value = 0;
    temperatureHour1.disabled = true;
    temperatureHour2.disabled = true;
    temperatureHour3.disabled = true;
    clavePruebaInput.disabled = true;
    btnRequest.disabled = false;
    btnClear.disabled = false;
    btnLoadTemplate.disabled = true;
    noSerieInput.disabled = false;

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
        btnLoadTemplate.disabled = false;
        GetTemplateJSON()
    }
    else {
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
        $(`#NoSerieSpand`).text("Caracter(es) no permitido.");
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
                    noSerieInput.disabled = true;

                    viewModel = data.response.Structure;
                    LoadTreeView(viewModel.TreeViewItem);
                    LoadForm(viewModel);

                    btnRequest.disabled = true;
                    btnClear.disabled = false;
                    btnLoadTemplate.disabled = false;
                    clavePruebaInput.disabled = false;
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
async function GetFilterJSON() {
    var path = path = "/Cgd/GetFilter/";

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

function GetTemplateJSON() {
    var path = path = "/Cgd/GetTemplate/";

    var url = new URL(domain + path),
        params = {
            noSerie: noSerieInput.value,
            clavePrueba: clavePruebaInput.value,
            claveIdioma: claveIdiomaInput.value,
            temperatureHour1: temperatureHour1.value,
            temperatureHour2: temperatureHour2.value,
            temperatureHour3: temperatureHour3.value,
            oilType: oilType.value,
            comment: CommentsInput.value
        }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))
    $("#loader").css("display", "none");
    var win = window.open(url.toString(), '_blank');
    win.focus();
}

//Functions

function LoadTreeView(treeViewModel) {
    treeViewKendoElement = $("#treeview-kendo").kendoTreeView({
        template: kendo.template($("#treeview").html()),
        dataSource: treeViewModel,
        dragAndDrop: false,
        checkboxes: false,
        select: onSelect,
        loadOnDemand: true
    });
}
function onSelect(e) {
    var text = this.text(e.node);
    if (text.split('.').length > 1) {
        var id = text.split('.')[0].split('-')[2].split('_')[1];
        console.log("Selecting: " + this.text(e.node));
        console.log(id);

        GetPDFJSON(id, "CGD").then(
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
async function GetPDFJSON(code, tyoeReport) {
    var path = path = "/Cgd/GetPDFReport/";
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
function LoadForm(response) {
    claveIdiomaInput.value = response.ClaveIdioma;
    //noPruebaInput.value = response.NoPrueba;
    oilType.value = response.OilType;
}
var ResultValidations;
function ValidateForm() {

    ResultValidations = $("#form_menu_sup").data("kendoValidator").validate();
    if (ResultValidations) {
        if (clavePruebaInput.value == 'DDT') {
            if ((parseInt(temperatureHour1.value) == 0 || parseInt(temperatureHour1.value) == undefined) &&
                (parseInt(temperatureHour2.value) == 0 || parseInt(temperatureHour2.value) == undefined) &&
                (parseInt(temperatureHour3.value) == 0 || parseInt(temperatureHour3.value) == undefined)) {
                ShowFailedMessage("La Hora de Temperatura debe ser ingresada en alguno de los tres campos para este tipo de prueba");
                return false;
            }

            if (parseInt(temperatureHour2.value) != 0) {
                if (parseInt(temperatureHour1.value) > parseInt(temperatureHour2.value)) {
                    ShowFailedMessage("La Hora de Temperatura 2 no puede ser menor a la 1");
                    return false;
                }
            }

            if (parseInt(temperatureHour3.value) != 0) {
                if (parseInt(temperatureHour2.value) > parseInt(temperatureHour3.value)) {
                    ShowFailedMessage("La Hora de Temperatura 3 no puede ser menor a la 2");
                    return false;
                }

                if (parseInt(temperatureHour1.value) > parseInt(temperatureHour3.value)) {
                    ShowFailedMessage("La Hora de Temperatura 3 no puede ser menor a la 1");
                    return false;
                }
            }
        } else {
            temperatureHour1.disabled = true;
            temperatureHour2.disabled = true;
            temperatureHour3.disabled = true;
            temperatureHour1.value = 0;
            temperatureHour2.value = 0;
            temperatureHour3.value = 0;
            DDT = false;
        }
        return true;
    }else {
        return false;
    }
}

function MapToViewModel() {
    viewModel.ClaveIdioma = claveIdiomaInput.value;
    viewModel.ClavePrueba = clavePruebaInput.value;
    //viewModel.NoPrueba = noPruebaInput.value;
    viewModel.Comments = CommentsInput.value;
    viewModel.NoSerie = noSerieInput.value;
    viewModel.TemperatureHour1 = temperatureHour1.value;
    viewModel.TemperatureHour2 = temperatureHour2.value;
    viewModel.TemperatureHour3 = temperatureHour3.value;
    viewModel.OilType = oilType.value;
}