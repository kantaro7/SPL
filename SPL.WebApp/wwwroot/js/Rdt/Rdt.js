
//Variables
let viewModel;
let btnRequest = document.getElementById("btnRequest");
let btnClear = document.getElementById("btnClear");
let btnLoadTemplate = document.getElementById("btnLoadTemplate");
let noSerieInput = document.getElementById("NoSerie");
let CommentsInput = document.getElementById("Comments");
let noPruebaInput = document.getElementById("NoPrueba");
let angularDisplacement = document.getElementById("AngularDisplacement");
let clavePruebaInput = document.getElementById("ClavePrueba");
let norm = document.getElementById("Norm");
let connection = document.getElementById("Connection");
let spreadsheetElement;
let treeViewKendoElement;
let claveIdiomaInput = document.getElementById("ClaveIdioma");
let aTPosition = document.getElementById("ATPosition");
let bTPosition = document.getElementById("BTPosition");
let terPosition = document.getElementById("TerPosition");
let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");
let panelExcel = document.getElementById("spreadsheet");

//Mostrar info necesaria para consultar el reporte
document.querySelector('#btnInfo').addEventListener('click', () => {
    var text = 'Considerar que para este reporte:<br><br>'
        + 'El “No. De Serie” del Aparato se debe de encontrar registrado en SPL.<br>'
        + 'Contar con Información en la pantalla: “Diseño del Aparato”.<br>'
        + 'Se deben de contar con las plantillas cargadas para las pruebas correspondientes del reporte.';
    ShowWarningMessage(text);
});

//Evento


$("#NoSerie").focus();


$("#NoSerie").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $("#btnRequest").click();
    }
});

btnClear.addEventListener("click", function () {

    noSerieInput.value = '';
    //noPruebaInput.value = '';
    CommentsInput.value = '';
    claveIdiomaInput.value = '';
    clavePruebaInput.value = '';
 

    angularDisplacement.value = '';
    norm.value = '';
    connection.value = '-1';
    aTPosition.value = '';
    bTPosition.value = '';
    terPosition.value = '';

    aTPosition.options.length = 0;
    bTPosition.options.length = 0;
    terPosition.options.length = 0;

    aTPosition.disabled = true;
    bTPosition.disabled = true;
    terPosition.disabled = true;


    btnRequest.disabled = false;
    btnClear.disabled = false;
    btnLoadTemplate.disabled = true;
    noSerieInput.disabled = false;

    if (treeViewKendoElement !== undefined) {
        kendo.destroy(treeViewKendoElement);
        $("#treeview-kendo").empty();
    }
   

});

btnLoadTemplate.addEventListener("click", function () {
    if (ValidateForm()) {
        btnRequest.disabled = true;
        btnClear.disabled = false;
        btnLoadTemplate.disabled = true;
        MapToViewModel();
        $("#loader").css("display", "block");
        GetTemplateJSON();
    }
    else {
        $("#loader").css("display", "none");
    }
});

