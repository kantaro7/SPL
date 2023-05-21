

//Variables and initializations of components
var editor;
var banderaErrorGrids = false;
var errorEnTablas = false;

let resultValidations = true;
let requestInicial = true;

let viewModel;

let btnRequest = document.getElementById("btnRequest");
let btnSave = document.getElementById("btnSave");
let btnClear = document.getElementById("btnClear");

let newTensionModal = new bootstrap.Modal(document.getElementById('newTensionModal'))
let btnNewTensionYes = document.getElementById("btnNewTensionYes");
let btnNewTensionNo = document.getElementById("btnNewTensionNo");

let noSerieInput = document.getElementById("NoSerie");
let temperatureInput = document.getElementById("Temperature");
let unitMeasuringInput = document.getElementById("UnitMeasuring");

let tableAt = $("#tableAt");
let tableBt = $("#tableBt");
let tableTer = $("#tableTer");

/********************* */
/********************* */
/********************* */
/********************* */
/********************* */
let tableAtHX = $("#tableAtHX");
let tableAtHN = $("#tableAtHN");
let tableAtHH = $("#tableAtHH");
let tableBtXN = $("#tableBtXN");
let tableBtXX = $("#tableBtXX");
let tableTerYY = $("#tableTerYY");
let tableTerYN = $("#tableTerYN");

let tableAtLL = $("#tableAtLL");
let tableAtLN = $("#tableAtLN");

let tableBtLL = $("#tableBtLL");
let tableBtLN = $("#tableBtLN");


let tableTerLL = $("#tableTerLL");
let tableTerLN = $("#tableTerLN");


let errorHX = false;
let errorHH = false;
let errorHN = false;
let errorXX = false;
let errorXN = false;
let errorYY = false;
let errorYN = false;

let errorLLAT = false;
let errorLLBT = false;
let errorLLTer = false;
let errorLNAT = false;
let errorLNBT = false;
let errorLNTer = false;



let html = `<div id="tabstrip">
                            <ul>
                                <li id="liHX">
                                    H-X
                                </li>
                                <li id="liHN">
                                    H-N
                                </li>
                                <li id="liHH">
                                    H-H
                                </li>
                                <li id="liXX">
                                    X-X
                                </li>
                                    <li id="liXN">
                                    X-N
                                </li>
                                <li id="liYY">
                                    Y-Y
                                </li >
                                <li id="liYN">
                                    Y-N
                                </li>
                              
                                 <li id="liLN">
                                    L-L
                                </li>
                                 <li id="liLL">
                                    L-N
                                </li>
                            </ul>

                            <div class="row">
                                     <div id="divAtHX" class="col-md-4 text-center" style="">
                                            <label class="form-label"><strong>AT</strong></label>
                                            <div class="col-md-12">
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="positionATHX" class="form-control form-control-sm" placeholder="No. Posiciones" disabled>
                                                        <label class="form-label">No. Posiciones</label>
                                                        <span id="positionATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="nominalATHX" class="form-control form-control-sm" placeholder="Nominal" disabled>
                                                        <label class="form-label">Nominal</label>
                                                        <span id="nominalATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div>
                                                    <div id="tableAtHX"></div>
                                                </div>
                                            </div>
                                    </div>
                            </div>

                            <div class="row">
                                  <div id="divAtHN" class="col-md-4 text-center" style="">
                                            <label class="form-label"><strong>AT</strong></label>
                                            <div class="col-md-12">
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="positionATHN" class="form-control form-control-sm" placeholder="No. Posiciones" disabled>
                                                        <label class="form-label">No. Posiciones</label>
                                                        <span id="positionATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="nominalATHN" class="form-control form-control-sm" placeholder="Nominal" disabled>
                                                        <label class="form-label">Nominal</label>
                                                        <span id="nominalATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div>
                                                    <div id="tableAtHN"></div>
                                                </div>
                                            </div>
                                    </div>
                            </div>

                            <div class="row">
                                   <div id="divAtHH" class="col-md-4 text-center" style="">
                                            <label class="form-label"><strong>AT</strong></label>
                                            <div class="col-md-12">
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="positionATHH" class="form-control form-control-sm" placeholder="No. Posiciones" disabled>
                                                        <label class="form-label">No. Posiciones</label>
                                                        <span id="positionATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="nominalATHH" class="form-control form-control-sm" placeholder="Nominal" disabled>
                                                        <label class="form-label">Nominal</label>
                                                        <span id="nominalATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div>
                                                    <div id="tableAtHH"></div>
                                                </div>
                                            </div>
                                    </div>
                            </div>

                             <div class="row">
                                <div id="divBtXX" class="col-md-4 text-center" style="">
                                            <label class="form-label"><strong>BT</strong></label>
                                           <div class="col-md-12">
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="positionBTXX" class="form-control form-control-sm" placeholder="No. Posiciones" disabled>
                                                        <label class="form-label">No. Posiciones</label>
                                                        <span id="positionATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="nominalBTXX" class="form-control form-control-sm" placeholder="Nominal" disabled>
                                                        <label class="form-label">Nominal</label>
                                                        <span id="nominalATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div>
                                                    <div id="tableBtXX"></div>
                                                </div>
                                            </div>
                                    </div>
                            </div>

                            <div class="row">
                                <div id="divBtXN" class="col-md-4 text-center" style="">
                                            <label class="form-label"><strong>BT</strong></label>
                                            <div class="col-md-12">
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="positionBTXN" class="form-control form-control-sm" placeholder="No. Posiciones" disabled>
                                                        <label class="form-label">No. Posiciones</label>
                                                        <span id="positionATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="nominalBTXN" class="form-control form-control-sm" placeholder="Nominal" disabled>
                                                        <label class="form-label">Nominal</label>
                                                        <span id="nominalATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div>
                                                    <div id="tableBtXN"></div>
                                                </div>
                                            </div>
                                    </div>
                            </div>

                          

                            <div class="row">
                                <div id="divTerYY" class="col-md-4 text-center" style="">
                                            <label class="form-label"><strong>Ter</strong></label>
                                            <div class="col-md-12">
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="positionTERYY" class="form-control form-control-sm" placeholder="No. Posiciones" disabled>
                                                        <label class="form-label">No. Posiciones</label>
                                                        <span id="positionATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="nominalTERYY" class="form-control form-control-sm" placeholder="Nominal" disabled>
                                                        <label class="form-label">Nominal</label>
                                                        <span id="nominalATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-12">
                                                <div>
                                                    <div id="tableTerYY"></div>
                                                </div>
                                            </div>
                                    </div>
                            </div>

                            <div class="row">
                                <div id="divTerYN" class="col-md-4 text-center" style="">
                                            <label class="form-label"><strong>Ter</strong></label>
                                            <div class="col-md-12">
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="positionTERYN" class="form-control form-control-sm" placeholder="No. Posiciones" disabled>
                                                        <label class="form-label">No. Posiciones</label>
                                                        <span id="positionATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="nominalTERYN" class="form-control form-control-sm" placeholder="Nominal" disabled>
                                                        <label class="form-label">Nominal</label>
                                                        <span id="nominalATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div>
                                                    <div id="tableTerYN"></div>
                                                </div>
                                            </div>
                                    </div>
                            </div>

                            <div class="row">
                                 <div id="divLL-AT" class="col-md-4" style="">
                                                <label class="form-label"><strong>AT</strong></label>
                                                <div class="col-md-12">
                                                    <div class="input-group-sm mb-3">
                                                        <div class="form-floating">
                                                            <input maxlength="150" id="positionATLL" class="form-control form-control-sm" placeholder="No. Posiciones" disabled>
                                                            <label class="form-label">No. Posiciones</label>
                                                            <span id="positionATSpan" class="text-danger"></span>
                                                        </div>
                                                    </div>
                                                    <div class="input-group-sm mb-3">
                                                        <div class="form-floating">
                                                            <input maxlength="150" id="nominalATLL" class="form-control form-control-sm" placeholder="Nominal" disabled>
                                                            <label class="form-label">Nominal</label>
                                                            <span id="nominalATSpan" class="text-danger"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div>
                                                        <div id="tableAtLL"></div>
                                                    </div>
                                                </div>
                                   </div>

                                    <div id="divLL-BT" class="col-md-4" style="">
                                            <label class="form-label"><strong>BT</strong></label>
                                            <div class="col-md-12">
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="positionBTLL" class="form-control form-control-sm" placeholder="No. Posiciones" disabled>
                                                        <label class="form-label">No. Posiciones</label>
                                                        <span id="positionATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="nominalBTLL" class="form-control form-control-sm" placeholder="Nominal" disabled>
                                                        <label class="form-label">Nominal</label>
                                                        <span id="nominalATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div>
                                                    <div id="tableBtLL"></div>
                                                </div>
                                            </div>
                                    </div>

                                    <div id="divLL-Ter" class="col-md-4" style="">
                                            <label class="form-label"><strong>Ter</strong></label>
                                            <div class="col-md-12">
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="positionTERLL" class="form-control form-control-sm" placeholder="No. Posiciones" disabled>
                                                        <label class="form-label">No. Posiciones</label>
                                                        <span id="positionATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="nominalTERLL" class="form-control form-control-sm" placeholder="Nominal" disabled>
                                                        <label class="form-label">Nominal</label>
                                                        <span id="nominalATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div>
                                                    <div id="tableTerLL"></div>
                                                </div>
                                            </div>
                                    </div>
                            </div>

                            <div class="row">
                                 <div id="divLN-AT" class="col-md-4" style="">
                                                <label class="form-label"><strong>AT</strong></label>
                                                <div class="col-md-12">
                                                    <div class="input-group-sm mb-3">
                                                        <div class="form-floating">
                                                            <input maxlength="150" id="positionATLN" class="form-control form-control-sm" placeholder="No. Posiciones" disabled>
                                                            <label class="form-label">No. Posiciones</label>
                                                            <span id="positionATSpan" class="text-danger"></span>
                                                        </div>
                                                    </div>
                                                    <div class="input-group-sm mb-3">
                                                        <div class="form-floating">
                                                            <input maxlength="150" id="nominalATLN" class="form-control form-control-sm" placeholder="Nominal" disabled>
                                                            <label class="form-label">Nominal</label>
                                                            <span id="nominalATSpan" class="text-danger"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div>
                                                        <div id="tableAtLN"></div>
                                                    </div>
                                                </div>
                                   </div>

                                    <div id="divLN-BT" class="col-md-4" style="">
                                            <label class="form-label"><strong>BT</strong></label>
                                            <div class="col-md-12">
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="positionBTLN" class="form-control form-control-sm" placeholder="No. Posiciones" disabled>
                                                        <label class="form-label">No. Posiciones</label>
                                                        <span id="positionATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="nominalBTLN" class="form-control form-control-sm" placeholder="Nominal" disabled>
                                                        <label class="form-label">Nominal</label>
                                                        <span id="nominalATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div>
                                                    <div id="tableBtLN"></div>
                                                </div>
                                            </div>
                                    </div>

                                    <div id="divLN-Ter" class="col-md-4" style="">
                                            <label class="form-label"><strong>Ter</strong></label>
                                            <div class="col-md-12">
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="positionTERLN" class="form-control form-control-sm" placeholder="No. Posiciones" disabled>
                                                        <label class="form-label">No. Posiciones</label>
                                                        <span id="positionATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                                <div class="input-group-sm mb-3">
                                                    <div class="form-floating">
                                                        <input maxlength="150" id="nominalTERLN" class="form-control form-control-sm" placeholder="Nominal" disabled>
                                                        <label class="form-label">Nominal</label>
                                                        <span id="nominalATSpan" class="text-danger"></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div>
                                                    <div id="tableTerLN"></div>
                                                </div>
                                            </div>
                                    </div>
                            </div>

             </div>`

