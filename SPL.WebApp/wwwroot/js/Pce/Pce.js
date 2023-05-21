
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
let testPositionAT = document.getElementById("TestPositionAT");
let testPositionBT = document.getElementById("TestPositionBT");
let testPositionTer = document.getElementById("TestPositionTer");
let windingEnergized = document.getElementById("WindingEnergized");
let vNStart = document.getElementById("VNStart");
let vNEnd = document.getElementById("VNEnd");
let vNInterval = document.getElementById("VNInterval");
let grafic = document.getElementById("Grafic");
let spreadsheetElement;
let treeViewKendoElement;
let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");
let panelExcel = document.getElementById("spreadsheet");

vNEnd.value = '110';
//Evento

//Mostrar info necesaria para consultar el reporte
document.querySelector('#btnInfo').addEventListener('click', () => {
    var text =
        'Considerar que para este reporte:<br><br>'
        + 'El “No. De Serie” del Aparato se debe de encontrar registrado en SPL.<br>'
        + 'Se debe contar con Información en la pantalla: “Diseño del Aparato”.<br>'
        + 'Se deben de contar con las plantillas cargadas para las pruebas correspondientes del reporte:<br>'
        + '<ul>'
        + '<li>Antes y Después de Pruebas Dieléctricas (Ambos Idiomas)</li>'
        + '<li>Antes de Pruebas Dieléctricas (Ambos Idiomas)</li>'
        + '<li>Después de Pruebas Dieléctricas (Ambos Idiomas)</li>'
        + '</ul>'
        + '<br>'
        + 'Se deberá de seleccionar una de las posiciones para cada prueba de las tensiones, “AT”, “BT” o “Ter”';
    ShowWarningMessageHtmlContent(text);
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

    testPositionAT.value = '';
    testPositionBT.value = '';
    testPositionTer.value = '';
    windingEnergized.value = '';
    vNStart.value = '90';
    vNEnd.value = '110';
    vNInterval.value = '10';
    grafic.value = false;

    testPositionAT.options.length = 0;
    testPositionBT.options.length = 0;
    testPositionTer.options.length = 0;

    testPositionAT.disabled = true;
    testPositionBT.disabled = true;
    testPositionTer.disabled = true;

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
        if (ValidateVn(vNStart.value, vNEnd.value, vNInterval.value)) {
            MapToViewModel();
            btnRequest.disabled = true;
            btnClear.disabled = false;
            btnLoadTemplate.disabled = false;
            $("#loader").css("display", "block");

            ValidateFilterJSON(null).then(
                data => {
                    if (data.response.Code !== -1) {
                        GetTemplateJSON();
                    }
                    else {
                        ShowFailedMessage(data.response.Description);
                    }
                }
            );

        } else {
            $("#loader").css("display", "none");
        }
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
    }
});

//Requests

async function ValidateFilterJSON() {
    var path = path = "/Pce/ValidateFilter/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value, clavePrueba: clavePruebaInput.value, claveIdioma: claveIdiomaInput.value, testPositionAT: testPositionAT.value, testPositionBT: testPositionBT.value, testPositionTer: testPositionTer.value, vNStart: vNStart.value, vNEnd: vNEnd.value, vNInterval: vNInterval.value, windingEnergized: windingEnergized.value, grafic: grafic.checked, comment: CommentsInput.value }
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

async function GetFilterJSON() {
    var path = path = "/Pce/GetFilter/";

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
    var path = path = "/Pce/GetTemplate/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value, clavePrueba: clavePruebaInput.value, claveIdioma: claveIdiomaInput.value, testPositionAT: testPositionAT.value, testPositionBT: testPositionBT.value, testPositionTer: testPositionTer.value, vNStart: vNStart.value, vNEnd: vNEnd.value, vNInterval: vNInterval.value, windingEnergized: windingEnergized.value, grafic: grafic.checked, comment: CommentsInput.value}
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))
    $("#loader").css("display", "none");
    var win = window.open(url.toString(), '_blank');
    win.focus();
}

//Functions

