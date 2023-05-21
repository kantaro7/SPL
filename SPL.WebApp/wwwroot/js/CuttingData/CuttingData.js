//Variables and initializations of components
var editor;
var banderaErrorGrids = false;
let resultValidations = true;
let viewModel;
var idCutting;
let btnRequest = document.getElementById("btnRequest");
let btnSave = document.getElementById("btnSave");
let btnValidateDataTables = document.getElementById("btnValidate");
let btnGenerarR = document.getElementById("btnGenerarR");
let btnNew = document.getElementById("btnNew");
let btnClear = document.getElementById("btnClear");
let noSerieInput = document.getElementById("NoSerie");
let tableAt = $("#tableAt");
let tableBt = $("#tableBt");
let tableTer = $("#tableTer");
let divAT = document.getElementById("DivAT");
let divBT = document.getElementById("DivBT");
let divTER = document.getElementById("DivTer");
let positionATInput = document.getElementById("positionAT");
let nominalATInput = document.getElementById("nominalAT");
let positionBTInput = document.getElementById("positionBT");
let positionTERInput = document.getElementById("positionTER");
let nominalTERInput = document.getElementById("nominalTER");

//Events

$("#NoSerie").focus();



$("#NoSerie").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $("#btnRequest").click();
    }
});

btnNew.addEventListener("click", function () {
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
    if (noSerieInput.value) {
        postData(domain + "/CuttingData/ValidDataCuttingNew/", viewModel).then(data => {
            if (data.response.Code !== -1 && data.response.Code !== -2) {
                viewModel = data.response.Structure;
                LoadForm(1);
            }
            else {
                ShowFailedMessage(data.response.Description);
                $("#loader").css("display", "none");
            }
        });
    }
    else {
        $("#loader").css("display", "none");

        ShowSuccessMessage('Por favor ingrese un No. Serie.');
        btnSave.disabled = true;
        btnLoadDataSidco.disabled = true;
    }
});

btnGenerarR.addEventListener("click", function () {
    var path = path = "/Etd/Index/";
    var url = new URL(domain + path),
        params = {
            idCutting: idCutting,
            noSerie: noSerieInput.value
        }

    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    $("#loader").css("display", "none");
    var win = window.open(url.toString(), '_blank');
    win.focus();
});

$("#selectDate").blur(function () {
    let id = $("#selectDate").val();
    viewModel.StabilizationData = viewModel.StabilizationDatas.find(x => x.IdReg == id);
    viewModel.FechaDatos = viewModel.StabilizationData.FechaDatos;
    $("#Date").val(viewModel.StabilizationData.FechaDatos);



    $("#loaderIndex").css("display", "block");
    if (noSerieInput.value) {
        postData(domain + "/CuttingData/ValidDataCuttingFecha/", viewModel).then(data => {
            if (data.response.Code !== -1 && data.response.Code !== 2) {
                viewModel = data.response.Structure;
                let id = $("#selectDate").val();
                let stabilization = viewModel.StabilizationDatas.find(x => x.IdReg == id);
                $("#Date").val(stabilization.FechaDatos);

                LoadFormFecha(1);
            } else {
                ShowFailedMessage(data.response.Description);
                $("#loaderIndex").css("display", "none");
            }
        });

    }
    else {
        $("#loaderIndex").css("display", "none");

        ShowSuccessMessage('Por favor ingrese un No. Serie.');
        btnSave.disabled = true;
        btnLoadDataSidco.disabled = true;
    }
});

$('#selectUltimaHora').on('change', function () {
    $("#loaderIndex").css("display", "block");
    if (noSerieInput.value) {
        postData(domain + "/CuttingData/ValidDataCuttingUltimaHora/", viewModel).then(data => {
            if (data.response.Code !== -1 && data.response.Code !== 2) {
                viewModel = data.response.Structure;
                LoadFormUltimaHora(1);
            } else {
                ShowFailedMessage(data.response.Description);
                $("#loaderIndex").css("display", "none");
            }
        });
    }
    else {
        $("#loaderIndex").css("display", "none");
        ShowSuccessMessage('Por favor ingrese un No. Serie.');
        btnSave.disabled = true;
        btnLoadDataSidco.disabled = true;
    };
});

