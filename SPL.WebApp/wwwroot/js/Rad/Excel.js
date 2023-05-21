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
    if (btnSave.disabled == false) {
        $("#loader").css("display", "block");
        LoadWorkbook();
        btnSave.disabled = true;
        postData(domain + "/Rad/SavePDF/", viewModel)
            .then(data => {
                if (data.response.Code !== -1) {
                    ShowSuccessMessage("Guardado Exitoso.")
                    btnSave.disabled = true;
                    btnValidate.disabled = true;
                    LoadPDF(data.response.nameFile, data.response.file);
                    initCss();
                    $("#loader").css("display", "none");
                }
                else {
                    ShowFailedMessage(data.response.Description);
                    btnSave.disabled = false;
                    $("#loader").css("display", "none");
                }
            });
    }
});



btnValidate.addEventListener("click", function () {

    if (btnValidate.disabled == false) {
        $("#loader").css("display", "block");
        LoadWorkbook();
        calculado = true;
        postData(domain + "/Rad/Validate/", viewModel)
            .then(data => {
                if (data.response.Code !== -1) {
                    viewModel = data.response.Structure;
                    SetDataSourceSpredsheet(viewModel.Workbook);
                    ShowSuccessMessage("Validación Exitosa. Por favor corrobore los calculos.")
                    btnSave.disabled = false;
                    btnValidate.disabled = false;
                    if (data.response.Description !== '') {
                        ShowFailedMessage(data.response.Description);
                    }
                    $("#loader").css("display", "none");
                    initCss();
                }
                else {
                    ShowFailedMessage(data.response.Description);
                    initCss();
                    $("#loader").css("display", "none");
                }
            });
    }
});

 function initCss() {
    $(".k-tabstrip-items").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-button-icontext").css({ "background": "#8854FF", "color": "#fff" });
     $(".k-spreadsheet-quick-access-toolbar").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-link").css({ "color": "#fff" });
}

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

function LoadPDF(nameFile,File) {
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

function LoadTemplate(workbook) {
    SetConfigExcel(workbook);
}

function SetDataSourceSpredsheet(workbook) {
    spreadsheetElement.data("kendoSpreadsheet").destroy()
    $("#spreadsheet").empty(); 
    SetConfigExcel(workbook);
}

function SetConfigExcel(workbook) {


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

    var drawing = kendo.spreadsheet.Drawing.fromJSON({
        topLeftCell: "A1",
        offsetX: 0,
        offsetY: 0,
        width: 220,
        height: 43,
        image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    });
    sheet.addDrawing(drawing);

    var celdaImg = viewModel.RADReportsDTO.ConfigurationReports.find(x => x.Dato === "ImgRAD");

    if (celdaImg !== undefined && celdaImg !== null) {
        drawing = kendo.spreadsheet.Drawing.fromJSON({
            topLeftCell: celdaImg.Celda,
            offsetX: 0,
            offsetY: 10,
            width: 50,
            height: 40,
            image: spreadsheet.addImage("/images/imgRAD.jpg")
        });
        sheet.addDrawing(drawing);
    }

   

    var range = spreadsheet.activeSheet().range("A60:J200");
    range.enable(false);
    initCss();
}
function LoadWorkbook(loadWorkbook = false, loadOfficial = false) {
    viewModel.Workbook.sheets = $("#spreadsheet").getKendoSpreadsheet().toJSON().sheets;
}



