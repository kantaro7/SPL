
//Variables
let viewModel;
let settingsToDisplayPCIReportsDTO;
let btnRequest = document.getElementById("btnRequest");
let btnClear = document.getElementById("btnClear");
let btnLoadTemplate = document.getElementById("btnLoadTemplate");
let noSerieInput = document.getElementById("NoSerie");
let CommentsInput = document.getElementById("Comments");
let noPruebaInput = document.getElementById("NoPrueba");
//noPruebaInput.value = '';
let claveIdiomaInput = document.getElementById("ClaveIdioma");
let clavePruebaInput = document.getElementById("ClavePrueba");
let materialDevanado = document.getElementById("MaterialDevanado");
let capReducida = document.getElementById("CapacidadReducidaBaja");
let autoTransformador = document.getElementById("AutoTransformador");
let monofasico = document.getElementById("Monofasico");
let capacidadPrueba = document.getElementById("CapacidadPrueba");
let spreadsheetElement;
let treeViewKendoElement;

let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");
let panelExcel = document.getElementById("spreadsheet");
let pasar = false
let pasar2 = false
let pasarSelect = false
let letraSec = ''
let letraPri = ''
let pruebas = []
let arrPri = []
let arrSec = []
let lastTestConValidSelection = null;

//Mostrar info necesaria para consultar el reporte
document.querySelector('#btnInfo').addEventListener('click', () => {
    var text =
        'Considerar que para este reporte:<br><br>'
        + 'El “No. De Serie” del Aparato se debe de encontrar registrado en SPL.<br>'
        + 'Se debe contar con Información en la pantalla: “Diseño del Aparato”.<br>'
        + 'Se deben de contar con las plantillas cargadas para las pruebas correspondientes del reporte:<br>'
        + '<ul>'
        + '<li>Alta Tensión / Baja Tensión (Ambos Idiomas)</li>'
        + '<li>Alta Tensión / Terciario (Ambos Idiomas)</li>'
        + '<li>Baja Tensión / Terciario (Ambos Idiomas)</li>'
        + '</ul>'
        + '<br>'
        + 'Debe de existir información en la pantalla: “Tensión de la Placa”.<br>'
        + 'Deberá de existir información del aparato a cargar, en el reporte: “Resistencias Óhmica de los Devanados” (ROD), con conexión de prueba con un valor de : “L-L”.<br>'
        + 'Deberá de existir información del aparato a cargar, en el reporte: “Pérdidas en Vacío y Corriente de Excitación” (PCE) en la prueba: “Antes de Pruebas Dieléctricas” (SAN).<br>'
        + 'Se deberá de seleccionar solo una opción como: “Primaria” y solo una opción para: “Secundaria”, no pueden existir 2 del mismo tipo para las tensiones a elegir.';
    ShowWarningMessageHtmlContent(text);
});

//Evento

function clearPosiciones() {
    $("#selectAT").prop("disabled", false)
    $("#ATPrimaria").prop("disabled", false)
    $("#ATSecundaria").prop("disabled", false)

    $("#selectBT").prop("disabled", false)
    $("#BTPrimaria").prop("disabled", false)
    $("#BTSecundaria").prop("disabled", false)

    $("#selectTer").prop("disabled", false)
    $("#TerPrimaria").prop("disabled", false)
    $("#TerSecundaria").prop("disabled", false)

    pasarSelect = false
    pasar = false
}

$("#NoSerie").focus();

//$("#ClavePrueba").change(function () {

//    clearPosiciones()

//    if (this.value == "AYB") {
//        $("#selectTer").prop("disabled", true)
//        $("#TerPrimaria").prop("disabled", true)
//        $("#TerSecundaria").prop("disabled", true)
//    } else if (this.value == "AYT") {
//        $("#selectBT").prop("disabled", true)
//        $("#BTPrimaria").prop("disabled", true)
//        $("#BTSecundaria").prop("disabled", true)
//    } else if (this.value == "BYT") {
//        $("#selectAT").prop("disabled", true)
//        $("#ATPrimaria").prop("disabled", true)
//        $("#ATSecundaria").prop("disabled", true)
//    } else {
//        $("#selectTer").prop("disabled", true)
//        $("#selectAT").prop("disabled", true)
//        $("#selectBT").prop("disabled", true)
//        $("#TerPrimaria").prop("disabled", true)
//        $("#TerSecundaria").prop("disabled", true)
//        $("#ATPrimaria").prop("disabled", true)
//        $("#ATSecundaria").prop("disabled", true)
//        $("#BTPrimaria").prop("disabled", true)
//        $("#BTSecundaria").prop("disabled", true)

