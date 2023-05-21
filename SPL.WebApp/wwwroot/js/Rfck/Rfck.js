//Variables
let viewModel;
let rows;
//let btnSave = document.getElementById("btnSave");
let btnAdd = document.getElementById("btnAdd");

function TodoBien(view, cant) {
    viewModel = view;
    rows = cant;
}

// Funciones
function EditRowData(i) {
    // ocultando botones principales
    $('#btnEdit_'+i).hide();
    $('#btnDelete_'+i).hide();

    // ocultando columnas de texto
    $('#ct_'+i).hide();
    $('#cf_'+i).hide();

    // monstrando botones de edicion
    $('#btnUpdate_'+i).show();
    $('#btnCancel_'+i).show();

    // Mostrando campos para edicion
    $('#txtct_' + i).show();
    $('#txtcf_' + i).show();

    // Asignando valores
    $('#txtct_' + i).val(viewModel.CorrectionFactors[i].CoolingType);
    $('#txtcf_' + i).val(viewModel.CorrectionFactors[i].FactorCorr);

    btnAdd.disabled = true;
    //btnSave.disabled = true;
}


function RemoveData(i) {
    // mostrando editar y borrar
    $('#btnEdit_' + i).show();
    $('#btnDelete_' + i).show();

    // mostrando tabla normal
    $('#ct_' + i).show();
    $('#cf_' + i).show();

    // ocultando actualizar y cancelar
    $('#btnUpdate_' + i).hide();
    $('#btnCancel_' + i).hide();

    // ocultando campos
    $('#txtct_' + i).hide();
    $('#txtcf_' + i).hide();

    // reestableciendo valores de los campos
    $('#inputct_' + i).val(viewModel.CorrectionFactors[i].CoolingType);
    $('#inputcf_' + i).val(viewModel.CorrectionFactors[i].FactorCorr);

    btnAdd.disabled = false;
    //btnSave.disabled = false;
}

function DeleteData(i) {
    Swal.fire({
        title: '¿Esta seguro de borrar este registro?',
        showDenyButton: true,
        confirmButtonText: 'Borrar',
        denyButtonText: `Cancelar`,
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            $('#Tr_' + i).remove();
            viewModel.CorrectionFactors[i].CoolingType = '';
            viewModel.CorrectionFactors[i].FactorCorr = 0;
            Swal.fire('¡Registro Borrado!', '', 'success');
            saveAllData();
        } else if (result.isDenied) {
            Swal.fire('¡Borrado Cancelado!', '', 'info')
        }
    })
}

function AddRowData() {
    var html =
        '<tr id ="Tr_' + rows + '"> '
        + '<td id="ct_' + rows + '" style="display:none;"></td>'
        + '<td id="cf_' + rows + '" style="display:none;"></td>'
        + '<td id="txtct_' + rows + '"><input type="text" id="inputct_' + rows + '"   value=""  /></td>'
        + '<td id="txtcf_' + rows + '"><input type="number" id="inputcf_' + rows + '"   value="0" /></td>'
        + '<td >'
        + '<input type="button" class="btn btn-primary" id="btnUpdate_' + rows + '"   value="Actualizar"  onclick="UpdateData(' + rows + ')" style="display:none;"/>'
        + '<input type="button" class="btn btn-primary" id="btnSave_' + rows + '"   value="Guardar"  onclick="SaveData(' + rows + ')"/ style="margin-right: 5px;">'
        + '<input type="button" class="btn btn-primary" id="btnCancelT_' + rows + '"  value="Cancelar" onclick="RemoveRow(this)";/>'
        + '<input type="button" class="btn btn-primary" id="btnCancel_' + rows + '" value="Cancelar" onclick="RemoveData(' + rows + ')" style="display:none;" />'
        + '<input type="button" class="btn btn-primary" id="btnEdit_' + rows + '" value="Editar" onclick="EditRowData(' + rows + ')" style="display:none;" />'
        + '<input type="button" class="btn btn-primary" id="btnDelete_' + rows + '" value="Eliminar" onclick="DeleteData(' + rows + ')" style="display:none;" />'
        + '</td>'
        + '</tr>';
    $(html).appendTo($("#CFTable"));
    btnAdd.disabled = true;
    //btnSave.disabled = true;
    $('#inputct_' + rows).focus();
}