function ValidateVn(VNStart, VNEnd, VNInterval) {
    VNStart = parseInt(VNStart);
    VNEnd = parseInt(VNEnd);
    VNInterval = parseInt(VNInterval);
    if (VNStart > VNEnd) {
        ShowSuccessMessage('Por favor ingrese un Porcentaje inicial de Vn menor al final, favor de corregirlo');
        return false;
    }

    var renglones = ((VNEnd - VNStart) / VNInterval) + 1;

    if ('AYD' == clavePruebaInput.value) {
        if (renglones > 12) {
            ShowFailedMessage("Porcentajes de Vn no permitidos excede de 12 valores que soporta la plantilla, favor de corregirlos");
            return false;
        }
        else {
            if (grafic.checked) {
                ShowFailedMessage("Este tipo de prueba no puede incluir gráfica, por favor deshabilitarla");
                return false;
            }
        }
    }
    else {
        if (renglones > 24) {
            ShowFailedMessage("Porcentajes de Vn no permitidos excede de 24 valores que soporta la plantilla, favor de corregirlos");
            return false;
        }
        else {
            if (renglones < 6 && grafic.checked) {
                ShowFailedMessage("Porcentajes de Vn no suficientes para la gráfica, favor de corregirlos o deshabilitar el gráfico");
                return false;
            }
        }
    }

    return true
}

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


        GetPDFJSON(id, "PCE").then(
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
    var path = path = "/Pce/GetPDFReport/";

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

    var ATs = response.TestPositionsAT.length;
    var BTs = response.TestPositionsBT.length;
    var Ters = response.TestPositionsTer.length;
    var WEs = response.WindingsEnergized.length;
    var opt;
    var length;
    if (ATs > 1) {
        testPositionAT.removeAttribute('disabled');

        length = testPositionAT.options.length;
        for (i = length - 1; i > 0; i--) {
            testPositionAT.options[i] = null;
        }

        response.TestPositionsAT.forEach(function (data, key) {
            opt = document.createElement("option");
            opt.value = data.Value;
            opt.innerHTML = data.Text;
            testPositionAT.appendChild(opt);
        });
    }

    if (BTs > 1) {
        testPositionBT.removeAttribute('disabled');

        length = testPositionBT.options.length;
        for (i = length - 1; i > 0; i--) {
            testPositionBT.options[i] = null;
        }

        response.TestPositionsBT.forEach(function (data, key) {
            opt = document.createElement("option");
            opt.value = data.Value;
            opt.innerHTML = data.Text;
            testPositionBT.appendChild(opt);
        });
    }

    if (Ters > 1) {
        testPositionTer.removeAttribute('disabled');

        length = testPositionTer.options.length;
        for (i = length - 1; i > 0; i--) {
            testPositionTer.options[i] = null;
        }

        response.TestPositionsTer.forEach(function (data, key) {
            opt = document.createElement("option");
            opt.value = data.Value;
            opt.innerHTML = data.Text;
            testPositionTer.appendChild(opt);
        });
    }

    if (WEs > 0) {
        length = windingEnergized.options.length;
        for (i = length - 1; i > 0; i--) {
            windingEnergized.options[i] = null;
        }

        response.WindingsEnergized.forEach(function (data, key) {
            opt = document.createElement("option");
            opt.value = data.Value;
            opt.innerHTML = data.Text;
            windingEnergized.appendChild(opt);
        });
    }
    windingEnergized.value = response.LessWindingEnergized;

    
}
var ResultValidations;

function ValidateForm() {

    ResultValidations = $("#form_menu_sup").data("kendoValidator").validate();
    if (ResultValidations) {
        if (viewModel.AT && testPositionAT.value == "") {
            ShowFailedMessage("Posicion AT es requerida");
            return false;
        }

        if (viewModel.BT && testPositionBT.value == "") {
            ShowFailedMessage("Posicion BT es requerida");
            return false;
        }

        if (viewModel.Ter && testPositionTer.value == "") {
            ShowFailedMessage("Posicion Ter es requerida");
            return false;
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
    viewModel.ClavePrueba = clavePruebaInput.value;
    //viewModel.NoPrueba = noPruebaInput.value;
    viewModel.Comments = CommentsInput.value;
    viewModel.NoSerie = noSerieInput.value;

    viewModel.TestPositionAT = testPositionAT.value;
    viewModel.TestPositionBT = testPositionBT.value;
    viewModel.TestPositionTer = testPositionTer.value;
    viewModel.WindingEnergized = windingEnergized.value;
    viewModel.VNStart = vNStart.value;
    viewModel.VNEnd = vNEnd.value;
    viewModel.VNInterval = vNInterval.value;
    viewModel.Grafic = grafic.value;
}