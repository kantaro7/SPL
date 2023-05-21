

//Variables and initializations of components
var editor;
var banderaErrorGrids = false;

let resultValidations = true;

let viewModel;

let btnRequest = document.getElementById("btnRequest");
let btnSave = document.getElementById("btnSave");
let btnClear = document.getElementById("btnClear");

let newTensionModal = new bootstrap.Modal(document.getElementById('newTensionModal'))
let btnNewTensionYes = document.getElementById("btnNewTensionYes");
let btnNewTensionNo = document.getElementById("btnNewTensionNo");

let noSerieInput = document.getElementById("NoSerie");

let tableAt = $("#tableAt");
let tableBt = $("#tableBt");
let tableTer = $("#tableTer");

let divAT = document.getElementById("DivAT");
let divBT = document.getElementById("DivBT");
let divTER = document.getElementById("DivTer");

let positionATInput = document.getElementById("positionAT");
let nominalATInput = document.getElementById("nominalAT");
let positionBTInput = document.getElementById("positionBT");
let nominalBTInput = document.getElementById("nominalBT");
let positionTERInput = document.getElementById("positionTER");
let nominalTERInput = document.getElementById("nominalTER");

//Events

$("#NoSerie").focus();



$("#NoSerie").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $("#btnRequest").click();
    }
});

btnSave.addEventListener("click", function () {

    if (banderaErrorGrids)
        return;

    $("#loader").css("display", "block");
    ResultValidations = true;

    MapToViewModel();

    if (resultValidations) {
        postData(domain + "/PlateTension/Post/", viewModel)
            .then(data => {
                if (data.response.Code !== -1) {
                    $("#loader").css("display", "none");
                    ShowSuccessMessage("Guardado Exitoso")
                }
                else {
                    $("#loader").css("display", "none");
                    ShowFailedMessage(data.response.Description);
                }

               
            });
    } else {
        ShowFailedMessage("Hay tension(es) vacías, por favor verifique.")
        $("#loader").css("display", "none");
    }

 
});

btnClear.addEventListener("click", function () {
    ClearForm();
});

btnNewTensionYes.addEventListener("click", function () {

    $("#loader").css("display", "block");
    GetPlateTensionJSON(true).then(
        data => {
            if (data.response.Code !== -1) {

                btnSave.disabled = false;
                noSerieInput.disabled = true;

                viewModel = data.response.Structure;
                LoadForm();
            }
            else {
                ShowFailedMessage(data.response.Description);
            }
            $("#loader").css("display", "none");
        }
    );

});

btnNewTensionNo.addEventListener("click", function () {

    $("#loader").css("display", "block");
    GetPlateTensionJSON(false).then(
        data => {
            if (data.response.Code !== -1) {

                btnSave.disabled = false;
                noSerieInput.disabled = true;

                viewModel = data.response.Structure;
                LoadForm();

            }
            else {
                ShowFailedMessage(data.response.Description);
            }
            $("#loader").css("display", "none");
        }
    );

});

