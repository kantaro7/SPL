let viewModel;
let settingsToDisplayPRDReportsDTO;
let btnSave = document.getElementById("btnSave");
let btnValidate = document.getElementById("btnValidate");
let btnRefresh = document.getElementById("btnRefresh");
var baseTemplate;
let spreadsheetElement;
var calculado = false;
let panelExcel = document.getElementById("spreadsheet");
var dlg;
let resultCheckPassword = false;
function TodoBien(view) {
    viewModel = view;
    baseTemplate = viewModel.Workbook;
    LoadTemplate(viewModel.Workbook);
    initCss();
    btnRefresh.disabled = false;
    btnSave.disabled = true;
    btnValidate.disabled = false;
}

btnRefresh.addEventListener("click", function () {
    if (calculado) {
        location.reload();
    } else {
        btnRefresh.disabled = false;
        btnSave.disabled = true;
        btnValidate.disabled = false;
        SetDataSourceSpredsheet(baseTemplate);
        initCss();
    }
});

btnSave.addEventListener("click", function () {
    LoadWorkbook();

    $("#loader").css("display", "block");
    postData(domain + "/Tdp/SavePDF/", viewModel)
        .then(data => {
            if (data.response.status !== -1) {
                ShowSuccessMessage("Guardado Exitoso.")
                btnSave.disabled = true;
                btnRefresh.disabled = false;
                btnValidate.disabled = true;
                LoadPDF(data.response.nameFile, data.response.file);
                initCss();
            }
            else {
                ShowFailedMessage(data.response.description);
            }
            $("#loader").css("display", "none");
        });
});

btnValidate.addEventListener("click", function () {

    var spreadsheet = $("#spreadsheet").data("kendoSpreadsheet");
    var sheet = spreadsheet.activeSheet();

    var posAT = spreadsheet.activeSheet().range(viewModel.CeldasPositions.CeldaAT);
    var posBT = spreadsheet.activeSheet().range(viewModel.CeldasPositions.CeldaBT);
    var posTer = spreadsheet.activeSheet().range(viewModel.CeldasPositions.CeldaTer);
    if (posAT.value() === null) {
        ShowFailedMessage("Favor de proporcionar la posición de alta tensión")
        return
    }

    if (posBT.value() === null) {
        ShowFailedMessage("Favor de proporcionar la posición de baja tensión")
        return
    }


    var resultAt = viewModel.Positions.AltaTension.find(x => x === posAT?.value()?.toString())
    var resultBt = viewModel.Positions.BajaTension.find(x => x === posBT?.value()?.toString())
    var resultTer = viewModel.Positions.Terciario.find(x => x === posTer?.value()?.toString())

    if (resultAt === undefined) {
        ShowFailedMessage("La posición de alta tensión no es válida")
        return

    }
    if (resultBt === undefined) {
        ShowFailedMessage("La posición de baja tensión no es válida")
        return

    }

    if (posTer.value() !== null) {
        if (resultTer === undefined) {
            ShowFailedMessage("La posición terciario no es válida")
            return

        }
    }
    


    viewModel.Notes = (sheet.range('D49').value() !== null ? sheet.range('D49').value() : "") + " " + (sheet.range('C50').value() !== null ? sheet.range('C50').value() : "")
    $("#loader").css("display", "block");
    LoadWorkbook();
    var data = viewModel;
    calculado = true;
    postData(domain + "/Tdp/ValidateData/", data)
        .then(data => {
            if (data.response.Code === -2) {
                ShowFailedMessage(data.response.Description);
                btnSave.disabled = false;
                btnValidate.disabled = false;
                btnRefresh.disabled = false;
            }
            else if (data.response.Code === -1) {
                ShowFailedMessage(data.response.Description);
                btnSave.disabled = true;
                btnValidate.disabled = false;
                btnRefresh.disabled = false;
                $("#loader").css("display", "none");
            }
            else if (data.response.Code !== -1) {
                viewModel = data.response.Structure;
                SetDataSourceSpredsheet(viewModel.Workbook);
                ShowSuccessMessage("Validación Exitosa. Por favor corrobore los calculos.")
                btnRefresh.disabled = false;
                btnSave.disabled = false;
                btnValidate.disabled = false;
                initCss();
                if (data.response.Description !== '') {
                    ShowFailedMessageWithModal(data.response.Description);
                }
            }
            else {
                viewModel = data.response.Structure;
                SetDataSourceSpredsheet(viewModel.Workbook);
                ShowFailedMessage(data.response.Description);
                btnSave.disabled = false;
                btnValidate.disabled = false;
                btnRefresh.disabled = false;
                initCss();
            }
            $("#loader").css("display", "none");
        });

});

