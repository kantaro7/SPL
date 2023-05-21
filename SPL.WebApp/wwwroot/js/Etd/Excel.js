
//Variables
let viewModel;
let btnSave = document.getElementById("btnSave");
let btnValidate = document.getElementById("btnValidate");
let spreadsheetElement;
let btnRefresh = document.getElementById("btnRefresh");
var baseTemplate;
var calculado = false;
let graficImg1 = document.getElementById("graficImg1");
let graficImg2 = document.getElementById("graficImg2");
let graficImg3 = document.getElementById("graficImg3");
let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");
let panelExcel = document.getElementById("spreadsheet");

var chart1;
var chart2;
var chart3;

//Evento
function TodoBien(view) {
    viewModel = view;
    baseTemplate = viewModel.Workbook;
    GenerateGraphic(viewModel.Coords);

    if (viewModel.Coords.length == 2) {
        chart1.getJpgBase64String(function (response1) {
            viewModel.Base64Graphics[0] = response1;
            graficImg1.src = "data:image/jpeg;base64," + response1;
            chart2.getJpgBase64String(function (response2) {
                viewModel.Base64Graphics[1] = response2;
                graficImg2.src = "data:image/jpeg;base64," + response2;
                LoadTemplate(viewModel.Workbook, response1, response2);
                initCss();
            });
            
        });
    } else {
        chart1.getJpgBase64String(function (response1) {
            viewModel.Base64Graphics[0] = response1;
            graficImg1.src = "data:image/jpeg;base64," + response1;
            chart2.getJpgBase64String(function (response2) {
                viewModel.Base64Graphics[1] = response2;
                graficImg2.src = "data:image/jpeg;base64," + response2;
                chart3.getJpgBase64String(function (response3) {
                    viewModel.Base64Graphics[2] = response3;
                    graficImg3.src = "data:image/jpeg;base64," + response3;
                    LoadTemplate(viewModel.Workbook, response1, response2, response3);
                    initCss();
                });
            });

        });
    }

    
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
        if (viewModel.Base64Graphics.length == 2) {
            SetDataSourceSpredsheet(baseTemplate, viewModel.Base64Graphics[0], viewModel.Base64Graphics[1]);
            initCss();
        } else if (viewModel.Base64Graphics.length == 3) {
            SetDataSourceSpredsheet(baseTemplate, viewModel.Base64Graphics[0], viewModel.Base64Graphics[1], viewModel.Base64Graphics[2]);
            initCss();
        }
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

function LoadTemplate(workbook, grafico1 = "", grafico2 = "", grafico3 = "") {
    SetConfigExcel(workbook, grafico1, grafico2, grafico3);
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
    postData(domain + "/Etd/SavePDF/", viewModel)
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
    postData(domain + "/Etd/ValidateData/", data)
        .then(data => {
            if (data.response.Code === -2) {
                ShowFailedMessage(data.response.Description);
                btnSave.disabled = false;
                btnValidate.disabled = false;
                $("#loader").css("display", "none");
            }
            else if (data.response.Code !== -1) {
                viewModel = data.response.Structure;
                ShowSuccessMessage("Validación Exitosa. Por favor corrobore los calculos.")
                btnSave.disabled = false;
                btnValidate.disabled = false;
                if (viewModel.Base64Graphics.length == 2) {
                    SetDataSourceSpredsheet(viewModel.Workbook, viewModel.Base64Graphics[0], viewModel.Base64Graphics[1]);
                    initCss();
                } else if (viewModel.Base64Graphics.length == 3) {
                    SetDataSourceSpredsheet(viewModel.Workbook, viewModel.Base64Graphics[0], viewModel.Base64Graphics[1], viewModel.Base64Graphics[2]);
                    initCss();
                }
              
                if (data.response.Description !== '') {
                    ShowFailedMessage(data.response.Description);
                }

                $("#loader").css("display", "none");
            } else {
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

function SetDataSourceSpredsheet(workbook, grafico1 = "", grafico2 = "", grafico3 = "") {
    spreadsheetElement.data("kendoSpreadsheet").destroy()
    $("#spreadsheet").empty();
    SetConfigExcel(workbook, grafico1, grafico2, grafico3);
}

function SetConfigExcel(workbook, grafico1 = "", grafico2 = "", grafico3 = "") {
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
    var drawing1 = kendo.spreadsheet.Drawing.fromJSON({
        topLeftCell: "A1",
        offsetX: 0,
        offsetY: 0,
        width: 220,
        height: 43,
        image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    });
    sheet.addDrawing(drawing1);

    var drawing2 = kendo.spreadsheet.Drawing.fromJSON({
        topLeftCell: "A60",
        offsetX: 0,
        offsetY: 0,
        width: 220,
        height: 43,
        image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    });
    sheet.addDrawing(drawing2);

    var drawing3 = kendo.spreadsheet.Drawing.fromJSON({
        topLeftCell: "A119",
        offsetX: 0,
        offsetY: 0,
        width: 220,
        height: 43,
        image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    });
    sheet.addDrawing(drawing3);
    if (viewModel.Base64Graphics.length == 3) {
        var drawing4 = kendo.spreadsheet.Drawing.fromJSON({
            topLeftCell: "A178",
            offsetX: 0,
            offsetY: 0,
            width: 220,
            height: 43,
            image: spreadsheet.addImage("/images/prolecge_excel.jpg")
        });
        sheet.addDrawing(drawing4);
    }

    if (grafico1 != "") {
        var graficoo1 = kendo.spreadsheet.Drawing.fromJSON({
            topLeftCell: "E82",
            offsetX: 0,
            offsetY: 0,
            width: 500,
            height: 170,
            image: spreadsheet.addImage("data:image/png;base64,"+grafico1)
        });
        sheet.addDrawing(graficoo1);
    }

    if (grafico2 != "") {
        var graficoo2 = kendo.spreadsheet.Drawing.fromJSON({
            topLeftCell: "E141",
            offsetX: 0,
            offsetY: 0,
            width: 500,
            height: 170,
            image: spreadsheet.addImage("data:image/png;base64," + grafico2)
        });
        sheet.addDrawing(graficoo2);
    }

    if (grafico3 != "") {
        var graficoo3 = kendo.spreadsheet.Drawing.fromJSON({
            topLeftCell: "E200",
            offsetX: 0,
            offsetY: 0,
            width: 500,
            height: 170,
            image: spreadsheet.addImage("data:image/png;base64," + grafico3)
        });
        sheet.addDrawing(graficoo3);
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

    for (var k = 0; k < datas.length; k++) {
        var valorxy = [[0, 0]];
        for (var i = 1; i < datas[k].length + 1; i++) {
            valorxy[i] = [Number(datas[k][i - 1].x), Number(datas[k][i - 1].y)];
        }
        if (k == 0) {
            // create a chart
            chart1 = anychart.line();

            // create a line series and set the data
            var series1 = chart1.line(valorxy);
            // set the container id
            chart1.container("container");
            chart1.bgColor = "#F8F4F0";


            /*    chart.yScale(anychart.scales.log());*/
            chart1.yScale().ticks().interval(20, 000);
            // initiate drawing the chart
            chart1.draw();
        } else if (k == 1) {
            // create a chart
            chart2 = anychart.line();

            // create a line series and set the data
            var series2 = chart2.line(valorxy);
            // set the container id
            chart2.container("container");
            chart2.bgColor = "#F8F4F0";


            /*    chart.yScale(anychart.scales.log());*/
            chart2.yScale().ticks().interval(20, 000);
            // initiate drawing the chart
            chart2.draw();
        } else if (k == 2) {
            // create a chart
            chart3 = anychart.line();

            // create a line series and set the data
            var series = chart3.line(valorxy);
            // set the container id
            chart3.container("container");
            chart3.bgColor = "#F8F4F0";


            /*    chart.yScale(anychart.scales.log());*/
            chart3.yScale().ticks().interval(20, 000);
            // initiate drawing the chart
            chart3.draw();
        }

        
    }
    

    $(".anychart-credits-text").css("color", "transparent");
    $('.anychart-credits').css('display', 'none');
}


function LoadWorkbook(loadWorkbook = false, loadOfficial = false) {
    viewModel.Workbook.sheets = $("#spreadsheet").getKendoSpreadsheet().toJSON().sheets;
}