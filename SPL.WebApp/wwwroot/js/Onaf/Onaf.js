let viewModel;
let btnRequest2
let btnClear2;
let btnSave2;
let btnFile;
let noSerieInput2;
let modelFormData;
var banderaFileErrorPeso = false;
var banderaFileErrorExtension = false;
let noSerieInput = document.getElementById("NoSerie");
let tipoInformacion = document.getElementById("TipoInformacion");
let fecha = document.getElementById("Fecha");
let altura = document.getElementById("Altura");
let btnSave = document.getElementById("btnSave");
let btnClear = document.getElementById("btnClear");
let btnConsult = document.getElementById("btnConsult");
let btnLoad = document.getElementById("btnLoad");
let aceptaReemplazarData = false
//fecha.value = '2014-02-24'

altura.disabled = true;
fecha.disabled = true;


$("#btnObtener").click(async function () {
    await consult()
})

$("#btnLoad").click(async function () {
    aceptaReemplazarData = false
    $.ajax({
        type: "POST",
        url: domain + "/Onaf/LoadPartial",
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            $("#row-modal").html(response);
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

    await sleep()
    modalAddBoq.show();
    noSerieInput2 = document.getElementById("NoSerie2");
    btnSave2 = document.getElementById("btnSave2");
    btnSave2.disabled = true
    btnClear2 = document.getElementById("btnClear2");
    btnRequest2 = document.getElementById("btnCon2");
    btnSave2.disabled = true

    btnFile = document.getElementById("File");


    $("#btnSave2").click(async function () {
        $("#loader2").css("display", "initial");
        var fileUpload = $("#File").get(0);
        var files = fileUpload.files;

        if (files[0] === undefined) {
            $("#loader2").css("display", "none");
            ShowFailedMessage("Debe seleccionar un archivo")
            return
        }


        if (banderaFileErrorExtension && banderaFileErrorPeso) {
            modelFormData = new FormData();
            modelFormData.append("NoSerie", noSerieInput2.value);
            modelFormData.append("IsFromFile", true);
            modelFormData.append("AcceptaReemplazarData", aceptaReemplazarData);

            if (files.length > 0)
                modelFormData.append("File", files[0]);


            var ResultValidations = $("#form_base_template").data("kendoValidator").validate();

            if (ResultValidations) {
                if (!banderaFileErrorExtension) {
                    $("#FileSpan").removeClass('k-hidden');
                    $('#FileSpan').text("Extensión de archivo no permitida");
                    return;
                } else if (!banderaFileErrorPeso) {
                    $("#FileSpan").removeClass('k-hidden');
                    $('#FileSpan').text("El peso máximo para el tipo de archivo " + extension + " es: " + pesoMaximo);
                    return;
                }
                btnSave2.disabled = true
                $("#loader2").css("display", "block");
                $.ajax({
                    url: "/Onaf/Save/",
                    method: "POST",
                    contentType: false,
                    processData: false,
                    data: modelFormData,
                    success: function (result) {
                        console.log(result.response)
                        if (result.response.Code === 1) {
                            viewModel = result.response.Structure

                            if (viewModel.Lista.length > 0 && viewModel.AcceptaReemplazarData === false) {
                                kendo.confirm(result.response.Description).then(function () {
                                    aceptaReemplazarData = true
                                    $('#btnSave2').trigger('click');
                                    //kendo.alert("You chose the Ok action.");
                                }, function () {
                                    btnSave2.disabled = false
                                    $("#loader2").css("display", "none");
                                });
                            } else {
                                ShowSuccessMessage(result.response.Description, 4500);
                                $("#loader2").css("display", "none");
                                $("#btnClear2").trigger('click')
                                // modalAddBoq.hide();
                            }
                        }
                        else {
                            $("#btnClear2").trigger('click')
                            ShowFailedMessage(result.response.Description);
                            $("#loader2").css("display", "none");
                        }
                        aceptaReemplazarData = false;
                    }
                });
            }
            else {

                $("#loader2").css("display", "none");
                ShowFailedMessage("Faltan campos requeridos");
            }
        }
        else {
            $("#loader2").css("display", "none");
            ShowFailedMessage("Error en peso o extension de archivo seleccionado");
        }
    });

    $("#btnClear2").click(function () {
        noSerieInput2.value = ''
        noSerieInput2.disabled = false
        btnSave2.disabled = true
        btnFile.value = ''
        btnFile.disabled = true
        $(".field-validation-error").text("")

    })

    $("#File").on("change", async function () {
        $("#loader2").css("display", "none");
        $('#FileSpan').text("");
        $("#FileSpan").removeClass('k-hidden');

        banderaFileErrorExtension = false;
        banderaFileErrorPeso = false;
        var fileUpload = $("#File").get(0);

        var files = fileUpload.files;

        if (files.length > 0) {
            let nameFile = files[0].name;

            var extension = "." + nameFile.substr((nameFile.lastIndexOf('.') + 1));
            $("#loader2").css("display", "initial");
            await GetConfigurationFilesJSON(8).then(
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
                                    $("#btnSave2").prop("disabled", false)
                                    //$(".k-invalid-msg").remove()
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
            $("#loader2").css("display", "none");
        }
    })

    $("#NoSerie2").change(async function () {
        $("#loader2").css("display", "initial");
        if (noSerieInput2.value === undefined || noSerieInput2.value === "" || noSerieInput2.value === null) {
            $('#NoSerieSpand2').text("Requerido");
            $("#loader2").css("display", "none");
            return;
        }
        else {
            $('#NoSerieSpand2').text('');
        }

        if (!(/^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ0-9-@]*$/.test(noSerieInput2.value))) {
            $('#NoSerieSpand2').text("Character(es) no permitido.");
            return;
        }
        else {
            $('#NoSerieSpand2').text('');
        }

        $("#loader2").css("display", "block");
        if (noSerieInput2.value) {
            $("#loader2").css("display", "initial");
            GetInfo(noSerieInput2.value).then(
                data => {
                    if (data.response.Code !== -1) {
                        noSerieInput2.disabled = true;
                        viewModel = data.response.Structure;
                        btnClear2.disabled = false;
                        $("#File").prop('disabled', false);
                        $("#btnCon2").prop("disabled", "true")

                    }
                    else {
                        ShowFailedMessage(data.response.Description);
                    }
                    $("#loader2").css("display", "none");
                }
            );
        }
        else {
            $("#loader2").css("display", "none");

            ShowSuccessMessage('Por favor ingrese un No. Serie.');
            btnSave2.disabled = true;
        }

    })
})

btnClear.addEventListener("click", function () {
    btnSave.disabled = true;
    btnConsult.disabled = true;
    btnLoad.disabled = false;
    tipoInformacion.disabled = true;
    noSerieInput.disabled = false
    noSerieInput.value = ''
    tipoInformacion.value = ''
    altura.value = ''
    altura.disabled = true;
    fecha.disabled = true;
    fecha.value = ''
    $("#btnObtener").prop("disabled", true)

    $("#TipoInformacion option[value!='']").each(function () {
        $(this).remove();
    });

    $("#Altura option[value!='']").each(function () {
        $(this).remove();
    });

    if ($('#grid').data().kendoGrid != null) {
        $('#grid').data().kendoGrid.destroy();
        $('#grid').empty();
    }

});

btnSave.addEventListener("click", function () {

    $("#loader").css("display", "block");
    var data = JSON.stringify($("#grid").data("kendoGrid").dataSource.data().map(({
        Hora,
        Altura,
        TipoInfo,
        FechaDatos,
        Creadopor,
        NoSerie,
        D16,
        D20,
        D25,
        D315,
        D40,
        D50,
        D63,
        D80,
        D100,
        D125,
        D160,
        D200,
        D250,
        D3151,
        D400,
        D500,
        D630,
        D800,
        D1000,
        D1250,
        D1600,
        D2000,
        D2500,
        D3150,
        D4000,
        D5000,
        D6300,
        D8000,
        D10000,

    }) => ({
        Hora,
        Altura,
        TipoInfo,
        FechaDatos,
        Creadopor,
        NoSerie,
        D16,
        D20,
        D25,
        D315,
        D40,
        D50,
        D63,
        D80,
        D100,
        D125,
        D160,
        D200,
        D250,
        D3151,
        D400,
        D500,
        D630,
        D800,
        D1000,
        D1250,
        D1600,
        D2000,
        D2500,
        D3150,
        D4000,
        D5000,
        D6300,
        D8000,
        D10000
    })))

    modelFormData = new FormData();
    modelFormData.append("NoSerie", noSerieInput.value);
    modelFormData.append("TipoInformacion", tipoInformacion.value);
    //modelFormData.append("Pasar", viewModel.Pasar); //
    modelFormData.append("DataSource", data);
    modelFormData.append("Altura", altura.value);

    $.ajax({
        url: "/Onaf/Save/",
        method: "POST",
        contentType: false,
        processData: false,
        data: modelFormData,
        success: function (result) {

            if (result.response.Code === 1) {
                viewModel = result.response.Structure

                if ($('#grid').data().kendoGrid != null) {
                    $('#grid').data().kendoGrid.destroy();
                    $('#grid').empty();
                }
                btnClear.click();
                $("#loader").css("display", "none");
                if (!viewModel.Pasar && viewModel.Lista.length > 0) {//.
                    kendo.confirm(result.response.Description).then(function () {
                        viewModel.Pasar = true
                        $('#btnSave2').trigger('click');
                    }, function () {
                        // kendo.alert("You chose to Cancel action.");
                    });


                } else {
                    if ($('#grid').data().kendoGrid != null) {
                        $('#grid').data().kendoGrid.destroy();
                        $('#grid').empty();
                    }
                    ShowSuccessMessage("Guardado Exitoso.");
                    modalAddBoq.hide();
                }
            }
            else {
                $("#loader").css("display", "none");
                ShowFailedMessage(result.response.Description);
            }
        }
    });

});

//$("#NoSerie").focus();

$("#NoSerie").change(async function () {
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

    var split = noSerieInput.value.split('-')
    if (split.length === 1) {
        $(`#NoSerieSpand`).text("Formato invalido");
        return
    }
    else {
        if (split[1] === '') {
            $(`#NoSerieSpand`).text("Formato invalido");
            return
        }
    }

    $("#loader").css("display", "block");
    if (noSerieInput.value) {
        GetInfo(noSerieInput.value).then(
            data => {
                if (data.response.Code !== -1) {
                    //btnSave.disabled = false;
                    noSerieInput.disabled = true;
                    viewModel = data.response.Structure;
                    tipoInformacion.disabled = false
                    altura.disabled = true
                    btnConsult.disabled = false
                    fecha.disabled = false
                    console.log(viewModel)



                    $("#TipoInformacion option[value!='']").each(function () {
                        $(this).remove();
                    });

                    $.each(viewModel.TiposInformacion, function (i, val) {
                        $("#TipoInformacion").append("<option value='" + val.Value + "'>" + val.Text + "</option>");
                    });
                }
                else {
                    ShowFailedMessage(data.response.Description);
                }
                $("#loader").css("display", "none");
            }
        );
    }
    else {
        $("#loader").css("display", "none");

        ShowSuccessMessage('Por favor ingrese un No. Serie.');
        btnSave.disabled = true;
    }
})

async function getAlturas() {
    if (ValidateForm()) {

        $("#loader").css("display", "initial");

        await GetAlturas().then(
            data => {
                if (data.response.Code === 1) {
                    fecha.disabled = true
                    tipoInformacion.disabled = true
                    altura.disabled = false
                    btnConsult.disabled = true

                    $("#Altura option[value!='']").each(function () {
                        $(this).remove();
                    });

                    $.each(data.response.Structure.ListaAlturas, function (i, val) {
                        $("#Altura").append("<option value='" + val + "'>" + val + "</option>");
                    });

                    $("#Altura").prop("selectedIndex", 1)
                    $("#btnObtener").prop("disabled", false)

                } else {
                    ShowFailedMessage(data.response.Description)
                }
            }
        );

        $("#loader").css("display", "none");
    }
    else {
        ShowFailedMessage("Faltan campos por llenar")
        $("#loader").css("display", "none");
    }
}

async function consult() {
    if (ValidateForm()) {

        $("#loader").css("display", "initial");

        await GetInformationOctaves().then(
            data => {
                if (data.response.Code === 1) {
                    btnConsult.disabled = true;
                    btnLoad.disabled = true;
                    fecha.disabled = true
                    tipoInformacion.disabled = true
                    btnSave.disabled = false
                    altura.disabled = true
                    console.log(data)
                    setDataInTable2(data.response.Structure.Lista)
                } else {
                    ShowFailedMessage("No ha sido encontrada información para los filtros seleccionados")
                }
            }
        );

        $("#loader").css("display", "none");
    }
    else {
        ShowFailedMessage("Faltan campos por llenar")
        $("#loader").css("display", "none");
    }
}


async function GetInfo(value) {
    var path = path = "/Onaf/GetInfo/";

    var url = new URL(domain + path),
        params = { noSerie: value }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

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

async function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function setDataInTable2(data) {
    var source = []
    /*source.push(
         {
             Orden: 1,
             Hora: "werwer",
             D16: "465456.245"
         }
     )*/
    //console.log(data)
    var dataSourceTable = new kendo.data.DataSource({
        data: data,
        pageSize: 10,
        schema: {
            model: {
                fields: {
                    Hora: {
                        nullable: false,
                        type: "string",
                        validation: {
                            required: true,
                            horavalidation: function (input) {
                                if (input.is("[name='Hora']") && input.val() != "") {
                                    var split = input.val().split(':');
                                    console.log(split)

                                    if (split.length !== 3) {
                                        input.attr("data-horavalidation-msg", "Debe ser una hora valida en el formato HH24:MI:SS.MSS");
                                        return false
                                    }

                                    if (split[2].split('.').length != 2) {
                                        input.attr("data-horavalidation-msg", "Debe ser una hora valida en el formato HH24:MI:SS.MSS");
                                        return false
                                    }
                                }

                                return true;
                            }
                        }
                    },
                    D16: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D20: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D25: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D315: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D40: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D50: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D63: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D80: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D100: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D125: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D160: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D200: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D250: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D3151: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D400: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D500: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D630: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D800: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D1000: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D1250: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D1600: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D2000: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D2500: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D3150: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },
                    },
                    D4000: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },

                    },
                    D5000: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },

                    },
                    D6300: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },

                    },
                    D8000: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },

                    },
                    D10000: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },

                    },
                    TipoInfo: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },

                    },
                    Creadopor: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },

                    },
                    NoSerie: {
                        nullable: false,
                        type: "string",
                        validation: {
                            maxLength: 50,
                            required: {
                                message: "Campo requerido"
                            },
                            pattern: {
                                value: "^[0-9]\\d{0,5}(\\.\\d{1,5})?%?$",
                                message: "El ruido medido en el decibel debe ser numérico mayor a cero considerando 6 enteros con 5 decimales"
                            },
                        },

                    },
                }
            }
        }
    });

    var grid = $("#grid").kendoGrid({
        dataSource: dataSourceTable,
        editable: "incell",

        scrollable: true,
        columns: [
            {
                title: "#",
                template: "#= ++record #",
                width: 70
            },
            {
                title: "Hora", width: 200, field: "Hora", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "HoraCllass" }
            },
            {
                title: "D16", width: 150, field: "D16", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D16", "id": "D16_#= uid #" },
            },
            {
                title: "D20", width: 150, field: "D20", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D20", "id": "D20_#= uid #" }
            },
            {
                title: "D25", width: 150, field: "D25", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D25", "id": "D25_#= uid #" }
            },
            {
                title: "D31.5", width: 150, field: "D315", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D315", "id": "D315_#= uid #" }
            },
            {
                title: "D40", width: 150, field: "D40", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D40", "id": "D40_#= uid #" }
            },
            {
                title: "D50", width: 150, field: "D50", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D50", "id": "D50_#= uid #" }
            },
            {
                title: "D63", width: 150, field: "D63", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D63", "id": "D63_#= uid #" }
            },
            {
                title: "D80", width: 150, field: "D80", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D80", "id": "D80_#= uid #" }
            },
            {
                title: "D100", width: 150, field: "D100", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D100", "id": "D100_#= uid #" }
            },
            {
                title: "D125", width: 150, field: "D125", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D125", "id": "D125_#= uid #" }
            },
            {
                title: "D160", width: 150, field: "D160", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D160", "id": "D160_#= uid #" }
            },
            {
                title: "D200", width: 150, field: "D200", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D200", "id": "D200_#= uid #" }
            },
            {
                title: "D250", width: 150, field: "D250", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D250", "id": "D250_#= uid #" }
            },
            {
                title: "D315", width: 150, field: "D3151", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D3151", "id": "D3151_#= uid #" }
            },
            {
                title: "D400", width: 150, field: "D400", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D400", "id": "D400_#= uid #" }
            },
            {
                title: "D500", width: 150, field: "D500", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D500", "id": "D500_#= uid #" }
            },
            {
                title: "D630", width: 150, field: "D630", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D630", "id": "D630_#= uid #" }
            },
            {
                title: "D800", width: 150, field: "D800", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D800", "id": "D800_#= uid #" }
            },
            {
                title: "D1000", width: 150, field: "D1000", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D1000", "id": "D1000_#= uid #" }
            },
            {
                title: "D1250", width: 150, field: "D1250", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D1250", "id": "D1250_#= uid #" }
            },
            {
                title: "D1600", width: 150, field: "D1600", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D1600", "id": "D1600_#= uid #" }
            },
            {
                title: "D2000", width: 150, field: "D2000", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D2000", "id": "D2000_#= uid #" }
            },
            {
                title: "D2500", width: 150, field: "D2500", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D2500", "id": "D2500_#= uid #" }
            },
            {
                title: "D3150", width: 150, field: "D3150", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D3150", "id": "D3150_#= uid #" }
            },
            {
                title: "D4000", width: 150, field: "D4000", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D4000", "id": "D4000_#= uid #" }
            },
            {
                title: "D5000", width: 150, field: "D5000", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D5000", "id": "D5000_#= uid #" }
            },
            {
                title: "D6300", width: 150, field: "D6300", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D6300", "id": "D6300_#= uid #" }
            },
            {
                title: "D8000", width: 150, field: "D8000", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D8000", "id": "D8000_#= uid #" }
            },
            {
                title: "D10000", width: 150, field: "D10000", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D10000", "id": "D10000_#= uid #" }
            },
            {
                title: "TipoInfo", width: 150, field: "TipoInfo", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D10000", "id": "D10000_#= uid #" },
                hidden: true

            },
            {
                title: "Creadopor", width: 150, field: "Creadopor", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D10000", "id": "D10000_#= uid #" },
                hidden: true

            },
            {
                title: "FechaDatos", width: 150, field: "FechaDatos", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D10000", "id": "D10000_#= uid #" },
                hidden: true

            },
            {
                title: "NoSerie", width: 150, field: "NoSerie", headerAttributes: {
                    "class": "table-header-cell k-text-center",
                },
                attributes: { style: 'text-align: center', "class": "D10000", "id": "D10000_#= uid #" },
                hidden: true

            },

        ],
        pageable: true,
        dataBinding: function () {//genera el incremental en la tabla
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
    }).data("kendoGrid");



    grid.table.on('keydown', function (e) {
        var r;
        var validator = null;

        try {
            //console.log(e.target.id)
            r = eval('grid.dataSource.options.schema.model.fields.' + e.target.id + '.validation.pattern.value')
            validator = new RegExp(r);
        } catch (error) {
            //console.log(error);
        }
        var pasa = true
        if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {
            e.preventDefault();
            var currentNumberOfItems = grid.dataSource.view().length;
            var tt = $(e.target).closest('tr')
            var lCompleteRow = $(e.target).closest('tr').children("td:eq(2)").attr('id');
            // console.log(lCompleteRow)
            var row = $(e.target).closest('tr').index();
            var col = grid.cellIndex($(e.target).closest('td'));

            var dataItem = grid.dataItem($(e.target).closest('tr'));

            var field = grid.columns[col].field;
            var value = $(e.target).val();
            //console.log(value)
            if (value === '') {
                pasa = false
            } else {
                if (e.target.id !== "Hora") {
                    if (validator !== null && !validator.test(value)) {
                        pasa = false
                    }
                } else {
                    var split = value.split(':');

                    if (split.length !== 3) {
                        pasa = false
                    } else {
                        if (split[2].split('.').length != 2) {
                            pasa = false
                        }
                    }


                }

            }

            dataItem.set(field, value);

            if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < grid.columns.length) {

                var nextCellRow = row;
                var nextCellCol = col;

                if (e.shiftKey) {
                    if (nextCellRow - 1 < 0) {
                        nextCellRow = currentNumberOfItems - 1;
                        nextCellCol--;
                    } else {
                        nextCellRow--;
                    }
                } else {
                    if (nextCellRow + 1 >= currentNumberOfItems) {
                        nextCellRow = 0;
                        nextCellCol++;
                    } else {
                        nextCellRow++;
                    }
                }

                if (nextCellCol >= grid.columns.length || nextCellCol < 0) {
                    return;
                }

                // wait for cell to close and Grid to rebind when changes have been made
                setTimeout(function () {
                    if (pasa) {
                        grid.editCell(grid.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                    }
                });
            }

            if (value > 776) {
                ShowWarningMessage("El decibel es muy alto para el calculo de la formula")
            }
        }

        if (e.keyCode === kendo.keys.TAB && $($(e.target).closest('.k-edit-cell'))[0]) {
            e.preventDefault();
            var currentNumberOfItems = grid.dataSource.view().length;

            var row = $(e.target).closest('tr').index();
            var col = grid.cellIndex($(e.target).closest('td'));

            // console.log("row " + row);
            //console.log("column " + col);

            var dataItem = grid.dataItem($(e.target).closest('tr'));

            var field = grid.columns[col].field;
            var value = $(e.target).val();


            if (value === '') {
                pasa = false
            } else {
                if (e.target.id !== "Hora") {
                    if (validator !== null && !validator.test(value)) {
                        pasa = false
                    }
                } else {
                    var split = value.split(':');

                    if (split.length !== 3) {
                        pasa = false
                    } else {
                        if (split[2].split('.').length != 2) {
                            pasa = false
                        }
                    }


                }

            }

            dataItem.set(field, value);
            if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < grid.columns.length) {

                var nextCellRow = row;
                var nextCellCol = col;

                if (nextCellCol + 1 <= grid.columns.length) {
                    nextCellCol++;

                    if (nextCellCol >= grid.columns.length && nextCellRow + 1 <= currentNumberOfItems - 1) {
                        nextCellRow++
                        nextCellCol = 1

                        setTimeout(function () {
                            if (pasa) {
                                grid.editCell(grid.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                            }
                        });
                    } else {
                        setTimeout(function () {
                            if (pasa) {
                                grid.editCell(grid.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                            }
                        });
                    }
                }

            }
        }


    });
}

