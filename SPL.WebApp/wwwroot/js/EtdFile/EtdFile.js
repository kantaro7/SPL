﻿
var banderaFileErrorExtension = false;
var banderaFileErrorPeso = false;
var pesoMaximo;
let modelFormData = new FormData();
var ConfigurationFiles = [];
//Variables and initializations of components
var editor;
var banderaErrorGrids = false;
var errorEnTablas = false;

let resultValidations = true;
let requestInicial = true;

let viewModel;

let btnRequest = document.getElementById("btnRequest");
let btnSave = document.getElementById("btnSave");
let btnClear = document.getElementById("btnClear");
let btnDown = document.getElementById("btnDownload");
let btnUp = document.getElementById("btnCargar");
let inputFile = document.getElementById("File");



let noSerieInput = document.getElementById("NoSerie");
let temperatureInput = document.getElementById("Temperature");
let unitMeasuringInput = document.getElementById("UnitMeasuring");
let claveIdiomaInput = document.getElementById("claveIdiomaD");
let claveIdiomaInput2 = document.getElementById("ClaveIdioma2");
btnDown.disabled = true
btnUp.disabled = true
let currentTap = "1";
let spreadsheetElement;


let img64;


//Events
$("#NoSerie").val("");
$("#NoSerie").focus();
$("#btnRequest").disabled = false;
$("#btnRequest").disabled = false;

$("#selectAT").val("");
$("#selectBT").val("");
$("#selectTER").val("");

$("#selectAT").prop("disabled", true);
$("#selectBT").prop("disabled", true);
$("#selectTer").prop("disabled", true);

$("#TipoEnfriamiento").val("");
$("#TipoEnfriamiento").prop("disabled", true);

$("#Otro").val("");
$("#Otro").prop("disabled", true);

$("#selectCapacidades").val("");
$("#selectCapacidades").prop("disabled", true);

$("#claveIdiomaD").val("");
$("#claveIdiomaD").prop("disabled", true);

$("#Altitud1").val("");
$("#Altitud1").prop("disabled", true);


$("#Altitud2").val("");
$("#Altitud2").prop("disabled", true);

$("#Cliente").val("");
$("#Cliente").prop("disabled", true);

$("#CapacidadReporte").val("");
$("#CapacidadReporte").prop("disabled", true);


$("#CapacidadReporte").val("");
$("#CapacidadReporte").prop("disabled", true);




$("#claveIdiomaC").val("");
$("#claveIdiomaC").prop("disabled", true);

$("#TipoEnfriamiento2").val("");
$("#TipoEnfriamiento2").prop("disabled", true);

$("#Otro2").val("");
$("#Otro2").prop("disabled", true);

$("#OtraCapacidad").val("");
$("#OtraCapacidad").prop("disabled", true);

$("#File").val("");
$("#File").prop("disabled", true);

$("#File").val("");
$("#File").prop("disabled", true);

$("#File").val("");
$("#File").prop("disabled", true);

$('input[type=checkbox]').prop('checked', false);
$('.child-checkbox input[type=checkbox]')
    .attr('disabled', true);

$("#errores").val("");
$("#errores").prop("disabled", true);

$("#NoSerie").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $("#btnRequest").click();
    }
});


btnClear.addEventListener("click", function () {
    $("#SelectCapacidades option[value!='']").each(function () {
        $(this).remove();
    });
    $("#TipoEnfriamiento option[value!='']").each(function () {
        $(this).remove();
    });


    $("#TipoEnfriamiento2 option[value!='']").each(function () {
        $(this).remove();
    });

    $("#selectAT option[value!='']").each(function () {
        $(this).remove();
    });

    $("#selectBT option[value!='']").each(function () {
        $(this).remove();
    });

    $("#selectTer option[value!='']").each(function () {
        $(this).remove();
    });

    $("#Altitud1").val("")
    $("#Altitud2").val("")
    $("#Cliente").val("")
    $("#CapacidadReporte").val("")

    $("#Otro").val("")
    $("#Otro").prop("disabled", true)

    $("#OtraCapacidad").val("")
    $("#OtraCapacidad").prop("disabled", true)
    //CapacidadesList

    $("#Otro2").val("")
    $("#Otro2").prop("disabled", true)

    $("#selectTer").prop("disabled", true)
    $("#selectBT").prop("disabled", true)
    $("#selectAT").prop("disabled", true)

    claveIdiomaInput.value = ''
    claveIdiomaInput2.value = ''
    btnRequest.disabled = false;
    btnDown.disabled = true
    btnUp.disabled = true

    $("span.k-invalid-msg").remove()

});

function check(e) {
    tecla = (document.all) ? e.keyCode : e.which;

    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8) {
        return true;
    }

    // Patrón de entrada, en este caso solo acepta numeros y letras
    patron = /[A-Za-z0-9]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}


btnRequest.addEventListener("click", async function () {
    if (noSerieInput.value === undefined || noSerieInput.value === "" || noSerieInput.value === null) {
        $(`#NoSerieSpand`).text("Requerido");
        return;
    }
    else {
        $(`#NoSerieSpand`).text("");
    }

    if (!(/^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ0-9-@]*$/.test(noSerieInput.value))) {
        $(`#NoSerieSpand`).text("Character(es) no permitido.");
        return;
    }
    else {
        $(`#NoSerieSpand`).text("");
    }
    /*
    if (currentTap === 1) {
        var validate_form = $("#form_menu_sup1").data("kendoValidator").validate();
    }
    else if (currentTap === 2) {
        var validate_form = $("#form_menu_sup1").data("kendoValidator").validate();
    } else {
        return
    }*/



    $("#loader").css("display", "block");

    await GetFilter().then(
        data => {
            if (data.response.Code !== -1) {
                noSerieInput.disabled = true;

                viewModel = data.response.Structure;
                LoadForm(viewModel);

                btnRequest.disabled = true;
                btnClear.disabled = false;
            }
            else {
                ShowFailedMessage(data.response.Description);
            }
            $("#loader").css("display", "none");
        }
    );
    $("#loader").css("display", "none");
});


$("#Enfriamiento").on("change", function () {
    let val = $("#Enfriamiento").val()

    if (val === '0') {
        $("#Otro").prop("disabled", false)
    } else {
        $("#Otro").prop("disabled", true)
    }
});

$("#SelectCapacidades").on("change", function () {
    let val = $("#SelectCapacidades").val()

    if (val === 'Otro') {
        $("#OtraCapacidad").prop("disabled", false)
    } else {
        $("#OtraCapacidad").prop("disabled", true)
    }
});

