let viewModel;
let settingsToDisplayPRDReportsDTO;
let btnSave = document.getElementById("btnSave");
let btnValidate = document.getElementById("btnValidate");
let btnRefresh = document.getElementById("btnRefresh");
var baseTemplate;
let spreadsheetElement;
var calculado = false;
let panelExcel = document.getElementById("spreadsheet");
var pasarResult= false

let maxFile = 5;
let configuraciones =[];
let idModule = 6
let errorFiles = false
let contador = 0
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
    return new Promise(function(resolve, reject) {
        var reader = new FileReader();
        reader.onload = function() { resolve(reader.result);  };
        reader.onerror = reject;
        reader.readAsDataURL(elemento.rawFile);
    });
}

btnValidate.addEventListener("click", async function () {
    $("#loader").css("display", "block");
    await onClickSelect();

    if (configuraciones === null || configuraciones === undefined) {
        ShowFailedMessage("No se ha podido cargar la configuracion de archivos para ARF")
        $("#loader").css("display", "none");
    } else {
        $("#loader").css("display", "none");
        let modalReCalcular = new bootstrap.Modal($("#modalReCalcular"));
        modalReCalcular.show();
    }

  
});

btnSave.addEventListener("click",async function () {

   var files = $('#files').data('kendoUpload').getFiles()
    if(files.length > 0 ){
        $.each(files,function(){
            //console.log(this)
            if(this.error !== undefined){
               error =true
            }
        })
    }else{
        ShowFailedMessage("Debe seleccionar por lo menos un archivo")
        return
    }

    if(error){
        ShowFailedMessage("Se han encontrados errores los archivos.")
        return
    }
    

    LoadWorkbook()
    let cont = 0
    viewModel.Archivos = []

    console.log(viewModel)
    //await test().then(data => viewModel.Archivos.push(data))
    for(const elemento of files){
        var element = new Object();
        await test(elemento).then(data => {element.Base64 = data , element.Name = elemento.name})
        viewModel.Archivos.push(element)

    }
    //if (!pasarResult) {
    //    ShowFailedMessage("Favor de proporcionar el resultado de la prueba")
    //    return
    //}
    if (CheckPositions() ) {
        var spreadsheet = $("#spreadsheet").data("kendoSpreadsheet");
        var sheet = spreadsheet.activeSheet();
        

        viewModel.AT1 = sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda).value();
        viewModel.BT1 = sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda).value();
        viewModel.Ter1 = sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda).value();
        viewModel.Paginas = sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TotalPaginas').Celda).value();
        viewModel.TempAceite1 = sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 1).Celda).value();
        viewModel.Resultado = sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda).value();

        if (viewModel.Resultado == null || viewModel.Resultado == "" || viewModel.Resultado == undefined) {
            ShowFailedMessage("Favor de proporcionar el resultado de la prueba")
            return
        }


        if (viewModel.Pruebas === "LYP") {
            viewModel.AT2 = sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 2).Celda).value();
            viewModel.BT2 = sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 2).Celda).value();
            viewModel.Ter2 = sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 2).Celda).value();
            viewModel.TempAceite2 = sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 2).Celda).value();
        }



        $("#loader").css("display", "block");
        postData(domain + "/Arf/SavePDF/", viewModel)
            .then(data => {
                console.log(data.response)
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

    kendo.spreadsheet.defineFunction("REGEXP_MATCH", function(str, pattern, flags){
        var rx;
        try {
            rx = flags ? new RegExp(pattern, flags) : new RegExp(pattern);
        } catch(ex) {
            // could not compile regexp, return some error code
            return new kendo.spreadsheet.CalcError("REGEXP");
        }
        return rx.test(str);
    }).args([
        [ "str", "string" ],
        [ "pattern", "string" ],
        [ "flags", [ "or", "string", "null" ] ]
    ]);


    kendo.spreadsheet.defineFunction("CHECK_RESULT", function (str, lenguaje, flags) {
        var posi;
        try {
            if (lenguaje === 'EN') {
                if (str === "Accepted" || str === "Rejected") {
                    pasarResult = true
                    return true
                } else {
                    pasarResult = false
                    return false
                }
            } else {
                if (str === "Aceptado" || str === "Rechazado") {
                    pasarResult = true
                    return true
                } else {
                    pasarResult = false
                    return false
                }
            }

        } catch (ex) {
            // could not compile regexp, return some error code
            return ex;
        }
        //alert(rx.test(str))
    }).args([
        ["str", "string"],
        ["lenguaje", "string"],
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

    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Fecha').Celda).enable(true)
    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NoSerie').Celda).value(viewModel.NoSerie)
    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NivelTension').Celda).value(viewModel.VoltageLevel)

    if (viewModel.Pruebas === "LYP") {

        if (viewModel.TerciarioOSegunda === "CT" && viewModel.Columnas === 3) {
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).
                value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).value().replace('?', ''));

            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 2).Celda).
                value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 2).Celda).value().replace('?', ''));

        }

        if (viewModel.TerciarioOSegunda === "2B" && viewModel.Columnas === 3) {
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 1).Celda).
                value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).value());
                

            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).
                value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).value().replace('?', '1'));

            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 1).Celda).
                value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 1).Celda).value().replace('?', '2'));


            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 2).Celda).
                value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 2).Celda).value());


            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 2).Celda).
                value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 2).Celda).value().replace('?', '1'));

            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 2).Celda).
                value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 2).Celda).value().replace('?', '2'));
        }



        if (viewModel.TerciarioOSegunda !== "CT" && viewModel.TerciarioOSegunda !== "2B") {
            spreadsheet.activeSheet().range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTerciario' && x.Seccion === 2).Celda).value("");
            spreadsheet.activeSheet().range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTerciario' && x.Seccion === 1).Celda).value("");
        } else {

            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Terciario' && x.Seccion === 1).Celda).value(viewModel.TerciarioLab)
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Terciario' && x.Seccion === 2).Celda).value(viewModel.TerciarioEmp)
        }

        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NivelAceite' && x.Seccion === 1).Celda).value(viewModel.NivelAceiteLab)
        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NivelAceite' && x.Seccion === 2).Celda).value(viewModel.NivelAceiteEmp)

        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Boquillas' && x.Seccion === 1).Celda).value(viewModel.BoquillasLab)
        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Boquillas' && x.Seccion === 2).Celda).value(viewModel.BoquillasEmp)

        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NucleoHerrajes' && x.Seccion === 1).Celda).value(viewModel.NucleosLab)
        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NucleoHerrajes' && x.Seccion === 2).Celda).value(viewModel.NucleosEmp)

        if (viewModel.NivelAceiteLab === "Vacío" || viewModel.NivelAceiteLab === "Empty") {
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 1).Celda + ":P21").borderBottom(null);
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTempAceite' && x.Seccion === 1).Celda).value("");
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'UMTempAceite' && x.Seccion === 1).Celda).value("");
        }
        else {
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite').Celda).validation({
                comparerType: "custom",
                dataType: "custom",
                from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite').Celda + ', "^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,1})?%?$"))',
                titleTemplate: "Error",
                showButton: true,
                type: "reject",
                allowNulls: false,
                messageTemplate: "La temperatura del aceite debe ser mayor a cero considerando 3 enteros con 1 decimal."
            });
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite').Celda + ":P21").enable(true)
        }

        if (viewModel.NivelAceiteEmp === "Vacío" || viewModel.NivelAceiteEmp === "Empty") {
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 2).Celda + ":P35").borderBottom(null);
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTempAceite' && x.Seccion === 2).Celda).value("");
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'UMTempAceite' && x.Seccion === 2).Celda).value("");
        } else {
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 2).Celda).validation({
                comparerType: "custom",
                dataType: "custom",
                from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 2).Celda + ', "^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,1})?%?$"))',
                titleTemplate: "Error",
                showButton: true,
                type: "reject",
                allowNulls: false,
                messageTemplate: "La temperatura del aceite debe ser mayor a cero considerando 3 enteros con 1 decimal."
            });
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 2).Celda + ":P35").enable(true)
              
        }

    }
    else
    {
        if (viewModel.TerciarioOSegunda === "CT" && viewModel.Columnas === 3 ) {
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).
                value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).value().replace('?', ''));
        }

        if (viewModel.TerciarioOSegunda === "2B" && viewModel.Columnas === 3) {
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 1).Celda).
                value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).value());


            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).
                value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).value().replace('?', '1'));

            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 1).Celda).
                value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 1).Celda).value().replace('?', '2'));
        }

        if (viewModel.Pruebas === "LAB") {
            if (viewModel.TerciarioOSegunda !== "CT" && viewModel.TerciarioOSegunda !== "2B") {
                spreadsheet.activeSheet().range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTerciario' && x.Seccion === 1).Celda).value("");
            } else {
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Terciario' && x.Seccion === 1).Celda).value(viewModel.TerciarioLab)
            }

            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NivelAceite' && x.Seccion === 1).Celda).value(viewModel.NivelAceiteLab)
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Boquillas' && x.Seccion === 1).Celda).value(viewModel.BoquillasLab)
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NucleoHerrajes' && x.Seccion === 1).Celda).value(viewModel.NucleosLab)


            if (viewModel.NivelAceiteLab === "Vacío" || viewModel.NivelAceiteLab === "Empty") {
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 1).Celda + ":P21").borderBottom(null);
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTempAceite' && x.Seccion === 1).Celda).value("");
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'UMTempAceite' && x.Seccion === 1).Celda).value("");
            }
            else {
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite').Celda).validation({
                    comparerType: "custom",
                    dataType: "custom",
                    from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite').Celda + ', "^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,1})?%?$"))',
                    titleTemplate: "Error",
                    showButton: true,
                    type: "reject",
                    allowNulls: false,
                    messageTemplate: "La temperatura del aceite debe ser mayor a cero considerando 3 enteros con 1 decimal."
                });
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite').Celda + ":P21").enable(true)
            }
        }

        if (viewModel.Pruebas === "PLA") {
            if (viewModel.TerciarioOSegunda !== "CT" && viewModel.TerciarioOSegunda !== "2B") {
                spreadsheet.activeSheet().range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTerciario' && x.Seccion === 1).Celda).value("");
            } else {
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Terciario' && x.Seccion === 1).Celda).value(viewModel.TerciarioEmp)
            }

            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NivelAceite' && x.Seccion === 1).Celda).value(viewModel.NivelAceiteEmp)
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Boquillas' && x.Seccion === 1).Celda).value(viewModel.BoquillasEmp)
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NucleoHerrajes' && x.Seccion === 1).Celda).value(viewModel.NucleosEmp)

            if (viewModel.NivelAceiteEmp === "Vacío" || viewModel.NivelAceiteEmp === "Empty") {
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 1).Celda + ":P21").borderBottom(null);
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTempAceite' && x.Seccion === 1).Celda).value("");
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'UMTempAceite' && x.Seccion === 1).Celda).value("");
            } else {
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 1).Celda).validation({
                    comparerType: "custom",
                    dataType: "custom",
                    from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 1).Celda + ', "^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,1})?%?$"))',
                    titleTemplate: "Error",
                    showButton: true,
                    type: "reject",
                    allowNulls: false,
                    messageTemplate: "La temperatura del aceite debe ser mayor a cero considerando 3 enteros con 1 decimal."
                });
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 1).Celda + ":P21").enable(true)

            }
        }
      
    }


    if (viewModel.Pruebas === "LYP") {
        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda).validation({
            comparerType: "custom",
            dataType: "custom",
            from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda +
                ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda +
                ')<=5)',
            titleTemplate: "Error",
            showButton: true,
            type: "reject",
            allowNulls: false,
            messageTemplate: "La posición en AT solo puede contener letras y/o números y no puede excederse de 5 caracteres."
        });

        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 2).Celda).validation({
            comparerType: "custom",
            dataType: "custom",
            from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 2).Celda +
                ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 2).Celda +
                ')<=5)',
            titleTemplate: "Error",
            showButton: true,
            type: "reject",
            allowNulls: false,
            messageTemplate: "La posición en AT solo puede contener letras y/o números y no puede excederse de 5 caracteres."
        });

 

        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda).validation({
            comparerType: "custom",
            dataType: "custom",
            from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda +
                ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda +
                ')<=5)',
            titleTemplate: "Error",
            showButton: true,
            type: "reject",
            allowNulls: false,
            messageTemplate: "La posición en BT solo puede contener letras y/o números y no puede excederse de 5 caracteres."
        });
        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 2).Celda).validation({
            comparerType: "custom",
            dataType: "custom",
            from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 2).Celda +
                ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 2).Celda +
                ')<=5)',
            titleTemplate: "Error",
            showButton: true,
            type: "reject",
            allowNulls: false,
            messageTemplate: "La posición en BT solo puede contener letras y/o números y no puede excederse de 5 caracteres."
        });

        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda).enable(true)
        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 2).Celda).enable(true)
        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda).enable(true)
        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 2).Celda).enable(true)


        if (viewModel.Columnas === 3) {
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda).validation({
                comparerType: "custom",
                dataType: "custom",
                from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda +
                    ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda +
                    ')<=5)',
                titleTemplate: "Error",
                showButton: true,
                type: "reject",
                allowNulls: false,
                messageTemplate: "La posición en Ter solo puede contener letras y/o números y no puede excederse de 5 caracteres."
            });
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 2).Celda).validation({
                comparerType: "custom",
                dataType: "custom",
                from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 2).Celda +
                    ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 2).Celda +
                    ')<=5)',
                titleTemplate: "Error",
                showButton: true,
                type: "reject",
                allowNulls: false,
                messageTemplate: "La posición en Ter solo puede contener letras y/o números y no puede excederse de 5 caracteres."
            });
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda).enable(true)
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 2).Celda).enable(true)
            if (viewModel.TerciarioDisponible === 'No') {
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda).clear();
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 1).Celda).clear();
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Terciario' && x.Seccion === 1).Celda).clear();
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTerciario' && x.Seccion === 1).Celda).clear();

                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 2).Celda).clear();
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 2).Celda).clear();
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Terciario' && x.Seccion === 2).Celda).clear();
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTerciario' && x.Seccion === 2).Celda).clear();
            }
        }


    }
    else {

        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda).validation({
            comparerType: "custom",
            dataType: "custom",
            from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda +
                ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda +
                ')<=5)',
            titleTemplate: "Error",
            showButton: true,
            type: "reject",
            allowNulls: false,
            messageTemplate: "La posición en AT solo puede contener letras y/o números y no puede excederse de 5 caracteres."
        });
        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda).validation({
            comparerType: "custom",
            dataType: "custom",
            from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda +
                ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda +
                ')<=5)',
            titleTemplate: "Error",
            showButton: true,
            type: "reject",
            allowNulls: false,
            messageTemplate: "La posición en BT solo puede contener letras y/o números y no puede excederse de 5 caracteres."
        });
        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda).enable(true)
        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda).enable(true)


        if (viewModel.Columnas === 3) {
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda).validation({
                comparerType: "custom",
                dataType: "custom",
                from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda +
                    ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda +
                    ')<=5)',
                titleTemplate: "Error",
                showButton: true,
                type: "reject",
                allowNulls: false,
                messageTemplate: "La posición en Ter solo puede contener letras y/o números y no puede excederse de 5 caracteres."
            });
            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda).enable(true)

            if (viewModel.TerciarioDisponible === 'No') {
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda).clear();
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 1).Celda).clear();
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Terciario' && x.Seccion === 1).Celda).clear();
                sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTerciario' && x.Seccion === 1).Celda).clear();
            }
        }

    }



    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TotalPaginas').Celda).validation({
        comparerType: "between",
        dataType: "number",
        from: '1',
        to : '999',
        titleTemplate: "Error",
        showButton: true,
        type: "reject",
        allowNulls: false,
        messageTemplate: "El total de páginas debe ser mayor a cero considerando 3 enteros sin decimales"
    });
    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TotalPaginas').Celda).enable(true)


    //sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado' && x.Seccion === 1).Celda).validation({
    //    comparerType: "custom",
    //    dataType: "custom",
    //    from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"'+viewModel.ClaveIdioma+'"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda +')<=15)',
    //    titleTemplate: "Error",
    //    showButton: true,
    //    type: "reject",Favor de proporcionar el resultado de la prueba
    //    allowNulls: false,
    //    messageTemplate: viewModel.ClaveIdioma === "EN" ? "Su valor solo puede ser Accepted o Rejected" : "Su valor solo puede ser Aceptado o Rechazado"
    //});
    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado' && x.Seccion === 1).Celda).validation({
        comparerType: "list",
        dataType: "list",
        from: viewModel.ClaveIdioma == 'ES' ? '"Aceptado,Rechazado"' : '"Accepted,Rejected"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });
    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ":S46").value(viewModel.ClaveIdioma == 'ES' ? 'Seleccione...' : 'Select...')
    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda+ ":S46").enable(true)



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
    if (totalFiles.length + 1 > 5 ) {

        ShowFailedMessage("Solo se permiten 5 archivos");
        e.preventDefault();
    }


    console.log(configuraciones)
    var files = e.files;

    for (var i = 0; i < files.length; i += 1) {
        var file = files[i];

        var result =  configuraciones.filter(x=>x.Extension.toUpperCase() == file.extension.toUpperCase())

        if(result.length === 0){
            file.error ="La extension "+file.extension + " no ha sido encontrada en la configuracion"
        }else if(result.length > 0){
            var pesoMax = result[0].PesoMaximo ;
            console.log(file.size)
            console.log(pesoMax )
            if(pesoMax * 1024  * 1024 < file.size){
                file.error ="El peso del archivo supera los "+result[0].PesoMaximo+"MB"
            }else{
                file.success ="Archivo agregado"
                setTimeout(function(){
                 $(".k-file-success div.uno span#wrapper-message").css("margin-left","35px")
                })
            }
        }
    }
}

