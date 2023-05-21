namespace SPL.WebApp.Domain.Services.Imp.ISZ
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public class IszService : IIszService
    {
        #region Error message
        private readonly ICorrectionFactorService _correcctionFactor;
        private readonly INozzleInformationService _nozzleInformationService;
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion
        public IszService(ICorrectionFactorService correcctionFactor, INozzleInformationService nozzleInformationService)
        {
            _correcctionFactor = correcctionFactor;
            _nozzleInformationService = nozzleInformationService;
        }
        public void PrepareTemplate_Isz(SettingsToDisplayISZReportsDTO reportsDTO, ref Workbook workbook, string ClavePrueba, decimal degreesCor, List<PlateTensionDTO> tension, ref int filas, ref string posiMayor,
            string idioma, string[] AtList = null, string[] BTList = null, string[] TerList = null, string devanadoEnergizado = null, string seleccionadoTodosABT = null)
        {
            try
            {

                string data = "";
                for (int i = 0; i < 60; i++)
                {
                    if (workbook.Sheets[0].Rows[i] != null)
                    {
                        if (workbook.Sheets[0].Rows[i].Cells != null)
                        {
                            foreach (Cell cell in workbook.Sheets[0].Rows[i].Cells)
                            {
                                if (cell != null)
                                {
                                    data += cell.Value?.ToString() ?? "null";
                                    data += ",";
                                }
                                else
                                {
                                    data += "null,";
                                }
                            }
                        }
                        else
                        {
                            data += "null";
                        }
                    }
                    data += Environment.NewLine;
                }

                #region Update Readonly all cells
                workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));
                #endregion

                int[] _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cliente")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.Client;

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoSerie")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.NoSerie;

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Capacidad")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.Capacity) ? string.Empty : $"{reportsDTO.HeadboardReport.Capacity} MVA";

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Color = "Black";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "MM/dd/yyyy";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "date",
                    AllowNulls = false,
                    MessageTemplate = $"Fecha es requerida y debe estar dentro del rango 1/1/1900 - {DateTime.Now:MM/dd/yyyy}",
                    ComparerType = "between",
                    From = "DATEVALUE(\"1/1/1900\")",
                    To = $"DATEVALUE(\"{DateTime.Now:MM/dd/yyyy}\")",
                    Type = "reject",
                    TitleTemplate = "Error",
                    ShowButton = true
                };

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("CapacidadBase")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "between",
                    From = "1",
                    To = "9999.999",
                    AllowNulls = false,
                    MessageTemplate = "Capacidad base debe ser mayor a cero considerando 4 enteros con 3 decimales",
                    Type = "reject",
                    TitleTemplate = "Error"

                };
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.BaseCapacity;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##,##0.000";

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Temperatura")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "between",
                    From = "1",
                    To = "999.99",
                    AllowNulls = false,
                    MessageTemplate = "Temperatura debe ser mayor a cero considerando 3 enteros con 2 decimales",
                    Type = "reject",
                    TitleTemplate = "Error"

                };
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##,##0.00";

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitDatosCorr") && c.ClavePrueba == ClavePrueba).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", degreesCor.ToString());

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPotenciaCorr") && c.ClavePrueba == ClavePrueba).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", degreesCor.ToString());

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitDatosMedidos") && c.ClavePrueba == ClavePrueba).Celda);

                if (ClavePrueba != "ABT")
                {
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", devanadoEnergizado);
                }
                else
                {
                    string[] denavadosHEaders = devanadoEnergizado.Split(",");
                    foreach (var item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TitDatosMedidos") && c.ClavePrueba == ClavePrueba).Select((value, i) => new { i, value }))
                    {
                        _positionWB = GetRowColOfWorbook(item.value.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", denavadosHEaders[item.i]);
                    }

                    foreach (var item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TitDatosCorr") && c.ClavePrueba == ClavePrueba).Select((value, i) => new { i, value }))
                    {
                        _positionWB = GetRowColOfWorbook(item.value.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", degreesCor.ToString());
                    }

                    foreach (var item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TitPotenciaCorr") && c.ClavePrueba == ClavePrueba).Select((value, i) => new { i, value }))
                    {
                        _positionWB = GetRowColOfWorbook(item.value.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", degreesCor.ToString());
                    }
                }

                BorderStyle boder = new()
                {
                    Color = "Black",
                    Size = 1
                };

                BorderStyle noBoder = new()
                {
                    Color = "White",
                    Size = 1
                };

                int cantidadFilasABT = 0;
                string posicionMayor = "";

                if (ClavePrueba == "AYB")
                {
                    filas = AtList.Count() > 1 ? AtList.Count() : BTList.Count();
                }
                else
                if (ClavePrueba == "AYT")
                {
                    filas = AtList.Count() > 1 ? AtList.Count() : TerList.Count();
                }
                else
                if (ClavePrueba == "BYT")
                {
                    filas = BTList.Count() > 1 ? BTList.Count() : TerList.Count();
                }
                else
                if (ClavePrueba == "ABT")
                {
                    if (AtList.Count() > 1)
                    {
                        cantidadFilasABT = AtList.Count();
                        posicionMayor = "AT";
                    }
                    else if (BTList.Count() > 1)
                    {
                        cantidadFilasABT = BTList.Count();
                        posicionMayor = "BT";
                    }
                    else if (TerList.Count() > 1)
                    {
                        cantidadFilasABT = TerList.Count();
                        posicionMayor = "TER";
                    }
                    else if (seleccionadoTodosABT == "AT")
                    {
                        cantidadFilasABT = AtList.Count();
                        posicionMayor = "AT";
                    }
                    else if (seleccionadoTodosABT == "BT")
                    {
                        cantidadFilasABT = BTList.Count();
                        posicionMayor = "BT";
                    }
                    else if (seleccionadoTodosABT.ToUpper() == "TER")
                    {
                        cantidadFilasABT = TerList.Count();
                        posicionMayor = "TER";
                    }
                }

                if (ClavePrueba != "ABT")
                {
                    for (int i = 1; i <= filas; i++)
                    {

                        decimal tension1 = 0;
                        decimal tension2 = 0;

                        if (ClavePrueba == "AYB")
                        {
                            _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "AT" && x.Posicion == (AtList.Count() > 1 ? AtList[i - 1] : AtList[0])).FirstOrDefault()?.Tension.ToString(), out tension1);
                            _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "BT" && x.Posicion == (BTList.Count() > 1 ? BTList[i - 1] : BTList[0])).FirstOrDefault()?.Tension.ToString(), out tension2);

                            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos_1") && c.ClavePrueba == ClavePrueba).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Value = AtList.Count() > 1 ? AtList[i - 1] : AtList[0];
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 1].Value = BTList.Count() > 1 ? BTList[i - 1] : BTList[0];

                            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension_1") && c.ClavePrueba == ClavePrueba).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Value = tension1;
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 1].Value = tension2;
                        }
                        else if (ClavePrueba == "AYT")
                        {
                            _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "AT" && x.Posicion == (AtList.Count() > 1 ? AtList[i - 1] : AtList[0])).FirstOrDefault()?.Tension.ToString(), out tension1);
                            _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "TER" && x.Posicion == (TerList.Count() > 1 ? TerList[i - 1] : TerList[0])).FirstOrDefault()?.Tension.ToString(), out tension2);

                            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos_1") && c.ClavePrueba == ClavePrueba).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Value = AtList.Count() > 1 ? AtList[i - 1] : AtList[0];
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 1].Value = TerList.Count() > 1 ? TerList[i - 1] : TerList[0];

                            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension_1") && c.ClavePrueba == ClavePrueba).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Value = tension1;
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 1].Value = tension2;
                        }
                        else if (ClavePrueba == "BYT")
                        {
                            _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "BT" && x.Posicion == (BTList.Count() > 1 ? BTList[i - 1] : BTList[0])).FirstOrDefault()?.Tension.ToString(), out tension1);
                            _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "TER" && x.Posicion == (TerList.Count() > 1 ? TerList[i - 1] : TerList[0])).FirstOrDefault()?.Tension.ToString(), out tension2);

                            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos_1") && c.ClavePrueba == ClavePrueba).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Value = BTList.Count() > 1 ? BTList[i - 1] : BTList[0];
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 1].Value = TerList.Count() > 1 ? TerList[i - 1] : TerList[0];

                            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension_1") && c.ClavePrueba == ClavePrueba).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Value = tension1;
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 1].Value = tension2;

                        }

                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos_1") && c.ClavePrueba == ClavePrueba).Celda);

                        for (int j = 0; j < 11; j++)
                        {
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderBottom = boder;
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderRight = boder;
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderLeft = boder;
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderTop = boder;

                        }

                        if (filas == 1)
                        {
                            for (int j = 0; j < 11; j++)
                            {
                                workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].BorderBottom = noBoder;
                                workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].BorderRight = noBoder;
                                workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].BorderLeft = noBoder;
                            }
                        }
                    }

                    for (int i = 0; i < filas; i++)
                    {
                        string celdaLetra = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionVrms") && c.ClavePrueba == ClavePrueba)?.Celda[0].ToString();
                        int celdaNumber = Convert.ToInt32(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionVrms") && c.ClavePrueba == ClavePrueba)?.Celda.Substring(1, 2)) + i;
                        string val = "AND(REGEXP_MATCH(" + celdaLetra + celdaNumber.ToString() + ",@^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,1})?%?$@))";
                        val = val.Replace('@', '"');
                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionVrms") && c.ClavePrueba == ClavePrueba).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            AllowNulls = false,
                            MessageTemplate = "El valor de la tensión vrms debe ser numérico mayor a cero considerando 6 enteros con 1 decimales",
                            From = val,
                            ComparerType = "custom",
                            Type = "reject",
                            TitleTemplate = "Error",
                            ShowButton = true

                        };
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##,##0.0";

                        celdaLetra = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == ClavePrueba)?.Celda[0].ToString();
                        celdaNumber = Convert.ToInt32(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == ClavePrueba)?.Celda.Substring(1, 2)) + i;
                        val = "AND(REGEXP_MATCH(" + celdaLetra + celdaNumber.ToString() + ",@^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,3})?%?$@))"; val = val.Replace('@', '"');
                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == ClavePrueba).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            AllowNulls = false,
                            MessageTemplate = "El valor de la corriente irms debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                            From = val,
                            ComparerType = "custom",
                            Type = "reject",
                            TitleTemplate = "Error",
                            ShowButton = true

                        };
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##,##0.000";

                        celdaLetra = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PotenciaKW") && c.ClavePrueba == ClavePrueba)?.Celda[0].ToString();
                        celdaNumber = Convert.ToInt32(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PotenciaKW") && c.ClavePrueba == ClavePrueba)?.Celda.Substring(1, 2)) + i;
                        val = "AND(REGEXP_MATCH(" + celdaLetra + celdaNumber.ToString() + ",@^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,3})?%?$@))"; val = val.Replace('@', '"');
                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PotenciaKW") && c.ClavePrueba == ClavePrueba).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            AllowNulls = false,
                            MessageTemplate = "El valor de la potencia KW debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                            From = val,
                            ComparerType = "custom",
                            Type = "reject",
                            TitleTemplate = "Error",
                            ShowButton = true

                        };
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##,##0.000";
                    }
                }
                else
                {
                    //BTList = new string[] { "OLA" };
                    decimal tension1 = 0;
                    decimal tension2 = 0;
                    decimal tension3 = 0;
                    for (int i = 1; i <= cantidadFilasABT; i++)
                    {

                        if (posicionMayor.ToUpper() == "AT")
                        {
                            _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "BT" && x.Posicion == BTList[0]).FirstOrDefault()?.Tension.ToString(), out tension2);
                            _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "TER" && x.Posicion == TerList[0]).FirstOrDefault()?.Tension.ToString(), out tension3);

                            IEnumerable<ConfigurationReportsDTO> celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Pos_1") && c.ClavePrueba == ClavePrueba && (c.Seccion == 1 || c.Seccion == 2));
                            ConfigurationReportsDTO celdaLast = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos_1") && c.ClavePrueba == ClavePrueba && c.Seccion == 3);

                            _positionWB = GetRowColOfWorbook(celdaLast.Celda);
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = BTList[0];
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Value = TerList[0];

                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Value = tension2;
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Value = tension3;
                            foreach (ConfigurationReportsDTO item in celdas)
                            {
                                if (item.Seccion == 1)
                                {

                                    _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "AT" && x.Posicion == AtList[i - 1]).FirstOrDefault()?.Tension.ToString(), out tension1);
                                    _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "BT" && x.Posicion == BTList[0]).FirstOrDefault()?.Tension.ToString(), out tension2);

                                    _positionWB = GetRowColOfWorbook(item.Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Value = AtList[i - 1];
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 1].Value = BTList[0];
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 2].Value = tension1;
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 3].Value = tension2;
                                    for (int j = 0; j < 11; j++)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderBottom = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderRight = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderLeft = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderTop = boder;

                                    }
                                }
                                else if (item.Seccion == 2)
                                {
                                    _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "AT" && x.Posicion == AtList[i - 1]).FirstOrDefault()?.Tension.ToString(), out tension1);
                                    _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "TER" && x.Posicion == TerList[0]).FirstOrDefault()?.Tension.ToString(), out tension2);

                                    _positionWB = GetRowColOfWorbook(item.Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Value = AtList[i - 1];
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 1].Value = TerList[0];
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 2].Value = tension1;
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 3].Value = tension2;
                                    for (int j = 0; j < 11; j++)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderBottom = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderRight = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderLeft = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderTop = boder;

                                    }
                                }
                            }

                            AgregarValidacionesABT(ClavePrueba, reportsDTO, cantidadFilasABT, posicionMayor, ref workbook);
                        }
                        else if (posicionMayor.ToUpper() == "BT")
                        {

                            _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "AT" && x.Posicion == AtList[0]).FirstOrDefault()?.Tension.ToString(), out tension1);
                            _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "TER" && x.Posicion == TerList[0]).FirstOrDefault()?.Tension.ToString(), out tension3);

                            IEnumerable<ConfigurationReportsDTO> celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Pos_1") && c.ClavePrueba == ClavePrueba && (c.Seccion == 1 || c.Seccion == 3));
                            ConfigurationReportsDTO celdaLast = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos_1") && c.ClavePrueba == ClavePrueba && c.Seccion == 2);
                            _positionWB = GetRowColOfWorbook(celdaLast.Celda);
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = AtList[0];
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Value = TerList[0];

                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Value = tension1;
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Value = tension3;

                            foreach (ConfigurationReportsDTO item in celdas)
                            {
                                if (item.Seccion == 1)
                                {

                                    _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "AT" && x.Posicion == AtList[i - 1]).FirstOrDefault()?.Tension.ToString(), out tension1);
                                    _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "BT" && x.Posicion == BTList[0]).FirstOrDefault()?.Tension.ToString(), out tension2);

                                    _positionWB = GetRowColOfWorbook(item.Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Value = AtList[i - 1];
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 1].Value = BTList[0];
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 2].Value = tension1;
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 3].Value = tension2;
                                    for (int j = 0; j < 11; j++)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderBottom = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderRight = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderLeft = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderTop = boder;

                                    }
                                }
                                else if (item.Seccion == 3)
                                {
                                    _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "BT" && x.Posicion == BTList[i - 1]).FirstOrDefault()?.Tension.ToString(), out tension2);
                                    _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "TER" && x.Posicion == TerList[0]).FirstOrDefault()?.Tension.ToString(), out tension3);

                                    _positionWB = GetRowColOfWorbook(item.Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Value = BTList[i - 1];
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 1].Value = TerList[0];
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 2].Value = tension2;
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 3].Value = tension3;
                                    for (int j = 0; j < 11; j++)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderBottom = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderRight = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderLeft = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderTop = boder;

                                    }
                                }
                            }
                            AgregarValidacionesABT(ClavePrueba, reportsDTO, cantidadFilasABT, posicionMayor, ref workbook);
                        }
                        else if (posicionMayor.ToUpper() == "TER")
                        {
                            _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "AT" && x.Posicion == AtList[i - 1]).FirstOrDefault()?.Tension.ToString(), out tension1);
                            _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "BT" && x.Posicion == BTList[0]).FirstOrDefault()?.Tension.ToString(), out tension2);

                            IEnumerable<ConfigurationReportsDTO> celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Pos_1") && c.ClavePrueba == ClavePrueba && (c.Seccion == 2 || c.Seccion == 3));
                            ConfigurationReportsDTO celdaLast = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos_1") && c.ClavePrueba == ClavePrueba && c.Seccion == 1);
                            _positionWB = GetRowColOfWorbook(celdaLast.Celda);

                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = AtList[0];
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Value = BTList[0];
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 2].Value = tension1;
                            workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 3].Value = tension2;

                            foreach (ConfigurationReportsDTO item in celdas)
                            {
                                if (item.Seccion == 2)
                                {

                                    _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "AT" && x.Posicion == AtList[i - 1]).FirstOrDefault()?.Tension.ToString(), out tension1);
                                    _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "TER" && x.Posicion == TerList[0]).FirstOrDefault()?.Tension.ToString(), out tension3);

                                    _positionWB = GetRowColOfWorbook(item.Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Value = AtList[i - 1];
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 1].Value = TerList[0];
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 2].Value = tension1;
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 3].Value = tension3;
                                    for (int j = 0; j < 11; j++)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderBottom = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderRight = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderLeft = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderTop = boder;

                                    }
                                }
                                else if (item.Seccion == 3)
                                {
                                    _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "BT" && x.Posicion == BTList[i - 1]).FirstOrDefault()?.Tension.ToString(), out tension2);
                                    _ = decimal.TryParse(tension.Where(x => x.TipoTension.ToUpper() == "TER" && x.Posicion == TerList[0]).FirstOrDefault()?.Tension.ToString(), out tension3);

                                    _positionWB = GetRowColOfWorbook(item.Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Value = BTList[i - 1];
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 1].Value = TerList[0];
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 2].Value = tension2;
                                    workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + 3].Value = tension3;
                                    for (int j = 0; j < 11; j++)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderBottom = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderRight = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderLeft = boder;
                                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1] + j].BorderTop = boder;

                                    }
                                }
                            }

                            AgregarValidacionesABT(ClavePrueba, reportsDTO, cantidadFilasABT, posicionMayor, ref workbook);
                        }
                    }
                }

                if (ClavePrueba == "ABT")
                {
                    posiMayor = posicionMayor;
                    filas = cantidadFilasABT;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AgregarValidacionesABT(string ClavePrueba, SettingsToDisplayISZReportsDTO reportsDTO, int filas, string posicionMayor, ref Workbook workbook)
        {
            int[] _positionWB = new int[] { };

            int[] array = new int[] { };

            int seccionRestante = 0;

            if (posicionMayor.ToUpper() == "AT")
            {
                array = new int[] { 1, 2 };
                seccionRestante = 3;
            }
            else if (posicionMayor.ToUpper() == "BT")
            {
                array = new int[] { 1, 3 };
                seccionRestante = 2;
            }
            else if (posicionMayor.ToUpper() == "TER")
            {
                array = new int[] { 2, 3 };
                seccionRestante = 1;
            }

            for (int i = 0; i < filas; i++)
            {
                foreach (int sec in array)
                {
                    string celdaLetra = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == sec && c.Dato.Equals("TensionVrms") && c.ClavePrueba == ClavePrueba)?.Celda[0].ToString();
                    int celdaNumber = Convert.ToInt32(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == sec && c.Dato.Equals("TensionVrms") && c.ClavePrueba == ClavePrueba)?.Celda.Substring(1, 2)) + i;
                    string val = "AND(REGEXP_MATCH(" + celdaLetra + celdaNumber.ToString() + ",@^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,1})?%?$@))";
                    val = val.Replace('@', '"');
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == sec && c.Dato.Equals("TensionVrms") && c.ClavePrueba == ClavePrueba).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "custom",
                        AllowNulls = false,
                        MessageTemplate = "El valor de la tensión vrms debe ser numérico mayor a cero considerando 6 enteros con 1 decimales",
                        From = val,
                        ComparerType = "custom",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true

                    };
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##,##0.0";

                    celdaLetra = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == sec && c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == ClavePrueba)?.Celda[0].ToString();
                    celdaNumber = Convert.ToInt32(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == sec && c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == ClavePrueba)?.Celda.Substring(1, 2)) + i;
                    val = "AND(REGEXP_MATCH(" + celdaLetra + celdaNumber.ToString() + ",@^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,3})?%?$@))"; val = val.Replace('@', '"');
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == sec && c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == ClavePrueba).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "custom",
                        AllowNulls = false,
                        MessageTemplate = "El valor de la corriente irms debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                        From = val,
                        ComparerType = "custom",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true

                    };
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##,##0.000";

                    celdaLetra = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == sec && c.Dato.Equals("PotenciaKW") && c.ClavePrueba == ClavePrueba)?.Celda[0].ToString();
                    celdaNumber = Convert.ToInt32(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == sec && c.Dato.Equals("PotenciaKW") && c.ClavePrueba == ClavePrueba)?.Celda.Substring(1, 2)) + i;
                    val = "AND(REGEXP_MATCH(" + celdaLetra + celdaNumber.ToString() + ",@^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,3})?%?$@))"; val = val.Replace('@', '"');
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == sec && c.Dato.Equals("PotenciaKW") && c.ClavePrueba == ClavePrueba).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "custom",
                        AllowNulls = false,
                        MessageTemplate = "El valor de la potencia KW debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                        From = val,
                        ComparerType = "custom",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true

                    };
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##,##0.000";
                }
            }

            string celdaLetra2 = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == seccionRestante && c.Dato.Equals("TensionVrms") && c.ClavePrueba == ClavePrueba)?.Celda[0].ToString();
            int celdaNumber2 = Convert.ToInt32(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == seccionRestante && c.Dato.Equals("TensionVrms") && c.ClavePrueba == ClavePrueba)?.Celda.Substring(1, 2));
            string val2 = "AND(REGEXP_MATCH(" + celdaLetra2 + celdaNumber2.ToString() + ",@^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,1})?%?$@))"; val2 = val2.Replace('@', '"');

            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == seccionRestante && c.Dato.Equals("TensionVrms") && c.ClavePrueba == ClavePrueba).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
            {
                DataType = "custom",
                AllowNulls = false,
                MessageTemplate = "El valor de la tensión vrms debe ser numérico mayor a cero considerando 6 enteros con 1 decimales",
                From = val2,
                ComparerType = "custom",
                Type = "reject",
                TitleTemplate = "Error",
                ShowButton = true

            };
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##,##0.0";

            celdaLetra2 = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == seccionRestante && c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == ClavePrueba)?.Celda[0].ToString();
            celdaNumber2 = Convert.ToInt32(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == seccionRestante && c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == ClavePrueba)?.Celda.Substring(1, 2));
            val2 = "AND(REGEXP_MATCH(" + celdaLetra2 + celdaNumber2.ToString() + ",@^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,3})?%?$@))"; val2 = val2.Replace('@', '"');

            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == seccionRestante && c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == ClavePrueba).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
            {
                DataType = "custom",
                AllowNulls = false,
                MessageTemplate = "El valor de la corriente irms debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                From = val2,
                ComparerType = "custom",
                Type = "reject",
                TitleTemplate = "Error",
                ShowButton = true

            };
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##,##0.000";

            celdaLetra2 = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == seccionRestante && c.Dato.Equals("PotenciaKW") && c.ClavePrueba == ClavePrueba)?.Celda[0].ToString();
            celdaNumber2 = Convert.ToInt32(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == seccionRestante && c.Dato.Equals("PotenciaKW") && c.ClavePrueba == ClavePrueba)?.Celda.Substring(1, 2));
            val2 = "AND(REGEXP_MATCH(" + celdaLetra2 + celdaNumber2.ToString() + ",@^[0-9]\\\\d{0,5}(\\\\.\\\\d{1,3})?%?$@))"; val2 = val2.Replace('@', '"');

            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Seccion == seccionRestante && c.Dato.Equals("PotenciaKW") && c.ClavePrueba == ClavePrueba).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
            {
                DataType = "custom",
                AllowNulls = false,
                MessageTemplate = "El valor de la potencia KW debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                From = val2,
                ComparerType = "custom",
                Type = "reject",
                TitleTemplate = "Error",
                ShowButton = true

            };
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##,##0.000";

        }

        #region Private Methods
        private void CopyColumn(Workbook origin, int[] position, ref Workbook official)
        {
            string cell = "NOM";
            int count = position[0];
            while (cell is not "" and not null)
            {
                cell = origin.Sheets[0].Rows[count].Cells[position[1]].Value?.ToString();
                if (cell is not "" and not null)
                    official.Sheets[0].Rows[count].Cells[position[1]].Value = cell;
                count++;
            }
        }
        private int[] GetRowColOfWorbook(string cell)
        {
            int[] position = new int[2];
            string row = string.Empty, col = string.Empty;

            for (int i = 0; i < cell.Length; i++)
            {
                if (char.IsDigit(cell[i]))
                {
                    col += cell[i];
                }
                else
                {
                    row += cell[i];
                }
            }

            position[0] = Convert.ToInt32(col);

            for (int i = 0; i < row.Length; i++)
            {
                position[1] += char.ToUpper(row[i]) - 64;
            }

            position[0] += -1;
            position[1] += -1;

            return position;
        }

        #endregion

    }
}
