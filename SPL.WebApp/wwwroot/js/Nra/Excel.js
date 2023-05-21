let viewModel;
let settingsToDisplayPRDReportsDTO;
let btnSave = document.getElementById("btnSave");
let btnValidate = document.getElementById("btnValidate");
let btnRefresh = document.getElementById("btnRefresh");
var baseTemplate;
let spreadsheetElement;
var calculado = false;
let panelExcel = document.getElementById("spreadsheet");


let maxFile = 5;
let configuraciones = [];
let idModule = 5
let errorFiles = false
let contador = 0
let error = false

function TodoBien(view) {
    viewModel = view;
    baseTemplate = viewModel.Workbook;
    LoadTemplate(viewModel.Workbook);
    initCss();
    btnRefresh.disabled = false;
    btnSave.disabled = false;
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

btnValidate.addEventListener("click", async function () {
    $("#loader").css("display", "block");
    LoadWorkbook();
    var data = viewModel;


    postData(domain + "/Nra/ValidateData/", data)
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
                btnRefresh.disabled = false;
                btnSave.disabled = false;
                btnValidate.disabled = false;
                initCss();
                if (data.response.Description !== '') {
                    ShowFailedMessage(data.response.Description);
                } else {
                    ShowSuccessMessage("Validación Exitosa. Por favor corrobore los calculos.")
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

btnSave.addEventListener("click", async function () {

    var spreadsheet = $("#spreadsheet").data("kendoSpreadsheet");
    viewModel.Notas = (spreadsheet.activeSheet().range("C61:J61").value() === null || spreadsheet.activeSheet().range("C61:J61").value() === "" ?
        "" : spreadsheet.activeSheet().range("C61:J61").value()) + " " + (spreadsheet.activeSheet().range("C62:J62").value() === null || spreadsheet.activeSheet().range("C62:J62").value() === "" ?
            "" : spreadsheet.activeSheet().range("C62:J62").value())
    viewModel.Notas = viewModel.Notas.trim()
    LoadWorkbook()
    $("#loader").css("display", "block");
    postData(domain + "/Nra/SavePDF/", viewModel)
        .then(data => {
            if (data.response.Code !== -1) {
                ShowSuccessMessage("Guardado Exitoso.")
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

    var range = spreadsheet.activeSheet().range("A1:J200");
    range.enable(false);
    initCss();


    let incremental = 13
    let seccion = 1;
    let i = 0;
    let flag = false;

    spreadsheet.activeSheet().range("F9").color("black");
    spreadsheet.activeSheet().range("J9").color("black");
    spreadsheet.activeSheet().range("H9").color("black")
    spreadsheet.activeSheet().range("M8").color("black");
    spreadsheet.activeSheet().range("M9").color("black");
    spreadsheet.activeSheet().range("M10").color("black")
    spreadsheet.activeSheet().range("M11").color("black")
    spreadsheet.activeSheet().range("E10").color("black")


    if (viewModel.Pruebas === "RUI") {



        spreadsheet.activeSheet().range("B22:N35").textAlign("center")
        spreadsheet.activeSheet().range("L16").textAlign("center")
        spreadsheet.activeSheet().range("B22:N35").bold(false)

        spreadsheet.activeSheet().range("B21:O61").color("black");

        if (viewModel.CantidadColumnas === 4) {
            spreadsheet.activeSheet().range("D13:D16").color("black");

            spreadsheet.activeSheet().range("H13:H16").color("black");

            spreadsheet.activeSheet().range("B22:N56").fontSize(8);

            if (viewModel.CantMediciones <= 36) {
                sheet.range("F23:G23").borderBottom({ size: 0, color: "white" });
                sheet.range("F23:G23").borderRight({ size: 0, color: "white" });
                sheet.range("F23:G23").borderLeft({ size: 0, color: "white" });

                sheet.range("E23").borderBottom({ size: 0, color: "white" });

                sheet.range("M23:N23").borderBottom({ size: 0, color: "white" });
                sheet.range("M23:N23").borderRight({ size: 0, color: "white" });
                sheet.range("M23:N23").borderLeft({ size: 0, color: "white" });

                sheet.range("L23").borderBottom({ size: 0, color: "white" });
            }

        } else {

            spreadsheet.activeSheet().range("B27:N56").fontSize(8)

            if (viewModel.CantMediciones <= 31) {
                sheet.range("F28:G28").borderBottom({ size: 0, color: "white" });
                sheet.range("F28:G28").borderRight({ size: 0, color: "white" });
                sheet.range("F28:G28").borderLeft({ size: 0, color: "white" });

                sheet.range("E28").borderBottom({ size: 0, color: "white" });

                sheet.range("M28:N28").borderBottom({ size: 0, color: "white" });
                sheet.range("M28:N28").borderRight({ size: 0, color: "white" });
                sheet.range("M28:N28").borderLeft({ size: 0, color: "white" });

                sheet.range("L28").borderBottom({ size: 0, color: "white" });
            }
        }


        sheet.range("B10").value(viewModel.Alimentacion)
        if (viewModel.Resultado !== "" || viewModel.Resultado !== null) {
            spreadsheet.activeSheet().range("N61").value(viewModel.Resultado)
        }

        spreadsheet.activeSheet().range("G58").value(viewModel.SettingsNRA.Warranty)
        if (!viewModel.EsCargaData) {
            spreadsheet.activeSheet().range("L12").enable(true)
            spreadsheet.activeSheet().range("L14").enable(true)
        }



        spreadsheet.activeSheet().range("M8:N8").enable(true)
        spreadsheet.activeSheet().range("M9:N9").enable(true)

        sheet.range("M8").validation({
            comparerType: "custom",
            dataType: "custom",
            from: 'AND(REGEXP_MATCH(M8, "^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,1})?%?$"))',
            titleTemplate: "Error",
            showButton: true,
            type: "reject",
            allowNulls: false,
            messageTemplate: "La altura debe ser numerico considerando 3 enteros con 1 decimal."
        });
        sheet.range("M8:N8").enable(true)

        sheet.range("M9").validation({
            comparerType: "custom",
            dataType: "custom",
            from: 'AND(REGEXP_MATCH(M9, "^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,1})?%?$"))',
            titleTemplate: "Error",
            showButton: true,
            type: "reject",
            allowNulls: false,
            messageTemplate: "El perimetro debe ser numerico considerando 3 enteros con 1 decimal."
        });
        sheet.range("M9:N9").enable(true)

        sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosAT').Celda).validation({
            comparerType: "custom",
            dataType: "custom",
            from: 'AND(REGEXP_MATCH(' + viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosAT').Celda +
                ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosAT').Celda +
                ')<=5)',
            titleTemplate: "Error",
            showButton: true,
            type: "reject",
            allowNulls: false,
            messageTemplate: "La posición en AT solo puede contener letras y/o números y no puede excederse de 5 caracteres."
        });
        sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosAT').Celda).enable(true)
        sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosBT').Celda).validation({
            comparerType: "custom",
            dataType: "custom",
            from: 'AND(REGEXP_MATCH(' + viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosBT').Celda +
                ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosBT').Celda +
                ')<=5)', titleTemplate: "Error",
            showButton: true,
            type: "reject",
            allowNulls: false,
            messageTemplate: "La posición en BT solo puede contener letras y/o números y no puede excederse de 5 caracteres."
        });
        sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosBT').Celda).enable(true)


        if (viewModel.Positions.TerNom !== null && viewModel.Positions.TerNom !== '') {
            sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosTer').Celda).validation({
                comparerType: "custom",
                dataType: "custom",
                from: 'AND(REGEXP_MATCH(' + viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosTer').Celda +
                    ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosTer').Celda +
                    ')<=5)', titleTemplate: "Error",
                showButton: true,
                type: "reject",
                allowNulls: false,
                messageTemplate: "La posición en Ter solo puede contener letras y/o números y no puede excederse de 5 caracteres."
            });

            sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosTer').Celda).enable(true)
        }



        if (!viewModel.EsCargaData) {
            let incrementalSeccion1 = viewModel.CantidadColumnas == 10 ? 27 : 22;
            let maximaMediciones = viewModel.CantidadColumnas == 10 ? 60 : 70;
            let breakSesiones = viewModel.CantidadColumnas == 10 ? 30 : 35;

            for (let i = 1; i <= viewModel.CantidadColumnas; i++) {
                console.log(i)
                sheet.range("D" + incremental).validation({
                    comparerType: "custom",
                    dataType: "custom",
                    from: 'AND(REGEXP_MATCH(D' + incremental +
                        ', "^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,5})?%?$"),LEN(D' + incremental +
                        ')<=5,D' + incremental + ')', titleTemplate: "Error",
                    showButton: true,
                    type: "reject",
                    allowNulls: false,
                    messageTemplate: "Las posiciones DBA tienen que ser mayor a 0"
                });

                sheet.range("D" + incremental).enable(true)

                sheet.range("H" + incremental).validation({
                    comparerType: "custom",
                    dataType: "custom",
                    from: 'AND(REGEXP_MATCH(H' + incremental +
                        ', "^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,5})?%?$"),LEN(H' + incremental +
                        ')<=5,H' + incremental + ')', titleTemplate: "Error",
                    showButton: true,
                    type: "reject",
                    allowNulls: false,
                    messageTemplate: "Las posiciones DbaCorr tienen que ser mayor a 0"
                });

                sheet.range("H" + incremental).enable(true)
                incremental++;
            }

            if (viewModel.Altura === "1/3" || viewModel.Altura === "2/3") {


                $.each(viewModel.SettingsNRA.MatrixHeight13.slice(0, viewModel.CantMediciones), function (i, val) {
                    if (seccion <= breakSesiones) {
                        sheet.range("C" + incrementalSeccion1).validation({
                            comparerType: "custom",
                            dataType: "custom",
                            from: 'AND(REGEXP_MATCH(C' + incrementalSeccion1 +
                                ', "^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,5})?%?$"),LEN(C' + incrementalSeccion1 +
                                ')<=5,C' + incrementalSeccion1 + ')', titleTemplate: "Error",
                            showButton: true,
                            type: "reject",
                            allowNulls: false,
                            messageTemplate: "Los DBA tienen que ser mayor a 0"
                        });
                        sheet.range("C" + incrementalSeccion1).enable(true)

                        /*sheet.range("D" + incrementalSeccion1).validation({
                            comparerType: "custom",
                            dataType: "custom",
                            from: 'AND(REGEXP_MATCH(D' + incrementalSeccion1 +
                                ', "^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,5})?%?$"),LEN(D' + incrementalSeccion1 +
                                ')<=5,D' + incrementalSeccion1 + ')', titleTemplate: "Error",
                            showButton: true,
                            type: "reject",
                            allowNulls: false,
                            messageTemplate: "Los DbaCorr tienen que ser mayor a 0"
                        });
                        sheet.range("D" + incrementalSeccion1).enable(true)*/
                    }

                    if (seccion > breakSesiones) {
                        if (!flag) {
                            flag = true;
                            incrementalSeccion1 = viewModel.CantidadColumnas == 10 ? 27 : 22;
                        }
                        sheet.range("F" + incrementalSeccion1).validation({
                            comparerType: "custom",
                            dataType: "custom",
                            from: 'AND(REGEXP_MATCH(F' + incrementalSeccion1 +
                                ', "^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,5})?%?$"),LEN(F' + incrementalSeccion1 +
                                ')<=5,F' + incrementalSeccion1 + ')', titleTemplate: "Error",
                            showButton: true,
                            type: "reject",
                            allowNulls: false,
                            messageTemplate: "Los Dba tienen que ser mayor a 0"
                        });
                        sheet.range("F" + incrementalSeccion1).enable(true)

                        /*  sheet.range("G" + incrementalSeccion1).validation({
                              comparerType: "custom",
                              dataType: "custom",
                              from: 'AND(REGEXP_MATCH(G' + incrementalSeccion1 +
                                  ', "^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,5})?%?$"),LEN(G' + incrementalSeccion1 +
                                  ')<=5,G' + incrementalSeccion1 + ')', titleTemplate: "Error",
                              showButton: true,
                              type: "reject",
                              allowNulls: false,
                              messageTemplate: "Los DbaCorr tienen que ser mayor a 0"
                          });
                          sheet.range("G" + incrementalSeccion1).enable(true)*/


                    }

                    if (seccion === viewModel.CantMediciones) {
                        return;
                    }

                    if (seccion > maximaMediciones) {
                        return;
                    }

                    seccion++;
                    incrementalSeccion1++;
                });


                seccion = 1;
                flag = false;
                incrementalSeccion1 = viewModel.CantidadColumnas == 10 ? 27 : 22;

                $.each(viewModel.SettingsNRA.MatrixHeight23.slice(0, viewModel.CantMediciones), function (i, val) {

                    if (seccion <= breakSesiones) {
                        sheet.range("J" + incrementalSeccion1).validation({
                            comparerType: "custom",
                            dataType: "custom",
                            from: 'AND(REGEXP_MATCH(J' + incrementalSeccion1 +
                                ', "^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,5})?%?$"),LEN(J' + incrementalSeccion1 +
                                ')<=5,J' + incrementalSeccion1 + ')', titleTemplate: "Error",
                            showButton: true,
                            type: "reject",
                            allowNulls: false,
                            messageTemplate: "Los Dba tienen que ser mayor a 0"
                        });
                        sheet.range("J" + incrementalSeccion1).enable(true)

                        /*sheet.range("K" + incrementalSeccion1).validation({
                            comparerType: "custom",
                            dataType: "custom",
                            from: 'AND(REGEXP_MATCH(K' + incrementalSeccion1 +
                                ', "^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,5})?%?$"),LEN(K' + incrementalSeccion1 +
                                ')<=5,K' + incrementalSeccion1 + ')', titleTemplate: "Error",
                            showButton: true,
                            type: "reject",
                            allowNulls: false,
                            messageTemplate: "Los DbaCorr tienen que ser mayor a 0"
                        });
                        sheet.range("K" + incrementalSeccion1).enable(true)*/
                    }

                    if (seccion > breakSesiones) {
                        if (!flag) {
                            flag = true;
                            incrementalSeccion1 = viewModel.CantidadColumnas == 10 ? 27 : 22;
                        }
                        sheet.range("M" + incrementalSeccion1).validation({
                            comparerType: "custom",
                            dataType: "custom",
                            from: 'AND(REGEXP_MATCH(M' + incrementalSeccion1 +
                                ', "^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,5})?%?$"),LEN(M' + incrementalSeccion1 +
                                ')<=5,M' + incrementalSeccion1 + ')', titleTemplate: "Error",
                            showButton: true,
                            type: "reject",
                            allowNulls: false,
                            messageTemplate: "Los Dba tienen que ser mayor a 0"
                        });
                        sheet.range("M" + incrementalSeccion1).enable(true)

                        /*sheet.range("N" + incrementalSeccion1).validation({
                            comparerType: "custom",
                            dataType: "custom",
                            from: 'AND(REGEXP_MATCH(N' + incrementalSeccion1 +
                                ', "^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,5})?%?$"),LEN(N' + incrementalSeccion1 +
                                ')<=5,N' + incrementalSeccion1 + ')', titleTemplate: "Error",
                            showButton: true,
                            type: "reject",
                            allowNulls: false,
                            messageTemplate: "Los DbaCorr tienen que ser mayor a 0"
                        });
                        sheet.range("N" + incrementalSeccion1).enable(true)*/


                    }

                    if (seccion === viewModel.CantMediciones) {
                        return;
                    }

                    if (seccion > maximaMediciones) {
                        return;
                    }

                    seccion++;
                    incrementalSeccion1++;
                });
            }
            else {
                $.each(viewModel.SettingsNRA.MatrixHeight12.slice(0, viewModel.CantMediciones), function (i, val) {
                    if (seccion <= breakSesiones) {
                        sheet.range("C" + incrementalSeccion1).validation({
                            comparerType: "custom",
                            dataType: "custom",
                            from: 'AND(REGEXP_MATCH(C' + incrementalSeccion1 +
                                ', "^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,5})?%?$"),LEN(C' + incrementalSeccion1 +
                                ')<=5,C' + incrementalSeccion1 + ')', titleTemplate: "Error",
                            showButton: true,
                            type: "reject",
                            allowNulls: false,
                            messageTemplate: "Los Dba tienen que ser mayor a 0"
                        });
                        sheet.range("C" + incrementalSeccion1).enable(true)

                        /* sheet.range("D" + incrementalSeccion1).validation({
                             comparerType: "custom",
                             dataType: "custom",
                             from: 'AND(REGEXP_MATCH(D' + incrementalSeccion1 +
                                 ', "^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,5})?%?$"),LEN(D' + incrementalSeccion1 +
                                 ')<=5,D' + incrementalSeccion1 + ')', titleTemplate: "Error",
                             showButton: true,
                             type: "reject",
                             allowNulls: false,
                             messageTemplate: "Los DbaCorr tienen que ser mayor a 0"
                         });
                         sheet.range("D" + incrementalSeccion1).enable(true)*/
                    }

                    if (seccion > breakSesiones) {
                        if (!flag) {
                            flag = true;
                            incrementalSeccion1 = viewModel.CantidadColumnas == 10 ? 27 : 22;
                        }
                        sheet.range("F" + incrementalSeccion1).validation({
                            comparerType: "custom",
                            dataType: "custom",
                            from: 'AND(REGEXP_MATCH(F' + incrementalSeccion1 +
                                ', "^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,5})?%?$"),LEN(F' + incrementalSeccion1 +
                                ')<=5,F' + incrementalSeccion1 + ')', titleTemplate: "Error",
                            showButton: true,
                            type: "reject",
                            allowNulls: false,
                            messageTemplate: "Los Dba tienen que ser mayor a 0"
                        });
                        sheet.range("F" + incrementalSeccion1).enable(true)

                        /* sheet.range("G" + incrementalSeccion1).validation({
                             comparerType: "custom",
                             dataType: "custom",
                             from: 'AND(REGEXP_MATCH(G' + incrementalSeccion1 +
                                 ', "^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,5})?%?$"),LEN(G' + incrementalSeccion1 +
                                 ')<=5,G' + incrementalSeccion1 + ')', titleTemplate: "Error",
                             showButton: true,
                             type: "reject",
                             allowNulls: false,
                             messageTemplate: "Los NOM   DbaCorr tienen que ser mayor a 0"
                         });
                         sheet.range("G" + incrementalSeccion1).enable(true)*/


                    }

                    if (seccion === viewModel.CantMediciones) {
                        return;
                    }

                    if (seccion > maximaMediciones) {
                        return;
                    }

                    seccion++;
                    incrementalSeccion1++;
                });
            }


        }
        else {
            $.each(viewModel.SettingsNRA.matrixThreeAnt.slice(0, viewModel.CantidadColumnas), function (i, val) {

                sheet.range("D" + incremental).value(val.Dba.toFixed(1))
                incremental++;
            });

            incremental = 13

            $.each(viewModel.SettingsNRA.matrixThreeDes.slice(0, viewModel.CantidadColumnas), function (i, val) {
                sheet.range("H" + incremental).value(val.Dba.toFixed(1))
                incremental++;
            });
        }

    }

    if (viewModel.Pruebas === "RUI" && viewModel.Altura === "1/2") {
        if (viewModel.CantidadColumnas === 4) {

            let valor = spreadsheet.activeSheet().range("B19").value();
            valor = valor.replace("1/3", "1/2")
            spreadsheet.activeSheet().range("B19").value(valor);

            spreadsheet.activeSheet().range("I19").value('');
            spreadsheet.activeSheet().range("I19:N23").borderLeft({ size: 2, color: "white" });
            spreadsheet.activeSheet().range("I19:N23").borderRight({ size: 2, color: "white" });
            spreadsheet.activeSheet().range("I19:N23").borderBottom({ size: 2, color: "white" });
            spreadsheet.activeSheet().range("I19:N23").borderTop({ size: 2, color: "white" });

            spreadsheet.activeSheet().range("I21").value('');
            spreadsheet.activeSheet().range("I20").value('');
            spreadsheet.activeSheet().range("J20").value('');
            spreadsheet.activeSheet().range("K20").value('');
            spreadsheet.activeSheet().range("L20").value('');
            spreadsheet.activeSheet().range("M20").value('');
            spreadsheet.activeSheet().range("N20").value('');

            spreadsheet.activeSheet().range("J21").value('');
            spreadsheet.activeSheet().range("K21").value('');
            spreadsheet.activeSheet().range("L21").value('');
            spreadsheet.activeSheet().range("M21").value('');
            spreadsheet.activeSheet().range("N21").value('');

        } else {
            let valor = spreadsheet.activeSheet().range("B24").value();
            valor = valor.replace("1/3", "1/2")
            spreadsheet.activeSheet().range("B24").value(valor);

            spreadsheet.activeSheet().range("I24:N28").borderLeft({ size: 2, color: "white" });
            spreadsheet.activeSheet().range("I24:N28").borderRight({ size: 2, color: "white" });
            spreadsheet.activeSheet().range("I24:N28").borderBottom({ size: 2, color: "white" });
            spreadsheet.activeSheet().range("I24:N28").borderTop({ size: 2, color: "white" });

            spreadsheet.activeSheet().range("I24").value('');
            spreadsheet.activeSheet().range("I26").value('');
            spreadsheet.activeSheet().range("J26").value('');
            spreadsheet.activeSheet().range("K26").value('');
            spreadsheet.activeSheet().range("L26").value('');
            spreadsheet.activeSheet().range("M26").value('');
            spreadsheet.activeSheet().range("N26").value('');

            spreadsheet.activeSheet().range("J25").value('');
            spreadsheet.activeSheet().range("K25").value('');
            spreadsheet.activeSheet().range("L25").value('');
            spreadsheet.activeSheet().range("M25").value('');
            spreadsheet.activeSheet().range("N25").value('');
        }


    }

    if (viewModel.Pruebas === "OCT") {
        spreadsheet.activeSheet().range("F79").color("black");
        spreadsheet.activeSheet().range("J79").color("black");
        spreadsheet.activeSheet().range("H79").color("black")
        spreadsheet.activeSheet().range("M78").color("black");
        spreadsheet.activeSheet().range("M79").color("black");
        spreadsheet.activeSheet().range("M80").color("black")
        spreadsheet.activeSheet().range("M81").color("black")
        spreadsheet.activeSheet().range("E80").color("black")

        if (viewModel.CantidadColumnas === 4) {

            spreadsheet.activeSheet().range("D13:N21").textAlign("center")
            spreadsheet.activeSheet().range("D13:N21").color("black")

            spreadsheet.activeSheet().range("B24:N59").textAlign("center")
            spreadsheet.activeSheet().range("B24:N59").color("black")
            spreadsheet.activeSheet().range("B24:N59").fontFamily("Arial")
            spreadsheet.activeSheet().range("B24:N59").fontSize(8)
            spreadsheet.activeSheet().range("B24:N59").bold(false)

            spreadsheet.activeSheet().range("B83:N126").textAlign("center")
            spreadsheet.activeSheet().range("B83:N126").color("black")
            spreadsheet.activeSheet().range("B83:N126").fontFamily("Arial")
            spreadsheet.activeSheet().range("B83:N126").fontSize(8)
            spreadsheet.activeSheet().range("B83:N126").bold(false)



        }

        sheet.range("C62:J62").enable(true)
        sheet.range("C61:J61").enable(true)
        let rowbreak = viewModel.CantidadColumnas == 4 ? 36 : 24;
        sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosAT').Celda).enable(true)
        sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosBT').Celda).enable(true)

        if (viewModel.Positions.TerNom !== null && viewModel.Positions.TerNom !== '') {
            /* sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosTer').Celda).validation({
                 comparerType: "custom",
                 dataType: "custom",
                 from: 'AND(REGEXP_MATCH(' + viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosTer').Celda +
                     ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosTer').Celda +
                     ')<=5)', titleTemplate: "Error",
                 showButton: true,
                 type: "reject",
                 allowNulls: false,
                 messageTemplate: "La posición en Ter solo puede contener letras y/o números y no puede excederse de 5 caracteres."
             });*/

            sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosTer').Celda).enable(true)
        }

        sheet.range("B10").value(viewModel.Alimentacion);

        if (viewModel.CantidadColumnas === 10) {
            let valor = viewModel.UnionMatrices.length;
            if (valor > rowbreak) {
                sheet.range("B80").value(viewModel.Alimentacion);
                sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 2).Celda).enable(true);
                sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 2).Celda).enable(true);
                if (viewModel.Positions.TerNom !== null && viewModel.Positions.TerNom !== '') {
                    sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 2).Celda).enable(true);
                }
            }
        } else {
            let valor = viewModel.UnionMatrices.length;
            if (valor > rowbreak) {
                sheet.range("B80").value(viewModel.Alimentacion);
                sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 2).Celda).enable(true);
                sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 2).Celda).enable(true);
                if (viewModel.Positions.TerNom !== null && viewModel.Positions.TerNom !== '') {
                    sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 2).Celda).enable(true);
                }
            }
        }

        if (viewModel.ActivarSegundoHeader) {
            sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'Resultado' && x.Seccion === 2).Celda).value(viewModel.Resultado);
            sheet.range("M78").validation({
                comparerType: "custom",
                dataType: "custom",
                from: 'AND(REGEXP_MATCH(M78, "^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,1})?%?$"))',
                titleTemplate: "Error",
                showButton: true,
                type: "reject",
                allowNulls: false,
                messageTemplate: "El perimetro debe ser numerico considerando 3 enteros con 1 decimal."
            });
            sheet.range("M78:N78").enable(true)

            sheet.range("M79").validation({
                comparerType: "custom",
                dataType: "custom",
                from: 'AND(REGEXP_MATCH(M79, "^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,1})?%?$"))',
                titleTemplate: "Error",
                showButton: true,
                type: "reject",
                allowNulls: false,
                messageTemplate: "La altura debe ser numerico considerando 3 enteros con 1 decimal."
            });
            sheet.range("M79:N79").enable(true)

            sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion == 2).Celda).enable(true)
            sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion == 2).Celda).enable(true)

            if (viewModel.Positions.TerNom !== null && viewModel.Positions.TerNom !== '') {
                /* sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosTer').Celda).validation({
                     comparerType: "custom",
                     dataType: "custom",
                     from: 'AND(REGEXP_MATCH(' + viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosTer').Celda +
                         ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosTer').Celda +
                         ')<=5)', titleTemplate: "Error",
                     showButton: true,
                     type: "reject",
                     allowNulls: false,
                     messageTemplate: "La posición en Ter solo puede contener letras y/o números y no puede excederse de 5 caracteres."
                 });*/

                sheet.range(viewModel.SettingsNRA.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion == 2).Celda).enable(true)
            }
        }

        sheet.range("D126:N126").enable(true)

        sheet.range("M8").validation({
            comparerType: "custom",
            dataType: "custom",
            from: 'AND(REGEXP_MATCH(M8, "^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,1})?%?$"))',
            titleTemplate: "Error",
            showButton: true,
            type: "reject",
            allowNulls: false,
            messageTemplate: "La altura debe ser numerico considerando 3 enteros con 1 decimal."
        });
        sheet.range("M8:N8").enable(true)
        sheet.range("M9").validation({
            comparerType: "custom",
            dataType: "custom",
            from: 'AND(REGEXP_MATCH(M9, "^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,1})?%?$"))',
            titleTemplate: "Error",
            showButton: true,
            type: "reject",
            allowNulls: false,
            messageTemplate: "El perimetro debe ser numerico considerando 3 enteros con 1 decimal."
        });
        sheet.range("M9:N9").enable(true)

        $.each(viewModel.SettingsNRA.UnionMatrices, function (i, val) {
            sheet.range("D" + incremental).value(val.Dba)
            sheet.range("E" + incremental).value(val.Decibels315)
            sheet.range("F" + incremental).value(val.Decibels63)
            sheet.range("G" + incremental).value(val.Decibels125)
            sheet.range("H" + incremental).value(val.Decibels250)
            sheet.range("I" + incremental).value(val.Decibels500)
            sheet.range("J" + incremental).value(val.Decibels1000)
            sheet.range("K" + incremental).value(val.Decibels2000)
            sheet.range("L" + incremental).value(val.Decibels4000)
            sheet.range("M" + incremental).value(val.Decibels8000)
            sheet.range("N" + incremental).value(val.Decibels10000)
        });

        if (viewModel.CantidadColumnas === 4) {
            if (viewModel.EsCargaData) {
                sheet.range("D21").value(viewModel.SettingsNRA.AmbProm.Dba.toFixed(0))
                sheet.range("E21").value(viewModel.SettingsNRA.AmbProm.Decibels315.toFixed(0))
                sheet.range("F21").value(viewModel.SettingsNRA.AmbProm.Decibels63.toFixed(0))
                sheet.range("G21").value(viewModel.SettingsNRA.AmbProm.Decibels125.toFixed(0))
                sheet.range("H21").value(viewModel.SettingsNRA.AmbProm.Decibels250.toFixed(0))
                sheet.range("I21").value(viewModel.SettingsNRA.AmbProm.Decibels500.toFixed(0))
                sheet.range("J21").value(viewModel.SettingsNRA.AmbProm.Decibels1000.toFixed(0))
                sheet.range("K21").value(viewModel.SettingsNRA.AmbProm.Decibels2000.toFixed(0))
                sheet.range("L21").value(viewModel.SettingsNRA.AmbProm.Decibels4000.toFixed(0))
                sheet.range("M21").value(viewModel.SettingsNRA.AmbProm.Decibels8000.toFixed(0))
                sheet.range("N21").value(viewModel.SettingsNRA.AmbProm.Decibels10000.toFixed(0))
            } else {
            }
        }
        else {
            if (viewModel.EsCargaData) {
                sheet.range("D33").value(viewModel.SettingsNRA.AmbProm.Dba.toFixed(0))
                sheet.range("E33").value(viewModel.SettingsNRA.AmbProm.Decibels315.toFixed(0))
                sheet.range("F33").value(viewModel.SettingsNRA.AmbProm.Decibels63.toFixed(0))
                sheet.range("G33").value(viewModel.SettingsNRA.AmbProm.Decibels125.toFixed(0))
                sheet.range("H33").value(viewModel.SettingsNRA.AmbProm.Decibels250.toFixed(0))
                sheet.range("I33").value(viewModel.SettingsNRA.AmbProm.Decibels500.toFixed(0))
                sheet.range("J33").value(viewModel.SettingsNRA.AmbProm.Decibels1000.toFixed(0))
                sheet.range("K33").value(viewModel.SettingsNRA.AmbProm.Decibels2000.toFixed(0))
                sheet.range("L33").value(viewModel.SettingsNRA.AmbProm.Decibels4000.toFixed(0))
                sheet.range("M33").value(viewModel.SettingsNRA.AmbProm.Decibels8000.toFixed(0))
                sheet.range("N33").value(viewModel.SettingsNRA.AmbProm.Decibels10000.toFixed(0))
            }
        }
        if (viewModel.EsCargaData) {
            sheet.range("D126").value(viewModel.SettingsNRA.AmbTrans.Dba.toFixed(0))
            sheet.range("E126").value(viewModel.SettingsNRA.AmbTrans.Decibels315.toFixed(0))
            sheet.range("F126").value(viewModel.SettingsNRA.AmbTrans.Decibels63.toFixed(0))
            sheet.range("G126").value(viewModel.SettingsNRA.AmbTrans.Decibels125.toFixed(0))
            sheet.range("H126").value(viewModel.SettingsNRA.AmbTrans.Decibels250.toFixed(0))
            sheet.range("I126").value(viewModel.SettingsNRA.AmbTrans.Decibels500.toFixed(0))
            sheet.range("J126").value(viewModel.SettingsNRA.AmbTrans.Decibels1000.toFixed(0))
            sheet.range("K126").value(viewModel.SettingsNRA.AmbTrans.Decibels2000.toFixed(0))
            sheet.range("L126").value(viewModel.SettingsNRA.AmbTrans.Decibels4000.toFixed(0))
            sheet.range("M126").value(viewModel.SettingsNRA.AmbTrans.Decibels8000.toFixed(0))
            sheet.range("N126").value(viewModel.SettingsNRA.AmbTrans.Decibels10000.toFixed(0))
        }
        else {
            sheet.range("D126").value()
            sheet.range("E126").value()
            sheet.range("F126").value()
            sheet.range("G126").value()
            sheet.range("H126").value()
            sheet.range("I126").value()
            sheet.range("J126").value()
            sheet.range("K126").value()
            sheet.range("L126").value()
            sheet.range("M126").value()
            sheet.range("N126").value()

            let valor = viewModel.SettingsNRA.matrixThreeAnt.slice(0, viewModel.CantidadColumnas).length
                + viewModel.SettingsNRA.matrixThreeDes.slice(0, viewModel.CantidadColumnas).length
            let p = 13
            for (let o = 0; o <= valor; o++) {

                sheet.range("D" + p).enable(true)
                sheet.range("E" + p).enable(true)
                sheet.range("F" + p).enable(true)
                sheet.range("G" + p).enable(true)
                sheet.range("H" + p).enable(true)
                sheet.range("I" + p).enable(true)
                sheet.range("J" + p).enable(true)
                sheet.range("K" + p).enable(true)
                sheet.range("L" + p).enable(true)
                sheet.range("M" + p).enable(true)
                sheet.range("N" + p).enable(true)
                p++;
            }

            let inicio1 = viewModel.CantidadColumnas === 4 ? 24 : 36;
            let inicio2 = 83
            let rowbreak = viewModel.CantidadColumnas == 4 ? 36 : 24;
            $.each(viewModel.UnionMatrices, function (i, val) {
                sheet.range("D" + inicio1).enable(true)
                sheet.range("E" + inicio1).enable(true)
                sheet.range("F" + inicio1).enable(true)
                sheet.range("G" + inicio1).enable(true)
                sheet.range("H" + inicio1).enable(true)
                sheet.range("I" + inicio1).enable(true)
                sheet.range("J" + inicio1).enable(true)
                sheet.range("K" + inicio1).enable(true)
                sheet.range("L" + inicio1).enable(true)
                sheet.range("M" + inicio1).enable(true)
                sheet.range("N" + inicio1).enable(true)

                i++;
                inicio1++;

                if (i >= rowbreak && !flag) {
                    i = 0;
                    flag = true;
                    inicio1 = inicio2;
                }
            });
        }
    }
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

    var path = path = "/Pir/GetConfigurationFiles/?pIdModule=" + idModule;
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