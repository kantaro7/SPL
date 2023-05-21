let viewModel;
let settingsToDisplayPRDReportsDTO;
let btnSave = document.getElementById("btnSave");
let btnValidate = document.getElementById("btnValidate");
let btnRefresh = document.getElementById("btnRefresh");
var baseTemplate;
let spreadsheetElement;
var calculado = false;
let panelExcel = document.getElementById("spreadsheet");


let maxFile = 5;
let configuraciones =[];
let idModule = 5
let errorFiles = false
let contador = 0
let error = false
let pasarAT = false
let pasarBT = false
let pasarTer = false

let devanadoEnergizado = false
let devanadoInducido = false

function TodoBien(view) {
    viewModel = view;
    baseTemplate = viewModel.Workbook;
    //console.log(viewModel)
    LoadTemplate(viewModel.Workbook);
    initCss();
    btnRefresh.disabled = false;
    btnSave.disabled = false;
    btnValidate.disabled = false;

}

btnRefresh.addEventListener("click", function () {
    if (calculado) {
        location.reload();
    } else {
        btnRefresh.disabled = false;
        btnSave.disabled = true;
        btnValidate.disabled = false;
        SetDataSourceSpredsheet(baseTemplate);
        initCss();
    }
});

 
async function test(elemento) {
    return new Promise(function(resolve, reject) {
        var reader = new FileReader();
        reader.onload = function() { resolve(reader.result);  };
        reader.onerror = reject;
        reader.readAsDataURL(elemento.rawFile);
    });
}

btnValidate.addEventListener("click", function () {
});

btnSave.addEventListener("click",async function () {

    var spreadsheet = $("#spreadsheet").data("kendoSpreadsheet");
    var sheet = spreadsheet.activeSheet();
    let resultado = sheet.range("O50").value()

    if(resultado.indexOf("Selec")>-1){
        ShowFailedMessage("Favor de seleccionar aceptado o rechazado");
        return
    }else{
        viewModel.Resultado = resultado
    }
    if (resultado === "Aceptado" || resultado === "Accepted") {
        viewModel.IsReportAproved = true;
    }
    else {
        viewModel.IsReportAproved = false;
    }
    if(pasarBT && pasarBT && pasarTer && devanadoInducido && devanadoEnergizado){
        LoadWorkbook()
        let notas =  (sheet.range("D49").value() !== null ? sheet.range("D49").value() : "" ) + " " + (sheet.range("D50").value() !== null ? sheet.range("D50").value() : "" )
        viewModel.Notas = notas
        $("#loader").css("display", "block");
        postData(domain + "/Tin/SavePDF/", viewModel)
            .then(data => {
                console.log(data.response)
                if (data.response.Code !== -1) {
                    ShowSuccessMessage("Guardado Exitoso.")
                    btnSave.disabled = true;
                    btnRefresh.disabled = false;
                    btnValidate.disabled = true;
                    LoadPDF(data.response.nameFile, data.response.file);
                    initCss();
                }
                else {
                    ShowFailedMessage(data.response.Description);
                }
                $("#loader").css("display", "none");
        });
    }else{
        ShowFailedMessage("Por favor corregir los errores de posiciones y/o devenados")
    }
  
});

//Requests
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

function LoadTemplate(workbook) {
    SetConfigExcel(workbook);
}
function initCss() {
    $(".k-tabstrip-items").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-button-icontext").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-spreadsheet-quick-access-toolbar").css({ "background": "#8854FF", "color": "#fff" });
    $(".k-link").css({ "color": "#fff" });
}

function SetDataSourceSpredsheet(workbook) {
    spreadsheetElement.data("kendoSpreadsheet").destroy()
    $("#spreadsheet").empty();
    SetConfigExcel(workbook);
}

