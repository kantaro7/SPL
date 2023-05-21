namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public class RddService : IRddService
    {
        #region Error message
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion

        public void PrepareTemplate_RDD(SettingsToDisplayRDDReportsDTO reportsDTO, ref Workbook workbook,  string claveIdioma, PositionsDTO positions)
        {
            try
            {
                #region Update Readonly all cells
                workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));
                #endregion

                #region Head
                string devString = claveIdioma is "ES" ? "AT,BT" : "HV,LV";
                string atPositions = string.Join(',', positions.AltaTension);
                string btPositions = string.Join(',', positions.BajaTension);
                int[] _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cliente")).Celda);
                // [3, 2]
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
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitConexion")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.TitConexion;

                
                // CAP
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cap")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##,###,##0.000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "99999999.999",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"
                };

                // PosAT
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = positions.AltaTension.FirstOrDefault();
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "list",
                    ShowButton = true,
                    ComparerType = "list",
                    From = "\"" + atPositions + "\"",
                    AllowNulls = false,
                    Type = "reject"
                };
                // PosBT
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = positions.BajaTension.FirstOrDefault();
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "list",
                    ShowButton = true,
                    ComparerType = "list",
                    From = "\"" + btPositions + "\"",
                    AllowNulls = false,
                    Type = "reject"
                };
                // DevanadoEner
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DevanadoEner")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = claveIdioma is "ES" ? "AT" : "HV";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "list",
                    ShowButton = true,
                    ComparerType = "list",
                    From = "\"" + devString + "\"",
                    AllowNulls = false,
                    Type = "reject"
                };

                // TensionEner
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionEner")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "###,##0.000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "999999.999",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"
                };

                // DevanadoCorto
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DevanadoCorto")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = claveIdioma is "ES" ? "BT" : "LV";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "list",
                    ShowButton = true,
                    ComparerType = "list",
                    From = "\"" + devString + "\"",
                    AllowNulls = false,
                    Type = "reject"
                };

                // TensionCorto
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionCorto")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "###,##0.000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "999999.999",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"
                };
                #endregion

                #region DataFill
                // FASES
                string[] fases = reportsDTO.Phase.Split(',');
                IEnumerable<ConfigurationReportsDTO> lista = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fase"));
                foreach (ConfigurationReportsDTO item in lista)
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
                    for (int i = 0; i < 3; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = fases[i];
                    }
                }

                // Corriente
                lista = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Corriente"));
                foreach (ConfigurationReportsDTO item in lista)
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
                    for (int i = 0; i < 3; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "###,##0.000";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "0",
                            To = "999999.999",
                            AllowNulls = false,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"
                        };
                    }
                }

                // TensionAplicada
                lista = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TensionAplicada"));
                foreach (ConfigurationReportsDTO item in lista)
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
                    for (int i = 0; i < 3; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "###,##0.000";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "0",
                            To = "999999.999",
                            AllowNulls = false,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"
                        };
                    }
                }

                // Perdidas
                lista = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Perdidas"));
                foreach (ConfigurationReportsDTO item in lista)
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
                    for (int i = 0; i < 3; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "###,##0.000";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "0",
                            To = "999999.999",
                            AllowNulls = false,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"
                        };
                    }
                }

                // Porc_FP
                lista = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Porc_FP"));
                foreach (ConfigurationReportsDTO item in lista)
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
                    for (int i = 0; i < 3; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "###,##0.000";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "0",
                            To = "999999.999",
                            AllowNulls = false,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"
                        };
                    }
                }

                #endregion

                #region Columns
                // Resistencia
                lista = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Resistencia"));
                foreach (ConfigurationReportsDTO item in lista)
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
                    for (int i = 0; i < 3; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "###,##0.000";
                    }
                }

                // Impedancia
                lista = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Impedancia"));
                foreach (ConfigurationReportsDTO item in lista)
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
                    for (int i = 0; i < 3; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "###,##0.000";
                    }
                }

                // Reactancia
                lista = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Reactancia"));
                foreach (ConfigurationReportsDTO item in lista)
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
                    for (int i = 0; i < 3; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "###,##0.000";
                    }
                }

                // Porc_X
                lista = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Porc_X"));
                foreach (ConfigurationReportsDTO item in lista)
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
                    for (int i = 0; i < 3; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "###,##0.000";
                    }
                }
                #endregion
            }
            catch (Exception ex) {
                throw;
            }
        }

        public void Prepare_RDD_Test(SettingsToDisplayRDDReportsDTO reportsDTO, Workbook workbook, ref RDDTestsGeneralDTO rddTestsGeneralDTO)
        {
            int[] _positionWB;

            #region TopData
            // CAP
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cap")).Celda);
            rddTestsGeneralDTO.Capacity = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString());

            // PosAT
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT")).Celda);
            rddTestsGeneralDTO.PositionAT = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();

            // PosBT
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT")).Celda);
            rddTestsGeneralDTO.PositionBT = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();

            // DevanadoEner
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DevanadoEner")).Celda);
            rddTestsGeneralDTO.EnergizedWinding = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();

            // TensionEner
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionEner")).Celda);
            rddTestsGeneralDTO.VoltageEW = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString());

            // DevanadoCorto
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DevanadoCorto")).Celda);
            rddTestsGeneralDTO.ShortWinding = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();

            // TensionCorto
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionCorto")).Celda);
            rddTestsGeneralDTO.VoltageSW = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString());

            #endregion

            #region Data

            IEnumerable<ConfigurationReportsDTO> celdas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fase"));
            int section = 0;
            foreach (ConfigurationReportsDTO report in celdas)
            {
                _positionWB = this.GetRowColOfWorbook(report.Celda);
                for(int i = 0; i < 3; i++)
                {
                    rddTestsGeneralDTO.OutRDDTests[section].RDDTestsDetails.Add(new RDDTestsDetailsDTO()
                    {
                        Phase = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value.ToString(),
                        CurrentA = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Value.ToString()),
                        AppliedVoltage = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Value.ToString()),
                        LossesW = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value.ToString()),
                        PorcFp = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].Value.ToString())
                    });
                }
                section++;
            }
            #endregion
        }

        public void PrepareIndexOfRDD(ResultRDDTestsDTO resultRDDTestsDTO, SettingsToDisplayRDDReportsDTO reportsDTO, string idioma, ref Workbook workbook)
        {
            IEnumerable<ConfigurationReportsDTO> resistencia = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Resistencia"));
            int section = 0;
            foreach (ConfigurationReportsDTO item in resistencia)
            {
                int[] init = this.GetRowColOfWorbook(item.Celda);
                for (int i = 0; i < 3; i++)
                {
                    workbook.Sheets[0].Rows[init[0] + i].Cells[init[1]].Value = resultRDDTestsDTO.RDDTestsGeneral.OutRDDTests[section].RDDTestsDetails[i].Resistance;
                    workbook.Sheets[0].Rows[init[0] + i].Cells[init[1] + 1].Value = resultRDDTestsDTO.RDDTestsGeneral.OutRDDTests[section].RDDTestsDetails[i].Impedance;
                    workbook.Sheets[0].Rows[init[0] + i].Cells[init[1] + 2].Value = resultRDDTestsDTO.RDDTestsGeneral.OutRDDTests[section].RDDTestsDetails[i].Reactance;
                    workbook.Sheets[0].Rows[init[0] + i].Cells[init[1] + 3].Value = resultRDDTestsDTO.RDDTestsGeneral.OutRDDTests[section].RDDTestsDetails[i].PorcX;
                }
                section++;
            }

            int[] resultLocation = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
            bool resultReport = !resultRDDTestsDTO.Results.Any();

            workbook.Sheets[0].Rows[resultLocation[0]].Cells[resultLocation[1]].Value = resultReport ?
                reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorAceptado{idioma}")).Formato :
                reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorRechazado{idioma}")).Formato;
        }

        public DateTime GetDate(SettingsToDisplayRDDReportsDTO reportsDTO, Workbook workbook)
        {
            int[] _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha")).Celda);
            string fecha = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
            return DateTime.TryParseExact(fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime objDT)
                ? objDT
                : DateTime.Now.Date;
        }

        public string Verify_RDD_Columns(SettingsToDisplayRDDReportsDTO reportsDTO, Workbook workbook)
        {
            string result = string.Empty;
            // CAP
            int[] _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cap")).Celda);
            if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString()))
            {
                result += $" Falta Valor capacidad ({reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cap")).Celda}).";
            }

            // PosAT
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT")).Celda);
            if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString()))
            {
                result += $" Falta Valor posicion AT ({reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT")).Celda}).";
            }

            // PosBT
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT")).Celda);
            if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString()))
            {
                result += $" Falta Valor posicion BT ({reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT")).Celda}).";
            }

            // DevanadoEner
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DevanadoEner")).Celda);
            if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString()))
            {
                result += $" Falta Valor devanado energizado ({reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DevanadoEner")).Celda}).";
            }

            // TensionEner
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionEner")).Celda);
            if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString()))
            {
                result += $" Falta Valor tension energizado ({reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionEner")).Celda}).";
            }

            // DevanadoCorto
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DevanadoCorto")).Celda);
            if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString()))
            {
                result += $" Falta Valor devanado corto ({reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DevanadoCorto")).Celda}).";
            }

            // TensionCorto
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionCorto")).Celda);
            if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString()))
            {
                result += $" Falta Valor tension corto ({reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionCorto")).Celda}).";
            }

            IEnumerable<ConfigurationReportsDTO> starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fase"));
            foreach (ConfigurationReportsDTO column in starts)
            {
                _positionWB = this.GetRowColOfWorbook(column.Celda);
                for (int i = 0; i < 3; i++)
                {
                    if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value?.ToString()))
                    {
                        result += $" Falta Valor Fase ({column.Celda} la fila num {i+1}).";
                    }

                    if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Value?.ToString()))
                    {
                        result += $" Falta Valor Corriente ({column.Celda} + {i} columna la fila num {i + 2}).";

                    }

                    if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Value?.ToString()))
                    {
                        result += $" Falta Valor Tension Aplicada ({column.Celda} + {i} columna la fila num {i + 3}).";
                    }

                    if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value?.ToString()))
                    {
                        result += $" Falta Valor Perdidas ({column.Celda} + {i} columna la fila num {i + 4}).";
                    }

                    if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].Value?.ToString()))
                    {
                        result += $" Falta Valor %FP ({column.Celda} + {i} columna la fila num {i + 5}).";

                    }
                }
            }

            starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha"));

            foreach (ConfigurationReportsDTO column in starts)
            {
                _positionWB = this.GetRowColOfWorbook(column.Celda);
                if(string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString()))
                {
                    result += $" Falta Valor Fecha ({column.Celda}).";
                }
            }

            return result;
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