function RemoveRow(obj) {
    var tr = $(obj).closest('tr');
    tr.remove();
    btnAdd.disabled = false;
}

function SaveData(i) {
    var ct = $('#inputct_' + i).val();
    var cf = $('#inputcf_' + i).val();
    if (Validate(ct, cf)) {
        $('#ct_' + i).html(ct);
        $('#cf_' + i).html(cf);
        $('#ct_' + i).show();
        $('#cf_' + i).show();
        $('#txtct_' + i).hide();
        $('#txtcf_' + i).hide();
        $('#btnSave_' + i).remove();
        $('#btnCancelT_' + i).remove();
        $('#btnEdit_' + i).show();
        $('#btnDelete_' + i).show();
        viewModel.CorrectionFactors.push({ 'CoolingType' : ct, 'FactorCorr' : cf });
        rows = rows + 1;
        btnAdd.disabled = false;
        //btnSave.disabled = false;
        saveAllData();
    }
}

function UpdateData(i) {
    var ct = $('#inputct_' + i).val();
    var cf = $('#inputcf_' + i).val();
    if (Validate(ct, cf)) {
        $('#ct_' + i).html(ct);
        $('#cf_' + i).html(cf);
        $('#ct_' + i).show();
        $('#cf_' + i).show();
        $('#txtct_' + i).hide();
        $('#txtcf_' + i).hide();
        $('#btnEdit_' + i).show();
        $('#btnDelete_' + i).show();
        $('#btnUpdate_' + i).hide();
        $('#btnCancel_' + i).hide();
        viewModel.CorrectionFactors[i].CoolingType = ct;
        viewModel.CorrectionFactors[i].FactorCorr = cf;
        rows = rows + 1;
        btnAdd.disabled = false;
        //btnSave.disabled = false;
        saveAllData();
    }
}

function Validate(ct, cf) {
    if (ct.trim() == '' || ct == undefined) {
        ShowFailedMessage("El Tipo de Enfriamiento es requerido");
        return false;
    }

    if (!(/^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ0-9@]*$/.test(ct))) {
        ShowFailedMessage("El Tipo de Enfriamiento debe contener solo letras y números");
        return false;
    }

    if (ct.length > 200) {
        ShowFailedMessage("El Tipo de Enfriamiento debe contener menos de 200 caracteres");
        return false;
    }

    if (cf.trim() == '' || cf == 0) {
        ShowFailedMessage("El Factor de Corrección es requerido");
        return false;
    }

    if (cf > 999.99) {
        ShowFailedMessage("El factor de corrección debe ser numérico mayor a cero considerando 3 enteros con 2 decimales");
        return false;
    }

    if (cf.split('.').length > 1) {
        if (cf.split('.')[1].length > 2) {
            ShowFailedMessage("El factor de corrección debe ser numérico mayor a cero considerando 3 enteros con 2 decimales");
            return false;
        }
    }

    let flag = false;

    for (var i = 0; i < viewModel.CorrectionFactors.length; i++) {
        if (viewModel.CorrectionFactors[i].CoolingType == ct && viewModel.CorrectionFactors[i].FactorCorr == cf) {
            flag = true;
        }
    }

    if (flag) {
        ShowFailedMessage("El factor de corrección para el tipo de enfriamiento ya existe, favor de corregirlo");
        return false;
    }
    return true;
}

function saveAllData () {
    $("#loader").css("display", "block");
    //btnSave.disabled = true;
    postData(domain + "/Rfck/Save/", viewModel)
        .then(data => {
            if (data.response.status !== -1) {
                ShowSuccessMessage("Registros Guardados Exitosamente.")
                //btnSave.disabled = false;
                setTimeout(location.reload(), 2000);
            }
            else {
                //btnSave.disabled = false;
                ShowFailedMessage(data.response.description);
            }
            $("#loader").css("display", "none");
        });
};

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