function LoadForm(response) {
/*    if (currentTap === "1") {*/
    claveIdiomaInput.value = response.ClaveIdioma;

            $("#SelectCapacidades option[value!='']").each(function () {
                $(this).remove();
            });
    $.each(response.CapacidadesList, function (i, val) {
        if (val == null) {
               $("#SelectCapacidades").append("<option value='Otro'>" + 'Otro' + "</option>");
           } else {
               $("#SelectCapacidades").append("<option value='" + val + "'>" + val + "</option>");}

        });

        $("#TipoEnfriamiento option[value!='']").each(function () {
            $(this).remove();
        });

        $.each(response.EnfriamientosList, function (i, val) {
            $("#TipoEnfriamiento").append("<option value='" + val + "'>" + val + "</option>");
        });

        $('#TipoEnfriamiento').append($('<option>', {
            value: 0,
            text: 'Otro'
        }));
        $("#Altitud1").val(response.Altitud1)
        $("#Altitud2").val(response.Altitud2)
        $("#Cliente").val(response.Cliente)
        $("#CapacidadReporte").val(response.CapacidadReporte)


        if (response.PositionsDTO.AltaTension !== null && response.PositionsDTO.AltaTension.length > 0) {

            $("#selectAT").prop("disabled", false);

            $.each(response.PositionsDTO.AltaTension, function (i, val) {
                $("#selectAT").append("<option value='" + val + "'>" + val + "</option>");
            });
        }

        if (response.PositionsDTO.BajaTension !== null && response.PositionsDTO.BajaTension.length > 0) {

            $("#selectBT").prop("disabled", false);

            $.each(response.PositionsDTO.BajaTension, function (i, val) {
                $("#selectBT").append("<option value='" + val + "'>" + val + "</option>");
            });
        }

        if (response.PositionsDTO.Terciario !== null && response.PositionsDTO.Terciario.length > 0) {

            $("#selectTer").prop("disabled", false);

            $.each(response.PositionsDTO.Terciario, function (i, val) {
                $("#selectTer").append("<option value='" + val + "'>" + val + "</option>");
            });
        }
        btnDown.disabled = false

/*    } else if (currentTap === "2") {*/
        claveIdiomaInput2.value = response.ClaveIdioma;

       
        $("#TipoEnfriamiento2 option[value!='']").each(function () {
            $(this).remove();
        });

        $.each(response.EnfriamientosList, function (i, val) {
            $("#TipoEnfriamiento2").append("<option value='" + val + "'>" + val + "</option>");
        });

        $('#TipoEnfriamiento2').append($('<option>', {
            value: 0,
            text: 'Otro'
        }));
    //}

    $("#btnDownload").prop("disabled", false);
    $("#btnCargar").prop("disabled", false);

    $("#selectAT").prop("disabled", false);
    $("#selectBT").prop("disabled", false);
    $("#selectTer").prop("disabled", false);


    $("#TipoEnfriamiento").prop("disabled", false);



    $("#selectCapacidades").prop("disabled", false);


    $("#claveIdiomaD").prop("disabled", false);


    $("#claveIdiomaC").prop("disabled", false);

    $("#TipoEnfriamiento2").prop("disabled", false);



    $("#File").prop("disabled", false);


    $('input[type=checkbox]').prop('checked', true);
    $('.child-checkbox input[type=checkbox]')
        .attr('disabled', false);


    $("#errores").disabled = false;
   
}



btnDown.addEventListener("click", async function () {
    $("#loader").css("display", "block");

   var result = ValidateForm(currentTap);
    if (result) {
        if (currentTap == 2) {
            var files = $('#files').data('kendoUpload').getFiles()
            if (files.length > 0) {
                $.each(files, function () {
                    //console.log(this)
                    if (this.error !== undefined) {
                        error = true
                    }
                })
            } else {
                $("#loader").css("display", "none");
                ShowFailedMessage("Debe seleccionar por lo menos un archivo")
                return
            }

            if (error) {
                $("#loader").css("display", "none");
                ShowFailedMessage("Se han encontrado errores en los archivos.")
                return
            }

            for (const elemento of files) {
                var element = new Object();
                await test(elemento).then(data => { element.Base64 = data, element.Name = elemento.name })
                viewModel.Archivos.push(element)

            }
        }
      



        $("#loader").css("display", "block");

        DownloadFile();

    }
   else{
        ShowFailedMessage("Faltan campos por llenar")
        $("#loader").css("display", "none"); }

});


btnUp.addEventListener("click", async function () {
    $("#loader").css("display", "block");

    var result = ValidateForm("2");
    if (result) {
        if (currentTap == "2") {
            var files = $('#files').data('kendoUpload').getFiles()
            if (files.length > 0) {
                $.each(files, function () {
                    //console.log(this)
                    if (this.error !== undefined) {
                        error = true
                    }
                })
            } else {
                $("#loader").css("display", "none");
                ShowFailedMessage("Debe seleccionar por lo menos un archivo")
                return
            }

            if (error) {
                $("#loader").css("display", "none");
                ShowFailedMessage("Se han encontrado errores en los archivos.")
                return
            }

            //for (const elemento of files) {
            //    var element = new Object();
            //    await test(elemento).then(data => { element.Base64 = data, element.Name = elemento.name })
            //    viewModel.Archivos.push(element)

            //}
        }




        $("#loader").css("display", "block");

        LoadFile();

    }
    else {
        ShowFailedMessage("Faltan campos por llenar")
        $("#loader").css("display", "none");
    }

});



$("#NoSerie").val("");
$("#NoSerie").focus();
$("#btnRequest").disabled = false;
$("#btnRequest").disabled = false;

$("#selectAT").val("");
$("#selectBT").val("");
$("#selectTER").val("");

$("#selectAT").prop("disabled", true);
$("#selectBT").prop("disabled", true);
$("#selectTer").prop("disabled", true);

$("#TipoEnfriamiento").val("");
$("#TipoEnfriamiento").prop("disabled", true);

$("#Otro").val("");
$("#Otro").prop("disabled", true);

$("#selectCapacidades").val("");
$("#selectCapacidades").prop("disabled", true);

$("#claveIdiomaD").val("");
$("#claveIdiomaD").prop("disabled", true);

$("#Altitud1").val("");
$("#Altitud1").prop("disabled", true);


$("#Altitud2").val("");
$("#Altitud2").prop("disabled", true);

$("#Cliente").val("");
$("#Cliente").prop("disabled", true);

$("#CapacidadReporte").val("");
$("#CapacidadReporte").prop("disabled", true);


$("#CapacidadReporte").val("");
$("#CapacidadReporte").prop("disabled", true);




$("#claveIdiomaC").val("");
$("#claveIdiomaC").prop("disabled", true);

$("#TipoEnfriamiento2").val("");
$("#TipoEnfriamiento2").prop("disabled", true);

$("#Otro2").val("");
$("#Otro2").prop("disabled", true);

$("#File").val("");
$("#File").prop("disabled", true);

$("#File").val("");
$("#File").prop("disabled", true);

$("#File").val("");
$("#File").prop("disabled", true);



async function LoadFile() {
    $("#errores").text("");
    //var path = path = "/EtdFile/DownloadFile/";

    //var url = new URL(domain + path),
    //    params = {
    //        noSerie: $("#NoSerie").val(),
    //        clavePrueba:"",
    //        claveIdioma: $("#claveIdiomaD").val(),
    //        posAT: $("#selectAT").val(),
    //        posBT: $("#selectBT").val(),
    //        posTer: $("#selectTer").val(),
    //        coolingType: $("#TipoEnfriamiento").val(),
    //        otherCoolingType: $("#Otro").val(),
    //        capacity: $("#selectCapacidades").val() ,
    //        altitud1: $("#Altitud1").val(),
    //        altitud2: $("#Altitud2").val(),
    //        clientName: $("#Cliente").val(),
    //        reportCapacities: $("#CapacidadReporte").val(),
    //        grados:0}
    //Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))
    //$("#loader").css("display", "none");
    //var win = window.open(url.toString(), '_blank');
    //win.focus();


    LoadFileJSON(null);
        //data => {
            //if (data.response.Code !== -1) {

            //    //spreadsheetElement.data("kendoSpreadsheet").destroy()
            //    //$("#spreadsheet").empty();
            //  //  SetConfigExcel(data.response.Structure);

            //    // generaDescargablePdf(data.response.Structure, $("#TipoEnfriamiento").val() +" "+ $("#NoSerie").val()+ ".xlsx")

            //    GetGraphisByReport();


            //}
            //else {

            //    $("#errores").text(data.response.Description);
            //   // ShowFailedMessage(data.response.Structure);
            //}
            //$("#loader").css("display", "none");
       
    //);

}





async function GetGraphisReportJSON() {
    var path = path = "/EtdFile/GetTestDataExcel/";

    var url = new URL(domain + path),
        params = {}
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const test = await response.json();
        return test;
    }

    else {
        ShowFailedMessage('Error, por favor contacte al administrador del sistema.');
        return null;
    }
}


$("#btnExportGrafica").on("click", async function () {
    var chart = $("#pdfViewer").getKendoChart();
    chart.exportImage().done(function (data) {
        img64 = data;
        kendo.saveAs({
            dataURI: data,
            fileName: "chart.jpg"
        });
    });
});


