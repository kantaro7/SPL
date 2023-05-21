let viewModel;
let settingsToDisplayPRDReportsDTO;
let btnSave = document.getElementById("btnSave");
let btnValidate = document.getElementById("btnValidate");
let btnRefresh = document.getElementById("btnRefresh");
var baseTemplate;
let spreadsheetElement;
var calculado = false;
let panelExcel = document.getElementById("spreadsheet");
var pasarResult = false

let configuraciones = [];
let idModule = 7
let errorFiles = false
let error = false

function TodoBien(view) {
    viewModel = view;
    baseTemplate = viewModel.Workbook;
    //console.log(viewModel)
    LoadTemplate(viewModel.Workbook);
    initCss();
    btnRefresh.disabled = false;
    btnSave.disabled = false;
    btnValidate.disabled = false;

    pasarTer = viewModel.Columnas === 3 ? false : true
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


async function test(elemento) {
    return new Promise(function (resolve, reject) {
        var reader = new FileReader();
        reader.onload = function () { resolve(reader.result); };
        reader.onerror = reject;
        reader.readAsDataURL(elemento.rawFile);
    });
}

btnValidate.addEventListener("click", async function () {
    $("#loader").css("display", "block");
    await onClickSelect();

    if (configuraciones === null || configuraciones === undefined) {
        ShowFailedMessage("No se ha podido cargar la configuracion de archivos para IND")
        $("#loader").css("display", "none");
    } else {
        $("#loader").css("display", "none");
        let modalReCalcular = new bootstrap.Modal($("#modalReCalcular"));
        modalReCalcular.show();
    }


});

btnSave.addEventListener("click", async function () {
    error = false;
    var files = $('#files').data('kendoUpload').getFiles()
    if (files.length > 0) {
        $.each(files, function () {
            //console.log(this)
            if (this.error !== undefined) {
                error = true;
            }
        })
    } else {
        ShowFailedMessage("Debe seleccionar por lo menos un archivo")
        return;
    }

    if (error) {
        ShowFailedMessage("Se han encontrados errores los archivos.")
        return;
    }

    LoadWorkbook();
    let cont = 0;
    viewModel.Archivos = [];

    //await test().then(data => viewModel.Archivos.push(data))
    for (const elemento of files) {
        var element = new Object();
        await test(elemento).then(data => { element.Base64 = data, element.Name = elemento.name });
        viewModel.Archivos.push(element);
    }

    $("#loader").css("display", "block");
    postData(domain + "/Ind/SavePDF/", viewModel)
        .then(data => {
            if (data.response.Code !== -1) {
                ShowSuccessMessage("Guardado Exitoso.");
                btnSave.disabled = true;
                btnRefresh.disabled = false;
                btnValidate.disabled = true;
                LoadPDF(data.response.nameFile, data.response.file);
                initCss();
            }
            else {
                ShowFailedMessage(data.response.Description);
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
    kendo.spreadsheet.defineFunction("REGEXP_MATCH", function (str, pattern, flags) {
        var rx;
        try {
            rx = flags ? new RegExp(pattern, flags) : new RegExp(pattern);
        } catch (ex) {
            // could not compile regexp, return some error code
            return new kendo.spreadsheet.CalcError("REGEXP");
        }
        return rx.test(str);
    }).args([
        ["str", "string"],
        ["pattern", "string"],
        ["flags", ["or", "string", "null"]]
    ]);


    kendo.spreadsheet.defineFunction("HOLA", function (str, valore, flags) {
        return true
    }).args([
        ["str", "string"],
        ["valore", "string"],
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
                    text: "Cargar Archivos",
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
    var range = spreadsheet.activeSheet().range("A1:T200");
    range.enable(false);

    // validates if the formula returns a non-false value (see the `from` field).

    var drawing = kendo.spreadsheet.Drawing.fromJSON({
        topLeftCell: "A1",
        offsetX: 0,
        offsetY: 0,
        width: 220,
        height: 43,
        image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    });

    sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'Fecha').Celda).enable(true)


    if (viewModel.Pruebas === 'TER' || viewModel.Pruebas === 'ALA' || viewModel.Pruebas === 'CBO') {
        sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'TotPaginas' && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)
    }

    if (viewModel.Pruebas === 'TER') {
        sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'ValorKW' && x.ClavePrueba === viewModel.Pruebas).Celda + ":N12").enable(true)
        sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'ValorMVA' && x.ClavePrueba === viewModel.Pruebas).Celda + ":R12").enable(true)
        sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'Anexo' && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)

    }

    if (viewModel.Pruebas === 'ACT') {
        sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'NoPagina1' && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)
        sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'NoPagina2' && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)
        sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'Anexo' && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)

    }

    if (viewModel.Pruebas === 'ALA') {
        sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'NoPagina1' && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)
        sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'NoPagina2' && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)
        sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'NoPagina3' && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)
        sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'NoPagina4' && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)
        sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'NoPagina5' && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)
    }

    if (viewModel.Pruebas === 'CBO') {
        sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'Anexo' && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)
        sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'TotPaginas' && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)
    }


    if (viewModel.Pruebas === 'CTC') {
        sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'Anexo' && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)

        if (viewModel.LlevaTC.toUpperCase() === "SI") {
            sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'NoPaginaIni' && x.Seccion === 1 && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)
            sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'NoPaginaFin' && x.Seccion === 1 && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)
            sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'NoPaginaIni' && x.Seccion === 2 && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)
            sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'NoPaginaFin' && x.Seccion === 2 && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)
        } else {
            sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'NoPaginaIni' && x.Seccion === 1 && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)
            sheet.range(viewModel.SettingsIND.ConfigurationReports.find(x => x.Dato === 'NoPaginaFin' && x.Seccion === 1 && x.ClavePrueba === viewModel.Pruebas).Celda).enable(true)
        }
    }

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
    viewModel
}
function onSelectFiles(e) {
    var totalFiles = e.sender.getFiles();
    if (totalFiles.length + 1 > 5) {

        ShowFailedMessage("Solo se permiten 5 archivos");
        e.preventDefault();
    }


    console.log(configuraciones)
    var files = e.files;

    for (var i = 0; i < files.length; i += 1) {
        var file = files[i];

        var result = configuraciones.filter(x => x.Extension.toUpperCase() == file.extension.toUpperCase())

        if (result.length === 0) {
            file.error = "La extension " + file.extension + " no ha sido encontrada en la configuracion"
        } else if (result.length > 0) {
            var pesoMax = result[0].PesoMaximo;
            console.log(file.size)
            console.log(pesoMax)
            if (pesoMax * 1024 * 1024 < file.size) {
                file.error = "El peso del archivo supera los " + result[0].PesoMaximo + "MB"
            } else {
                file.success = "Archivo agregado"
                setTimeout(function () {
                    $(".k-file-success div.uno span#wrapper-message").css("margin-left", "35px")
                })
            }
        }
    }
}

async function onClickSelect() {
    await GetConfigurationFilesJSON().then(
        data => {
            if (data.response.Code === 1) {
                configuraciones = data.response.Structure;
                console.log(configuraciones)
            } else {
                ShowFailedMessage("Error al obtener la confiuracion del archivo")
            }
        }
    )
}

async function GetConfigurationFilesJSON() {

    var path = path = "/Ind/GetConfigurationFiles/?pIdModule=" + idModule;
    var url = new URL(domain + path)

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

