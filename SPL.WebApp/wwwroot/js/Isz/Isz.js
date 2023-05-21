
//Variables
let viewModel;
let settingsToDisplayPCIReportsDTO;
let btnRequest = document.getElementById("btnRequest");
let btnClear = document.getElementById("btnClear");
let btnLoadTemplate = document.getElementById("btnLoadTemplate");
let noSerieInput = document.getElementById("NoSerie");
let CommentsInput = document.getElementById("Comments");
let noPruebaInput = document.getElementById("NoPrueba");
let neutrosInput = document.getElementById("CantidadNeutros");

let atImput = document.getElementById("selectAT");
let btImput = document.getElementById("selectBT");
let terImput = document.getElementById("selectTer");
let primeravez = true
//noPruebaInput.value = '';
let claveIdiomaInput = document.getElementById("ClaveIdioma");
let pruebaInput = document.getElementById("Pruebas");
let materialDevanadoInput = document.getElementById("MaterialDevanado");
let impedanciaGarantizadaInput = document.getElementById("ImpedanciaGarantizada");
let gradosInput = document.getElementById("Grados");
let otroInput = document.getElementById("Otro");
let spreadsheetElement;
let treeViewKendoElement;

let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");
let panelExcel = document.getElementById("spreadsheet");
let atList = [];
let btList = [];
let terList = [];

//Mostrar info necesaria para consultar el reporte
document.querySelector('#btnInfo').addEventListener('click', () => {
    var text =
        'Considerar que para este reporte:<br><br>'
        + 'El “No. De Serie” del Aparato se debe de encontrar registrado en SPL.<br>'
        + 'Se debe contar con Información en la pantalla: “Diseño del Aparato”.<br>'
        + 'Se deben de contar con las plantillas cargadas para las pruebas correspondientes del reporte:<br>'
        + '<ul>'
        + '<li>Alta Tensión y Baja Tensión (Ambos Idiomas)</li>'
        + '<li>Alta Tensión y Terciario (Ambos Idiomas)</li>'
        + '<li>Baja Tensión y Terciario (Ambos Idiomas)</li>'
        + '</ul>'
        + '<br>'
        + 'Debe de existir información en la pantalla: “Tensión de la Placa”.<br>'
        + 'Se deberá de seleccionar una de las opciones para el apartado de las posiciones.';
    ShowWarningMessageHtmlContent(text);
});







