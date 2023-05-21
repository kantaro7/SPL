
//Variables
let viewModel; 
let btnSave = document.getElementById("btnSave");
let btnRequest = document.getElementById("btnRequest");
let btnClear = document.getElementById("btnClear");
let noSerieInput = document.getElementById("NoSerie");
let tORLimite = document.getElementById("TORLimite");
let tORHoja = document.getElementById("TORHoja");
let aORLimite = document.getElementById("AORLimite");
let aORHoja = document.getElementById("AORHoja");
let gATLimite = document.getElementById("GATLimite");
let gATHoja = document.getElementById("GATHoja");
let gBTLimite = document.getElementById("GBTLimite");
let gBTHoja = document.getElementById("GBTHoja");
let gTERLimite = document.getElementById("GTERLimite");
let gTERHoja = document.getElementById("GTERHoja");
let aATLimite = document.getElementById("AATLimite");
let aATHoja = document.getElementById("AATHoja");
let aBTLimite = document.getElementById("ABTLimite");
let aBTHoja = document.getElementById("ABTHoja");
let aTERLimite = document.getElementById("ATERLimite");
let aTERHoja = document.getElementById("ATERHoja");
let hATLimite = document.getElementById("HATLimite");
let hATHoja = document.getElementById("HATHoja");
let hBTLimite = document.getElementById("HBTLimite");
let hBTHoja = document.getElementById("HBTHoja");
let hTERLimite = document.getElementById("HTERLimite");
let hTERHoja = document.getElementById("HTERHoja");
let cteTiempo = document.getElementById("CteTiempo");
let cteAmbiente = document.getElementById("CteAmbiente");
let bOR = document.getElementById("BOR");
let kWDiseno = document.getElementById("KWDiseno");
let tOI = document.getElementById("TOI");
let tOILimite = document.getElementById("TOILimite");

//Evento
$("#NoSerie").focus();
$("#NoSerie").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $("#btnRequest").click();
    }
});

function filtroEntero() {
    var tecla = event.key;
    if (['.', 'e'].includes(tecla))
        event.preventDefault()
}

function validateFloatKeyPress(input, evt, ints, decimals) {
    // Backspace = 8, Enter = 13, ‘0′ = 48, ‘9′ = 57, ‘.’ = 46, ‘-’ = 43
    

    return true;
}

btnClear.addEventListener("click", function () {
    noSerieInput.value = '';
    tORLimite.value = 0;
    tORLimite.disabled = true;
    tORHoja.value = 0;
    tORHoja.disabled = true;
    aORLimite.value = 0;
    aORLimite.disabled = true;
    aORHoja.value = 0;
    aORHoja.disabled = true;
    gATLimite.value = 0;
    gATLimite.disabled = true;
    gATHoja.value = 0;
    gATHoja.disabled = true;
    gBTLimite.value = 0;
    gBTLimite.disabled = true;
    gBTHoja.value = 0;
    gBTHoja.disabled = true;
    gTERLimite.value = 0;
    gTERLimite.disabled = true;
    gTERHoja.value = 0;
    gTERHoja.disabled = true;
    aATLimite.value = 0;
    aATLimite.disabled = true;
    aATHoja.value = 0;
    aATHoja.disabled = true;
    aBTLimite.value = 0;
    aBTLimite.disabled = true;
    aBTHoja.value = 0;
    aBTHoja.disabled = true;
    aTERLimite.value = 0;
    aTERLimite.disabled = true;
    aTERHoja.value = 0;
    aTERHoja.disabled = true;
    hATLimite.value = 0;
    hATLimite.disabled = true;
    hATHoja.value = 0;
    hATHoja.disabled = true;
    hBTLimite.value = 0;
    hBTLimite.disabled = true;
    hBTHoja.value = 0;
    hBTHoja.disabled = true;
    hTERLimite.value = 0;
    hTERLimite.disabled = true;
    hTERHoja.value = 0;
    hTERHoja.disabled = true;
    cteTiempo.value = 0;
    cteTiempo.disabled = true;
    cteAmbiente.value = 0;
    cteAmbiente.disabled = true;
    bOR.value = 0;
    bOR.disabled = true;
    kWDiseno.value = 0;
    kWDiseno.disabled = true;
    tOI.value = 0;
    tOI.disabled = true;
    tOILimite.value = 0;
    tOILimite.disabled = true;

    btnRequest.disabled = false;
    btnClear.disabled = false;
    btnSave.disabled = true;
    noSerieInput.disabled = false;

    if (treeViewKendoElement !== undefined) {
        kendo.destroy(treeViewKendoElement);
        $("#treeview-kendo").empty();
    }
});

btnSave.addEventListener("click", function () {
    if (ValidateForm(viewModel.AT, viewModel.BT, viewModel.TER)) {
        MapToViewModel();
        $("#loader").css("display", "block");
        btnRequest.disabled = true;
        postData(domain + "/Rdde/Save/", viewModel)
            .then(data => {
                if (data.response.status !== -1) {
                    ShowSuccessMessage("Guardado Exitoso.")
                    btnSave.disabled = true;
                    btnClear.click();
                }
                else {
                    btnSave.disabled = false;
                    ShowFailedMessage(data.response.description);
                }
                $("#loader").css("display", "none");
            });
    }
    else {
        $("#loader").css("display", "none");
    }
});

