namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;

    using Telerik.Web.Spreadsheet;

    public class CgdService : ICgdService
    {
        #region Error message
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion

        public void PrepareTemplate_CGD(SettingsToDisplayCGDReportsDTO reportsDTO, string typeTest, int hour1, int hour2, int hour3, ref Workbook workbook)
        {
            try
            {
                #region Update Readonly all cells
                workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));

                #endregion

                int[] _positionWB;

                #region Head

                foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Cliente")))
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.Client;
                }

                foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("NoSerie")))
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.NoSerie;
                }

                foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Capacidad")))
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.Capacity) ? string.Empty : $"{reportsDTO.HeadboardReport.Capacity} MVA";
                }

                

                foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Metodo")))
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Enable = true;
                    
                }

                foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha")))
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
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
                }

                #endregion

                if (TestType.DDT.ToString().Equals(typeTest)) 
                {
                    foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Notas")))
                    {
                        _positionWB = this.GetRowColOfWorbook(item.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 6].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 7].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 8].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 9].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 10].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 11].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 12].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 1].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 2].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 3].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 4].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 5].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 6].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 7].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 8].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 9].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 10].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 11].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 12].Enable = true;
                    }

                    int count = 0;
                    foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("HrsTemp")))
                    {
                        _positionWB = this.GetRowColOfWorbook(item.Celda);
                        if (count is 0 && hour1 is not 0)
                        {
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = hour1.ToString();
                        }

                        if (count is 1 && hour2 is not 0)
                        {
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = hour2.ToString();
                        }

                        if (count is 2 && hour3 is not 0)
                        {
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = hour3.ToString();
                        }
                        count++;
                    }

                    foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("AntesPpm_1")))
                    {
                        _positionWB = this.GetRowColOfWorbook(item.Celda);
                        for (int i = 0; i < 9; i++)
                        {
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##,###,##0.0";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].FontFamily = "Arial Unicode MS";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Format = "##,###,##0.0";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Enable = true;
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].FontFamily = "Arial Unicode MS";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Format = "##,###,##0.0";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Enable = true;
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                            {
                                DataType = "number",
                                ComparerType = "greaterThan",
                                From = "-1",
                                To = "99999999",
                                AllowNulls = false,
                                MessageTemplate = $"{messageErrorNumeric}",
                                Type = "reject",
                                TitleTemplate = "Error"
                            };
                        }
                    }

                    foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("DespuesPpm_1")))
                    {
                        _positionWB = this.GetRowColOfWorbook(item.Celda);
                        for (int i = 0; i < 9; i++)
                        {
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##,###,##0.0";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].FontFamily = "Arial Unicode MS";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Format = "##,###,##0.0";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Enable = true;
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].FontFamily = "Arial Unicode MS";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Format = "##,###,##0.0";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Enable = true;
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                            {
                                DataType = "number",
                                ComparerType = "greaterThan",
                                From = "-1",
                                To = "99999999",
                                AllowNulls = false,
                                MessageTemplate = $"{messageErrorNumeric}",
                                Type = "reject",
                                TitleTemplate = "Error"
                            };

                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].FontFamily = "Arial Unicode MS";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Format = "##,###,##0.0";
                        }
                    }

                    foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Result")))
                    {
                        _positionWB = this.GetRowColOfWorbook(item.Celda);
                        for (int i = 0; i < 4; i++)
                        {
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##0.0";

                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].FontFamily = "Arial Unicode MS";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Format = "##0.0";
                            
                        }
                    }

                    foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("LimiteMax")))
                    {
                        _positionWB = this.GetRowColOfWorbook(item.Celda);
                        for (int i = 0; i < 5; i++)
                        {
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##0.0";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                            {
                                DataType = "number",
                                ComparerType = "greaterThan",
                                From = "-1",
                                To = "999",
                                AllowNulls = false,
                                MessageTemplate = $"{messageErrorNumeric}",
                                Type = "reject",
                                TitleTemplate = "Error"
                            };
                        }
                    }
                }
                else if (TestType.ADP.ToString().Equals(typeTest))
                {
                    foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Notas")))
                    {
                        _positionWB = this.GetRowColOfWorbook(item.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 6].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 7].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 8].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 1].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 2].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 3].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 4].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 5].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 6].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 7].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 8].Enable = true;
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ResultadoPpm_1")).Celda);
                    for (int i = 0; i < 9; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##,###,##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "-1",
                            To = "99999999",
                            AllowNulls = false,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"
                        };

                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Format = "##,###,##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Enable = false;
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ResultadoPpm_2")).Celda);
                    for (int i = 0; i < 4; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = i == 0 ? "###,##0" : "###,##0.00";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = false;

                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AceptacionPpm")).Celda);
                    for (int i = 0; i < 4; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = i is 1 or 2 ? "###,##0" : "###,##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "-1",
                            To = "999999",
                            AllowNulls = false,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"
                        };


                    }
                }
                else if (TestType.DDD.ToString().Equals(typeTest))
                {
                    foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Notas")))
                    {
                        _positionWB = this.GetRowColOfWorbook(item.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 6].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 7].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 8].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 9].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 10].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 11].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 12].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 1].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 2].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 3].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 4].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 5].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 6].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 7].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 8].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 9].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 10].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 11].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 12].Enable = true;
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AntesPpm_1")).Celda);
                    for (int i = 0; i < 9; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##,###,##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Format = "##,###,##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Format = "##,###,##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "-1",
                            To = "99999999",
                            AllowNulls = false,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"
                        };

                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Format = "##,###,##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].Format = "##,###,##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].Format = "##,###,##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "-1",
                            To = "99999999",
                            AllowNulls = false,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"
                        };
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("LimiteMax")).Celda);
                    for (int i = 0; i < 5; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
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
                }
                // DDE y DSC
                else
                {
                    foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Notas")))
                    {
                        _positionWB = this.GetRowColOfWorbook(item.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 6].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 7].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 8].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 9].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 10].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 11].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 12].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 1].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 2].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 3].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 4].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 5].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 6].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 7].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 8].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 9].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 10].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 11].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1] + 12].Enable = true;
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AntesPpm_1")).Celda);
                    for (int i = 0; i < 9; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "##,###,##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Format = "##,###,##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Format = "##,###,##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "-1",
                            To = "99999999",
                            AllowNulls = false,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"
                        };

                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Format = "##,###,##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].Format = "##,###,##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].Format = "##,###,##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "-1",
                            To = "99999999",
                            AllowNulls = false,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"
                        };

                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 6].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 6].Format = "##,###,##0.0";
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AntesPpm_2")).Celda);
                    for (int i = 0; i < 4; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = i == 0 ? "###,##0" : "###,##0.00";

                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Format = i == 0 ? "###,##0" : "###,##0.00";
                    }
                }
            }
            catch (Exception ex) {
                throw;
            }
        }

        public bool Verify_CGD_ColumnsToCalculate(SettingsToDisplayCGDReportsDTO reportsDTO, Workbook workbook, string typeTest)
        {
            int[] _positionWB;

            foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Metodo")))
            {
                _positionWB = this.GetRowColOfWorbook(item.Celda);
                if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString()))
                {
                    return false;
                }
            }

            if (TestType.DDT.ToString().Equals(typeTest))
            {
                foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("AntesPpm_1")))
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
                    for (int i = 0; i < 9; i++)
                    {
                        if(string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value?.ToString()))
                        {
                            return false;
                        }
                    }
                }

                foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("DespuesPpm_1")))
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
                    for (int i = 0; i < 9; i++)
                    {
                        if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value?.ToString()))
                        {
                            return false;
                        }
                    }
                }
            }
            else if (TestType.ADP.ToString().Equals(typeTest))
            {
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ResultadoPpm_1")).Celda);
                for (int i = 0; i < 9; i++)
                {
                    if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value?.ToString()))
                    {
                        return false;
                    }
                }
            }
            else if (TestType.DDD.ToString().Equals(typeTest))
            {
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AntesPpm_1")).Celda);
                for (int i = 0; i < 9; i++)
                {
                    if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value?.ToString()))
                    {
                        return false;
                    }
                    if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value?.ToString()))
                    {
                        return false;
                    }
                }

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("LimiteMax")).Celda);
                for (int i = 0; i < 2; i++)
                {
                    if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value?.ToString()))
                    {
                        return false;
                    }
                }
            }
            // DDE y DSC
            else
            {
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AntesPpm_1")).Celda);
                for (int i = 0; i < 9; i++)
                {
                    if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value?.ToString()))
                    {
                        return false;
                    }
                    if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value?.ToString()))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void Prepare_CGD_Test(SettingsToDisplayCGDReportsDTO reportsDTO, Workbook workbook, string typeTest, string lenguage, ref List<CGDTestsDTO> _cgdTestsDTOs)
        {
            int[] _positionWB;
            int x = 0;
            foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Metodo")))
            {
                _positionWB = this.GetRowColOfWorbook(item.Celda);
                _cgdTestsDTOs[x].Method = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();
                x++;
            }
            x = 0;



      
            



            foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Notas")))
            {
                _positionWB = this.GetRowColOfWorbook(item.Celda);
                string message = "";

                for (int i = 0; i < 2; i++)
                {


                    message += workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value?.ToString();

                }
                _cgdTestsDTOs[x].Grades = message;
                x++;
            }

            if (TestType.DDT.ToString().Equals(typeTest))
            {
                int count = 0;
                List<ConfigurationReportsDTO> start = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("AntesPpm_1")).ToList();
                List<ConfigurationReportsDTO> start2 = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("LimiteMax")).ToList();

                foreach (CGDTestsDTO test in _cgdTestsDTOs)
                {
                    _positionWB = this.GetRowColOfWorbook(start[count].Celda);
                    for (int i = 0; i < 9; i++)
                    {
                        string col = "";
                        switch (i)
                        {
                            case 0:
                                col = CGDKeys.Hydrogen;
                                break;
                            case 1:
                                col = CGDKeys.Oxygen;
                                break;
                            case 2:
                                col = CGDKeys.Nitrogen;
                                break;
                            case 3:
                                col = CGDKeys.Methane;
                                break;
                            case 4:
                                col = CGDKeys.CarbonMonoxide;
                                break;
                            case 5:
                                col = CGDKeys.CarbonDioxide;
                                break;
                            case 6:
                                col = CGDKeys.Ethylene;
                                break;
                            case 7:
                                col = CGDKeys.Ethane;
                                break;
                            case 8:
                                col = CGDKeys.Acetylene;
                                break;
                            default:
                                break;
                        }
                        test.CGDTestsDetails.Add(new CGDTestsDetailsDTO { 
                            Key = col,
                            Value1 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value.ToString()),
                            Value2 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value.ToString())
                        });
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        string col = "";
                        switch (i)
                        {
                            case 0:
                                col = CGDKeys.TotalGases;
                                break;
                            case 1:
                                col = CGDKeys.CombustibleGases;
                                break;
                            case 2:
                                col = CGDKeys.PorcCombustibleGas;
                                break;
                            case 3:
                                col = CGDKeys.PorcGasContent;
                                break;
                            default:
                                break;
                        }

                        test.CGDTestsDetails.Add(new CGDTestsDetailsDTO
                        {
                            Key = col
                        });
                    }
                    _positionWB = this.GetRowColOfWorbook(start2[count].Celda);
                    for (int i = 0; i < 5; i++)
                    {
                        string col = "";
                        switch (i)
                        {
                            case 0:
                                col = CGDKeys.AcetyleneTest;
                                break;
                            case 1:
                                col = CGDKeys.HydrogenTest;
                                break;
                            case 2:
                                col = CGDKeys.MethaneEthyleneEthaneTest;
                                break;
                            case 3:
                                col = CGDKeys.CarbonMonoxideTest;
                                break;
                            case 4:
                                col = CGDKeys.CarbonDioxideTest;
                                break;
                            default:
                                break;
                        }

                        test.CGDTestsDetails.Add(new CGDTestsDetailsDTO
                        {
                            Key = col,
                            Value3 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value.ToString())
                        });
                    }
                    count++;
                }
            }
            else if (TestType.ADP.ToString().Equals(typeTest))
            {
                ConfigurationReportsDTO start = reportsDTO.ConfigurationReports.First(c => c.Dato.Equals("ResultadoPpm_1"));
                _positionWB = this.GetRowColOfWorbook(start.Celda);
                for (int i = 0; i < 9; i++)
                {
                    string col = "";
                    switch (i)
                    {
                        case 0:
                            col = CGDKeys.Hydrogen;
                            break;
                        case 1:
                            col = CGDKeys.Oxygen;
                            break;
                        case 2:
                            col = CGDKeys.Nitrogen;
                            break;
                        case 3:
                            col = CGDKeys.Methane;
                            break;
                        case 4:
                            col = CGDKeys.CarbonMonoxide;
                            break;
                        case 5:
                            col = CGDKeys.CarbonDioxide;
                            break;
                        case 6:
                            col = CGDKeys.Ethylene;
                            break;
                        case 7:
                            col = CGDKeys.Ethane;
                            break;
                        case 8:
                            col = CGDKeys.Acetylene;
                            break;
                        default:
                            break;
                    }

                    _cgdTestsDTOs[0].CGDTestsDetails.Add(new CGDTestsDetailsDTO
                    {
                        Key = col,
                        Value1 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value),
                        Value2 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Value)
                    });
                }

                for (int i = 0; i < 4; i++)
                {
                    string col = "";
                    switch (i)
                    {
                        case 0:
                            col = CGDKeys.TotalGases;
                            break;
                        case 1:
                            col = CGDKeys.CombustibleGases;
                            break;
                        case 2:
                            col = CGDKeys.PorcCombustibleGas;
                            break;
                        case 3:
                            col = CGDKeys.PorcGasContent;
                            break;
                        default:
                            break;
                    }

                    _cgdTestsDTOs[0].CGDTestsDetails.Add(new CGDTestsDetailsDTO
                    {
                        Key = col
                    });
                }
            }
            else if (TestType.DDD.ToString().Equals(typeTest))
            {
                ConfigurationReportsDTO start = reportsDTO.ConfigurationReports.First(c => c.Dato.Equals("AntesPpm_1"));
                _positionWB = this.GetRowColOfWorbook(start.Celda);
                for (int i = 0; i < 9; i++)
                {
                    string col = "";
                    switch (i)
                    {
                        case 0:
                            col = CGDKeys.Hydrogen;
                            break;
                        case 1:
                            col = CGDKeys.Oxygen;
                            break;
                        case 2:
                            col = CGDKeys.Nitrogen;
                            break;
                        case 3:
                            col = CGDKeys.Methane;
                            break;
                        case 4:
                            col = CGDKeys.CarbonMonoxide;
                            break;
                        case 5:
                            col = CGDKeys.CarbonDioxide;
                            break;
                        case 6:
                            col = CGDKeys.Ethylene;
                            break;
                        case 7:
                            col = CGDKeys.Ethane;
                            break;
                        case 8:
                            col = CGDKeys.Acetylene;
                            break;
                        default:
                            break;
                    }

                    _cgdTestsDTOs[0].CGDTestsDetails.Add(new CGDTestsDetailsDTO
                    {
                        Key = col,
                        Value1 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value),
                        Value2 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value)
                    });
                }

                for (int i = 0; i < 4; i++)
                {
                    string col = "";
                    switch (i)
                    {
                        case 0:
                            col = CGDKeys.TotalGases;
                            break;
                        case 1:
                            col = CGDKeys.CombustibleGases;
                            break;
                        case 2:
                            col = CGDKeys.PorcCombustibleGas;
                            break;
                        case 3:
                            col = CGDKeys.PorcGasContent;
                            break;
                        default:
                            break;
                    }

                    _cgdTestsDTOs[0].CGDTestsDetails.Add(new CGDTestsDetailsDTO
                    {
                        Key = col
                    });
                }

                for (int i = 0; i < 2; i++)
                {
                    string col = "";
                    switch (i)
                    {
                        case 0:
                            col = CGDKeys.AcetyleneTest;
                            break;
                        case 1:
                            col = CGDKeys.HydrogenTest;
                            break;
                        default:
                            break;
                    }

                    _cgdTestsDTOs[0].CGDTestsDetails.Add(new CGDTestsDetailsDTO
                    {
                        Key = col
                    });
                }
            }
            // DDE y DSC
            else
            {
                ConfigurationReportsDTO start = reportsDTO.ConfigurationReports.First(c => c.Dato.Equals("AntesPpm_1"));
                _positionWB = this.GetRowColOfWorbook(start.Celda);
                for (int i = 0; i < 9; i++)
                {
                    string col = "";
                    switch (i)
                    {
                        case 0:
                            col = CGDKeys.Hydrogen;
                            break;
                        case 1:
                            col = CGDKeys.Oxygen;
                            break;
                        case 2:
                            col = CGDKeys.Nitrogen;
                            break;
                        case 3:
                            col = CGDKeys.Methane;
                            break;
                        case 4:
                            col = CGDKeys.CarbonMonoxide;
                            break;
                        case 5:
                            col = CGDKeys.CarbonDioxide;
                            break;
                        case 6:
                            col = CGDKeys.Ethylene;
                            break;
                        case 7:
                            col = CGDKeys.Ethane;
                            break;
                        case 8:
                            col = CGDKeys.Acetylene;
                            break;
                        default:
                            break;
                    }

                    _cgdTestsDTOs[0].CGDTestsDetails.Add(new CGDTestsDetailsDTO
                    {
                        Key = col,
                        Value1 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value),
                        Value2 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value)
                    });
                }

                for (int i = 0; i < 4; i++)
                {
                    string col = "";
                    switch (i)
                    {
                        case 0:
                            col = CGDKeys.TotalGases;
                            break;
                        case 1:
                            col = CGDKeys.CombustibleGases;
                            break;
                        case 2:
                            col = CGDKeys.PorcCombustibleGas;
                            break;
                        case 3:
                            col = CGDKeys.PorcGasContent;
                            break;
                        default:
                            break;
                    }

                    _cgdTestsDTOs[0].CGDTestsDetails.Add(new CGDTestsDetailsDTO
                    {
                        Key = col
                    });
                }
            }
        }

        public void PrepareIndexOfCGD(ResultCGDTestsDTO resultCGDTestsDTO, SettingsToDisplayCGDReportsDTO reportsDTO, ref Workbook workbook, string idioma)
        {
            int[] _positionWB;

            if (TestType.DDT.ToString().Equals(resultCGDTestsDTO.CGDTests[0].KeyTest))
            {
                int section = 0;
                List<ConfigurationReportsDTO> IncrementoPpm = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("IncrementoPpm")).ToList();
                List<ConfigurationReportsDTO> AntesPpm_2 = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("AntesPpm_2")).ToList();

                List<ConfigurationReportsDTO> ResultAbajo = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Result")).ToList();
                List<ConfigurationReportsDTO> ValidacionAbajo = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Validacion")).ToList();
                foreach (CGDTestsDTO test in resultCGDTestsDTO.CGDTests)
                {
                    _positionWB = this.GetRowColOfWorbook(IncrementoPpm[section].Celda);
                    for (int i = 0; i < 9; i++)
                    {
                        string col = "";
                        switch (i)
                        {
                            case 0:
                                col = CGDKeys.Hydrogen;
                                break;
                            case 1:
                                col = CGDKeys.Oxygen;
                                break;
                            case 2:
                                col = CGDKeys.Nitrogen;
                                break;
                            case 3:
                                col = CGDKeys.Methane;
                                break;
                            case 4:
                                col = CGDKeys.CarbonMonoxide;
                                break;
                            case 5:
                                col = CGDKeys.CarbonDioxide;
                                break;
                            case 6:
                                col = CGDKeys.Ethylene;
                                break;
                            case 7:
                                col = CGDKeys.Ethane;
                                break;
                            case 8:
                                col = CGDKeys.Acetylene;
                                break;
                            default:
                                break;
                        }
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = test.CGDTestsDetails.FirstOrDefault(x => x.Key.Equals(col)).Value3;
                    }

                    _positionWB = this.GetRowColOfWorbook(AntesPpm_2[section].Celda);
                    for (int i = 0; i < 4; i++)
                    {
                        string col = "";
                        switch (i)
                        {
                            case 0:
                                col = CGDKeys.TotalGases;
                                break;
                            case 1:
                                col = CGDKeys.CombustibleGases;
                                break;
                            case 2:
                                col = CGDKeys.PorcCombustibleGas;
                                break;
                            case 3:
                                col = CGDKeys.PorcGasContent;
                                break;
                            default:
                                break;
                        }
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = test.CGDTestsDetails.FirstOrDefault(x => x.Key.Equals(col)).Value1;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value = test.CGDTestsDetails.FirstOrDefault(x => x.Key.Equals(col)).Value2;
                    }

                    _positionWB = this.GetRowColOfWorbook(ResultAbajo[section].Celda);
                    for (int i = 0; i < 5; i++)
                    {
                        string col = "";
                        switch (i)
                        {
                            case 0:
                                col = CGDKeys.AcetyleneTest;
                                break;
                            case 1:
                                col = CGDKeys.HydrogenTest;
                                break;
                            case 2:
                                col = CGDKeys.MethaneEthyleneEthaneTest;
                                break;
                            case 3:
                                col = CGDKeys.CarbonMonoxideTest;
                                break;
                            case 4:
                                col = CGDKeys.CarbonDioxideTest;
                                break;
                            default:
                                break;
                        }
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = test.CGDTestsDetails.FirstOrDefault(x => x.Key.Equals(col)).Value2;
                        if (idioma is "ES")
                        {
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value = resultCGDTestsDTO.CGDTests[0].CGDTestsDetails.FirstOrDefault(x => x.Key.Equals(col)).Value4.ToString() == "1" ? "Aceptado" : "Rechazado";
                        }
                        else
                        {
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value = resultCGDTestsDTO.CGDTests[0].CGDTestsDetails.FirstOrDefault(x => x.Key.Equals(col)).Value4.ToString() == "1" ? "Accepted" : "Rejected";
                        }
                    }
                    section++;
                }
            }
            else if (TestType.ADP.ToString().Equals(resultCGDTestsDTO.CGDTests[0].KeyTest))
            {
                ConfigurationReportsDTO start = reportsDTO.ConfigurationReports.First(c => c.Dato.Equals("ResultadoPpm_2"));
                _positionWB = this.GetRowColOfWorbook(start.Celda);
                for (int i = 0; i < 4; i++)
                {
                    string col = "";
                    switch (i)
                    {
                        case 0:
                            col = CGDKeys.TotalGases;
                            break;
                        case 1:
                            col = CGDKeys.CombustibleGases;
                            break;
                        case 2:
                            col = CGDKeys.PorcCombustibleGas;
                            break;
                        case 3:
                            col = CGDKeys.PorcGasContent;
                            break;
                        default:
                            break;
                    }
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = resultCGDTestsDTO.CGDTests[0].CGDTestsDetails.FirstOrDefault(x => x.Key.Equals(col)).Value1;
                }
            }
            else if (TestType.DDD.ToString().Equals(resultCGDTestsDTO.CGDTests[0].KeyTest))
            {
                ConfigurationReportsDTO start = reportsDTO.ConfigurationReports.First(c => c.Dato.Equals("IncrementoPpm"));
                _positionWB = this.GetRowColOfWorbook(start.Celda);
                for (int i = 0; i < 9; i++)
                {
                    string col = "";
                    switch (i)
                    {
                        case 0:
                            col = CGDKeys.Hydrogen;
                            break;
                        case 1:
                            col = CGDKeys.Oxygen;
                            break;
                        case 2:
                            col = CGDKeys.Nitrogen;
                            break;
                        case 3:
                            col = CGDKeys.Methane;
                            break;
                        case 4:
                            col = CGDKeys.CarbonMonoxide;
                            break;
                        case 5:
                            col = CGDKeys.CarbonDioxide;
                            break;
                        case 6:
                            col = CGDKeys.Ethylene;
                            break;
                        case 7:
                            col = CGDKeys.Ethane;
                            break;
                        case 8:
                            col = CGDKeys.Acetylene;
                            break;
                        default:
                            break;
                    }

                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = resultCGDTestsDTO.CGDTests[0].CGDTestsDetails.FirstOrDefault(x => x.Key.Equals(col)).Value1;
                }

                start = reportsDTO.ConfigurationReports.First(c => c.Dato.Equals("AntesPpm_2"));
                _positionWB = this.GetRowColOfWorbook(start.Celda);
                for (int i = 0; i < 4; i++)
                {
                    string col = "";
                    switch (i)
                    {
                        case 0:
                            col = CGDKeys.TotalGases;
                            break;
                        case 1:
                            col = CGDKeys.CombustibleGases;
                            break;
                        case 2:
                            col = CGDKeys.PorcCombustibleGas;
                            break;
                        case 3:
                            col = CGDKeys.PorcGasContent;
                            break;
                        default:
                            break;
                    }
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = resultCGDTestsDTO.CGDTests[0].CGDTestsDetails.FirstOrDefault(x => x.Key.Equals(col)).Value1;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value = resultCGDTestsDTO.CGDTests[0].CGDTestsDetails.FirstOrDefault(x => x.Key.Equals(col)).Value2;
                }
                start = reportsDTO.ConfigurationReports.First(c => c.Dato.Equals("Result"));
                _positionWB = this.GetRowColOfWorbook(start.Celda);
                for (int i = 0; i < 2; i++)
                {
                    string col = "";
                    switch (i)
                    {
                        case 0:
                            col = CGDKeys.AcetyleneTest;
                            break;
                        case 1:
                            col = CGDKeys.HydrogenTest;
                            break;
                        default:
                            break;
                    }
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = resultCGDTestsDTO.CGDTests[0].CGDTestsDetails.FirstOrDefault(x => x.Key.Equals(col)).Value2;

                    if(idioma is "ES")
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value = resultCGDTestsDTO.CGDTests[0].CGDTestsDetails.FirstOrDefault(x => x.Key.Equals(col)).Value4.ToString() == "1" ? "Aceptado" : "Rechazado";
                    }
                    else
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value = resultCGDTestsDTO.CGDTests[0].CGDTestsDetails.FirstOrDefault(x => x.Key.Equals(col)).Value4.ToString() == "1" ? "Accepted" : "Rejected";
                    }
                }
            }
            // DDE y DSC
            else
            {
                ConfigurationReportsDTO start = reportsDTO.ConfigurationReports.First(c => c.Dato.Equals("IncrementoPpm"));
                _positionWB = this.GetRowColOfWorbook(start.Celda);
                for (int i = 0; i < 9; i++)
                {
                    string col = "";
                    switch (i)
                    {
                        case 0:
                            col = CGDKeys.Hydrogen;
                            break;
                        case 1:
                            col = CGDKeys.Oxygen;
                            break;
                        case 2:
                            col = CGDKeys.Nitrogen;
                            break;
                        case 3:
                            col = CGDKeys.Methane;
                            break;
                        case 4:
                            col = CGDKeys.CarbonMonoxide;
                            break;
                        case 5:
                            col = CGDKeys.CarbonDioxide;
                            break;
                        case 6:
                            col = CGDKeys.Ethylene;
                            break;
                        case 7:
                            col = CGDKeys.Ethane;
                            break;
                        case 8:
                            col = CGDKeys.Acetylene;
                            break;
                        default:
                            break;
                    }

                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = resultCGDTestsDTO.CGDTests[0].CGDTestsDetails.FirstOrDefault(x => x.Key.Equals(col)).Value3;
                }

                start = reportsDTO.ConfigurationReports.First(c => c.Dato.Equals("AntesPpm_2"));
                _positionWB = this.GetRowColOfWorbook(start.Celda);
                for (int i = 0; i < 4; i++)
                {
                    string col = "";
                    switch (i)
                    {
                        case 0:
                            col = CGDKeys.TotalGases;
                            break;
                        case 1:
                            col = CGDKeys.CombustibleGases;
                            break;
                        case 2:
                            col = CGDKeys.PorcCombustibleGas;
                            break;
                        case 3:
                            col = CGDKeys.PorcGasContent;
                            break;
                        default:
                            break;
                    }
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = resultCGDTestsDTO.CGDTests[0].CGDTestsDetails.FirstOrDefault(x => x.Key.Equals(col)).Value1;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value = resultCGDTestsDTO.CGDTests[0].CGDTestsDetails.FirstOrDefault(x => x.Key.Equals(col)).Value2;
                }
            }
            int seccion = 1;
            foreach (ConfigurationReportsDTO item in reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Resultado")))
            {
                _positionWB = this.GetRowColOfWorbook(item.Celda);
                bool resultReport = !resultCGDTestsDTO.Results.Where(x => x.Column == seccion).Any();

                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultReport ?
                    reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorAceptado{idioma}")).Formato :
                    reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorRechazado{idioma}")).Formato;
                seccion++;
            }
        }

        public DateTime[] GetDate(Workbook origin, SettingsToDisplayCGDReportsDTO reportsDTO)
        {
            IEnumerable<ConfigurationReportsDTO> fechaCelda = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha"));
            DateTime[] result = new DateTime[3];
            int count = 0;
            foreach (ConfigurationReportsDTO item in fechaCelda)
            {
                int[] _positionWB = this.GetRowColOfWorbook(item.Celda);
                string fecha = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
                DateTime date = DateTime.TryParseExact(fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime objDT)
                    ? objDT
                    : DateTime.Now.Date;
                result[count] = date;
                count++;
            }
            
            return result;
        }

        public string[] GetResults(Workbook origin, SettingsToDisplayCGDReportsDTO reportsDTO)
        {
            IEnumerable<ConfigurationReportsDTO> resultCelda = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Resultado"));
            string[] result = new string[3];
            int count = 0;
            foreach (ConfigurationReportsDTO item in resultCelda)
            {
                int[] _positionWB = this.GetRowColOfWorbook(item.Celda);
                string res = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
                result[count] = res;
                count++;
            }

            return result;
        }

        public bool Verify_CGD_Columns(SettingsToDisplayCGDReportsDTO reportsDTO, Workbook workbook)
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

        public class CGDKeys
        {
            public const string Hydrogen = "HYDROGEN";
            public const string Oxygen = "OXYGEN";
            public const string Nitrogen = "NITROGEN";
            public const string Methane = "METHANE";
            public const string CarbonMonoxide = "CARBONMONOXIDE";
            public const string CarbonDioxide = "CARBONDIOXIDE";
            public const string Ethylene = "ETHYLENE";
            public const string Ethane = "ETHANE";
            public const string Acetylene = "ACETYLENE";
            public const string TotalGases = "TOTALGASES";
            public const string CombustibleGases = "COMBUSTIBLEGASES";
            public const string PorcCombustibleGas = "PORCCOMBUSTIBLEGAS";
            public const string PorcGasContent = "PORCGASCONTENT";
            public const string AcetyleneTest = "ACETYLENETEST";
            public const string HydrogenTest = "HYDROGENTEST";
            public const string MethaneEthyleneEthaneTest = "METHANEETHYLENEETHANETEST";
            public const string CarbonMonoxideTest = "CARBONMONOXIDETEST";
            public const string CarbonDioxideTest = "CARBONDIOXIDETEST";

        }
        #endregion

    }
}
