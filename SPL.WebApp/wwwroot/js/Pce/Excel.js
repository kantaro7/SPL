
//Variables
let viewModel;
let btnSave = document.getElementById("btnSave");
let btnValidate = document.getElementById("btnValidate");
let spreadsheetElement;
let btnRefresh = document.getElementById("btnRefresh");
var baseTemplate;
var calculado = false;
let graficImg = document.getElementById("graficImg");
let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");
let panelExcel = document.getElementById("spreadsheet");

var chart;

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

function LoadTemplate(workbook, grafico = "") {
    SetConfigExcel(workbook, grafico);
}
function initCss() {
    $(".k-tabstrip-items").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-button-icontext").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-spreadsheet-quick-access-toolbar").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-link").css({ "color": "#fff" });
}


btnSave.addEventListener("click", function () {
    LoadWorkbook();
    $("#loader").css("display", "block");
    postData(domain + "/Pce/SavePDF/", viewModel)
        .then(data => {
            if (data.response.status !== -1) {
                ShowSuccessMessage("Guardado Exitoso.")
                btnSave.disabled = true;
                btnValidate.disabled = true;
                LoadPDF(data.response.nameFile, data.response.file);
                initCss();
                $("#loader").css("display", "none");
            }
            else {
                ShowFailedMessage(data.response.description);
                $("#loader").css("display", "none");
            }
           
        });
});

btnValidate.addEventListener("click", function () {
    $("#loader").css("display", "block");
    LoadWorkbook();
    var data = viewModel;
    postData(domain + "/Pce/ValidateData/", data)
        .then(data => {
            if (data.response.Code === -2) {
                ShowFailedMessage(data.response.Description);
                btnSave.disabled = false;
                btnValidate.disabled = false;
                $("#loader").css("display", "none");
            }
            else if (data.response.Code !== -1) {
                viewModel = data.response.Structure;
                if (data.response.Description != "") {
                    ShowWarningMessage("Validación Exitosa. "+ data.response.Description);
                }
                btnSave.disabled = false;
                btnValidate.disabled = false;
                if (viewModel.Grafic) {
                    GenerateGraphic(data.response.Coords);
                    chart.getJpgBase64String(function (response) {
                        viewModel.Base64Graphic = response;
                        graficImg.src = "data:image/jpeg;base64,"+response;
                        //graficImg.removeAttribute('style');
                        //graficImg.setAttribute('style', 'width: 300px; height: 200px;')
                        //document.getElementById("container").removeAttribute('style');
                        //document.getElementById("container").setAttribute('style', 'display: none');
                        SetDataSourceSpredsheet(viewModel.Workbook, response);
                        initCss();
                    });

                } else {
                    SetDataSourceSpredsheet(viewModel.Workbook);
                    initCss();
                }              

                $("#loader").css("display", "none");
            }
            else {
                viewModel = data.response.Structure;
                SetDataSourceSpredsheet(viewModel.Workbook);
                ShowFailedMessage(data.response.Description);
                btnSave.disabled = false;
                btnValidate.disabled = false;
                initCss();
                $("#loader").css("display", "none");
            }
            $("#loader").css("display", "none");
        });

});

function SetDataSourceSpredsheet(workbook, grafico = "") {
    spreadsheetElement.data("kendoSpreadsheet").destroy()
    $("#spreadsheet").empty();
    SetConfigExcel(workbook, grafico);
}

function SetConfigExcel(workbook, grafico = "") {
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


    sheet.addDrawing(drawing);
    if (grafico != "") {
        var graficoo = kendo.spreadsheet.Drawing.fromJSON({
            topLeftCell: "B40",
            offsetX: 0,
            offsetY: 0,
            width: 500,
            height: 170,
            image: spreadsheet.addImage("data:image/png;base64,"+grafico)
        });
        sheet.addDrawing(graficoo);
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

function GenerateGraphic(datas) {
    var valorxy = [[0,0]];
    for (var i = 1; i < datas.length + 1; i++) {
        valorxy[i] = [Number(datas[i - 1].x), Number(datas[i - 1].y)];
    }
  
    // create a chart
    chart = anychart.scatter();

    // create a line series and set the data
    var series = chart.line(valorxy);
    // set the container id
    chart.container("container");
    chart.bgColor = "#F8F4F0";

   
/*    chart.yScale(anychart.scales.log());*/
    chart.yScale().ticks().interval(5);
    chart.xScale().ticks().interval(0.2);
    // initiate drawing the chart
    chart.draw();

    $(".anychart-credits-text").css("color", "transparent");
    $('.anychart-credits').css('display', 'none');
}


function LoadWorkbook(loadWorkbook = false, loadOfficial = false) {
    viewModel.Workbook.sheets = $("#spreadsheet").getKendoSpreadsheet().toJSON().sheets;
}