btnRequest.addEventListener("click", function () {
    if (noSerieInput.value === undefined || noSerieInput.value === "" || noSerieInput.value === null) {
        $(`#NoSerieSpand`).text("Requerido");
        return;
    }
    else {
        $(`#NoSerieSpand`).text("");
    }

    if (!(/^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ0-9-@]*$/.test(noSerieInput.value))) {
        $(`#NoSerieSpand`).text("Caracter(es) no permitido.");
        return;
    }
    else {
        $(`#NoSerieSpand`).text("");
    }

    $("#loader").css("display", "block");
    if (noSerieInput.value) {

        GetDataJSON(null).then(
            data => {
                if (data.response.Code !== -1) {
                    noSerieInput.disabled = true;
                    viewModel = data.response.Structure;
                    LoadForm(viewModel);
                    btnRequest.disabled = true;
                    btnClear.disabled = false;
                    btnSave.disabled = false;
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
    }
});

//Requests
async function GetDataJSON() {
    var path = "/Rdde/GetData/";

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
//Functions

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

function LoadForm(response) {
    tORLimite.value = response.TORLimite;
    tORHoja.value = response.TORHoja;
    aORLimite.value = response.AORLimite;
    aORHoja.value = response.AORHoja;
    gATLimite.value = response.GATLimite;
    gATHoja.value = response.GATHoja;
    gBTLimite.value = response.GBTLimite;
    gBTHoja.value = response.GBTHoja;
    gTERLimite.value = response.GTERLimite;
    gTERHoja.value = response.GTERHoja;
    aATLimite.value = response.AATLimite;
    aATHoja.value = response.AATHoja;
    aBTLimite.value = response.ABTLimite;
    aBTHoja.value = response.ABTHoja;
    aTERLimite.value = response.ATERLimite;
    aTERHoja.value = response.ATERHoja;
    hATLimite.value = response.HATLimite;
    hATHoja.value = response.HATHoja;
    hBTLimite.value = response.HBTLimite;
    hBTHoja.value = response.HBTHoja;
    hTERLimite.value = response.HTERLimite;
    hTERHoja.value = response.HTERHoja;
    cteTiempo.value = response.CteTiempo;
    cteAmbiente.value = response.CteAmbiente;
    bOR.value = response.BOR;
    kWDiseno.value = response.KWDiseno;
    tOI.value = response.TOI;
    tOILimite.value = response.TOILimite;
    tORLimite.disabled = false;
    tORHoja.disabled = false;
    aORLimite.disabled = false;
    aORHoja.disabled = false;
    gATLimite.disabled = false;
    gATHoja.disabled = false;
    gBTLimite.disabled = false;
    gBTHoja.disabled = false;
    gTERLimite.disabled = false;
    gTERHoja.disabled = false;
    aATLimite.disabled = false;
    aATHoja.disabled = false;
    aBTLimite.disabled = false;
    aBTHoja.disabled = false;
    aTERLimite.disabled = false;
    aTERHoja.disabled = false;
    hATLimite.disabled = false;
    hATHoja.disabled = false;
    hBTLimite.disabled = false;
    hBTHoja.disabled = false;
    hTERLimite.disabled = false;
    hTERHoja.disabled = false;
    cteTiempo.disabled = false;
    cteAmbiente.disabled = false;
    bOR.disabled = false;
    kWDiseno.disabled = false;
    tOI.disabled = false;
    tOILimite.disabled = false;
}
var ResultValidations;
function ValidateForm(at = false, bt = false, ter = false) {
    ResultValidations = $("#form_menu_sup").data("kendoValidator").validate();
    if (ResultValidations) {
        if (at) {
            if (parseInt(gATLimite.value) == 0 || parseInt(gATLimite.value) == undefined)  {
                ShowFailedMessage("El Gradiente Limite de Alta Tensión es requerido y debe ser mayor a 0");
                return false;
            }

            if (parseInt(gATHoja.value) == 0 || parseInt(gATHoja.value) == undefined)  {
                ShowFailedMessage("El Gradiente Hoja de Enf. de Alta Tensión es requerido y debe ser mayor a 0");
                return false;
            }

            if (parseInt(aATLimite.value) == 0 || parseInt(aATLimite.value) == undefined)  {
                ShowFailedMessage("El AWR Limite de Alta Tensión es requerido y debe ser mayor a 0");
                return false;
            }

            if (parseInt(aATHoja.value) == 0 || parseInt(aATHoja.value) == undefined)  {
                ShowFailedMessage("El AWR Hoja de Enf. de Alta Tensión es requerido y debe ser mayor a 0");
                return false;
            }

            if (parseInt(hATLimite.value) == 0 || parseInt(hATLimite.value) == undefined)  {
                ShowFailedMessage("El HSR Limite de Alta Tensión es requerido y debe ser mayor a 0");
                return false;
            }

            if (parseInt(hATHoja.value) == 0 || parseInt(hATHoja.value) == undefined)  {
                ShowFailedMessage("El HSR Hoja de Enf. de Alta Tensión es requerido y debe ser mayor a 0");
                return false;
            }

        }

        if (bt) {
            if (parseInt(gBTLimite.value) == 0 || parseInt(gBTLimite.value) == undefined)  {
                ShowFailedMessage("El Gradiente Limite de Baja Tensión es requerido y debe ser mayor a 0");
                return false;
            }

            if (parseInt(gBTHoja.value) == 0 || parseInt(gBTHoja.value) == undefined)  {
                ShowFailedMessage("El Gradiente Hoja de Enf. de Baja Tensión es requerido y debe ser mayor a 0");
                return false;
            }

            if (parseInt(aBTLimite.value) == 0 || parseInt(aBTLimite.value) == undefined)  {
                ShowFailedMessage("El AWR Limite de Baja Tensión es requerido y debe ser mayor a 0");
                return false;
            }

            if (parseInt(aBTHoja.value) == 0 || parseInt(aBTHoja.value) == undefined)  {
                ShowFailedMessage("El AWR Hoja de Enf. de Baja Tensión es requerido y debe ser mayor a 0");
                return false;
            }

            if (parseInt(hBTLimite.value) == 0 || parseInt(hBTLimite.value) == undefined)  {
                ShowFailedMessage("El HSR Limite de Baja Tensión es requerido y debe ser mayor a 0");
                return false;
            }

            if (parseInt(hBTHoja.value) == 0 || parseInt(hBTHoja.value) == undefined)  {
                ShowFailedMessage("El HSR Hoja de Enf. de Baja Tensión es requerido y debe ser mayor a 0");
                return false;
            }

        }

        if (ter) {
            if (parseInt(gTERLimite.value) == 0 || parseInt(gTERLimite.value) == undefined)  {
                ShowFailedMessage("El Gradiente Limite de Terciario es requerido y debe ser mayor a 0");
                return false;
            }

            if (parseInt(gTERHoja.value) == 0 || parseInt(gTERHoja.value) == undefined)  {
                ShowFailedMessage("El Gradiente Hoja de Enf. de Terciario es requerido y debe ser mayor a 0");
                return false;
            }

            if (parseInt(aTERLimite.value) == 0 || parseInt(aTERLimite.value) == undefined)  {
                ShowFailedMessage("El AWR Limite de Terciario es requerido y debe ser mayor a 0");
                return false;
            }

            if (parseInt(aTERHoja.value) == 0 || parseInt(aTERHoja.value) == undefined)  {
                ShowFailedMessage("El AWR Hoja de Enf. de Terciario es requerido y debe ser mayor a 0");
                return false;
            }

            if (parseInt(hTERLimite.value) == 0 || parseInt(hTERLimite.value) == undefined)  {
                ShowFailedMessage("El HSR Limite de Terciario es requerido y debe ser mayor a 0");
                return false;
            }

            if (parseInt(hTERHoja.value) == 0 || parseInt(hTERHoja.value) == undefined)  {
                ShowFailedMessage("El HSR Hoja de Enf. de Terciario es requerido y debe ser mayor a 0");
                return false;
            }

        }
        return true;
    }else {
        return false;
    }
}

function MapToViewModel() {
    viewModel.NoSerie = noSerieInput.value;
    viewModel.TORLimite = tORLimite.value;
    viewModel.TORHoja = tORHoja.value;
    viewModel.AORLimite = aORLimite.value;
    viewModel.AORHoja = aORHoja.value;
    viewModel.GATLimite = gATLimite.value;
    viewModel.GATHoja = gATHoja.value;
    viewModel.GBTLimite = gBTLimite.value;
    viewModel.GBTHoja = gBTHoja.value;
    viewModel.GTERLimite = gTERLimite.value;
    viewModel.GTERHoja = gTERHoja.value;
    viewModel.AATLimite = aATLimite.value;
    viewModel.AATHoja = aATHoja.value;
    viewModel.ABTLimite = aBTLimite.value;
    viewModel.ABTHoja = aBTHoja.value;
    viewModel.ATERLimite = aTERLimite.value;
    viewModel.ATERHoja = aTERHoja.value;
    viewModel.HATLimite = hATLimite.value;
    viewModel.HATHoja = hATHoja.value;
    viewModel.HBTLimite = hBTLimite.value;
    viewModel.HBTHoja = hBTHoja.value;
    viewModel.HTERLimite = hTERLimite.value;
    viewModel.HTERHoja = hTERHoja.value;
    viewModel.CteTiempo = cteTiempo.value;
    viewModel.CteAmbiente = cteAmbiente.value;
    viewModel.BOR = bOR.value;
    viewModel.KWDiseno = kWDiseno.value;
    viewModel.TOI = tOI.value;
    viewModel.TOILimite = tOILimite.value;
}