function SetConfigExcel(workbook) {

    kendo.spreadsheet.defineFunction("VALIDATE_RESULT", function(str,idioma,flags){
        if(idioma === "ES" ){
            if (str === "Aceptado" || str === "Rechazado") {
                if (str === "Aceptado") {
                    viewModel.IsReportAproved = true;
                }
                if (str === "Rechazado") {
                    viewModel.IsReportAproved = false;
                }
                return true
            }else{
                return false
            }
        }else if(idioma === "EN"){
            if (str === "Accepted" || str === "Rejected") {
                if (str === "Accepted") {
                    viewModel.IsReportAproved = true;
                }
                if (str === "Rejected") {
                    viewModel.IsReportAproved = false;
                }

                return true
            }else{
                return false
            }
        }else{
            ShowFailedMessage("Error en el idioma")
        }

    
       
    }).args([
        [ "str", "string" ],
        [ "idioma", "string" ],
        [ "flags", [ "or", "string", "null" ] ]
    ]);

    kendo.spreadsheet.defineFunction("VALIDATE_RELTENSION", function(str, pattern, flags){
        var rx;
        var test1;
        var test2;
        rx = flags ? new RegExp(pattern, flags) : new RegExp(pattern);
        try {
            if(str.includes('-')){
                var sp = str.split('-')
                if(sp.length == 2 )
                {

                    test1 = rx.test(sp[0])
                    test2 = rx.test(sp[1])

                    if(test1 && test2){
                        return true;
                    }else{
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }else{
                test1 = rx.test(str)
                return test1
            }

            //rx = flags ? new RegExp(pattern, flags) : new RegExp(pattern);
        } catch(ex) {
            // could not compile regexp, return some error code
            return new kendo.spreadsheet.CalcError("REGEXP");
        }
        //alert(rx.test(str))
        //return rx.test(str);
    }).args([
        [ "str", "string" ],
        [ "pattern", "string" ],
        [ "flags", [ "or", "string", "null" ] ]
    ]);



    kendo.spreadsheet.defineFunction("REGEXP_MATCH", function(str, pattern, flags){
        var rx;
        try {
            rx = flags ? new RegExp(pattern, flags) : new RegExp(pattern);
        } catch(ex) {
            // could not compile regexp, return some error code
            return new kendo.spreadsheet.CalcError("REGEXP");
        }
        //alert(rx.test(str))
        return rx.test(str);
    }).args([
        [ "str", "string" ],
        [ "pattern", "string" ],
        [ "flags", [ "or", "string", "null" ] ]
    ]);


    kendo.spreadsheet.defineFunction("EXITS_IN_POSITIONS", function(str,lista,posicion,flags){
        var posi;
        try {
            if(str !== ''){
                posi = lista.split(",")
                if(posi.filter(x=>x == str)[0]!== undefined){

                    if(posicion === "AT"){
                        pasarAT = true;
                    }

                    if(posicion === "BT"){
                        pasarBT = true;
                    }

                    if(posicion === "Ter"){
                        pasarTer = true;
                    }

                    return true
                }else{
                    ShowFailedMessage("La posición " + str+ " no existe dentro de las posiciones de "+ posicion)

                    if(posicion === "AT"){
                        pasarAT = false;
                    }

                    if(posicion === "BT"){
                        pasarBT = false;
                    }

                    if(posicion === "Ter"){
                        pasarTer = false;
                    }

                    return true;
                }
            }else{
                
                if(posicion === "AT"){
                    pasarAT = false;
                }

                if(posicion === "BT"){
                    pasarBT = false;
                }

                if(posicion === "Ter"){
                    pasarTer = false;
                }
                return true;
            }
        } catch(ex) {
            // could not compile regexp, return some error code
            return ex;
        }
        //alert(rx.test(str))
    }).args([
        [ "str", "string" ],
        [ "lista", "string" ],
        [ "posicion", "string" ],
        [ "flags", [ "or", "string", "null" ] ]
    ]);

    kendo.spreadsheet.defineFunction("CHECK_DEVANADO_ENERGIZADO", function(str,idioma,flags){
        try {
            var cellDevanadoInducido =$("#spreadsheet").data("kendoSpreadsheet").sheets()[0].range("L16").values()[0][0];
            if(str !== '')
            {
               if(idioma === 'EN'){
                    if(str === "HV" || str === "LV"){
                        if(cellDevanadoInducido !== null){
                            if(str !==  cellDevanadoInducido){
                                devanadoEnergizado = true;
                                return true;  
                            }else{
                                devanadoEnergizado = false;
                                return false;  
                            }
                        }else{
                            devanadoEnergizado = true;
                            return true; 
                        }
                    }else{
                        devanadoEnergizado = false    
                        return false 
                    }
               }else if(idioma === 'ES'){
                   if(str === "AT" || str === "BT"){
                        if(cellDevanadoInducido !== null){
                            if(str !==  cellDevanadoInducido){
                                devanadoEnergizado = true;
                                return true;  
                            }else{
                                devanadoEnergizado = false;
                                return false;  
                            }
                        }else{
                            devanadoEnergizado = true;
                            return true; 
                        }
                    }else{
                        devanadoEnergizado = false 
                        return false    
                    }
               }
               else{
                    devanadoEnergizado = false    
                    return false 
               }
            }else
            {
                devanadoEnergizado = false
                return false
            }
        } catch(ex) {
            // could not compile regexp, return some error code
            return ex;
        }
        //alert(rx.test(str))
    }).args([
        [ "str", "string" ],
        [ "idioma", "string" ],
        [ "flags", [ "or", "string", "null" ] ]
    ]);

    kendo.spreadsheet.defineFunction("CHECK_DEVANADO_INDUCIDO", function(str,idioma,flags){
        try {
            var cellDevanadoEnergizado =$("#spreadsheet").data("kendoSpreadsheet").sheets()[0].range("F16").values()[0][0];
            if(str !== '')
            {
               if(idioma === 'EN'){
                    if(str === "HV" || str === "LV"){
                        if(cellDevanadoEnergizado !== null){
                            if(str !==  cellDevanadoEnergizado){
                                devanadoInducido = true;
                                return true;  
                            }else{
                                devanadoInducido = false;
                                return false;  
                            }
                        }else{
                            devanadoInducido = true;
                            return true; 
                        }
                    }else{
                        devanadoInducido = false    
                        return false 
                    }
               }else if(idioma === 'ES'){
                   if(str === "AT" || str === "BT"){
                        if(cellDevanadoEnergizado !== null){
                            if(str !==  cellDevanadoEnergizado){
                                devanadoInducido = true;
                                return true;  
                            }else{
                                devanadoInducido = false;
                                return false;  
                            }
                        }else{
                            devanadoInducido = true;
                            return true; 
                        }
                    }else{
                        devanadoInducido = false 
                        return false    
                    }
               }
               else{
                    devanadoInducido = false    
                    return false 
               }
            }else
            {
                devanadoInducido = false
                return false
            }
        } catch(ex) {
            // could not compile regexp, return some error code
            return ex;
        }
        //alert(rx.test(str))
    }).args([
        [ "str", "string" ],
        [ "idioma", "string" ],
        [ "flags", [ "or", "string", "null" ] ]
    ]);

    kendo.spreadsheet.defineFunction("CHECK_TIME", function(str, pattern, flags){
        var rx;
        try {
            rx = flags ? new RegExp(pattern, flags) : new RegExp(pattern);
        } catch(ex) {
            // could not compile regexp, return some error code
            return new kendo.spreadsheet.CalcError("REGEXP");
        }
        //alert(rx.test(str))
        return rx.test(str);
    }).args([
        [ "str", "string" ],
        [ "pattern", "string" ],
        [ "flags", [ "or", "string", "null" ] ]
    ]);


    spreadsheetElement = $("#spreadsheet").kendoSpreadsheet({
        sheets: workbook.sheets,
        toolbar: {

            backgroundColor: "#3f51b5 !important",
            textColor: "#fff",
            home: [

                {
                    type: "button",
                    text: "Guardar",
                    showText: "both",
                    icon: "k-icon k-i-save",

                    click: function () {
                        btnSave.click();
                        

                    }
                },
                {
                    type: "button",
                    text: "Reiniciar",
                    showText: "both",
                    icon: "k-icon k-i-refresh",

                    click: function () {
                        btnRefresh.click();
                    }
                },
                {
                    type: "button",
                    text: "No Prueba: " + viewModel.NoPrueba,
                    showText: "both",
                    attributes: { "style": "color:#8854FF;pointer-events:none;border:none;background:none;" }
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
    var range = spreadsheet.activeSheet().range("A65:J200");
    range.enable(false);

    // validates if the formula returns a non-false value (see the `from` field).

    sheet.range("O50").validation({
        comparerType: "list",
        dataType: "list",
        from: viewModel.ClaveIdioma == 'ES' ? '"Aceptado,Rechazado"' : '"Accepted,Rejected"',
        titleTemplate : "Error",
        showButton : true,
        type : "reject",
        allowNulls : false
    });
    sheet.range("O50:Q50").enable(true);
    sheet.range("O50:Q50").value(viewModel.ClaveIdioma == 'ES' ? 'Seleccione...' : 'Select...');
    sheet.range("D49:K49").enable(true);
    sheet.range("D50:K50").enable(true);

    sheet.range(viewModel.Celdas.FrecuenciaPrueba).validation({
        comparerType: "custom",
        dataType: "custom",
        from: 'AND(REGEXP_MATCH('+viewModel.Celdas.FrecuenciaPrueba+', "^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,3})?%?$"))',
        titleTemplate : "Error",
        showButton : true,
        type : "reject",
        allowNulls : false,
        messageTemplate : "La frecuencia de prueba debe ser numérica mayor a cero considerando 6 enteros con 3 decimales."
    });


    sheet.range(viewModel.Celdas.RelTension).validation({
        comparerType: "custom",
        dataType: "custom",
        from: 'AND(VALIDATE_RELTENSION('+viewModel.Celdas.RelTension+', "^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,3})?%?$"))',
        titleTemplate : "Error",
        showButton : true,
        type : "reject",
        allowNulls : false,
        messageTemplate : "La relación de tensión debe estar en el formato 999.99-999.999"
    });

    sheet.range(viewModel.Celdas.CeldaTiempo).validation({
        comparerType: "custom",
        dataType: "custom",
        from: 'AND(CHECK_TIME('+viewModel.Celdas.CeldaTiempo+', "^[1-9][0-9]*$"),LEN('+viewModel.Celdas.CeldaTiempo+')<=3)',
        titleTemplate : "Error",
        showButton : true,
        type : "reject",
        allowNulls : false,
        messageTemplate : "El tiempo debe ser numérico mayor a cero considerando 3 enteros sin decimales"
    });

    if(viewModel.Positions.ATNom !== null && viewModel.Positions.ATNom !== '' ){
        if(viewModel.Celdas.CeldaAT !== undefined && viewModel.Celdas.CeldaAT !== null && viewModel.Celdas.CeldaAT !== ''){
            //  alert(viewModel.Celdas.CeldaAT)
              sheet.range(viewModel.Celdas.CeldaAT).validation({
                  comparerType: "custom",
                  dataType: "custom",
                  from: 'AND(REGEXP_MATCH('+viewModel.Celdas.CeldaAT+', "^[a-zA-Z0-9]*$"),LEN('+viewModel.Celdas.CeldaAT+')<=5,EXITS_IN_POSITIONS('+viewModel.Celdas.CeldaAT+',"'+viewModel.Positions.AltaTension.join()+'","AT"))',
                  titleTemplate : "Error",
                  showButton : true,
                  type : "reject",
                  allowNulls : false,
                  messageTemplate : "La posición en AT solo puede contener letras y/o números y no puede excederse de 5 caracteres."
              });
        }
    }else{
        sheet.range(viewModel.Celdas.CeldaAT).enable(false)
        pasarAT = true
    }
   
    if(viewModel.Positions.BTNom !== null && viewModel.Positions.BTNom !== '' ){
        if(viewModel.Celdas.CeldaBT !== undefined && viewModel.Celdas.CeldaBT !== null && viewModel.Celdas.CeldaBT !== ''){
            //alert(viewModel.Celdas.CeldaBT)
            sheet.range(viewModel.Celdas.CeldaBT).validation({
                comparerType: "custom",
                dataType: "custom",
                from: 'AND(REGEXP_MATCH('+viewModel.Celdas.CeldaBT+', "^[a-zA-Z0-9]*$"),LEN('+viewModel.Celdas.CeldaBT+')<=5,EXITS_IN_POSITIONS('+viewModel.Celdas.CeldaBT+',"'+viewModel.Positions.BajaTension.join()+'","BT"))',
                titleTemplate : "Error",
                showButton : true,
                type : "reject",
                allowNulls : false,
                messageTemplate : "La posición en BT solo puede contener letras y/o números y no puede excederse de 5 caracteres."
            });
        }
    }else{
        sheet.range(viewModel.Celdas.CeldaBT).enable(false)
        pasarBT = true
    }
  
    if(viewModel.Positions.TerNom !== null && viewModel.Positions.TerNom !== '' ){
        if(viewModel.Celdas.CeldaTer !== undefined && viewModel.Celdas.CeldaTer !== null && viewModel.Celdas.CeldaTer !== ''){
        // alert(viewModel.Celdas.CeldaTer)
            sheet.range(viewModel.Celdas.CeldaTer).validation({
                comparerType: "custom",
                dataType: "custom",
                from: 'AND(REGEXP_MATCH('+viewModel.Celdas.CeldaTer+', "^[a-zA-Z0-9]*$"),LEN('+viewModel.Celdas.CeldaTer+')<=5,EXITS_IN_POSITIONS('+viewModel.Celdas.CeldaTer+',"'+viewModel.Positions.Terciario.join()+'","Ter"))',
                titleTemplate : "Error",
                showButton : true,
                type : "reject",
                allowNulls : false,
                messageTemplate : "La posición en Ter solo puede contener letras y/o números y no puede excederse de 5 caracteres."
            });
        
        }
    }else{
        sheet.range(viewModel.Celdas.CeldaTer).enable(false)
        pasarTer = true
    }

    if(viewModel.Celdas.CeldaDevanadoEnergizado !== undefined && viewModel.Celdas.CeldaDevanadoEnergizado !== null && viewModel.Celdas.CeldaDevanadoEnergizado !== ''){
        // alert(viewModel.Celdas.CeldaTer)
        var message;
        if(viewModel.ClaveIdioma === "ES")
        {
            message = "El devanado energizado no puede excederse de 3 caracteres, debe ser AT o BT y no puede ser igual al devanado inducido";
        }
        else
        {
            message = "El devanado energizado no puede excederse de 3 caracteres, debe ser LV o HV y no puede ser igual al devanado inducido";
        }

         sheet.range(viewModel.Celdas.CeldaDevanadoEnergizado).validation({
             comparerType: "custom",
             dataType: "custom",
             from: 'AND(CHECK_DEVANADO_ENERGIZADO('+viewModel.Celdas.CeldaDevanadoEnergizado+',"'+viewModel.ClaveIdioma+'")',
             titleTemplate : "Error",
             showButton : true,
             type : "reject",
             allowNulls : false,
             messageTemplate : message
         });
     }

    if(viewModel.Celdas.CeldaDevanadoInducido !== undefined && viewModel.Celdas.CeldaDevanadoInducido !== null && viewModel.Celdas.CeldaDevanadoInducido !== ''){
        // alert(viewModel.Celdas.CeldaTer)
        var message;
        if(viewModel.ClaveIdioma === "ES")
        {
            message = "El devanado inducido no puede excederse de 3 caracteres, debe ser AT o BT y no puede ser igual al devanado energizado";
        }
        else
        {
            message = "El devanado inducido no puede excederse de 3 caracteres, debe ser LV o HV y no puede ser igual al devanado energizado";
        }

         sheet.range(viewModel.Celdas.CeldaDevanadoInducido).validation({
             comparerType: "custom",
             dataType: "custom",
             from: 'AND(CHECK_DEVANADO_INDUCIDO('+viewModel.Celdas.CeldaDevanadoInducido+',"'+viewModel.ClaveIdioma+'")',
             titleTemplate : "Error",
             showButton : true,
             type : "reject",
             allowNulls : false,
             messageTemplate : message
         });
     }
 

    var drawing = kendo.spreadsheet.Drawing.fromJSON({
        topLeftCell: "A1",
        offsetX: 0,
        offsetY: 0,
        width: 220,
        height: 43,
        image: spreadsheet.addImage("/images/prolecge_excel.jpg")
    });

    sheet.addDrawing(drawing);
    initCss();
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
    window.close();
}

function showPdfInNewTab(base64Data, fileName) {

    let pdfWindow = window.open("");
    pdfWindow.document.write("<html<head><title>" + fileName + "</title><style>body{margin: 0px;}iframe{border-width: 0px;}</style></head>");
    pdfWindow.document.write("<body><embed width='100%' height='100%' src='data:application/pdf;base64, " + encodeURI(base64Data) + "#toolbar=0&navpanes=0&scrollbar=0'></embed></body></html>");
}
function LoadWorkbook(loadWorkbook = false, loadOfficial = false) {
    viewModel.Workbook.sheets = $("#spreadsheet").getKendoSpreadsheet().toJSON().sheets;
}

function onSelectFiles(e) {

    var totalFiles = e.sender.getFiles();
    if (totalFiles.length + 1 > 5 ) {

        ShowFailedMessage("Solo se permiten 5 archivos");
        e.preventDefault();
    }


    console.log(configuraciones)
    var files = e.files;

    for (var i = 0; i < files.length; i += 1) {
        var file = files[i];

        var result =  configuraciones.filter(x=>x.Extension.toUpperCase() == file.extension.toUpperCase())

        if(result.length === 0){
            file.error ="La extension "+file.extension + " no ha sido encontrada en la configuracion"
        }else if(result.length > 0){
            var pesoMax = result[0].PesoMaximo ;
            console.log(file.size)
            console.log(pesoMax )
            if(pesoMax * 1024  * 1024 < file.size){
                file.error ="El peso del archivo supera los "+result[0].PesoMaximo+"MB"
            }else{
                file.success ="Archivo agregado"
                setTimeout(function(){
                 $(".k-file-success div.uno span#wrapper-message").css("margin-left","35px")
                })
            }
        }
    }
}

async function onClickSelect(){
   await GetConfigurationFilesJSON().then(
        data => {
            if(data.response.Code === 1){
                configuraciones = data.response.Structure;
                console.log(configuraciones)
            }else{
                ShowFailedMessage("Error al obtener la confiuracion del archivo")
            }
        }
    )
}

async function GetConfigurationFilesJSON() {

    var path = path = "/Pir/GetConfigurationFiles/?pIdModule="+idModule;
    var url = new URL(domain + path)

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