//Requests
async function postData(url = '', data = {}) {
    // Opciones por defecto estan marcadas con un *
    const response = await fetch(url, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        mode: 'same-origin', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
            'Content-Type': 'application/json'
            // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'same-origin', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: JSON.stringify(data) // body data type must match "Content-Type" header
    });
    if (response.ok) {
        return response.json(); // parses JSON response into native JavaScript objects
    }
    else {
        return null
    }

}

//Functions

function LoadTemplate(workbook) {
    SetConfigExcel(workbook);
}
function initCss() {
    $(".k-tabstrip-items").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-button-icontext").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-spreadsheet-quick-access-toolbar").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-link").css({ "color": "#fff" });
}

function SetDataSourceSpredsheet(workbook) {
    spreadsheetElement.data("kendoSpreadsheet").destroy()
    $("#spreadsheet").empty();
    SetConfigExcel(workbook);
}

function SetConfigExcel(workbook) {

    kendo.spreadsheet.defineFunction("CHECK_POSITIONS", function (str, pattern, flags) {
        var rx;
        try {
            rx = flags ? new RegExp(pattern, flags) : new RegExp(pattern);
        } catch (ex) {
            // could not compile regexp, return some error code
            return new kendo.spreadsheet.CalcError("REGEXP");
        }
        //alert(rx.test(str))
        return rx.test(str);
    }).args([
        ["str", "string"],
        ["pattern", "string"],
        ["flags", ["or", "string", "null"]]
    ]);


    spreadsheetElement = $("#spreadsheet").kendoSpreadsheet({
        sheets: workbook.sheets,
        toolbar: {

            backgroundColor: "#3f51b5 !important",
            textColor: "#fff",
            home: [

                {
                    type: "button",
                    text: "Calcular",
                    showText: "both",
                    icon: "k-icon k-i-calculator",


                    click: function () {
                        btnValidate.click();
                 
                    }
                },
                {
                    type: "button",
                    text: "Guardar",
                    showText: "both",
                    icon: "k-icon k-i-save",

                    click: function () {
                        btnSave.click();
                        

                    }
                },
                {
                    type: "button",
                    text: "Reiniciar",
                    showText: "both",
                    icon: "k-icon k-i-refresh",

                    click: function () {
                        btnRefresh.click();
                    }
                },
                {
                    type: "button",
                    text: "No Prueba: " + viewModel.NoPrueba,
                    showText: "both",
                    attributes: { "style": "color:#8854FF;pointer-events:none;border:none;background:none;" }
                }
            ],
            insert: false,
            data: false,
            redo: false,
            undo: false,
        }
    });

    var spreadsheet = $("#spreadsheet").data("kendoSpreadsheet");
    var sheet = spreadsheet.activeSheet();
    var range = spreadsheet.activeSheet().range("A65:J200");
    range.enable(false);
    sheet.range('D49:G49').enable(true);
    sheet.range('C50:G50').enable(true);
    sheet.range(viewModel.CeldasPositions.CeldaAT).validation({
        comparerType: "custom",
        dataType: "custom",
        from: 'AND(CHECK_POSITIONS(' + viewModel.CeldasPositions.CeldaAT + ',"^[a-zA-Z0-9]*$"),LEN(' + viewModel.CeldasPositions.CeldaAT + ')<=5)',
        titleTemplate: "Error",
        showButton: true,
        type: "reject",
        allowNulls: false,
        messageTemplate: "La posición de alta tensión no puede excederse de 5 caracteres y no puede estar vacio"
    });
    sheet.range(viewModel.CeldasPositions.CeldaAT).enable(true);

    sheet.range(viewModel.CeldasPositions.CeldaBT).validation({
        comparerType: "custom",
        dataType: "custom",
        from: 'AND(CHECK_POSITIONS(' + viewModel.CeldasPositions.CeldaBT + ',"^[a-zA-Z0-9]*$"),LEN(' + viewModel.CeldasPositions.CeldaBT + ')<=5)',
        titleTemplate: "Error",
        showButton: true,
        type: "reject",
        allowNulls: false,
        messageTemplate: "La posición de baja tensión no puede excederse de 5 caracteres y no puede estar vacio"
    });
    sheet.range(viewModel.CeldasPositions.CeldaBT).enable(true);

    sheet.range(viewModel.CeldasPositions.CeldaTer).validation({
        comparerType: "custom",
        dataType: "custom",
        from: 'AND(CHECK_POSITIONS(' + viewModel.CeldasPositions.CeldaTer + ',"^[a-zA-Z0-9]*$"),LEN(' + viewModel.CeldasPositions.CeldaTer + ')<=5)',
        titleTemplate: "Error",
        showButton: true,
        type: "reject",
        allowNulls: true,
        messageTemplate: "La posición de terciario no puede excederse de 5 caracteres"
    });
    sheet.range(viewModel.CeldasPositions.CeldaTer).enable(true);

    var drawing = kendo.spreadsheet.Drawing.fromJSON({
        topLeftCell: "A1",
        offsetX: 0,
        offsetY: 0,
        width: 220,
        height: 43,
        image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    });


    sheet.addDrawing(drawing);
    initCss();
}

