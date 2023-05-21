var banderaFileErrorExtension = false;
var banderaFileErrorPeso = false;
var pesoMaximo;
let modelFormData = new FormData();
var ConfigurationFiles = [];

let languageSelect = document.getElementById("Language");
let fileInput = document.getElementById("File");

let btnSave = document.getElementById("btnSave");
let btnClear = document.getElementById("btnClear");

//Events

btnClear.addEventListener("click", function () {

    languageSelect.value = '';
    fileInput.value = null;

});



//function
function MapFormToViewModel() {
    modelFormData = new FormData();

    modelFormData.append("Language", languageSelect.value);

    var fileUpload = $("#File").get(0);

    var files = fileUpload.files;

    if (files.length > 0)
        modelFormData.append("File", files[0]);
}


function ejecuteValidUrl() {

    $('#FileSpan').text("");
    $("#FileSpan").removeClass('k-hidden');

    banderaFileErrorExtension = false;
    banderaFileErrorPeso = false;
    var fileUpload = $("#File").get(0);

    var files = fileUpload.files;

    if (files.length > 0) {
        let nameFile = files[0].name;

        var extension ="."+ nameFile.substr((nameFile.lastIndexOf('.') + 1));

        GetConfigurationFilesJSON(9).then(
            data => {
                if (data.response.Structure !== null && data.response.Structure !== undefined) {
                    console.log(data.response.Structure);
                    ConfigurationFiles = data.response.Structure;
                    for (var i = 0; i < data.response.Structure.length; i++) {
                        if (extension == data.response.Structure[i].ExtensionArchivoNavigation.Extension) {
                            banderaFileErrorExtension = true;
                            if (files[0].size < (data.response.Structure[i].MaximoPeso * 1024 * 1024)) {
                                pesoMaximo = data.response.Structure[i].MaximoPeso * 1024 * 1024;
                                banderaFileErrorPeso = true;
                            }
                            break;
                        }    
                    }

                    if (!banderaFileErrorExtension) {
                          $("#FileSpan").removeClass('k-hidden');
                          $('#FileSpan').text("Extensión de archivo no permitida");
                    }else if (!banderaFileErrorPeso) {
                        $("#FileSpan").removeClass('k-hidden');
                        $('#FileSpan').text("El peso máximo para el tipo de archivo " + extension + " es: " + pesoMaximo);
                    }
                    


                }
            }
        );
    }

}

async function GetConfigurationFilesJSON(module) {

    var path = '/BaseTemplateConsolidatedReport/GetConfigurationFiles';

    var url = new URL(domain + path),
        params = {
            pIdModule: module
        }

    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const data = await response.json();
        return data;
    }

    else {

        return null;
    }


}
//Request


btnSave.addEventListener("click", function () {

    var ResultValidations = $("#form_base_template").data("kendoValidator").validate();

    if (ResultValidations) {
        if (!banderaFileErrorExtension) {
            $("#FileSpan").removeClass('k-hidden');
            $('#FileSpan').text("Extensión de archivo no permitida");
            return;
        } else if (!banderaFileErrorPeso) {
            $("#FileSpan").removeClass('k-hidden');
            $('#FileSpan').text("El peso máximo para el tipo de archivo " + extension + " es: " + pesoMaximo);
            return;
        }
        $("#loader").css("display", "block");

        MapFormToViewModel();
        $("#loader").css("display", "block");
        $.ajax({
            url:"/BaseTemplateConsolidatedReport/Save",
            method: "POST",
            contentType: false,
            processData: false,
            data: modelFormData,
            success: function (result) {

                if (result.response.Code === 1) {
                    ShowSuccessMessage("Guardado Exitoso.")
                }
                else {
                    ShowFailedMessage(result.response.Description);
                }
            }
        });
        $("#loader").css("display", "none");
      

    }
    else {

        $("#loader").css("display", "none");
        ShowFailedMessage("Faltan campos requeridos");

    }

});