btnRequest.addEventListener("click", function () {
    $("#loader").css("display", "block");
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

        GetPlateTensionJSON(null).then(
            data => {
                if (data.response.Code !== -1 && data.response.Code !== 2) {
                    //artifactViewModel.map(data.response);

                    btnSave.disabled = false;
                    noSerieInput.disabled = true;

                    viewModel = data.response.Structure;
                    LoadForm();
                    $("#loader").css("display", "none");
                }
                else if (data.response.Code === 2) {
                    AskUpdateInfo();
                    $("#loader").css("display", "none");
                }
                else {
                    ShowFailedMessage(data.response.Description);
                    $("#loader").css("display", "none");
                }
    
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

async function GetPlateTensionJSON(isNewTensionResponse) {
    var path = path = "/PlateTension/Get/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value, newTensonResponse: isNewTensionResponse }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const plateTension = await response.json();
        return plateTension;
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

function AskUpdateInfo() {

    newTensionModal.show();

}

//Functions

function MapToViewModel() {
    resultValidations = true;
    viewModel.NoSerie = noSerieInput.value;
    //map table AT
    tableAt.data("kendoGrid").dataSource._pristineData.forEach(function (data, key) {
        if (data.tension !== '' && data.tension !== undefined && data.tension !== null) {
            viewModel.TableAT[key].TENSION = data.tension;
        }
        else {
            resultValidations = false;
        }

        if (viewModel.TableAT[key].Existente === null)
            viewModel.TableAT[key].Existente = false;
    });
    //map table BT
    tableBt.data("kendoGrid").dataSource._pristineData.forEach(function (data, key) {

        if (data.tension !== '' && data.tension !== undefined && data.tension !== null) {
            viewModel.TableBT[key].TENSION = data.tension;
        }
        else {
            resultValidations = false;
        }

        if (viewModel.TableBT[key].Existente === null)
            viewModel.TableBT[key].Existente = false;
    });
    //map table TER
    tableTer.data("kendoGrid").dataSource._pristineData.forEach(function (data, key) {

        if (data.tension !== '' && data.tension !== undefined && data.tension !== null) {
            viewModel.TableTER[key].TENSION = data.tension;
        }
        else {
            resultValidations = false;
        }

        if (viewModel.TableTER[key].Existente === null)
            viewModel.TableTER[key].Existente = false;
    });

}

function ClearForm() {

    noSerieInput.disabled = false;
    noSerieInput.value = '';
    btnSave.disabled = true;

    if (tableAt.data("kendoGrid")) {
        tableAt.data("kendoGrid").destroy();
        tableAt.empty();
    }
    if (tableBt.data("kendoGrid")) {
        tableBt.data("kendoGrid").destroy();
        tableBt.empty();
    }
    if (tableTer.data("kendoGrid")) {
        tableTer.data("kendoGrid").destroy();
        tableTer.empty();
    }

    divAT.style.display = "none";
    nominalATInput.value = '';
    positionATInput.value = '';
    divBT.style.display = "none";
    nominalBTInput.value = '';
    positionBTInput.value = '';
    divTER.style.display = "none";
    nominalTERInput.value = '';
    positionTERInput.value = '';
}

function LoadForm() {

    if (viewModel.TableAT !== null && CheckNominalValue(viewModel.TableAT)) {
        divAT.style.display = "block";
        nominalATInput.value = viewModel.PositionTapBaan.NominalAT;

        positionATInput.value = viewModel.LoadNewTension ? viewModel.PositionTapBaan.PositionAT :viewModel.Positions.PositionAT;
    }
    else {

        divAT.style.display = "none";
        nominalATInput.value = '';
        positionATInput.value = '';
    }

    if (viewModel.TableBT !== null && CheckNominalValue(viewModel.TableBT)) {

        divBT.style.display = "block";
        nominalBTInput.value = viewModel.PositionTapBaan.NominalBT;
        positionBTInput.value = viewModel.LoadNewTension ? viewModel.PositionTapBaan.PositionBT : viewModel.Positions.PositionBT;

    }
    else {

        divBT.style.display = "none";
        nominalBTInput.value = '';
        positionBTInput.value = '';

    }

    if (viewModel.TableTER !== null && CheckNominalValue(viewModel.TableTER)) {

        divTER.style.display = "block";
        nominalTERInput.value = viewModel.PositionTapBaan.NominalTER;
        positionTERInput.value = viewModel.LoadNewTension ? viewModel.PositionTapBaan.PositionTER : viewModel.Positions.PositionTER;

    }
    else {

        divTER.style.display = "none";
        nominalTERInput.value = '';
        positionTERInput.value = '';

    }

    LoadTables();

    ShowSuccessMessage("Consulta Exitosa.")
}

function CheckNominalValue(tableTer) {

    if (tableTer.length === 1) {
        if (!(parseFloat(tableTer[0].TENSION) > 0))
            return false;
        else
            return true;
    }
    else
        return true;
}

function LoadTables() {
    let dataSourceAT = [];
    let dataSourceBT = [];
    let dataSourceTER = [];

    viewModel.TableAT.forEach(function (data, key) {

        var tension = data.TENSION.replace(",", ".");
        dataSourceAT.push({ id: key, position: data.POSICION, tension: tension });
        
    });    
    
    tableAt.kendoGrid({
        dataSource: {
            data: dataSourceAT,
            schema: {
                model: {
                    id: "id",
                    fields: {
                        position: { editable: false },
                        tension: {
                            
                            format: "{0:n3}",
                            decimals: 3,
                            validation: {
                                required: true,
                                validateFunction: function (isInput) {
                                    return validateInput(isInput);
                                }
                            },
                            editable: true

                        }
                    }
                }
            },
            change: function (e) {
                if (e.action == "itemchange") {
                    this.sync();
                }
            }
            
        },

        scrollable: true,
        height: 300,
        columns: [{
            field: "position",
            title: "Posición"
        },
        {
            field: "tension",
            title: "Tensión",
           
       
        }],
        editable: true
        
    });


    var grid1 = tableAt.data("kendoGrid");

    grid1.table.on('keydown', function (e) {
        var currentNumberOfItems = grid1.dataSource.view().length;
        var row = $(e.target).closest('tr').index();
        var col = grid1.cellIndex($(e.target).closest('td'));

        var dataItem = grid1.dataItem($(e.target).closest('tr'));
        var field = grid1.columns[col].field;
        var value = $(e.target).val();
  
     

        if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {

            if (validateInput($(e.target)) == true) {
                e.preventDefault();

                dataItem.set(field, value);
                if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < grid1.columns.length) {
                    var nextCellRow = row;
                    var nextCellCol = col;

                    if (e.shiftKey) {
                        if (nextCellRow - 1 < 0) {
                            nextCellRow = currentNumberOfItems - 1;
                            nextCellCol--;
                        } else {
                            nextCellRow--;
                        }
                    } else {
                        if (nextCellRow + 1 >= currentNumberOfItems) {
                            nextCellRow = 0;
                            nextCellCol++;
                        } else {
                            nextCellRow++;
                        }
                    }

                    if (nextCellCol >= grid1.columns.length || nextCellCol < 0) {
                        return;
                    }



                    // wait for cell to close and Grid to rebind when changes have been made

                    setTimeout(function () {
                        grid1.editCell(grid1.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                    });


                }
            }
           
        }
     


  
        
    });






    function validateInput(input) {

        banderaErrorGrids = false;
        var tension = $(input).val();

        if (tension == "" || tension == null || tension == undefined) {
            banderaErrorGrids = true;
        }
      
        var result = tension.split(".");
        
        if (!(/^[0-9]+$/.test(result[0]))) {
            banderaErrorGrids = true;
            input.attr("data-validateFunction-msg", "Solo se permiten números");
            return false;
        }
        if (result.length > 1) {
            if (!(/^[0-9]+$/.test(result[1]))) {
                banderaErrorGrids = true;
                input.attr("data-validateFunction-msg", "Solo se permiten números");
                return false;
            }
        }
     

        if (tension > 99999999.999) {
            banderaErrorGrids = true;
            input.attr("data-validateFunction-msg", "El número no debe ser mayor a 99,999,999.999");
            return false;
        }

        if (result.length == 2) {
            if (result[1].length > 3) {

                banderaErrorGrids = true;
                input.attr("data-validateFunction-msg", "El número debe contener 3 o menos decimales");
                return false;
            }
        }
        else
        if (result.length > 2) {
            banderaErrorGrids = true;
            input.attr("data-validateFunction-msg", "Formato incorrecto");
            return false;
        }


        return true;
    }



    //<span class="k-numeric-wrap k-state-default">
    //    <input type="text" class="k-formatted-value k-input" title="tension" tabindex="0" role="spinbutton" autocomplete="off" aria-disabled="false" aria-valuenow="555" style="display: inline-block;">
    //    <input type="text" id="tension" name="tension" title="tension" data-type="number" data-bind="value:tension" data-role="numerictextbox" role="spinbutton" class="k-input" aria-disabled="false" style="display: block;" aria-valuenow="5555">
    //            <span class="k-icon k-i-warning" style="display: none;">
    //            </span>
    //            <span class="k-select">
    //                <span role="button" unselectable="on" class="k-link k-link-increase" aria-label="Incrementar valor" title="Incrementar valor">
    //                    <span unselectable="on" class="k-icon k-i-arrow-60-up">
    //                    </span>
    //                </span>
    //                <span role="button" unselectable="on" class="k-link k-link-decrease" aria-label="Disminuir valor" title="Disminuir valor">
    //                    <span unselectable="on" class="k-icon k-i-arrow-60-down">
    //                    </span>
    //                </span>
    //            </span>
    //            </span>

  
  
    viewModel.TableBT.forEach(function (data, key) {
        var tension = data.TENSION.replace(",", ".");
        dataSourceBT.push({ id: key, position: data.POSICION, tension: tension });

    });

    tableBt.kendoGrid({
        dataSource: {
            data: dataSourceBT,
            schema: {
                model: {
                    id: "id",
                    fields: {
                        position: { editable: false },
                        tension: {

                            format: "{0:n3}",
                            decimals: 3,
                            validation: {
                                required: true,
                                validateFunction: function (isInput) {
                                    return validateInput(isInput);
                                }
                            },
                            editable: true

                        }
                    }
                }
            },
            change: function (e) {
                if (e.action == "itemchange") {
                    this.sync();
                }
            }
        },
      
        scrollable: true,
        height: 300,
        columns: [{
            field: "position",
            title: "Posición"
        },
        {
            field: "tension",
            title: "Tensión"
        }],
        editable: true

    });

    var grid2 = tableBt.data("kendoGrid");
    grid2.table.on('keydown', function (e) {
        var currentNumberOfItems = grid2.dataSource.view().length;
        var row = $(e.target).closest('tr').index();
        var col = grid2.cellIndex($(e.target).closest('td'));

        var dataItem = grid2.dataItem($(e.target).closest('tr'));
        var field = grid2.columns[col].field;
        var value = $(e.target).val();



        if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {

            if (validateInput($(e.target)) == true) {
                e.preventDefault();

                dataItem.set(field, value);
                if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < grid2.columns.length) {
                    var nextCellRow = row;
                    var nextCellCol = col;

                    if (e.shiftKey) {
                        if (nextCellRow - 1 < 0) {
                            nextCellRow = currentNumberOfItems - 1;
                            nextCellCol--;
                        } else {
                            nextCellRow--;
                        }
                    } else {
                        if (nextCellRow + 1 >= currentNumberOfItems) {
                            nextCellRow = 0;
                            nextCellCol++;
                        } else {
                            nextCellRow++;
                        }
                    }

                    if (nextCellCol >= grid2.columns.length || nextCellCol < 0) {
                        return;
                    }



                    // wait for cell to close and Grid to rebind when changes have been made

                    setTimeout(function () {
                        grid2.editCell(grid2.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                    });


                }
            }

        }





    });


    viewModel.TableTER?.forEach(function (data, key) {
        var tension = data.TENSION.replace(",", ".");
        dataSourceTER.push({ id: key, position: data.POSICION, tension: tension });

    });

    tableTer.kendoGrid({
        dataSource: {
            data: dataSourceTER,
            schema: {
                model: {
                    id: "id",
                    fields: {
                        position: { editable: false },
                        tension: {

                            format: "{0:n3}",
                            decimals: 3,
                            validation: {
                                required: true,
                                validateFunction: function (isInput) {
                                    return validateInput(isInput);
                                }
                            },
                            editable: true

                        }
                    }
                }
            },
            change: function (e) {
                if (e.action == "itemchange") {
                    this.sync();
                }
            }
        },
 
        scrollable: true,
        height: 300,
        columns: [{
            field: "position",
            title: "Posición"
        },
        {
            field: "tension",
            title: "Tensión"
        }],
        editable: true

    });


    var grid3 = tableTer.data("kendoGrid");

    grid3.table.on('keydown', function (e) {
        var currentNumberOfItems = grid3.dataSource.view().length;
        var row = $(e.target).closest('tr').index();
        var col = grid3.cellIndex($(e.target).closest('td'));

        var dataItem = grid3.dataItem($(e.target).closest('tr'));
        var field = grid3.columns[col].field;
        var value = $(e.target).val();



        if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {

            if (validateInput($(e.target)) == true) {
                e.preventDefault();

                dataItem.set(field, value);
                if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < grid3.columns.length) {
                    var nextCellRow = row;
                    var nextCellCol = col;

                    if (e.shiftKey) {
                        if (nextCellRow - 1 < 0) {
                            nextCellRow = currentNumberOfItems - 1;
                            nextCellCol--;
                        } else {
                            nextCellRow--;
                        }
                    } else {
                        if (nextCellRow + 1 >= currentNumberOfItems) {
                            nextCellRow = 0;
                            nextCellCol++;
                        } else {
                            nextCellRow++;
                        }
                    }

                    if (nextCellCol >= grid3.columns.length || nextCellCol < 0) {
                        return;
                    }



                    // wait for cell to close and Grid to rebind when changes have been made

                    setTimeout(function () {
                        grid3.editCell(grid3.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                    });


                }
            }

        }





    });
}

  