//        $("#selectBT option[value!='']").each(function () {
//            $(this).remove();
//        });

//        $("#selectAT option[value!='']").each(function () {
//            $(this).remove();
//        });

//        $("#selectTer option[value!='']").each(function () {
//            $(this).remove();
//        });
//    }
//})

$(".radio").click(function () {

    if (this.id == "ATPrimaria" || this.id == "ATSecundaria") {
        if ($("#ATPrimaria").is(":checked") && $("#ATSecundaria").is(":checked")) {
            ShowFailedMessage("No se puede establecer una posicion primaria y secundaria para AT");
            pasar = false;
            //$("#"+this.id).prop("checked" , false)

        } else {
            pasar = true;
        }
    }
    if (this.id == "BTPrimaria" || this.id == "BTSecundaria") {
        if ($("#BTPrimaria").is(":checked") && $("#BTSecundaria").is(":checked")) {
            ShowFailedMessage("No se puede establecer una posicion primaria y secundaria para BT");
            pasar = false;
            //$("#"+this.id).prop("checked" , false)

        } else {
            pasar = true;
        }
    }

    if (this.id == "TerPrimaria" || this.id == "TerSecundaria") {
        if ($("#TerPrimaria").is(":checked") && $("#TerSecundaria").is(":checked")) {
            ShowFailedMessage("No se puede establecer una posicion primaria y secundaria para Ter");
            pasar = false;
            //$("#"+this.id).prop("checked" , false)

        } else {
            pasar = true;
        }
    }

});


$("#CapacidadPrueba").change(function () {
    if ($("#CapacidadPrueba").val().length > 3) {
        $("#CapacidadPrueba").val(lastTestConValidSelection);
    } else {
        lastTestConValidSelection = $(this).val();
    }
});


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
    //clavePruebaInput.value = ''
    capacidadPrueba.value = ''
    pasar2 = false
    btnRequest.disabled = false;
    btnClear.disabled = false;
    btnLoadTemplate.disabled = true;
    noSerieInput.disabled = false;

    $("#selectTer").prop("disabled", true)
    $("#selectAT").prop("disabled", true)
    $("#selectBT").prop("disabled", true)
    $("#TerPrimaria").prop("disabled", true)
    $("#TerSecundaria").prop("disabled", true)
    $("#ATPrimaria").prop("disabled", true)
    $("#ATSecundaria").prop("disabled", true)
    $("#BTPrimaria").prop("disabled", true)
    $("#BTSecundaria").prop("disabled", true)

    $("#ATSecundaria").prop("checked", false)
    $("#ATPrimaria").prop("checked", false)
    $("#BTSecundaria").prop("checked", false)
    $("#BTPrimaria").prop("checked", false)
    $("#TerSecundaria").prop("checked", false)
    $("#TerPrimaria").prop("checked", false)

    $("#selectBT option[value!='']").each(function () {
        $(this).remove();
    });

    $("#selectAT option[value!='']").each(function () {
        $(this).remove();
    });

    $("#selectTer option[value!='']").each(function () {
        $(this).remove();
    });

    $("#CapacidadPrueba option[value!='']").each(function () {
        $(this).remove();
    });

    if (treeViewKendoElement !== undefined) {
        kendo.destroy(treeViewKendoElement);
        $("#treeview-kendo").empty();
    }


});

