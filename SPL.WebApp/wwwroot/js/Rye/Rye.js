
//Variables
let viewModel;
let settingsToDisplayPRDReportsDTO;
let btnRequest = document.getElementById("btnRequest");
let btnClear = document.getElementById("btnClear");
let btnLoadTemplate = document.getElementById("btnLoadTemplate");
let noSerieInput = document.getElementById("NoSerie");
let CommentsInput = document.getElementById("Comments");
let noPruebaInput = document.getElementById("NoPrueba");
let claveIdiomaInput = document.getElementById("ClaveIdioma");
let clavePruebaInput = document.getElementById("ClavePrueba");
let aTConnectionsAmount = document.getElementById("ATConnectionsAmount");
let bTConnectionsAmount = document.getElementById("BTConnectionsAmount");
let terConnectionsAmount = document.getElementById("TerConnectionsAmount");
let aTTestVoltage = document.getElementById("ATTestVoltage");
let bTTestVoltage = document.getElementById("BTTestVoltage");
let terTestVoltage = document.getElementById("TerTestVoltage");
let coolingType = document.getElementById("CoolingType");
let spreadsheetElement;
let treeViewKendoElement;

let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");

//Mostrar info necesaria para consultar el reporte
document.querySelector('#btnInfo').addEventListener('click', () => {
    var text =
        'Considerar que para este reporte:<br><br>'
        + 'El “No. De Serie” del Aparato se debe de encontrar registrado en SPL.<br>'
        + 'Se debe contar con Información en la pantalla: “Diseño del Aparato”.<br>'
        + 'Se deben de contar con las plantillas cargadas para las pruebas correspondientes del reporte:<br>'
        + '<ul>'
        + '<li>Única (Ambos Idiomas) </li>'
        + '</ul>'
        + '<br>'
        + 'Se deberá de seleccionar un “Tipo de Enfriamiento”';
    ShowWarningMessageHtmlContent(text);
});

//Evento
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
    aTConnectionsAmount.value = '';
    bTConnectionsAmount.value = '';
    terConnectionsAmount.value = '';
    aTTestVoltage.value = '';
    bTTestVoltage.value = '';
    terTestVoltage.value = '';
    coolingType.value = '';
    coolingType.options.length = 0;
    aTConnectionsAmount.disabled = true;
    bTConnectionsAmount.disabled = true;
    terConnectionsAmount.disabled = true;
    aTTestVoltage.disabled = true;
    bTTestVoltage.disabled = true;
    terTestVoltage.disabled = true;
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
    var path = path = "/Rye/GetFilter/";

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
    var path = path = "/Rye/GetTemplate/";

    var url = new URL(domain + path),
        params = {
            noSerie: noSerieInput.value,
            clavePrueba: clavePruebaInput.value,
            claveIdioma: claveIdiomaInput.value,
            aTConnectionsAmount: aTConnectionsAmount.value,
            bTConnectionsAmount: bTConnectionsAmount.value,
            terConnectionsAmount: terConnectionsAmount.value,
            aTTestVoltage: aTTestVoltage.value,
            bTTestVoltage: bTTestVoltage.value,
            terTestVoltage: terTestVoltage.value,
            coolingType: coolingType.value,
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


        GetPDFJSON(id, "RYE").then(
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
    var path = path = "/Rye/GetPDFReport/";

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
    aTConnectionsAmount.value = response.ATConnectionsAmount
    bTConnectionsAmount.value = response.BTConnectionsAmount
    terConnectionsAmount.value = response.TerConnectionsAmount
    aTTestVoltage.value = response.ATTestVoltage
    bTTestVoltage.value = response.BTTestVoltage
    terTestVoltage.value = response.TerTestVoltage

    aTConnectionsAmount.disabled = false;
    bTConnectionsAmount.disabled = false;
    terConnectionsAmount.disabled = response.TerConnectionsAmount == 0;
    aTTestVoltage.disabled = false;
    bTTestVoltage.disabled = false;
    terTestVoltage.disabled = response.TerConnectionsAmount == 0;

    var length = coolingType.options.length;
    for (i = length - 1; i > 0; i--) {
        coolingType.options[i] = null;
    }

    response.CoolingTypes.forEach(function (data, key) {
        opt = document.createElement("option");
        opt.value = data;
        opt.innerHTML = data;
        coolingType.appendChild(opt);
    });
}
var ResultValidations;
function ValidateForm() {

    ResultValidations = $("#form_menu_sup").data("kendoValidator").validate();
    if (ResultValidations) {
        if (!terConnectionsAmount.disabled) {
            if (terConnectionsAmount.value == 0 || terConnectionsAmount.value == undefined) {
                ShowFailedMessage('No. de Conexiones Ter es requerida y debe ser mayor a 0.');
                return false;
            }
        }

        if (!terTestVoltage.disabled) {
            if (terTestVoltage.value == 0 || terTestVoltage.value == undefined) {
                ShowFailedMessage('Tensión de Prueba Ter es requerida y debe ser mayor a 0.');
                return false;
            }
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
    viewModel.ATConnectionsAmount = aTConnectionsAmount.value;
    viewModel.BTConnectionsAmount = bTConnectionsAmount.value;
    viewModel.TerConnectionsAmount = terConnectionsAmount.value;
    viewModel.ATTestVoltage = aTTestVoltage.value;
    viewModel.BTTestVoltage = bTTestVoltage.value;
    viewModel.TerTestVoltage = terTestVoltage.value;
    viewModel.CoolingType = coolingType.value;
}