
//Variables
let viewModel;
let settingsToDisplayETDReportsDTO;
let btnRequest = document.getElementById("btnRequest");
let btnInfo = document.querySelector('#btnInfo');
let btnClear = document.getElementById("btnClear");
let btnLoadTemplate = document.getElementById("btnLoadTemplate");
let noSerieInput = document.getElementById("NoSerie");
let CommentsInput = document.getElementById("Comments");
let noPruebaInput = document.getElementById("NoPrueba");
let claveIdiomaInput = document.getElementById("ClaveIdioma");
let clavePruebaInput = document.getElementById("ClavePrueba");
let regressionType = document.getElementById("RegressionType");
let overload = document.getElementById("Overload");
let lVDifferentCapacity = document.getElementById("LVDifferentCapacity");
let terReducedCapacity = document.getElementById("TerReducedCapacity");
let capacity1 = document.getElementById("Capacity1");
let capacity2 = document.getElementById("Capacity2");
let terB2 = document.getElementById("TerB2");
let splitWinding = document.getElementById("SplitWinding");
let connection = document.getElementById("Connection");
let spreadsheetElement;
let treeViewKendoElement;

let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");

function PassVM(viewMode) {
    viewModel = viewMode;
};

//Mostrar info necesaria para consultar el reporte
btnInfo.addEventListener('click', () => {
    console.log('Llegue');
    var text = 'Considerar que para este reporte debe:<br><br>'
        + 'Existir información de características.<br>'
        + 'Existir información de garantías.<br>'
        + 'Existir información de capacidades';
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
    noSerieInput.value = '';
    CommentsInput.value = '';
    claveIdiomaInput.value = '';
    clavePruebaInput.value = '';

    regressionType.value = '';
    overload.value = '';
    lVDifferentCapacity.value = '';
    terReducedCapacity.value = '';
    capacity1.value = 0;
    capacity2.value = 0;
    terB2.value = '';
    splitWinding.value = '';
    connection.value = 0;
 
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

        GetFilterJSON().then(
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
    var path = "/Etd/GetFilter/";

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
    var path = "/Etd/GetTemplate/";

    var url = new URL(domain + path),
        params = {
            noSerie: noSerieInput.value,
            clavePrueba: clavePruebaInput.value,
            claveIdioma: claveIdiomaInput.value,
            date: date.value,
            regressionType: regressionType.value,
            overload : overload.value,
            lVDifferentCapacity: lVDifferentCapacity.value,
            terCapRed: terReducedCapacity.value,
            capacityBt: capacity1.value,
            capacityTer: capacity2.value,
            tertiary2B: terB2.value,
            windingSplit: splitWinding.value,
            connection: connection.value,
            idCuttingData: viewModel.IdCuttingData,
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


        GetPDFJSON(id, "ETD").then(
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
    var path = "/Etd/GetPDFReport/";

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
}
var ResultValidations;
function ValidateForm() {

    ResultValidations = $("#form_menu_sup").data("kendoValidator").validate();
    if (ResultValidations) {
        if (lVDifferentCapacity.value == 1 && capacity1.value <= 0) {
            ShowFailedMessage('Error, capacidad de Baja Tensión Diferente Capacidad es requerido.');
            return false;
        }

        if (terReducedCapacity.value == 1 && capacity2.value <= 0) {
            ShowFailedMessage('Error, capacidad de Terciaria Reducida es requerido.');
            return false;
        }
        return true;
    }else {
        return false;
    }
}
