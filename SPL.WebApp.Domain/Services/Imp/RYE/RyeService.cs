namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public class RyeService : IRyeService
    {
        #region Error message
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion

        public void PrepareTemplate_RYE(SettingsToDisplayRYEReportsDTO reportsDTO, ref Workbook workbook, decimal perdidasVacio, decimal perdidasEnf, decimal porcZ, decimal perdidasCarga)
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

                #region DataFill
                // Cooling Type
                _positionWB = this.GetRowColOfWorbook("D10");
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.CoolingType;

                // Cooling Capacity
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("CapacidadEnf")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.CoolingCapacity;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "###";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "999999",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"

                };

                // PorcZ
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PorcZ")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "#,##0.000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = porcZ;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
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

                // PerdidasVacio
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidasVacio")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "###,##0.000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = perdidasVacio;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "999999",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"

                };

                // PerdidasCarga
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidasCarga")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = perdidasCarga;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "###,##0.000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "999999",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"

                };

                // PerdidasEnf
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidasEnf")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = perdidasEnf;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "###,##0.000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = reportsDTO.CoolingType.Contains("ONAN") ? "greaterThanOrEqualTo" : "greaterThan",
                    From = "0",
                    To = "999999",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"

                };

                // PerdidasTotales
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidasTotales")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "###,##0.000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "999999",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"

                };
                #endregion

                #region DataCalculate
                // Porc_Z
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Porc_Z")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "#,##0.0000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                // PorcR
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PorcR")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "#,##0.0000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                // PorcX
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PorcX")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "#,##0.0000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                // XEntreR
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("XEntreR")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "#,##0.0000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                // Factor de Potencia
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("FactPotencia")).Celda);
                for (int i = 0; i < 7; i++)
                {
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Format = "##0.00";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].FontFamily = "Arial Unicode MS";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Value = 1 - (0.05 * i);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "greaterThan",
                        From = "0",
                        To = "999",
                        AllowNulls = false,
                        MessageTemplate = $"{messageErrorNumeric}",
                        Type = "reject",
                        TitleTemplate = "Error"

                    };

                }

                // Factor de Potencia
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fact_Potencia")).Celda);
                for (int i = 0; i < 7; i++)
                {
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Format = "##0.00";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].FontFamily = "Arial Unicode MS";
                }

                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Formula = "=D21";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Formula = "=E21";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Formula = "=F21";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Formula = "=G21";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4].Formula = "=H21";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5].Formula = "=I21";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 6].Formula = "=J21";

                // MVA
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PorcMVA")).Celda);
                for (int i = 0; i < 7; i++)
                {
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##0";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "greaterThan",
                        From = "0",
                        To = "999",
                        AllowNulls = false,
                        MessageTemplate = $"{messageErrorNumeric}",
                        Type = "reject",
                        TitleTemplate = "Error"

                    };
                }

                // PorcReg
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PorcReg")).Celda);
                for (int i = 0; i < 7; i++)
                {
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].Format = "#,##0.0000";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + i].FontFamily = "Arial Unicode MS";
                }

                // FACTOR DE POTENCIA
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Eficiencia")).Celda);
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 7; col++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + col].Format = "#,##0.0000";
                        workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + col].FontFamily = "Arial Unicode MS";
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool Verify_RYE_ColumnsToCalculate(SettingsToDisplayRYEReportsDTO reportsDTO, Workbook workbook)
        {
            IEnumerable<ConfigurationReportsDTO> starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("PorcZ") || c.Dato.Equals("PerdidasVacio") || c.Dato.Equals("PerdidasCarga") || c.Dato.Equals("PerdidasEnf") || c.Dato.Equals("PerdidasTotales"));
            int[] _positionWB;
            foreach (ConfigurationReportsDTO item in starts)
            {
                _positionWB = this.GetRowColOfWorbook(item.Celda);
                string cell = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
                if (cell is null or "")
                {
                    return false;
                }
            }

            return true;
        }

        public void Prepare_RYE_Test(SettingsToDisplayRYEReportsDTO reportsDTO, Workbook workbook, ref OutRYETestsDTO _ryeTestDTO)
        {
            int[] _positionWB;
            // CapacidadEnf
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("CapacidadEnf")).Celda);
            _ryeTestDTO.Capacity = decimal.Parse(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString(), NumberStyles.Float);

            // PorcZ
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PorcZ")).Celda);
            _ryeTestDTO.PorcZ = decimal.Parse(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString(), NumberStyles.Float);

            // PerdidasVacio
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidasVacio")).Celda);
            _ryeTestDTO.EmptyLosses = decimal.Parse(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString(), NumberStyles.Float);

            // PerdidasCarga
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidasCarga")).Celda);
            _ryeTestDTO.Lostload = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());

            // PerdidasEnf
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidasEnf")).Celda);
            _ryeTestDTO.LostCooldown = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());

            // PerdidasTotales
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidasTotales")).Celda);
            _ryeTestDTO.TotalLosses = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());

            // Factor de Potencia
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("FactPotencia")).Celda);
            _ryeTestDTO.FactPot1 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value);
            _ryeTestDTO.FactPot2 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Value);
            _ryeTestDTO.FactPot3 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Value);
            _ryeTestDTO.FactPot4 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Value);
            _ryeTestDTO.FactPot5 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4].Value);
            _ryeTestDTO.FactPot6 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5].Value);
            _ryeTestDTO.FactPot7 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 6].Value);

            // MVA
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PorcMVA")).Celda);
            _ryeTestDTO.RYETestsDetails.Add(new RYETestsDetailsDTO { PercentageMVA = Convert.ToInt32(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value) });
            _ryeTestDTO.RYETestsDetails.Add(new RYETestsDetailsDTO { PercentageMVA = Convert.ToInt32(workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].Value) });
            _ryeTestDTO.RYETestsDetails.Add(new RYETestsDetailsDTO { PercentageMVA = Convert.ToInt32(workbook.Sheets[0].Rows[_positionWB[0] + 2].Cells[_positionWB[1]].Value) });
            _ryeTestDTO.RYETestsDetails.Add(new RYETestsDetailsDTO { PercentageMVA = Convert.ToInt32(workbook.Sheets[0].Rows[_positionWB[0] + 3].Cells[_positionWB[1]].Value) });
            _ryeTestDTO.RYETestsDetails.Add(new RYETestsDetailsDTO { PercentageMVA = Convert.ToInt32(workbook.Sheets[0].Rows[_positionWB[0] + 4].Cells[_positionWB[1]].Value) });
            _ryeTestDTO.RYETestsDetails.Add(new RYETestsDetailsDTO { PercentageMVA = Convert.ToInt32(workbook.Sheets[0].Rows[_positionWB[0] + 5].Cells[_positionWB[1]].Value) });
            _ryeTestDTO.RYETestsDetails.Add(new RYETestsDetailsDTO { PercentageMVA = Convert.ToInt32(workbook.Sheets[0].Rows[_positionWB[0] + 6].Cells[_positionWB[1]].Value) });
            _ryeTestDTO.RYETestsDetails.Add(new RYETestsDetailsDTO { PercentageMVA = Convert.ToInt32(workbook.Sheets[0].Rows[_positionWB[0] + 7].Cells[_positionWB[1]].Value) });
            _ryeTestDTO.RYETestsDetails.Add(new RYETestsDetailsDTO { PercentageMVA = Convert.ToInt32(workbook.Sheets[0].Rows[_positionWB[0] + 8].Cells[_positionWB[1]].Value) });
        }

        public void PrepareIndexOfRYE(ResultRYETestsDTO resultRYETestsDTO, SettingsToDisplayRYEReportsDTO reportsDTO, ref Workbook workbook)
        {
            // Porc_Z
            int[] _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Porc_Z")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = Math.Round(resultRYETestsDTO.RYETests.PorcZ, 4).ToString("0.0000");

            // PorcR
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PorcR")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultRYETestsDTO.RYETests.PorcR.ToString();

            // PorcX
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PorcX")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultRYETestsDTO.RYETests.PorcX.ToString();

            // XEntreR
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("XEntreR")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultRYETestsDTO.RYETests.XIntoR.ToString();

            // PorcReg
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PorcReg")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultRYETestsDTO.RYETests.PorcReg1.ToString();
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Value = resultRYETestsDTO.RYETests.PorcReg2.ToString();
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Value = resultRYETestsDTO.RYETests.PorcReg3.ToString();
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Value = resultRYETestsDTO.RYETests.PorcReg4.ToString();
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4].Value = resultRYETestsDTO.RYETests.PorcReg5.ToString();
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5].Value = resultRYETestsDTO.RYETests.PorcReg6.ToString();
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 6].Value = resultRYETestsDTO.RYETests.PorcReg7.ToString();

            // FACTOR DE POTENCIA
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Eficiencia")).Celda);
            int row = 0;
            foreach (RYETestsDetailsDTO detail in resultRYETestsDTO.RYETests.RYETestsDetails)
            {
                workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1]].Value = detail.Efficiency1.ToString();
                workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 1].Value = detail.Efficiency2.ToString();
                workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 2].Value = detail.Efficiency3.ToString();
                workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 3].Value = detail.Efficiency4.ToString();
                workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 4].Value = detail.Efficiency5.ToString();
                workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 5].Value = detail.Efficiency6.ToString();
                workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 6].Value = detail.Efficiency7.ToString();
                row++;
            }
        }

        public DateTime GetDate(Workbook origin, SettingsToDisplayRYEReportsDTO reportsDTO)
        {
            ConfigurationReportsDTO fechaCelda = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha"));
            int[] _positionWB = this.GetRowColOfWorbook(fechaCelda.Celda);
            string strDate = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();
            DateTime fecha = DateTime.Today;

            if (double.TryParse(strDate, out double oaDate))
            {
                fecha = DateTime.FromOADate(oaDate);
            }
            else if (DateTime.TryParseExact(strDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime objDT))
            {
                fecha = objDT;
            }

            return fecha;
        }

        public bool Verify_RYE_Columns(SettingsToDisplayRYEReportsDTO reportsDTO, Workbook workbook)
        {
            IEnumerable<ConfigurationReportsDTO> starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("PorcZ") || c.Dato.Equals("PerdidasVacio") || c.Dato.Equals("PerdidasCarga") || c.Dato.Equals("PerdidasEnf") || c.Dato.Equals("PerdidasTotales") || c.Dato.Equals("Porc_Z") || c.Dato.Equals("PorcX") || c.Dato.Equals("PorcR") || c.Dato.Equals("XentreR"));
            int[] _positionWB;
            foreach (ConfigurationReportsDTO item in starts)
            {
                _positionWB = this.GetRowColOfWorbook(item.Celda);
                string cell = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
                if (cell is null or "")
                {
                    return false;
                }
            }

            return true;
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
                {
                    official.Sheets[0].Rows[count].Cells[position[1]].Value = cell;
                }
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
