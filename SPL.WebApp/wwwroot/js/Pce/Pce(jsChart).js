
//Variables
let viewModel;
let settingsToDisplayRODReportsDTO;
let btnRequest = document.getElementById("btnRequest");
let btnSave = document.getElementById("btnSave");
let btnValidate = document.getElementById("btnValidate");
let btnClear = document.getElementById("btnClear");
let btnLoadTemplate = document.getElementById("btnLoadTemplate");
let noSerieInput = document.getElementById("NoSerie");
let CommentsInput = document.getElementById("Comments");
let noPruebaInput = document.getElementById("NoPrueba");
let claveIdiomaInput = document.getElementById("ClaveIdioma");
let officialWorkbook;

let clavePruebaInput = document.getElementById("ClavePrueba");
let testPositionAT = document.getElementById("TestPositionAT");
let testPositionBT = document.getElementById("TestPositionBT");
let testPositionTer = document.getElementById("TestPositionTer");
let windingEnergized = document.getElementById("WindingEnergized");
let vNStart = document.getElementById("VNStart");
let vNEnd = document.getElementById("VNEnd");
let vNInterval = document.getElementById("VNInterval");
let grafic = document.getElementById("Grafic");
let spreadsheetElement;
let treeViewKendoElement;

let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");
let panelExcel = document.getElementById("spreadsheet");

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


    testPositionAT.value = '';
    testPositionBT.value = '';
    testPositionTer.value = '';
    windingEnergized.value = '';
    vNStart.value = '90';
    vNEnd.value = '110';
    vNInterval.value = '10';
    grafic.value = false;

    testPositionAT.options.length = 0;
    testPositionBT.options.length = 0;
    testPositionTer.options.length = 0;

    testPositionAT.disabled = true;
    testPositionBT.disabled = true;
    testPositionTer.disabled = true;

    btnRequest.disabled = false;
    btnClear.disabled = false;
    btnLoadTemplate.disabled = true;
    btnSave.disabled = true;
    btnValidate.disabled = true;
    noSerieInput.disabled = false;

    if (spreadsheetElement !== undefined && spreadsheetElement.data("kendoSpreadsheet") !== undefined) {
        spreadsheetElement.data("kendoSpreadsheet").destroy();
        $("#spreadsheet").empty();
    }
    if (treeViewKendoElement !== undefined) {
        kendo.destroy(treeViewKendoElement);
        $("#treeview-kendo").empty();
    }
   

});

