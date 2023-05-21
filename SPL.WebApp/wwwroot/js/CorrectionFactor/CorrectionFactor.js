$("#btnDeleteYes").click(function () {

    params = {
        EspecificacionId: $("#EspecificacionId").val(),
        TemperatureId: $("#TemperatureId").val(),
        FactorCorreccionId: $("#FactorCorreccionId").val()
    }

    $("#loader").css("display", "block");

    postData(domain + "/CorrectionFactor/DeleteRegister", params)
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


async function editOrSaveRegister() {
    $("#btnSave").prop("disabled", true);
    $("#loader").css("display", "block");

    var validate_form = $("#form_factor_correccion").data("kendoValidator");
    if (!validate_form.validate()) {
        $("#loader").css("display", "none");
        $("#btnSave").prop("disabled", false);
        return
    }

    let temperatura = $("#TemperatureId").val();
    let especificacion = $("#EspecificacionId").val();

    if (isSave) {
        await getData(domain + "/CorrectionFactor/ValidateNewRegister/?Temperatura=" + temperatura + "&Especificacion=" + especificacion).then(data => {
            if (data.response.Code !== -1) {

                if (data.response.Structure.length > 0) {

                    if (isSave) {
                        ShowWarningMessage("El factor de corrección para la especificación y temperatura ya existe, favor de corregirlo");
                        sendPost = false;
                        $("#loader").css("display", "none");
                        $("#btnSave").prop("disabled", false);
                        return;
                    }
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
            EspecificacionId: $("#EspecificacionId").val(),
            TemperatureId: $("#TemperatureId").val(),
            FactorCorreccionId: $("#FactorCorreccionId").val(),
            OperationType: op,
            Creadopor: $("#creadopor").val(),
            Fechacreacion: $("#fechacreacion").val() == "" ? undefined : $("#fechacreacion").val(),
            Modificadopor: $("#modificadopor").val(),
            Fechamodificacion: $("#fechamodificacion").val() == "" ? undefined : $("#fechamodificacion").val(),
        }
        await postData(domain + "/CorrectionFactor/SaveRegister", params)
            .then(data => {
                if (data.response.Code !== -1) {

                    ShowSuccessMessage(data.response.Description)
                    console.log(data.response)
                    var newRow = {
                        Especificación: params.EspecificacionId, Temperatura: params.TemperatureId, Factor: params.FactorCorreccionId,
                        Creadopor: data.response.Structure.Creadopor, Modificadopor: data.response.Structure.Modificadopor,
                        Fechacreacion: data.response.Structure.Fechacreacion, Fechamodificacion:  data.response.Structure.Fechamodificacion
};

                    if (op === 1) {
                        var grid = $("#grid").data("kendoGrid");
                        grid.dataSource.insert(0, newRow);
                        grid.dataSource.sync();
                    }
                    else
                    {
                        
                        var grid = $("#grid").data("kendoGrid");
                        var dataSource = grid.dataSource;
                        var index = dataSource.indexOf(itemToDelete);
                        dataSource.at(index).Factor = $("#FactorCorreccionId").val();
                        grid.setDataSource(dataSource);
                    }
                    clearFields();
                    $("#loader").css("display", "none");
                }
                else {
                    ShowFailedMessage(data.response.Description);
                    $("#loader").css("display", "none");
                    $("#btnSave").prop("disabled", false);
                }
                $("#loader").css("display", "none");
            });
    }
}



function deleteRegister() {
    modalConfirmDelete.show();
}


function newElement() {
    $("#btnNew").prop("disabled", true);
    $("#btnDelete").prop("disabled", true);

    $("#btnClear").prop("disabled", false);
    $("#btnSave").prop("disabled", false);
 
    $("#EspecificacionId").prop("disabled", false);
    $("#FactorCorreccionId").prop("disabled", false);
    $("#TemperatureId").prop("disabled", false);

    isSave = true;
}


function clearFields() {
    $("#TemperatureId").val("");
    $("#FactorCorreccionId").val("");
    $("#creadopor").val("");
    $("#fechacreacion").val("");
    $("#modificadopor").val("");
    $("#fechamodificacion").val("");
    $('#EspecificacionId').prop('selectedIndex', 0);

    $("#btnSave").prop("disabled", true);
    $("#btnDelete").prop("disabled", true);
    $("#btnNew").prop("disabled", false);

    $("#EspecificacionId").prop("disabled", true);
    $("#FactorCorreccionId").prop("disabled", true);
    $("#TemperatureId").prop("disabled", true);

    $("#EspecificacionId_validationMessage").css("display", "none")
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
//    //alert(event.keyCode);
//    $("#TemperatureId").val($("#TemperatureId").val().replace(/^0+/, ''));
//});