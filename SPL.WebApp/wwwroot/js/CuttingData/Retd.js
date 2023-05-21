//Variables
let viewModel;
let selectETD;
let rows;
let selectRow;
let newETD;

// Botones
let btnRequest = document.getElementById("btnRequest");
let btnNew = document.getElementById("btnNew");
let btnClear1 = document.getElementById("btnClear1");
let btnClear2 = document.getElementById("btnClear2");
let btnStart = document.getElementById("btnStart");
let btnValidate = document.getElementById("btnValidate");
let btnSave = document.getElementById("btnSave");
let noSerieInput = document.getElementById("NoSerie");

// Campos
let fechaDatos = document.getElementById("FechaDatos");
let coolingType = document.getElementById("CoolingType");
let overElevation = document.getElementById("OverElevation");
let factEnf = document.getElementById("FactEnf");
let posAt = document.getElementById("PosAt");
let posBt = document.getElementById("PosBt");
let posTer = document.getElementById("PosTer");
let intervalo = document.getElementById("Intervalo");
let altitudeF1 = document.getElementById("AltitudeF1");
let altitudeF2 = document.getElementById("AltitudeF2");
let capacidad = document.getElementById("Capacidad");
let factAlt = document.getElementById("FactAlt");
let devanadoSplit = document.getElementById("DevanadoSplit");
let porcCarga = document.getElementById("PorcCarga");
let sobrecarga = document.getElementById("Sobrecarga");
let perdidas = document.getElementById("Perdidas");
let cantTermoPares = document.getElementById("CantTermoPares");
let corriente = document.getElementById("Corriente");
let titulo = document.getElementById("titulo");
let color = document.getElementById("color");
let estabilizado = document.getElementById("estabilizado");

// Cosas
let tableAt = $("#tableAt");
let tableBt = $("#tableBt");
let tableTer = $("#tableTer");
let divAT = document.getElementById("DivAT");
let divBT = document.getElementById("DivBT");
let divTER = document.getElementById("DivTer");
let positionATInput = document.getElementById("positionAT");
let nominalATInput = document.getElementById("nominalAT");
let positionBTInput = document.getElementById("positionBT");
let nominalBTInput = document.getElementById("nominalBT");
let positionTERInput = document.getElementById("positionTER");
let nominalTERInput = document.getElementById("nominalTER");


$("#NoSerie").focus();

$("#NoSerie").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $("#btnRequest").click();
    }
});

coolingType.addEventListener("change", function () {
    if (viewModel != undefined) {
        if (coolingType.value == '') {
            overElevation.value = '';
            factEnf.value = '';
        } else {
            let index = viewModel.CoolingTypes.indexOf(coolingType.value);
            overElevation.value = index >= 0 ? viewModel.OverElevations[index] : '';
            factEnf.value = coolingType.value.includes('ONAN') ? 0.04 : 0.06;
        }
    } else {
        overElevation.value = '';
        factEnf.value = '';
    }
});