async function GetPDFJSON(code, tyoeReport) {
    var path = path = "/Rdt/GetPDFReport/";

    var url = new URL(domain + path),
        params = { code: code, typeReport: tyoeReport }
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
function onSelect(e) {
    var text = this.text(e.node);
    if (text.split('.').length > 1) {
        var id = text.split('.')[0].split('-')[2].split('_')[1];
        console.log("Selecting: " + this.text(e.node));
        console.log(id);


        GetPDFJSON(id, "RDT").then(
            data => {
                if (data != null) {

                    LoadPDF(this.text(e.node), data.data);
                }
                else {
                    ShowFailedMessage(data.response.Description);
                }
                $("#loader").css("display", "none");
            }
        );
    }

}
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

        GetFilterJSON(null).then(
            data => {
                if (data.response.Code !== -1) {
                    btnSave.disabled = false;
                    noSerieInput.disabled = true;
                    viewModel = data.response.Structure;
                    LoadTreeView(viewModel.TreeViewItem);
                    LoadForm(viewModel);
                    btnRequest.disabled = true;
                    btnClear.disabled = false;
                    btnLoadTemplate.disabled = false;
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
        btnLoadDataSidco.disabled = true;
    }
});

//Requests

async function GetFilterJSON() {
    var path = path = "/Rdt/GetFilter/";

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

async function GetTemplateJSON() {
    var path = path = "/Rdt/GetTemplate/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value, clavePrueba: clavePruebaInput.value, claveIdioma: claveIdiomaInput.value, angular: angularDisplacement.value, norm: norm.value, conexion: connection.value, posAT: aTPosition.value, posBT: bTPosition.value, posTer: terPosition.value, comment: CommentsInput.value }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))
    $("#loader").css("display", "none");
    var win = window.open(url.toString(), '_blank');
    win.focus();
}

//Functions


function LoadTreeView(treeViewModel) {
    treeViewKendoElement = $("#treeview-kendo").kendoTreeView({
        template: kendo.template($("#treeview").html()),
        dataSource: treeViewModel,
        dragAndDrop: false,
        checkboxes: false,
        select: onSelect,
        loadOnDemand: true
    });

}

function LoadForm(response) {
    claveIdiomaInput.value = response.ClaveIdioma;
    //noPruebaInput.value = response.NoPrueba;
    angularDisplacement.value = response.AngularDisplacement;
    norm.value = response.Norm;

    var length = aTPosition.options.length;
    for (i = length - 1; i > 0; i--) {
        aTPosition.options[i] = null;
    }
    length = bTPosition.options.length;
    for (i = length - 1; i > 0; i--) {
        bTPosition.options[i] = null;
    }
    length = terPosition.options.length;
    for (i = length - 1; i > 0; i--) {
        terPosition.options[i] = null;
    }
    var opt;

    if (response.ATCount == 0) {
        aTPosition.options.length = 0;
        aTPosition.disabled = true;
    } else {
        response.ATs.forEach(function (data, key) {

            opt = document.createElement("option");
            opt.value = data.Posicion;
            opt.innerHTML = data.Posicion;

            aTPosition.appendChild(opt);
        });
        aTPosition.removeAttribute('disabled');
    }

    if (response.BTCount == 0) {
        bTPosition.options.length = 0;
        bTPosition.disabled = true;
    } else {
        response.BTs.forEach(function (data, key) {

            opt = document.createElement("option");
            opt.value = data.Posicion;
            opt.innerHTML = data.Posicion;

            bTPosition.appendChild(opt);
        });
        bTPosition.removeAttribute('disabled');
    }

    if (response.TerCount == 0) {
        terPosition.options.length = 0;
        terPosition.disabled = true;
    } else {
        response.Ters.forEach(function (data, key) {

            opt = document.createElement("option");
            opt.value = data.Posicion;
            opt.innerHTML = data.Posicion;

            terPosition.appendChild(opt);
        });
        terPosition.removeAttribute('disabled');
    }
    
    if (response.ATCount == 0 || response.BTCount == 0 || response.TerCount == 0) {
        var length = clavePruebaInput.options.length;
        for (i = length - 1; i > 0; i--) {
            clavePruebaInput.options[i] = null;
        }
        response.Tests.forEach(function (data, key) {
            opt = document.createElement("option");
            opt.value = data.Clave;
            opt.innerHTML = data.Descripcion;
            clavePruebaInput.appendChild(opt);
        });
    }
    

    


}
var ResultValidations;

function ValidateForm() {

    ResultValidations = $("#form_menu_sup").data("kendoValidator").validate();

    $("#atSpan").removeClass('k-hidden');
    $("#btSpan").removeClass('k-hidden');
    $("#terSpan").removeClass('k-hidden');

    $('#atSpan').text("");
    $('#btSpan').text("");
    $('#terSpan').text("");

    if (ResultValidations) {

        if (clavePruebaInput.value == "ABT") {
            if (aTPosition.value == "Todas" && bTPosition.value == "Todas") {
       
                //$('#atSpand').text("Solo se permite un valor <Todas>");
               
                //$('#btSpand').text("Solo se permite un valor <Todas>");

                ShowFailedMessage("Solo se permite un valor <Todas> para la posición AT y BT ");
       
                return false;
            }
            if (aTPosition.value != "Todas" && bTPosition.value != "Todas") {
             
                //$('#atSpand').text("Debe haber almenos un valor <Todas>");
              
                //$('#btSpand').text("Debe haber almenos un valor <Todas>");
                ShowFailedMessage("Debe haber almenos un valor <Todas> para la posición AT y BT ");
          
    
                return false;
            }
        }
        if (clavePruebaInput.value == "ATT") {
            if (aTPosition.value == "Todas" && terPosition.value == "Todas") {
              
                //$('#atSpand').text("Solo se permite un valor <Todas>");
             
                //$('#terSpand').text("Solo se permite un valor <Todas>");
                ShowFailedMessage("Solo se permite un valor <Todas> para la posición AT y TER ");

                return false;
            }
            if (aTPosition.value != "Todas" && terPosition.value != "Todas") {
              
                //$('#atSpand').text("Debe haber almenos un valor <Todas>");
        
                //$('#terSpand').text("Debe haber almenos un valor <Todas>");
                ShowFailedMessage("Debe haber almenos un valor <Todas> para la posición AT y TER  ");

                return false;
            }
        }
        if (clavePruebaInput.value == "BTT") {
            if (terPosition.value == "Todas" && bTPosition.value == "Todas") {
           
                //$('#terSpand').text("Solo se permite un valor <Todas>");
           
                //$('#btSpand').text("Solo se permite un valor <Todas>");
                ShowFailedMessage("Solo se permite un valor <Todas> para la posición BT Y TER");

                return false;
            }
            if (terPosition.value != "Todas" && bTPosition.value != "Todas") {
       
                //$('#terSpand').text("Debe haber almenos un valor <Todas>");
             
                //$('#btSpand').text("Debe haber almenos un valor <Todas>");

                ShowFailedMessage("Debe haber almenos un valor <Todas> para la posición BT Y TER");

                return false;
            }
        }
       
        return true;
    }
    else {
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

function MapToViewModel(loadWorkbook = false, loadOfficial = false) {

    viewModel.ClaveIdioma = claveIdiomaInput.value;
    viewModel.ClavePrueba = clavePruebaInput.value;

    //viewModel.NoPrueba = noPruebaInput.value;
    viewModel.Comments = CommentsInput.value;

    viewModel.NoSerie = noSerieInput.value;
    viewModel.AngularDisplacement = angularDisplacement.value;
    viewModel.AngularDisplacementValue = angularDisplacement.options[angularDisplacement.selectedIndex].innerHTML;
    viewModel.Norm = norm.value;
    viewModel.Connection = connection.value;
    viewModel.ATPosition = aTPosition.value;
    viewModel.BTPosition = bTPosition.value;
    viewModel.TerPosition = terPosition.value;

    if (loadWorkbook)
        viewModel.Workbook.sheets = $("#spreadsheet").getKendoSpreadsheet().toJSON().sheets;
}