$('#selectPrimerCorte').on('change', function () {
    $("#loaderIndex").css("display", "block");
    if (noSerieInput.value) {
        let dataUltimaHora = viewModel.StabilizationData.StabilizationDataDetails.find(x => x.FechaHora == viewModel.FirstCut);
        if (dataUltimaHora == undefined) {
            ShowFailedMessage("No se encuentra fecha ultima hora de terecer corte.");
            $("#loaderIndex").css("display", "none");
        } else {
            viewModel.SectionCuttingData1.TempAceiteProm = dataUltimaHora.AmbienteProm + dataUltimaHora.AorCorr;
            LoadFormPrimerCorte(1);
        }
        //postData(domain + "/CuttingData/ValidDataCuttingPrimerCorte/", viewModel).then(data => {
        //    if (data.response.Code !== -1 && data.response.Code !== 2) {
        //        LoadFormPrimerCorte(1, data.response.Structure);
        //    }else {
        //        ShowFailedMessage(data.response.Description);
        //        $("#loaderIndex").css("display", "none");
        //    }
        //});
    }
    else {
        $("#loaderIndex").css("display", "none");
        ShowSuccessMessage('Por favor ingrese un No. Serie.');
        btnSave.disabled = true;
        btnLoadDataSidco.disabled = true;
    }
});

$('#selectSegundoCorte').on('change', function () {
    $("#loaderIndex").css("display", "block");
    if (noSerieInput.value) {
        let dataUltimaHora = viewModel.StabilizationData.StabilizationDataDetails.find(x => x.FechaHora == viewModel.SecondCut);
        if (dataUltimaHora == undefined) {
            ShowFailedMessage("No se encuentra fecha ultima hora de segundo corte.");
            $("#loaderIndex").css("display", "none");
        } else {
            viewModel.SectionCuttingData2.TempAceiteProm = dataUltimaHora.AmbienteProm + dataUltimaHora.AorCorr;
            LoadFormSegundoCorte(1);
        }

        //postData(domain + "/CuttingData/ValidDataCuttingSegundoCorte/", viewModel).then(data => {
        //    if (data.response.Code !== -1 && data.response.Code !== 2) {
        //        LoadFormSegundoCorte(1, data.response.Structure);
        //    }else {
        //        ShowFailedMessage(data.response.Description);
        //        $("#loaderIndex").css("display", "none");
        //    }
        //});
    }
    else {
        $("#loaderIndex").css("display", "none");
        ShowSuccessMessage('Por favor ingrese un No. Serie.');
        btnSave.disabled = true;
        btnLoadDataSidco.disabled = true;
    }
});

$('#selectTercerCorte').on('change', function () {
    $("#loaderIndex").css("display", "block");
    if (noSerieInput.value) {
        let dataUltimaHora = viewModel.StabilizationData.StabilizationDataDetails.find(x => x.FechaHora == viewModel.ThirdCut);
        if (dataUltimaHora == undefined) {
            ShowFailedMessage("No se encuentra fecha ultima hora de segundo corte.");
            $("#loaderIndex").css("display", "none");
        } else {
            viewModel.SectionCuttingData3.TempAceiteProm = dataUltimaHora.AmbienteProm + dataUltimaHora.AorCorr;
            LoadFormTercerCorte(1);
        }
        //postData(domain + "/CuttingData/ValidDataCuttingTercerCorte/", viewModel).then(data => {
        //    if (data.response.Code !== -1 && data.response.Code !== 2) {
        //        LoadFormTercerCorte(1, data.response.Structure);
        //    }else {
        //        ShowFailedMessage(data.response.Description);
        //        $("#loaderIndex").css("display", "none");
        //    }
        //});
    }
    else {
        $("#loaderIndex").css("display", "none");
        ShowSuccessMessage('Por favor ingrese un No. Serie.');
        btnSave.disabled = true;
        btnLoadDataSidco.disabled = true;
    }
});

btnSave.addEventListener("click", function () {

    if (banderaErrorGrids)
        return;

    $("#loader").css("display", "block");
    ResultValidations = $("#form_detalle").data("kendoValidator").validate();
    MapToViewModel();

    if (resultValidations) {
        postData(domain + "/CuttingData/Post/", viewModel)
            .then(data => {
                if (data.response.Code !== -1) {

                    ShowSuccessMessage("Guardado Exitoso")
                    idCutting = data.response.Id;
                }
                else {
                    ShowFailedMessage(data.response.Description);
                }
            });
    } else {
        ShowFailedMessage("Hay tension(es) vacías, por favor verifique.")
    }

    $("#loader").css("display", "none");
});