async function GetGraphisByReport() {

    await GetGraphisReportJSON().then(
        data => {
            model = data.response
            $("#pdfViewer").kendoChart({
                chartArea: {
                    width: 600,
                    height: 400
                },
                title: {
                    text: "Charge current vs. charge time" //Nombre del grafico
                },
                legend: {
                    visible: true //habilita la leyenda
                },
                seriesDefaults: {
                    type: "scatterLine"   //tipo de grafico, en este caso es scatterLine y ambas graficas son del mismo tipo

                },
                series: [{
                    name: "0.8C",        //leyenda del lado de la parte derecha  , se puede comentar
                    data: model.data     // esta seria la data del archivo excel en formato decimal[][] para la grafica 1 
                }, {
                    name: "3.1C",          //leyenda del lado de la parte derecha  , se puede comentar
                    markers: { size: 0 },  // hace que los circulos de las graficas desaparezcam y se vean las dos solapadas
                    data: model.data2   // esta seria la data del archivo excel en formato decimal[][] para la grafica 2, viene el controlador
                }],
                xAxis: {
                    min: model.MinX,    //Define el valor minimo a colocar del eje X
                    max: model.MaxX,    //Define el valor maximo a colocar del eje X
                    labels: {
                        format: "{0}"
                    },
                    title: {
                        text: "Time"    //Define el nombre del eje X, segun el excel creo que es t/10
                    },
                    minorUnit: 0.6,     // Define el salto min de numeros entre los puntos de la grafica
                    majorUnit: 3        // Define el salto max de numeros entre los puntos de la grafica
                },
                yAxis: {
                    min: model.MinY,    //Define el valor minimo a colocar del eje Y
                    max: model.MaxY,    //Define el valor maximo a colocar del eje Y
                    labels: {
                        format: "{0:N4}"    //Formato de numeros del eje Y
                    },
                    title: {
                        text: "Charge"      //Define el titulo del eje Y, segun la grafica del archivo excel
                    },
                    minorUnit: 0.04,         // Define el salto min de numeros entre los puntos de la grafica
                    majorUnit: 0.1          // Define el salto max de numeros entre los puntos de la grafica
                }

            });
        }

    );
}










async function DownloadFile() {
    //var path = path = "/EtdFile/DownloadFile/";

    //var url = new URL(domain + path),
    //    params = {
    //        noSerie: $("#NoSerie").val(),
    //        clavePrueba:"",
    //        claveIdioma: $("#claveIdiomaD").val(),
    //        posAT: $("#selectAT").val(),
    //        posBT: $("#selectBT").val(),
    //        posTer: $("#selectTer").val(),
    //        coolingType: $("#TipoEnfriamiento").val(),
    //        otherCoolingType: $("#Otro").val(),
    //        capacity: $("#selectCapacidades").val() ,
    //        altitud1: $("#Altitud1").val(),
    //        altitud2: $("#Altitud2").val(),
    //        clientName: $("#Cliente").val(),
    //        reportCapacities: $("#CapacidadReporte").val(),
    //        grados:0}
    //Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))
    //$("#loader").css("display", "none");
    //var win = window.open(url.toString(), '_blank');
    //win.focus();


    DownloadFileJSON(null).then(
        data => {
            if (data.response.Code !== -1) {
          
                //spreadsheetElement.data("kendoSpreadsheet").destroy()
                //$("#spreadsheet").empty();
                SetConfigExcel(data.response.Structure);

               // generaDescargablePdf(data.response.Structure, $("#TipoEnfriamiento").val() +" "+ $("#NoSerie").val()+ ".xlsx")
            }
            else {
                ShowFailedMessage(data.response.Structure);
            }
            $("#loader").css("display", "none");
        }
    );

}


