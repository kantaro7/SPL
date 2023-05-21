
//Variables
let viewModel;
let settingsToDisplayPCIReportsDTO;
let btnRequest = document.getElementById("btnRequest");
let btnClear = document.getElementById("btnClear");
let btnLoadTemplate = document.getElementById("btnLoadTemplate");
let noSerieInput = document.getElementById("NoSerie");
let CommentsInput = document.getElementById("Comments");
let noPruebaInput = document.getElementById("NoPrueba");
//noPruebaInput.value = '';
let claveIdiomaInput = document.getElementById("ClaveIdioma");
let clavePruebaInput = document.getElementById("ClavePrueba");
let voltajePruebaInput = document.getElementById("VoltajePrueba");

let spreadsheetElement;
let treeViewKendoElement;

let topPane = document.getElementById("top-pane");
let bottonPane = document.getElementById("botton-pane");
let panelExcel = document.getElementById("spreadsheet");
let pasar = false
let pasarSelect = false
let letraSec =''
let letraPri = ''
let pruebas = []
let arrPri = []
let arrSec = []
let statusAllPosSec = false
let contRadio = 0

//Evento

function clearPosiciones(){
    $("#selectAT").prop("disabled",false)
    $("#ATPrimaria").prop("disabled",false)
    $("#ATSecundaria").prop("disabled",false)

    $("#selectBT").prop("disabled",false)
    $("#BTPrimaria").prop("disabled",false)
    $("#BTSecundaria").prop("disabled",false)

    $("#selectTer").prop("disabled",false)
    $("#TerPrimaria").prop("disabled",false)
    $("#TerSecundaria").prop("disabled",false)

    pasarSelect = false
    pasar = false
   
}

$("#NoSerie").focus();

//Mostrar info necesaria para consultar el reporte
document.querySelector('#btnInfo').addEventListener('click', () => {
    var text =
        'Considerar que para este reporte:<br><br>'
        + 'El “No. De Serie” del Aparato se debe de encontrar registrado en SPL.<br>'
        + 'Se debe contar con Información en la pantalla: “Diseño del Aparato”.<br>'
        + 'Se deben de contar con las plantillas cargadas para las pruebas correspondientes del reporte:<br>'
        + '<ul>'
        + '<li>Única (Ambos Idiomas)</li>'
        + '</ul>'
        + '<br>'
        + 'Se deberá de ingresar el voltaje de la prueba que se encuentre en existencia en la pantalla: “Diseño del Aparato”, dicha validación se realizará al intentar cargar la plantilla.<br>'
        + 'Se deberá de seleccionar solo 2 combinaciones de posiciones, no 3 al mismo tiempo, además de que una de las 2 deberá de ser solo la primaria, es decir seleccionar solo una de las posiciones y la otra la secundaria, es decir, podrá seleccionar 1 o más posiciones para este caso.';
    ShowWarningMessageHtmlContent(text);
});

$("#ClavePrueba").change(function () {

    //clearPosiciones()

    //if(this.value == "AYB"){
    //    $("#selectTer").prop("disabled",true)
    //    $("#TerPrimaria").prop("disabled",true)
    //    $("#TerSecundaria").prop("disabled",true)
    //} else if(this.value == "AYT"){
    //    $("#selectBT").prop("disabled",true)
    //    $("#BTPrimaria").prop("disabled",true)
    //    $("#BTSecundaria").prop("disabled",true)
    //}else if(this.value == "BYT"){
    //    $("#selectAT").prop("disabled",true)
    //    $("#ATPrimaria").prop("disabled",true)
    //    $("#ATSecundaria").prop("disabled",true)
    //}else{
    //    $("#selectTer").prop("disabled",true)
    //    $("#selectAT").prop("disabled",true)
    //    $("#selectBT").prop("disabled",true)
    //    $("#TerPrimaria").prop("disabled",true)
    //    $("#TerSecundaria").prop("disabled",true)
    //    $("#ATPrimaria").prop("disabled",true)
    //    $("#ATSecundaria").prop("disabled",true)
    //    $("#BTPrimaria").prop("disabled",true)
    //    $("#BTSecundaria").prop("disabled",true)

    //    $("#selectBT option[value!='']").each(function() {
    //        $(this).remove();
    //    });

    //    $("#selectAT option[value!='']").each(function() {
    //        $(this).remove();
    //    });

    //    $("#selectTer option[value!='']").each(function() {
    //        $(this).remove();
    //    });
    //}
});