btnValidateDataTables.addEventListener("click", function () {

    if (banderaErrorGrids)
        return;

    $("#loaderValidate").css("display", "block");
    ResultValidations = true;

 
    tableAt.data("kendoGrid").dataSource._pristineData.forEach(function (data, key) {
        if (data.resistencia !== '' && data.resistencia !== undefined && data.resistencia !== null) {
            viewModel.TableAT[key].RESISTENCIA = data.resistencia;
        }
        else {
            resultValidations = false;
        }

        //if (viewModel.TableAT[key].Existente === null)
        //    viewModel.TableAT[key].Existente = false;
    });
    //map table BT
    tableBt.data("kendoGrid").dataSource._pristineData.forEach(function (data, key) {

        if (data.resistencia !== '' && data.resistencia !== undefined && data.resistencia !== null) {
            viewModel.TableBT[key].RESISTENCIA = data.resistencia;
        }
        else {
            resultValidations = false;
        }

        //if (viewModel.TableBT[key].Existente === null)
        //    viewModel.TableBT[key].Existente = false;
    });
    //map table TER
    tableTer.data("kendoGrid").dataSource._pristineData.forEach(function (data, key) {

        if (data.resistencia !== '' && data.resistencia !== undefined && data.resistencia !== null) {
            viewModel.TableTER[key].RESISTENCIA = data.resistencia;
        }
        else {
            resultValidations = false;
        }

        //if (viewModel.TableTER[key].Existente === null)
        //    viewModel.TableTER[key].Existente = false;
    });


    viewModel.Constante = $("#Constante").val();


/*    if (resultValidations) {*/
        postData(domain + "/CuttingData/ValidateDataTables/", viewModel)
            .then(data => {
                if (data.response.Code !== -1) {
                    $("#loaderValidate").css("display", "none");
                    viewModel = data.response.Structure; 
                    LoadTables();
                    ShowSuccessMessage("Temperaturas actualizadas")
               

                }
                else {
                    $("#loaderValidate").css("display", "none");
                    ShowFailedMessage(data.response.Description);
                }
            });
    //} else {
    //    ShowFailedMessage("Hay tension(es) vacías, por favor verifique.")
    //}


});

//btnClear.addEventListener("click", function () {
//    ClearForm();
//});

//btnNewTensionYes.addEventListener("click", function () {

//    $("#loader").css("display", "block");
//    GetCuttingDataJSON(true).then(
//        data => {
//            if (data.response.Code !== -1) {

//                btnSave.disabled = false;
//                noSerieInput.disabled = true;

//                viewModel = data.response.Structure;
//                LoadForm();
//            }
//            else {
//                ShowFailedMessage(data.response.Description);
//            }
//            $("#loader").css("display", "none");
//        }
//    );

//});

//btnNewTensionNo.addEventListener("click", function () {

//    $("#loader").css("display", "block");
//    GetCuttingDataJSON(false).then(
//        data => {
//            if (data.response.Code !== -1) {

//                btnSave.disabled = false;
//                noSerieInput.disabled = true;

//                viewModel = data.response.Structure;
//                LoadForm();

//            }
//            else {
//                ShowFailedMessage(data.response.Description);
//            }
//            $("#loader").css("display", "none");
//        }
//    );

//});

