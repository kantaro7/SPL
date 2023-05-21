
//variables

let btnNew = document.getElementById("btnNew");
let btnSave = document.getElementById("btnSave");
let btnDelete = document.getElementById("btnDelete");
let btnClear = document.getElementById("btnClear");
let btnDeleteYes = document.getElementById("btnDeleteYes");
let brandInput = document.getElementById("BrandId");
let typeInput = document.getElementById("Type");
let descriptionInput = document.getElementById("Description");
let statusInput = document.getElementById("Status");
let creadoPorInput = document.getElementById("creadopor");
let fechaCreacionInput = document.getElementById("fechacreacion");
let modificadoPorInput = document.getElementById("modificadopor");
let fechaModificacionInput = document.getElementById("fechamodificacion");




//Events

btnNew.addEventListener("click", function () {

    newElement();

});

btnDeleteYes.addEventListener("click", function () {
    DeleteRequest();
});

//Function

async function editOrSaveRegister() {
    var validate_form = $("#nozzless_mark_type").data("kendoValidator");
    if (!validate_form.validate()) {
        return
    }
    $("#loader").css("display", "initial");
    btnSave.disabled = true;
    btnClear.disabled = true;

    let brandId = $("#BrandId").val();
    let description = descriptionInput.value;
    if (isSave) {
        await getData(domain + "/NozzleBrandType/ValidateNewNozzleTypeBrand/?brandId=" + brandId + "&description=" + description).then(data => {
            if (data.response.Code !== -1) {

                if (data.response.Structure !== 0) {

                    if (isSave) {
                        ShowWarningMessage("El tipo de boquilla por marca y descripción ya existe, favor de corregirlo");
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

        var model = {
            BrandId: $("#BrandId").val(),
            Type: $("#Type").val(),
            Description: $("#Description").val(),
            Status: $("#Status").val(),
            OperationType: op,
            CreadoPor: $("#creadopor").val(),
            FechaCreacion: $("#fechacreacion").val(),
            ModificadoPor: $("#modificadopor").val(),
            FechaModificacion: $("#fechamodificacion").val()

        }


        await postData(domain + "/NozzleBrandType/SaveRegisterNozzleTypeMark", model)
            .then(data => {
                if (data.response.Code !== -1) {

                    ShowSuccessMessage(data.response.Description)
                    var newRow = {
                        Marca: data.response.Structure.Marca,
                        IdTipo: data.response.Structure.IdTipo,
                        Descripcion: data.response.Structure.Descripcion,
                        Estatus: data.response.Structure.Estatus ? "Activo" : "Inactivo",
                        IdMarca: data.response.Structure.IdMarca,
                        EstatusId: data.response.Structure.EstatusId,
                        CreadoPor: data.response.Structure.Creadopor,
                        FechaCreacion: data.response.Structure.Fechacreacion,
                        ModificadoPor: data.response.Structure.Modificadopor,
                        FechaModificacion: data.response.Structure.Fechamodificacion
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
                        dataSource.at(index).Marca = data.response.Structure.Marca;
                        dataSource.at(index).IdTipo = data.response.Structure.IdTipo;
                        dataSource.at(index).Descripcion = data.response.Structure.Descripcion;
                        dataSource.at(index).Estatus = data.response.Structure.Estatus ? "Activo" : "Inactivo";
                        dataSource.at(index).IdMarca = data.response.Structure.IdMarca;
                        dataSource.at(index).EstatusId = data.response.Structure.EstatusId;
                        dataSource.at(index).CreadoPor = data.response.Structure.Creadopor;
                        dataSource.at(index).FechaCreacion = data.response.Structure.Fechacreacion;
                        dataSource.at(index).ModificadoPor = data.response.Structure.Modificadopor;
                        dataSource.at(index).FechaModificacion = data.response.Structure.Fechamodificacion;
                        grid.setDataSource(dataSource);
                        grid.dataSource.page(1);
                    }
                    clearFields();
                }
                else {
                    ShowFailedMessage(data.response.Description);
                }

            });
    }

    btnClear.disabled = false;
    $("#loader").css("display", "none");
}



async function DeleteRequest() {

    $("#loader").css("display", "initial");
    var params = {
        BrandId: $("#BrandId").val(),
        Type: $("#Type").val(),
        Description: $("#Description").val(),
        Status: $("#Status").val()
    }

    btnClear.disabled = true;
    btnSave.disabled = true;
    btnDelete.disabled = true;

   await postData(domain + "/NozzleBrandType/DeleteNozzlesBrandType", params)
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

}



function deleteRegister() {
    modalConfirmDelete.show();
}


function newElement() {

    btnNew.disabled = true;
    btnDelete.disabled = true;
    btnClear.disabled = false;
    btnSave.disabled = false;

    brandInput.disabled = false;
    typeInput.disabled = true;
    descriptionInput.disabled = false;
    statusInput.disabled = false;
    $('#Status').prop('selectedIndex', 1);
    isSave = true;
}


function clearFields() {

    $('#BrandId').prop('selectedIndex', 0);
    $('#Status').prop('selectedIndex', 0);
    typeInput.value = 0;
    descriptionInput.value = "";
    creadoPorInput.value = "";
    fechaCreacionInput.value = "";
    modificadoPorInput.value = "";
    fechaModificacionInput.value = "";

    $("#btnSave").prop("disabled", true);
    $("#btnDelete").prop("disabled", true);
    $("#btnNew").prop("disabled", false);

    brandInput.disabled = true;
    typeInput.disabled = true;
    descriptionInput.disabled = true;
    statusInput.disabled = true;

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