btnRequest.addEventListener("click", function () {
    if ($("#form_serie").data("kendoValidator").validate()) {
        $("#loader").css("display", "block");
        if (noSerieInput.value) {
            GetDataJSON().then(
                data => {
                    if (data.response.Code !== -1) {
                        noSerieInput.disabled = true;
                        viewModel = data.response.Structure;
                        LoadPrimaryTable(viewModel.StabilizationDatas);
                        PreLoadForm();
                        rows = viewModel.StabilizationDatas.length;
                        btnRequest.disabled = true;
                        btnNew.disabled = false;
                        btnClear1.disabled = false;
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
        }
    }
});

btnNew.addEventListener("click", function () {
    titulo.innerHTML = "Registrar Datos de Estabilización (ETD)";
    fechaDatos.disabled = false;
    coolingType.disabled = false;
    overElevation.disabled = true;
    factEnf.disabled = true;
    posAt.disabled = false;
    posBt.disabled = false;
    posTer.disabled = false;
    intervalo.disabled = false;
    altitudeF1.disabled = true;
    altitudeF2.disabled = true;
    capacidad.disabled = false;
    factAlt.disabled = true;
    devanadoSplit.disabled = false;
    porcCarga.disabled = false;
    sobrecarga.disabled = false;
    perdidas.disabled = false;
    cantTermoPares.disabled = false;
    corriente.disabled = false;

    porcCarga.value = 100;
    cantTermoPares.value = 1;
    color.setAttribute('style', 'display: none');
    estabilizado.setAttribute('style', 'display: none');;
    newETD = true;
    btnNew.disabled = true;
    btnStart.disabled = false;
});

btnStart.addEventListener("click", function () {
    btnStart.disabled = true;
    $("#loader").css("display", "block");
    let ResultValidations = $("#form_detalle").data("kendoValidator").validate();
    let validPos = validPositions();
    if (ResultValidations && validPos == '') {
        LoadSelectETD(newETD);
        postData(domain + "/Retd/Iniciar/", selectETD)
            .then(data => {
                if (data.response.Code === -1) {
                    ShowFailedMessage(data.response.description);
                    btnStart.disabled = false;
                    $("#loader").css("display", "none");
                }
                else{
                    selectETD = data.response.Structure;
                    btnValidate.disabled = false;
                    perdidas.value = selectETD.Perdidas;
                    LoadSecundaryTable(selectETD)
                    if (data.response.Code == -2) {
                        ShowWarningMessage("Inicio Exitoso pero con advertencias. " + data.response.description)
                    } else {
                        ShowSuccessMessage("Inicio Exitoso. Por favor Verifique y complete los datos de la tabla inferior.")
                    }
                }
                $("#loader").css("display", "none");
            });
    } else {
        if (validPos != '') {
            ShowFailedMessage(validPos);
        }
        btnStart.disabled = false;
        $("#loader").css("display", "none");
    }
});

btnValidate.addEventListener("click", function () {
    btnValidate.disabled = true;
    $("#loader").css("display", "block");
    let error = ValidateValidate();
    if (error == '') {
        LoadSelectETDDetails(newETD);
        postData(domain + "/Retd/Validate/", selectETD)
            .then(data2 => {
                if (data2.response.Code === -1) {
                    ShowFailedMessage(data2.response.Description);
                    btnValidate.disabled = false;
                    $("#loader").css("display", "none");
                }
                else {
                    selectETD = data2.response.Structure;
                    btnValidate.disabled = false;
                    UploadValidateData();
                    ShowSuccessMessage("Validacion Exitosa.")
                    btnSave.disabled = false;
                    if (selectETD.estabilizado) {
                        color.innerHTML = 'VERDE';
                        estabilizado.innerHTML = 'ESTABILIZADO'
                    } else{
                        color.innerHTML = 'ROJO';
                        estabilizado.innerHTML = 'NO ESTABILIZADO'
                    }
                }
                $("#loader").css("display", "none");
            });
    } else {
        btnValidate.disabled = false;
        ShowFailedMessage(error);
        $("#loader").css("display", "none");
    }
});

btnSave.addEventListener("click", function () {
    btnSave.disabled = true;
    $("#loader").css("display", "block");
    let error = ValidateValidate();
    if (error == '') {
        LoadSelectETDDetails(newETD);
        postData(domain + "/Retd/Save/", selectETD)
            .then(data2 => {
                if (data2.response.Code === -1) {
                    ShowFailedMessage(data2.response.Description);
                    btnValidate.disabled = false;
                    $("#loader").css("display", "none");
                }
                else {
                    selectETD = data2.response.Structure;
                    btnValidate.disabled = false;
                    ShowSuccessMessage("Guardado Exitoso.")
                    setTimeout(location.reload(), 3000);
                }
                $("#loader").css("display", "none");
            });
    } else {
        btnValidate.disabled = false;
        ShowFailedMessage(error);
        $("#loader").css("display", "none");
    }
});


function validPositions() {
    if (posAt.value != '' && posBt.value != '' && posTer.value != '') {
        return 'Solo puede colocar posiciones en 2 tipos de tension';
    } else {
        return '';
    }
}

// Cierra un dato
function close(posi) {
    if (viewModel.StabilizationDatas[posi] != undefined) {
        Swal.fire({
            title: '¿Esta seguro de cerrar este registro?',
            showDenyButton: true,
            showCancelButton: true,
            confirmButtonText: 'Cerrar',
            denyButtonText: `Cancelar`,
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                GetDataJSON().then(
                    data => {
                        if (data.response.Code !== -1) {
                            Swal.fire('¡Registro Cerrado!', '', 'success');
                            setTimeout(location.reload(), 2000);
                        }
                        else {
                            ShowFailedMessage(data.response.Description);
                        }
                        $("#loader").css("display", "none");
                    }
                );
            } else if (result.isDenied) {
                Swal.fire('¡Cerrado Cancelado!', '', 'info')
            }
        })
    } else {
        ShowFailedMessage('Error dato de estabilizacion no encontrado');
    }
}

// Llena los promedios con los datos de la validacion
function UploadValidateData() {
    // Llenando los promedios
    let promSup = selectETD.cantTermoPares + 2;
    let promInf = promSup + selectETD.cantTermoPares + 1;
    let promAmb = promInf + 4;
    let promAOR = promAmb + 2;
    let promTOR = promAOR + 1;
    let promBOR = promTOR + 1;
    let corrAOR = promBOR + 1;
    let corrTOR = corrAOR + 1;

    for (var i = 0; i < 52; i++) {
        $('#inputch_' + promSup + '_' + i).val(selectETD.StabilizationDataDetails.PromRadSup[i] == null ? '' : selectETD.StabilizationDataDetails.PromRadSup[i]);
        $('#inputch_' + promInf + '_' + i).val(selectETD.StabilizationDataDetails.PromRadInf[i] == null ? '' : selectETD.StabilizationDataDetails.PromRadInf[i]);
        $('#inputch_' + promAmb + '_' + i).val(selectETD.StabilizationDataDetails.AmbienteProm[i] == null ? '' : selectETD.StabilizationDataDetails.AmbienteProm[i]);
        $('#inputch_' + promAOR + '_' + i).val(selectETD.StabilizationDataDetails.Aor[i] == null ? '' : selectETD.StabilizationDataDetails.Aor[i]);
        $('#inputch_' + promTOR + '_' + i).val(selectETD.StabilizationDataDetails.Tor[i] == null ? '' : selectETD.StabilizationDataDetails.Tor[i]);
        $('#inputch_' + promBOR + '_' + i).val(selectETD.StabilizationDataDetails.Bor[i] == null ? '' : selectETD.StabilizationDataDetails.Bor[i]);
        $('#inputch_' + corrAOR + '_' + i).val(selectETD.StabilizationDataDetails.AorCorr[i] == null ? '' : selectETD.StabilizationDataDetails.AorCorr[i]);
        $('#inputch_' + corrTOR + '_' + i).val(selectETD.StabilizationDataDetails.TorCorr[i] == null ? '' : selectETD.StabilizationDataDetails.TorCorr[i]);
    }
}

// Llena el objeto selectETD con los datos del formulario
function LoadSelectETD(nuevo) {
    if (nuevo) {
        selectETD = new Object();
        selectETD.StabilizationDataDetails = new Object();
        selectETD.NoSerie = noSerieInput.value;
    }

    selectETD.FechaDatos = fechaDatos.value;
    selectETD.CoolingType = coolingType.value;
    selectETD.OverElevation = overElevation.value;
    selectETD.FactEnf = factEnf.value;
    selectETD.PosAt = posAt.value;
    selectETD.PosBt = posBt.value;
    selectETD.PosTer = posTer.value;
    selectETD.Intervalo = intervalo.value;
    selectETD.UmIntervalo = intervalo.value == 1 ? 'Hr' : 'Min';
    selectETD.AltitudeF1 = altitudeF1.value;
    selectETD.AltitudeF2 = altitudeF2.value;
    selectETD.Capacidad = capacidad.value;
    selectETD.FactAlt = factAlt.value;
    selectETD.DevanadoSplit = devanadoSplit.value;
    selectETD.PorcCarga = porcCarga.value;
    selectETD.Sobrecarga = sobrecarga.value;
    selectETD.Perdidas = 0;
    selectETD.CantTermoPares = cantTermoPares.value;
    selectETD.Corriente = corriente.value;

}

function LoadSelectETDDetails(nuevo) {
    if (nuevo) {
        selectETD.StabilizationDataDetails = new Object();
    }

    // KwMedidos
    let pos = 0
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.KwMedidos.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;

    // AmpMedidos
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.AmpMedidos.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;

    // Superiores
    if (selectETD.cantTermoPares >= 1) {
        selectETD.StabilizationDataDetails.CanalSup1 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabSupRadBco1.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 2) {
        selectETD.StabilizationDataDetails.CanalSup2 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabSupRadBco2.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 3) {
        selectETD.StabilizationDataDetails.CanalSup3 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabSupRadBco3.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 4) {
        selectETD.StabilizationDataDetails.CanalSup4 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabSupRadBco4.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 5) {
        selectETD.StabilizationDataDetails.CanalSup5 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabSupRadBco5.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 6) {
        selectETD.StabilizationDataDetails.CanalSup6 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabSupRadBco6.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 7) {
        selectETD.StabilizationDataDetails.CanalSup7 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabSupRadBco7.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 8) {
        selectETD.StabilizationDataDetails.CanalSup8 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabSupRadBco8.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 9) {
        selectETD.StabilizationDataDetails.CanalSup9 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabSupRadBco9.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 10) {
        selectETD.StabilizationDataDetails.CanalSup10 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabSupRadBco10.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    // PromRadSup
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.PromRadSup.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;

    // Inferiores
    if (selectETD.cantTermoPares >= 1) {
        selectETD.StabilizationDataDetails.CanalInf1 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabInfRadBco1.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 2) {
        selectETD.StabilizationDataDetails.CanalInf2 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabInfRadBco2.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 3) {
        selectETD.StabilizationDataDetails.CanalInf3 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabInfRadBco3.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 4) {
        selectETD.StabilizationDataDetails.CanalInf4 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabInfRadBco4.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 5) {
        selectETD.StabilizationDataDetails.CanalInf5 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabInfRadBco5.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 6) {
        selectETD.StabilizationDataDetails.CanalInf6 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabInfRadBco6.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 7) {
        selectETD.StabilizationDataDetails.CanalInf7 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabInfRadBco7.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 8) {
        selectETD.StabilizationDataDetails.CanalInf8 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabInfRadBco8.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 9) {
        selectETD.StabilizationDataDetails.CanalInf9 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabInfRadBco9.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    if (selectETD.cantTermoPares >= 10) {
        selectETD.StabilizationDataDetails.CanalInf10 = $('#inputcanal_' + pos).val() == '' ? null : $('#inputcanal_' + pos).val();
        for (let i = 0; i < 52; i++) {
            selectETD.StabilizationDataDetails.CabInfRadBco10.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
        }
        pos++;
    }

    // PromRadInf
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.PromRadInf.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;

    // Ambientes
    selectETD.StabilizationDataDetails.CanalAmb1 = ($('#inputcanal_' + pos).val() != '') ? $('#inputcanal_' + pos).val() : null;
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.Ambiente1.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;
    selectETD.StabilizationDataDetails.CanalAmb2 = ($('#inputcanal_' + pos).val() != '') ? $('#inputcanal_' + pos).val() : null;
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.Ambiente2.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;
    selectETD.StabilizationDataDetails.CanalAmb3 = ($('#inputcanal_' + pos).val() != '') ? $('#inputcanal_' + pos).val() : null;
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.Ambiente3.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;

    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.Ambiente3.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;

    selectETD.StabilizationDataDetails.AmbienteProm = ($('#inputcanal_' + pos).val() != '') ? $('#inputcanal_' + pos).val() : null;
    pos++;

    // TempTapa
    selectETD.StabilizationDataDetails.CanalTtapa = ($('#inputcanal_' + pos).val() != '') ? $('#inputcanal_' + pos).val() : null;
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.TempTapa.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;

    // AOR
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.Aor.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;

    // TOR
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.Tor.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;

    // BOR
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.Bor.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;

    // AOR CORR
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.AorCorr.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;

    // TOR CORR
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.TorCorr.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;

    // canal temp instr
    selectETD.StabilizationDataDetails.CanalInst1 = ($('#inputcanal_' + pos).val() != '') ? $('#inputcanal_' + pos).val() : null;
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.TempInstr1.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;
    selectETD.StabilizationDataDetails.CanalInst2 = ($('#inputcanal_' + pos).val() != '') ? $('#inputcanal_' + pos).val() : null;
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.TempInstr2.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;
    selectETD.StabilizationDataDetails.CanalInst3 = ($('#inputcanal_' + pos).val() != '') ? $('#inputcanal_' + pos).val() : null;
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.TempInstr3.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;

    // VerifVent1
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.VerifVent1.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;

    // VerifVent2
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.VerifVent2.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
    pos++;

    // Presion
    for (let i = 0; i < 52; i++) {
        selectETD.StabilizationDataDetails.Presion.push($('#inputch_' + pos + '_' + i).val() == '' ? null : $('#inputch_' + pos + '_' + i).val());
    }
}

// Inicializa el formulario
function PreLoadForm() {
    altitudeF1.value = viewModel.Altitude1;
    altitudeF2.value = viewModel.Altitude2;
    factAlt.value = viewModel.AltitudeFactor;

    $("#PosAt option[value!='']").each(function () {
        $(this).remove();
    });

    $("#PosBt option[value!='']").each(function () {
        $(this).remove();
    });

    $("#PosTer option[value!='']").each(function () {
        $(this).remove();
    });

    $("#CoolingType option[value!='']").each(function () {
        $(this).remove();
    });

    $.each(viewModel.Positions.AltaTension, function (i, val) {
        $("#PosAt").append("<option value='" + val + "'>" + val + "</option>");
    });

    $.each(viewModel.Positions.BajaTension, function (i, val) {
        $("#PosBt").append("<option value='" + val + "'>" + val + "</option>");
    });

    $.each(viewModel.Positions.Terciario, function (i, val) {
        $("#PosTer").append("<option value='" + val + "'>" + val + "</option>");
    });

    $.each(viewModel.CoolingTypes, function (i, val) {
        $("#CoolingType").append("<option value='" + val + "'>" + val + "</option>");
    });
}

// Boton de editar
function EditRowData(posicion) {
    selectETD = viewModel.StabilizationDatas[posicion];
    selectRow = posicion;

    if (!selectETD.Estatus) {
        titulo.innerHTML = "Editar Datos de Estabilización (ETD)";
        fechaDatos.disabled = false;
        coolingType.disabled = false;
        overElevation.disabled = true;
        factEnf.disabled = true;
        posAt.disabled = false;
        posBt.disabled = false;
        posTer.disabled = false;
        intervalo.disabled = true;
        altitudeF1.disabled = true;
        altitudeF2.disabled = true;
        capacidad.disabled = false;
        factAlt.disabled = true;
        devanadoSplit.disabled = false;
        porcCarga.disabled = false;
        sobrecarga.disabled = false;
        perdidas.disabled = false;
        cantTermoPares.disabled = true;
        corriente.disabled = false;
    } else {
        titulo.innerHTML = "Consultar Datos de Estabilización (ETD)";
        fechaDatos.disabled = true;
        coolingType.disabled = false;
        overElevation.disabled = true;
        factEnf.disabled = true;
        posAt.disabled = false;
        posBt.disabled = false;
        posTer.disabled = false;
        intervalo.disabled = true;
        altitudeF1.disabled = true;
        altitudeF2.disabled = true;
        capacidad.disabled = false;
        factAlt.disabled = true;
        devanadoSplit.disabled = false;
        porcCarga.disabled = false;
        sobrecarga.disabled = false;
        perdidas.disabled = false;
        cantTermoPares.disabled = true;
        corriente.disabled = false;
        btnValidate.disabled = true;
        btnSave.disabled = true;
    }
    let estable = false;
    for (let item of selectETD.StabilizationDataDetails.Estable) {
        if (item) {
            estable = true;
        }
    }

    color.setAttribute('style', 'display: block');
    estabilizado.setAttribute('style', 'display: block');

    if (estable) {
        color.innerHTML = "Verde";
        estabilizado.innerHTML = "ESTABILIZADO";
    } else {
        color.innerHTML = "Rojo";
        estabilizado.innerHTML = "NO ESTABILIZADO";
    }

    LoadForm(selectETD);

    LoadSecundaryTable(selectETD);
}

// Cargar los datos en el formulario
function LoadForm(data) {
    fechaDatos.disabled = data.FechaDatos;
    coolingType.disabled = data.CoolingType;
    overElevation.disabled = data.OverElevation;
    factEnf.disabled = data.FactEnf;
    posAt.disabled = data.PosAt;
    posBt.disabled = data.PosBt;
    posTer.disabled = data.PosTer;
    intervalo.disabled = data.Intervalo;
    altitudeF1.disabled = data.AltitudeF1;
    altitudeF2.disabled = data.AltitudeF2;
    capacidad.disabled = data.Capacidad;
    factAlt.disabled = data.FactAlt;
    devanadoSplit.disabled = data.DevanadoSplit;
    porcCarga.disabled = data.PorcCarga;
    sobrecarga.disabled = data.Sobrecarga;
    perdidas.disabled = data.Perdidas;
    cantTermoPares.disabled = data.CantTermoPares;
    corriente.disabled = data.Corriente;
}

// Cargar tabla principal
function LoadPrimaryTable(datas) {
    for (var i = 0; i < datas.length; i++) {
        let fila = AddPrimaryRow(datas[i], i)
        $(fila).appendTo($("#CFTable"));
    }
}

// Cargar tabla Secundaria
function LoadSecundaryTable(data) {
    data.StabilizationDataDetails.FechaHora.forEach(function (FechaHora, index) {
        let time = new Date(FechaHora);
        $('$thora_' + index).html(time.getHours() + ":" + time.getMinutes() + ":" + time.getSeconds());
    });
    // renglones
    let filaNum = 0;
    let content = AddSecundaryRow(data.StabilizationDataDetails.KwMedidos, filaNum, 'kW Medidos');
    filaNum++;
    content += AddSecundaryRow(data.StabilizationDataDetails.AmpMedidos, filaNum, 'Amp. Medidos');
    filaNum++;
    // CSRB1
    content += AddSecundaryRow(data.StabilizationDataDetails.CabSupRadBco1, filaNum, data.StabilizationDataDetails.CanalSup1, 'Cab. Sup. Rad. BCO. 1');
    filaNum++;

    // CSRB2
    if (data.CantTermoPares >= 2) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabSupRadBco2, filaNum, data.StabilizationDataDetails.CanalSup2, 'Cab. Sup. Rad. BCO. 2');
        filaNum++;
    }

    // CSRB3
    if (data.CantTermoPares >= 3) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabSupRadBco3, filaNum, data.StabilizationDataDetails.CanalSup3, 'Cab. Sup. Rad. BCO. 3');
        filaNum++;
    }

    // CSRB4
    if (data.CantTermoPares >= 4) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabSupRadBco4, filaNum, data.StabilizationDataDetails.CanalSup4, 'Cab. Sup. Rad. BCO. 4');
        filaNum++;
    }

    // CSRB5
    if (data.CantTermoPares >= 5) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabSupRadBco5, filaNum, data.StabilizationDataDetails.CanalSup5, 'Cab. Sup. Rad. BCO. 5');
        filaNum++;
    }

    // CSRB6
    if (data.CantTermoPares >= 6) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabSupRadBco6, filaNum, data.StabilizationDataDetails.CanalSup6, 'Cab. Sup. Rad. BCO. 6');
        filaNum++;
    }

    // CSRB7
    if (data.CantTermoPares >= 7) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabSupRadBco7, filaNum, data.StabilizationDataDetails.CanalSup7, 'Cab. Sup. Rad. BCO. 7');
        filaNum++;
    }

    // CSRB8
    if (data.CantTermoPares >= 8) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabSupRadBco8, filaNum, data.StabilizationDataDetails.CanalSup8, 'Cab. Sup. Rad. BCO. 8');
        filaNum++;
    }

    // CSRB9
    if (data.CantTermoPares >= 9) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabSupRadBco9, filaNum, data.StabilizationDataDetails.CanalSup9, 'Cab. Sup. Rad. BCO. 9');
        filaNum++;
    }

    // CSRB10
    if (data.CantTermoPares >= 10) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabSupRadBco10, filaNum, data.StabilizationDataDetails.CanalSup10, 'Cab. Sup. Rad. BCO. 10');
        filaNum++;
    }

    content += AddSecundaryRow(data.StabilizationDataDetails.PromRadSup, filaNum, 'PROMEDIO Rad. Sup.', true);
    filaNum++;


    content += AddSecundaryRow(data.StabilizationDataDetails.CabInfRadBco1, filaNum, data.StabilizationDataDetails.CanalInf1, 'Cab. Inf. Rad. BCO. 1');
    filaNum++;

    // CIRB2
    if (data.CantTermoPares >= 2) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabInfRadBco2, filaNum, data.StabilizationDataDetails.CanalInf2, 'Cab. Inf. Rad. BCO. 2');
        filaNum++;
    }

    // CIRB3
    if (data.CantTermoPares >= 3) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabInfRadBco3, filaNum, data.StabilizationDataDetails.CanalInf3, 'Cab. Inf. Rad. BCO. 3');
        filaNum++;
    }

    // CIRB4
    if (data.CantTermoPares >= 4) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabInfRadBco4, filaNum, data.StabilizationDataDetails.CanalInf4, 'Cab. Inf. Rad. BCO. 4');
        filaNum++;
    }

    // CIRB5
    if (data.CantTermoPares >= 5) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabInfRadBco5, filaNum, data.StabilizationDataDetails.CanalInf5, 'Cab. Inf. Rad. BCO. 5');
        filaNum++;
    }

    // CIRB6
    if (data.CantTermoPares >= 6) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabInfRadBco6, filaNum, data.StabilizationDataDetails.CanalInf6, 'Cab. Inf. Rad. BCO. 6');
        filaNum++;
    }

    // CIRB7
    if (data.CantTermoPares >= 7) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabInfRadBco7, filaNum, data.StabilizationDataDetails.CanalInf7, 'Cab. Inf. Rad. BCO. 7');
        filaNum++;
    }

    // CIRB8
    if (data.CantTermoPares >= 8) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabInfRadBco8, filaNum, data.StabilizationDataDetails.CanalInf8, 'Cab. Inf. Rad. BCO. 8');
        filaNum++;
    }

    // CIRB9
    if (data.CantTermoPares >= 9) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabInfRadBco9, filaNum, data.StabilizationDataDetails.CanalInf9, 'Cab. Inf. Rad. BCO. 9');
        filaNum++;
    }

    // CIRB10
    if (data.CantTermoPares >= 10) {
        content += AddSecundaryRow(data.StabilizationDataDetails.CabInfRadBco10, filaNum, data.StabilizationDataDetails.CanalInf10, 'Cab. Inf. Rad. BCO. 10');
        filaNum++;
    }



    content += AddSecundaryRow(data.StabilizationDataDetails.PromRadInf, filaNum, 'PROMEDIO Rad. Inf.', true);
    filaNum++;



    content += AddSecundaryRow(data.StabilizationDataDetails.Ambiente1, filaNum, data.StabilizationDataDetails.CanalAmb1, 'Ambiente No.1');
    filaNum++;

    content += AddSecundaryRow(data.StabilizationDataDetails.Ambiente2, filaNum, data.StabilizationDataDetails.CanalAmb2, 'Ambiente No.2');
    filaNum++;

    content += AddSecundaryRow(data.StabilizationDataDetails.Ambiente3, filaNum, data.StabilizationDataDetails.CanalAmb3, 'Ambiente No.3');
    filaNum++;

    content += AddSecundaryRow(data.StabilizationDataDetails.AmbienteProm, filaNum, 'Ambiente PROMEDIO', true);
    filaNum++;

    content += AddSecundaryRow(data.StabilizationDataDetails.TempTapa, filaNum, data.StabilizationDataDetails.CanalTtapa, 'Temperatura en Tapa');
    filaNum++;

    content += AddSecundaryRow(data.StabilizationDataDetails.Aor, filaNum, 'Elevación del  Aceite  Promedio (AOR)', true);
    filaNum++;

    content += AddSecundaryRow(data.StabilizationDataDetails.Tor, filaNum, 'Elevación del  Aceite  Superior  (TOR)', true);
    filaNum++;

    content += AddSecundaryRow(data.StabilizationDataDetails.Bor, filaNum, 'Elevación del aceite Inferior (BOR)', true);
    filaNum++;

    content += AddSecundaryRow(data.StabilizationDataDetails.AorCorr, filaNum, 'AOR cor. x Altitud a ' + data.AltitudeF1 + ' ' + data.AltitudeF2, true);
    filaNum++;

    content += AddSecundaryRow(data.StabilizationDataDetails.TorCorr, filaNum, 'TOR cor. x Altitud a ' + data.AltitudeF1 + ' ' + data.AltitudeF2, true);
    filaNum++;

    content += AddSecundaryRow(data.StabilizationDataDetails.TempInstr1, filaNum, data.StabilizationDataDetails.CanalInst1, 'Temp. En Instrumento');
    filaNum++;

    content += AddSecundaryRow(data.StabilizationDataDetails.TempInstr2, filaNum, data.StabilizationDataDetails.CanalInst2, 'Temp. En Instrumento');
    filaNum++;

    content += AddSecundaryRow(data.StabilizationDataDetails.TempInstr3, filaNum, data.StabilizationDataDetails.CanalInst3, 'Temp. En Instrumento');
    filaNum++;

    content += AddSecundaryRow(data.StabilizationDataDetails.VerifVent1, filaNum, 'Verificación de motobombas (flujo, funcionamiento, etc)', false, true);
    filaNum++;

    content += AddSecundaryRow(data.StabilizationDataDetails.VerifVent2, filaNum, 'Verificación de motobombas (flujo, funcionamiento, etc)', false, true);
    filaNum++;

    content += AddSecundaryRow(data.StabilizationDataDetails.Presion, filaNum, 'Presión');

    $(content).appendTo($("#SecondTable"));
}

