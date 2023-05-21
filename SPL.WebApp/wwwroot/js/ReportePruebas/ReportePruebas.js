
//Variables
let viewModel;
let claveIdiomaInput = document.getElementById("ClaveIdioma");
let btnRequest = document.getElementById("btnRequest");
let btnClear = document.getElementById("btnClear");
let noSerieInput = document.getElementById("NoSerie");
let reportInput = document.getElementById("Reporte");
let spreadsheetElement;
let treeViewKendoElement;;

let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");
let panelExcel = document.getElementById("spreadsheet");
let data =[]
btnRequest.disabled = false
//Evento
$("#NoSerie").focus();

//Mostrar info necesaria para consultar el reporte

$("#NoSerie").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $("#btnRequest").click();
    }
});

btnClear.addEventListener("click", function () {

    noSerieInput.disabled = false
    noSerieInput.value = '';
    reportInput.value =''
    btnRequest.disabled = false;

    if ($("#treeview").data("kendoTreeView") !== undefined) {
        $("#treeview").data("kendoTreeView").destroy()
        $("#treeview").empty();
    }
  
});

function LoadTreeView(treeViewModel) {

    $.each(viewModel.datasource, function (key, val) {


        let insertar = {
            PRUEBA: "Descripcion Prueba",
            isParent: false,
            IDIOMA: "Idioma",
            RESULTADO: "Resultado",
            COMENTARIOS: "Comentarios",
            FECHA: "Fecha",
            isHeader: true
        };
        val.items.splice(0, 0, insertar)
    });
    var ds = new kendo.data.HierarchicalDataSource({
        data: treeViewModel.datasource
    });
    
     $("#treeview").kendoTreeView({
        template: kendo.template($("#treeview-template").html()),
        dataSource: ds,
        dragAndDrop: false,
        loadOnDemand: false,
    }).data("kendoTreeView");
}

btnRequest.addEventListener("click", function () {
    if (ValidateForm()) {


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
    } else {
        return;
    }


    $("#loader").css("display", "block");
    if (noSerieInput.value) {
        CheckInfo().then(
            data => {
                if (data.Code !== -1) {
                    noSerieInput.disabled = true;
                    viewModel = data.Structure;
                    LoadTreeView(viewModel);
                    btnRequest.disabled = true;
                    btnClear.disabled = false;

                }
                else {
                    ShowFailedMessage(data.Description);
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
});

//Requests


//Functions

async function CheckInfo() {
    var path = path = "/ReportePruebas/CheckInfo/";

    var url = new URL(domain + path),
        params =
        {
            noSerie: noSerieInput.value,
            typeReport: reportInput.value,
        }
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




function selectedItem(esto) {
    var split = $(esto)[0].id.split("_")
    console.log(split)
    /*if ($(esto).is(":checked")) {
        var split = $(esto)[0].id.split("_")
        var obj = viewModel.datasource.find(x => x.items.find(y => y.ID_REP === parseInt(split[0]) && y.TIPO_REPORTE === split[1])).items.filter(o => o.ID_REP === parseInt(split[0]))
        if (obj !== null && obj !== undefined) {
            obj[0].isChecked = 'checked'
        }

        //data.push($(esto)[0].id)
    } else {
        var split = $(esto)[0].id.split("_")
        var obj = viewModel.datasource.find(x => x.items.find(y => y.ID_REP === parseInt(split[0]) && y.TIPO_REPORTE === split[1])).items.filter(o => o.ID_REP === parseInt(split[0]))
        if (obj !== null && obj !== undefined) {
            obj[0].isChecked = ''
        }
    }*/

    GetPDFJSON(parseInt( split[0]), split[1]).then(
        data => {
            if (data != null) {

                LoadPDF("", data.data);
            }
            else {
                ShowFailedMessage(data.response.Description);
            }
            $("#loader").css("display", "none");
        }
    );
}
function LoadForm(response) {
    claveIdiomaInput.value = response.ClaveIdioma;
    //noPruebaInput.value = response.NoPrueba;
    pruebas = response.CharacteristicsArtifact;

}
var ResultValidations;

function ValidateForm() {

    ResultValidations = $("#form_menu_sup").data("kendoValidator").validate();
    if (ResultValidations) {
        return true;
    } else {
        return false;
    }
}

function LoadPDF(nameFile, File) {
    var byteCharacters = atob(File);
    var byteNumbers = new Array(byteCharacters.length);
    for (var i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    var byteArray = new Uint8Array(byteNumbers);
    var file = new Blob([byteArray], { type: 'application/pdf;base64' });
    var fileURL = URL.createObjectURL(file);
    window.open(fileURL);
}


function showPdfInNewTab(base64Data, fileName) {

    let pdfWindow = window.open("");
    pdfWindow.document.write("<html<head><title>" + fileName + "</title><style>body{margin: 0px;}iframe{border-width: 0px;}</style></head>");
    pdfWindow.document.write("<body><embed width='100%' height='100%' src='data:application/pdf;base64, " + encodeURI(base64Data) + "#toolbar=0&navpanes=0&scrollbar=0'></embed></body></html>");
}

function onError(e) {
    console.log(e)
    console.log(e.sender)
}


//Functions This is likely because the managed PInvoke signature does not match the unmanaged target signature. Check that the calling convention and parameters of the PInvoke signature match the target unmanaged signature.
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

async function GetPDFJSON(code, typeReport) {
    var path = path = "/ReportePruebas/GetPDFReport/";

    var url = new URL(domain + path),
        params = { code: code, typeReport: typeReport }
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