var editSave = /*false; edit true save false*/



    $('#LimiteMax').keypress(function (event) {
    
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            event.preventDefault();
          /*  editOrSaveRegister();*/
        }
        //Stop the event from propogation to other handlers
        //If this line will be removed, then keypress event handler attached 
        //at document level will also be triggered
        event.stopPropagation();
    });




$("#btnDeleteYes").click(function () {


    params = {
        Id: $("#id").val(),
        TipoReporte: $("#TipoReporte").val(),
        ClavePrueba: $("#ClavePrueba").val(),
        TipoAceite: $("#TipoAceite").val(),
        LimiteMax: $("#LimiteMax").val()
    }

    $("#loader").css("display", "block");

    postData(domain + "/LimitCgd/DeleteLimitCgd", params)
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
  
    var validate_form = $("#form_contgascgd").data("kendoValidator");
    if (!validate_form.validate()) {
        return
    }
    $("#loader").css("display", "block");

    if (!sendPost) {
        sendPost = true;
        params = {
            Id: $("#id").val(),
            TipoReporte: $("#TipoReporte").val(),
            ClavePrueba: $("#ClavePrueba").val(),
            TipoAceite: $("#TipoAceite").val(),
            LimiteMax: $("#LimiteMax").val(),
            DesPrueba: $("#desPrueba").val(),
            DesTipoReporte: $("#desTipoReporte").val(),
            IsSave: isSave
        }

        await postData(domain + "/LimitCgd/SaveLimitCgd", params)
            .then(datos => {
                if (datos.response.Code !== -1) {

                    var newRow = {
                        Id: params.Id,
                        TipoReporte: params.TipoReporte,
                        ClavePrueba: params.ClavePrueba,
                        TipoAceite: params.TipoAceite,
                        LimiteMax: params.LimiteMax,
                        DesPrueba: params.DesPrueba,
                        DesTipoReporte: params.DesTipoReporte,
                        Creadopor: datos.response.Structure.Creadopor,
                        Modificadopor: datos.response.Structure.Modificadopor,
                        Fechacreacion: datos.response.Structure.Fechacreacion,
                        Fechamodificacion: datos.response.Structure.Fechamodificacion,
                    };
                    if (isSave == true) {
                        var exist = false;
                        var grid = $("#grid").data("kendoGrid");
                        var dataSource = grid.dataSource;
                        var data = dataSource.data();
                        var repetido = false;
                        for (item in data) {
                            //if (data[item].TipoReporte == newRow.TipoReporte &&
                            //    data[item].ClavePrueba == newRow.ClavePrueba &&
                            //    data[item].TipoAceite == newRow.TipoAceite) {
                            //    repetido = true;
                            //}

                            if (data[item].TipoReporte == newRow.TipoReporte &&
                                data[item].ClavePrueba == newRow.ClavePrueba &&
                                data[item].TipoAceite == newRow.TipoAceite) {
                                exist = true;
                             
                                var index = dataSource.indexOf(data[item]);
                                dataSource.at(index).LimiteMax = newRow.LimiteMax;
                                grid.setDataSource(dataSource);
                                grid.dataSource.page(1);
                            
                                break;
                                
                            }
                        }
                        if (!exist) {
                            grid.dataSource.insert(0, newRow);
                            grid.dataSource.sync();
                            grid.dataSource.page(1);
                            clearFields();

                        }

                        if (exist) {
                            ShowWarningMessage("El límite máximo para el reporte, prueba y tipo de aceite ya existe")
                        } else {
                            clearFields();
                            ShowSuccessMessage(datos.response.Description)
                        }
                    }
                    else
                    {
                        var grid = $("#grid").data("kendoGrid");
                        var dataSource = grid.dataSource;
                        var index = dataSource.indexOf(itemToDelete);
                        dataSource.at(index).LimiteMax = newRow.LimiteMax;
                        grid.setDataSource(dataSource);
                        grid.dataSource.page(1);
                        ShowSuccessMessage("Actualización Exitosa")
                        clearFields();
                    }
               
                    sendPost = false;
                    $("#loader").css("display", "none");
                }
                else {
                    ShowFailedMessage(datos.response.Description);
                    sendPost = false;
                    $("#loader").css("display", "none");
                }
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

    $("#ClavePrueba").empty();
    $('#ClavePrueba').append('<option value="' + "" + '">' + 'Seleccione...' + '</option>');

    $("#TipoReporte").prop("disabled", false);
    $("#ClavePrueba").prop("disabled", false);
    $("#TipoAceite").prop("disabled", false);
    $("#LimiteMax").prop("disabled", false);
    $("#LimiteMax").val("");
    $("#TipoReporte").val("");
    $("#ClavePrueba").val("");
    $("#TipoAceite").val("");
    $("#id").val("0");
    $('#TipoReporte').focus().select()
    isSave = true;
}


function clearFields() {
    $("#TipoReporte").val("");
    $("#ClavePrueba").val("");
    $("#TipoAceite").val("");
    $("#LimiteMax").val("0");
    $("#id").val("0");
    $('#TipoReporte_validationMessage').css('display', 'none');
    $('#ClavePrueba_validationMessage').css('display', 'none');
    $('#TipoAceite_validationMessage').css('display', 'none');
    $('#LimiteMax_validationMessage').css('display', 'none');

    $("#btnSave").prop("disabled", true);
    $("#btnDelete").prop("disabled", true);
    $("#btnNew").prop("disabled", false);

    $("#TipoReporte").prop("disabled", true);
    $("#ClavePrueba").prop("disabled", true);
    $("#TipoAceite").prop("disabled", true);
    $("#LimiteMax").prop("disabled", true);


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