async function onClickSelect(){
    await GetConfigurationFilesJSON().then(
        data => {
            if(data.response.Code === 1){
                configuraciones = data.response.Structure;
                console.log(configuraciones)
            }else{
                ShowFailedMessage("Error al obtener la confiuracion del archivo")
            }
        }
    )
}

async function GetConfigurationFilesJSON() {

    var path = path = "/Arf/GetConfigurationFiles/?pIdModule="+idModule;
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

function CheckPositions() {
    var spreadsheet = $("#spreadsheet").data("kendoSpreadsheet");
    var sheet = spreadsheet.activeSheet();
    var at1
    var at2
    var bt1
    var bt2
    var ter1
    var ter2
    var seguir

    if (viewModel.Pruebas === "LYP") {

        at1 = viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda;
        bt1 = viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda;
        at2 = viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 2).Celda;
        bt2 = viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 2).Celda;
        ter1 = viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda;
        ter2 = viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 2).Celda;

        if (viewModel.Positions.AltaTension.find(x => x === sheet.range(at1).value()?.toString()) === undefined) {
            ShowFailedMessage("La posicion en alta para la primera seccion no es valida")
            return false
        }

        if (viewModel.Positions.AltaTension.find(x => x === sheet.range(at2).value()?.toString()) === undefined) {
            ShowFailedMessage("La posicion en alta para la segunda seccion no es valida")
            return false
        }

        if (viewModel.Positions.BajaTension.find(x => x === sheet.range(bt1).value()?.toString()) === undefined) {
            ShowFailedMessage("La posicion en baja para la primera seccion no es valida")
            return false
        }

        if (viewModel.Positions.BajaTension.find(x => x === sheet.range(bt2).value()?.toString()) === undefined) {
            ShowFailedMessage("La posicion en baja para la segunda seccion no es valida")
            return false
        }





        if (viewModel.Columnas === 3 ) {
            if (viewModel.TerciarioOSegunda !== "2B") {
                if (viewModel.Positions.Terciario.find(x => x === sheet.range(ter1).value()?.toString()) === undefined) {
                    ShowFailedMessage("La posicion en terciario para la primera seccion no es valida")
                    return false
                }

                if (viewModel.Positions.Terciario.find(x => x === sheet.range(ter2).value()?.toString()) === undefined) {
                    ShowFailedMessage("La posicion en terciario para la segunda seccion no es valida")
                    return false
                }
            } else {
                if (viewModel.Positions.BajaTension.find(x => x === sheet.range(ter1).value()?.toString()) === undefined) {
                    ShowFailedMessage("La posicion segunda baja para la primera seccion no es valida")
                    return false
                }

                if (viewModel.Positions.BajaTension.find(x => x === sheet.range(ter2).value()?.toString()) === undefined) {
                    ShowFailedMessage("La posicion segunda baja para la segunda seccion no es valida")
                    return false
                }
            }
          

        } 


        var totalPaginas = sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TotalPaginas').Celda).value()
        if (totalPaginas === undefined || totalPaginas === null) {
            ShowFailedMessage("Favor de proporcionar el total de páginas")
            return false
        }

        var temp
        if (viewModel.NivelAceiteLab === "Lleno" || viewModel.NivelAceiteLab === "Full") {
            temp = sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 1).Celda).value()
            if (temp === undefined || temp === null ) {
                ShowFailedMessage("Favor de proporcionar temperatura del aceite")
                return false
            }
        }

        if (viewModel.NivelAceiteEmp === "Lleno" || viewModel.NivelAceiteEmp === "Full") {
            temp = sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 2).Celda).value()
            if (temp === undefined || temp === null) {
                ShowFailedMessage("Favor de proporcionar temperatura del aceite")
                return false
            }
        }



        return true


    }
    else {
        at1 = viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda;
        bt1 = viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda;
        ter1 = viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda;
        if (viewModel.Positions.AltaTension.find(x => x === sheet.range(at1).value()?.toString()) === undefined) {
            ShowFailedMessage("La posicion en alta no es valida")
            return false
        }
        if (viewModel.Positions.BajaTension.find(x => x === sheet.range(bt1).value()?.toString()) === undefined) {
            ShowFailedMessage("La posicion en baja no es valida")
            return false
        }


        if (viewModel.Columnas === 3) {
            if (viewModel.TerciarioOSegunda !== "2B") {
                if (viewModel.TerciarioDisponible !== 'No') {
                    if (viewModel.Positions.Terciario.find(x => x === sheet.range(ter1).value()?.toString()) === undefined) {
                        ShowFailedMessage("La posicion en terciario no es valida")
                        return false
                    }
                }
            } else {
                if (viewModel.Positions.BajaTension.find(x => x === sheet.range(ter1).value()?.toString()) === undefined) {
                    ShowFailedMessage("La posicion segunda baja no es valida")
                    return false
                }
            }
          
        }


        var totalPaginas = sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TotalPaginas').Celda).value()
        if (totalPaginas === undefined || totalPaginas === null) {
            ShowFailedMessage("Favor de proporcionar el total de páginas")
            return false
        }


        var temp

        if (viewModel.Pruebas === "LAB") {
            if (viewModel.NivelAceiteLab === "Lleno" || viewModel.NivelAceiteLab === "Full") {
                temp = sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 1).Celda).value()
                if (temp === undefined || temp === null) {
                    ShowFailedMessage("Favor de proporcionar temperatura del aceite")
                    return false
                }
            }
        }


        if (viewModel.Pruebas === "PLA") {
            if (viewModel.NivelAceiteEmp === "Lleno" || viewModel.NivelAceiteEmp === "Full") {
                temp = sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 1).Celda).value()
                if (temp === undefined || temp === null) {
                    ShowFailedMessage("Favor de proporcionar temperatura del aceite")
                    return false
                }
            }
        }



        return true

    }





}

