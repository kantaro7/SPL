let viewModel;
let settingsToDisplayPRDReportsDTO;
let btnSave = document.getElementById("btnSave");
let btnValidate = document.getElementById("btnValidate");
let btnRefresh = document.getElementById("btnRefresh");
var baseTemplate;
let spreadsheetElement;
var calculado = false;
let panelExcel = document.getElementById("spreadsheet");
let valueAcepCapReCalcuar;
let valueAcepFpReCalcuar;
function TodoBien(view) {
    viewModel = view;
    baseTemplate = viewModel.Workbook;
    LoadTemplate(viewModel.Workbook);
    initCss();
    valueAcepCapReCalcuar = viewModel.AcceptanceValueCap;
    valueAcepFpReCalcuar = viewModel.AcceptanceValueFP;
    btnRefresh.disabled = false;
    btnSave.disabled = true;
    btnValidate.disabled = false;
}
function LoadTemplate(workbook) {
    SetConfigExcel(workbook);
}
function initCss() {
    $(".k-tabstrip-items").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-button-icontext").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-spreadsheet-quick-access-toolbar").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-link").css({ "color": "#fff" });
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
    var range = spreadsheet.activeSheet().range("A65:J200");
    range.enable(false);

    var drawing = kendo.spreadsheet.Drawing.fromJSON({
        topLeftCell: "A1",
        offsetX: 0,
        offsetY: 0,
        width: 220,
        height: 43,
        image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    });


    sheet.addDrawing(drawing);
}
//Evento
$(document).ready(function () {
    let modalReCalcular = new bootstrap.Modal($("#modalReCalcular"));
    

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
        LoadWorkbook()
        var spreadsheet = $("#spreadsheet").data("kendoSpreadsheet");
        var sheet = spreadsheet.activeSheet()
        viewModel.FechaSec1 = sheet.range("I10").value();
        viewModel.FechaSec2 = sheet.range("I31").value();
        $("#loader").css("display", "block");
        postData(domain + "/Fpb/SavePDF/", viewModel)
            .then(data => {
                if (data.response.status !== -1) {
                    ShowSuccessMessage("Guardado Exitoso.")
                    btnRefresh.disabled = false;
                    btnSave.disabled = true;
                    btnValidate.disabled = true;
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

    btnReCalcular.addEventListener("click", function () {
        if ($("#ValorAcepCap").val() == '' || $("#ValorAcepCap").val() == undefined) {
            ShowFailedMessage('Valor de Aceptacion de Capacitancia es requerido');
        } else if ($("#ValorAcepFp").val() == '' || $("#ValorAcepFp").val() == undefined) {
            ShowFailedMessage('Valor de Aceptacion de Factor de Potencia es requerido');
        } else {
            modalReCalcular.hide();
            viewModel.AcceptanceValueCap = $("#ValorAcepCap").val();
            viewModel.AcceptanceValueFP = $("#ValorAcepFp").val();
            btnValidate.click();
        }
        

    });

    btnValidate.addEventListener("click", function () {
        $("#loader").css("display", "block");
        LoadWorkbook();
        calculado = true;
        var data = viewModel;
        postData(domain + "/Fpb/ValidateData/", data)
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
                    $("#loader").css("display", "none");
                }
                else if (data.response.Code !== -1) {
                    viewModel = data.response.Structure;
                    SetDataSourceSpredsheet(viewModel.Workbook);
                    if (data.response.description != "") {
                        ShowFailedMessage(data.response.Description);
                    }
                    ShowSuccessMessage("Validación Exitosa. Por favor corrobore los calculos.")
                    btnRefresh.disabled = false;
                    btnSave.disabled = false;
                    btnValidate.disabled = false;
                    initCss();

                    var Bandera = false;
                    if (data.response.Description !== '') {
                        ShowFailedMessage(data.response.Description);
                        for (let i = 0; i < viewModel.ValidateCapacitancesCAP.length; i++) {

                            if (viewModel.ValidateCapacitancesCAP[i].Valor > valueAcepCapReCalcuar) {
                                Bandera = true;
                                var dataMap = viewModel.ValidateCapacitancesCAP;
                                let dataS = dataMap.map((item) => {
                                    return {
                                        Sección: item.Seccion,
                                        Validación: item.Columnas,
                                        Resultado: item.Valor

                                    }
                                })


                                $("#grid").kendoGrid({
                                    dataSource: dataS,
                                    pageable: {
                                        input: false,
                                        numeric: true,
                                        butonCount: 5,
                                        pageSize: 10,
                                        alwaysVisible: true,
                                        previousNext: true
                                    },
                                    columns: [
                                        { field: "Sección", width: 40 },
                                        { field: "Validación", width: 210 },
                                        { field: "Resultado", width: 50 }


                                    ]

                                });

                                break;
                             
                            }
                       
                        }




                        for (let i = 0; i < viewModel.ValidateCapacitancesFP.length; i++) {

                            if (viewModel.ValidateCapacitancesFP[i].Valor > valueAcepFpReCalcuar) {
                                Bandera = true;
                                var dataMap = viewModel.ValidateCapacitancesFP;
                                let dataS = dataMap.map((item) => {
                                    return {
                                        Sección: item.Seccion,
                                        Validación: item.Columnas,
                                        Resultado: item.Valor

                                    }
                                })


                                $("#grid2").kendoGrid({
                                    dataSource: dataS,
                                    pageable: {
                                        input: false,
                                        numeric: true,
                                        butonCount: 5,
                                        pageSize: 10,
                                        alwaysVisible: true,
                                        previousNext: true
                                    },
                                    columns: [
                                        { field: "Sección", width: 40 },
                                        { field: "Validación", width: 210 },
                                        { field: "Resultado", width: 50 }


                                    ]

                                });

                                break;
                            }
                            
                        }

                    if (Bandera) {
                        $("#ValorAcepCapEmple").text(valueAcepCapReCalcuar);
                        $("#ValorAcepFpEmple").text(valueAcepFpReCalcuar);
                        modalReCalcular.show();
                        $("#loader").css("display", "none");
                    }


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

        var drawing = kendo.spreadsheet.Drawing.fromJSON({
            topLeftCell: "A1",
            offsetX: 0,
            offsetY: 0,
            width: 220,
            height: 43,
            image: spreadsheet.addImage("/images/prolecge_excel.jpg")
        });


        sheet.addDrawing(drawing);
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
});