btnRequest.addEventListener("click", function () {

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
    if (noSerieInput.value) {

        GetCuttingDataJSON(null).then(
            data => {
                if (data.response.Code !== -1 && data.response.Code !== 2) {
                    if (data.response.Structure.HeaderCuttingDatas.length == 0) {
                        $("#mesaggecuttingData").text("No se encontraron resultados");
                    }
                    btnSave.disabled = false;
                    noSerieInput.disabled = true;
                    viewModel = data.response.Structure;
                    var dataMap = data.response.Structure.HeaderCuttingDatas;
                    let dataS = dataMap.map((item) => {
                        return {
                            IdCorte: item.IdCorte,
                            TipoEnf: item.StabilizationData.CoolingType,
                            Temp: item.StabilizationData.OverElevation,
                            UltimaHora: item.UltimaHora,
                            PrimerCorte: item.PrimerCorte,
                            SegundoCorte: item.SegundoCorte,
                            TercerCorte: item.TercerCorte,
                        }
                    })
                    $("#gridCuttingDatas").kendoGrid({
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
                            { field: "IdCorte", hidden: true },
                            { field: "TipoEnf", title: "Tipo Enfriamiento" },
                            { field: "Temp", title: "S.E. (°C)" },
                            { field: "UltimaHora", title: "Ultima Hora" },
                            { field: "PrimerCorte", title: "Primer Corte" },
                            { field: "SegundoCorte", title: "Segundo Corte" },
                            { field: "TercerCorte", title: "Tercer Corte" },
                            {
                                command: [
                                    {
                                        name: "edit",
                                        text: "editar",
                                        click: function (e) {
                                            var dataItem = grid.dataItem(this);
                                            itemToDelete = grid.dataItem(this);
                                            console.log(itemToDelete);
                                            console.log(dataItem);
                                            GetCuttingDataEspecificoJSON(dataItem.IdCorte, noSerieInput.value).then(
                                                data => {
                                                    if (data.response.Code !== -1 && data.response.Code !== 2) {
                                                        btnSave.disabled = false;
                                                        noSerieInput.disabled = true;
                                                        viewModel = data.response.Structure;
                                                        $("#btnNew").prop("disabled", true);
                                                        $("#btnSave").prop("disabled", false);
                                                        $("#btnDelete").prop("disabled", false);
                                                        $("#btnClear").prop("disabled", false);
                                                    }
                                                    else if (data.response.Code === 2) {
                                                        AskUpdateInfo();
                                                    }
                                                    else {
                                                        ShowFailedMessage(data.response.Description);
                                                    }
                                                    $("#loader").css("display", "none");
                                                    $("#mesaggecuttingData").text("")
                                                }
                                            );
                                            isSave = false;
                                        },
                                        className: "btn btn-primary btn-sm",
                                    }
                                ]
                            }
                        ],
                        
                        selectable: "row",
                        dataBound: function (e) {
                            var grid = this;
                            grid.tbody.find("tr").dblclick(function (e) {
                                var dataItem = grid.dataItem(this);
                                itemToDelete = grid.dataItem(this);
                                console.log(itemToDelete);
                                console.log(dataItem);
                                GetCuttingDataEspecificoJSON(dataItem.IdCorte, noSerieInput.value).then(
                                    data => {
                                        if (data.response.Code !== -1 && data.response.Code !== 2) {
                                            btnSave.disabled = false;
                                            noSerieInput.disabled = true;
                                            viewModel = data.response.Structure;
                                            $("#IdMarca").prop("disabled", true);
                                            $("#Descripcion").prop("disabled", false);
                                            $("#Estatus").prop("disabled", false);
                                            $("#btnNew").prop("disabled", true);
                                            $("#btnSave").prop("disabled", false);
                                            $("#btnDelete").prop("disabled", false);
                                            $("#btnClear").prop("disabled", false);
                                        }
                                        else if (data.response.Code === 2) {
                                            AskUpdateInfo();
                                        }
                                        else {
                                            ShowFailedMessage(data.response.Description);
                                        }
                                        $("#loader").css("display", "none");
                                        $("#mesaggecuttingData").text("")
                                    }
                                );
                                isSave = false;
                            });
                        }
                    });

                    $("#loader").css("display", "none");
                    $("#mesaggecuttingData").text("");
                }
                else if (data.response.Code === 2) {
                    $("#mesaggecuttingData").text("No se encontraron datos")
                    AskUpdateInfo();
                    $("#loader").css("display", "none");
                }
                else {
                    ShowFailedMessage(data.response.Description);
                    $("#mesaggecuttingData").text("No se encontraron datos")
                    $("#loader").css("display", "none");
                }
            }
        );
    }
    else {
        $("#loader").css("display", "none");
        ShowSuccessMessage('Por favor ingrese un No. Serie.');
        btnSave.disabled = true;
        btnLoadDataSidco.disabled = true;
    }
});

//Requests

async function GetCuttingDataJSON(isNewTensionResponse) {
    var path = path = "/CuttingData/Get/";
    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value}
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const CuttingData = await response.json();
        return CuttingData;
    }

    else {
        ShowFailedMessage('Error, por favor contacte al administrador del sistema.');
        return null;
    }
}


async function ValidDataCuttingNew(isNewTensionResponse) {
    postData(domain + "/CuttingData/Post/", viewModel)
        .then(data => {
            if (data.response.Code !== -1) {
                ShowSuccessMessage("Guardado Exitoso")
            }
            else {
                ShowFailedMessage(data.response.Description);
            }
        });
    var path = path = "/CuttingData/ValidDataCuttingNew/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const CuttingData = await response.json();
        return CuttingData;
    }

    else {
        ShowFailedMessage('Error, por favor contacte al administrador del sistema.');
        return null;
    }
}