function AddPrimaryRow(row, pos) {
    return '<tr id ="Tr_' + pos + '"> '
        + '<td id="ct_' + pos + '">' + row.CoolingType + '</td>'
        + '<td id="se_' + pos + '">' + row.Sobrecarga +'</td>'
        + '<td id="dt_' + pos + '">' + row.FechaDatos +'</td>'
        + '<td id="at_' + pos + '">' + row.PosAt +'</td>'
        + '<td id="bt_' + pos + '">' + row.PosBt +'</td>'
        + '<td id="ter_' + pos + '">' + row.PosTer +'</td>'
        + '<td id="cap_' + pos + '">' + row.Capacidad +'</td>'
        + '<td id="pe_' + pos + '">' + row.Perdidas +'</td>'
        + '<td id="es_' + pos + '" style="background-color : ' + (row.Estatus ? 'red' : 'green') + '"></td>'
        + '<td >'
        + '<input type="button" class="btn btn-primary" id="btnEdit_' + pos + '" value="Editar" onclick="EditRowData(' + pos + ')" />'
        + '<input type="button" class="btn btn-primary" id="btnCerrar_' + pos + '" value="Cerrar" onclick="CloseData(' + pos + ')" style="display:' + (row.Estatus ? 'none' : 'block') + '"/>'
        + '</td>'
        + '</tr>';
}