function LoadPDF(nameFile, File) {    
    console.log(File)
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
function LoadWorkbook(loadWorkbook = false, loadOfficial = false) {
    viewModel.Workbook.sheets = $("#spreadsheet").getKendoSpreadsheet().toJSON().sheets;
}
function dialogo() {

    crearDialog();
    var scriptTemplate = kendo.template($("#jobSplitTemplate").html());
    var scriptData = { id: 5 };

    dlg.html(scriptTemplate(scriptData));
    dlg.data("kendoDialog").open();

    $(".k-dialog-buttongroup").css("display", "initial")
    $(".k-dialog-buttongroup").css("text-align", "center")

    $(".k-window-title").css("text-align", "center")
    $(".k-window-content").css("text-align", "center")
    $(".k-window-title").css("display", "initial")

}
function crearDialog() {
    var dialog = $('#dialog')
    dlg = dialog.kendoDialog({
        width: "600px",
        title: "Desea cambiar el resultado de la prueba a Aceptado?",
        content: "Para cambiar el resultado debe introducir la clave de autorizacion en el recuadro de abajo",
        closable: false,
        visible: false,
        modal: true,
        actions: [
            {
                text: 'Ok',
                action: fun,
                cssClass: "btnOk"
            },
            {
                text: 'Cancel', cssClass: "dialogClass", primary: true
            }
        ],
        close: onClose
    });
}
function onClose() {
    var dialog = $("#dialog").data("kendoDialog");
    dialog.destroy();

    $("#example").append("<div id='dialog'></div>")
}
function fun() {
    $("#loaderdialog").css("display", "initial");
    isCheckingPassword = true

    if (!resultCheckPassword) {
        CheckClaveAuth().then(
            data => {

                if (data.response.Code !== -1 && data.response.Structure) {
                    $("#loaderdialog").css("display", "none");
                    $("#span-resultado").css("display", "initial")
                    $("#resultado-clave").html("<strong>Clave correcta. El resultado de la prueba ha sido cambiado a Aceptado</strong>")
                    $("#resultado-clave").css("color", "green")
                    resultCheckPassword = true
                    $(".btnOk").text("Cerrar")
                    $(".dialogClass").css("display", "none")

                    viewModel.ClaveAutoriza = true
                    viewModel.IsReportAproved = true

                    var spreadsheet = $("#spreadsheet").data("kendoSpreadsheet");
                    var celdas = viewModel.SettingsTDP.ConfigurationReports.filter(x => x.Dato === "Resultado")
                    let idioma = viewModel.ClaveIdioma === "EN" ? "Accepted" : "Aceptado"
                    for (let o = 0; o < celdas.length; o++) {
                        var range = spreadsheet.activeSheet().range(celdas[o].Celda).value(idioma);
                    }
                }
                else {
                    $("#loaderdialog").css("display", "none");
                    $("#span-resultado").css("display", "initial")
                    $("#resultado-clave").html("<strong>Clave incorrecta</strong>")
                    $("#resultado-clave").css("color", "red")
                    isCheckingPassword = false
                }
            }
        );
    } else {
        resultCheckPassword = false
        isCheckingPassword = false
        $(".dialogClass").click()
    }

    return false;
}
async function CheckClaveAuth() {
    var path = "/Tdp/GetClaveAuth/";
    console.log(viewModel);
    var url = new URL(domain + path),
        params = {
            nroSerie: viewModel.NoSerie,
            keyTest: viewModel.Pruebas,
            lenguage: viewModel.ClaveIdioma,
            noCycles: viewModel.NroCiclosText,
            totalTime: viewModel.TiempoTotal,
            interval: viewModel.TiempoIntervalo,
            timeLevel: viewModel.NivelHora,
            outputLevel: viewModel.NivelRealce,
            descMayPc: viewModel.DescargaPC,
            descMayMv: viewModel.DescargaUV,
            incMaxPc: viewModel.IncrementoMaxPC,
            voltageLevels: viewModel.VoltageLevel,
            measurementType: viewModel.TipoMedicion,
            terminalsTest: viewModel.TerminalesProbar,
            claveIntroducida: $("#claveAuth").val()
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