function getDevenadoEnergizado() {
    $("#loader").css("display", "block");
    if (!primeravez) {

        getDevenadoEnergizadoJson(noSerieInput.value, pruebaInput.value).then(
            data => {
                if (data !== null && data !== undefined) {
           


                    $("#loader").css("display", "none");



                    if (pruebaInput.value === "AYB") {

                        $("#DevanadoEnergizado1 option[value!='']").each(function () {
                            $(this).remove();
                        });

                        $.each(data.response.DevanadosAT, function (index, value) {
                            $("#DevanadoEnergizado1").append("<option selected value='" + value.Devanado + "'>" + value.Devanado + "</option>")
                        })

                        $("#DevanadoEnergizado1").prop("selectedIndex", data.response.SeleccionadoAT);
                        

                        $("#selectAT").prop("disabled", false)
                        $("#selectBT").prop("disabled", false)
                        $("#selectTer").prop("disabled", true)

                        $("#DevanadoEnergizado1").prop("disabled", false)
                        $("#DevanadoEnergizado1").prop("selectedIndex", 0)

                        $("#DevanadoEnergizado2").prop("disabled", true)
                        $("#DevanadoEnergizado2").prop("selectedIndex", 0)

                        $("#DevanadoEnergizado3").prop("disabled", true)
                        $("#DevanadoEnergizado3").prop("selectedIndex", 0)

                        $("#Impedancia1").val(viewModel.WarrantiesArtifact.ZPositiveHx??0);
                        $("#Impedancia2").val("");
                        $("#Impedancia3").val("");

                        $("#Impedancia2").prop("disabled", true)
                        $("#Impedancia3").prop("disabled", true)
                        $("#Impedancia1").prop("disabled", false)

                    }
                    else if (pruebaInput.value === "AYT") {


                        $("#DevanadoEnergizado2 option[value!='']").each(function () {
                            $(this).remove();
                        });

                        $.each(data.response.DevanadosBT, function (index, value) {
                            $("#DevanadoEnergizado2").append("<option selected value='" + value.Devanado + "'>" + value.Devanado + "</option>")
                        })

                        $("#DevanadoEnergizado2").prop("selectedIndex", data.response.SeleccionadoBT);
                        

                        $("#selectAT").prop("disabled", false)
                        $("#selectBT").prop("disabled", true)
                        $("#selectTer").prop("disabled", false)

                        $("#DevanadoEnergizado1").prop("disabled", true)
                        $("#DevanadoEnergizado1").prop("selectedIndex", 0)

                        $("#DevanadoEnergizado2").prop("disabled", false)
                        $("#DevanadoEnergizado2").prop("selectedIndex", 0)

                        $("#DevanadoEnergizado3").prop("disabled", true)
                        $("#DevanadoEnergizado3").prop("selectedIndex", 0)


                        $("#Impedancia1").val("");
                        $("#Impedancia2").val(viewModel.WarrantiesArtifact.ZPositiveHy??0);
                        $("#Impedancia3").val("");

                        $("#Impedancia1").prop("disabled", true)
                        $("#Impedancia3").prop("disabled", true)
                        $("#Impedancia2").prop("disabled", false)
                    }
                    else if (pruebaInput.value === "BYT") {

                        $("#DevanadoEnergizado3 option[value!='']").each(function () {
                            $(this).remove();
                        });

                        $.each(data.response.DevanadosTer, function (index, value) {
                            $("#DevanadoEnergizado3").append("<option selected value='" + value.Devanado + "'>" + value.Devanado + "</option>")
                        })

                        $("#DevanadoEnergizado2").prop("selectedIndex", data.response.SeleccionadoTer);
                        $("#selectAT").prop("disabled", true)
                        $("#selectBT").prop("disabled", false)
                        $("#selectTer").prop("disabled", false)
                        $("#DevanadoEnergizado1").prop("disabled", true)
                        $("#DevanadoEnergizado1").prop("selectedIndex", 0)

                        $("#DevanadoEnergizado2").prop("disabled", true)
                        $("#DevanadoEnergizado2").prop("selectedIndex", 0)

                        $("#DevanadoEnergizado3").prop("disabled", false)
                        $("#DevanadoEnergizado3").prop("selectedIndex", 0)

                        $("#Impedancia1").val("");
                        $("#Impedancia2").val("");
                        $("#Impedancia3").val(viewModel.WarrantiesArtifact.ZPositiveXy ?? 0);

                        $("#Impedancia2").prop("disabled", true)
                        $("#Impedancia1").prop("disabled", true)
                        $("#Impedancia3").prop("disabled", false)

                    }
                    else if (pruebaInput.value === "ABT") {
                        $("#selectAT").prop("disabled", false)
                        $("#selectBT").prop("disabled", false)
                        $("#selectTer").prop("disabled", false)

                        $("#DevanadoEnergizado1").prop("disabled", false)
                        $("#DevanadoEnergizado1").prop("selectedIndex", 0)
                        $("#DevanadoEnergizado2").prop("disabled", false)
                        $("#DevanadoEnergizado2").prop("selectedIndex", 0)
                        $("#DevanadoEnergizado3").prop("disabled", false)
                        $("#DevanadoEnergizado3").prop("selectedIndex", 0)
                        $("#Impedancia1").val(viewModel.WarrantiesArtifact.ZPositiveHx??0);
                        $("#Impedancia2").val(viewModel.WarrantiesArtifact.ZPositiveHy??0);
                        $("#Impedancia3").val(viewModel.WarrantiesArtifact.ZPositiveXy??0);
                    }

                    $("#selectAT option:selected").prop("selected", false)
                    $("#selectBT option:selected").prop("selected", false)
                    $("#selectTer option:selected").prop("selected", false)

                    $("#DevanadoEnergizado1 option:selected").prop("selected", false)
                    $("#DevanadoEnergizado2 option:selected").prop("selected", false)
                    $("#DevanadoEnergizado3 option:selected").prop("selected", false)


                }
                $("#loader").css("display", "none");
            }
        );
    } else {
        primeravez = false
    }


}

async function getDevenadoEnergizadoJson(nroserie, keytests) {

    var path = '/Isz/GetDataDevenadoEnergizado';

    var url = new URL(domain + path),
        params = {
            noSerieSimple: nroserie,
            keyTests: keytests
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


//Evento
$("#NoSerie").focus();

$("#Grados").change(function(){
    if (gradosInput.value === 'Otro') {
        otroInput.disabled = false
    } else {
        otroInput.disabled = true
        otroInput.value = ''
        $("#Otro_validationMessage").text('')
    }
})

$("#Pruebas").on('change', function (e) {
    if (pruebaInput.value === "AYB") {
        terImput.disabled = true
        atImput.disabled = false
        btImput.disabled = false
    }

    if (pruebaInput.value === "AYT") {
        terImput.disabled = false
        atImput.disabled = false
        btImput.disabled = true
    }

    if (pruebaInput.value === "BYT") {
        terImput.disabled = false
        atImput.disabled = true
        btImput.disabled = false
    }
});


$("#NoSerie").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $("#btnRequest").click();
    }
});
//Evento

