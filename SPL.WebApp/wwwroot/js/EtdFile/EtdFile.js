
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
let claveIdiomaInput2 = document.getElementById("claveIdiomaC");
btnDown.disabled = true
btnUp.disabled = true
let currentTap = "1";
let spreadsheetElement;





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
    CapacidadesList

    $("#Otro2").val("")
    $("#Otro2").prop("disabled", true)

    $("#selectTer").prop("disabled", true)
    $("#selectBT").prop("disabled", true)
    $("#selectAT").prop("disabled", true)

    claveIdiomaInput.value = ''
    claveIdiomaInput2.value = ''
    btnRequest.disabled = false
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

function LoadForm(response) {
/*    if (currentTap === "1") {*/
    claveIdiomaInput.value = response.ClaveIdioma;

            $("#SelectCapacidades option[value!='']").each(function () {
                $(this).remove();
            });
        $.each(response.CapacidadesList, function (i, val) {
            $("#SelectCapacidades").append("<option value='" + val + "'>" + val + "</option>");
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

    
    spreadsheetElement = $("#spreadsheet").kendoSpreadsheet({
        sheets: workbook.sheets,
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
    //var range = spreadsheet.activeSheet().range("A1:T200");
    //range.enable(false);

    // validates if the formula returns a non-false value (see the `from` field).

    //var drawing = kendo.spreadsheet.Drawing.fromJSON({
    //    topLeftCell: "A1",
    //    offsetX: 0,
    //    offsetY: 0,
    //    width: 220,
    //    height: 43,
    //    image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    //});

    //sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Fecha').Celda).enable(true)
    //sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NoSerie').Celda).value(viewModel.NoSerie)

    //if (viewModel.Pruebas === "LYP") {

    //    if (viewModel.TerciarioOSegunda === "CT" && viewModel.Columnas === 3) {
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).
    //            value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).value().replace('?', ''));

    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 2).Celda).
    //            value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 2).Celda).value().replace('?', ''));

    //    }

    //    if (viewModel.TerciarioOSegunda === "2B" && viewModel.Columnas === 3) {
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 1).Celda).
    //            value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).value());


    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).
    //            value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).value().replace('?', '1'));

    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 1).Celda).
    //            value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 1).Celda).value().replace('?', '2'));


    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 2).Celda).
    //            value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 2).Celda).value());


    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 2).Celda).
    //            value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 2).Celda).value().replace('?', '1'));

    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 2).Celda).
    //            value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 2).Celda).value().replace('?', '2'));
    //    }



    //    if (viewModel.TerciarioOSegunda !== "CT" && viewModel.TerciarioOSegunda !== "2B") {
    //        spreadsheet.activeSheet().range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTerciario' && x.Seccion === 2).Celda).value("");
    //        spreadsheet.activeSheet().range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTerciario' && x.Seccion === 1).Celda).value("");
    //    } else {

    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Terciario' && x.Seccion === 1).Celda).value(viewModel.TerciarioLab)
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Terciario' && x.Seccion === 2).Celda).value(viewModel.TerciarioEmp)
    //    }

    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NivelAceite' && x.Seccion === 1).Celda).value(viewModel.NivelAceiteLab)
    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NivelAceite' && x.Seccion === 2).Celda).value(viewModel.NivelAceiteEmp)

    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Boquillas' && x.Seccion === 1).Celda).value(viewModel.BoquillasLab)
    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Boquillas' && x.Seccion === 2).Celda).value(viewModel.BoquillasEmp)

    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NucleoHerrajes' && x.Seccion === 1).Celda).value(viewModel.NucleosLab)
    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NucleoHerrajes' && x.Seccion === 2).Celda).value(viewModel.NucleosEmp)

    //    if (viewModel.NivelAceiteLab === "Vacío" || viewModel.NivelAceiteLab === "Empty") {
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 1).Celda + ":P21").borderBottom(null);
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTempAceite' && x.Seccion === 1).Celda).value("");
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'UMTempAceite' && x.Seccion === 1).Celda).value("");
    //    }
    //    else {
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite').Celda).validation({
    //            comparerType: "custom",
    //            dataType: "custom",
    //            from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite').Celda + ', "^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,1})?%?$"))',
    //            titleTemplate: "Error",
    //            showButton: true,
    //            type: "reject",
    //            allowNulls: false,
    //            messageTemplate: "La temperatura del aceite debe ser mayor a cero considerando 3 enteros con 1 decimal."
    //        });
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite').Celda + ":P21").enable(true)
    //    }

    //    if (viewModel.NivelAceiteEmp === "Vacío" || viewModel.NivelAceiteEmp === "Empty") {
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 2).Celda + ":P35").borderBottom(null);
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTempAceite' && x.Seccion === 2).Celda).value("");
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'UMTempAceite' && x.Seccion === 2).Celda).value("");
    //    } else {
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 2).Celda).validation({
    //            comparerType: "custom",
    //            dataType: "custom",
    //            from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 2).Celda + ', "^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,1})?%?$"))',
    //            titleTemplate: "Error",
    //            showButton: true,
    //            type: "reject",
    //            allowNulls: false,
    //            messageTemplate: "La temperatura del aceite debe ser mayor a cero considerando 3 enteros con 1 decimal."
    //        });
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 2).Celda + ":P35").enable(true)

    //    }

    //}
    //else {
    //    if (viewModel.TerciarioOSegunda === "CT" && viewModel.Columnas === 3) {
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).
    //            value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).value().replace('?', ''));
    //    }

    //    if (viewModel.TerciarioOSegunda === "2B" && viewModel.Columnas === 3) {
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 1).Celda).
    //            value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).value());


    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).
    //            value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosBT' && x.Seccion === 1).Celda).value().replace('?', '1'));

    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 1).Celda).
    //            value(sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 1).Celda).value().replace('?', '2'));
    //    }

    //    if (viewModel.Pruebas === "LAB") {
    //        if (viewModel.TerciarioOSegunda !== "CT" && viewModel.TerciarioOSegunda !== "2B") {
    //            spreadsheet.activeSheet().range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTerciario' && x.Seccion === 1).Celda).value("");
    //        } else {
    //            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Terciario' && x.Seccion === 1).Celda).value(viewModel.TerciarioLab)
    //        }

    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NivelAceite' && x.Seccion === 1).Celda).value(viewModel.NivelAceiteLab)
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Boquillas' && x.Seccion === 1).Celda).value(viewModel.BoquillasLab)
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NucleoHerrajes' && x.Seccion === 1).Celda).value(viewModel.NucleosLab)


    //        if (viewModel.NivelAceiteLab === "Vacío" || viewModel.NivelAceiteLab === "Empty") {
    //            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 1).Celda + ":P21").borderBottom(null);
    //            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTempAceite' && x.Seccion === 1).Celda).value("");
    //            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'UMTempAceite' && x.Seccion === 1).Celda).value("");
    //        }
    //        else {
    //            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite').Celda).validation({
    //                comparerType: "custom",
    //                dataType: "custom",
    //                from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite').Celda + ', "^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,1})?%?$"))',
    //                titleTemplate: "Error",
    //                showButton: true,
    //                type: "reject",
    //                allowNulls: false,
    //                messageTemplate: "La temperatura del aceite debe ser mayor a cero considerando 3 enteros con 1 decimal."
    //            });
    //            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite').Celda + ":P21").enable(true)
    //        }
    //    }

    //    if (viewModel.Pruebas === "PLA") {
    //        if (viewModel.TerciarioOSegunda !== "CT" && viewModel.TerciarioOSegunda !== "2B") {
    //            spreadsheet.activeSheet().range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTerciario' && x.Seccion === 1).Celda).value("");
    //        } else {
    //            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Terciario' && x.Seccion === 1).Celda).value(viewModel.TerciarioEmp)
    //        }

    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NivelAceite' && x.Seccion === 1).Celda).value(viewModel.NivelAceiteEmp)
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Boquillas' && x.Seccion === 1).Celda).value(viewModel.BoquillasEmp)
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'NucleoHerrajes' && x.Seccion === 1).Celda).value(viewModel.NucleosEmp)

    //        if (viewModel.NivelAceiteEmp === "Vacío" || viewModel.NivelAceiteEmp === "Empty") {
    //            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 1).Celda + ":P21").borderBottom(null);
    //            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTempAceite' && x.Seccion === 1).Celda).value("");
    //            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'UMTempAceite' && x.Seccion === 1).Celda).value("");
    //        } else {
    //            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 1).Celda).validation({
    //                comparerType: "custom",
    //                dataType: "custom",
    //                from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 1).Celda + ', "^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,1})?%?$"))',
    //                titleTemplate: "Error",
    //                showButton: true,
    //                type: "reject",
    //                allowNulls: false,
    //                messageTemplate: "La temperatura del aceite debe ser mayor a cero considerando 3 enteros con 1 decimal."
    //            });
    //            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TempAceite' && x.Seccion === 1).Celda + ":P21").enable(true)

    //        }
    //    }

    //}


    //if (viewModel.Pruebas === "LYP") {
    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda).validation({
    //        comparerType: "custom",
    //        dataType: "custom",
    //        from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda +
    //            ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda +
    //            ')<=5)',
    //        titleTemplate: "Error",
    //        showButton: true,
    //        type: "reject",
    //        allowNulls: false,
    //        messageTemplate: "La posición en AT solo puede contener letras y/o números y no puede excederse de 5 caracteres."
    //    });

    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 2).Celda).validation({
    //        comparerType: "custom",
    //        dataType: "custom",
    //        from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 2).Celda +
    //            ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 2).Celda +
    //            ')<=5)',
    //        titleTemplate: "Error",
    //        showButton: true,
    //        type: "reject",
    //        allowNulls: false,
    //        messageTemplate: "La posición en AT solo puede contener letras y/o números y no puede excederse de 5 caracteres."
    //    });



    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda).validation({
    //        comparerType: "custom",
    //        dataType: "custom",
    //        from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda +
    //            ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda +
    //            ')<=5)',
    //        titleTemplate: "Error",
    //        showButton: true,
    //        type: "reject",
    //        allowNulls: false,
    //        messageTemplate: "La posición en BT solo puede contener letras y/o números y no puede excederse de 5 caracteres."
    //    });
    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 2).Celda).validation({
    //        comparerType: "custom",
    //        dataType: "custom",
    //        from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 2).Celda +
    //            ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 2).Celda +
    //            ')<=5)',
    //        titleTemplate: "Error",
    //        showButton: true,
    //        type: "reject",
    //        allowNulls: false,
    //        messageTemplate: "La posición en BT solo puede contener letras y/o números y no puede excederse de 5 caracteres."
    //    });

    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda).enable(true)
    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 2).Celda).enable(true)
    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda).enable(true)
    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 2).Celda).enable(true)


    //    if (viewModel.Columnas === 3) {
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda).validation({
    //            comparerType: "custom",
    //            dataType: "custom",
    //            from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda +
    //                ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda +
    //                ')<=5)',
    //            titleTemplate: "Error",
    //            showButton: true,
    //            type: "reject",
    //            allowNulls: false,
    //            messageTemplate: "La posición en Ter solo puede contener letras y/o números y no puede excederse de 5 caracteres."
    //        });
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 2).Celda).validation({
    //            comparerType: "custom",
    //            dataType: "custom",
    //            from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 2).Celda +
    //                ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 2).Celda +
    //                ')<=5)',
    //            titleTemplate: "Error",
    //            showButton: true,
    //            type: "reject",
    //            allowNulls: false,
    //            messageTemplate: "La posición en Ter solo puede contener letras y/o números y no puede excederse de 5 caracteres."
    //        });
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda).enable(true)
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 2).Celda).enable(true)
    //    }


    //}
    //else {

    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda).validation({
    //        comparerType: "custom",
    //        dataType: "custom",
    //        from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda +
    //            ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda +
    //            ')<=5)',
    //        titleTemplate: "Error",
    //        showButton: true,
    //        type: "reject",
    //        allowNulls: false,
    //        messageTemplate: "La posición en AT solo puede contener letras y/o números y no puede excederse de 5 caracteres."
    //    });
    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda).validation({
    //        comparerType: "custom",
    //        dataType: "custom",
    //        from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda +
    //            ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda +
    //            ')<=5)',
    //        titleTemplate: "Error",
    //        showButton: true,
    //        type: "reject",
    //        allowNulls: false,
    //        messageTemplate: "La posición en BT solo puede contener letras y/o números y no puede excederse de 5 caracteres."
    //    });
    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosAT' && x.Seccion === 1).Celda).enable(true)
    //    sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosBT' && x.Seccion === 1).Celda).enable(true)


    //    if (viewModel.Columnas === 3) {
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda).validation({
    //            comparerType: "custom",
    //            dataType: "custom",
    //            from: 'AND(REGEXP_MATCH(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda +
    //                ', "^[a-zA-Z0-9]*$"),LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda +
    //                ')<=5)',
    //            titleTemplate: "Error",
    //            showButton: true,
    //            type: "reject",
    //            allowNulls: false,
    //            messageTemplate: "La posición en Ter solo puede contener letras y/o números y no puede excederse de 5 caracteres."
    //        });
    //        sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda).enable(true)

    //        if (viewModel.TerciarioDisponible === 'No') {
    //            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'PosTer' && x.Seccion === 1).Celda).clear();
    //            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitPosTer' && x.Seccion === 1).Celda).clear();
    //            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Terciario' && x.Seccion === 1).Celda).clear();
    //            sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TitTerciario' && x.Seccion === 1).Celda).clear();
    //        }
    //    }

    //}



    //sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TotalPaginas').Celda).validation({
    //    comparerType: "between",
    //    dataType: "number",
    //    from: '1',
    //    to: '999',
    //    titleTemplate: "Error",
    //    showButton: true,
    //    type: "reject",
    //    allowNulls: false,
    //    messageTemplate: "El total de páginas debe ser mayor a cero considerando 3 enteros sin decimales"
    //});
    //sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'TotalPaginas').Celda).enable(true)


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
    sheet.range("B4").validation({
        comparerType: "list",
        dataType: "list",
        from: '"Aceptado,Rechazado"',
        /* from: 'AND(CHECK_RESULT(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ',"' + viewModel.ClaveIdioma + '"), LEN(' + viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ')<=15)',*/
        showButton: true,
        type: "reject",
        allowNulls: false,
    });
    //sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ":S46").value(viewModel.ClaveIdioma == 'ES' ? 'Seleccione...' : 'Select...')
    //sheet.range(viewModel.SettingsARF.ConfigurationReports.find(x => x.Dato === 'Resultado').Celda + ":S46").enable(true)



    sheet.addDrawing(drawing);
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
            capacity: $("#selectCapacidades").val(),
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
    btnRequest.disabled = false;

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