function AddSecundaryRow(row, pos, canal, A, bloqueo = false, select = false) {
    let fila =
        '<tr id ="Tr2_' + pos + '"> '
        + '<td id="cho_' + pos + '">' + A + '</td>';
    if (canal == null) {
        fila += '<td id="cc_' + pos + '" ></td>';
    } else {
        fila += '<td id="cc_' + pos + '" ><input type="number" id="inputcanal_' + pos + '"   value="' + canal + '" /></td>';
    }

    for (var i = 0; i < 52; i++) {
        if (select) {
            fila += '<td id="ch' + pos + '_' + i + '"><select id="inputch_' + pos + '_' + i + '" class = "form-select form-select-sm"><option value= "" selected> Seleccione...</option>' +
                '<option value="true">On</option><option value="false">Off</option></select></td>';
        }
        else {
            fila += '<td id="ch' + pos + '_' + i + '"><input type="number" ' + (bloqueo ? 'disabled' : '') + ' id="inputch_' + pos + '_' + i + '"   value="' + (row[i] == null ? '' : row[i]) + '" /></td>';
        }
    }

    fila += '</tr>';
    return fila;
}

// Validar antes del validar
function ValidateValidate() {
    let msg = '';

    // Validando canales
    msg += ValidateCanales(selectETD.CantTermoPares);

    // Validando columnas
    msg += ValidateColumns(selectETD.CantTermoPares);

    return msg;
}