async function ValidDataCuttingFecha(isNewTensionResponse) {
    var path = path = "/CuttingData/ValidDataCuttingFecha/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value, date: $("#Date").val() , idStabilizacion:-1 }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const CuttingData = await response.json();
        return CuttingData;
    }

    else {
        ShowFailedMessage('Error, por favor contacte al administrador del sistema.');
        return null;
    }
}

async function ValidDataUltimaHora(isNewTensionResponse) {
    var path = path = "/CuttingData/ValidDataCuttingUltimaHora/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value, date: $("#Date").val(), ultimahora: $("#selectUltimaHora option:selected").text() }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const CuttingData = await response.json();
        return CuttingData;
    }

    else {
        ShowFailedMessage('Error, por favor contacte al administrador del sistema.');
        return null;
    }
}

async function ValidDataPrimerCorte(isNewTensionResponse) {
    var path = path = "/CuttingData/ValidDataCuttingPrimerCorte/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value, date: $("#Date").val(), ultimahora: $("#selectPrimerCorte option:selected").text() }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const CuttingData = await response.json();
        return CuttingData;
    }

    else {
        ShowFailedMessage('Error, por favor contacte al administrador del sistema.');
        return null;
    }
}

async function ValidDataSegundoCorte(isNewTensionResponse) {
    var path = path = "/CuttingData/ValidDataCuttingSegundoCorte/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value, date: $("#Date").val(), ultimahora: $("#selectSegundoCorte option:selected").text() }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const CuttingData = await response.json();
        return CuttingData;
    }

    else {
        ShowFailedMessage('Error, por favor contacte al administrador del sistema.');
        return null;
    }
}

async function ValidDataTercerCorte(isNewTensionResponse) {
    var path = path = "/CuttingData/ValidDataCuttingTercerCorte/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value, date: $("#Date").val(), ultimahora: $("#selectTercerCorte option:selected").text() }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const CuttingData = await response.json();
        return CuttingData;
    }

    else {
        ShowFailedMessage('Error, por favor contacte al administrador del sistema.');
        return null;
    }
}