/********************* */
/********************* */
/********************* */
/********************* */
/********************* */



let divAT = document.getElementById("DivAT");
let divBT = document.getElementById("DivBT");
let divTER = document.getElementById("DivTer");
let divTables = document.getElementById("titleTables");

let positionATInput = document.getElementById("positionAT");
let nominalATInput = document.getElementById("nominalAT");
let positionBTInput = document.getElementById("positionBT");
let nominalBTInput = document.getElementById("nominalBT");
let positionTERInput = document.getElementById("positionTER");
let nominalTERInput = document.getElementById("nominalTER");

const PATTERN = /^[1-9]\d{0,2}(?:\.\d{0,1})?$/;

//Events

$("#NoSerie").focus();


$("#NoSerie").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $("#btnRequest").click();
    }
});

btnSave.addEventListener("click", function () {
    var tabStrip = $("#tabstrip").kendoTabStrip().data("kendoTabStrip");
    var arregloData = []

    var errore = false

    if (viewModel.ResistDesignHX.length > 0) {
        var arrayHX =[]

        $("#tableAtHX").data("kendoGrid").dataSource.view().forEach(function (item) {
            //get the value of name
            arrayHX.push(item.resistance.toString())

            arregloData.push({
                Posicion: item.position,
                IdSeccion: 'AT',
                ConexionPrueba: 'H-X',
                NoSerie: noSerieInput.value,
                Orden:item.id+1,
                Resistencia: parseFloat(item.resistance.toString()),
                Temperatura: temperatureInput.value,
                UnidadMedida: unitMeasuringInput.value === "0" ? "Ohms" : "Miliohms"
            })
        })

        var infoHX = arrayHX.filter(c => c !== '0');

        if (viewModel.ResistDesignHX.length === infoHX.length) {//Ok
        }
        else if (infoHX.length === 0) // Ok{
        {
        }
        else if (viewModel.ResistDesignHX.length !== infoHX.length) {
            tabStrip.select(0);
            ShowFailedMessage("Debe introducir valores diferentes a 0 en todas las posiciones"); errore = true; return;

        }

        viewModel.ResistDesignHX = arregloData
    }

    if (viewModel.ResistDesignHN.length > 0) {
        var arrayHN = []
        arregloData= []
        $("#tableAtHN").data("kendoGrid").dataSource.view().forEach(function (item) {
            //get the value of name
            arrayHN.push(item.resistance.toString())

            arregloData.push({
                Posicion: item.position,
                IdSeccion: 'AT',
                ConexionPrueba: 'H-N',
                NoSerie: noSerieInput.value,
                Orden: item.id + 1,
                Resistencia: parseFloat(item.resistance.toString()),
                Temperatura: temperatureInput.value,
                UnidadMedida: unitMeasuringInput.value === "0" ? "Ohms" : "Miliohms"
            })
        })

        var infoHN = arrayHN.filter(c => c !== '0');

        if (viewModel.ResistDesignHN.length === infoHN.length) {//Ok
        }
        else if (infoHN.length === 0) // Ok{
        {
        }
        else if (viewModel.ResistDesignHN.length !== infoHN.length) {
            tabStrip.select(1);
            ShowFailedMessage("Debe introducir valores diferentes a 0 en todas las posiciones"); errore = true; return;


        }

        viewModel.ResistDesignHN = arregloData
    }

    if (viewModel.ResistDesignHH.length > 0) {
        var arrayHH = []
        arregloData = []
        $("#tableAtHH").data("kendoGrid").dataSource.view().forEach(function (item) {
            //get the value of name
            arrayHH.push(item.resistance.toString())

            arregloData.push({
                Posicion: item.position,
                IdSeccion: 'AT',
                ConexionPrueba: 'H-H',
                NoSerie: noSerieInput.value,
                Orden: item.id + 1,
                Resistencia: parseFloat(item.resistance.toString()),
                Temperatura: temperatureInput.value,
                UnidadMedida: unitMeasuringInput.value === "0" ? "Ohms" : "Miliohms"
            })
        })

        var infoHH = arrayHH.filter(c => c !== '0');

        if (viewModel.ResistDesignHH.length === infoHH.length) {//Ok
        }
        else if (infoHH.length === 0) // Ok{
        {
        }
        else if (viewModel.ResistDesignHH.length !== infoHH.length) {
            tabStrip.select(2);
            ShowFailedMessage("Debe introducir valores diferentes a 0 en todas las posiciones"); errore = true; return;

        }
        viewModel.ResistDesignHH = arregloData
    }

    if (viewModel.ResistDesignXX.length > 0) {
        var arrayXX = []
        arregloData = []
        $("#tableBtXX").data("kendoGrid").dataSource.view().forEach(function (item) {
            //get the value of name
            arrayXX.push(item.resistance.toString())
            arregloData.push({
                Posicion: item.position,
                IdSeccion: 'BT',
                ConexionPrueba: 'X-X',
                NoSerie: noSerieInput.value,
                Orden: item.id + 1,
                Resistencia: parseFloat(item.resistance.toString()),
                Temperatura: temperatureInput.value,
                UnidadMedida: unitMeasuringInput.value === "0" ? "Ohms" : "Miliohms"
            })

        })

        var infoXX = arrayXX.filter(c => c !== '0');

        if (viewModel.ResistDesignXX.length === infoXX.length) {//Ok
        }
        else if (infoXX.length === 0) // Ok{
        {
        }
        else if (viewModel.ResistDesignXX.length !== infoXX.length) {
            tabStrip.select(3);
            ShowFailedMessage("Debe introducir valores diferentes a 0 en todas las posiciones"); errore = true; return;

        }
        viewModel.ResistDesignXX = arregloData
        console.log(viewModel.ResistDesignXX)
    }

    if (viewModel.ResistDesignXN.length > 0) {
        var arrayXN = []
        arregloData = []
        $("#tableBtXN").data("kendoGrid").dataSource.view().forEach(function (item) {
            //get the value of name
            arrayXN.push(item.resistance.toString())

            arregloData.push({
                Posicion: item.position,
                IdSeccion: 'BT',
                ConexionPrueba: 'X-N',
                NoSerie: noSerieInput.value,
                Orden: item.id + 1,
                Resistencia: parseFloat(item.resistance.toString()),
                Temperatura: temperatureInput.value,
                UnidadMedida: unitMeasuringInput.value === "0" ? "Ohms" : "Miliohms"
            })
        })

        var infoXN = arrayXN.filter(c => c !== '0');

        if (viewModel.ResistDesignXN.length === infoXN.length) {//Ok
        }
        else if (infoXN.length === 0) // Ok{
        {
        }
        else if (viewModel.ResistDesignXN.length !== infoXN.length) {
            tabStrip.select(4);
            ShowFailedMessage("Debe introducir valores diferentes a 0 en todas las posiciones"); errore = true; return;


        }

        viewModel.ResistDesignXN = arregloData

        console.log(viewModel.ResistDesignXN)
    }

    if (viewModel.ResistDesignYN.length > 0) {
        var arrayYN = []
        arregloData = []
        $("#tableTerYN").data("kendoGrid").dataSource.view().forEach(function (item) {
            //get the value of name
            arrayYN.push(item.resistance.toString())

            arregloData.push({
                Posicion: item.position,
                IdSeccion: 'TER',
                ConexionPrueba: 'Y-N',
                NoSerie: noSerieInput.value,
                Orden: item.id + 1,
                Resistencia: parseFloat(item.resistance.toString()),
                Temperatura: temperatureInput.value,
                UnidadMedida: unitMeasuringInput.value === "0" ? "Ohms" : "Miliohms"
            })
        })

        var infoYN = arrayYN.filter(c => c !== '0');

        if (viewModel.ResistDesignYN.length === infoYN.length) {//Ok
        }
        else if (infoYN.length === 0) // Ok{
        {
        }
        else if (viewModel.ResistDesignYN.length !== infoYN.length) {
            tabStrip.select(5);
            ShowFailedMessage("Debe introducir valores diferentes a 0 en todas las posiciones"); errore = true; return;


        }

        viewModel.ResistDesignYN = arregloData
    }

    if (viewModel.ResistDesignYY.length > 0) {
     /*   alert("")*/
        var arrayYY = []
        arregloData = []
        $("#tableTerYY").data("kendoGrid").dataSource.view().forEach(function (item) {
            //get the value of name
            arrayYY.push(item.resistance.toString())

            arregloData.push({
                Posicion: item.position,
                IdSeccion: 'TER',
                ConexionPrueba: 'Y-Y',
                NoSerie: noSerieInput.value,
                Orden: item.id + 1,
                Resistencia: parseFloat(item.resistance.toString()),
                Temperatura: temperatureInput.value,
                UnidadMedida: unitMeasuringInput.value === "0" ? "Ohms" : "Miliohms"
            })
        })

        var infoYY = arrayYY.filter(c => c !== '0');

        if (viewModel.ResistDesignYY.length === infoYY.length) {//Ok
        }
        else if (infoYY.length === 0) // Ok{
        {
           
        }
        else if (viewModel.ResistDesignYY.length !== infoYY.length) {
            tabStrip.select(6);
            ShowFailedMessage("Debe introducir valores diferentes a 0 en todas las posiciones"); errore = true; return;


        }
        viewModel.ResistDesignYY = arregloData
    }



    if (viewModel.ResistDesignLLAT.length > 0) {
        var arrayLLAT = []
        arregloData = []
        $("#tableAtLL").data("kendoGrid").dataSource.view().forEach(function (item) {
            //get the value of name
            arrayLLAT.push(item.resistance.toString())

            arregloData.push({
                Posicion: item.position,
                IdSeccion: 'AT',
                ConexionPrueba: 'L-L',
                NoSerie: noSerieInput.value,
                Orden: item.id + 1,
                Resistencia: parseFloat(item.resistance.toString()),
                Temperatura: temperatureInput.value,
                UnidadMedida: unitMeasuringInput.value === "0" ? "Ohms" : "Miliohms"
            })

        })

        var infoLLAT = arrayLLAT.filter(c => c !== '0');

        if (viewModel.ResistDesignLLAT.length === infoLLAT.length) {//Ok
        }
        else if (infoLLAT.length === 0) // Ok{
        {
        }
        else if (viewModel.ResistDesignLLAT.length !== infoLLAT.length) {
            tabStrip.select(7);
            ShowFailedMessage("Debe introducir valores diferentes a 0 en todas las posiciones"); errore = true; return;


        }

        viewModel.ResistDesignLLAT = arregloData
    }

    if (viewModel.ResistDesignLLBT.length > 0) {
        var arrayLLBT = []
        arregloData = []
        $("#tableBtLL").data("kendoGrid").dataSource.view().forEach(function (item) {
            //get the value of name
            arrayLLBT.push(item.resistance.toString())

            arregloData.push({
                Posicion: item.position,
                IdSeccion: 'BT',
                ConexionPrueba: 'L-L',
                NoSerie: noSerieInput.value,
                Orden: item.id + 1,
                Resistencia: parseFloat(item.resistance.toString()),
                Temperatura: temperatureInput.value,
                UnidadMedida: unitMeasuringInput.value === "0" ? "Ohms" : "Miliohms"
            })
        })

        var infoLLBT = arrayLLBT.filter(c => c !== '0');

        if (viewModel.ResistDesignLLBT.length === infoLLBT.length) {//Ok
        }
        else if (infoLLBT.length === 0) // Ok{
        {
        }
        else if (viewModel.ResistDesignLLBT.length !== infoLLBT.length) {
            tabStrip.select(7);
            ShowFailedMessage("Debe introducir valores diferentes a 0 en todas las posiciones"); errore = true; return;


        }

        viewModel.ResistDesignLLBT = arregloData
    }

    if (viewModel.ResistDesignLLTER.length > 0) {
        var arrayLLTer = []
        arregloData = []
        $("#tableTerLL").data("kendoGrid").dataSource.view().forEach(function (item) {
            //get the value of name
            arrayLLTer.push(item.resistance.toString())

            arregloData.push({
                Posicion: item.position,
                IdSeccion: 'TER',
                ConexionPrueba: 'L-L',
                NoSerie: noSerieInput.value,
                Orden: item.id + 1,
                Resistencia: parseFloat(item.resistance.toString()),
                Temperatura: temperatureInput.value,
                UnidadMedida: unitMeasuringInput.value === "0" ? "Ohms" : "Miliohms"
            })
        })

        var infoLLTer = arrayLLTer.filter(c => c !== '0');

        if (viewModel.ResistDesignLLTER.length === infoLLTer.length) {//Ok
        }
        else if (infoLLTer.length === 0) // Ok{
        {
        }
        else if (viewModel.ResistDesignLLTER.length !== infoLLTer.length) {
            tabStrip.select(7);
            ShowFailedMessage("Debe introducir valores diferentes a 0 en todas las posiciones"); errore = true; return;


        }

        viewModel.ResistDesignLLTER = arregloData
    }


    if (viewModel.ResistDesignLNAT.length > 0) {
        var arrayLNAT = []
        arregloData = []
        $("#tableAtLN").data("kendoGrid").dataSource.view().forEach(function (item) {
            //get the value of name
            arrayLNAT.push(item.resistance.toString())

            arregloData.push({
                Posicion: item.position,
                IdSeccion: 'AT',
                ConexionPrueba: 'L-N',
                NoSerie: noSerieInput.value,
                Orden: item.id + 1,
                Resistencia: parseFloat(item.resistance.toString()),
                Temperatura: temperatureInput.value,
                UnidadMedida: unitMeasuringInput.value === "0" ? "Ohms" : "Miliohms"
            })
        })

        var infoLNAT = arrayLNAT.filter(c => c !== '0');

        if (viewModel.ResistDesignLNAT.length === infoLNAT.length) {//Ok
        }
        else if (infoLNAT.length === 0) // Ok{
        {
        }
        else if (viewModel.ResistDesignLNAT.length !== infoLNAT.length) {
            tabStrip.select(8);
            ShowFailedMessage("Debe introducir valores diferentes a 0 en todas las posiciones"); errore = true; return;


        }

        viewModel.ResistDesignLNAT = arregloData
    }

    if (viewModel.ResistDesignLNBT.length > 0) {
        var arrayLNBT = []
        arregloData = []
        $("#tableBtLN").data("kendoGrid").dataSource.view().forEach(function (item) {
            //get the value of name
            arrayLNBT.push(item.resistance.toString())
            arregloData.push({
                Posicion: item.position,
                IdSeccion: 'BT',
                ConexionPrueba: 'L-N',
                NoSerie: noSerieInput.value,
                Orden: item.id + 1,
                Resistencia: parseFloat(item.resistance.toString()),
                Temperatura: temperatureInput.value,
                UnidadMedida: unitMeasuringInput.value === "0" ? "Ohms" : "Miliohms"
            })
        })

        var infoLNBT = arrayLNBT.filter(c => c !== '0');

        if (viewModel.ResistDesignLNBT.length === infoLNBT.length) {//Ok
        }
        else if (infoLNBT.length === 0) // Ok{
        {
        }
        else if (viewModel.ResistDesignLNBT.length !== infoLNBT.length) {
            tabStrip.select(8);
            ShowFailedMessage("Debe introducir valores diferentes a 0 en todas las posiciones"); errore = true; return;


        }

        viewModel.ResistDesignLNBT = arregloData
    }

    if (viewModel.ResistDesignLNTER.length > 0) {
        var arrayLNTer = []
        arregloData = []
        $("#tableTerLN").data("kendoGrid").dataSource.view().forEach(function (item) {
            //get the value of name
            arrayLNTer.push(item.resistance.toString())
            arregloData.push({
                Posicion: item.position,
                IdSeccion: 'TER',
                ConexionPrueba: 'L-N',
                NoSerie: noSerieInput.value,
                Orden: item.id + 1,
                Resistencia: parseFloat(item.resistance.toString()),
                Temperatura: temperatureInput.value,
                UnidadMedida: unitMeasuringInput.value === "0" ? "Ohms" : "Miliohms"
            })
        })

        var infoLNTer = arrayLNTer.filter(c => c !== '0');

        if (viewModel.ResistDesignLNTER.length === infoLNTer.length) {//Ok
        }
        else if (infoLNTer.length === 0) // Ok{
        {
        }
        else if (viewModel.ResistDesignLNTER.length !== infoLNTer.length) {
            tabStrip.select(8);
            ShowFailedMessage("Debe introducir valores diferentes a 0 en todas las posiciones"); errore = true; return;


        }

        viewModel.ResistDesignLNTER = arregloData
    }


    if (errorHX === true || errorHN === true || errorHH === true || errorXX === true || errorXN === true || errorYY === true || errorYN === true || errorLLAT === true ||
        errorLLBT === true || errorLLTer === true || errorLNAT === true || errorLNBT === true || errorLNTer === true) {
        ShowFailedMessage("Revisar formato introducido en las resistencias. Solo se permiten numeros")
        return
    }

    if (!errore) {
        $("#loader").css("display", "initial");
        postData(domain + "/ResistanceTwentyDegrees/Post/", viewModel)
            .then(data => {
                if (data.response.Code !== -1) {

                    ShowSuccessMessage("Guardado Exitoso")
                    ClearForm();
                    $("#loader").css("display", "none");
                }
                else {
                    ShowFailedMessage(data.response.Description);
                    $("#loader").css("display", "none");
                }
            });
    }
    /****************************************************/

    //$('div[id^="tabstrip-"]').length

    /*if (banderaErrorGrids)
        return;

    $("#loader").css("display", "block");
    ResultValidations = true;

    MapToViewModel();

    if (resultValidations) {
        postData(domain + "/ResistanceTwentyDegrees/Post/", viewModel)
            .then(data => {
                if (data.response.Code !== -1) {

                    ShowSuccessMessage("Guardado Exitoso")
                    ClearForm();
                }
                else {
                    ShowFailedMessage(data.response.Description);
                }
            });
    } else {
        ShowFailedMessage("Hay resistencias(s) vacías, por favor verifique.")
    }

    $("#loader").css("display", "none");*/
});