//Requests
btnClear.addEventListener("click", function () {

    noSerieInput.value = '';
    //noPruebaInput.value = '';
    CommentsInput.value = '';
    claveIdiomaInput.value = '';
    pruebaInput.value = ''
    gradosInput.value = ''
    neutrosInput.value =''
    btnRequest.disabled = false;
    pruebaInput.disabled = false
    primeravez =true
    btnClear.disabled = false;
    btnLoadTemplate.disabled = true;
    noSerieInput.disabled = false;
    
    $("#Otro").prop("disabled",true)
    $("#Otro").val("")

    if (treeViewKendoElement !== undefined) {
        kendo.destroy(treeViewKendoElement);
        $("#treeview-kendo").empty();
    }

    $("#selectAT").prop("disabled",true)
    $("#selectBT").prop("disabled",true)
    $("#selectTer").prop("disabled",true)

   
    $("#selectAT option[value!='']").each(function() {
        $(this).remove();
    });

    $("#selectBT option[value!='']").each(function() {
        $(this).remove();
    });


    $("#selectTer option[value!='']").each(function() {
        $(this).remove();
    });

    $("#Pruebas option[value!='']").each(function() {
        $(this).remove();
    });

    $("#selectAT option:selected").prop("selected", false)
    $("#selectBT option:selected").prop("selected", false)
    $("#selectTer option:selected").prop("selected", false)

    $("#DevanadoEnergizado1 option:selected").prop("selected", false)
    $("#DevanadoEnergizado2 option:selected").prop("selected", false)
    $("#DevanadoEnergizado3 option:selected").prop("selected", false)

    $("#Impedancia2").prop("disabled", true)
    $("#Impedancia1").prop("disabled", true)
    $("#Impedancia3").prop("disabled", true)

    $("#Impedancia3").val("");
    $("#Impedancia2").val("");
    $("#Impedancia1").val("");

    $("#DevanadoEnergizado1").prop("disabled", true)
    $("#DevanadoEnergizado2").prop("disabled", true)
    $("#DevanadoEnergizado3").prop("disabled", true)
});