btnLoadTemplate.addEventListener("click", function () {
    if (ValidateForm()) {

        MapToViewModel();

        $("#loader").css("display", "block");
        GetTemplateJSON().then(
            data => {
                if (data.response.Code !== -1) {

                    viewModel.PCEReportsDTO = data.response.Structure;
                    LoadTemplate(data.workbook)
                    officialWorkbook = data.workbook2;
                    viewModel.OfficialWorkbook = data.workbook2;
                    viewModel.Workbook = data.workbook;
                    viewModel.NoPrueba = data.noPrueba;
                    //noPruebaInput.value = data.noPrueba;
                    btnRequest.disabled = true;
                    btnClear.disabled = false;
                    btnLoadTemplate.disabled = true;
                    btnSave.disabled = true;
                    btnValidate.disabled = false;
                    initCss();
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
      //  ShowFailedMessage("Hay Campos que faltan.");
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
                    btnSave.disabled = false;
                    btnValidate.disabled = true;
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
    var path = path = "/Pce/GetFilter/";

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
    var path = path = "/Pce/GetTemplate/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value, clavePrueba: clavePruebaInput.value, claveIdioma: claveIdiomaInput.value, testPositionAT: testPositionAT.value, testPositionBT: testPositionBT.value, testPositionTer: testPositionTer.value, vNStart: vNStart.value, vNEnd: vNEnd.value, vNInterval: vNInterval.value, windingEnergized: windingEnergized.value, grafic: grafic.checked}
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

//Functions

function LoadTemplate(workbook, grafico = "") {
    SetConfigExcel(workbook, grafico);
}
function initCss() {
    $(".k-tabstrip-items").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-button-icontext").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-spreadsheet-quick-access-toolbar").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-link").css({ "color": "#fff" });
}


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


function onSelect(e) {
    var text = this.text(e.node);
    if (text.split('.').length > 1) {
        var id = text.split('.')[0].split('-')[2].split('_')[1];
        console.log("Selecting: " + this.text(e.node));
        console.log(id);


        GetPDFJSON(id, "PCE").then(
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

async function GetPDFJSON(code, tyoeReport) {
    var path = path = "/Pce/GetPDFReport/";

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

function LoadForm(response) {
    claveIdiomaInput.value = response.ClaveIdioma;
    //noPruebaInput.value = response.NoPrueba;

    var ATs = response.TestPositionsAT.length;
    var BTs = response.TestPositionsBT.length;
    var Ters = response.TestPositionsTer.length;
    var WEs = response.WindingsEnergized.length;
    var opt;
    var length;
    if (ATs > 1) {
        testPositionAT.removeAttribute('disabled');

        length = testPositionAT.options.length;
        for (i = length - 1; i > 0; i--) {
            testPositionAT.options[i] = null;
        }

        response.TestPositionsAT.forEach(function (data, key) {
            opt = document.createElement("option");
            opt.value = data.Value;
            opt.innerHTML = data.Text;
            testPositionAT.appendChild(opt);
        });
    }

    if (BTs > 1) {
        testPositionBT.removeAttribute('disabled');

        length = testPositionBT.options.length;
        for (i = length - 1; i > 0; i--) {
            testPositionBT.options[i] = null;
        }

        response.TestPositionsBT.forEach(function (data, key) {
            opt = document.createElement("option");
            opt.value = data.Value;
            opt.innerHTML = data.Text;
            testPositionBT.appendChild(opt);
        });
    }

    if (Ters > 1) {
        testPositionTer.removeAttribute('disabled');

        length = testPositionTer.options.length;
        for (i = length - 1; i > 0; i--) {
            testPositionTer.options[i] = null;
        }

        response.TestPositionsTer.forEach(function (data, key) {
            opt = document.createElement("option");
            opt.value = data.Value;
            opt.innerHTML = data.Text;
            testPositionTer.appendChild(opt);
        });
    }

    if (WEs > 0) {
        length = windingEnergized.options.length;
        for (i = length - 1; i > 0; i--) {
            windingEnergized.options[i] = null;
        }

        response.WindingsEnergized.forEach(function (data, key) {
            opt = document.createElement("option");
            opt.value = data.Value;
            opt.innerHTML = data.Text;
            windingEnergized.appendChild(opt);
        });
    }
    windingEnergized.value = response.LessWindingEnergized;

    
}
var ResultValidations;

function ValidateForm() {

    ResultValidations = $("#form_menu_sup").data("kendoValidator").validate();
    if (ResultValidations) {
        if (viewModel.AT && testPositionAT.value == "") {
            ShowFailedMessage("Posicion AT es requerida");
            return false;
        }

        if (viewModel.BT && testPositionBT.value == "") {
            ShowFailedMessage("Posicion BT es requerida");
            return false;
        }

        if (viewModel.Ter && testPositionTer.value == "") {
            ShowFailedMessage("Posicion Ter es requerida");
            return false;
        }

        return true;
    }else {
        return false;
    }
}

btnSave.addEventListener("click", function () {
    if (viewModel.Grafic) {
        var ncanva = document.getElementById("myChart");
        viewModel.Base64Graphic = ncanva.toDataURL("image/png").replace('data:image/png;base64,','');
    }
    MapToViewModel(true, true);
    $("#loader").css("display", "block");
    postData(domain + "/Pce/SavePDF/", viewModel)
        .then(data => {
            if (data.response.status !== -1) {
                ShowSuccessMessage("Guardado Exitoso.")
                btnRequest.disabled = true;
                btnClear.disabled = false;
                btnLoadTemplate.disabled = true;
                btnSave.disabled = true;
                btnValidate.disabled = true;
                noSerieInput.disabled = true;
                btnValidate.disabled = true;
                LoadPDF(data.response.nameFile, data.response.file);

                initCss();
            }
            else {
                ShowFailedMessage(data.response.description);
            }
            $("#loader").css("display", "none");
        });

});

btnValidate.addEventListener("click", function () {
    $("#loader").css("display", "block");
    MapToViewModel(true);
    var data = viewModel;

    postData(domain + "/Pce/ValidateData/", data)
        .then(data => {
            if (data.response.Code === -2) {
                ShowFailedMessage(data.response.Description);

                btnSave.disabled = false;
                btnValidate.disabled = false;
                btnClear.disabled = false;
                noSerieInput.disabled = true;
            }
            else if (data.response.Code !== -1) {
                viewModel = data.response.Structure;
                ShowSuccessMessage("Validación Exitosa. Por favor corrobore los calculos.")

                btnRequest.disabled = true;
                btnClear.disabled = false;
                btnLoadTemplate.disabled = true;
                btnSave.disabled = false;
                noSerieInput.disabled = true;
                btnValidate.disabled = false;

                initCss();

                if (viewModel.Grafic) {
                    GenerateGraphic(data.response.Coords);
                    var ncanva = document.getElementById("myChart");
                    viewModel.Base64Graphic = ncanva.toDataURL("image/png").replace('data:image/png;base64,', '');;
                    SetDataSourceSpredsheet(viewModel.Workbook, viewModel.Base64Graphic);
                } else {
                    SetDataSourceSpredsheet(viewModel.Workbook);

                }

                if (data.response.Description !== '') {
                    ShowFailedMessage(data.response.Description);
                }
            }
            else {
                viewModel = data.response.Structure;
                SetDataSourceSpredsheet(viewModel.Workbook);
                ShowFailedMessage(data.response.Description);
                btnSave.disabled = false;
                btnValidate.disabled = false;
                btnClear.disabled = false;
                noSerieInput.disabled = true;
                initCss();
            }
            $("#loader").css("display", "none");
        });

});

function SetDataSourceSpredsheet(workbook, grafico = "") {
    spreadsheetElement.data("kendoSpreadsheet").destroy()
    $("#spreadsheet").empty();
    SetConfigExcel(workbook, grafico);
}





function SetConfigExcel(workbook, grafico = "") {
    spreadsheetElement = $("#spreadsheet").kendoSpreadsheet({
        sheets: workbook.sheets,
        toolbar: {
            backgroundColor: "#3f51b5 !important",
            textColor: "#fff",
            home: [

                {
                    type: "button",
                    text: "Calcular",
                    showText: "both",
                    icon: "k-icon k-i-calculator",


                    click: function () {
                        btnValidate.click();
                    }
                },
                {
                    type: "button",
                    text: "Guardar",
                    showText: "both",
                    icon: "k-icon k-i-save",

                    click: function () {
                        btnSave.click();
                    }
                }
            ],
            insert: false,
            data: false,
            redo: false,
            undo: false,
        }
    });

    var spreadsheet = $("#spreadsheet").data("kendoSpreadsheet");
    var sheet = spreadsheet.activeSheet();
    var drawing = kendo.spreadsheet.Drawing.fromJSON({
        topLeftCell: "A1",
        offsetX: 0,
        offsetY: 0,
        width: 220,
        height: 43,
        image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    });
    sheet.addDrawing(drawing);
    if (grafico != "") {
        var graficoo = kendo.spreadsheet.Drawing.fromJSON({
            topLeftCell: "C25",
            offsetX: 0,
            offsetY: 0,
            width: 420,
            height: 380,
            image: spreadsheet.addImage(grafico)
        });
        sheet.addDrawing(graficoo);
    }
    
    var range = spreadsheet.activeSheet().range("A60:J200");
    range.enable(false);

    
    
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

function GenerateGraphic(datas) {
    var valorx = [];
    var valory = [];
    valorx[0] = 0;
    valory[0] = 0;
    for (var i = 1; i < datas.length + 1; i++) {
        valorx[i] = datas[i-1].x;
        valory[i] = datas[i-1].y;
    }


    var ctx = document.getElementById('myChart').getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: valorx,
            datasets: [{
                data: valory,
                fill: false,
                backgroundColor: 'rgb(0,0,0)',
                bordercolor: 'rgb(0,0,0)',
                tension: 0.5
            }]
        },
        options: {
            responsive: true,
            backgroundColor: 'rgb(0,0,0)',
            plugins: {
                legend: {
                    position: 'top',
                },
                title: {
                    display: true,
                    text: 'Grafico PCE'
                }
            }
        },
        scales: {
            x: {
                grid: {
                    display: true,
                    drawOnChartArea: true,
                    drawTicks: true,
                }
            },
            y: {
                grid: {
                    display: true,
                    drawBorder: true,
                    color: 'rgb(0,0,0)'
                },
            }
        }
    });
}

function MapToViewModel(loadWorkbook = false, loadOfficial = false) {

    viewModel.ClaveIdioma = claveIdiomaInput.value;
    viewModel.ClavePrueba = clavePruebaInput.value;
    //viewModel.NoPrueba = noPruebaInput.value;
    viewModel.Comments = CommentsInput.value;
    viewModel.NoSerie = noSerieInput.value;

    viewModel.TestPositionAT = testPositionAT.value;
    viewModel.TestPositionBT = testPositionBT.value;
    viewModel.TestPositionTer = testPositionTer.value;
    viewModel.WindingEnergized = windingEnergized.value;
    viewModel.VNStart = vNStart.value;
    viewModel.VNEnd = vNEnd.value;
    viewModel.VNInterval = vNInterval.value;
    viewModel.Grafic = grafic.value;

    if (loadWorkbook)
        viewModel.Workbook.sheets = $("#spreadsheet").getKendoSpreadsheet().toJSON().sheets;
    if (loadOfficial)
        viewModel.OfficialWorkbook = officialWorkbook;
}