$(".radio").click(function(){

    if(this.id == "ATPrimaria" || this.id == "ATSecundaria"){
        if($("#ATPrimaria").is(":checked") && $("#ATSecundaria").is(":checked")){
            ShowFailedMessage("No se puede establecer una posicion primaria y secundaria para AT");
            pasar =false;
            contRadio--;
            //$("#"+this.id).prop("checked" , false)
            
        }else{
            pasar =true;
            contRadio++;
        }
    }
    if(this.id == "BTPrimaria" || this.id == "BTSecundaria"){
        if($("#BTPrimaria").is(":checked") && $("#BTSecundaria").is(":checked")){
            ShowFailedMessage("No se puede establecer una posicion primaria y secundaria para BT");
            pasar =false;
            contRadio--;
            //$("#"+this.id).prop("checked" , false)
            
        }else{
            pasar =true;
            contRadio++;
        }
    }

    if(this.id == "TerPrimaria" || this.id == "TerSecundaria"){
        if($("#TerPrimaria").is(":checked") && $("#TerSecundaria").is(":checked")){
            ShowFailedMessage("No se puede establecer una posicion primaria y secundaria para Ter");
            pasar =false;
            contRadio--;
            //$("#"+this.id).prop("checked" , false)
            
        }else{
            pasar =true;
            contRadio++;
        }
    }

    //validar que si el aparato tiene las 3 , se desactive alguno cuando se seleccione 2
    if(pasar && contRadio === 2){
        if(!$("#ATPrimaria").is(":checked") && !$("#ATSecundaria").is(":checked")){
            $("#ATPrimaria").prop("disabled",true)
            $("#ATSecundaria").prop("disabled",true)
            $("#selectAT").prop("disabled",true)
        }

        if(!$("#BTPrimaria").is(":checked") && !$("#BTSecundaria").is(":checked")){
            $("#BTPrimaria").prop("disabled",true)
            $("#BTSecundaria").prop("disabled",true)
            $("#selectBT").prop("disabled",true)
        }

        if(!$("#TerPrimaria").is(":checked") && !$("#TerSecundaria").is(":checked")){
            $("#TerPrimaria").prop("disabled",true)
            $("#TerSecundaria").prop("disabled",true)
            $("#selectTer").prop("disabled",true)
        }
    }

});


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
    clavePruebaInput.value = ''
    voltajePruebaInput.value = ''



    btnRequest.disabled = false;
    btnClear.disabled = false;
    btnLoadTemplate.disabled = true;
    noSerieInput.disabled = false;

    $("#selectTer").prop("disabled",true)
    $("#selectAT").prop("disabled",true)
    $("#selectBT").prop("disabled",true)
    $("#TerPrimaria").prop("disabled",true)
    $("#TerSecundaria").prop("disabled",true)
    $("#ATPrimaria").prop("disabled",true)
    $("#ATSecundaria").prop("disabled",true)
    $("#BTPrimaria").prop("disabled",true)
    $("#BTSecundaria").prop("disabled",true)

    $("#ATSecundaria").prop("checked",false)
    $("#ATPrimaria").prop("checked",false)
    $("#BTSecundaria").prop("checked",false)
    $("#BTPrimaria").prop("checked",false)
    $("#TerSecundaria").prop("checked",false)
    $("#TerPrimaria").prop("checked",false)

    $("#selectBT option[value!='']").each(function() {
        $(this).remove();
    });

    $("#selectAT option[value!='']").each(function() {
        $(this).remove();
    });

    $("#selectTer option[value!='']").each(function() {
        $(this).remove();
    });

    $("#selectAT option[value='']").remove();
    $("#selectBT option[value='']").remove();
    $("#selectTer option[value='']").remove();

    $("#selectAT").append("<option value=''>Todas</option>");
    $("#selectBT").append("<option value=''>Todas</option>");
    $("#selectTer").append("<option value=''>Todas</option>");


    if (treeViewKendoElement !== undefined) {
        kendo.destroy(treeViewKendoElement);
        $("#treeview-kendo").empty();
    }
   
    voltajePruebaInput.value = 10
    statusAllPosSec = false
    contRadio = 0
});

