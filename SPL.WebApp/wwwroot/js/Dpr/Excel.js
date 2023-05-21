let viewModel;
let settingsToDisplayPRDReportsDTO;
let btnSave = document.getElementById("btnSave");
let btnValidate = document.getElementById("btnValidate");
let btnRefresh = document.getElementById("btnRefresh");
var baseTemplate;
let spreadsheetElement;
var calculado = false;
let panelExcel = document.getElementById("spreadsheet");
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
    postData(domain + "/Dpr/SavePDF/", viewModel)
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

    var valueTer = posTer.value() == null ? null : posTer.value();





    var resultAt = viewModel.Positions.AltaTension.find(x => x === posAT.value().toString())
    var resultBt = viewModel.Positions.BajaTension.find(x => x === posBT.value().toString())
    var resultTer = viewModel.Positions.Terciario.find(x => x === valueTer)




    if (resultAt === undefined) {
        if (posAT.value().toString() == "NOM") {

            resultAt = viewModel.Positions.AltaTension.find(x => x === viewModel.Positions.ATNom.toString())

            if (resultAt === undefined) {
                ShowFailedMessage("La posición de alta tensión no es válida")
                return
            }
           
        } else {
            ShowFailedMessage("La posición de alta tensión no es válida")
            return
        }

    }
    if (resultBt === undefined) {
        if (posBT.value().toString() == "NOM") {

            resultBt = viewModel.Positions.BajaTension.find(x => x === viewModel.Positions.BTNom.toString())

            if (resultBt === undefined) {
                ShowFailedMessage("La posición de baja tensión no es válida")
                return
            }

        } else {
            ShowFailedMessage("La posición de baja tensión no es válida")
            return
        }



    }

    if (valueTer !== null) {
        if (resultTer === undefined) {


            if (valueTer == "NOM") {

                resultTer = viewModel.Positions.Terciario.find(x => x === viewModel.Positions.TerNom.toString())

                if (resultTer === undefined) {
                    ShowFailedMessage("La posición terciario no es válida")
                    return;
                }

            } else {
                ShowFailedMessage("La posición terciario no es válida")
                return;
            }

        }
    }
    
    if (viewModel.ColumnsConfi == 6) {
        viewModel.Notes = (sheet.range('D49').value() !== null ? sheet.range('D49').value() : "") + " " + (sheet.range('D50').value() !== null ? sheet.range('D50').value() : "");
        viewModel.VoltageTest = sheet.range('D8').value();
    }
    else {
        viewModel.Notes = (sheet.range('E49').value() !== null ? sheet.range('E49').value() : "") + " " + (sheet.range('E50').value() !== null ? sheet.range('E50').value() : "");
        viewModel.VoltageTest = sheet.range('F8').value();
    }
       

    if (viewModel.Notes !== null && viewModel.Notes !== undefined && viewModel.Notes !== "") {
        if (viewModel.Notes.length > 100)  {
            ShowFailedMessage("Las notas no pueden superar los 100 caracteres");
            return;
        }
    }


    if (viewModel.VoltageTest !== null && viewModel.VoltageTest !== undefined && viewModel.VoltageTest !== "") {
        if (viewModel.VoltageTest.length > 20) {
            ShowFailedMessage("La tensión de prueba no puede excederse de 20 caracteres");
            return;
        }
    } else {

        ShowFailedMessage("La tensión de prueba es requerida");
        return;
    }


    
    $("#loader").css("display", "block");
    LoadWorkbook();
    var data = viewModel;
    calculado = true;
    postData(domain + "/Dpr/ValidateData/", data)
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
                    ShowFailedMessage(data.response.Description);
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
    sheet.range("N10").enable(true);




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