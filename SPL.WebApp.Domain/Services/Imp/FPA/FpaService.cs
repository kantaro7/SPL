namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public class FpaService : IFpaService
    {
        #region Error message
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion

        public void PrepareTemplate_FPA(SettingsToDisplayFPAReportsDTO reportsDTO, ref Workbook workbook, bool incluirSegundaFila, string keyTests, string oilType)
        {
            try
            {
                #region Update Readonly all cells
                workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));
                #endregion

                #region Head
                int[] _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cliente")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.Client;

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoSerie")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.NoSerie;

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Capacidad")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.Capacity) ? string.Empty : $"{reportsDTO.HeadboardReport.Capacity} MVA";

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "MM/dd/yyyy";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "date",
                    AllowNulls = false,
                    MessageTemplate = $"Fecha es requerida y debe estar dentro del rango 1/1/1900 - {DateTime.Now.ToString("MM/dd/yyyy")}",
                    ComparerType = "between",
                    From = "DATEVALUE(\"1/1/1900\")",
                    To = $"DATEVALUE(\"{DateTime.Now.ToString("MM/dd/yyyy")}\")",
                    Type = "reject",
                    TitleTemplate = "Error",
                    ShowButton = true

                };
                #endregion

                #region Top
                // TipoAceite
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TipoAceite")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.OilType;

                // TempAmbiente
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TempAmbiente")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##0.0";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "999.9",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"
                };

                // HumedadRelativa
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("HumedadRelativa")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##0.0";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "999.9",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"
                };
                #endregion

                #region Seccion 1
                // Mediciones
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Medicion") && c.Seccion is 1).Celda);
                for (int i = 0; i < 2; i++)
                {
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] - 2].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "#0.000";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "greaterThan",
                        From = "0",
                        To = "99.999",
                        AllowNulls = false,
                        MessageTemplate = $"{messageErrorNumeric}",
                        Type = "reject",
                        TitleTemplate = "Error"
                    };
                }

                // Escala
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Escala")).Celda);
                for (int i = 0; i < 2; i++)
                {
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##0.00";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "greaterThan",
                        From = "0",
                        To = "999.99",
                        AllowNulls = false,
                        MessageTemplate = $"{messageErrorNumeric}",
                        Type = "reject",
                        TitleTemplate = "Error"
                    };
                }

                // FactorCorr
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("FactorCorr")).Celda);
                for (int i = 0; i < 2; i++)
                {
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##0.00";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "greaterThan",
                        From = "0",
                        To = "999.99",
                        AllowNulls = false,
                        MessageTemplate = $"{messageErrorNumeric}",
                        Type = "reject",
                        TitleTemplate = "Error"
                    };
                }
                
                // FactorPotencia
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("FactorPotencia")).Celda);
                for (int i = 0; i < 2; i++)
                {
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##0.000";
                }

                // LimiteMax
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("LimiteMax") && c.Seccion is 1).Celda);
                for (int i = 0; i < 2; i++)
                {
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##0.00";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "between",
                        From = "0",
                        To = "999.99",
                        AllowNulls = false,
                        MessageTemplate = $"{messageErrorNumeric}",
                        Type = "reject",
                        TitleTemplate = "Error"
                    };
                    if(oilType == "Sintético")
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = 0;
                    }
                    else if(i==0 && oilType == "Mineral")
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = 0.05;
                    }
                    else if (i == 1 && oilType == "Mineral")
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = 0.30;
                    }

                    else if (i == 0 && oilType == "Vegetal")
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = 0.20;
                    }
                    else if (i == 1 && oilType == "Vegetal")
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = 4.00;
                    }
                }
                #endregion

                #region Seccion 2
                // Valor_1 2 3 4 5
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valor_1") && c.Seccion is 2).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 2].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 3].Enable = true;

                for (int i = 0; i < 5; i++)
                {

                  
                        workbook.Sheets[0].Rows[_positionWB[0] ].Cells[_positionWB[1] + i].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] ].Cells[_positionWB[1] + i].Format = "##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] ].Cells[_positionWB[1] + i].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "0",
                            To = "999.9",
                            AllowNulls = false,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"
                        };

                    if (incluirSegundaFila)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0]+1].Cells[_positionWB[1] + i].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]+1].Cells[_positionWB[1] + i].Format = "##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0]+1].Cells[_positionWB[1] + i].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "0",
                            To = "999.9",
                            AllowNulls = false,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"
                        };
                    }
                    
                }

                // Promedio
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Promedio") && c.Seccion is 2).Celda);
                for (int i = 0; i < 2; i++)
                {
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##0.0";
                }

                // LimiteMax
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("LimiteMax") && c.Seccion is 2).Celda);
                for (int i = 0; i < 2; i++)
                {
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##0.00";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "between",
                        From = "0",
                        To = "999.99",
                        AllowNulls = false,
                        MessageTemplate = $"{messageErrorNumeric}",
                        Type = "reject",
                        TitleTemplate = "Error"
                    };

                    if (!incluirSegundaFila)
                    {
                        break;
                    }
                    if (oilType == "Sintético")
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = 0;
                    }
                    else if (i == 0 && oilType == "Mineral")
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = 55;
                    }
                    else if (i == 1 && oilType == "Mineral")
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = 65;
                    }
                    else if (i == 0 && oilType == "Vegetal")
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = "N/A";
                    }
                    else if (i == 1 && oilType == "Vegetal")
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = 56;
                    }
                }

                #endregion

                #region Seccion 3
                // Valor_1 2 3
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valor_1") && c.Seccion is 3).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 2].Enable = true;
                for (int i = 0; i < 6; i+= 2)
                {
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i + 1].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Format = "#0.0";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "greaterThan",
                        From = "0",
                        To = "99",
                        AllowNulls = false,
                        MessageTemplate = $"{messageErrorNumeric}",
                        Type = "reject",
                        TitleTemplate = "Error"
                    };
                }

                // Promedio
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Promedio") && c.Seccion is 3).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##0.0";

                // LimiteMax
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("LimiteMax") && c.Seccion is 3).Celda);
                workbook.Sheets[0].Rows[_positionWB[0] ].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0] ].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0] ].Cells[_positionWB[1]].Format = "##0.00";
                workbook.Sheets[0].Rows[_positionWB[0] ].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "between",
                    From = "0",
                    To = "999.99",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"
                };
                if (oilType == "Sintético")
                {
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = 0;
                }
                else if (oilType == "Mineral")
                {
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = 5;
                }
                else if (oilType == "Vegetal")
                {
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = 150;
                }

                #endregion


                #region Seccion 4


                // Medicion
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Medicion") && c.Seccion is 4).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Format = "##0.0";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "999.9",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"
                };
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "#0.00";
                    //if(reportsDTO.Measurement is not 0)
                    //    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.Measurement;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "greaterThan",
                        From = "0",
                        To = "99.99",
                        AllowNulls = false,
                        MessageTemplate = $"{messageErrorNumeric}",
                        Type = "reject",
                        TitleTemplate = "Error"
                    };


                if (keyTests.ToUpper().Equals("ADP"))
                {
                    // LimiteMax
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("LimiteMax") && c.Seccion is 4).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##0.00";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "between",
                        From = "0",
                        To = "999.99",
                        AllowNulls = false,
                        MessageTemplate = $"{messageErrorNumeric}",
                        Type = "reject",
                        TitleTemplate = "Error"
                    };
                    if (reportsDTO.PorcMaximumLimit is not 0)
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = 0.5;

                    if (oilType == "Sintético")
                    {
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = 0;
                    }
                    else if (oilType == "Mineral")
                    {
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = 0.5;
                    }
                    else if (oilType == "Vegetal")
                    {
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = 0.7;
                    }


                }




                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Notas")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].Enable = true;
                    }
                }
                #endregion
            }
            catch (Exception ex) {
                throw;
            }
        }

        public void Prepare_FPA_Test(SettingsToDisplayFPAReportsDTO reportsDTO, Workbook workbook, ref FPATestsDTO fpaTestsDTO, bool incluirSegundaFila)
        {
            int[] _positionWB;

            #region Top
            // TempAmbiente
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TempAmbiente")).Celda);
            fpaTestsDTO.AmbientTemperature = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString());

            // HumedadRelativa
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("HumedadRelativa")).Celda);
            fpaTestsDTO.RelativeHumidity = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString());
            #endregion

            #region Seccion 1
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion") && c.Seccion is 1).Celda);
            for (int i = 0; i < 2; i++)
            {
                fpaTestsDTO.FPATestsDetails.FPAPowerFactor[i].Temperature = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value.ToString();
                fpaTestsDTO.FPATestsDetails.FPAPowerFactor[i].ASMT = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value.ToString();
                fpaTestsDTO.FPATestsDetails.FPAPowerFactor[i].Measurement = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].Value.ToString());
                fpaTestsDTO.FPATestsDetails.FPAPowerFactor[i].Scale = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 7].Value.ToString());
                fpaTestsDTO.FPATestsDetails.FPAPowerFactor[i].CorrectionFactor = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 9].Value.ToString());
                fpaTestsDTO.FPATestsDetails.FPAPowerFactor[i].MaxLimitFP1 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 15].Value.ToString());
            }
            #endregion

            #region Seccion 2
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion") && c.Seccion is 2).Celda);

            fpaTestsDTO.FPATestsDetails.FPADielectricStrength[0].Electrodes = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();
            fpaTestsDTO.FPATestsDetails.FPADielectricStrength[0].OpeningMm = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4].Value.ToString());
            fpaTestsDTO.FPATestsDetails.FPADielectricStrength[0].ASTM = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 6].Value.ToString();
            fpaTestsDTO.FPATestsDetails.FPADielectricStrength[0].OneSt = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 8].Value.ToString());
            fpaTestsDTO.FPATestsDetails.FPADielectricStrength[0].TwoNd = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 9].Value.ToString());
            fpaTestsDTO.FPATestsDetails.FPADielectricStrength[0].ThreeRd = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 10].Value.ToString());
            fpaTestsDTO.FPATestsDetails.FPADielectricStrength[0].FourTh = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 11].Value.ToString());
            fpaTestsDTO.FPATestsDetails.FPADielectricStrength[0].FiveTh = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 12].Value.ToString());
            if(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 15].Value.ToString() == "N/A")
            {
                fpaTestsDTO.FPATestsDetails.FPADielectricStrength[0].MinLimit1 = 0;
            }
            else
            {
                fpaTestsDTO.FPATestsDetails.FPADielectricStrength[0].MinLimit1 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 15].Value.ToString());
            }

            if (incluirSegundaFila)
            {
                fpaTestsDTO.FPATestsDetails.FPADielectricStrength[1].Electrodes = workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].Value.ToString();
                fpaTestsDTO.FPATestsDetails.FPADielectricStrength[1].OpeningMm = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 4].Value.ToString());
                fpaTestsDTO.FPATestsDetails.FPADielectricStrength[1].ASTM = workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 6].Value.ToString();
                fpaTestsDTO.FPATestsDetails.FPADielectricStrength[1].OneSt = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 8].Value.ToString());
                fpaTestsDTO.FPATestsDetails.FPADielectricStrength[1].TwoNd = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 9].Value.ToString());
                fpaTestsDTO.FPATestsDetails.FPADielectricStrength[1].ThreeRd = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 10].Value.ToString());
                fpaTestsDTO.FPATestsDetails.FPADielectricStrength[1].FourTh = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 11].Value.ToString());
                fpaTestsDTO.FPATestsDetails.FPADielectricStrength[1].FiveTh = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 12].Value.ToString());
                fpaTestsDTO.FPATestsDetails.FPADielectricStrength[1].MinLimit1 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 15].Value.ToString());
            }
            else
            {
                fpaTestsDTO.FPATestsDetails.FPADielectricStrength.RemoveAt(1);
            }

            #endregion

            #region Seccion 3
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion") && c.Seccion is 3).Celda);
            fpaTestsDTO.FPATestsDetails.FPAWaterContent.Test = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();
            fpaTestsDTO.FPATestsDetails.FPAWaterContent.ASTM = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5].Value.ToString();
            fpaTestsDTO.FPATestsDetails.FPAWaterContent.OneSt = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 7].Value.ToString());
            fpaTestsDTO.FPATestsDetails.FPAWaterContent.TwoNd = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 9].Value.ToString());
            fpaTestsDTO.FPATestsDetails.FPAWaterContent.ThreeRd = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 11].Value.ToString());
            fpaTestsDTO.FPATestsDetails.FPAWaterContent.MaxLimit1 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 15].Value.ToString());
            #endregion

            #region Seccion 4
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Asmt") && c.Seccion is 4).Celda);
            fpaTestsDTO.FPATestsDetails.FPAGasContent.ASTM = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();
            fpaTestsDTO.FPATestsDetails.FPAGasContent.Measurement = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Value.ToString());

            if(reportsDTO.BaseTemplate.ClavePrueba == "DDP")
            {
                fpaTestsDTO.FPATestsDetails.FPAGasContent.Limit1 = 0;
            }
            else
            {
                fpaTestsDTO.FPATestsDetails.FPAGasContent.Limit1 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4]?.Value?.ToString());

            }
            #endregion
        }

        public void PrepareIndexOfFPA(ResultFPATestsDTO resultFPATestsDTO, SettingsToDisplayFPAReportsDTO reportsDTO, string idioma, ref Workbook workbook, bool incluirSegundaFila, string keyTests)
        {
            #region Seccion 1
            // FactorPotencia
            int[] _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("FactorPotencia")).Celda);
            for (int i = 0; i < 2; i++)
            {
                workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = resultFPATestsDTO.FPATests.FPATestsDetails.FPAPowerFactor[i].PowerFactor;
            }

            // Validacion
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Validacion") && c.Seccion is 1).Celda);
            for (int i = 0; i < 2; i++)
            {
                workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = resultFPATestsDTO.FPATests.FPATestsDetails.FPAPowerFactor[i].MaxLimitFP2;
            }
            #endregion

            #region Seccion 2
            // Promedio
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Promedio") && c.Seccion is 2).Celda);

            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultFPATestsDTO.FPATests.FPATestsDetails.FPADielectricStrength[0].Average;

            if (incluirSegundaFila)
            {
                workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].Value = resultFPATestsDTO.FPATests.FPATestsDetails.FPADielectricStrength[1].Average;
            }

            // Validacion
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Validacion") && c.Seccion is 2).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultFPATestsDTO.FPATests.FPATestsDetails.FPADielectricStrength[0].MinLimit2;

            if (incluirSegundaFila)
            {
                workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].Value = resultFPATestsDTO.FPATests.FPATestsDetails.FPADielectricStrength[1].MinLimit2;
            }

            #endregion

            #region Seccion 3

            // Promedio
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Promedio") && c.Seccion is 3).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultFPATestsDTO.FPATests.FPATestsDetails.FPAWaterContent.Average;

            // LimiteMax
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Validacion") && c.Seccion is 3).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultFPATestsDTO.FPATests.FPATestsDetails.FPAWaterContent.MaxLimit2;

            #endregion

            #region Seccion 4
            if (keyTests.ToUpper().Equals("ADP"))
            {
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Validacion") && c.Seccion is 4).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultFPATestsDTO.FPATests.FPATestsDetails.FPAGasContent.Limit2;
            }
            // LimiteMax
           
            #endregion
            
            int[] resultLocation = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
            bool resultReport = !resultFPATestsDTO.Results.Any();

            workbook.Sheets[0].Rows[resultLocation[0]].Cells[resultLocation[1]].Value = resultReport ?
                reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorAceptado{idioma}")).Formato :
                reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorRechazado{idioma}")).Formato;
        }

        public DateTime GetDate(SettingsToDisplayFPAReportsDTO reportsDTO, Workbook workbook)
        {
            int[] _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha")).Celda);
            string fecha = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
            return DateTime.TryParseExact(fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime objDT)
                ? objDT
                : DateTime.Now.Date;
        }

        public string GetGrades(SettingsToDisplayFPAReportsDTO reportsDTO, Workbook workbook)
        {
            int[] _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Notas")).Celda);
            string message = "";

            for (int i = 0; i < 2; i++)
            {
             

                    message += workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value?.ToString();
                
            }


            return message;
        }

        public bool Verify_FPA_Columns(SettingsToDisplayFPAReportsDTO reportsDTO, Workbook workbook, bool incluirSegundaFila, string keytests)
        {
            #region Top
            // TempAmbiente
            int[] _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TempAmbiente")).Celda);
            if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()))
            {
                return false;
            }

            // HumedadRelativa
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("HumedadRelativa")).Celda);
            if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()))
            {
                return false;
            }
            #endregion

            #region Seccion 1
            // Mediciones
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Medicion") && c.Seccion is 1).Celda);
            for (int i = 0; i < 2; i++)
            {
                if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString()))
                {
                    return false;
                }
            }

            // Escala
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Escala")).Celda);
            for (int i = 0; i < 2; i++)
            {
                if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString()))
                {
                    return false;
                }
            }

            // FactorCorr
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("FactorCorr")).Celda);
            for (int i = 0; i < 2; i++)
            {
                if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString()))
                {
                    return false;
                }
            }
            #endregion

            #region Seccion 2
           /* // Apertura
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Apertura")).Celda);
            for (int i = 0; i < 2; i++)
            {
                if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString()))
                {
                    return false;
                }
            }*/

            // Valor_1 2 3 4 5
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valor_1") && c.Seccion is 2).Celda);
            for (int i = 0; i < 5; i++)
            {
                if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i]?.Value?.ToString()))
                {
                    return false;
                }

                if (incluirSegundaFila)
                {
                    if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] +1 ].Cells[_positionWB[1] + i]?.Value?.ToString()))
                    {
                        return false;
                    }
                }

            }
            #endregion

            #region Seccion 3
            // Valor_1 2 3
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valor_1") && c.Seccion is 3).Celda);
            for (int i = 0; i < 6; i+= 2)
            {
                if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i]?.Value?.ToString()))
                {
                    return false;
                }
            }
            #endregion

            #region Seccion 4
            if (keytests.ToUpper().Equals("ADP"))
            {
                // Medicion
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Medicion") && c.Seccion is 4).Celda);
                if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()))
                {
                    return false;
                }
            }
            #endregion

            return true;
        }

        #region Private Methods
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