btnLoadTemplate.addEventListener("click", function () {
    if (ValidateForm()) {

        MapToViewModel();
        validarPosicionesYselect()
        if(!pasar || !pasarSelect){
            return
        }
        btnRequest.disabled = true;
        btnLoadTemplate.disabled = true;
        btnClear.disabled = false;
        $("#loader").css("display", "block");

        ValidateFilterJSON(null).then(
            data => {
                if (data.response.Code !== -1) {
                    GetTemplateJSON();
                }
                else {
                    ShowFailedMessage(data.response.Description);
                }
            }
        );

    }
    else {
        ShowFailedMessage("Faltan campos por llenar")
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



                    console.log(data.response.Structure );
                    LoadTreeView(viewModel.TreeViewItem);
                    LoadForm(viewModel);

                    btnRequest.disabled = true;
                    btnClear.disabled = false;
                    btnLoadTemplate.disabled = false;
                   /* $("#selectTer").prop("disabled", false)
                    $("#selectAT").prop("disabled", false)
                    $("#selectBT").prop("disabled", false)
                    $("#TerPrimaria").prop("disabled", false)
                    $("#TerSecundaria").prop("disabled", false)
                    $("#ATPrimaria").prop("disabled", false)
                    $("#ATSecundaria").prop("disabled", false)
                    $("#BTPrimaria").prop("disabled", false)
                    $("#BTSecundaria").prop("disabled", false)*/
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

function validarPosicionesYselect(){
    if($("*[id*='Primaria']:checked").length == 0){
        arrPri =[]
        ShowFailedMessage('Debe seleccionar una posicion Primaria');
        return
    }
    if($("*[id*='Secundaria']:checked").length == 0){
        arrSec =[]
        ShowFailedMessage('Debe seleccionar una posicion Secundaria');
        return
    }


    letraPri = $("*[id*='Primaria']:checked")[0].id.replace("Primaria",'')
    letraSec = $("*[id*='Secundaria']:checked")[0].id.replace("Secundaria",'')


    arrPri = $("#select"+letraPri).val()
    arrSec = $("#select"+letraSec).val()


    if(arrPri == null){
        ShowFailedMessage("Debe seleccionar por lo menos una posicion primaria")
        arrPri =[]
        return
    }
    
    if(arrPri.length === 1){
        if(arrPri[0] === ''){
            ShowFailedMessage("Debe seleccionar por lo menos una posicion primaria")
            arrPri =[]
            return 
        }
    }else{
        ShowFailedMessage("Debe seleccionar por lo menos una posicion primaria")
        arrPri =[]
        return
    }

    if(arrSec[0] === ''){
        //arrSec = $("#select"+letraSec).val().filter(x=> x!='')
        if(letraSec === "AT"){
            arrSec = viewModel.Positions.AltaTension
        }
        if(letraSec === "BT"){
            arrSec = viewModel.Positions.BajaTension
        }
        if(letraSec === "Ter"){
            arrSec = viewModel.Positions.Terciario
        }
        if(arrSec.length > 33 ){
            ShowFailedMessage("Cantidad de posiciones en secundaria no permitidas, solo se permiten como máximo 33")
            arrSec =[]
            return
        }else if(arrSec.length === 0){
            ShowFailedMessage("Debe seleccionar por lo menos una posicion Secundaria")
            arrSec =[]
            return
        }
        statusAllPosSec = true
    }else{
        if(arrSec.length > 33){
            ShowFailedMessage("Cantidad de posiciones en secundaria no permitidas, solo se permiten como máximo 33")
            arrSec =[]
            return
        }

        if(letraSec==='AT'){
            if(viewModel.Positions.AltaTension.length === arrSec.length){
                statusAllPosSec =true 
            }
        }

        if(letraSec==='BT'){
            if(viewModel.Positions.BajaTension.length === arrSec.length){
                statusAllPosSec =true 
            }
        }

        if(letraSec==='Ter'){
            if(viewModel.Positions.Terciario.length === arrSec.length){
                statusAllPosSec =true 
            }
        }
    }
 
     pasarSelect = true 

}

//Requests

async function GetFilterJSON() {
        var path = path = "/Cem/GetFilter/";

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

async function ValidateFilterJSON() {
    var path = path = "/Cem/ValidateFilter/";

    var url = new URL(domain + path),
        params = {
            nroSerie: noSerieInput.value,
            keyTest: clavePruebaInput.value,
            lenguage: claveIdiomaInput.value,
            cantPosPri: arrPri.length,
            cantPosSec: arrSec.length,
            idPosPrimary: letraPri,
            idPosSecundary: letraSec,
            posicionesPrimarias: arrPri,
            posicionesSecundarias: arrSec,
            testsVoltage: voltajePruebaInput.value,
            comment: CommentsInput.value,
            statusAllPosSec: statusAllPosSec
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

async function GetTemplateJSON() {
    var path = path = "/Cem/GetTemplate/";
    var url = new URL(domain + path),
        params = { 
            nroSerie: noSerieInput.value,
            keyTest: clavePruebaInput.value,
            lenguage: claveIdiomaInput.value,
            cantPosPri: arrPri.length,
            cantPosSec: arrSec.length,
            idPosPrimary: letraPri,
            idPosSecundary: letraSec,
            posicionesPrimarias : arrPri,
            posicionesSecundarias: arrSec,
            testsVoltage: voltajePruebaInput.value,
            comment: CommentsInput.value,
            statusAllPosSec :  statusAllPosSec
            }

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
        select : onSelect,
        checkboxes: false,
        loadOnDemand: true
    });

}

function LoadForm(response) {
    claveIdiomaInput.value = response.ClaveIdioma;
    //noPruebaInput.value = response.NoPrueba;
    voltajePruebaInput.value = 10;
console.log(response.Positions)


  if(response.Positions.AltaTension.length > 0){
    $.each(response.Positions.AltaTension , function(i,val){
        $("#selectAT").append("<option value='"+val +"'>"+val+"</option>");
    }); 

    if(response.Positions.AltaTension.length==1){
        $("#selectAT option[value='']").remove();
        $("#selectAT").prop("selectedIndex",0);
    }

    $("#selectAT").prop("disabled",false)
    $("#ATPrimaria").prop("disabled",false)
    $("#ATSecundaria").prop("disabled",false)
  }
 
  if(response.Positions.BajaTension.length > 0){
        $.each(response.Positions.BajaTension , function(i,val){
            $("#selectBT").append("<option value='"+val +"'>"+val+"</option>");
       });

    if(response.Positions.BajaTension.length==1){
        $("#selectBT option[value='']").remove();
        $("#selectBT").prop("selectedIndex",0);
    }


    $("#selectBT").prop("disabled",false)
    $("#BTPrimaria").prop("disabled",false)
    $("#BTSecundaria").prop("disabled",false)

   }

   if(response.Positions.Terciario.length > 0){

    $.each(response.Positions.Terciario , function(i,val){
  		$("#selectTer").append("<option value='"+val +"'>"+val+"</option>");
    });

    if(response.Positions.Terciario.length==1){
        $("#selectTer option[value='']").remove();
        $("#selectTer").prop("selectedIndex",0);
    }
    
    $("#selectTer").prop("disabled",false)
    $("#TerPrimaria").prop("disabled",false)
    $("#TerSecundaria").prop("disabled",false)

   }

   

}
var ResultValidations;

function ValidateForm() {

    ResultValidations = $("#form_menu_sup").data("kendoValidator").validate();
    if (ResultValidations) {
        return true;
    }else {
        return false;
    }
}

function LoadPDF(nameFile, File) {    
    console.log(File)
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

    viewModel.CantidadPosicionPrimaria = arrPri.length;
    viewModel.CantidadPosicionSecundaria = arrSec.length;
    viewModel.PosicionPrimaria = letraPri;
    viewModel.PosicionSecundaria = letraSec;
    viewModel.ReigstrosPosicionesPrimarias = arrPri;
    viewModel.ReigstrosPosicionesSecundarias = arrSec;

    if (loadWorkbook)
        viewModel.Workbook.sheets = $("#spreadsheet").getKendoSpreadsheet().toJSON().sheets;
    if (loadOfficial)
        viewModel.OfficialWorkbook = officialWorkbook;
}

function onSelect(e) {
    var text = this.text(e.node);

    if (text.split('.').length > 1) {
        var id = text.split('.')[0].split('-')[2].split('_')[1];
        console.log("Selecting: " + this.text(e.node));
        console.log(id);

        GetPDFJSON(id, "CEM").then(
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
    var path = path = "/Cem/GetPDFReport/";

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
