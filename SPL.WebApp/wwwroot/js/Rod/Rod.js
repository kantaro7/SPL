
//Variables
let viewModel;
let settingsToDisplayRODReportsDTO;
let btnRequest = document.getElementById("btnRequest");
let btnClear = document.getElementById("btnClear");
let btnLoadTemplate = document.getElementById("btnLoadTemplate");
let noSerieInput = document.getElementById("NoSerie");
let CommentsInput = document.getElementById("Comments");
let noPruebaInput = document.getElementById("NoPrueba");
let claveIdiomaInput = document.getElementById("ClaveIdioma");
//let unitType = document.getElementById("UnitType");
//let unitTypeName = document.getElementById("UnitTypeName");
//let clavePruebaInput = document.getElementById("ClavePrueba");
let connection = document.getElementById("Connection");
let material = document.getElementById("Material");
let testVoltage = document.getElementById("TestVoltage");
let unitOfMeasurement = document.getElementById("UnitOfMeasurement");
let spreadsheetElement;
let treeViewKendoElement;
let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");
let panelExcel = document.getElementById("spreadsheet");
let arrAt = [];
let arrBt = [];
let arrTer = [];
let cantPosAt;
let cantPosBt;
let cantPosTer
let numeroColumnas = 0
let clavePrueba;

//Mostrar info necesaria para consultar el reporte
document.querySelector('#btnInfo').addEventListener('click', () => {
    var text =
        'Considerar que para este reporte:<br><br>'
        + 'El “No. De Serie” del Aparato se debe de encontrar registrado en SPL.<br>'
        + 'Se debe contar con Información en la pantalla: “Diseño del Aparato”.<br>'
        + 'Se deben de contar con las plantillas cargadas para las pruebas correspondientes del reporte:<br>'
        + '<ul>'
        + '<li>Alta Tensión y Baja Tensión (Ambos Idiomas)</li>'
        + '<li>Alta Tensión y Terciario (Ambos Idiomas)</li>'
        + '<li>Baja Tensión y Terciario (Ambos Idiomas)</li>'
        + '<li>Alta Tensión (Ambos Idiomas)</li>'
        + '<li>Baja Tensión (Ambos Idiomas)</li>'
        + '<li>Terciario (Ambos Idiomas)</li>'
        + '</ul>';
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
    //clavePruebaInput.value = '';


    //unitType.value = '';
    connection.value = '';
    material.value = 'Cobre';
    testVoltage.value = '';
    unitOfMeasurement.value = 'Ohms';


    connection.options.length = 0;

    testVoltage.disabled = true;
    connection.disabled = true;


    btnRequest.disabled = false;
    btnClear.disabled = false;
    btnLoadTemplate.disabled = true;
    noSerieInput.disabled = false;

    if (treeViewKendoElement !== undefined) {
        kendo.destroy(treeViewKendoElement);
        $("#treeview-kendo").empty();
    }

    $("#selectTer").prop("disabled", true);
    $("#selectAT").prop("disabled", true);
    $("#selectBT").prop("disabled", true);

    $("#selectBT option[value!='']").each(function () {
        $(this).remove();
    });

    $("#selectAT option[value!='']").each(function () {
        $(this).remove();
    });

    $("#selectTer option[value!='']").each(function () {
        $(this).remove();
    });

    $('#Connection').removeAttr('multiple', 'multiple');
});


