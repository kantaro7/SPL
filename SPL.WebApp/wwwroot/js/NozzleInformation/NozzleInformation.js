function consult() {
    sonNuevosRegistros = false
    var nroSerie = $("#NumeroSerie").val();
    var myRegEx = new RegExp('^.*(?=.*[0-9])(?=.*[A-Za-z])(?=.*[-])'); //make sure the var is a number
    var validate_form = $("#form_nozzle_information").data("kendoValidator");
    if (!validate_form.validate()) {
        return
    }
    var result = myRegEx.exec(nroSerie);

    if (result === null) {
        alert("Solo puede contener letras, números y el guion");
        $("#NumeroSerieCustomValidator").text("Solo puede contener letras, números y el guion");
        $("#NumeroSerieCustomValidator").removeClass("k-hidden")
        //$("#NumeroSerie_validationMessage").removeClass("k-hidden")
    } else {
        $("#loader").css("display", "block");
        getData(domain + "/NozzleInformation/GetRecordNozzleInformation/?numeroSerie=" + nroSerie).then(data => {
            //console.log(data)

            if (data.Code !== -1) {

                if (data.Structure.TotalQuantity == 0) {
                    ShowWarningMessage(data.Description);
                    $("#CantidadTotal").val("")
                    $("#loader").css("display", "none");
                } else {
                    totalQuantity = data.Structure.TotalQuantity
                    $("#CantidadTotal").val(data.Structure.TotalQuantity)
                    sonNuevosRegistros = data.Structure.NozzleInformation.length == 0 ? true : false
                    setDataInTable(data.Structure.NozzleInformation, data.Structure.TotalQuantity);
                    $("#btnSave").prop("disabled",false)
                    $("#NumeroSerie").prop("disabled",true)
                    $("#btnConsult").prop("disabled",true)
                    $("#btnAdd").prop("disabled", false)
                    $("#loader").css("display", "none");
                }
            }
            else {
                ShowFailedMessage(data.Description);
                $("#loader").css("display", "none");
            }
        });
    }

}


function clearFields() {
    if ($('#grid').data().kendoGrid != null) {
        $('#grid').data().kendoGrid.destroy();
        $('#grid').empty();
    }
  
    //$("#grid").data("kendoGrid").dataSource.data([]);
    $("#NumeroSerie").val("")
    $("#CantidadTotal").val("")

    $("#btnSave").prop("disabled", true)
    $("#NumeroSerie").prop("disabled", false)
    $("#btnConsult").prop("disabled", false)
    $("#btnAdd").prop("disabled", true)

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
