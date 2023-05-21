let viewModel;
let settingsToDisplayPRDReportsDTO;
let btnSave = document.getElementById("btnSave");
let btnValidate = document.getElementById("btnValidate");
let btnRefresh = document.getElementById("btnRefresh");
var baseTemplate;
let spreadsheetElement;
var calculado = false;
let panelExcel = document.getElementById("spreadsheet");

//Evento
function TodoBien(view) {
    viewModel = view;
    baseTemplate = viewModel.Workbook;
    LoadTemplate(viewModel.Workbook, viewModel.Celdas, viewModel.Nombres);
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
    postData(domain + "/Prd/SavePDF/", viewModel)
        .then(data => {
            if (data.response.status !== -1) {
                ShowSuccessMessage("Guardado Exitoso.")
                btnSave.disabled = true;
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
    $("#loader").css("display", "block");
    LoadWorkbook();
    var data = viewModel;
    calculado = true;
    postData(domain + "/Prd/ValidateData/", data)
        .then(data => {
            if (data.response.Code === -2) {
                ShowFailedMessage(data.response.Description);
                btnSave.disabled = false;
                btnValidate.disabled = false;
            }
            else if (data.response.Code !== -1) {
                viewModel = data.response.Structure;
                SetDataSourceSpredsheet(viewModel.Workbook, viewModel.Celdas, viewModel.Nombres);
                ShowSuccessMessage("Validación Exitosa. Por favor corrobore los calculos.")
                btnSave.disabled = false;
                btnValidate.disabled = false;
                initCss();

                if (data.response.Description !== '') {
                    ShowFailedMessage(data.response.Description);
                }
            }
            else {
                viewModel = data.response.Structure;
                SetDataSourceSpredsheet(viewModel.Workbook, viewModel.Celdas, viewModel.Nombres);
                ShowFailedMessage(data.response.Description);
                btnSave.disabled = false;
                btnValidate.disabled = false;
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
function LoadTemplate(workbook, celdas, nombres) {
    SetConfigExcel(workbook, celdas, nombres);
}
function initCss() {
    $(".k-tabstrip-items").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-button-icontext").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-spreadsheet-quick-access-toolbar").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-link").css({ "color": "#fff" });
}

function SetDataSourceSpredsheet(workbook, celdas, nombres) {
    spreadsheetElement.data("kendoSpreadsheet").destroy()
    $("#spreadsheet").empty();
    SetConfigExcel(workbook, celdas, nombres);
}
function SetConfigExcel(workbook, celdas, nombres) {
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
            zoom:"150%"
        }
    });
    var spreadsheet = $("#spreadsheet").data("kendoSpreadsheet");
    var sheet = spreadsheet.activeSheet();
    var drawing = kendo.spreadsheet.Drawing.fromJSON({
        topLeftCell: "A1",
        offsetX: 0,
        offsetY: 0,
        width: 220,
        height: 43,
        image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    });
    sheet.addDrawing(drawing);

    length = celdas.length;
    for (i = length - 1; i > 0; i--) {
        if (nombres[i].includes('ImgU') || nombres[i].includes('ImgR4s') || nombres[i].includes('ImgReactLineal') || nombres[i].includes('ImgReactancia')) {
            drawing = kendo.spreadsheet.Drawing.fromJSON({
                topLeftCell: celdas[i],
                offsetX: 0,
                offsetY: 0,
                width: 30,
                height: 30,
                image: spreadsheet.addImage("/images/" + nombres[i])
            });
        }
        else if (nombres[i].includes('ImgPjm') || nombres[i].includes('ImgPjmc') || nombres[i].includes('ImgPt') || nombres[i].includes('ImgPe')) {
            drawing = kendo.spreadsheet.Drawing.fromJSON({
                topLeftCell: celdas[i],
                offsetX: 0,
                offsetY: 0,
                width: 160,
                height: 33,
                image: spreadsheet.addImage("/images/" + nombres[i])
            });
        }
        else if (nombres[i].includes('ImgP') || nombres[i].includes('ImgXc') || nombres[i].includes('ImgXm')) {
            drawing = kendo.spreadsheet.Drawing.fromJSON({
                topLeftCell: celdas[i],
                offsetX: 0,
                offsetY: 0,
                width: 55,
                height: 33,
                image: spreadsheet.addImage("/images/" + nombres[i])
            });
        }
        else {
            drawing = kendo.spreadsheet.Drawing.fromJSON({
                topLeftCell: celdas[i],
                offsetX: 0,
                offsetY: 0,
                width: 140,
                height: 33,
                image: spreadsheet.addImage("/images/" + nombres[i])
            });
        }
        
        sheet.addDrawing(drawing);
    }

    var range = spreadsheet.activeSheet().range("A60:J200");
    range.enable(false);
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
function LoadWorkbook(loadWorkbook = false, loadOfficial = false) {
    viewModel.Workbook.sheets = $("#spreadsheet").getKendoSpreadsheet().toJSON().sheets;
}