btnLoadTemplate.addEventListener("click", async function () {
    if (ValidateForm()) {
        if (ValidatePosiciones() && ValidarConexiones() && ValidarCombinaciones()) {
            MapToViewModel();
            btnRequest.disabled = true;
            btnClear.disabled = false;
            $("#loader").css("display", "block");

            if (!viewModel.IsAutrans) {
                await ValidarATBTTer();
            }
            else {
                const conexiones = $("#Connection").val();

                const hasATConnections = conexiones.some((elem) => {
                    return elem.charAt(0) === 'H';
                });

                const hasBTConnections = conexiones.some((elem) => {
                    return elem.charAt(0) === 'X';
                });

                const hasTerConnections = conexiones.some((elem) => {
                    return elem.charAt(0) === 'Y';
                });

                if (cantPosAt > 0) {
                    if (!hasATConnections) {
                        ShowFailedMessage("No puede seleccionar posiciones de alta tensión ya que no selecciono ninguna conexión de prueba correspondiente a alta tensión")
                        $("#loader").css("display", "none");
                        return
                    }
                }
                else {
                    if (hasATConnections) {
                        ShowFailedMessage("No puede seleccionar conexiones de prueba de alta tensión ya que no hay posiciones de alta tensión seleccionadas")
                        $("#loader").css("display", "none");
                        return
                    }
                }

                if (cantPosBt > 0) {
                    if (!hasBTConnections) {
                        ShowFailedMessage("No puede seleccionar posiciones de baja tensión ya que no selecciono ninguna conexión de prueba correspondiente a baja tensión")
                        $("#loader").css("display", "none");
                        return
                    }
                } else {
                    if (hasBTConnections) {
                        ShowFailedMessage("No puede seleccionar conexiones de prueba de baja tensión ya que no hay posiciones de baja tensión seleccionadas")
                        $("#loader").css("display", "none");
                        return
                    }
                }

                if (cantPosTer > 0) {
                    if (!hasTerConnections) {
                        ShowFailedMessage("No puede seleccionar posiciones de terciario ya que no selecciono ninguna conexión de prueba correspondiente a terciario")
                        $("#loader").css("display", "none");
                        return
                    }
                } else {
                    if (hasTerConnections) {
                        ShowFailedMessage("No puede seleccionar conexiones de prueba de terciario ya que no hay posiciones de terciario seleccionadas")
                        $("#loader").css("display", "none");
                        return
                    }
                }
            }

            await ValidateFilterJSON().then(
                data => {

                    $("#loader").css("display", "none");

                    if (data.response.Code !== -1) {
                        clavePrueba = data.response.Structure[0];
                        numeroColumnas = data.response.Structure[1];

                        GetTemplateJSON();
                    }
                    else {
                        ShowFailedMessage(data.response.Description);
                    }
                }
            );
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

        GetFilterJSON().then(
            data => {
                if (data.response.Code !== -1) {
                    btnSave.disabled = false;
                    noSerieInput.disabled = true;

                    viewModel = data.response.Structure;
                    console.log(viewModel);
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

function ValidatePosiciones() {
    var selectedValuesAt = $("#selectAT").val();
    var selectedValuesBt = $("#selectBT").val();
    var selectedValuesTer = $("#selectTer").val();
    let valor = false;

    arrTer = []
    arrAt = []
    arrBt = []
    if ((selectedValuesAt === null || selectedValuesAt.length === 0) && (selectedValuesBt === null || selectedValuesBt.length === 0) && (selectedValuesTer === null || selectedValuesTer.length === 0)) {
        ShowFailedMessage("Debe seleccionar al menos una posición para cualquiera de las tensiones habilitadas")
        return false
    }


    if (selectedValuesAt != null && selectedValuesAt.length > 0) {
        if (selectedValuesAt.length > 36) {
            ShowFailedMessage('Cantidad de posiciones AT no permitidas, solo se permiten como máximo 36 y mínimo 1.');
            return false;
        }
        else {
            console.log(selectedValuesAt[0])
            if (selectedValuesAt[0] == '') {
                arrAt = viewModel.Positions.AltaTension;
                if (arrAt.length > 36) {
                    ShowFailedMessage('Cantidad de posiciones AT no permitidas, solo se permiten como máximo 36 y mínimo 1.');
                    return false;
                }

            } else {
                arrAt = selectedValuesAt;
            }

            valor = true
        }
    }

    if (selectedValuesBt != null && selectedValuesBt.length > 0) {
        if (selectedValuesBt.length > 36) {
            ShowFailedMessage('Cantidad de posiciones BT no permitidas, solo se permiten como máximo 36 y mínimo 1.');
            return false;
        }
        else {
            if (selectedValuesBt[0] == '') {
                arrBt = viewModel.Positions.BajaTension;
                if (arrBt.length > 36) {
                    ShowFailedMessage('Cantidad de posiciones BT no permitidas, solo se permiten como máximo 36 y mínimo 1.');
                    return false;
                }
            } else {
                arrBt = selectedValuesBt;
            }

            valor = true
        }
    }

    if (selectedValuesTer != null && selectedValuesTer.length > 0) {

        if (selectedValuesTer.length > 36) {
            ShowFailedMessage('Cantidad de posiciones TER no permitidas, solo se permiten como máximo 36 y mínimo 1.');
            return false;
        }
        else {
            if (selectedValuesTer[0] == '') {
                arrTer = viewModel.Positions.Terciario;
                if (arrTer.length > 36) {
                    ShowFailedMessage('Cantidad de posiciones BT no permitidas, solo se permiten como máximo 36 y mínimo 1.');
                    return false;
                }
            } else {
                arrTer = selectedValuesTer;
            }
        }



        valor = true
    }


    //set cantidades
    cantPosAt = arrAt.length;
    cantPosBt = arrBt.length;
    cantPosTer = arrTer.length;

    return valor
}

async function ValidateFilterJSON() {
    var path = "/Rod/ValidateFilter/";

    var url = new URL(domain + path),
        params = {
            noSerie: noSerieInput.value,
            claveIdioma: claveIdiomaInput.value,
            connection: $("#Connection").val(),
            material: material.value,
            unitOfMeasurement: unitOfMeasurement.value,
            testVoltage: testVoltage.value,
            comment: CommentsInput.value,
            posicionesAt: cantPosAt,
            posicionesBt: cantPosBt,
            posicionesTer: cantPosTer
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

async function GetFilterJSON() {
    var path = path = "/Rod/GetFilter/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value, measuring: unitOfMeasurement.value }
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
    var path = path = "/Rod/GetTemplate/";
    var url = new URL(domain + path),
        params = {
            noSerie: noSerieInput.value,
            clavePrueba: clavePrueba,
            claveIdioma: claveIdiomaInput.value,
            connection: $("#Connection").val(),
            unitType: viewModel.UnitType,
            material: material.value,
            unitOfMeasurement: unitOfMeasurement.value,
            testVoltage: testVoltage.value,
            comment: CommentsInput.value,
            posicionesAt: arrAt,
            posicionesBt: arrBt,
            posicionesTer: arrTer,
            numberColumns: numeroColumnas
        }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))
    $("#loader").css("display", "none");
    var win = window.open(url.toString(), '_blank');
    win.focus();
}

//Functions

function ValidarCombinaciones() {
    let arrayvalidar = $("#Connection").val();

    if (viewModel.IsAutrans) {
        let allowed = false

        let firstLetters = [];

        for (let i = 0; i < arrayvalidar.length; i++) {

            let firstLetter = arrayvalidar[i].charAt(0);
            let secondLetter = arrayvalidar[i].charAt(2);

            if ((firstLetter === 'H' || secondLetter === 'H') && !viewModel.HasAT) {
                allowed = false;
                break;
            }
            if ((firstLetter === 'X' || secondLetter === 'X') && !viewModel.HasBT) {
                allowed = false;
                break;
            }
            if ((firstLetter === 'Y' || secondLetter === 'Y') && !viewModel.HasTer) {
                allowed = false;
                break;
            }
            if (firstLetters.includes(firstLetter)) {
                allowed = false;
                break;
            } else {
                firstLetters.push(firstLetter);
            }

            allowed = true;
        }

        if (!allowed) {
            ShowFailedMessage("La combinación no es permitida, favor de corregirla.")
        }

        return allowed;
    }
    else {
        return true
    }
}


async function ValidarATBTTer() {
    await GetInfoResistance20Grades(noSerieInput.value, $("#Connection").val(), unitOfMeasurement.value,).then(
        data => {
            console.log(data)
        }
    );
}


function ValidarConexiones() {
    if ($('#Connection').val() == null) {
        ShowFailedMessage("El campo conexión de prueba es requerido")
        return false
    }
    else if (viewModel.IsAutrans) {
        if ($('#Connection').val().length > 3) {
            ShowFailedMessage("Solo se permite un máximo de 3 posiciones para un aparato tipo Autotransformador")
            return false
        } else {
            return true
        }
    } else {
        return true
    }

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

        GetPDFJSON(id, "ROD").then(
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

async function GetInfoResistance20Grades(noSerie, connection, unitOfMeasurement) {
    var path = path = "/Rod/CheckInfoResistance20Grades/";

    var url = new URL(domain + path),
        params = {
            noSerie: noSerie, connection: connection, unitOfMeasurement: unitOfMeasurement, posicionesAt: cantPosAt,
            posicionesBt: cantPosBt,
            posicionesTer: cantPosTer
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

async function GetPDFJSON(code, tyoeReport) {
    var path = path = "/Rod/GetPDFReport/";

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
    //unitType.value = response.UnitType;
    //unitTypeName.value = response.UnitTypeName;
    testVoltage.value = '';
    testVoltage.removeAttribute('disabled');

    var length = connection.options.length;
    for (i = length - 1; i > 0; i--) {
        connection.options[i] = null;
    }

    var opt;


    response.Connections.forEach(function (data, key) {

        opt = document.createElement("option");
        opt.value = data.Value;
        opt.innerHTML = data.Text;

        connection.appendChild(opt);
    });
    connection.removeAttribute('disabled');



    $.each(response.Positions.AltaTension, function (i, val) {
        $("#selectAT").append("<option value='" + val + "'>" + val + "</option>");
    });


    $.each(response.Positions.BajaTension, function (i, val) {
        $("#selectBT").append("<option value='" + val + "'>" + val + "</option>");
    });

    $.each(response.Positions.Terciario, function (i, val) {
        $("#selectTer").append("<option value='" + val + "'>" + val + "</option>");
    });

    if (response.Positions.Terciario.length > 0)
        $("#selectTer").prop("disabled", false);

    if (response.Positions.AltaTension.length > 0)
        $("#selectAT").prop("disabled", false);

    if (response.Positions.BajaTension.length > 0)
        $("#selectBT").prop("disabled", false);

    if (response.IsAutrans) {
        $('#Connection').attr('multiple', 'multiple');
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
    window.close();
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

    //viewModel.UnitType = unitType.value;
    viewModel.Connection = connection.value;
    viewModel.Material = material.value;
    viewModel.TestVoltage = testVoltage.value;
    viewModel.UnitOfMeasurement = unitOfMeasurement.value;
    viewModel.PosicionesAt = arrAt;
    viewModel.PosicionesBt = arrBt;
    viewModel.PosicionesTer = arrTer;
    if (loadWorkbook)
        viewModel.Workbook.sheets = $("#spreadsheet").getKendoSpreadsheet().toJSON().sheets;
}