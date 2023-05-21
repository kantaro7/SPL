var sendPost = false;

$("#btnDeleteYes").click(function () {
    let estatus;
    if ($("#Estatus").val() == "1") {
        estatus = true;
    } else {
        estatus = false;
    }
    params = {
        IdMarca: $("#IdMarca").val(),
        Estatus: estatus,
        Descripcion: $("#Descripcion").val(),
    }

    $("#loader").css("display", "block");

    postData(domain + "/Mark/DeleteMark", params)
        .then(data => {
            if (data.response.Code != -1 && data.response.Structure != -1) {
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
                //clearFields();
            }
        });
}); 
async function editOrSaveRegister() {
    $("#btnSave").prop("disabled", true);
    $("#loader").css("display", "block");

    var validate_form = $("#form_mark").data("kendoValidator");
    if (!validate_form.validate()) {
        $("#loader").css("display", "none");
        $("#btnSave").prop("disabled", false);
        return
    }
    let estatus;
    if ($("#Estatus").val() == "1") {
        estatus = true;
    } else {
        estatus = false;
    }

    if (!sendPost) {
        sendPost = true;
        params = {
            IdMarca: $("#IdMarca").val(),
            Descripcion: $("#Descripcion").val(),
            Estatus: estatus,
            Creadopor: $("#creadopor").val(),
            Fechacreacion: $("#fechacreacion").val() == '' ? undefined : $("#fechacreacion").val(),
            Modificadopor: $("#modificadopor").val(),
            Fechamodificacion: $("#fechamodificacion").val() == '' ? undefined : $("#fechamodificacion").val(),
        }

        await postData(domain + "/Mark/SaveMark", params)
            .then(data => {
                if (data.response.Code !== -1) {

                    ShowSuccessMessage(data.response.Description)
                    var newRow = {
                        Marca: data.response.Code,
                        Descripción: params.Descripcion,
                        Estatus: params.Estatus ? "Activa" : "In-Activa",
                        Creadopor: params.Creadopor,
                        Fechacreacion: params.Fechacreacion,
                        Modificadopor: params.Modificadopor,
                        Fechamodificacion: params.Fechamodificacion,
                    };

                    if ($("#IdMarca").val() === "0") {
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
                        dataSource.at(index).Descripción = newRow.Descripción;
                        dataSource.at(index).Estatus = newRow.Estatus;
                        dataSource.at(index).Creadopor = newRow.Creadopor;
                        dataSource.at(index).Modificadopor = newRow.Modificadopor;
                        dataSource.at(index).Fechacreacion = newRow.Fechacreacion;
                        dataSource.at(index).Fechamodificacion = newRow.Fechamodificacion;
                        grid.setDataSource(dataSource);
                        grid.dataSource.page(1);
                    }
                    clearFields();
                    sendPost = false;
                    $("#loader").css("display", "none");
                }
                else {
                    ShowFailedMessage(data.response.Description);
                    $("#btnSave").prop("disabled", false);
                    sendPost = false;
                    $("#loader").css("display", "none");
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
    $("#IdMarca").val("0");
    $("#IdMarca").prop("disabled", true);
    $("#Estatus").val("1");
    $("#creadopor").val("");
    $("#modificadopor").val("");
    $("#fechacreacion").val("");
    $("#fechamodificacion").val("");
    $("#Estatus").prop("disabled", false);
    $("#Descripcion").prop("disabled", false);
    $("#Descripcion").focus();
    isSave = true;
}


function clearFields() {
    $("#IdMarca").val("");
    $("#Descripcion").val("");
    $("#Estatus").val("");
    $("#creadopor").val("");
    $("#modificadopor").val("");
    $("#fechacreacion").val("");
    $("#fechamodificacion").val("");

    $("#btnSave").prop("disabled", true);
    $("#btnDelete").prop("disabled", true);
    $("#btnNew").prop("disabled", false);

    $("#IdMarca").prop("disabled", true);
    $("#Descripcion").prop("disabled", true);
    $("#Estatus").prop("disabled", true);

    $("#IdMarca_validationMessage").css("display", "none")
    $("#Descripcion_validationMessage").css("display", "none")
    $("#Estatus_validationMessage").css("display", "none")
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