// Valida las columnas
function ValidateColumns(termos) {
    var result = '';
    let existVal = false;
    let existNull = false;
    let filitas = 20 + (termos * 2);

    let inhabilitadas = [];
    inhabilitadas.push(termos + 2);
    inhabilitadas.push(inhabilitadas[0] + termos + 1);
    inhabilitadas.push(inhabilitadas[1] + 4);
    inhabilitadas.push(inhabilitadas[2] + 2);
    inhabilitadas.push(inhabilitadas[3] + 1);
    inhabilitadas.push(inhabilitadas[4] + 1);
    inhabilitadas.push(inhabilitadas[5] + 1);
    inhabilitadas.push(inhabilitadas[6] + 1);
    inhabilitadas.push(inhabilitadas[7] + 6);

    for (var i = 0; i < 52; i++) {
        existVal = false;
        existNull = false;
        for (var f = 0; f < filitas; f++) {
            if (inhabilitadas.indexOf(f) == -1) {
                if ($('$inputch_' + f + '_' + i).val() == undefined || $('$inputch_' + f + '_' + i).val() == null || $('$inputch_' + f + '_' + i).val() == '') {
                    existNull = true;
                } else {
                    existVal = true;
                }
            }
        }

        if (existVal && existNull) {
            result += 'Todos los campos de la columna '+ (i+1) + ' despues de Canal son requeridos';
        }
    }

    return result;
}