btnLoadTemplate.addEventListener("click", async function () {

    if (ValidateForm()) {

        MapToViewModel();
        let validacion = validarPosicionesYselect()
        if (!validacion) {
            return
        }

        if (!pasar || !pasarSelect) {
            return
        }
        btnRequest.disabled = true;
        btnClear.disabled = false;
        $("#loader").css("display", "block");

        let conexPruebaSP = ""
        let clavePruebaSP = ""

        console.log(conexPruebaSP)
        console.log(clavePruebaSP)

        await CheckValidationsRodPci().then(
            data => {
                if (data.response.Code !== -1) {
                    console.log(data.response.Structure)
                    pasar2 = true

                    conexPruebaSP = data.response.Structure.ConexionPruebaFind;
                    clavePruebaSP = data.response.Structure.ClavePruebaFind;
                }
                else {
                    pasar2 = false
                    ShowFailedMessage(data.response.Description);
                    $("#loader").css("display", "none");
                }
            }
        );

        if (pasar2) {
            await ValidateFilterJSON(null).then(
                data => {
                    if (data.response.Code !== -1) {
                        GetTemplateJSON(conexPruebaSP, clavePruebaSP);
                    }
                    else {
                        ShowFailedMessage(data.response.Description);
                        return
                    }
                }
            );
        }
    }
    else {
        ShowFailedMessage("Faltan campos por llenar")

    }

    $("#loader").css("display", "none");
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

function validarPosicionesYselect() {
    if ($("*[id*='Primaria']:checked").length == 0) {
        arrPri = []
        ShowFailedMessage('Debe seleccionar una posicion Primaria');
        return false
    }
    if ($("*[id*='Secundaria']:checked").length == 0) {
        arrSec = []
        ShowFailedMessage('Debe seleccionar una posicion Secundaria');

        return false
    }


    letraPri = $("*[id*='Primaria']:checked")[0].id.replace("Primaria", '')
    letraSec = $("*[id*='Secundaria']:checked")[0].id.replace("Secundaria", '')


    arrPri = $("#select" + letraPri).val()
    arrSec = $("#select" + letraSec).val()


    if (arrPri == null) {
        ShowFailedMessage("Debe seleccionar por lo menos una posicion primaria")
        arrPri = []
        return false
    }

    if (arrPri.length === 1) {
        if (arrPri[0] === '') {
            $('#select' + letraPri + ' option').prop('selected', true);
            arrPri = $("#select" + letraPri).val().filter(x => x != '')
            if (arrPri.length !== 1) {
                ShowFailedMessage("Solo se permite seleccionar 1 posición como primaria")
                return false
            }
        }
    } else if (arrPri.length > 1) {
        ShowFailedMessage("Solo se permite seleccionar 1 posición como primaria")
        return false
    }

    if (arrPri.length === 1) {
        if (arrSec[0] === '') {
            $('#select' + letraSec + ' option').prop('selected', true);
            arrSec = $("#select" + letraSec).val().filter(x => x != '')
            if (arrSec.length > 33 || arrSec.length == 0) {
                ShowFailedMessage("Cantidad de posiciones en secundaria no permitidas, solo se permiten como máximo 33 y mínimo 1")
                return false
            }
        } else {
            arrSec = $("#select" + letraSec).val().filter(x => x != '')
            if (arrSec.length > 33 || arrSec.length === 0) {
                ShowFailedMessage("Cantidad de posiciones en secundaria no permitidas, solo se permiten como máximo 33 y mínimo 1")
                return false
            }
        }
    }

    pasarSelect = true
    return pasarSelect
}

//Requests


async function CheckValidationsRodPci() {

    letraPri = $("*[id*='Primaria']:checked")[0].id.replace("Primaria", '')
    letraSec = $("*[id*='Secundaria']:checked")[0].id.replace("Secundaria", '')

    var arrAT = []
    var arrBT = []
    var arrTer = []

    var isAt = false
    var isBt = false
    var isTer = false

    if ((letraPri === 'AT' && letraSec === 'BT') || (letraSec === 'AT' && letraPri === 'BT')) {

        isAt = true
        isBt = true
        arrAT = $("#selectAT").val()
        arrBT = $("#selectBT").val()
    }


    if ((letraPri === 'AT' && letraSec === 'Ter') || (letraSec === 'AT' && letraPri === 'Ter')) {
        isAt = true
        isTer = true
        arrAT = $("#selectAT").val()
        arrTer = $("#selectTer").val()
    }

    if ((letraPri === 'BT' && letraSec === 'Ter') || (letraSec === 'BT' && letraPri === 'Ter')) {

        isBt = true
        isTer = true
        arrBT = $("#selectBT").val()
        arrTer = $("#selectTer").val()
    }




    var primaria = $("*[id*='Primaria']:checked")[0].id
    var secundaria = $("*[id*='Secundaria']:checked")[0].id

    if (primaria === "ATPrimaria") {
        isAt = true
    } else if (primaria === "BTPrimaria") {
        isBt = true
    } else if (primaria === "TerPrimaria") {
        isTer = true
    }

    if (secundaria === "ATSecundaria") {
        isAt = true
    } else if (primaria === "BTSecundaria") {
        isBt = true
    } else if (primaria === "TerSecundaria") {
        isTer = true
    }


    var path = path = "/Pci/CheckValidationsRodPci/";

    var url = new URL(domain + path),
        params = {
            nroSerie: noSerieInput.value,
            capacity: $("#CapacidadPrueba").val(),
            windingMaterial: materialDevanado.value,
            posAt: arrAT,
            posBt: arrBT,
            posTer: arrTer,
            isAT: isAt,
            isBT: isBt,
            isTer: isTer,

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

async function ValidateFilterJSON() {
    var path = path = "/Pci/ValidateFilter/";

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

async function GetFilterJSON() {
    var path = path = "/Pci/GetFilter/";

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


async function GetTemplateJSON(conex, claveP) {
    var path = path = "/Pci/GetTemplate/";

    var url = new URL(domain + path),
        params = {
            nroSerie: noSerieInput.value,
            //keyTest: clavePruebaInput.value,
            lenguage: claveIdiomaInput.value,
            windingMaterial: $("#MaterialDevanado").val(),
            capRedBaja: $("#CapacidadReducidaBaja").val() === "0" ? false : true,
            autotransformer: $("#AutoTransformador").val() === "0" ? false : true,
            monofasico: $("#Monofasico").val() === "0" ? false : true,
            testCapacity: $("#CapacidadPrueba").val(),
            cantPosPri: arrPri.length,
            cantPosSec: arrSec.length,
            posPi: letraPri,
            posSec: letraSec,
            posicionesPrimarias: arrPri,
            posicionesSecundarias: arrSec,
            comment: CommentsInput.value,
            keyTestSp: claveP,
            testConnex: conex,
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
        select: onSelect,
        checkboxes: false,
        loadOnDemand: true
    });

}

function LoadForm(response) {
    claveIdiomaInput.value = response.ClaveIdioma;
    //noPruebaInput.value = response.NoPrueba;

    pruebas = response.CharacteristicsArtifact;

    $.each(response.Positions.AltaTension, function (i, val) {
        $("#selectAT").append("<option value='" + val + "'>" + val + "</option>");
        $("#selectAT").prop("disabled", false);
        $("#ATPrimaria").prop("disabled", false);
        $("#ATSecundaria").prop("disabled", false);
    });

    $.each(response.Positions.BajaTension, function (i, val) {
        $("#selectBT").append("<option value='" + val + "'>" + val + "</option>");
        $("#selectBT").prop("disabled", false);
        $("#BTPrimaria").prop("disabled", false);
        $("#BTSecundaria").prop("disabled", false);
    });

    $.each(response.Positions.Terciario, function (i, val) {
        $("#selectTer").append("<option value='" + val + "'>" + val + "</option>");
        $("#selectTer").prop("disabled", false);
        $("#TerPrimaria").prop("disabled", false);
        $("#TerSecundaria").prop("disabled", false);
    });

    $("#CapacidadPrueba option[value!='']").each(function () {
        $(this).remove();
    });

    // Conjunto vacío para tener solo valores distintos
    const capacidades = new Set();

    for (const obj of response.CharacteristicsArtifact) {
        // Paso 3: obtener los valores de las propiedades y agregarlos al conjunto
        capacidades.add(obj.Mvaf1);
        capacidades.add(obj.Mvaf2);
        capacidades.add(obj.Mvaf3);
        capacidades.add(obj.Mvaf4);
    }

    const arrayCapacidades = Array.from(capacidades); // convertir el conjunto en una matriz

    arrayCapacidades.sort((a, b) => a - b); // ordenar la matriz en orden descendente

    arrayCapacidades.forEach(capacidad => {
        if (capacidad > 0) {
            $("#CapacidadPrueba").append("<option value='" + capacidad + "'>" + (capacidad * 1000) + "</option>");
        }
    });

    if (response.EsAutotransformador) {
        autoTransformador.value = 1
    }

    if (response.EsMonofasico) {
        monofasico.value = 1
    }

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
    //viewModel.ClavePrueba = clavePruebaInput.value;
    //viewModel.NoPrueba = noPruebaInput.value;
    viewModel.Comments = CommentsInput.value;
    viewModel.NoSerie = noSerieInput.value;
    viewModel.MaterialDevanado = materialDevanado.value;
    viewModel.CapacidadReducidaBaja = capReducida.value;
    viewModel.AutoTransformador = autoTransformador.value;
    viewModel.Monofasico = monofasico.value;
    viewModel.CapacidadPrueba = capacidadPrueba.value;
    viewModel.CantidadPosicionPrimaria = arrPri.length;
    viewModel.CantidadPosicionSecundaria = arrSec.length;
    viewModel.PosicionPrimaria = letraPri;
    viewModel.PosicionSecundaria = letraSec;
    viewModel.ReigstrosPosicionesPrimarias = arrPri;
    viewModel.ReigstrosPosicionesSecundarias = arrSec;

    if (loadWorkbook)
        viewModel.Workbook.sheets = $("#spreadsheet").getKendoSpreadsheet().toJSON().sheets;
    if (loadOfficial)
        viewModel.OfficialWorkbook = officialWorkbook;
}

function onSelect(e) {
    var text = this.text(e.node);

    if (text.split('.').length > 1) {
        var id = text.split('.')[0].split('-')[2].split('_')[1];

        GetPDFJSON(id, "PCI").then(
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
    var path = path = "/Pci/GetPDFReport/";

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