btnLoadTemplate.addEventListener("click", function () {
    if (ValidateForm()) {

        atList = []
        btList = []
        terList = []

        if (pruebaInput.value === "AYB") {
            if ($("#selectAT").val() === null || $("#selectBT").val() === null) {
                ShowFailedMessage("Favor de seleccionar las posiciones de AT y BT")
                return
            }

            var firstElement1 = $("#selectAT").val()[0];
            var firstElement2 = $("#selectBT").val()[0];

            if (firstElement1 === '' && firstElement2 === '') {
                ShowFailedMessage("Solo se permite seleccionar todas las posiciones en una de ellas, favor de corregirlo")
                return
            }

            if ((firstElement1 === '' && firstElement2 !== '') || (firstElement1 !== '' && firstElement2 === '')) {
                if ((firstElement1 === '' && $("#selectBT").val().length === 1) || (firstElement2 === '' && $("#selectAT").val().length === 1)) {

                } else {
                    var posicionesAT = getPosiciones("selectAT").length;
                    var posicionesBT = getPosiciones("selectBT").length;
                    if (posicionesBT > 5) {
                        ShowFailedMessage("Debe seleccionar máximo 5 posiciones para BT")
                        return
                    }
                    if (posicionesAT > 5) {
                        ShowFailedMessage("Debe seleccionar máximo 5 posiciones para AT")
                        return
                    }
                }

            } else {
                /*ShowFailedMessage("Debe seleccionar todas las posiciones en AT o BT")
                return*/
            }

            if (firstElement1 === '') {
                atList = Array.from($('#selectAT option').map((index, option) => option.value).filter(x => x != ''))
                btList = firstElement2
            }

            if (firstElement2 === '') {
                btList = Array.from($('#selectBT option').map((index, option) => option.value).filter(x => x != ''))
                atList = firstElement1
            }

            if ($("#DevanadoEnergizado1").val() !== '') {
  
            } else {
                ShowFailedMessage("Debe seleccionar un devanado energizado para AT y BT")
                return
            }

            if ($("#selectAT").val()[0] !== '' && $("#selectAT").val().length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para AT no puede exceder de 5, favor corregirlas")
                return
            } else if ($("#selectAT").val()[0] === '' && Array.from($('#selectAT option').map((index, option) => option.value).filter(x => x != '')).length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para AT no puede exceder de 5, favor corregirlas")
                return
            }


            if ($("#selectBT").val()[0] !== '' && $("#selectBT").val().length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para BT no puede exceder de 5, favor corregirlas")
                return
            } else if ($("#selectBT").val()[0] === '' && Array.from($('#selectBT option').map((index, option) => option.value).filter(x => x != '')).length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para BT no puede exceder de 5, favor corregirlas")
                return
            }


        }
        else if (pruebaInput.value === "AYT") {

            if ($("#selectAT").val() === null || $("#selectTer").val() === null) {
                ShowFailedMessage("Favor de seleccionar las posiciones de AT y Ter")
                return
            }

            var firstElement1 = $("#selectAT").val()[0];
            var firstElement2 = $("#selectTer").val()[0];

            if (firstElement1 === '' && firstElement2 === '') {
                ShowFailedMessage("Solo se permite seleccionar todas las posiciones en una de ellas, favor de corregirlo")
                return
            }

            if ((firstElement1 === '' && firstElement2 !== '') || (firstElement1 !== '' && firstElement2 === '')) {
                if ((firstElement1 === '' && $("#selectAT").val().length === 1) || (firstElement2 === '' && $("#selectTer").val().length === 1)) {

                } else {
                    var posicionesAT = getPosiciones("selectAT").length;
                    var posicionesTer = getPosiciones("selectTer").length;
                    if (posicionesAT > 5) {
                        ShowFailedMessage("Debe seleccionar máximo 5 posiciones para AT")
                        return
                    }
                    if (posicionesTer > 5) {
                        ShowFailedMessage("Debe seleccionar máximo 5 posiciones para Ter")
                        return
                    }
                }

            } else {
                /*ShowFailedMessage("Debe seleccionar todas las posiciones en AT o Ter")
                return*/
            }



            if (firstElement1 === '') {
                atList = Array.from($('#selectAT option').map((index, option) => option.value).filter(x => x != ''))
                terList = firstElement2
            }

            if (firstElement2 === '') {
                terList = Array.from($('#selectTer option').map((index, option) => option.value).filter(x => x != ''))
                atList = firstElement1
            }


            if ($("#DevanadoEnergizado2").val() !== '') {

            } else {
                ShowFailedMessage("Debe seleccionar un devanado energizado para AT y Ter")
                return
            }

            if ($("#selectAT").val()[0] !== '' && $("#selectAT").val().length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para AT no puede exceder de 5, favor corregirlas")
                return
            } else if ($("#selectAT").val()[0] === '' && Array.from($('#selectAT option').map((index, option) => option.value).filter(x => x != '')).length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para AT no puede exceder de 5, favor corregirlas")
                return
            }

            if ($("#selectTer").val()[0] !== '' && $("#selectTer").val().length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para TER no puede exceder de 5, favor corregirlas")
                return
            } else if ($("#selectTer").val()[0] === '' && Array.from($('#selectTer option').map((index, option) => option.value).filter(x => x != '')).length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para TER no puede exceder de 5, favor corregirlas")
                return
            }

            
        }
        else if (pruebaInput.value === "BYT") {
            if ($("#selectBT").val() === null || $("#selectTer").val() === null) {
                ShowFailedMessage("Favor de seleccionar las posiciones de BT y Ter")
                return
            }


            var firstElement1 = $("#selectBT").val()[0];
            var firstElement2 = $("#selectTer").val()[0];

            if (firstElement1 === '' && firstElement2 === '') {
                ShowFailedMessage("Solo se permite seleccionar todas las posiciones en una de ellas, favor de corregirlo")
                return
            }


            if ((firstElement1 === '' && firstElement2 !== '') || (firstElement1 !== '' && firstElement2 === '')) {
                if ((firstElement1 === '' && $("#selectBT").val().length === 1) || (firstElement2 === '' && $("#selectTer").val().length === 1)) {

                } else {
                    var posicionesBT = getPosiciones("selectBT").length;
                    var posicionesTer = getPosiciones("selectTer").length;
                    if (posicionesBT > 5) {
                        ShowFailedMessage("Debe seleccionar máximo 5 posiciones para BT")
                        return
                    }
                    if (posicionesTer > 5) {
                        ShowFailedMessage("Debe seleccionar máximo 5 posiciones para TER")
                        return
                    }
                }

            } else {
                /*ShowFailedMessage("Debe seleccionar todas las posiciones en BT o Ter")
                return*/
            }


            if (firstElement1 === '') {
                btList = Array.from($('#selectBT option').map((index, option) => option.value).filter(x => x != ''))
                terList = firstElement2
            }

            if (firstElement2 === '') {
                terList = Array.from($('#selectTer option').map((index, option) => option.value).filter(x => x != ''))
                btList = firstElement1
            }


            if ($("#DevanadoEnergizado3").val() !== '') {

            } else {
                ShowFailedMessage("Debe seleccionar un devanado energizado para BT y Ter")
                return
            }

            if ($("#selectBT").val()[0] !== '' && $("#selectBT").val().length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para BT no puede exceder de 5, favor corregirlas")
                return
            } else if ($("#selectBT").val()[0] === '' && Array.from($('#selectBT option').map((index, option) => option.value).filter(x => x != '')).length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para BT no puede exceder de 5, favor corregirlas")
                return
            }


            if ($("#selectTer").val()[0] !== '' && $("#selectTer").val().length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para TER no puede exceder de 5, favor corregirlas")
                return
            } else if ($("#selectTer").val()[0] === '' && Array.from($('#selectTer option').map((index, option) => option.value).filter(x => x != '')).length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para TER no puede exceder de 5, favor corregirlas")
                return
            }


        }
        else if (pruebaInput.value === "ABT") {
            if ($("#selectAT").val() === null || $("#selectBT").val() === null || $("#selectTer").val() === null) {
                ShowFailedMessage("Favor de seleccionar las posiciones de AT, BT y Ter")
                return
            }
            var firstElement1 = $("#selectAT").val()[0];
            var firstElement2 = $("#selectBT").val()[0];
            var firstElement3 = $("#selectTer").val()[0];
            let cont = 0 

            if (firstElement1 === '')
                cont++;

            if (firstElement2 === '')
                cont++;

            if (firstElement3 === '')
                cont++;


            if (cont > 1) {
                ShowFailedMessage("Solo se permite seleccionar todas las posiciones en una de ellas, favor de corregirlo")
                return
            } else if (cont === 0) {
                ShowFailedMessage("Debe seleccionar la opcion de 'Todas' para al menos una posicion, favor de corregirlo")
                return
            }


            if (firstElement1 === '' && firstElement2 !== '' && firstElement3 !== '') {// TODAS TER
                var posicionesBT = getPosiciones("selectBT").length;
                var posicionesTer = getPosiciones("selectTer").length;
                if (posicionesBT > 5) {
                    ShowFailedMessage("Debe seleccionar máximo 5 posiciones para BT")
                    return
                }
                if (posicionesTer > 5) {
                    ShowFailedMessage("Debe seleccionar máximo 5 posiciones para TER")
                    return
                }
                // OK
            }
            else if (firstElement1 !== '' && firstElement2 === '' && firstElement3 !== '') { // TODAS BT
                var posicionesAT = getPosiciones("selectAT").length;
                var posicionesTer = getPosiciones("selectTer").length;
                if (posicionesAT > 5) {
                    ShowFailedMessage("Debe seleccionar máximo 5 posiciones para AT")
                    return
                }
                if (posicionesTer > 5) {
                    ShowFailedMessage("Debe seleccionar máximo 5 posiciones para TER")
                    return
                }
            }
            else if (firstElement1 !== '' && firstElement2 !== '' && firstElement3 === '') {//TODAS TER
                var posicionesAT = getPosiciones("selectAT").length;
                var posicionesBT = getPosiciones("selectBT").length;
                if (posicionesAT > 5) {
                    ShowFailedMessage("Debe seleccionar máximo 5 posiciones para AT")
                    return
                }
                if (posicionesBT > 5) {
                    ShowFailedMessage("Debe seleccionar máximo 5 posiciones para BT")
                    return
                }
            }



            if (firstElement1 === '') {
                atList = Array.from($('#selectAT option').map((index, option) => option.value).filter(x => x != ''))
            } else {
                atList = $('#selectAT').val()
            }

            if (firstElement2 === '') {
                btList = Array.from($('#selectBT option').map((index, option) => option.value).filter(x => x != ''))
            } else {
                btList = $('#selectBT').val()
            }

            if (firstElement3 === '') {
                terList = Array.from($('#selectTer option').map((index, option) => option.value).filter(x => x != ''))
            } else {
                terList = $('#selectTer').val()
            }


            if ($("#DevanadoEnergizado1").val() !== '' && $("#DevanadoEnergizado2").val() !== '' && $("#DevanadoEnergizado3").val() !== '') {

            } else {
                ShowFailedMessage("Debe seleccionar un devanado energizado para AT, BT y Ter")
                return
            }


            if ($("#selectAT").val()[0] !== '' && $("#selectAT").val().length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para AT no puede exceder de 5, favor corregirlas")
                return
            } else if ($("#selectAT").val()[0] === '' && Array.from($('#selectAT option').map((index, option) => option.value).filter(x => x != '')).length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para AT no puede exceder de 5, favor corregirlas")
                return
            }


            if ($("#selectBT").val()[0] !== '' && $("#selectBT").val().length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para BT no puede exceder de 5, favor corregirlas")
                return
            } else if ($("#selectBT").val()[0] === '' && Array.from($('#selectBT option').map((index, option) => option.value).filter(x => x != '')).length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para BT no puede exceder de 5, favor corregirlas")
                return
            }


            if ($("#selectTer").val()[0] !== '' && $("#selectTer").val().length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para TER no puede exceder de 5, favor corregirlas")
                return
            } else if ($("#selectTer").val()[0] === '' && Array.from($('#selectTer option').map((index, option) => option.value).filter(x => x != '')).length > 5) {
                ShowFailedMessage("La cantidad de posiciones seleccionadas para TER no puede exceder de 5, favor corregirlas")
                return
            }
        }


            


        if(neutrosInput.value === "0"){
            ShowFailedMessage("Cantidad de neutros solo puede ser 1 o 2")
            return
        }

        MapToViewModel();
        btnRequest.disabled = true;
        btnLoadTemplate.disabled = true;
        btnClear.disabled = false;
        pruebaInput.disabled = true
        $("#loader").css("display", "block");
        atList = getPosiciones("selectAT");
        btList = getPosiciones("selectBT");
        terList = getPosiciones("selectTer");
        GetTemplateJSON()
    }
    else {
        ShowFailedMessage("Faltan campos por llenar")
        $("#loader").css("display", "none");
    }
});

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

    $("#loader").css("display", "block");

    btnRequest.disabled = true
    btnClear.disabled = true
    if (noSerieInput.value) {

        await GetFilterJSON(null).then(
            data => {
                if (data.response.Code !== -1) {
                    btnSave.disabled = false;
                    noSerieInput.disabled = true;

                    viewModel = data.response.Structure;


                    console.log(data.response.Structure );
                    LoadTreeView(viewModel.TreeViewItem);
                    LoadForm(viewModel);
                    btnRequest.disabled = true;
                    btnClear.disabled = false;
                    btnLoadTemplate.disabled = false;
                    $("#Pruebas").change();
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
        btnLoadDataSidco.disabled = true;
        btnClear.disabled = false
        btnRequest.disabled = false
    }
});

//Requests


//Functions
async function GetFilterJSON() {
    var path = path = "/Isz/GetFilter/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value }
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


async function GetTemplateJSON() {
    var grados ;
    if(gradosInput.value == "Otro"){
        grados = otroInput.value
    }else{
        grados = gradosInput.value
    }

    var At = '';
    var Bt = '';
    var Ter = '';

    if (pruebaInput.value === "AYB") {
        /* alert('23423')*/
        if (atImput.value === '') {
            At = "Todas"
        } else {
            At = atImput.value
        }

        if (btImput.value === '') {
            Bt = "Todas"
        } else {
            Bt = btImput.value
        }
    }
    else if (pruebaInput.value === "AYT") {
        /*      alert('sdf')*/
        if (atImput.value === '') {
            At = "Todas"
        } else {
            At = atImput.value
        }

        if (terImput.value === '') {
            Ter = "Todas"
        } else {
            Ter = terImput.value
        }
    }

    else if (pruebaInput.value === "BYT") {
        if (btImput.value === '') {
            Bt = "Todas"
        } else {
            Bt = btImput.value
        }

        if (terImput.value === '') {
            Ter = "Todas"
        } else {
            Ter = terImput.value
        }
    } else if (pruebaInput.value === "ABT") {
        if (btImput.value === '') {
            Bt = "Todas"
        } else {
            Bt = btImput.value
        }

        if (terImput.value === '') {
            Ter = "Todas"
        } else {
            Ter = terImput.value
        }

        if (atImput.value === '') {
            At = "Todas"
        } else {
            At = atImput.value
        }
    }

    viewModel.AtSelected = At;
    viewModel.TerSelected = Ter;
    viewModel.BtSelected = Bt;

    //alert(At)
    //alert(Bt)
    //alert(Ter)
    let impedancia =''
    let devanado = ''
    let seleccionadoDos=''
    if (pruebaInput.value === "ABT") {
        devanado = $("#DevanadoEnergizado1").val() + "," + $("#DevanadoEnergizado2").val() + "," + $("#DevanadoEnergizado3").val()
        impedancia = $("#Impedancia1").val() + "," + $("#Impedancia2").val() + "," + $("#Impedancia3").val()

        if ($("#selectAT").val()[0] === "") {
            seleccionadoDos ="AT"
        }
        else if ($("#selectBT").val()[0] ==="") {
            seleccionadoDos = "BT"
        }
        else if ($("#selectTer").val()[0] === "") {
            seleccionadoDos = "TER"
        }

    } else if (pruebaInput.value === "AYB") {
        devanado = $("#DevanadoEnergizado1").val()
        impedancia = $("#Impedancia1").val() 
    } else if (pruebaInput.value === "AYT") {
        devanado = $("#DevanadoEnergizado2").val()
        impedancia = $("#Impedancia2").val()
    } else if (pruebaInput.value === "BYT") {
        devanado = $("#DevanadoEnergizado3").val()
        impedancia = $("#Impedancia3").val()
    }




    var path = path = "/Isz/GetTemplate/";
var url = new URL(domain + path),
    params = { 
        nroSerie: noSerieInput.value,
        keyTest: pruebaInput.value,
        lenguage: claveIdiomaInput.value,
        degreesCor : grados,
        posAT: At ,
        posBT: Bt,
        posTER:Ter,
        qtyNeutral : neutrosInput.value,
        materialWinding: materialDevanadoInput.value,
        ATList : atList,
        BTList : btList,
        TerList: terList,
        devanado: devanado,
        impedancia: impedancia,
        seleccionadoTodosABT: seleccionadoDos,
        comentarios: CommentsInput.value
    }

Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

$("#loader").css("display", "none");
var win = window.open(url.toString(), '_blank');
win.focus();
}


function LoadTreeView(treeViewModel) {
    treeViewKendoElement = $("#treeview-kendo").kendoTreeView({
        template: kendo.template($("#treeview").html()),
        dataSource: treeViewModel,
        dragAndDrop: false,
        select : onSelect,
        checkboxes: false,
        loadOnDemand: true
    });

}

function LoadForm(response) {
    claveIdiomaInput.value = response.ClaveIdioma;
//    noPruebaInput.value = response.NoPrueba;
    $("#Impedancia2").prop("disabled", false)
    $("#Impedancia1").prop("disabled", false)
    $("#Impedancia3").prop("disabled", false)
   pruebas = response.CharacteristicsArtifact;

   $.each(response.Positions.AltaTension , function(i,val){
  		$("#selectAT").append("<option value='"+val +"'>"+val+"</option>");
    });

        $.each(response.Positions.BajaTension , function(i,val){
            $("#selectBT").append("<option value='"+val +"'>"+val+"</option>");
    });

        $.each(response.Positions.Terciario , function(i,val){
            $("#selectTer").append("<option value='"+val +"'>"+val+"</option>");
    });

    $.each(response.ListaPruebas , function(i,val){
        $("#Pruebas").append("<option value='"+val.ClavePrueba +"'>"+val.Descripcion+"</option>");

        if(val.ClavePrueba == "AYB"){
            $("#selectAT").prop("disabled",false);
            $("#selectBT").prop("disabled",false);
            $("#DevanadoEnergizado1").prop("disabled",false);
            $("#DevanadoEnergizado2").prop("disabled", false);

            $("#Impedancia1").val(viewModel.WarrantiesArtifact.ZPositiveHx);
            $("#Impedancia2").val(viewModel.WarrantiesArtifact.ZPositiveHy);
            if ($("#selectTer").is(":disabled")) {
                $("#Impedancia3").prop("disabled", true)
            }
           // $("#Impedancia1").prop("disabled", false);
           // $("#Impedancia2").prop("disabled", false);
            //pruebaInput.value = "AYB"
        }

        if(val.ClavePrueba == "AYT"){
            $("#selectAT").prop("disabled",false);
            $("#selectTer").prop("disabled", false);
            $("#DevanadoEnergizado1").prop("disabled", false);
            $("#DevanadoEnergizado3").prop("disabled", false);
            $("#Impedancia1").val(viewModel.WarrantiesArtifact.ZPositiveHx);
            $("#Impedancia3").val(viewModel.WarrantiesArtifact.ZPositiveXy);
           // $("#Impedancia3").prop("disabled", false);
           // $("#Impedancia1").prop("disabled", false);
        }

        if(val.ClavePrueba == "BYT"){
            $("#selectTer").prop("disabled",false);
            $("#selectBT").prop("disabled", false);
            $("#DevanadoEnergizado2").prop("disabled", false);
            $("#DevanadoEnergizado3").prop("disabled", false);

            $("#Impedancia2").val(viewModel.WarrantiesArtifact.ZPositiveHy);
            $("#Impedancia3").val(viewModel.WarrantiesArtifact.ZPositiveXy);
           // $("#Impedancia3").prop("disabled",false)
           // $("#Impedancia2").prop("disabled",false);
        }
        if (val.ClavePrueba == "ABT") {
            $("#selectTer").prop("disabled", false);
            $("#selectBT").prop("disabled", false);
            $("#selectAT").prop("disabled", false);
            $("#DevanadoEnergizado2").prop("disabled", false);
            $("#DevanadoEnergizado3").prop("disabled", false);
            $("#DevanadoEnergizado1").prop("disabled", false);

           
            $("#Impedancia2").val(viewModel.WarrantiesArtifact.ZPositiveHy ?? 0);
            $("#Impedancia3").val(viewModel.WarrantiesArtifact.ZPositiveXy ?? 0);
            $("#Impedancia1").val(viewModel.WarrantiesArtifact.ZPositiveHx ?? 0);

        }


        if (viewModel.SeleccionameATB)
        {
            pruebaInput.value = "ABT"
        }

    });

    if (viewModel.ListaPruebas.length > 0) {
        var dato = viewModel.ListaPruebas[0]
        if (dato.ClavePrueba === "AYB") {
            pruebaInput.value = "AYB"
            terImput.disabled=true
        }

        if (dato.ClavePrueba === "AYT") {
            pruebaInput.value = "AYB"
            btImput.disabled = true
        }

        if (dato.ClavePrueba === "BYT") {
            pruebaInput.value = "AYB"
            atImput.disabled=true
        }
    }


    var maxOverElevationNumber = Math.max.apply(Math,viewModel.CharacteristicsArtifact.map(x=>x.OverElevation))
    var tempCorreccion = maxOverElevationNumber + 20

    
    var r = $('#Grados option[value="'+tempCorreccion+'"]').val()

    if(r === undefined){
        gradosInput.value =''
        $("#Otro").prop("disabled",false)
        $("#Otro").val(tempCorreccion)
        gradosInput.value ='Otro'
    }else{
        $("#Otro").prop("disabled",true)
        $("#Otro").val("")
        gradosInput.value = r
    }

    if(response.CantidadConexionesEstrellas > 1){
        neutrosInput.value = "2";
    }else   if(response.CantidadConexionesEstrellas === 1){
        neutrosInput.value = "1";
    }else{
        neutrosInput.value = "0";
    }     


    $("#DevanadoEnergizado1 option[value!='']").each(function() {
        $(this).remove();
    });

    $.each(viewModel.DevanadosAgregar.DevanadosAT, function (index, value) {
        $("#DevanadoEnergizado1").append("<option value='"+value.Devanado+"'>"+value.Devanado+"</option>")
    })

    $("#DevanadoEnergizado1").prop('selectedIndex', viewModel.DevanadosAgregar.SeleccionadoAT);


    $("#DevanadoEnergizado2 option[value!='']").each(function () {
        $(this).remove();
    });


    $.each(viewModel.DevanadosAgregar.DevanadosBT, function (index, value) {
        $("#DevanadoEnergizado2").append("<option value='" + value.Devanado + "'>" + value.Devanado + "</option>")
    })

    $("#DevanadoEnergizado2").prop('selectedIndex', viewModel.DevanadosAgregar.SeleccionadoBT);


    $("#DevanadoEnergizado3 option[value!='']").each(function () {

        $(this).remove();
    });

    $.each(viewModel.DevanadosAgregar.DevanadosTer, function (index, value) {
        $("#DevanadoEnergizado3").append("<option value='" + value.Devanado + "'>" + value.Devanado + "</option>")
    })

    $("#DevanadoEnergizado3").prop('selectedIndex', viewModel.DevanadosAgregar.SeleccionadoTer);



}
var ResultValidations;

function ValidateForm() {

    ResultValidations = $("#form_menu_sup").data("kendoValidator").validate();
    if (ResultValidations) {
        return true;
    }else {
        return false;
    }
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
}

function showPdfInNewTab(base64Data, fileName) {

    let pdfWindow = window.open("");
    pdfWindow.document.write("<html<head><title>" + fileName + "</title><style>body{margin: 0px;}iframe{border-width: 0px;}</style></head>");
    pdfWindow.document.write("<body><embed width='100%' height='100%' src='data:application/pdf;base64, " + encodeURI(base64Data) + "#toolbar=0&navpanes=0&scrollbar=0'></embed></body></html>");
}

function MapToViewModel(loadWorkbook = false, loadOfficial = false) {
    viewModel.ClaveIdioma = claveIdiomaInput.value;
    viewModel.Prueba = pruebaInput.value;
    viewModel.Comments = CommentsInput.value;
    viewModel.NoSerie = noSerieInput.value;
    viewModel.MaterialDevanado = materialDevanadoInput.value;
    viewModel.Otro = otroInput.value;
    viewModel.Grados = gradosInput.value;
    viewModel.ATList = $("#selectAT").val(); 
    viewModel.BTList = $("#selectBT").val(); 
    viewModel.TerList = $("#selectTer").val(); 




    if (loadWorkbook)
        viewModel.Workbook.sheets = $("#spreadsheet").getKendoSpreadsheet().toJSON().sheets;
    if (loadOfficial)
        viewModel.OfficialWorkbook = officialWorkbook;
}

function onSelect(e) {
    var text = this.text(e.node);

    if (text.split('.').length > 1) {
        var id = text.split('.')[0].split('-')[2].split('_')[1];
        console.log("Selecting: " + this.text(e.node));
        console.log(id);


        GetPDFJSON(id, "ISZ").then(
            data => {
                if (data != null) {

                    LoadPDF(this.text(e.node), data.data);
                }
                else {
                    ShowFailedMessage(data.response.Description);
                }
                $("#loader").css("display", "none");
            }
        );
    }

}

async function GetPDFJSON(code, tyoeReport) {
    var path = path = "/Pci/GetPDFReport/";

    var url = new URL(domain + path),
        params = { code: code, typeReport: tyoeReport }
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

function clearPosiciones(){
   
}
function getPosiciones(id) {
    var options = $("#" + id).val();
    if (options == null) {
        return [];
    }
    if (options[0] === '') {
        options = [];
        $("#"+id+" > option").each(function () {
            if (this.value !== '') {
                options.push(this.value);
            }
        });
    }
    return options;
}
//Functions