function timeout(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
async function sleep() {
    await timeout(1000);
}


async function sleep2() {
    await timeout(5000);
}


async function ejecuteValidUrl() {
    $('#FileSpan').text("");
    $("#FileSpan").removeClass('k-hidden');

    banderaFileErrorExtension = false;
    banderaFileErrorPeso = false;
    var fileUpload = $("#File").get(0);

    var files = fileUpload.files;

    if (files.length > 0) {
        let nameFile = files[0].name;
        var extension = "." + nameFile.substr((nameFile.lastIndexOf('.') + 1));
        await GetConfigurationFilesJSON(8).then(
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



                } else {
                    ShowFailedMessage("No se ha podido encontrar la configuracion para el modulo de archivos")
                }
            }
        );

        await sleep2()
    }

}

async function GetConfigurationFilesJSON(module) {

    var path = '/Onaf/GetConfigurationFiles';

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

async function GetInformationOctaves() {
    var path = path = "/Onaf/GetInformationOctaves/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value, fecha: fecha.value, altura: altura.value, tipo: tipoInformacion.value }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

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

async function GetAlturas() {
    var path = path = "/Onaf/GetAlturas/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value, fecha: fecha.value, tipo: tipoInformacion.value }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

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

function ValidateForm() {

    ResultValidations = $("#onaf_information").data("kendoValidator").validate();
    if (ResultValidations) {
        return true;
    } else {
        return false;
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