btnClear.addEventListener("click", function () {
    ClearForm();
});

btnNewTensionYes.addEventListener("click", function () {

    $("#loader").css("display", "block");
    GetResistanceTwentyDegreesJSON(true).then(
        data => {
            if (data.response.Code !== -1) {

                btnSave.disabled = false;
                noSerieInput.disabled = true;

                viewModel = data.response.Structure;
                GenerarDataSources(viewModel)
                //LoadForm();
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
    GetResistanceTwentyDegreesJSON(false).then(
        data => {
            if (data.response.Code !== -1) {

                btnSave.disabled = false;
                noSerieInput.disabled = true;

                viewModel = data.response.Structure;
                GenerarDataSources(viewModel)

            }
            else {
                ShowFailedMessage(data.response.Description);
            }
            $("#loader").css("display", "none");
        }
    );

});

function GenerarDataSources(viewModel) {
    let dataSourceHX = [];
    let dataSourceHH = [];
    let dataSourceHN = [];

    let dataSourceXN = [];
    let dataSourceXX = [];

    let dataSourceYN = [];
    let dataSourceYY = [];


    if (viewModel.ResistDesignHX.length > 0) {
        $('div[id^="divAtHX"]').css("display", "initial")
        viewModel.ResistDesignHX.forEach(function (data, key) {
            dataSourceHX.push({ id: key, position: data.Posicion, resistance: data.Resistencia });

        });
        var tabla1 =  $("#tableAtHX").kendoGrid({
            dataSource: {
                data: dataSourceHX,
                schema: {
                    model: {
                        id: "id",
                        fields: {
                            position: { editable: false },
                            resistance: {
                                format: "{0:n3}",
                                decimals: 4,
                                validation: {
                                    required: {
                                        message: "Requerido"
                                    },
                                    validateFunction: function (isInput) {
                                        return validateInput(isInput,"HX");
                                    }
                                },
                                editable: true

                            }
                        }
                    }
                }

            },
            editable: true,
            scrollable: true,
            height: 300,
            columns: [
                {
                    field: "position",
                    title: "Posición"
                },
                {
                    field: "resistance",
                    title: "Resistencia",

                }],
        });
        $("#liHX").css("display", "initial")

        var gridHX = tabla1.data("kendoGrid");

        gridHX.table.on('keydown', function (e) {
            var currentNumberOfItems = gridHX.dataSource.view().length;
            var row = $(e.target).closest('tr').index();
            var col = gridHX.cellIndex($(e.target).closest('td'));

            var dataItem = gridHX.dataItem($(e.target).closest('tr'));
            var field = gridHX.columns[col].field;
            var value = $(e.target).val();


            if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {

                if (validateInput($(e.target)) == true) {
                    e.preventDefault();

                    dataItem.set(field, value);
                    if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < gridHX.columns.length) {
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

                        if (nextCellCol >= gridHX.columns.length || nextCellCol < 0) {
                            return;
                        }



                        // wait for cell to close and Grid to rebind when changes have been made

                        setTimeout(function () {
                            gridHX.editCell(gridHX.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                        });


                    }
                }
            }

        });
        
        $("#positionATHX").val(viewModel.PositionsDTO.AltaTension.length)
        $("#nominalATHX").val(viewModel.PositionsDTO.ATNom )


    }

    if (viewModel.ResistDesignHH.length > 0) {
        $('div[id^="divAtHH"]').css("display", "initial")
        viewModel.ResistDesignHH.forEach(function (data, key) {
            dataSourceHH.push({ id: key, position: data.Posicion, resistance: data.Resistencia });

        });
        var tabla2 = $("#tableAtHH").kendoGrid({
            dataSource: {
                data: dataSourceHH,
                schema: {
                    model: {
                        id: "id",
                        fields: {
                            position: { editable: false },
                            resistance: {

                                format: "{0:n3}",
                                decimals: 4,
                                validation: {
                                    required: {
                                        message: "Requerido"
                                    },
                                    validateFunction: function (isInput) {
                                        return validateInput(isInput,"HH");
                                    }
                                },
                                editable: true

                            }
                        }
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
                field: "resistance",
                title: "Resistencia",


            }],
            editable: true

        });
        $("#liHH").css("display", "initial")

        var gridHH = tabla2.data("kendoGrid");

        gridHH.table.on('keydown', function (e) {
            var currentNumberOfItems = gridHH.dataSource.view().length;
            var row = $(e.target).closest('tr').index();
            var col = gridHH.cellIndex($(e.target).closest('td'));

            var dataItem = gridHH.dataItem($(e.target).closest('tr'));
            var field = gridHH.columns[col].field;
            var value = $(e.target).val();


            if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {

                if (validateInput($(e.target)) == true) {
                    e.preventDefault();

                    dataItem.set(field, value);
                    if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < gridHH.columns.length) {
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

                        if (nextCellCol >= gridHH.columns.length || nextCellCol < 0) {
                            return;
                        }



                        // wait for cell to close and Grid to rebind when changes have been made

                        setTimeout(function () {
                            gridHH.editCell(gridHH.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                        });


                    }
                }
            }

        });

        $("#positionATHH").val(viewModel.PositionsDTO.AltaTension.length)
        $("#nominalATHH").val(viewModel.PositionsDTO.ATNom)

    }

    if (viewModel.ResistDesignHN.length > 0) {
        $('div[id^="divAtHN"]').css("display", "initial")
        viewModel.ResistDesignHN.forEach(function (data, key) {
            dataSourceHN.push({ id: key, position: data.Posicion, resistance: data.Resistencia });

        });
        var tabla3 = $("#tableAtHN").kendoGrid({
            dataSource: {
                data: dataSourceHN,
                schema: {
                    model: {
                        id: "id",
                        fields: {
                            position: { editable: false },
                            resistance: {

                                format: "{0:n3}",
                                decimals: 4,
                                validation: {
                                    required: {
                                        message: "Requerido"
                                    },
                                    validateFunction: function (isInput) {
                                        return validateInput(isInput,"HN");
                                    }
                                },
                                editable: true

                            }
                        }
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
                field: "resistance",
                title: "Resistencia",


            }],
            editable: true

        });

        $("#liHN").css("display", "initial")

        var gridHN = tabla3.data("kendoGrid");
        gridHN.table.on('keydown', function (e) {
            var currentNumberOfItems = gridHN.dataSource.view().length;
            var row = $(e.target).closest('tr').index();
            var col = gridHN.cellIndex($(e.target).closest('td'));

            var dataItem = gridHN.dataItem($(e.target).closest('tr'));
            var field = gridHN.columns[col].field;
            var value = $(e.target).val();


            if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {

                if (validateInput($(e.target)) == true) {
                    e.preventDefault();

                    dataItem.set(field, value);
                    if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < gridHN.columns.length) {
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

                        if (nextCellCol >= gridHH.columns.length || nextCellCol < 0) {
                            return;
                        }



                        // wait for cell to close and Grid to rebind when changes have been made

                        setTimeout(function () {
                            gridHN.editCell(gridHN.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                        });


                    }
                }
            }

        });

        $("#positionATHN").val(viewModel.PositionsDTO.AltaTension.length)
        $("#nominalATHN").val(viewModel.PositionsDTO.ATNom)

    }

    if (viewModel.ResistDesignXN.length > 0) {
        $('div[id^="divBtXN"]').css("display", "initial")
        viewModel.ResistDesignXN.forEach(function (data, key) {
            dataSourceXN.push({ id: key, position: data.Posicion, resistance: data.Resistencia });

        });

       

        var tabla4 = $("#tableBtXN").kendoGrid({
            dataSource: {
                data: dataSourceXN,
                schema: {
                    model: {
                        id: "id",
                        fields: {
                            position: { editable: false },
                            resistance: {

                                format: "{0:n3}",
                                decimals: 4,
                                validation: {
                                    required: {
                                        message: "Requerido"
                                    },
                                    validateFunction: function (isInput) {
                                        return validateInput(isInput,"XN");
                                    }
                                },
                                editable: true

                            }
                        }
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
                field: "resistance",
                title: "Resistencia",


            }],
            editable: true

        });

        $("#liXN").css("display", "initial")

        var gridXN = tabla4.data("kendoGrid");
        gridXN.table.on('keydown', function (e) {
            var currentNumberOfItems = gridXN.dataSource.view().length;
            var row = $(e.target).closest('tr').index();
            var col = gridXN.cellIndex($(e.target).closest('td'));

            var dataItem = gridXN.dataItem($(e.target).closest('tr'));
            var field = gridXN.columns[col].field;
            var value = $(e.target).val();


            if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {

                if (validateInput($(e.target)) == true) {
                    e.preventDefault();

                    dataItem.set(field, value);
                    if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < gridXN.columns.length) {
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

                        if (nextCellCol >= gridHH.columns.length || nextCellCol < 0) {
                            return;
                        }



                        // wait for cell to close and Grid to rebind when changes have been made

                        setTimeout(function () {
                            gridXN.editCell(gridXN.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                        });


                    }
                }
            }

        });

        $("#positionBTXN").val(viewModel.PositionsDTO.BajaTension.length)
        $("#nominalBTXN").val(viewModel.PositionsDTO.BTNom)
    }

    if (viewModel.ResistDesignXX.length > 0) {
        $('div[id^="divBtXX"]').css("display", "initial")
        viewModel.ResistDesignXX.forEach(function (data, key) {
            dataSourceXX.push({ id: key, position: data.Posicion, resistance: data.Resistencia });

        });
        var tabla5 = $("#tableBtXX").kendoGrid({
            dataSource: {
                data: dataSourceXX,
                schema: {
                    model: {
                        id: "id",
                        fields: {
                            position: { editable: false },
                            resistance: {

                                format: "{0:n3}",
                                decimals: 4,
                                validation: {
                                    required: {
                                        message: "Requerido"
                                    },
                                    validateFunction: function (isInput) {
                                        return validateInput(isInput,"XX");
                                    }
                                },
                                editable: true

                            }
                        }
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
                field: "resistance",
                title: "Resistencia",


            }],
            editable: true

        });

        $("#liXX").css("display", "initial")

        var gridXX = tabla5.data("kendoGrid");
        gridXX.table.on('keydown', function (e) {
            var currentNumberOfItems = gridXX.dataSource.view().length;
            var row = $(e.target).closest('tr').index();
            var col = gridXX.cellIndex($(e.target).closest('td'));

            var dataItem = gridXX.dataItem($(e.target).closest('tr'));
            var field = gridXX.columns[col].field;
            var value = $(e.target).val();


            if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {

                if (validateInput($(e.target)) == true) {
                    e.preventDefault();

                    dataItem.set(field, value);
                    if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < gridXX.columns.length) {
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

                        if (nextCellCol >= gridHH.columns.length || nextCellCol < 0) {
                            return;
                        }



                        // wait for cell to close and Grid to rebind when changes have been made

                        setTimeout(function () {
                            gridXX.editCell(gridXX.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                        });


                    }
                }
            }

        });

        $("#positionBTXX").val(viewModel.PositionsDTO.BajaTension.length)
        $("#nominalBTXX").val(viewModel.PositionsDTO.BTNom)
    }

    if (viewModel.ResistDesignYN.length > 0) {
        $('div[id^="divTerYN"]').css("display", "initial")
        viewModel.ResistDesignYN.forEach(function (data, key) {
            dataSourceYN.push({ id: key, position: data.Posicion, resistance: data.Resistencia });

        });

        var tabla6 = $("#tableTerYN").kendoGrid({
            dataSource: {
                data: dataSourceYN,
                schema: {
                    model: {
                        id: "id",
                        fields: {
                            position: { editable: false },
                            resistance: {

                                format: "{0:n3}",
                                decimals: 4,
                                validation: {
                                    required: {
                                        message: "Requerido"
                                    },
                                    validateFunction: function (isInput) {
                                        return validateInput(isInput,"YN");
                                    }
                                },
                                editable: true

                            }
                        }
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
                field: "resistance",
                title: "Resistencia",


            }],
            editable: true

        });

        $("#liYN").css("display", "initial")

        var gridYN = tabla6.data("kendoGrid");
        gridYN.table.on('keydown', function (e) {
            var currentNumberOfItems = gridYN.dataSource.view().length;
            var row = $(e.target).closest('tr').index();
            var col = gridYN.cellIndex($(e.target).closest('td'));

            var dataItem = gridYN.dataItem($(e.target).closest('tr'));
            var field = gridYN.columns[col].field;
            var value = $(e.target).val();


            if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {

                if (validateInput($(e.target)) == true) {
                    e.preventDefault();

                    dataItem.set(field, value);
                    if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < gridYN.columns.length) {
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

                        if (nextCellCol >= gridHH.columns.length || nextCellCol < 0) {
                            return;
                        }



                        // wait for cell to close and Grid to rebind when changes have been made

                        setTimeout(function () {
                            gridYN.editCell(gridYN.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                        });


                    }
                }
            }

        });

        $("#positionTERYN").val(viewModel.PositionsDTO.TerNom)
        $("#nominalTERYN").val(viewModel.PositionsDTO.Terciario.length)
    }

    if (viewModel.ResistDesignYY.length > 0) {
        $('div[id^="divTerYY"]').css("display", "initial")
        viewModel.ResistDesignYY.forEach(function (data, key) {
            dataSourceYY.push({ id: key, position: data.Posicion, resistance: data.Resistencia });

        });
        var tabla7 = $("#tableTerYY").kendoGrid({
            dataSource: {
                data: dataSourceYY,
                schema: {
                    model: {
                        id: "id",
                        fields: {
                            position: { editable: false },
                            resistance: {

                                format: "{0:n3}",
                                decimals: 4,
                                validation: {
                                    required: {
                                        message: "Requerido"
                                    },
                                    validateFunction: function (isInput) {
                                        return validateInput(isInput,"YY");
                                    }
                                },
                                editable: true

                            }
                        }
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
                field: "resistance",
                title: "Resistencia",


            }],
            editable: true

        });

        $("#liYY").css("display", "initial")

        var gridYY = tabla7.data("kendoGrid");
        gridYY.table.on('keydown', function (e) {
            var currentNumberOfItems = gridYY.dataSource.view().length;
            var row = $(e.target).closest('tr').index();
            var col = gridYY.cellIndex($(e.target).closest('td'));

            var dataItem = gridYY.dataItem($(e.target).closest('tr'));
            var field = gridYY.columns[col].field;
            var value = $(e.target).val();


            if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {

                if (validateInput($(e.target)) == true) {
                    e.preventDefault();

                    dataItem.set(field, value);
                    if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < gridYY.columns.length) {
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

                        if (nextCellCol >= gridHH.columns.length || nextCellCol < 0) {
                            return;
                        }



                        // wait for cell to close and Grid to rebind when changes have been made

                        setTimeout(function () {
                            gridYY.editCell(gridYY.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                        });


                    }
                }
            }

        });

        $("#positionTERYY").val(viewModel.PositionsDTO.Terciario.length)
        $("#nominalTERYY").val(viewModel.PositionsDTO.TerNom )
    }


    /********************************************/
    if (viewModel.ResistDesignLLAT.length > 0) {
        $('div[id^="divLL-AT"]').css("display", "initial")

        viewModel.ResistDesignLLAT.forEach(function (data, key) {
            dataSourceHH.push({ id: key, position: data.Posicion, resistance: data.Resistencia });

        });
        var tabla8 = $("#tableAtLL").kendoGrid({
            dataSource: {
                data: dataSourceHH,
                schema: {
                    model: {
                        id: "id",
                        fields: {
                            position: { editable: false },
                            resistance: {

                                format: "{0:n3}",
                                decimals: 4,
                                validation: {
                                    required: {
                                        message: "Requerido"
                                    },
                                    validateFunction: function (isInput) {
                                        return validateInput(isInput,"LLAT");
                                    }
                                },
                                editable: true

                            }
                        }
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
                field: "resistance",
                title: "Resistencia",


            }],
            editable: true

        });

        $("#liLL").css("display", "initial")

        var gridLL = tabla8.data("kendoGrid");
        gridLL.table.on('keydown', function (e) {
            var currentNumberOfItems = gridLL.dataSource.view().length;
            var row = $(e.target).closest('tr').index();
            var col = gridLL.cellIndex($(e.target).closest('td'));

            var dataItem = gridLL.dataItem($(e.target).closest('tr'));
            var field = gridLL.columns[col].field;
            var value = $(e.target).val();


            if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {

                if (validateInput($(e.target)) == true) {
                    e.preventDefault();

                    dataItem.set(field, value);
                    if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < gridLL.columns.length) {
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

                        if (nextCellCol >= gridLL.columns.length || nextCellCol < 0) {
                            return;
                        }



                        // wait for cell to close and Grid to rebind when changes have been made

                        setTimeout(function () {
                            gridLL.editCell(gridLL.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                        });


                    }
                }
            }

                    });

        $("#positionATLL").val(viewModel.PositionsDTO.AltaTension.length )
        $("#nominalATLL").val(viewModel.PositionsDTO.ATNom)
    }

    if (viewModel.ResistDesignLLBT.length > 0) {
        $('div[id^="divLL-BT"]').css("display", "initial")
        viewModel.ResistDesignLLBT.forEach(function (data, key) {
            dataSourceXX.push({ id: key, position: data.Posicion, resistance: data.Resistencia });

        });
        var tabla9 = $("#tableBtLL").kendoGrid({
            dataSource: {
                data: dataSourceXX,
                schema: {
                    model: {
                        id: "id",
                        fields: {
                            position: { editable: false },
                            resistance: {

                                format: "{0:n3}",
                                decimals: 4,
                                validation: {
                                    required: {
                                        message: "Requerido"
                                    },
                                    validateFunction: function (isInput) {
                                        return validateInput(isInput,"LLBT");
                                    }
                                },
                                editable: true

                            }
                        }
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
                field: "resistance",
                title: "Resistencia",


            }],
            editable: true

        });

        $("#liLL").css("display", "initial")

        var gridLLBT = tabla9.data("kendoGrid");
        gridLLBT.table.on('keydown', function (e) {
            var currentNumberOfItems = gridLLBT.dataSource.view().length;
            var row = $(e.target).closest('tr').index();
            var col = gridLLBT.cellIndex($(e.target).closest('td'));

            var dataItem = gridLLBT.dataItem($(e.target).closest('tr'));
            var field = gridLLBT.columns[col].field;
            var value = $(e.target).val();


            if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {

                if (validateInput($(e.target)) == true) {
                    e.preventDefault();

                    dataItem.set(field, value);
                    if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < gridLLBT.columns.length) {
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

                        if (nextCellCol >= gridLLBT.columns.length || nextCellCol < 0) {
                            return;
                        }



                        // wait for cell to close and Grid to rebind when changes have been made

                        setTimeout(function () {
                            gridLLBT.editCell(gridLLBT.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                        });


                    }
                }
            }

        });

        $("#positionBTLL").val(viewModel.PositionsDTO.BajaTension.length)
        $("#nominalBTLL").val(viewModel.PositionsDTO.BTNom )
    }

    if (viewModel.ResistDesignLLTER.length > 0) {
        $('div[id^="divLL-Ter"]').css("display", "initial")
        viewModel.ResistDesignLLTER.forEach(function (data, key) {
            dataSourceYY.push({ id: key, position: data.Posicion, resistance: data.Resistencia });

        });
        var tabla10 = $("#tableTerLL").kendoGrid({
            dataSource: {
                data: dataSourceYY,
                schema: {
                    model: {
                        id: "id",
                        fields: {
                            position: { editable: false },
                            resistance: {

                                format: "{0:n3}",
                                decimals: 4,
                                validation: {
                                    required: {
                                        message: "Requerido"
                                    },
                                    validateFunction: function (isInput) {
                                        return validateInput(isInput,"LLTer");
                                    }
                                },
                                editable: true

                            }
                        }
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
                field: "resistance",
                title: "Resistencia",


            }],
            editable: true

        });

        $("#liLL").css("display", "initial")

        var gridLLTer = tabla10.data("kendoGrid");
        gridLLTer.table.on('keydown', function (e) {
            var currentNumberOfItems = gridLLTer.dataSource.view().length;
            var row = $(e.target).closest('tr').index();
            var col = gridLLTer.cellIndex($(e.target).closest('td'));

            var dataItem = gridLLTer.dataItem($(e.target).closest('tr'));
            var field = gridLLTer.columns[col].field;
            var value = $(e.target).val();


            if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {

                if (validateInput($(e.target)) == true) {
                    e.preventDefault();

                    dataItem.set(field, value);
                    if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < gridLLTer.columns.length) {
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

                        if (nextCellCol >= gridLLTer.columns.length || nextCellCol < 0) {
                            return;
                        }



                        // wait for cell to close and Grid to rebind when changes have been made

                        setTimeout(function () {
                            gridLNTer.editCell(gridLNTer.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                        });


                    }
                }
            }

        });

        $("#positionTERLL").val(viewModel.PositionsDTO.Terciario.length )
        $("#nominalTERLL").val(viewModel.PositionsDTO.TerNom)
    }


    if (viewModel.ResistDesignLNAT.length > 0) {
        $('div[id^="divLN-AT"]').css("display", "initial")
        viewModel.ResistDesignLNAT.forEach(function (data, key) {
            dataSourceHN.push({ id: key, position: data.Posicion, resistance: data.Resistencia });

        });
        var tabla11 = $("#tableAtLN").kendoGrid({
            dataSource: {
                data: dataSourceHN,
                schema: {
                    model: {
                        id: "id",
                        fields: {
                            position: { editable: false },
                            resistance: {

                                format: "{0:n3}",
                                decimals: 4,
                                validation: {
                                    required: {
                                        message: "Requerido"
                                    },
                                    validateFunction: function (isInput) {
                                        return validateInput(isInput,"LNAT");
                                    }
                                },
                                editable: true

                            }
                        }
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
                field: "resistance",
                title: "Resistencia",


            }],
            editable: true

        });

        $("#liLN").css("display", "initial")

        var gridLN = tabla11.data("kendoGrid");
        gridLN.table.on('keydown', function (e) {
            var currentNumberOfItems = gridLN.dataSource.view().length;
            var row = $(e.target).closest('tr').index();
            var col = gridLN.cellIndex($(e.target).closest('td'));

            var dataItem = gridLN.dataItem($(e.target).closest('tr'));
            var field = gridLN.columns[col].field;
            var value = $(e.target).val();


            if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {

                if (validateInput($(e.target)) == true) {
                    e.preventDefault();

                    dataItem.set(field, value);
                    if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < gridLN.columns.length) {
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

                        if (nextCellCol >= gridLN.columns.length || nextCellCol < 0) {
                            return;
                        }



                        // wait for cell to close and Grid to rebind when changes have been made

                        setTimeout(function () {
                            gridLN.editCell(gridLN.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                        });


                    }
                }
            }

        });

        $("#positionATLN").val(viewModel.PositionsDTO.AltaTension.length)
        $("#nominalATLN").val(viewModel.PositionsDTO.ATNom)
    }

    if (viewModel.ResistDesignLNBT.length > 0) {
        $('div[id^="divLN-BT"]').css("display", "initial")
        viewModel.ResistDesignLNBT.forEach(function (data, key) {
            dataSourceXN.push({ id: key, position: data.Posicion, resistance: data.Resistencia });

        });
        var tabla12 = $("#tableBtLN").kendoGrid({
            dataSource: {
                data: dataSourceXN,
                schema: {
                    model: {
                        id: "id",
                        fields: {
                            position: { editable: false },
                            resistance: {

                                format: "{0:n3}",
                                decimals: 4,
                                validation: {
                                    required: {
                                        message: "Requerido"
                                    },
                                    validateFunction: function (isInput) {
                                        return validateInput(isInput,"LNBT");
                                    }
                                },
                                editable: true

                            }
                        }
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
                field: "resistance",
                title: "Resistencia",


            }],
            editable: true

        });

        $("#liLN").css("display", "initial")

        var gridLNBT = tabla12.data("kendoGrid");
        gridLNBT.table.on('keydown', function (e) {
            var currentNumberOfItems = gridLNBT.dataSource.view().length;
            var row = $(e.target).closest('tr').index();
            var col = gridLNBT.cellIndex($(e.target).closest('td'));

            var dataItem = gridLNBT.dataItem($(e.target).closest('tr'));
            var field = gridLNBT.columns[col].field;
            var value = $(e.target).val();


            if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {

                if (validateInput($(e.target)) == true) {
                    e.preventDefault();

                    dataItem.set(field, value);
                    if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < gridLNBT.columns.length) {
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

                        if (nextCellCol >= gridLNBT.columns.length || nextCellCol < 0) {
                            return;
                        }



                        // wait for cell to close and Grid to rebind when changes have been made

                        setTimeout(function () {
                            gridLNBT.editCell(gridLNBT.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                        });


                    }
                }
            }

        });

        $("#positionBTLN").val(viewModel.PositionsDTO.BajaTension.length)
        $("#nominalBTLN").val(viewModel.PositionsDTO.BTNom)
    }

    if (viewModel.ResistDesignLNTER.length > 0) {
        $('div[id^="divLN-Ter"]').css("display", "initial")
        viewModel.ResistDesignLNTER.forEach(function (data, key) {
            dataSourceYN.push({ id: key, position: data.Posicion, resistance: data.Resistencia });

        });
        var tabla13 = $("#tableTerLN").kendoGrid({
            dataSource: {
                data: dataSourceYN,
                schema: {
                    model: {
                        id: "id",
                        fields: {
                            position: { editable: false },
                            resistance: {

                                format: "{0:n3}",
                                decimals: 4,
                                validation: {
                                    required: {
                                        message: "Requerido"
                                    },
                                    validateFunction: function (isInput) {
                                        return validateInput(isInput,"LNTer");
                                    }
                                },
                                editable: true

                            }
                        }
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
                field: "resistance",
                title: "Resistencia",


            }],
            editable: true

        });

        $("#liLN").css("display", "initial")

        var gridLNTer = tabla13.data("kendoGrid");
        gridLNTer.table.on('keydown', function (e) {
            var currentNumberOfItems = gridLNTer.dataSource.view().length;
            var row = $(e.target).closest('tr').index();
            var col = gridLNTer.cellIndex($(e.target).closest('td'));

            var dataItem = gridLNTer.dataItem($(e.target).closest('tr'));
            var field = gridLNTer.columns[col].field;
            var value = $(e.target).val();


            if (e.keyCode === kendo.keys.ENTER && $($(e.target).closest('.k-edit-cell'))[0]) {

                if (validateInput($(e.target)) == true) {
                    e.preventDefault();

                    dataItem.set(field, value);
                    if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < gridLNTer.columns.length) {
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

                        if (nextCellCol >= gridLNTer.columns.length || nextCellCol < 0) {
                            return;
                        }



                        // wait for cell to close and Grid to rebind when changes have been made

                        setTimeout(function () {
                            gridLLTer.editCell(gridLLTer.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                        });


                    }
                }
            }

        });

        $("#positionTERLN").val(viewModel.PositionsDTO.Terciario.length)
        $("#nominalTERLN").val(viewModel.PositionsDTO.TerNom)
    }

    /*******************************************/
    if (viewModel.TipoAparato === "AUT") {
        $('div[id^="tabstrip-"]').css("width", '45%')
    }
}

function updateError(idTable, valor) {
    if (idTable === "HX") {
        errorHX = valor
    }
    if (idTable === "HN") {
        errorHN = valor
    }
    if (idTable === "HH") {
        errorHH = valor
    }
    if (idTable === "XX") {
        errorXX = valor
    }
    if (idTable === "XN") {
        errorXN = valor
    }
    if (idTable === "YY") {
        errorYY = valor
    }
    if (idTable === "YN") {
        errorYN = valor
    }




    if (idTable === "LLAT") {
        errorLLAT = valor
    }

    if (idTable === "LLBT") {
        errorLLBT = valor
    }
    if (idTable === "LLTer") {
        errorLLTer = valor
    }

    if (idTable === "LNAT") {
        errorLNAT = valor
    }

    if (idTable === "LNBT") {
        errorLNBT = valor
    }
    if (idTable === "LNTer") {
        errorLNTer = valor
    }

}

btnRequest.addEventListener("click", async function () {


    if (!CheckFields())
        return;

  

    $("#loader").css("display", "initial");
    await GetResistanceTwentyDegreesJSON(null).then(
        data => {
            if (data.response.Code !== -1 && data.response.Code !== 2) {


                noSerieInput.disabled = true;
                unitMeasuringInput.disabled = true;
                temperatureInput.disabled = true;
                btnRequest.disabled = true;
                btnSave.disabled = false;
                viewModel = data.response.Structure;
                requestInicial = false
                console.log(viewModel)
                $('div[id^="divLL"]').css("display", "none")
                $('div[id^="divLN"]').css("display", "none")
                $('div[id^="divAt"]').css("display", "none")
                $('div[id^="divBt"]').css("display", "none")
                $('div[id^="divTer"]').css("display", "none")
                GenerarDataSources(viewModel);
            }
            else if (data.response.Code === 2) {
                requestInicial = false
                AskUpdateInfo();
            }
            else {
                ShowFailedMessage(data.response.Description);
            }
            $("#loader").css("display", "none");
        }
    );

    $("#tabstrip").css("display" ,"block")
     /*************************************************/


});

//Requests

function validateInput(input,idTabla = null) {
    banderaErrorGrids = false;
    var tension = $(input).val();



    if (tension == null || tension == "" || tension == undefined) {
        banderaErrorGrids = true;
        input.attr("data-validateFunction-msg", "Requerido");
        return false;
    }


    if (tension == "" || tension == null || tension == undefined) {
        banderaErrorGrids = true;
    }

    var result = tension.split(".");

    if (!(/^[0-9]+$/.test(result[0]))) {
        updateError(idTabla, true);
        banderaErrorGrids = true;
        input.attr("data-validateFunction-msg", "Solo se permiten números");
        return false;
    }
    else
    {
        updateError(idTabla, false);
    }


    if (result.length > 1) {
        if (!(/^[0-9]+$/.test(result[1]))) {
            banderaErrorGrids = true;
            updateError(idTabla, true);
            input.attr("data-validateFunction-msg", "Solo se permiten números");
            return false;
        }
        else
        {
            updateError(idTabla, false);
        }
    }


    if (tension > 99999999.9999) {
        banderaErrorGrids = true;
        updateError(idTabla, true);
        input.attr("data-validateFunction-msg", "El número no debe ser mayor a 99,999,999.9999");
        return false;
    } else {
        updateError(idTabla, false);
    }

    // 0 = Ohms
    if (result[0].length > 3 && unitMeasuringInput.value === '0') {
        updateError(idTabla, true);
        banderaErrorGrids = true;
        input.attr("data-validateFunction-msg", "La resistencia en ohms debe ser numérica mayor a cero considerando 3 enteros con 6 decimales.");
        return false;
    }
    // 1 = Miliohms
    else if (result[0].length > 3 && unitMeasuringInput.value === '1') {
        banderaErrorGrids = true;
        updateError(idTabla, true);
        input.attr("data-validateFunction-msg", "La resistencia en miliohms debe ser numérica mayor a cero considerando 3 enteros con 4 decimales.");
        return false;
    } else {
        updateError(idTabla, false);
    }


    if (result.length == 2) {
        if (result[1].length > 6 && unitMeasuringInput.value === '0') {
            updateError(idTabla, true);
            banderaErrorGrids = true;
            input.attr("data-validateFunction-msg", "La resistencia en ohms debe ser numérica mayor a cero considerando 3 enteros con 6 decimales.");
            return false;
        }
        else if (result[1].length > 4 && unitMeasuringInput.value === '1') {
            updateError(idTabla, true);
            banderaErrorGrids = true;
            input.attr("data-validateFunction-msg", "La resistencia en miliohms debe ser numérica mayor a cero considerando 3 enteros con 4 decimales.");
            return false;
        } else {
            updateError(idTabla, false);
        }
    }
    else if (result.length > 2) {
        updateError(idTabla, true);
        banderaErrorGrids = true;
        input.attr("data-validateFunction-msg", "La resistencia en miliohms debe ser numérica mayor a cero considerando 3 enteros con 4 decimales.");
        return false;
    } else {
        updateError(idTabla, false);
    }


    return true;
}


async function GetTestConection() {
    var path = path = "/ResistanceTwentyDegrees/GetConnectionTest/";

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const ResistanceTwentyDegrees = await response.json();
        return ResistanceTwentyDegrees;
    }

    else {
        ShowFailedMessage('Error, por favor contacte al administrador del sistema.');
        return null;
    }
}

async function GetResistanceTwentyDegreesJSON(isNewTensionResponse) {
    var path = path = "/ResistanceTwentyDegrees/Get/";
    var url = new URL(domain + path),
        params = {
            noSerie: noSerieInput.value,
            measuring: unitMeasuringInput.options[unitMeasuringInput.selectedIndex].text,
            temperature: parseFloat(temperatureInput.value),
            newTensonResponse: isNewTensionResponse,
            requestInicial: requestInicial
        }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const ResistanceTwentyDegrees = await response.json();
        return ResistanceTwentyDegrees;
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

function CheckFields() {

    var flagNoSerie = true, flagNoSerie2 = true, flagTemperature = true, flagRegTemperature = true, flagMeasuring = true;

    //check noSerie
    if (noSerieInput.value === undefined || noSerieInput.value === "" || noSerieInput.value === null) {
        $(`#NoSerieSpand`).text("Requerido");
        flagNoSerie = false;
    }
    else {
        $(`#NoSerieSpand`).text("");
        flagNoSerie = true;
    }

    if (!(/^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ0-9-@]*$/.test(noSerieInput.value))) {
        $(`#NoSerieSpand`).text("Character(es) no permitido.");
        flagNoSerie2 = false;
    }
    else {
        if (flagNoSerie) {

            $(`#NoSerieSpand`).text("");
            flagNoSerie2 = true;
        }
    }

    //Check Temperature
    if (temperatureInput.value === undefined || temperatureInput.value === "" || temperatureInput.value === null) {
        $("#TemperatureSpand").text("Requerido");
        flagTemperature = false;
    }
    else {
        $("#TemperatureSpand").text("");
        flagTemperature = true;
    }

    if (!PATTERN.test(temperatureInput.value)) {
        $("#TemperatureSpand").text("La temperatura ser numérica mayor a cero considerando 3 enteros con 1 decimal.");
        flagRegTemperature = false;
    }
    else {
        $("#TemperatureSpand").text("");
        flagRegTemperature = true;
    }

    //Check measuring resistance
    if (unitMeasuringInput.value === undefined || unitMeasuringInput.value === "" || unitMeasuringInput.value === null) {
        $("#UnitMeasuringSpand").text("Requerido");
        flagMeasuring = false;
    }
    else {
        $("#UnitMeasuringSpand").text("");
        flagMeasuring = true;
    }


    return (flagMeasuring && flagTemperature && flagNoSerie && flagNoSerie2 && flagRegTemperature);
}


function ClearForm() {

    noSerieInput.disabled = false;
    noSerieInput.value = '';
    btnSave.disabled = true;
    unitMeasuringInput.disabled = false;
    unitMeasuringInput.value = '';
    temperatureInput.disabled = false;
    temperatureInput.value = 0;
    btnRequest.disabled = false;

    var tabStrip = $("#tabstrip").kendoTabStrip().data("kendoTabStrip");
    tabStrip.remove($('#tabstrip'));

    $(".k-tabstrip-wrapper").append(html);

    $("#tabstrip").kendoTabStrip({
        select: onSelect,
        animation: {
            open: {
                effects: "fadeIn"
            }
        }
    });

    $('#tableAtHX').data().kendoGrid?.destroy();
    $('#tableAtHN').data().kendoGrid?.destroy();
    $('#tableAtHH').data().kendoGrid?.destroy();
    $('#tableBtXN').data().kendoGrid?.destroy();
    $('#tableBtXX').data().kendoGrid?.destroy();
    $('#tableTerYY').data().kendoGrid?.destroy();
    $('#tableTerYN').data().kendoGrid?.destroy();


    $('#tableTerLL').data().kendoGrid?.destroy();
    $('#tableTerLN').data().kendoGrid?.destroy();
    $('#tableAtLL').data().kendoGrid?.destroy();
    $('#tableAtLN').data().kendoGrid?.destroy();
    $('#tableBtLL').data().kendoGrid?.destroy();
    $('#tableBtLN').data().kendoGrid?.destroy();

    $('div[id^="divLL"]').css("display", "none")
    $('div[id^="divLN"]').css("display", "none")
    $('div[id^="divAt"]').css("display", "none")
    $('div[id^="divBt"]').css("display", "none")
    $('div[id^="divTer"]').css("display", "none")

    requestInicial = true;
    errore = false;

  
}

function CheckNominalValue(tableTer) {

    if (tableTer.length === 1) {
        if (!(parseFloat(tableTer[0].RESISTANCE) > 0))
            return false;
        else
            return true;
    }
    else
        return true;
}

