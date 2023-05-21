
let btnNew = document.getElementById("btnNew");
let btnSave = document.getElementById("btnSave");
let btnDelete = document.getElementById("btnDelete");
let btnClear = document.getElementById("btnClear");
let btnDeleteYes = document.getElementById("btnDeleteYes");

$("#btnDeleteYes").click(function () {
    btnClear.disabled = true
    btnSave.disabled = true
    btnDelete.disabled = true 

    params = {
        MarcaId: $("#MarcaId").val(),
        TipoId: $("#TipoId").val(),
        TemperatureId: $("#TemperatureId").val(),
        FactorCorreccionId: $("#FactorCorreccionId").val()
    }

    $("#loader").css("display", "block");

    postData(domain + "/CorrectionFactor/DeleteRegisterNozzleTypeMark", params)
        .then(data => {
            if (data.response.Code !== -1) {

                $("#loader").css("display", "none");

                var dataSource = $("#grid").data("kendoGrid").dataSource;
                dataSource.remove(itemToDelete);
                dataSource.sync();
                itemToDelete = null;
                ShowSuccessMessage(data.response.Description)
                clearFields();
            }
            else {

                $("#loader").css("display", "none");
                ShowFailedMessage(data.response.Description);
                clearFields();
            }

        });
}); 

async function laodTypesByMark(IdTipo) {
    $("#TipoId option[value!=0]").remove();
    var marcaId = $("#MarcaId").val();
    await getData(domain + "/CorrectionFactor/GetTypesByMark/?IdMarca=" + marcaId ).then(data => {
        if (data.response.Code !== -1) {
            for (i in data.response.Structure) {
                $("#TipoId").append("<option value=" + data.response.Structure[i].IdTipo + ">" + data.response.Structure[i].Descripcion + "</option>");
            }

            if (IdTipo !== 0) {
                $("#TipoId").val(IdTipo);
            }
        }
    });
}


async function editOrSaveRegister() {
    $("#loader").css("display", "block");
    var validate_form = $("#form_factor_correccion_mark_type").data("kendoValidator");
    if (!validate_form.validate()) {
        $("#loader").css("display", "none");
        return
    }

    btnClear.disabled = true 
    btnSave.disabled = true 
    btnDelete.disabled = true 

    let temperatura = $("#TemperatureId").val();
    let marcaId = $("#MarcaId").val();
    let tipoId = $("#TipoId").val();
    let factor = $("#FactorCorreccionId").val();

    if (isSave) {
        await getData(domain + "/CorrectionFactor/ValidateNewNozzleTypeMarkr/?MarcaId=" + marcaId + "&TipoId=" + tipoId + "&Temperatura=" + temperatura + "&Factor=" + factor).then(data => {
            if (data.response.Code !== -1) {

                if (data.response.Structure !== 0) {

                    if (isSave) {
                        ShowWarningMessage("El factor de corrección para la marca, tipo y temperatura ya existe, favor de corregirlo");
                        btnClear.disabled = false
                        btnSave.disabled = false 
                        sendPost = false;
                        return;
                    }
                    sendPost = true;
                } else {
                    sendPost = true;
                }
            }
            else {
                sendPost = true;
            }
        });
    } else {//es editar
        sendPost = true;
    } 

    if (sendPost) {
        if (isSave) {
            op = 1
        } else {
            op = 2
        }

        params = {
            MarcaId: $("#MarcaId").val(),
            TipoId: $("#TipoId").val(),
            Tipo: $("#TipoId option:selected").text(),
            Marca: $("#MarcaId option:selected").text(),
            TemperatureId: $("#TemperatureId").val(),
            FactorCorreccionId: $("#FactorCorreccionId").val(),
            OperationType: op,
            Creadopor: $("#creadopor").val(),
            Fechacreacion: $("#fechacreacion").val() == "" ? undefined : $("#fechacreacion").val(),
            Modificadopor: $("#modificadopor").val(),
            Fechamodificacion: $("#fechamodificacion").val() == "" ? undefined : $("#fechamodificacion").val(),

        }

        await postData(domain + "/CorrectionFactor/SaveRegisterNozzleTypeMark", params)
            .then(data => {
                if (data.response.Code !== -1) {

                    ShowSuccessMessage(data.response.Description)
                    var newRow = {
                        Marca: params.Marca, Tipo: params.Tipo,
                        Temperatura: params.TemperatureId, Factor: params.FactorCorreccionId,
                        IdMarca: params.MarcaId, IdTipo: params.TipoId,
                        Fechacreacion: data.response.Structure.Fechacreacion, Fechamodificacion: data.response.Structure.Fechamodificacion,
                        Creadopor: data.response.Structure.Creadopor, Modificadopor: data.response.Structure.Modificadopor,
                    };

                    if (op === 1) {
                        var grid = $("#grid").data("kendoGrid");
                        grid.dataSource.insert(0, newRow);
                        grid.dataSource.sync();
                        grid.dataSource.page(1);
                    }
                    else
                    {
                        
                        var grid = $("#grid").data("kendoGrid");
                        var dataSource = grid.dataSource;
                        var index = dataSource.indexOf(itemToDelete);
                        dataSource.at(index).Factor = $("#FactorCorreccionId").val();
                        grid.setDataSource(dataSource);
                        grid.dataSource.page(1);
                    }
                    clearFields();
                }
                else {
                    ShowFailedMessage(data.response.Description);
                    btnClear.disabled = false
                    btnSave.disabled = false 
                }

            });
    }

    $("#loader").css("display", "none");
}


function deleteRegister() {
    modalConfirmDelete.show();
}


function newElement() {
    $("#btnNew").prop("disabled", true);
    $("#btnDelete").prop("disabled", true);

    $("#btnClear").prop("disabled", false);
    $("#btnSave").prop("disabled", false);

    $("#MarcaId").prop("disabled", false);
    $("#TipoId").prop("disabled", false);
    $("#FactorCorreccionId").prop("disabled", false);
    $("#TemperatureId").prop("disabled", false);

    isSave = true;
}


function clearFields() {
    $("#TipoId option[value!=0]").remove();
    $("#TemperatureId").val("");
    $("#FactorCorreccionId").val("");
    $("#creadopor").val("");
    $("#fechacreacion").val("");
    $("#modificadopor").val("");
    $("#fechamodificacion").val("");
    $('#MarcaId').prop('selectedIndex', 0);
    $('#TipoId').prop('selectedIndex', 0);

    $("#btnSave").prop("disabled", true);
    $("#btnDelete").prop("disabled", true);
    $("#btnNew").prop("disabled", false);

    $("#MarcaId").prop("disabled", true);
    $("#TipoId").prop("disabled", true);
    $("#TemperatureId").prop("disabled", true);
    $("#FactorCorreccionId").prop("disabled", true);

    $("#MarcaId_validationMessage").css("display", "none")
    $("#TipoId_validationMessage").css("display", "none")
    $("#FactorCorreccionId_validationMessage").css("display", "none")
    $("#TemperatureId_validationMessage").css("display", "none")

    isSave = false;
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


async function getData(url = '') {
    // Opciones por defecto estan marcadas con un *
    const response = await fetch(url, {
        method: 'GET', // *GET, POST, PUT, DELETE, etc.
        mode: 'same-origin', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
            'Content-Type': 'application/json'
            // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'same-origin' // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
    });
    if (response.ok) {
        return response.json(); // parses JSON response into native JavaScript objects
    }
    else {
        return null
    }

}

//$("#TemperatureId").keyup(function (event) {
//    $("#TemperatureId").val($("#TemperatureId").val().replace(/^0+/, ''));
//});