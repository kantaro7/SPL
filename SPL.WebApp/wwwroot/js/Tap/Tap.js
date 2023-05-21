
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
let aTCapacitanceId = document.getElementById("ATCapacitanceId");
let bTCapacitanceId = document.getElementById("BTCapacitanceId");
let terCapacitanceId = document.getElementById("TerCapacitanceId");
let unitType = document.getElementById("UnitType");
let spreadsheetElement;
let treeViewKendoElement;

let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");

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
$("#NoSerie").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $("#btnRequest").click();
    }
});
btnClear.addEventListener("click", function () {
    $(".k-form-error").remove()
    $("#CapacitanciaTer").val("");
    $("#CapacitanciaTer").prop("disabled",true)
    unitType.value =''
    noSerieInput.value = '';
    //noPruebaInput.value = '';
    CommentsInput.value = '';
    claveIdiomaInput.value = '';
    aTConnectionsAmount.value = '';
    bTConnectionsAmount.value = '';
    terConnectionsAmount.value = '';
    aTCapacitanceId.value = '';
    bTCapacitanceId.value = '';
    terCapacitanceId.value = '';
    aTConnectionsAmount.disabled = true;
    bTConnectionsAmount.disabled = true;
    terConnectionsAmount.disabled = true;
    aTCapacitanceId.disabled = true;
    bTCapacitanceId.disabled = true;
    terCapacitanceId.disabled = true;
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
                if (data.response.Code === -2) {
                    ShowFailedMessage(data.response.Description);
                    LoadForm(viewModel);
                }
                else if (data.response.Code !== -1) {
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
                    viewModel = data.response.Structure;
                    LoadForm(viewModel);

                    btnRequest.disabled = false;
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
    var path = path = "/Tap/GetFilter/";

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
    var path = path = "/Tap/GetTemplate/";

    var url = new URL(domain + path),
        params = {
            noSerie: noSerieInput.value,
            clavePrueba: clavePruebaInput.value,
            claveIdioma: claveIdiomaInput.value,
            aTConnectionsAmount: aTConnectionsAmount.value,
            bTConnectionsAmount: bTConnectionsAmount.value,
            terConnectionsAmount: terConnectionsAmount.value,
            aTCapacitanceId: aTCapacitanceId.value,
            bTCapacitanceId: bTCapacitanceId.value,
            terCapacitanceId: terCapacitanceId.value,
            unitType: unitType.value,
            comment: CommentsInput.value,
            capacidadTer: $("#CapacitanciaTer").val(),
            nuevoDevanado: viewModel.NuevoDevanado,
            viejoDevanado: viewModel.ViejoDevanado,
            nuevaFila: viewModel.NuevaFila,
            viejaFila: viewModel.ViejaFila
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


        GetPDFJSON(id, "TAP").then(
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
    var path = path = "/Tap/GetPDFReport/";

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
    if (response.ATConnectionsAmount > 0) {
        aTConnectionsAmount.disabled = false;
        $(aTConnectionsAmount).attr({
            "max": response.ATConnectionsAmount,
            "min": 1,
        });
    }
    else {
        aTConnectionsAmount.disabled = true;
    }
    bTConnectionsAmount.value = response.BTConnectionsAmount
    if (response.BTConnectionsAmount > 0) {
        bTConnectionsAmount.disabled = false;
        $(bTConnectionsAmount).attr({
            "max": response.BTConnectionsAmount,
            "min": 1,
        });
    }
    else {
        bTConnectionsAmount.disabled = true;
    }
    terConnectionsAmount.value = response.TerConnectionsAmount
    if (response.TerConnectionsAmount > 0) {
        terConnectionsAmount.disabled = false;
        $(terConnectionsAmount).attr({
            "max": response.TerConnectionsAmount,
            "min": 1,
        });
    }
    else {
        terConnectionsAmount.disabled = true;
    }
    aTCapacitanceId.value = response.ATCapacitanceId
    bTCapacitanceId.value = response.BTCapacitanceId
    terCapacitanceId.value = response.TerCapacitanceId

    aTCapacitanceId.disabled = false;
    bTCapacitanceId.disabled = false;
    terCapacitanceId.disabled = response.TerConnectionsAmount == 0;
    unitType.value = response.UnitType;

    if (response.TerConnectionsAmount > 0 && (response.TerCapacitanceId === null || response.TerCapacitanceId === "")) {
        $("#TerCapacitanceId").prop("disabled" , true)
    }

    $("#CapacitanciaTer").prop("disabled", !response.EnableCapTer);
    unitType.value = response.TipoDevanadoNuevo;


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

        if (!terCapacitanceId.disabled) {
            if (terCapacitanceId.value == "" || terCapacitanceId.value == undefined) {
                ShowFailedMessage('Id de Capacitancia de Ter es requerida');
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
    viewModel.ATCapacitanceId = aTCapacitanceId.value;
    viewModel.BTCapacitanceId = bTCapacitanceId.value;
    viewModel.TerCapacitanceId = terCapacitanceId.value;
    viewModel.UnitType = unitType.value;
}