function SetConfigExcel(workbook) {
    var tipoEnf;
    $("#TipoEnfriamiento").val() == "0" ? (
        tipoEnf = "Otro"
    ) : (
        tipoEnf = $("#TipoEnfriamiento").val()
    );

    
    spreadsheetElement = $("#spreadsheet").kendoSpreadsheet({
        sheets: workbook.sheets,
        excel: {
            fileName: tipoEnf + " " + $("#NoSerie").val() + ".xlsx"
        }
        //toolbar: {

        //    backgroundColor: "#3f51b5 !important",
        //    textColor: "#fff",
        //    home: [

        //        {
        //            type: "button",
        //            text: "Cargar Archivos",
        //            showText: "both",
        //            icon: "k-icon k-i-calculator",


        //            click: function () {
        //                btnValidate.click();

        //            }
        //        },
        //        {
        //            type: "button",
        //            text: "Guardar",
        //            showText: "both",
        //            icon: "k-icon k-i-save",

        //            click: function () {
        //                btnSave.click();


        //            }
        //        },
        //        {
        //            type: "button",
        //            text: "Reiniciar",
        //            showText: "both",
        //            icon: "k-icon k-i-refresh",

        //            click: function () {
        //                btnRefresh.click();
        //            }
        //        },
        //        {
        //            type: "button",
        //            text: "No Prueba: " + viewModel.NoPrueba,
        //            showText: "both",
        //            attributes: { "style": "color:#8854FF;pointer-events:none;border:none;background:none;" }
        //        }
        //    ],
        //    insert: false,
        //    data: false,
        //    redo: false,
        //    undo: false,
        //}
    });

    var spreadsheet = $("#spreadsheet").data("kendoSpreadsheet");
    var sheet = spreadsheet.activeSheet();
    var sheetc1 = spreadsheet._workbook._sheets[1];
    var sheetc2 = spreadsheet._workbook._sheets[2];
    var sheetc3 = spreadsheet._workbook._sheets[3];

    var sheetf1 = spreadsheet._workbook._sheets[4];
    var sheetf2 = spreadsheet._workbook._sheets[5];
    var sheetf3 = spreadsheet._workbook._sheets[6];


    var drawing = kendo.spreadsheet.Drawing.fromJSON({
        topLeftCell: "A1",
        offsetX: 0,
        offsetY: 0,
        width: 220,
        height: 43,
        image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    });
    sheet.addDrawing(drawing);

    var drawing = kendo.spreadsheet.Drawing.fromJSON({
        topLeftCell: "A1",
        offsetX: 0,
        offsetY: 0,
        width: 220,
        height: 43,
        image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    });
    sheetc1.addDrawing(drawing);


    var drawing = kendo.spreadsheet.Drawing.fromJSON({
        topLeftCell: "A1",
        offsetX: 0,
        offsetY: 0,
        width: 220,
        height: 43,
        image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    });
    sheetc2.addDrawing(drawing);

    var drawing = kendo.spreadsheet.Drawing.fromJSON({
        topLeftCell: "A1",
        offsetX: 0,
        offsetY: 0,
        width: 220,
        height: 43,
        image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    });
    sheetc3.addDrawing(drawing);

    var drawing = kendo.spreadsheet.Drawing.fromJSON({
        topLeftCell: "A1",
        offsetX: 0,
        offsetY: 0,
        width: 220,
        height: 43,
        image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    });
    sheetf1.addDrawing(drawing);

    var drawing = kendo.spreadsheet.Drawing.fromJSON({
        topLeftCell: "A1",
        offsetX: 0,
        offsetY: 0,
        width: 220,
        height: 43,
        image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    });
    sheetf2.addDrawing(drawing);


    var drawing = kendo.spreadsheet.Drawing.fromJSON({
        topLeftCell: "A1",
        offsetX: 0,
        offsetY: 0,
        width: 220,
        height: 43,
        image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    });
    sheetf3.addDrawing(drawing);


    sheet.range("P2").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Perdidas,Corriente"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheet.range("P4").validation({
        comparerType: "list",
        dataType: "list",
        from: '"FASL,MSNM"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheet.range("P7").validation({
        comparerType: "list",
        dataType: "list",
        from: '"1 Hr,30 Min."',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheet.range("P9").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Si, No"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheet.range("P13").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Si, No"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheet.range("P16").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Ingles, Español"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheet.range("P18").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Serie, Paralelo, #NA"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheet.range("P21").validation({
        comparerType: "list",
        dataType: "list",
        from: '"NA, X aariba, Y arriba"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheet.range("P23").validation({
        comparerType: "list",
        dataType: "list",
        from: '"CU, AI"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });




    /*   ***CF1***/

    sheetc1.range("E10").validation({
        comparerType: "list",
        dataType: "list",
        from: '"1/0/00 12:00 AM,1/0/00 1:00 AM,1/0/00 2:00 AM,1/0/00 3:00 AM,1/0/00 4:00 AM,1/0/00 5:00 AM,1/0/00 6:00 AM,1/0/00 7:00 AM,1/0/00 8:00 AM,1/0/00 9:00 AM,1/0/00 10:00 AM,1/0/00 11:00 AM,1/0/00 12:00 PM,1/0/00 1:00 PM,1/0/00 2:00 PM,1/0/00 3:00 PM,1/0/00 4:00 PM,1/0/00 5:00 PM,1/0/00 6:00 PM,1/0/00 7:00 PM,1/0/00 8:00 PM,1/0/00 9:00 PM,1/0/00 10:00 PM,1/0/00 11:00 PM,1/1/00 12:00 AM,1/1/00 1:00 AM,1/1/00 2:00 AM,1/1/00 3:00 AM,1/1/00 4:00 AM,1/1/00 5:00 AM,1/1/00 6:00 AM,1/1/00 7:00 AM,1/1/00 8:00 AM,1/1/00 9:00 AM,1/1/00 10:00 AM,1/1/00 11:00 AM,1/1/00 12:00 PM,1/1/00 1:00 PM,1/1/00 2:00 PM,1/1/00 3:00 PM,1/1/00 4:00 PM,1/1/00 5:00 PM,1/1/00 6:00 PM,1/1/00 7:00 PM,1/1/00 8:00 PM,1/1/00 9:00 PM,1/1/00 10:00 PM,1/1/00 11:00 PM,1/2/00 12:00 AM,1/2/00 1:00 AM,1/2/00 2:00 AM,1/2/00 3:00 AM,NA"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc1.range("E13").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Cuadratica, Exponencial"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc1.range("K10").validation({
        comparerType: "list",
        dataType: "list",
        from: '"1/0/00 12:00 AM,1/0/00 1:00 AM,1/0/00 2:00 AM,1/0/00 3:00 AM,1/0/00 4:00 AM,1/0/00 5:00 AM,1/0/00 6:00 AM,1/0/00 7:00 AM,1/0/00 8:00 AM,1/0/00 9:00 AM,1/0/00 10:00 AM,1/0/00 11:00 AM,1/0/00 12:00 PM,1/0/00 1:00 PM,1/0/00 2:00 PM,1/0/00 3:00 PM,1/0/00 4:00 PM,1/0/00 5:00 PM,1/0/00 6:00 PM,1/0/00 7:00 PM,1/0/00 8:00 PM,1/0/00 9:00 PM,1/0/00 10:00 PM,1/0/00 11:00 PM,1/1/00 12:00 AM,1/1/00 1:00 AM,1/1/00 2:00 AM,1/1/00 3:00 AM,1/1/00 4:00 AM,1/1/00 5:00 AM,1/1/00 6:00 AM,1/1/00 7:00 AM,1/1/00 8:00 AM,1/1/00 9:00 AM,1/1/00 10:00 AM,1/1/00 11:00 AM,1/1/00 12:00 PM,1/1/00 1:00 PM,1/1/00 2:00 PM,1/1/00 3:00 PM,1/1/00 4:00 PM,1/1/00 5:00 PM,1/1/00 6:00 PM,1/1/00 7:00 PM,1/1/00 8:00 PM,1/1/00 9:00 PM,1/1/00 10:00 PM,1/1/00 11:00 PM,1/2/00 12:00 AM,1/2/00 1:00 AM,1/2/00 2:00 AM,1/2/00 3:00 AM,NA"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc1.range("K12").validation({
        comparerType: "list",
        dataType: "list",
        from: '"1/0/00 12:00 AM,1/0/00 1:00 AM,1/0/00 2:00 AM,1/0/00 3:00 AM,1/0/00 4:00 AM,1/0/00 5:00 AM,1/0/00 6:00 AM,1/0/00 7:00 AM,1/0/00 8:00 AM,1/0/00 9:00 AM,1/0/00 10:00 AM,1/0/00 11:00 AM,1/0/00 12:00 PM,1/0/00 1:00 PM,1/0/00 2:00 PM,1/0/00 3:00 PM,1/0/00 4:00 PM,1/0/00 5:00 PM,1/0/00 6:00 PM,1/0/00 7:00 PM,1/0/00 8:00 PM,1/0/00 9:00 PM,1/0/00 10:00 PM,1/0/00 11:00 PM,1/1/00 12:00 AM,1/1/00 1:00 AM,1/1/00 2:00 AM,1/1/00 3:00 AM,1/1/00 4:00 AM,1/1/00 5:00 AM,1/1/00 6:00 AM,1/1/00 7:00 AM,1/1/00 8:00 AM,1/1/00 9:00 AM,1/1/00 10:00 AM,1/1/00 11:00 AM,1/1/00 12:00 PM,1/1/00 1:00 PM,1/1/00 2:00 PM,1/1/00 3:00 PM,1/1/00 4:00 PM,1/1/00 5:00 PM,1/1/00 6:00 PM,1/1/00 7:00 PM,1/1/00 8:00 PM,1/1/00 9:00 PM,1/1/00 10:00 PM,1/1/00 11:00 PM,1/2/00 12:00 AM,1/2/00 1:00 AM,1/2/00 2:00 AM,1/2/00 3:00 AM,NA"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc1.range("K14").validation({
        comparerType: "list",
        dataType: "list",
        from: '"1/0/00 12:00 AM, 1/0/00 1:00 AM, 1/0/00 2:00 AM, 1/0/00 3:00 AM, 1/0/00 4:00 AM, 1/0/00 5:00 AM, 1/0/00 6:00 AM, 1/0/00 7:00 AM, 1/0/00 8:00 AM, 1/0/00 9:00 AM, 1/0/00 10:00 AM, 1/0/00 11:00 AM, 1/0/00 12:00 PM, 1/0/00 1:00 PM, 1/0/00 2:00 PM, 1/0/00 3:00 PM, 1/0/00 4:00 PM, 1/0/00 5:00 PM, 1/0/00 6:00 PM, 1/0/00 7:00 PM, 1/0/00 8:00 PM, 1/0/00 9:00 PM, 1/0/00 10:00 PM, 1/0/00 11:00 PM, 1/1/00 12:00 AM,1/1/00 1:00 AM, 1/1/00 2:00 AM, 1/1/00 3:00 AM, 1/1/00 4:00 AM, 1/1/00 5:00 AM, 1/1/00 6:00 AM, 1/1/00 7:00 AM, 1/1/00 8:00 AM, 1/1/00 9:00 AM, 1/1/00 10:00 AM, 1/1/00 11:00 AM, 1/1/00 12:00 PM, 1/1/00 1:00 PM, 1/1/00 2:00 PM, 1/1/00 3:00 PM, 1/1/00 4:00 PM, 1/1/00 5:00 PM, 1/1/00 6:00 PM, 1/1/00 7:00 PM,1/1/00 8:00 PM, 1/1/00 9:00 PM, 1/1/00 10:00 PM, 1/1/00 11:00 PM,1/2/00 12:00 AM, 1/2/00 1:00 AM, 1/2/00 2:00 AM, 1/2/00 3:00 AM, NA"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc1.range("F20").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Ohms, Miliohms"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc1.range("H20").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Ohms, Miliohms"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc1.range("J20").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Ohms, Miliohms"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });
    /*   ***CF1***/



    /*   ***CF2***/

    sheetc2.range("E10").validation({
        comparerType: "list",
        dataType: "list",
        from: '"1/0/00 12:00 AM,1/0/00 1:00 AM,1/0/00 2:00 AM,1/0/00 3:00 AM,1/0/00 4:00 AM,1/0/00 5:00 AM,1/0/00 6:00 AM,1/0/00 7:00 AM,1/0/00 8:00 AM,1/0/00 9:00 AM,1/0/00 10:00 AM,1/0/00 11:00 AM,1/0/00 12:00 PM,1/0/00 1:00 PM,1/0/00 2:00 PM,1/0/00 3:00 PM,1/0/00 4:00 PM,1/0/00 5:00 PM,1/0/00 6:00 PM,1/0/00 7:00 PM,1/0/00 8:00 PM,1/0/00 9:00 PM,1/0/00 10:00 PM,1/0/00 11:00 PM,1/1/00 12:00 AM,1/1/00 1:00 AM,1/1/00 2:00 AM,1/1/00 3:00 AM,1/1/00 4:00 AM,1/1/00 5:00 AM,1/1/00 6:00 AM,1/1/00 7:00 AM,1/1/00 8:00 AM,1/1/00 9:00 AM,1/1/00 10:00 AM,1/1/00 11:00 AM,1/1/00 12:00 PM,1/1/00 1:00 PM,1/1/00 2:00 PM,1/1/00 3:00 PM,1/1/00 4:00 PM,1/1/00 5:00 PM,1/1/00 6:00 PM,1/1/00 7:00 PM,1/1/00 8:00 PM,1/1/00 9:00 PM,1/1/00 10:00 PM,1/1/00 11:00 PM,1/2/00 12:00 AM,1/2/00 1:00 AM,1/2/00 2:00 AM,1/2/00 3:00 AM,NA"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc2.range("E13").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Cuadratica, Exponencial"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc2.range("K10").validation({
        comparerType: "list",
        dataType: "list",
        from: '"1/0/00 12:00 AM,1/0/00 1:00 AM,1/0/00 2:00 AM,1/0/00 3:00 AM,1/0/00 4:00 AM,1/0/00 5:00 AM,1/0/00 6:00 AM,1/0/00 7:00 AM,1/0/00 8:00 AM,1/0/00 9:00 AM,1/0/00 10:00 AM,1/0/00 11:00 AM,1/0/00 12:00 PM,1/0/00 1:00 PM,1/0/00 2:00 PM,1/0/00 3:00 PM,1/0/00 4:00 PM,1/0/00 5:00 PM,1/0/00 6:00 PM,1/0/00 7:00 PM,1/0/00 8:00 PM,1/0/00 9:00 PM,1/0/00 10:00 PM,1/0/00 11:00 PM,1/1/00 12:00 AM,1/1/00 1:00 AM,1/1/00 2:00 AM,1/1/00 3:00 AM,1/1/00 4:00 AM,1/1/00 5:00 AM,1/1/00 6:00 AM,1/1/00 7:00 AM,1/1/00 8:00 AM,1/1/00 9:00 AM,1/1/00 10:00 AM,1/1/00 11:00 AM,1/1/00 12:00 PM,1/1/00 1:00 PM,1/1/00 2:00 PM,1/1/00 3:00 PM,1/1/00 4:00 PM,1/1/00 5:00 PM,1/1/00 6:00 PM,1/1/00 7:00 PM,1/1/00 8:00 PM,1/1/00 9:00 PM,1/1/00 10:00 PM,1/1/00 11:00 PM,1/2/00 12:00 AM,1/2/00 1:00 AM,1/2/00 2:00 AM,1/2/00 3:00 AM,NA"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc2.range("K12").validation({
        comparerType: "list",
        dataType: "list",
        from: '"1/0/00 12:00 AM,1/0/00 1:00 AM,1/0/00 2:00 AM,1/0/00 3:00 AM,1/0/00 4:00 AM,1/0/00 5:00 AM,1/0/00 6:00 AM,1/0/00 7:00 AM,1/0/00 8:00 AM,1/0/00 9:00 AM,1/0/00 10:00 AM,1/0/00 11:00 AM,1/0/00 12:00 PM,1/0/00 1:00 PM,1/0/00 2:00 PM,1/0/00 3:00 PM,1/0/00 4:00 PM,1/0/00 5:00 PM,1/0/00 6:00 PM,1/0/00 7:00 PM,1/0/00 8:00 PM,1/0/00 9:00 PM,1/0/00 10:00 PM,1/0/00 11:00 PM,1/1/00 12:00 AM,1/1/00 1:00 AM,1/1/00 2:00 AM,1/1/00 3:00 AM,1/1/00 4:00 AM,1/1/00 5:00 AM,1/1/00 6:00 AM,1/1/00 7:00 AM,1/1/00 8:00 AM,1/1/00 9:00 AM,1/1/00 10:00 AM,1/1/00 11:00 AM,1/1/00 12:00 PM,1/1/00 1:00 PM,1/1/00 2:00 PM,1/1/00 3:00 PM,1/1/00 4:00 PM,1/1/00 5:00 PM,1/1/00 6:00 PM,1/1/00 7:00 PM,1/1/00 8:00 PM,1/1/00 9:00 PM,1/1/00 10:00 PM,1/1/00 11:00 PM,1/2/00 12:00 AM,1/2/00 1:00 AM,1/2/00 2:00 AM,1/2/00 3:00 AM,NA"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc2.range("K14").validation({
        comparerType: "list",
        dataType: "list",
        from: '"1/0/00 12:00 AM,1/0/00 1:00 AM,1/0/00 2:00 AM,1/0/00 3:00 AM,1/0/00 4:00 AM,1/0/00 5:00 AM,1/0/00 6:00 AM,1/0/00 7:00 AM,1/0/00 8:00 AM,1/0/00 9:00 AM,1/0/00 10:00 AM,1/0/00 11:00 AM,1/0/00 12:00 PM,1/0/00 1:00 PM,1/0/00 2:00 PM,1/0/00 3:00 PM,1/0/00 4:00 PM,1/0/00 5:00 PM,1/0/00 6:00 PM,1/0/00 7:00 PM,1/0/00 8:00 PM,1/0/00 9:00 PM,1/0/00 10:00 PM,1/0/00 11:00 PM,1/1/00 12:00 AM,1/1/00 1:00 AM,1/1/00 2:00 AM,1/1/00 3:00 AM,1/1/00 4:00 AM,1/1/00 5:00 AM,1/1/00 6:00 AM,1/1/00 7:00 AM,1/1/00 8:00 AM,1/1/00 9:00 AM,1/1/00 10:00 AM,1/1/00 11:00 AM,1/1/00 12:00 PM,1/1/00 1:00 PM,1/1/00 2:00 PM,1/1/00 3:00 PM,1/1/00 4:00 PM,1/1/00 5:00 PM,1/1/00 6:00 PM,1/1/00 7:00 PM,1/1/00 8:00 PM,1/1/00 9:00 PM,1/1/00 10:00 PM,1/1/00 11:00 PM,1/2/00 12:00 AM,1/2/00 1:00 AM,1/2/00 2:00 AM,1/2/00 3:00 AM,NA"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc2.range("F20").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Ohms, Miliohms"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc2.range("H20").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Ohms, Miliohms"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc2.range("J20").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Ohms, Miliohms"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });
    /*   ***CF2***/




    /*   ***CF3***/

    sheetc3.range("E10").validation({
        comparerType: "list",
        dataType: "list",
        from: '"1/0/00 12:00 AM,1/0/00 1:00 AM,1/0/00 2:00 AM,1/0/00 3:00 AM,1/0/00 4:00 AM,1/0/00 5:00 AM,1/0/00 6:00 AM,1/0/00 7:00 AM,1/0/00 8:00 AM,1/0/00 9:00 AM,1/0/00 10:00 AM,1/0/00 11:00 AM,1/0/00 12:00 PM,1/0/00 1:00 PM,1/0/00 2:00 PM,1/0/00 3:00 PM,1/0/00 4:00 PM,1/0/00 5:00 PM,1/0/00 6:00 PM,1/0/00 7:00 PM,1/0/00 8:00 PM,1/0/00 9:00 PM,1/0/00 10:00 PM,1/0/00 11:00 PM,1/1/00 12:00 AM,1/1/00 1:00 AM,1/1/00 2:00 AM,1/1/00 3:00 AM,1/1/00 4:00 AM,1/1/00 5:00 AM,1/1/00 6:00 AM,1/1/00 7:00 AM,1/1/00 8:00 AM,1/1/00 9:00 AM,1/1/00 10:00 AM,1/1/00 11:00 AM,1/1/00 12:00 PM,1/1/00 1:00 PM,1/1/00 2:00 PM,1/1/00 3:00 PM,1/1/00 4:00 PM,1/1/00 5:00 PM,1/1/00 6:00 PM,1/1/00 7:00 PM,1/1/00 8:00 PM,1/1/00 9:00 PM,1/1/00 10:00 PM,1/1/00 11:00 PM,1/2/00 12:00 AM,1/2/00 1:00 AM,1/2/00 2:00 AM,1/2/00 3:00 AM,NA"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc3.range("E13").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Cuadratica, Exponencial"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc3.range("K10").validation({
        comparerType: "list",
        dataType: "list",
        from: '"1/0/00 12:00 AM,1/0/00 1:00 AM,1/0/00 2:00 AM,1/0/00 3:00 AM,1/0/00 4:00 AM,1/0/00 5:00 AM,1/0/00 6:00 AM,1/0/00 7:00 AM,1/0/00 8:00 AM,1/0/00 9:00 AM,1/0/00 10:00 AM,1/0/00 11:00 AM,1/0/00 12:00 PM,1/0/00 1:00 PM,1/0/00 2:00 PM,1/0/00 3:00 PM,1/0/00 4:00 PM,1/0/00 5:00 PM,1/0/00 6:00 PM,1/0/00 7:00 PM,1/0/00 8:00 PM,1/0/00 9:00 PM,1/0/00 10:00 PM,1/0/00 11:00 PM,1/1/00 12:00 AM,1/1/00 1:00 AM,1/1/00 2:00 AM,1/1/00 3:00 AM,1/1/00 4:00 AM,1/1/00 5:00 AM,1/1/00 6:00 AM,1/1/00 7:00 AM,1/1/00 8:00 AM,1/1/00 9:00 AM,1/1/00 10:00 AM,1/1/00 11:00 AM,1/1/00 12:00 PM,1/1/00 1:00 PM,1/1/00 2:00 PM,1/1/00 3:00 PM,1/1/00 4:00 PM,1/1/00 5:00 PM,1/1/00 6:00 PM,1/1/00 7:00 PM,1/1/00 8:00 PM,1/1/00 9:00 PM,1/1/00 10:00 PM,1/1/00 11:00 PM,1/2/00 12:00 AM,1/2/00 1:00 AM,1/2/00 2:00 AM,1/2/00 3:00 AM,NA"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc3.range("K12").validation({
        comparerType: "list",
        dataType: "list",
        from: '"1/0/00 12:00 AM,1/0/00 1:00 AM,1/0/00 2:00 AM,1/0/00 3:00 AM,1/0/00 4:00 AM,1/0/00 5:00 AM,1/0/00 6:00 AM,1/0/00 7:00 AM,1/0/00 8:00 AM,1/0/00 9:00 AM,1/0/00 10:00 AM,1/0/00 11:00 AM,1/0/00 12:00 PM,1/0/00 1:00 PM,1/0/00 2:00 PM,1/0/00 3:00 PM,1/0/00 4:00 PM,1/0/00 5:00 PM,1/0/00 6:00 PM,1/0/00 7:00 PM,1/0/00 8:00 PM,1/0/00 9:00 PM,1/0/00 10:00 PM,1/0/00 11:00 PM,1/1/00 12:00 AM,1/1/00 1:00 AM,1/1/00 2:00 AM,1/1/00 3:00 AM,1/1/00 4:00 AM,1/1/00 5:00 AM,1/1/00 6:00 AM,1/1/00 7:00 AM,1/1/00 8:00 AM,1/1/00 9:00 AM,1/1/00 10:00 AM,1/1/00 11:00 AM,1/1/00 12:00 PM,1/1/00 1:00 PM,1/1/00 2:00 PM,1/1/00 3:00 PM,1/1/00 4:00 PM,1/1/00 5:00 PM,1/1/00 6:00 PM,1/1/00 7:00 PM,1/1/00 8:00 PM,1/1/00 9:00 PM,1/1/00 10:00 PM,1/1/00 11:00 PM,1/2/00 12:00 AM,1/2/00 1:00 AM,1/2/00 2:00 AM,1/2/00 3:00 AM,NA"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc3.range("K14").validation({
        comparerType: "list",
        dataType: "list",
        from: '"1/0/00 12:00 AM,1/0/00 1:00 AM,1/0/00 2:00 AM,1/0/00 3:00 AM,1/0/00 4:00 AM,1/0/00 5:00 AM,1/0/00 6:00 AM,1/0/00 7:00 AM,1/0/00 8:00 AM,1/0/00 9:00 AM,1/0/00 10:00 AM,1/0/00 11:00 AM,1/0/00 12:00 PM,1/0/00 1:00 PM,1/0/00 2:00 PM,1/0/00 3:00 PM,1/0/00 4:00 PM,1/0/00 5:00 PM,1/0/00 6:00 PM,1/0/00 7:00 PM,1/0/00 8:00 PM,1/0/00 9:00 PM,1/0/00 10:00 PM,1/0/00 11:00 PM,1/1/00 12:00 AM,1/1/00 1:00 AM,1/1/00 2:00 AM,1/1/00 3:00 AM,1/1/00 4:00 AM,1/1/00 5:00 AM,1/1/00 6:00 AM,1/1/00 7:00 AM,1/1/00 8:00 AM,1/1/00 9:00 AM,1/1/00 10:00 AM,1/1/00 11:00 AM,1/1/00 12:00 PM,1/1/00 1:00 PM,1/1/00 2:00 PM,1/1/00 3:00 PM,1/1/00 4:00 PM,1/1/00 5:00 PM,1/1/00 6:00 PM,1/1/00 7:00 PM,1/1/00 8:00 PM,1/1/00 9:00 PM,1/1/00 10:00 PM,1/1/00 11:00 PM,1/2/00 12:00 AM,1/2/00 1:00 AM,1/2/00 2:00 AM,1/2/00 3:00 AM,NA"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc3.range("F20").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Ohms, Miliohms"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc3.range("H20").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Ohms, Miliohms"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc3.range("J20").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Ohms, Miliohms"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });
    /*   ***CF3***/






    /*   ***CR1***/

    sheetc3.range("E10").validation({
        comparerType: "list",
        dataType: "list",
        from: '"1/0/00 12:00 AM,1/0/00 1:00 AM,1/0/00 2:00 AM,1/0/00 3:00 AM,1/0/00 4:00 AM,1/0/00 5:00 AM,1/0/00 6:00 AM,1/0/00 7:00 AM,1/0/00 8:00 AM,1/0/00 9:00 AM,1/0/00 10:00 AM,1/0/00 11:00 AM,1/0/00 12:00 PM,1/0/00 1:00 PM,1/0/00 2:00 PM,1/0/00 3:00 PM,1/0/00 4:00 PM,1/0/00 5:00 PM,1/0/00 6:00 PM,1/0/00 7:00 PM,1/0/00 8:00 PM,1/0/00 9:00 PM,1/0/00 10:00 PM,1/0/00 11:00 PM,1/1/00 12:00 AM,1/1/00 1:00 AM,1/1/00 2:00 AM,1/1/00 3:00 AM,1/1/00 4:00 AM,1/1/00 5:00 AM,1/1/00 6:00 AM,1/1/00 7:00 AM,1/1/00 8:00 AM,1/1/00 9:00 AM,1/1/00 10:00 AM,1/1/00 11:00 AM,1/1/00 12:00 PM,1/1/00 1:00 PM,1/1/00 2:00 PM,1/1/00 3:00 PM,1/1/00 4:00 PM,1/1/00 5:00 PM,1/1/00 6:00 PM,1/1/00 7:00 PM,1/1/00 8:00 PM,1/1/00 9:00 PM,1/1/00 10:00 PM,1/1/00 11:00 PM,1/2/00 12:00 AM,1/2/00 1:00 AM,1/2/00 2:00 AM,1/2/00 3:00 AM,NA"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc3.range("E13").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Cuadratica, Exponencial"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc3.range("K10").validation({
        comparerType: "list",
        dataType: "list",
        from: '"1/0/00 12:00 AM,1/0/00 1:00 AM,1/0/00 2:00 AM,1/0/00 3:00 AM,1/0/00 4:00 AM,1/0/00 5:00 AM,1/0/00 6:00 AM,1/0/00 7:00 AM,1/0/00 8:00 AM,1/0/00 9:00 AM,1/0/00 10:00 AM,1/0/00 11:00 AM,1/0/00 12:00 PM,1/0/00 1:00 PM,1/0/00 2:00 PM,1/0/00 3:00 PM,1/0/00 4:00 PM,1/0/00 5:00 PM,1/0/00 6:00 PM,1/0/00 7:00 PM,1/0/00 8:00 PM,1/0/00 9:00 PM,1/0/00 10:00 PM,1/0/00 11:00 PM,1/1/00 12:00 AM,1/1/00 1:00 AM,1/1/00 2:00 AM,1/1/00 3:00 AM,1/1/00 4:00 AM,1/1/00 5:00 AM,1/1/00 6:00 AM,1/1/00 7:00 AM,1/1/00 8:00 AM,1/1/00 9:00 AM,1/1/00 10:00 AM,1/1/00 11:00 AM,1/1/00 12:00 PM,1/1/00 1:00 PM,1/1/00 2:00 PM,1/1/00 3:00 PM,1/1/00 4:00 PM,1/1/00 5:00 PM,1/1/00 6:00 PM,1/1/00 7:00 PM,1/1/00 8:00 PM,1/1/00 9:00 PM,1/1/00 10:00 PM,1/1/00 11:00 PM,1/2/00 12:00 AM,1/2/00 1:00 AM,1/2/00 2:00 AM,1/2/00 3:00 AM,NA"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc3.range("K12").validation({
        comparerType: "list",
        dataType: "list",
        from: '"1/0/00 12:00 AM,1/0/00 1:00 AM,1/0/00 2:00 AM,1/0/00 3:00 AM,1/0/00 4:00 AM,1/0/00 5:00 AM,1/0/00 6:00 AM,1/0/00 7:00 AM,1/0/00 8:00 AM,1/0/00 9:00 AM,1/0/00 10:00 AM,1/0/00 11:00 AM,1/0/00 12:00 PM,1/0/00 1:00 PM,1/0/00 2:00 PM,1/0/00 3:00 PM,1/0/00 4:00 PM,1/0/00 5:00 PM,1/0/00 6:00 PM,1/0/00 7:00 PM,1/0/00 8:00 PM,1/0/00 9:00 PM,1/0/00 10:00 PM,1/0/00 11:00 PM,1/1/00 12:00 AM,1/1/00 1:00 AM,1/1/00 2:00 AM,1/1/00 3:00 AM,1/1/00 4:00 AM,1/1/00 5:00 AM,1/1/00 6:00 AM,1/1/00 7:00 AM,1/1/00 8:00 AM,1/1/00 9:00 AM,1/1/00 10:00 AM,1/1/00 11:00 AM,1/1/00 12:00 PM,1/1/00 1:00 PM,1/1/00 2:00 PM,1/1/00 3:00 PM,1/1/00 4:00 PM,1/1/00 5:00 PM,1/1/00 6:00 PM,1/1/00 7:00 PM,1/1/00 8:00 PM,1/1/00 9:00 PM,1/1/00 10:00 PM,1/1/00 11:00 PM,1/2/00 12:00 AM,1/2/00 1:00 AM,1/2/00 2:00 AM,1/2/00 3:00 AM,NA"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc3.range("K14").validation({
        comparerType: "list",
        dataType: "list",
        from: '"1/0/00 12:00 AM,1/0/00 1:00 AM,1/0/00 2:00 AM,1/0/00 3:00 AM,1/0/00 4:00 AM,1/0/00 5:00 AM,1/0/00 6:00 AM,1/0/00 7:00 AM,1/0/00 8:00 AM,1/0/00 9:00 AM,1/0/00 10:00 AM,1/0/00 11:00 AM,1/0/00 12:00 PM,1/0/00 1:00 PM,1/0/00 2:00 PM,1/0/00 3:00 PM,1/0/00 4:00 PM,1/0/00 5:00 PM,1/0/00 6:00 PM,1/0/00 7:00 PM,1/0/00 8:00 PM,1/0/00 9:00 PM,1/0/00 10:00 PM,1/0/00 11:00 PM,1/1/00 12:00 AM,1/1/00 1:00 AM,1/1/00 2:00 AM,1/1/00 3:00 AM,1/1/00 4:00 AM,1/1/00 5:00 AM,1/1/00 6:00 AM,1/1/00 7:00 AM,1/1/00 8:00 AM,1/1/00 9:00 AM,1/1/00 10:00 AM,1/1/00 11:00 AM,1/1/00 12:00 PM,1/1/00 1:00 PM,1/1/00 2:00 PM,1/1/00 3:00 PM,1/1/00 4:00 PM,1/1/00 5:00 PM,1/1/00 6:00 PM,1/1/00 7:00 PM,1/1/00 8:00 PM,1/1/00 9:00 PM,1/1/00 10:00 PM,1/1/00 11:00 PM,1/2/00 12:00 AM,1/2/00 1:00 AM,1/2/00 2:00 AM,1/2/00 3:00 AM,NA"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc3.range("F20").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Ohms, Miliohms"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });


    sheetc3.range("H20").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Ohms, Miliohms"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });

    sheetc3.range("J20").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Ohms, Miliohms"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });
    /*   ***CF3***/

    // document.querySelector('.k-button k-button-icon').click();
    //spreadsheet._workbook.fileName = $("#TipoEnfriamiento").val() + " " + $("#NoSerie").val() + ".xlsx";
    //spreadsheet._workbook._sheets[1] = sheetc1;
    spreadsheet.saveAsExcel();



    //sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ":S46").value(viewModel.ClaveIdioma == 'ES' ? 'Seleccione...' : 'Select...')
    //sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ":S46").enable(true)



    //sheet.addDrawing(drawing);
    //initCss();
}


function base64ToArrayBuffer(base64) {
    var binaryString = window.atob(base64);
    var binaryLen = binaryString.length;
    var bytes = new Uint8Array(binaryLen);
    for (var i = 0; i < binaryLen; i++) {
        var ascii = binaryString.charCodeAt(i);
        bytes[i] = ascii;
    }
    return bytes;
}
function LoadExcel(nameFile, File) {
    var byteCharacters = atob(File);
    var byteNumbers = new Array(byteCharacters.length);
    for (var i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    var byteArray = new Uint8Array(byteNumbers);
    var file = new Blob([byteArray], { type: 'application/x-excel' });
    var fileURL = URL.createObjectURL(file);
    window.open(fileURL);
    window.close();
}
function generaDescargablePdf(data, name) {
    var arrBuffer = base64ToArrayBuffer(data);
    // It is necessary to create a new blob object with mime-type explicitly set
    // otherwise only Chrome works like it should 
    var newBlob = new Blob([arrBuffer], { type: "application/vnd.ms-excel" });
    // IE doesn't allow using a blob object directly as link href
    // instead it is necessary to use msSaveOrOpenBlob 
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
        window.navigator.msSaveOrOpenBlob(newBlob);
        return;
    }
    // For other browsers:
    // Create a link pointing to the ObjectURL containing the blob. 
    var data = window.URL.createObjectURL(newBlob);
    var link = document.createElement('a');
    document.body.appendChild(link);
    //required in FF, optional for Chrome 
    link.href = data;
    link.download = name;
    link.click();
    window.URL.revokeObjectURL(data);
    link.remove();
}


async function DownloadFileJSON() {
    var path = path = "/EtdFile/DownloadFile/";

    var url = new URL(domain + path),
        params = {
            noSerie: $("#NoSerie").val(),
            clavePrueba: "",
            claveIdioma: $("#claveIdiomaD").val(),
            posAT: $("#selectAT").val(),
            posBT: $("#selectBT").val(),
            posTer: $("#selectTer").val(),
            coolingType: $("#TipoEnfriamiento").val(),
            otherCoolingType: $("#Otro").val(),
            capacity: $("#SelectCapacidades").val(),
            otherCapacity: $("#OtraCapacidad").val(),
            altitud1: $("#Altitud1").val(),
            altitud2: $("#Altitud2").val(),
            clientName: $("#Cliente").val(),
            reportCapacities: $("#CapacidadReporte").val(),
            grados: 0
        }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const result = await response.json();
        return result;
    }

    else {
        ShowFailedMessage('Error, por favor contacte al administrador del sistema.' + response);
        return null;
    }
}
async function LoadFileJSON() {
    var fileUpload = $("#File").get(0);

    var files = fileUpload.files;

    modelFormData = new FormData();

    modelFormData.append("NoSerie", $("#NoSerie").val() );
    modelFormData.append("ClaveIdioma2", $("#ClaveIdioma2").val());
    modelFormData.append("clavePrueba", "");
    modelFormData.append("Enfriamiento", $("#Enfriamiento2").val());
    modelFormData.append("OtroEnfriamiento", $("#Otro2").val());
    modelFormData.append("Check1", $("#check1").is(":checked"));
    modelFormData.append("Check2", $("#check2").is(":checked"));
    modelFormData.append("Check3", $("#check3").is(":checked"));
    var fileUpload = $("#File").get(0);

    var files = fileUpload.files;

    if (files.length > 0)
        modelFormData.append("file", files[0]);

    $.ajax({
        url: "/EtdFile/LoadFile/",
        method: "POST",
        contentType: false,
        processData: false,
        data: modelFormData,
        success: function (result) {

            if (result.response.Code === 1) {
                ShowSuccessMessage("Guardado Exitoso.")
                GetGraphisByReport();
            }
            else {
                ShowFailedMessage(result.response.Description);
                $("#loader").css("display", "none");
                var menssajes = "";
                $.each(result.response.Structure, function (index, value) {
                    menssajes += value.Message + "\n";
                });
                $("#errores").val(menssajes);

          
            }
        }
    });


    //var path = path = "/EtdFile/LoadFile/";

    //var url = new URL(domain + path),
    //    params = {
    //        noSerie: $("#NoSerie").val(),
    //        claveIdioma: $("#ClaveIdioma2").val(),
    //        clavePrueba  : " ",
    //        coolingType: $("#TipoEnfriamiento2").val(),
    //        otherCoolingType: $("#Otro2").val(),
    //        f1: $("#check1").val(),
    //        f2: $("#check2").val(),
    //        f3: $("#check3").val(),
    //        file: files[0]
    //    }
    //Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    //const response = await fetch(url);

    //if (response.ok && response.status === 200) {

    //    const result = await response.json();
    //    return result;
    //}

    //else {
    //    ShowFailedMessage('Error, por favor contacte al administrador del sistema.' + response);
    //    return null;
    //}
}

//Requests
function changeCssTap(valor) {
    currentTap = valor.toString()
    
    //$("#btnClear").trigger("click")
}

$("#btnRemove").on('click', function (e) {
    inputFile.value =''
})


function ValidateForm(tipo) {
    if (tipo === "1") {
        $(`#OtroSpand`).text("");
        $(`#OtraCapacidad`).text("");
        var resultValid = false;
        ResultValidations = $("#form_menu_sup1").data("kendoValidator").validate();
        if (ResultValidations) {
            resultValid =  true;
        } else {
            resultValid = false;
        }

        if ($("#TipoEnfriamiento").val() != "") {
            if ($("#TipoEnfriamiento").val() == "0") {
                if ($("#Otro").val() == "" || $("#Otro").val() == null || $("#Otro").val() == undefined) {
                    $(`#OtroSpand`).text("Requerido");
                    resultValid = false;
                }


            }
        }
        else {
            resultValid = false;
        } 

        if ($("#SelectCapacidades").val() != "") {
            if ($("#SelectCapacidades").val() == "Otro") {
                if ($("#OtraCapacidad").val() == "" || $("#OtraCapacidad").val() == null || $("#OtraCapacidad").val() == undefined) {
                    $(`#OtraCapacidadSpand`).text("Requerido");
                    resultValid = false;
                }


            }
        }
        else {
            resultValid = false;
        } 
        return resultValid;
     
    } else if (tipo === "2") {
        $(`#Otro2Spand`).text("");
        var resultValid = false;
        ResultValidations = $("#form_menu_sup2").data("kendoValidator").validate();
        if (ResultValidations) {
            resultValid = true;
        } else {
            resultValid = false;
        }

        if ($("#TipoEnfriamiento2").val() != "") {
            if ($("#TipoEnfriamiento2").val() == "0") {
                if ($("#Otro").val() == "" || $("#Otro").val() == null || $("#Otro").val() == undefined) {
                    $(`#Otro2Spand`).text("Requerido");
                    resultValid = false;
                }

            }
        }
        else {
            resultValid = false;
        } 
        return resultValid;
    } else {
        return resultValid;
    }
 
}

async function GetConfigurationFilesJSON(module) {

    var path = '/BaseTemplate/GetConfigurationFiles';

    var url = new URL(domain + path),
        params = {
            pIdModule: module
        }

    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const data = await response.json();
        return data;
    }

    else {

        return null;
    }


}
$("#TipoEnfriamiento").on("change", async function () {
    $("#Otro").val("");
    if ($("#TipoEnfriamiento").val() == "0") {
     
        $("#Otro").prop("disabled", false);
    }
    else {

        $("#Otro").prop("disabled", true);
    }

})

$("#TipoEnfriamiento2").on("change", async function () {

    $("#Otro2").val("");
    if ($("#TipoEnfriamiento2").val() == "0") {

        $("#Otro2").prop("disabled", false);
    }
    else {

        $("#Otro2").prop("disabled", true);
    }

})




$("#File").on("change", async function () {

    $('#FileSpan').text("");
    $("#FileSpan").removeClass('k-hidden');

    banderaFileErrorExtension = false;
    banderaFileErrorPeso = false;
    var fileUpload = $("#File").get(0);

    var files = fileUpload.files;

    if (files.length > 0) {
        let nameFile = files[0].name;

        var extension = "." + nameFile.substr((nameFile.lastIndexOf('.') + 1));
        $("#loader").css("display", "initial");
        await GetConfigurationFilesJSON(3).then(
            data => {
                if (data.response.Structure !== null && data.response.Structure !== undefined) {
                    console.log(data.response.Structure);
                    ConfigurationFiles = data.response.Structure;
                    for (var i = 0; i < data.response.Structure.length; i++) {
                        if (extension == data.response.Structure[i].ExtensionArchivoNavigation.Extension) {
                            banderaFileErrorExtension = true;
                            if (files[0].size < (data.response.Structure[i].MaximoPeso * 1024 * 1024)) {
                                pesoMaximo = data.response.Structure[i].MaximoPeso * 1024 * 1024;
                                banderaFileErrorPeso = true;
                            }
                            break;
                        }
                    }

                    if (!banderaFileErrorExtension) {
                        $("#FileSpan").removeClass('k-hidden');
                        $('#FileSpan').text("Extensión de archivo no permitida");
                    } else if (!banderaFileErrorPeso) {
                        $("#FileSpan").removeClass('k-hidden');
                        $('#FileSpan').text("El peso máximo para el tipo de archivo " + extension + " es: " + pesoMaximo);
                    }



                }
            }
        );
        //await sleep(5000)
        $("#loader").css("display", "none");
    }


})



async function GetFilter() {
    var path = path = "/EtdFile/GetFilter/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const ResistanceTwentyDegrees = await response.json();
        return ResistanceTwentyDegrees;
    }

    else {
        ShowFailedMessage('Error, por favor contacte al administrador del sistema.');
        return null;
    }
}







async function GetResistanceTwentyDegreesJSON(isNewTensionResponse) {
    var path = path = "/ResistanceTwentyDegrees/Get/";
    var url = new URL(domain + path),
        params = {
            noSerie: noSerieInput.value,
            measuring: unitMeasuringInput.options[unitMeasuringInput.selectedIndex].text,
            temperature: parseFloat(temperatureInput.value),
            newTensonResponse: isNewTensionResponse,
            requestInicial: requestInicial
        }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const ResistanceTwentyDegrees = await response.json();
        return ResistanceTwentyDegrees;
    }

    else {
        ShowFailedMessage('Error, por favor contacte al administrador del sistema.');
        return null;
    }
}


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


function ClearForm() {

    noSerieInput.disabled = false;
    noSerieInput.value = '';
    btnSave.disabled = true;
    unitMeasuringInput.disabled = false;
    unitMeasuringInput.value = '';
    temperatureInput.disabled = false;
    temperatureInput.value = 0;
   // btnRequest.disabled = false;

    /*var tabStrip = $("#tabstrip").kendoTabStrip().data("kendoTabStrip");
    tabStrip.remove($('#tabstrip'));

    $(".k-tabstrip-wrapper").append(html);

    $("#tabstrip").kendoTabStrip({
        select: onSelect,
        animation: {
            open: {
                effects: "fadeIn"
            }
        }
    });*/
}