// Valida los canales
function ValidateCanales(termos) {
    let result = '';
    let exist = false;
    let sup1;
    let sup2;
    let sup3;
    let sup4;
    let sup5;
    let sup6;
    let sup7;
    let sup8;
    let sup9;
    let sup10;
    let inf1;
    let inf2;
    let inf3;
    let inf4;
    let inf5;
    let inf6;
    let inf7;
    let inf8;
    let inf9;
    let inf10;
    let amb1;
    let amb2;
    let amb3;
    let tempTap;
    let tempInst1;
    let tempInst2;
    let tempInst3;
    switch (selectETD.CantTermoPares) {
        case 1:
            sup1 = $('$inputcanal_2').val();
            inf1 = $('$inputcanal_4').val();
            amb1 = $('$inputcanal_6').val();
            amb2 = $('$inputcanal_7').val();
            amb3 = $('$inputcanal_8').val();
            tempTap = $('$inputcanal_10').val();
            tempInst1 = $('$inputcanal_16').val();
            tempInst2 = $('$inputcanal_17').val();
            tempInst3 = $('$inputcanal_18').val();

            // Superior
            exist = false;
            if (sup1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_2_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 1 requerido' : '';
            }

            // Inferior
            exist = false;
            if (inf1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_4_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 1 requerido' : '';
            }

            // Ambientes
            exist = false;
            if (amb1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_6_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 1 requerido' : '';
            }

            exist = false;
            if (amb2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_7_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 2  requerido' : '';
            }

            exist = false;
            if (amb3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_8_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 3 requerido' : '';
            }

            // Tem[ tap]
            exist = false;
            if (tempTap == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_10_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura requerido' : '';
            }

            // Temp Inst
            exist = false;
            if (tempInst1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_16_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 1 requerido' : '';
            }

            exist = false;
            if (tempInst2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_17_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 2  requerido' : '';
            }

            exist = false;
            if (tempInst3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_18_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 3 requerido' : '';
            }

            break;
        case 2:
            sup1 = $('$inputcanal_2').val();
            sup2 = $('$inputcanal_3').val();
            inf1 = $('$inputcanal_5').val();
            inf2 = $('$inputcanal_6').val();
            amb1 = $('$inputcanal_8').val();
            amb2 = $('$inputcanal_9').val();
            amb3 = $('$inputcanal_10').val();
            tempTap = $('$inputcanal_12').val();
            tempInst1 = $('$inputcanal_18').val();
            tempInst2 = $('$inputcanal_19').val();
            tempInst3 = $('$inputcanal_20').val();

            // Superior
            exist = false;
            if (sup1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_2_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 1 requerido' : '';
            }

            exist = false;
            if (sup2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_3_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 2 requerido' : '';
            }

            // Inferior
            exist = false;
            if (inf1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_5_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 1 requerido' : '';
            }

            exist = false;
            if (inf2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_6_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 2 requerido' : '';
            }

            // Ambientes
            exist = false;
            if (amb1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_8_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 1 requerido' : '';
            }

            exist = false;
            if (amb2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_9_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 2  requerido' : '';
            }

            exist = false;
            if (amb3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_10_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 3 requerido' : '';
            }

            // Tem[ tap]
            exist = false;
            if (tempTap == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_12_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura requerido' : '';
            }

            // Temp Inst
            exist = false;
            if (tempInst1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_18_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 1 requerido' : '';
            }

            exist = false;
            if (tempInst2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_19_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 2  requerido' : '';
            }

            exist = false;
            if (tempInst3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_20_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 3 requerido' : '';
            }

            break;
        case 3:
            sup1 = $('$inputcanal_2').val();
            sup2 = $('$inputcanal_3').val();
            sup3 = $('$inputcanal_4').val();
            inf1 = $('$inputcanal_6').val();
            inf2 = $('$inputcanal_7').val();
            inf3 = $('$inputcanal_8').val();
            amb1 = $('$inputcanal_10').val();
            amb2 = $('$inputcanal_11').val();
            amb3 = $('$inputcanal_12').val();
            tempTap = $('$inputcanal_14').val();
            tempInst1 = $('$inputcanal_20').val();
            tempInst2 = $('$inputcanal_21').val();
            tempInst3 = $('$inputcanal_22').val();

            // Superior
            exist = false;
            if (sup1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_2_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 1 requerido' : '';
            }

            exist = false;
            if (sup2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_3_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 2 requerido' : '';
            }

            exist = false;
            if (sup3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_4_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 3 requerido' : '';
            }

            // Inferior
            exist = false;
            if (inf1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_6_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 1 requerido' : '';
            }

            exist = false;
            if (inf2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_7_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 2 requerido' : '';
            }

            exist = false;
            if (inf3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_8_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 3 requerido' : '';
            }

            // Ambientes
            exist = false;
            if (amb1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_10_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 1 requerido' : '';
            }

            exist = false;
            if (amb2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_11_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 2  requerido' : '';
            }

            exist = false;
            if (amb3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_12_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 3 requerido' : '';
            }

            // Tem[ tap]
            exist = false;
            if (tempTap == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_14_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura requerido' : '';
            }

            // Temp Inst
            exist = false;
            if (tempInst1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_20_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 1 requerido' : '';
            }

            exist = false;
            if (tempInst2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_21_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 2  requerido' : '';
            }

            exist = false;
            if (tempInst3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_22_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 3 requerido' : '';
            }

            break;
        case 4:
            sup1 = $('$inputcanal_2').val();
            sup2 = $('$inputcanal_3').val();
            sup3 = $('$inputcanal_4').val();
            sup4 = $('$inputcanal_5').val();
            inf1 = $('$inputcanal_7').val();
            inf2 = $('$inputcanal_8').val();
            inf3 = $('$inputcanal_9').val();
            inf4 = $('$inputcanal_10').val();
            amb1 = $('$inputcanal_12').val();
            amb2 = $('$inputcanal_13').val();
            amb3 = $('$inputcanal_14').val();
            tempTap = $('$inputcanal_16').val();
            tempInst1 = $('$inputcanal_22').val();
            tempInst2 = $('$inputcanal_23').val();
            tempInst3 = $('$inputcanal_24').val();

            // Superior
            exist = false;
            if (sup1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_2_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 1 requerido' : '';
            }

            exist = false;
            if (sup2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_3_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 2 requerido' : '';
            }

            exist = false;
            if (sup3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_4_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 3 requerido' : '';
            }

            exist = false;
            if (sup4 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_5_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 4 requerido' : '';
            }

            // Inferior
            exist = false;
            if (inf1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_7_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 1 requerido' : '';
            }

            exist = false;
            if (inf2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_8_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 2 requerido' : '';
            }

            exist = false;
            if (inf3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_9_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 3 requerido' : '';
            }

            exist = false;
            if (inf4 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_10_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 3 requerido' : '';
            }

            // Ambientes
            exist = false;
            if (amb1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_12_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 1 requerido' : '';
            }

            exist = false;
            if (amb2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_13_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 2  requerido' : '';
            }

            exist = false;
            if (amb3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_14_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 3 requerido' : '';
            }

            // Tem[ tap]
            exist = false;
            if (tempTap == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_16_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura requerido' : '';
            }

            // Temp Inst
            exist = false;
            if (tempInst1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_22_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 1 requerido' : '';
            }

            exist = false;
            if (tempInst2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_23_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 2  requerido' : '';
            }

            exist = false;
            if (tempInst3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_24_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 3 requerido' : '';
            }

            break;
        case 5:
            sup1 = $('$inputcanal_2').val();
            sup2 = $('$inputcanal_3').val();
            sup3 = $('$inputcanal_4').val();
            sup4 = $('$inputcanal_5').val();
            sup5 = $('$inputcanal_6').val();
            inf1 = $('$inputcanal_8').val();
            inf2 = $('$inputcanal_9').val();
            inf3 = $('$inputcanal_10').val();
            inf4 = $('$inputcanal_11').val();
            inf5 = $('$inputcanal_12').val();
            amb1 = $('$inputcanal_14').val();
            amb2 = $('$inputcanal_15').val();
            amb3 = $('$inputcanal_16').val();
            tempTap = $('$inputcanal_18').val();
            tempInst1 = $('$inputcanal_24').val();
            tempInst2 = $('$inputcanal_25').val();
            tempInst3 = $('$inputcanal_26').val();

            // Superior
            exist = false;
            if (sup1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_2_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 1 requerido' : '';
            }

            exist = false;
            if (sup2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_3_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 2 requerido' : '';
            }

            exist = false;
            if (sup3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_4_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 3 requerido' : '';
            }

            exist = false;
            if (sup4 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_5_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 4 requerido' : '';
            }

            exist = false;
            if (sup5 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_6_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 5 requerido' : '';
            }

            // Inferior
            exist = false;
            if (inf1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_8_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 1 requerido' : '';
            }

            exist = false;
            if (inf2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_9_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 2 requerido' : '';
            }

            exist = false;
            if (inf3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_10_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 3 requerido' : '';
            }

            exist = false;
            if (inf4 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_11_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 4 requerido' : '';
            }

            exist = false;
            if (inf5 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_12_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 5 requerido' : '';
            }

            // Ambientes
            exist = false;
            if (amb1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_14_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 1 requerido' : '';
            }

            exist = false;
            if (amb2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_15_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 2  requerido' : '';
            }

            exist = false;
            if (amb3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_16_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 3 requerido' : '';
            }

            // Tem[ tap]
            exist = false;
            if (tempTap == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_18_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura requerido' : '';
            }

            // Temp Inst
            exist = false;
            if (tempInst1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_24_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 1 requerido' : '';
            }

            exist = false;
            if (tempInst2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_25_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 2  requerido' : '';
            }

            exist = false;
            if (tempInst3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_26_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 3 requerido' : '';
            }

            break;
        case 6:
            sup1 = $('$inputcanal_2').val();
            sup2 = $('$inputcanal_3').val();
            sup3 = $('$inputcanal_4').val();
            sup4 = $('$inputcanal_5').val();
            sup5 = $('$inputcanal_6').val();
            sup6 = $('$inputcanal_7').val();
            inf1 = $('$inputcanal_9').val();
            inf2 = $('$inputcanal_10').val();
            inf3 = $('$inputcanal_11').val();
            inf4 = $('$inputcanal_12').val();
            inf5 = $('$inputcanal_13').val();
            inf6 = $('$inputcanal_14').val();
            amb1 = $('$inputcanal_16').val();
            amb2 = $('$inputcanal_17').val();
            amb3 = $('$inputcanal_18').val();
            tempTap = $('$inputcanal_20').val();
            tempInst1 = $('$inputcanal_26').val();
            tempInst2 = $('$inputcanal_27').val();
            tempInst3 = $('$inputcanal_28').val();

            // Superior
            exist = false;
            if (sup1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_2_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 1 requerido' : '';
            }

            exist = false;
            if (sup2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_3_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 2 requerido' : '';
            }

            exist = false;
            if (sup3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_4_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 3 requerido' : '';
            }

            exist = false;
            if (sup4 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_5_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 4 requerido' : '';
            }

            exist = false;
            if (sup5 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_6_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 5 requerido' : '';
            }

            exist = false;
            if (sup6 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_7_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 6 requerido' : '';
            }

            // Inferior
            exist = false;
            if (inf1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_9_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 1 requerido' : '';
            }

            exist = false;
            if (inf2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_10_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 2 requerido' : '';
            }

            exist = false;
            if (inf3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_11_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 3 requerido' : '';
            }

            exist = false;
            if (inf4 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_12_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 4 requerido' : '';
            }

            exist = false;
            if (inf5 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_13_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 5 requerido' : '';
            }

            exist = false;
            if (inf6 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_14_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 5 requerido' : '';
            }

            // Ambientes
            exist = false;
            if (amb1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_16_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 1 requerido' : '';
            }

            exist = false;
            if (amb2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_17_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 2  requerido' : '';
            }

            exist = false;
            if (amb3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_18_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 3 requerido' : '';
            }

            // Tem[ tap]
            exist = false;
            if (tempTap == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_20_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura requerido' : '';
            }

            // Temp Inst
            exist = false;
            if (tempInst1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_26_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 1 requerido' : '';
            }

            exist = false;
            if (tempInst2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_27_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 2  requerido' : '';
            }

            exist = false;
            if (tempInst3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_28_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 3 requerido' : '';
            }

            break;
        case 7:
            sup1 = $('$inputcanal_2').val();
            sup2 = $('$inputcanal_3').val();
            sup3 = $('$inputcanal_4').val();
            sup4 = $('$inputcanal_5').val();
            sup5 = $('$inputcanal_6').val();
            sup6 = $('$inputcanal_7').val();
            sup7 = $('$inputcanal_8').val();
            inf1 = $('$inputcanal_10').val();
            inf2 = $('$inputcanal_11').val();
            inf3 = $('$inputcanal_12').val();
            inf4 = $('$inputcanal_13').val();
            inf5 = $('$inputcanal_14').val();
            inf6 = $('$inputcanal_15').val();
            inf7 = $('$inputcanal_16').val();
            amb1 = $('$inputcanal_18').val();
            amb2 = $('$inputcanal_19').val();
            amb3 = $('$inputcanal_20').val();
            tempTap = $('$inputcanal_22').val();
            tempInst1 = $('$inputcanal_28').val();
            tempInst2 = $('$inputcanal_29').val();
            tempInst3 = $('$inputcanal_30').val();

            // Superior
            exist = false;
            if (sup1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_2_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 1 requerido' : '';
            }

            exist = false;
            if (sup2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_3_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 2 requerido' : '';
            }

            exist = false;
            if (sup3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_4_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 3 requerido' : '';
            }

            exist = false;
            if (sup4 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_5_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 4 requerido' : '';
            }

            exist = false;
            if (sup5 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_6_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 5 requerido' : '';
            }

            exist = false;
            if (sup6 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_7_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 6 requerido' : '';
            }

            exist = false;
            if (sup7 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_8_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 7 requerido' : '';
            }

            // Inferior
            exist = false;
            if (inf1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_10_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 1 requerido' : '';
            }

            exist = false;
            if (inf2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_11_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 2 requerido' : '';
            }

            exist = false;
            if (inf3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_12_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 3 requerido' : '';
            }

            exist = false;
            if (inf4 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_13_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 4 requerido' : '';
            }

            exist = false;
            if (inf5 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_14_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 5 requerido' : '';
            }

            exist = false;
            if (inf6 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_15_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 6 requerido' : '';
            }

            exist = false;
            if (inf7 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_16_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 7 requerido' : '';
            }

            // Ambientes
            exist = false;
            if (amb1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_18_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 1 requerido' : '';
            }

            exist = false;
            if (amb2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_19_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 2  requerido' : '';
            }

            exist = false;
            if (amb3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_20_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 3 requerido' : '';
            }

            // Tem[ tap]
            exist = false;
            if (tempTap == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_22_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura requerido' : '';
            }

            // Temp Inst
            exist = false;
            if (tempInst1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_28_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 1 requerido' : '';
            }

            exist = false;
            if (tempInst2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_29_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 2  requerido' : '';
            }

            exist = false;
            if (tempInst3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_30_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 3 requerido' : '';
            }

            break;
        case 8:
            sup1 = $('$inputcanal_2').val();
            sup2 = $('$inputcanal_3').val();
            sup3 = $('$inputcanal_4').val();
            sup4 = $('$inputcanal_5').val();
            sup5 = $('$inputcanal_6').val();
            sup6 = $('$inputcanal_7').val();
            sup7 = $('$inputcanal_8').val();
            sup8 = $('$inputcanal_9').val();
            inf1 = $('$inputcanal_11').val();
            inf2 = $('$inputcanal_12').val();
            inf3 = $('$inputcanal_13').val();
            inf4 = $('$inputcanal_14').val();
            inf5 = $('$inputcanal_15').val();
            inf6 = $('$inputcanal_16').val();
            inf7 = $('$inputcanal_17').val();
            inf8 = $('$inputcanal_18').val();
            amb1 = $('$inputcanal_20').val();
            amb2 = $('$inputcanal_21').val();
            amb3 = $('$inputcanal_22').val();
            tempTap = $('$inputcanal_24').val();
            tempInst1 = $('$inputcanal_30').val();
            tempInst2 = $('$inputcanal_31').val();
            tempInst3 = $('$inputcanal_32').val();

            // Superior
            exist = false;
            if (sup1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_2_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 1 requerido' : '';
            }

            exist = false;
            if (sup2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_3_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 2 requerido' : '';
            }

            exist = false;
            if (sup3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_4_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 3 requerido' : '';
            }

            exist = false;
            if (sup4 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_5_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 4 requerido' : '';
            }

            exist = false;
            if (sup5 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_6_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 5 requerido' : '';
            }

            exist = false;
            if (sup6 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_7_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 6 requerido' : '';
            }

            exist = false;
            if (sup7 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_8_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 7 requerido' : '';
            }

            exist = false;
            if (sup8 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_9_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 8 requerido' : '';
            }

            // Inferior
            exist = false;
            if (inf1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_11_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 1 requerido' : '';
            }

            exist = false;
            if (inf2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_12_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 2 requerido' : '';
            }

            exist = false;
            if (inf3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_13_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 3 requerido' : '';
            }

            exist = false;
            if (inf4 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_14_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 4 requerido' : '';
            }

            exist = false;
            if (inf5 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_15_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 5 requerido' : '';
            }

            exist = false;
            if (inf6 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_16_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 6 requerido' : '';
            }

            exist = false;
            if (inf7 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_17_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 7 requerido' : '';
            }

            exist = false;
            if (inf8 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_18_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 8 requerido' : '';
            }

            // Ambientes
            exist = false;
            if (amb1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_20_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 1 requerido' : '';
            }

            exist = false;
            if (amb2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_21_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 2  requerido' : '';
            }

            exist = false;
            if (amb3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_22_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 3 requerido' : '';
            }

            // Tem[ tap]
            exist = false;
            if (tempTap == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_24_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura requerido' : '';
            }

            // Temp Inst
            exist = false;
            if (tempInst1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_30_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 1 requerido' : '';
            }

            exist = false;
            if (tempInst2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_31_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 2  requerido' : '';
            }

            exist = false;
            if (tempInst3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_32_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 3 requerido' : '';
            }

            break;
        case 9:
            sup1 = $('$inputcanal_2').val();
            sup2 = $('$inputcanal_3').val();
            sup3 = $('$inputcanal_4').val();
            sup4 = $('$inputcanal_5').val();
            sup5 = $('$inputcanal_6').val();
            sup6 = $('$inputcanal_7').val();
            sup7 = $('$inputcanal_8').val();
            sup8 = $('$inputcanal_9').val();
            sup9 = $('$inputcanal_10').val();
            inf1 = $('$inputcanal_12').val();
            inf2 = $('$inputcanal_13').val();
            inf3 = $('$inputcanal_14').val();
            inf4 = $('$inputcanal_15').val();
            inf5 = $('$inputcanal_16').val();
            inf6 = $('$inputcanal_17').val();
            inf7 = $('$inputcanal_18').val();
            inf8 = $('$inputcanal_19').val();
            inf9 = $('$inputcanal_20').val();
            amb1 = $('$inputcanal_22').val();
            amb2 = $('$inputcanal_23').val();
            amb3 = $('$inputcanal_24').val();
            tempTap = $('$inputcanal_26').val();
            tempInst1 = $('$inputcanal_32').val();
            tempInst2 = $('$inputcanal_33').val();
            tempInst3 = $('$inputcanal_34').val();

            // Superior
            exist = false;
            if (sup1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_2_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 1 requerido' : '';
            }

            exist = false;
            if (sup2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_3_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 2 requerido' : '';
            }

            exist = false;
            if (sup3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_4_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 3 requerido' : '';
            }

            exist = false;
            if (sup4 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_5_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 4 requerido' : '';
            }

            exist = false;
            if (sup5 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_6_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 5 requerido' : '';
            }

            exist = false;
            if (sup6 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_7_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 6 requerido' : '';
            }

            exist = false;
            if (sup7 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_8_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 7 requerido' : '';
            }

            exist = false;
            if (sup8 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_9_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 8 requerido' : '';
            }

            exist = false;
            if (sup9 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_10_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 9 requerido' : '';
            }

            // Inferior
            exist = false;
            if (inf1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_12_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 1 requerido' : '';
            }

            exist = false;
            if (inf2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_13_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 2 requerido' : '';
            }

            exist = false;
            if (inf3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_14_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 3 requerido' : '';
            }

            exist = false;
            if (inf4 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_15_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 4 requerido' : '';
            }

            exist = false;
            if (inf5 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_16_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 5 requerido' : '';
            }

            exist = false;
            if (inf6 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_17_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 6 requerido' : '';
            }

            exist = false;
            if (inf7 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_18_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 7 requerido' : '';
            }

            exist = false;
            if (inf8 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_19_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 8 requerido' : '';
            }

            exist = false;
            if (inf9 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_20_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 9 requerido' : '';
            }

            // Ambientes
            exist = false;
            if (amb1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_22_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 1 requerido' : '';
            }

            exist = false;
            if (amb2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_23_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 2  requerido' : '';
            }

            exist = false;
            if (amb3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_24_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 3 requerido' : '';
            }

            // Tem[ tap]
            exist = false;
            if (tempTap == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_26_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura requerido' : '';
            }

            // Temp Inst
            exist = false;
            if (tempInst1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_32_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 1 requerido' : '';
            }

            exist = false;
            if (tempInst2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_33_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 2  requerido' : '';
            }

            exist = false;
            if (tempInst3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_34_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 3 requerido' : '';
            }

            break;
        case 10:
            sup1 = $('$inputcanal_2').val();
            sup2 = $('$inputcanal_3').val();
            sup3 = $('$inputcanal_4').val();
            sup4 = $('$inputcanal_5').val();
            sup5 = $('$inputcanal_6').val();
            sup6 = $('$inputcanal_7').val();
            sup7 = $('$inputcanal_8').val();
            sup8 = $('$inputcanal_9').val();
            sup9 = $('$inputcanal_10').val();
            sup10 = $('$inputcanal_11').val();
            inf1 = $('$inputcanal_13').val();
            inf2 = $('$inputcanal_14').val();
            inf3 = $('$inputcanal_15').val();
            inf4 = $('$inputcanal_16').val();
            inf5 = $('$inputcanal_17').val();
            inf6 = $('$inputcanal_18').val();
            inf7 = $('$inputcanal_19').val();
            inf8 = $('$inputcanal_20').val();
            inf9 = $('$inputcanal_21').val();
            inf10 = $('$inputcanal_22').val();
            amb1 = $('$inputcanal_24').val();
            amb2 = $('$inputcanal_25').val();
            amb3 = $('$inputcanal_26').val();
            tempTap = $('$inputcanal_28').val();
            tempInst1 = $('$inputcanal_34').val();
            tempInst2 = $('$inputcanal_35').val();
            tempInst3 = $('$inputcanal_36').val();

            // SUperior
            exist = false;
            if (sup1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_2_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 1 requerido' : '';
            }

            exist = false;
            if (sup2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_3_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 2 requerido' : '';
            }

            exist = false;
            if (sup3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_4_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 3 requerido' : '';
            }

            exist = false;
            if (sup4 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_5_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 4 requerido' : '';
            }

            exist = false;
            if (sup5 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_6_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 5 requerido' : '';
            }

            exist = false;
            if (sup6 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_7_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 6 requerido' : '';
            }

            exist = false;
            if (sup7 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_8_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 7 requerido' : '';
            }

            exist = false;
            if (sup8 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_9_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 8 requerido' : '';
            }

            exist = false;
            if (sup9 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_10_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 9 requerido' : '';
            }

            exist = false;
            if (sup10 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_11_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal superior 10 requerido' : '';
            }

            // Inferior
            exist = false;
            if (inf1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_13_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 1 requerido' : '';
            }

            exist = false;
            if (inf2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_14_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 2 requerido' : '';
            }

            exist = false;
            if (inf3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_15_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 3 requerido' : '';
            }

            exist = false;
            if (inf4 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_16_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 4 requerido' : '';
            }

            exist = false;
            if (inf5 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_17_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 5 requerido' : '';
            }

            exist = false;
            if (inf6 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_18_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 6 requerido' : '';
            }

            exist = false;
            if (inf7 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_19_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 7 requerido' : '';
            }

            exist = false;
            if (inf8 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_20_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 8 requerido' : '';
            }

            exist = false;
            if (inf9 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_21_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 9 requerido' : '';
            }

            exist = false;
            if (inf10 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_22_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal inferior 10 requerido' : '';
            }

            // Ambientes
            exist = false;
            if (amb1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_24_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 1 requerido' : '';
            }

            exist = false;
            if (amb2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_25_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 2  requerido' : '';
            }

            exist = false;
            if (amb3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_26_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Ambiente 3 requerido' : '';
            }

            // Tem[ tap]
            exist = false;
            if (tempTap == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_28_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura requerido' : '';
            }

            // Temp Inst
            exist = false;
            if (tempInst1 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_34_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 1 requerido' : '';
            }

            exist = false;
            if (tempInst2 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_35_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 2  requerido' : '';
            }

            exist = false;
            if (tempInst3 == undefined) {
                for (var i = 0; i < 52; i++) {
                    if ($('inputch_36_' + i).val() != undefined) {
                        exist = true;
                        break;
                    }
                }
                result += exist ? 'Canal de Temperatura 3 requerido' : '';
            }

            break;
        default:
            break;
    }

    return result;

}

async function GetDataJSON() {
    var path = "/Retd/GetStabilizationData/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {
        return await response.json();
    }

    else {
        ShowFailedMessage('Error, por favor contacte al administrador del sistema.');
        return null;
    }
}

async function CloseDataJSON(posi) {
    var path = "/Retd/Close/";
    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value, id: viewModel.StabilizationDatas[posi].IdReg }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {
        return await response.json();
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