async function GetCuttingDataEspecificoJSON(IdCutingData, NoSerie) {
    var path = "/CuttingData/GetCuttingDataEspecifico/";

    var url = new URL(domain + path),
        params = { IdCuttiingData: IdCutingData, NoSerie: NoSerie }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const CuttingData = await response.json();
        return CuttingData;
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

function AskUpdateInfo() {
    newTensionModal.show();
}

//Functions

function MapToViewModel() {
    resultValidations = true;
    viewModel.NoSerie = noSerieInput.value;
}

function ClearForm() {
    noSerieInput.disabled = false;
    noSerieInput.value = '';
    btnSave.disabled = true;
    $("#mesaggecuttingData").text("Ingresa un número de serie y presiona consultar")
}

function LoadForm(status) {

    $("#selectPosAt").empty();
    $("#selectPosBt").empty();
    $("#selectPosTer").empty();

    if (status == 1) {
        if (viewModel.PositionsAT == undefined || viewModel.PositionsAT.length <= 0) {
            $("#selectPosAt").prop("disabled", true)
        } else {
            $("#selectPosAt").prop("disabled", false)
            $.each(viewModel.PositionsAT, function (i, val) {
                $("#selectPosAt").append("<option value='" + val + "'>" + val + "</option>");
            });
        }

        if (viewModel.PositionsBT == undefined || viewModel.PositionsBT.length <= 0) {
            $("#selectPosBt").prop("disabled", true)
        } else {
            $("#selectPosBt").prop("disabled", false)
            $.each(viewModel.PositionsBT, function (i, val) {
                $("#selectPosBt").append("<option value='" + val + "'>" + val + "</option>");
            });
        }
        
        if (viewModel.PositionsTER == undefined || viewModel.PositionsTER.length <= 0) {
            $("#selectPosTer").prop("disabled", true)
        } else {
            $("#selectPosTer").prop("disabled", false)
            $.each(viewModel.PositionsBT, function (i, val) {
                $("#selectPosTer").append("<option value='" + val + "'>" + val + "</option>");
            });
        }
    
        $("#selectCoolingTypes").empty();
        $.each(viewModel.CoolingTypes, function (i, val) {
            $("#selectCoolingTypes").append("<option value='" + val.Value + "'>" + val.Description + "</option>");
        });

        $("#selectDate").empty();
        $("#selectDate").append("<option value=''>Seleccione...</option>");
        $.each(viewModel.Dates, function (i, val) {
            $("#selectDate").append("<option value='" + val.Value + "'>" + val.Description + "</option>");
        });
        ShowSuccessMessage("Consulta Exitosa.")

    }
    else {



    }
    $('#ModalDetailsCuttingData').modal('show');
}


function LoadFormFecha(status) {
    if (status == 1) {
        $("#selectCoolingTypes").val(viewModel.CoolingType);
        $("#OverElevation").val(viewModel.OverElevation);
        $("#selectPosAt").val(viewModel.PosAT);
        $("#selectPosBt").val(viewModel.PosBT);
        $("#selectPosTer").val(viewModel.PosTER);
        $("#Capacidad").val(viewModel.Capacidad);
        $("#Perdidas").val(viewModel.Perdidas);
        $("#Corriente").val(viewModel.Corriente);

        $.each(viewModel.LastHours, function (i, val) {
            $("#selectUltimaHora").append("<option value='" + val.Value + "'>" + val.Description + "</option>");
        });
        $.each(viewModel.FirstCuts, function (i, val) {
            $("#selectPrimerCorte").append("<option value='" + val.Value + "'>" + val.Description + "</option>");
        });

        $.each(viewModel.SecondCuts, function (i, val) {
            $("#selectSegundoCorte").append("<option value='" + val.Value + "'>" + val.Description + "</option>");
        });

        $.each(viewModel.ThirdCuts, function (i, val) {
            $("#selectTercerCorte").append("<option value='" + val.Value + "'>" + val.Description + "</option>");
        });

        LoadTables();

        $("#loaderIndex").css("display", "none");
        ShowSuccessMessage("Consulta Exitosa.")

    }
    else {



    }

}




function LoadFormUltimaHora(status) {

    if (status == 1) {
        //viewModel.CoolingType = info.CoolingType;
        //viewModel.OverElevation = info.OverElevation;
        //viewModel.PosAT = info.PosAT;
        //viewModel.PosBT = info.PosBT;
        //viewModel.PosTER = info.PosTER;
        //viewModel.Capacidad = info.Capacidad;
        //viewModel.Perdidas = info.Perdidas;
        //viewModel.Corriente = info.Corriente;

        //viewModel.Kw_Prueba = info.Kw_Prueba;
        //viewModel.Corriente = info.Corriente;
        //viewModel.Ambiente_prom = info.Ambiente_prom;
        //viewModel.TORxAltitud = info.TORxAltitud;
        //viewModel.AORxAltitud = info.AORxAltitud;



        $("#selectCoolingTypes").val(viewModel.CoolingType);
        $("#OverElevation").val(viewModel.OverElevation);
        $("#selectPosAt").val(viewModel.PosAT);
        $("#selectPosBt").val(viewModel.PosBT);
        $("#selectPosTer").val(viewModel.PosTER);
        $("#Capacidad").val(viewModel.Capacidad);
        $("#Perdidas").val(viewModel.Perdidas);
        $("#Corriente").val(viewModel.Corriente);




        $("#KwPrueba").val(viewModel.Kw_Prueba);
        $("#Corriente").val(viewModel.Corriente);
        $("#AmbProm").val(viewModel.Ambiente_prom);
        $("#TORxAltitud").val(viewModel.TORxAltitud);
        $("#AORxAltitud").val(viewModel.AORxAltitud);

        //viewModel.LastHours = info.LastHours;
        //viewModel.FirstCuts = info.FirstCuts;
        //viewModel.SecondCuts = info.SecondCuts;
        //viewModel.ThirdCuts = info.ThirdCuts;


        $.each(viewModel.LastHours, function (i, val) {
            $("#selectUltimaHora").append("<option value='" + val.Value + "'>" + val.Description + "</option>");
        });
        $.each(viewModel.FirstCuts, function (i, val) {
            $("#selectPrimerCorte").append("<option value='" + val.Value + "'>" + val.Description + "</option>");
        });

        $.each(viewModel.SecondCuts, function (i, val) {
            $("#selectSegundoCorte").append("<option value='" + val.Value + "'>" + val.Description + "</option>");
        });

        $.each(viewModel.ThirdCuts, function (i, val) {
            $("#selectTercerCorte").append("<option value='" + val.Value + "'>" + val.Description + "</option>");
        });

        $("#loaderIndex").css("display", "none");
        ShowSuccessMessage("Consulta Exitosa.")

    }
    else {



    }

}



function LoadFormPrimerCorte(status) {
    if (status == 1) {
        viewModel.SectionCuttingData1.TempAceiteProm = viewModel.SectionCuttingData1.TempAceiteProm;
        $('#TempAceitePromAT').val(viewModel.SectionCuttingData1.TempAceiteProm);
        $("#loaderIndex").css("display", "none");
        ShowSuccessMessage("Consulta Exitosa.")
    }
    else {

    }
}

function LoadFormSegundoCorte(status) {
    if (status == 1) {
        $('#TempAceitePromBT').val(viewModel.SectionCuttingData2.TempAceiteProm);
        viewModel.SectionCuttingData2.TempAceiteProm = viewModel.SectionCuttingData2.TempAceiteProm;
        $("#loaderIndex").css("display", "none");
        ShowSuccessMessage("Consulta Exitosa.")
    }
    else {

    }
}

function LoadFormTercerCorte(status) {
    if (status == 1) {
        $('#TempAceitePromTER').val(ViewModel.SectionCuttingData3.TempAceiteProm);
        viewModel.SectionCuttingData3.TempAceiteProm = ViewModel.SectionCuttingData3.TempAceiteProm; 
        $("#loaderIndex").css("display", "none");
        ShowSuccessMessage("Consulta Exitosa.")
    }
    else {

    }
    $('#ModalDetailsCuttingData').modal('show');
}

function CheckNominalValue(tableTer) {
    if (tableTer.length === 1) {
        if (!(parseFloat(tableTer[0].TENSION) > 0))
            return false;
        else
            return true;
    }
    else
        return true;
}

function validateInput(input) {

    banderaErrorGrids = false;
    var tension = $(input).val();

    if (tension == "" || tension == null || tension == undefined) {
        banderaErrorGrids = true;
    }

    var result = tension.split(".");
    if (!(/^[0-9]+$/.test(result[0]))) {
        banderaErrorGrids = true;
        input.attr("data-validateFunction-msg", "Solo se permiten números");
        return false;
    }
    if (result.length > 1) {
        if (!(/^[0-9]+$/.test(result[1]))) {
            banderaErrorGrids = true;
            input.attr("data-validateFunction-msg", "Solo se permiten números");
            return false;
        }
    }

    if (tension > 99999999.999) {
        banderaErrorGrids = true;
        input.attr("data-validateFunction-msg", "El número no debe ser mayor a 99,999,999.999");
        return false;
    }

    if (result.length == 2) {
        if (result[1].length > 3) {
            banderaErrorGrids = true;
            input.attr("data-validateFunction-msg", "El número debe contener 3 o menos decimales");
            return false;
        }
    }
    else
        if (result.length > 2) {
            banderaErrorGrids = true;
            input.attr("data-validateFunction-msg", "Formato incorrecto");
            return false;
        }
    return true;
}

function LoadTables() {
    let dataSourceAT = [];
    let dataSourceBT = [];
    let dataSourceTER = [];
    viewModel.TableAT.forEach(function (data, key) {
        dataSourceAT.push({ id: key, tiempo: data.TIEMPO, resistencia: data.RESISTENCIA, temperatura: data.TEMPERATURA });
    });

    tableAt.kendoGrid({
        dataSource: {
            data: dataSourceAT,
            schema: {
                model: {
                    id: "id",
                    fields: {
                        tiempo: { editable: false },
                        temperatura: {
                            editable: false,
                            format: "{0:n3}",
                            decimals: 3,
                        },
                        resistencia: {
                            format: "{0:n3}",
                            decimals: 3,
                            validation: {
                                required: true,
                                validateFunction: function (isInput) {
                                    return validateInput(isInput);
                                }
                            },
                            editable: true
                        }
                    }
                }
            },
            change: function (e) {
                if (e.action == "itemchange") {
                    this.sync();
                }
            }
        },
        scrollable: true,
        height: 300,
        columns: [{
            field: "tiempo",
            title: "Tiempo (min.seg)"
        },
        {
            field: "resistencia",
            title: "Resistencia",
        },
        {
            field: "temperatura",
            title: "Temp. de R (°C)",
        }],
        editable: true
    });

    var grid1 = tableAt.data("kendoGrid");

    grid1.table.on('keydown', function (e) {
        var currentNumberOfItems = grid1.dataSource.view().length;
        var row = $(e.target).closest('tr').index();
        var col = grid1.cellIndex($(e.target).closest('td'));
        var dataItem = grid1.dataItem($(e.target).closest('tr'));
        var field = grid1.columns[col].field;
        var value = $(e.target).val();

        if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {
            if (validateInput($(e.target)) == true) {
                e.preventDefault();
                dataItem.set(field, value);
                if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < grid1.columns.length) {
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

                    if (nextCellCol >= grid1.columns.length || nextCellCol < 0) {
                        return;
                    }

                    // wait for cell to close and Grid to rebind when changes have been made

                    setTimeout(function () {
                        grid1.editCell(grid1.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                    });
                }
            }
        }
    });

    

    viewModel.TableBT.forEach(function (data, key) {
        dataSourceBT.push({ id: key, tiempo: data.TIEMPO, resistencia: data.RESISTENCIA, temperatura: data.TEMPERATURA });
    });

    tableBt.kendoGrid({
        dataSource: {
            data: dataSourceBT,
            schema: {
                model: {
                    id: "id",
                    fields: {
                        tiempo: { editable: false },
                        temperatura: {
                            editable: false,
                            format: "{0:n3}",
                            decimals: 3,
                        },
                        resistencia: {
                            format: "{0:n3}",
                            decimals: 3,
                            validation: {
                                required: true,
                                validateFunction: function (isInput) {
                                    return validateInput(isInput);
                                }
                            },
                            editable: true
                        }
                    }
                }
            },
            change: function (e) {
                if (e.action == "itemchange") {
                    this.sync();
                }
            }
        },
        scrollable: true,
        height: 300,
        columns: [{
            field: "tiempo",
            title: "Tiempo (min.seg)", width: "110px" 
        },
        {
            field: "resistencia",
            title: "Resistencia",
             width: "110px" 
        },
        {
            field: "temperatura",
            title: "Temp. de R(°C)",
            width: "110px" 
        }],
        editable: true
    });

    var grid2 = tableBt.data("kendoGrid");
    grid2.table.on('keydown', function (e) {
        var currentNumberOfItems = grid2.dataSource.view().length;
        var row = $(e.target).closest('tr').index();
        var col = grid2.cellIndex($(e.target).closest('td'));
        var dataItem = grid2.dataItem($(e.target).closest('tr'));
        var field = grid2.columns[col].field;
        var value = $(e.target).val();

        if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {
            if (validateInput($(e.target)) == true) {
                e.preventDefault();
                dataItem.set(field, value);
                if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < grid2.columns.length) {
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
                    if (nextCellCol >= grid2.columns.length || nextCellCol < 0) {
                        return;
                    }

                    // wait for cell to close and Grid to rebind when changes have been made
                    setTimeout(function () {
                        grid2.editCell(grid2.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                    });
                }
            }
        }
    });


    viewModel.TableTER?.forEach(function (data, key) {
        dataSourceTER.push({ id: key, tiempo: data.TIEMPO, resistencia: data.RESISTENCIA, temperatura: data.TEMPERATURA });
    });

    tableTer.kendoGrid({
        dataSource: {
            data: dataSourceTER,
            schema: {
                model: {
                    id: "id",
                    fields: {
                        tiempo: { editable: false },
                        temperatura: {
                            editable: false,
                            format: "{0:n3}",
                            decimals: 3,
                        },
                        resistencia: {
                            format: "{0:n3}",
                            decimals: 3,
                            validation: {
                                required: true,
                                validateFunction: function (isInput) {
                                    return validateInput(isInput);
                                }
                            },
                            editable: true
                        }
                    }
                }
            },
            change: function (e) {
                if (e.action == "itemchange") {
                    this.sync();
                }
            }
        },
        scrollable: true,
        height: 300,
        columns: [{
            field: "tiempo",
            title: "Tiempo"
        },
        {
            field: "resistencia",
            title: "Resistencia",
        },
        {
            field: "temperatura",
            title: "Temperatura",
        }],
        editable: true
    });

    var grid3 = tableTer.data("kendoGrid");

    grid3.table.on('keydown', function (e) {
        var currentNumberOfItems = grid3.dataSource.view().length;
        var row = $(e.target).closest('tr').index();
        var col = grid3.cellIndex($(e.target).closest('td'));
        var dataItem = grid3.dataItem($(e.target).closest('tr'));
        var field = grid3.columns[col].field;
        var value = $(e.target).val();

        if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {
            if (validateInput($(e.target)) == true) {
                e.preventDefault();
                dataItem.set(field, value);
                if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < grid3.columns.length) {
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

                    if (nextCellCol >= grid3.columns.length || nextCellCol < 0) {
                        return;
                    }

                    setTimeout(function () {
                        grid3.editCell(grid3.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                    });
                }
            }
        }
    });
}


