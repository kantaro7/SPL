namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;

    using Telerik.Web.Spreadsheet;
    using SPL.WebApp.Domain.Helpers;

    public class RadService : IRadService
    {
        #region Error message
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion

        public void PrepareTemplate_RAD_CAYDES(SettingsToDisplayRADReportsDTO reportsDTO, ref Workbook workbook)
        {
            #region Error message
            const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
            #endregion
            #region Update Readonly all cells
            workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));
            #endregion

            #region Head

            int[] _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cliente")).Celda);

            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.Client;
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = false;

            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoSerie")).Celda);

            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.NoSerie;
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = false;

            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Capacidad")).Celda);

            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.Capacity) ? string.Empty : $"{reportsDTO.HeadboardReport.Capacity} MVA";
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = false;

            #endregion

            #region Body

            IEnumerable<ConfigurationReportsDTO> configDate = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha"));

            foreach (ConfigurationReportsDTO item in configDate)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);

                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "MM/dd/yyyy";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.NoteFc) ? DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : reportsDTO.HeadboardReport.NoteFc;
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
            }

            IEnumerable<ConfigurationReportsDTO> configTemp = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("UMTemp"));

            foreach (ConfigurationReportsDTO item in configTemp)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);

                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Format = "#,##0.0";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "999999999",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"
                };
            }

            IEnumerable<ConfigurationReportsDTO> configTension = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("UMTension"));

            foreach (ConfigurationReportsDTO item in configTension)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);

                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Format = "#,##0.00";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "999999999",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"
                };
            }

            //cell index is the index of the cell title column

            ConfigurationReportsDTO[] configcolumn1 = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TitColumna1")).ToArray();
            ConfigurationReportsDTO[] configcolumnValues = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Columna1")).ToArray();

            int[] _positionWB1 = GetRowColOfWorbook(configcolumn1[0].Celda);
            int[] _positionWB2 = GetRowColOfWorbook(configcolumn1[1].Celda);

            int[] _positionWBColumnBefore = GetRowColOfWorbook(configcolumnValues[0].Celda);
            int[] _positionWBColumnAfter = GetRowColOfWorbook(configcolumnValues[1].Celda);

            int cellIndex = _positionWBColumnBefore[1];
            foreach (ColumnTitleRADReportsDTO item in reportsDTO.TitleOfColumns)
            {
                workbook.Sheets[0].Rows[_positionWB1[0]].Cells[cellIndex].Value = item.Titulo;
                workbook.Sheets[0].Rows[_positionWB1[0]].Cells[cellIndex].Enable = false;
                workbook.Sheets[0].Rows[_positionWB2[0]].Cells[cellIndex].Value = item.Titulo;
                workbook.Sheets[0].Rows[_positionWB2[0]].Cells[cellIndex].Enable = false;

                //Before the test
                for (int i = _positionWBColumnBefore[0]; i < _positionWBColumnBefore[0] + 16; i++)
                {
                    if (i != _positionWBColumnBefore[0] + 13)
                    {
                        workbook.Sheets[0].Rows[i].Cells[cellIndex].Enable = i <= _positionWBColumnBefore[0] + 13;
                        workbook.Sheets[0].Rows[i].Cells[cellIndex].Format = "#,##0.00";
                        workbook.Sheets[0].Rows[i].Cells[cellIndex].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "0",
                            To = "999999999",
                            AllowNulls = true,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"

                        };
                    }
                }

                //After the test
                for (int i = _positionWBColumnAfter[0]; i < _positionWBColumnAfter[0] + 16; i++)
                {
                    if (i != _positionWBColumnAfter[0] + 13)
                    {
                        workbook.Sheets[0].Rows[i].Cells[cellIndex].Enable = i <= _positionWBColumnAfter[0] + 13;
                        workbook.Sheets[0].Rows[i].Cells[cellIndex].Format = "#,##0.00";
                        workbook.Sheets[0].Rows[i].Cells[cellIndex].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "0",
                            To = "999999999",
                            AllowNulls = true,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"

                        };
                    }
                }

                cellIndex++;
            }

            #endregion
        }

        public void PrepareTemplate_RAD_SA(SettingsToDisplayRADReportsDTO reportsDTO, ref Workbook workbook)
        {

            #region Update Readonly all cells
            workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));
            #endregion

            #region Head

            int[] _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cliente")).Celda);

            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.Client;
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = false;

            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoSerie")).Celda);

            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.NoSerie;
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = false;

            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Capacidad")).Celda);

            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.Capacity) ? string.Empty : $"{reportsDTO.HeadboardReport.Capacity} MVA";
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = false;

            ConfigurationReportsDTO configDate = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha"));

            _positionWB = GetRowColOfWorbook(configDate.Celda);

            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "MM/dd/yyyy";
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.NoteFc) ? DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : reportsDTO.HeadboardReport.NoteFc;
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

            #endregion

            #region Body

            ConfigurationReportsDTO configTemp = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTemp"));

            _positionWB = GetRowColOfWorbook(configTemp.Celda);

            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Enable = true;
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Format = "#,##0.0";
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Validation = new Validation()
            {
                DataType = "number",
                ComparerType = "greaterThan",
                From = "0",
                To = "999999999",
                AllowNulls = false,
                MessageTemplate = $"{messageErrorNumeric}",
                Type = "reject",
                TitleTemplate = "Error"
            };

            ConfigurationReportsDTO configTension = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTension"));

            _positionWB = GetRowColOfWorbook(configTension.Celda);

            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Enable = true;
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Format = "#,##0.00";
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Validation = new Validation()
            {
                DataType = "number",
                ComparerType = "greaterThan",
                From = "0",
                To = "999999999",
                AllowNulls = false,
                MessageTemplate = $"{messageErrorNumeric}",
                Type = "reject",
                TitleTemplate = "Error"
            };

            //cell index is the index of the cell title column

            ConfigurationReportsDTO configcolumn1 = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitColumna1"));
            ConfigurationReportsDTO configcolumnValues = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Columna1"));

            int[] _positionWB1 = GetRowColOfWorbook(configcolumn1.Celda);

            int[] _positionWBColumnBefore = GetRowColOfWorbook(configcolumnValues.Celda);

            int cellIndex = _positionWBColumnBefore[1];
            foreach (ColumnTitleRADReportsDTO item in reportsDTO.TitleOfColumns)
            {
                workbook.Sheets[0].Rows[_positionWB1[0]].Cells[cellIndex].Value = item.Titulo;
                workbook.Sheets[0].Rows[_positionWB1[0]].Cells[cellIndex].Enable = false;

                //Before the test
                for (int i = _positionWBColumnBefore[0]; i < _positionWBColumnBefore[0] + 16; i++)
                {
                    if (i != _positionWBColumnBefore[0] + 13)
                    {
                        workbook.Sheets[0].Rows[i].Cells[cellIndex].Enable = i <= _positionWBColumnBefore[0] + 13;
                        workbook.Sheets[0].Rows[i].Cells[cellIndex].Format = "#,##0.00";
                        workbook.Sheets[0].Rows[i].Cells[cellIndex].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "0",
                            To = "999999999",
                            AllowNulls = true,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"

                        };
                    }
                }

                cellIndex++;
            }

            #endregion
        }

        public void Prepare_RAD_Test(string testType, SettingsToDisplayRADReportsDTO reportsDTO, Workbook workbook, ref RADTestsDTO _radTestDTO)
        {
            if (testType.Equals(TestType.AYD.ToString()))
            {
                #region ListTime

                List<string> listTime = new();
                List<ColumnDTO> columbBeforeTest = new();
                List<ColumnDTO> columbAfterTest = new();

                ConfigurationReportsDTO[] getTemperatureLocation = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Temperatura")).ToArray();
                int[] _positionWBTemperature = GetRowColOfWorbook(getTemperatureLocation[0].Celda);
                ConfigurationReportsDTO[] getTensionLocation = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Tensión")).ToArray();
                int[] _positionWBTension = GetRowColOfWorbook(getTensionLocation[0].Celda);
                ConfigurationReportsDTO[] getDateLocation = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha")).ToArray();
                int[] _positionWBDate = GetRowColOfWorbook(getDateLocation[0].Celda);

                int[] _positionWBTemperature2 = GetRowColOfWorbook(getTemperatureLocation[1].Celda);
                int[] _positionWBTension2 = GetRowColOfWorbook(getTensionLocation[1].Celda);
                int[] _positionWBDate2 = GetRowColOfWorbook(getDateLocation[1].Celda);

                HeaderRADTestsDTO header = new()
                {
                    IdLoad = 0,
                    Section = 1,
                    Temperature = workbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1]].Value is null ? null : Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1]].Value),
                    Tension = workbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1]].Value is null ? null : Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1]].Value),
                    TestDate = DateUtils.getDateFromSheet(workbook.Sheets[0].Rows[_positionWBDate[0]].Cells[_positionWBDate[1]].Value.ToString()),
                    UnitOfMeasureOfTemperature = workbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1]].Value is null ? string.Empty : workbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1] + 1].Value.ToString(),
                    UnitOfMeasureOfTension = workbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1]].Value is null ? string.Empty : workbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1] + 1].Value.ToString()
                };

                HeaderRADTestsDTO header2 = new()
                {
                    IdLoad = 0,
                    Section = 2,
                    Temperature = workbook.Sheets[0].Rows[_positionWBTemperature2[0]].Cells[_positionWBTemperature2[1]].Value is null ? null : Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWBTemperature2[0]].Cells[_positionWBTemperature2[1]].Value),
                    Tension = workbook.Sheets[0].Rows[_positionWBTension2[0]].Cells[_positionWBTension2[1]].Value is null ? null : Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWBTension2[0]].Cells[_positionWBTension2[1]].Value),
                    TestDate = DateUtils.getDateFromSheet(workbook.Sheets[0].Rows[_positionWBDate2[0]].Cells[_positionWBDate2[1]].Value.ToString()),
                    UnitOfMeasureOfTemperature = workbook.Sheets[0].Rows[_positionWBTemperature2[0]].Cells[_positionWBTemperature2[1]].Value is null ? string.Empty : workbook.Sheets[0].Rows[_positionWBTemperature2[0]].Cells[_positionWBTemperature2[1] + 1].Value.ToString(),
                    UnitOfMeasureOfTension = workbook.Sheets[0].Rows[_positionWBTension2[0]].Cells[_positionWBTension2[1]].Value is null ? string.Empty : workbook.Sheets[0].Rows[_positionWBTension2[0]].Cells[_positionWBTension2[1] + 1].Value.ToString()
                };
                _radTestDTO.Headers = new List<HeaderRADTestsDTO>
                {
                    header,
                    header2
                };

                int[] _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo")).Celda);

                for (int i = _positionWB[0]; i < _positionWB[0] + 13; i++)
                {
                    listTime.Add(workbook.Sheets[0].Rows[i].Cells[_positionWB[1]].Value.ToString());
                }

                #endregion

                #region Get values from before test

                ConfigurationReportsDTO[] configcolumn1 = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TitColumna1")).ToArray();
                ConfigurationReportsDTO[] configcolumnValues = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Columna1")).ToArray();

                int[] _positionWBColumnBefore = GetRowColOfWorbook(configcolumnValues[0].Celda);
                int[] _positionWBColumnAfter = GetRowColOfWorbook(configcolumnValues[1].Celda);

                int cellIndex = _positionWBColumnBefore[1];
                foreach (ColumnTitleRADReportsDTO item in reportsDTO.TitleOfColumns)
                {
                    ColumnDTO columnDTO = new()
                    {
                        Name = item.Titulo,
                        Values = new List<decimal>()
                    };

                    //Before the test
                    for (int i = _positionWBColumnBefore[0]; i < _positionWBColumnBefore[0] + 13; i++)
                    {
                        if (workbook.Sheets[0].Rows[i].Cells[cellIndex].Value is not null)
                        {
                            columnDTO.Values.Add(Convert.ToDecimal(workbook.Sheets[0].Rows[i].Cells[cellIndex].Value));
                        }
                        else
                        {
                            columnDTO.Values.Add(0);
                        }
                    }

                    columbBeforeTest.Add(columnDTO);

                    columnDTO = new ColumnDTO
                    {
                        Name = item.Titulo,
                        Values = new List<decimal>()
                    };

                    //After the test
                    for (int i = _positionWBColumnAfter[0]; i < _positionWBColumnAfter[0] + 13; i++)
                    {
                        if (workbook.Sheets[0].Rows[i].Cells[cellIndex].Value is not null)
                        {
                            columnDTO.Values.Add(Convert.ToDecimal(workbook.Sheets[0].Rows[i].Cells[cellIndex].Value));
                        }
                        else
                        {
                            columnDTO.Values.Add(0);
                        }
                    }

                    columbAfterTest.Add(columnDTO);

                    cellIndex++;
                }
                #endregion

                _radTestDTO.Times = listTime;
                _radTestDTO.Columns = columbBeforeTest;
                _radTestDTO.Columns2 = columbAfterTest;

            }
            else
            {
                #region ListTime

                List<string> listTime = new();
                List<ColumnDTO> columbBeforeTest = new();

                int[] _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo")).Celda);

                for (int i = _positionWB[0]; i < _positionWB[0] + 13; i++)
                {
                    listTime.Add(workbook.Sheets[0].Rows[i].Cells[_positionWB[1]].Value.ToString());
                }

                #endregion

                int[] _positionWBTemperature = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Temperatura")).Celda);
                int[] _positionWBTension = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tensión")).Celda);

                HeaderRADTestsDTO header = new()
                {
                    IdLoad = 0,
                    Section = 1,
                    Temperature = workbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1]].Value is null ? null : Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1]].Value),
                    Tension = workbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1]].Value is null ? null : Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1]].Value),
                    TestDate = DateTime.Now,
                    UnitOfMeasureOfTemperature = workbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1]].Value is null ? string.Empty : workbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1] + 1].Value.ToString(),
                    UnitOfMeasureOfTension = workbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1]].Value is null ? string.Empty : workbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1] + 1].Value.ToString()
                };

                _radTestDTO.Headers = new List<HeaderRADTestsDTO>
                {
                    header
                };

                #region Get values from before test

                ConfigurationReportsDTO configcolumnValues = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Columna1"));

                int[] _positionWBColumnBefore = GetRowColOfWorbook(configcolumnValues.Celda);

                int cellIndex = _positionWBColumnBefore[1];
                foreach (ColumnTitleRADReportsDTO item in reportsDTO.TitleOfColumns)
                {
                    ColumnDTO columnDTO = new()
                    {
                        Name = item.Titulo,
                        Values = new List<decimal>()
                    };

                    //Before the test
                    for (int i = _positionWBColumnBefore[0]; i < _positionWBColumnBefore[0] + 13; i++)
                    {
                        if (workbook.Sheets[0].Rows[i].Cells[cellIndex].Value is not null)
                        {
                            columnDTO.Values.Add(Convert.ToDecimal(workbook.Sheets[0].Rows[i].Cells[cellIndex].Value));
                        }
                        else
                        {
                            columnDTO.Values.Add(0);
                        }
                    }

                    columbBeforeTest.Add(columnDTO);

                    cellIndex++;
                }
                #endregion

                _radTestDTO.Times = listTime;
                _radTestDTO.Columns = columbBeforeTest;
            }
        }

        public void PrepareIndexOfRAD(ResultRADTestsDTO resultRADTestsDTO, SettingsToDisplayRADReportsDTO reportsDTO, string ketLanguage, ref Workbook workbook)
        {
            if (resultRADTestsDTO.results.Count > 1)
            {

                ConfigurationReportsDTO[] configcolumnValuesIndAbs = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("IndAbs1")).ToArray();
                ConfigurationReportsDTO[] configcolumnValuesIndPola = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("IndPol1")).ToArray();

                int[] confInAbsBefore = GetRowColOfWorbook(configcolumnValuesIndAbs[0].Celda);
                int[] confIndPolariBefore = GetRowColOfWorbook(configcolumnValuesIndPola[0].Celda);
                int cellIndex = confInAbsBefore[1];
                //Test type is AYD

                #region Before
                ResultRADTestsDetailsDTO resultRAD = resultRADTestsDTO.results[0];

                for (int i = 0; i < resultRAD.AbsorptionIndexs.Count; i++)
                {
                    workbook.Sheets[0].Rows[confInAbsBefore[0]].Cells[cellIndex + i].Value = resultRAD.AbsorptionIndexs[i];
                }
                for (int i = 0; i < resultRAD.PolarizationIndexs.Count; i++)
                {
                    workbook.Sheets[0].Rows[confIndPolariBefore[0]].Cells[cellIndex + i].Value = resultRAD.PolarizationIndexs[i];
                }
                #endregion

                int[] confInAbsAfter = GetRowColOfWorbook(configcolumnValuesIndAbs[1].Celda);
                int[] confIndPolariAfter = GetRowColOfWorbook(configcolumnValuesIndPola[1].Celda);

                #region After
                resultRAD = resultRADTestsDTO.results[1];

                for (int i = 0; i < resultRAD.AbsorptionIndexs.Count; i++)
                {
                    workbook.Sheets[0].Rows[confInAbsAfter[0]].Cells[cellIndex + i].Value = resultRAD.AbsorptionIndexs[i];
                }
                for (int i = 0; i < resultRAD.PolarizationIndexs.Count; i++)
                {
                    workbook.Sheets[0].Rows[confIndPolariAfter[0]].Cells[cellIndex + i].Value = resultRAD.PolarizationIndexs[i];
                }
                #endregion

            }
            else
            {
                ConfigurationReportsDTO configcolumnValuesIndAbs = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("IndAbs1"));
                ConfigurationReportsDTO configcolumnValuesIndPola = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("IndPol1"));

                int[] confInAbsAfter = GetRowColOfWorbook(configcolumnValuesIndAbs.Celda);
                int[] confIndPolariAfter = GetRowColOfWorbook(configcolumnValuesIndPola.Celda);

                int cellIndex = confInAbsAfter[1];
                //Test type is SA

                #region Before
                ResultRADTestsDetailsDTO resultRAD = resultRADTestsDTO.results[0];

                for (int i = 0; i < resultRAD.AbsorptionIndexs.Count; i++)
                {
                    workbook.Sheets[0].Rows[confInAbsAfter[0]].Cells[cellIndex + i].Value = resultRAD.AbsorptionIndexs[i];
                }
                for (int i = 0; i < resultRAD.PolarizationIndexs.Count; i++)
                {
                    workbook.Sheets[0].Rows[confIndPolariAfter[0]].Cells[cellIndex + i].Value = resultRAD.PolarizationIndexs[i];
                }
                #endregion
            }

            ConfigurationReportsDTO resultLocationConfig = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado"));
            int[] resultLocation = GetRowColOfWorbook(resultLocationConfig.Celda);

            bool resultReport = true;
            foreach (ResultRADTestsDetailsDTO item in resultRADTestsDTO.results)
            {
                resultReport = !item.MessageErrors.Any();
                if (!resultReport)
                    break;
            }

            workbook.Sheets[0].Rows[resultLocation[0]].Cells[resultLocation[1]].Value = resultReport ?

                (ketLanguage.Equals("EN") ? reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptadoEN")).Formato : reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptadoES")).Formato) :

                (ketLanguage.Equals("EN") ? reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorRechazadoEN")).Formato : reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorRechazadoES")).Formato);
        }

        public string Validate(string testType, Workbook workbook, SettingsToDisplayRADReportsDTO reportsDTO)
        {
            List<string> errors = new();

            if (testType.Equals(TestType.AYD.ToString()))
            {

                ConfigurationReportsDTO[] getTemperatureLocation = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Temperatura")).ToArray();
                int[] _positionWBTemperature = GetRowColOfWorbook(getTemperatureLocation[0].Celda);

                ConfigurationReportsDTO[] getTensionLocation = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Tensión")).ToArray();
                int[] _positionWBTension = GetRowColOfWorbook(getTensionLocation[0].Celda);

                int[] _positionWBTemperature2 = GetRowColOfWorbook(getTemperatureLocation[1].Celda);
                int[] _positionWBTension2 = GetRowColOfWorbook(getTensionLocation[1].Celda);

                if (workbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1]].Value == null
                   && string.IsNullOrEmpty(Convert.ToString(workbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1]].Value)))
                {
                    errors.Add("Temperatura es Requerida.");
                }

                if (workbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1]].Value == null
                   && string.IsNullOrEmpty(Convert.ToString(workbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1]].Value)))
                {
                    errors.Add("Tensión es Requerida.");
                }
                if (workbook.Sheets[0].Rows[_positionWBTemperature2[0]].Cells[_positionWBTemperature2[1]].Value == null
                   && string.IsNullOrEmpty(Convert.ToString(workbook.Sheets[0].Rows[_positionWBTemperature2[0]].Cells[_positionWBTemperature2[1]].Value)))
                {
                    errors.Add("Temperatura es Requerida.");
                }

                if (workbook.Sheets[0].Rows[_positionWBTension2[0]].Cells[_positionWBTension2[1]].Value == null
                   && string.IsNullOrEmpty(Convert.ToString(workbook.Sheets[0].Rows[_positionWBTension2[0]].Cells[_positionWBTension2[1]].Value)))
                {
                    errors.Add("Tensión es Requerida.");
                }
            }
            else
            {

                ConfigurationReportsDTO[] getTemperatureLocation = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Temperatura")).ToArray();
                int[] _positionWBTemperature = GetRowColOfWorbook(getTemperatureLocation[0].Celda);

                ConfigurationReportsDTO[] getTensionLocation = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Tensión")).ToArray();
                int[] _positionWBTension = GetRowColOfWorbook(getTensionLocation[0].Celda);

                if (workbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1]].Value == null
                  && string.IsNullOrEmpty(Convert.ToString(workbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1]].Value)))
                {
                    errors.Add("Temperatura es Requerida.");
                }

                if (workbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1]].Value == null
                   && string.IsNullOrEmpty(Convert.ToString(workbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1]].Value)))
                {
                    errors.Add("Tensión es Requerida.");
                }
            }

            IEnumerable<ConfigurationReportsDTO> configDate = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha"));

            foreach (ConfigurationReportsDTO item in configDate)
            {
                int[] _positionWB = GetRowColOfWorbook(item.Celda);

                if (workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value == null
                  && string.IsNullOrEmpty(Convert.ToString(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value)))
                {
                    errors.Add("Fecha es Requerida.");
                }
            }

            return errors.Any() ? errors.Aggregate((c, n) => c + "*" + n) : null;
        }

        public void CloneWorkbook(string testType, string ketLanguage, Workbook workbook, SettingsToDisplayRADReportsDTO reportsDTO, ResultRADTestsDTO resultRADTestsDTO, ref Workbook officialWorkbook)
        {
            if (testType.Equals(TestType.AYD.ToString()))
            {

                ConfigurationReportsDTO[] getTemperatureLocation = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Temperatura")).ToArray();
                int[] _positionWBTemperature = GetRowColOfWorbook(getTemperatureLocation[0].Celda);

                ConfigurationReportsDTO[] getTensionLocation = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Tensión")).ToArray();
                int[] _positionWBTension = GetRowColOfWorbook(getTensionLocation[0].Celda);

                int[] _positionWBTemperature2 = GetRowColOfWorbook(getTemperatureLocation[1].Celda);
                int[] _positionWBTension2 = GetRowColOfWorbook(getTensionLocation[1].Celda);

                #region Headers

                officialWorkbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1]].Value = workbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1]].Value;
                officialWorkbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1]].Value = workbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1]].Value;

                officialWorkbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1] + 1].Value = workbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1] + 1].Value;
                officialWorkbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1] + 1].Value = workbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1] + 1].Value;

                officialWorkbook.Sheets[0].Rows[_positionWBTemperature2[0]].Cells[_positionWBTemperature2[1]].Value = workbook.Sheets[0].Rows[_positionWBTemperature2[0]].Cells[_positionWBTemperature2[1]].Value;
                officialWorkbook.Sheets[0].Rows[_positionWBTension2[0]].Cells[_positionWBTension2[1]].Value = workbook.Sheets[0].Rows[_positionWBTension2[0]].Cells[_positionWBTension2[1]].Value;

                officialWorkbook.Sheets[0].Rows[_positionWBTemperature2[0]].Cells[_positionWBTemperature2[1] + 1].Value = workbook.Sheets[0].Rows[_positionWBTemperature2[0]].Cells[_positionWBTemperature2[1] + 1].Value;
                officialWorkbook.Sheets[0].Rows[_positionWBTension2[0]].Cells[_positionWBTension2[1] + 1].Value = workbook.Sheets[0].Rows[_positionWBTension2[0]].Cells[_positionWBTension2[1] + 1].Value;

                #endregion

                #region Get values from before test

                ConfigurationReportsDTO[] configcolumn1 = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TitColumna1")).ToArray();
                ConfigurationReportsDTO[] configcolumnValues = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Columna1")).ToArray();

                int[] titilePositionBefore = GetRowColOfWorbook(configcolumn1[0].Celda);
                int titlePositionBeforeY = titilePositionBefore[1];

                int[] titilePositionAfter = GetRowColOfWorbook(configcolumn1[1].Celda);
                int titlePositionAfterY = titilePositionBefore[1];

                int[] _positionWBColumnBefore = GetRowColOfWorbook(configcolumnValues[0].Celda);
                int[] _positionWBColumnAfter = GetRowColOfWorbook(configcolumnValues[1].Celda);

                int cellIndex = _positionWBColumnBefore[1];
                foreach (ColumnTitleRADReportsDTO item in reportsDTO.TitleOfColumns)
                {
                    officialWorkbook.Sheets[0].Rows[titilePositionBefore[0]].Cells[titlePositionBeforeY++].Value = item.Titulo;

                    //Before the test
                    for (int i = _positionWBColumnBefore[0]; i < _positionWBColumnBefore[0] + 13; i++)
                    {
                        if (workbook.Sheets[0].Rows[i].Cells[cellIndex].Value is not null)
                        {
                            officialWorkbook.Sheets[0].Rows[i].Cells[cellIndex].Value = workbook.Sheets[0].Rows[i].Cells[cellIndex].Value;
                        }
                    }

                    officialWorkbook.Sheets[0].Rows[titilePositionAfter[0]].Cells[titlePositionAfterY++].Value = item.Titulo;

                    //After the test
                    for (int i = _positionWBColumnAfter[0]; i < _positionWBColumnAfter[0] + 13; i++)
                    {
                        if (workbook.Sheets[0].Rows[i].Cells[cellIndex].Value is not null)
                        {
                            officialWorkbook.Sheets[0].Rows[i].Cells[cellIndex].Value = workbook.Sheets[0].Rows[i].Cells[cellIndex].Value;
                        }
                    }

                    cellIndex++;
                }
                #endregion              

                ConfigurationReportsDTO[] configcolumnValuesIndAbs = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("IndAbs1")).ToArray();
                ConfigurationReportsDTO[] configcolumnValuesIndPola = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("IndPol1")).ToArray();

                int[] confInAbsBefore = GetRowColOfWorbook(configcolumnValuesIndAbs[0].Celda);
                int[] confIndPolariBefore = GetRowColOfWorbook(configcolumnValuesIndPola[0].Celda);

                int[] confInAbsAfter = GetRowColOfWorbook(configcolumnValuesIndAbs[1].Celda);
                int[] confIndPolariAfter = GetRowColOfWorbook(configcolumnValuesIndPola[1].Celda);

                cellIndex = confInAbsBefore[1];

                ResultRADTestsDetailsDTO resultRAD = resultRADTestsDTO.results[0];

                for (int i = 0; i < resultRAD.AbsorptionIndexs.Count; i++)
                {
                    officialWorkbook.Sheets[0].Rows[confInAbsBefore[0]].Cells[cellIndex + i].Value = workbook.Sheets[0].Rows[confInAbsBefore[0]].Cells[cellIndex + i].Value;
                    officialWorkbook.Sheets[0].Rows[confInAbsAfter[0]].Cells[cellIndex + i].Value = workbook.Sheets[0].Rows[confInAbsAfter[0]].Cells[cellIndex + i].Value;
                }
                for (int i = 0; i < resultRAD.PolarizationIndexs.Count; i++)
                {
                    officialWorkbook.Sheets[0].Rows[confIndPolariBefore[0]].Cells[cellIndex + i].Value = workbook.Sheets[0].Rows[confIndPolariBefore[0]].Cells[cellIndex + i].Value;
                    officialWorkbook.Sheets[0].Rows[confIndPolariAfter[0]].Cells[cellIndex + i].Value = workbook.Sheets[0].Rows[confIndPolariAfter[0]].Cells[cellIndex + i].Value;
                }

                ConfigurationReportsDTO resultLocationConfig = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado"));
                int[] resultLocation = GetRowColOfWorbook(resultLocationConfig.Celda);

                bool resultReport = true;
                foreach (ResultRADTestsDetailsDTO item in resultRADTestsDTO.results)
                {
                    resultReport = !item.MessageErrors.Any();
                    if (!resultReport)
                        break;
                }

                officialWorkbook.Sheets[0].Rows[resultLocation[0]].Cells[resultLocation[1]].Value = resultReport ?

                    (ketLanguage.Equals("EN") ? reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptadoEN")).Formato : reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptadoES")).Formato) :

                    (ketLanguage.Equals("EN") ? reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorRechazadoEN")).Formato : reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorRechazadoES")).Formato);

            }
            else
            {

                int[] _positionWBTemperature = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Temperatura")).Celda);
                int[] _positionWBTension = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tensión")).Celda);

                officialWorkbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1]].Value = workbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1]].Value;
                officialWorkbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1]].Value = workbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1]].Value;

                officialWorkbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1] + 1].Value = workbook.Sheets[0].Rows[_positionWBTemperature[0]].Cells[_positionWBTemperature[1] + 1].Value;
                officialWorkbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1] + 1].Value = workbook.Sheets[0].Rows[_positionWBTension[0]].Cells[_positionWBTension[1] + 1].Value;

                #region Get values from before test

                ConfigurationReportsDTO configcolumn1 = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitColumna1"));

                int[] titilePositionBefore = GetRowColOfWorbook(configcolumn1.Celda);
                int titlePositionBeforeY = titilePositionBefore[1];

                ConfigurationReportsDTO configcolumnValues = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Columna1"));

                int[] _positionWBColumnBefore = GetRowColOfWorbook(configcolumnValues.Celda);

                int cellIndex = _positionWBColumnBefore[1];
                foreach (ColumnTitleRADReportsDTO item in reportsDTO.TitleOfColumns)
                {
                    officialWorkbook.Sheets[0].Rows[titilePositionBefore[0]].Cells[titlePositionBeforeY++].Value = item.Titulo;

                    //Before the test
                    for (int i = _positionWBColumnBefore[0]; i < _positionWBColumnBefore[0] + 13; i++)
                    {
                        if (workbook.Sheets[0].Rows[i].Cells[cellIndex].Value is not null)
                        {
                            officialWorkbook.Sheets[0].Rows[i].Cells[cellIndex].Value = workbook.Sheets[0].Rows[i].Cells[cellIndex].Value;
                        }
                    }

                    cellIndex++;
                }
                #endregion

                ConfigurationReportsDTO configcolumnValuesIndAbs = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("IndAbs1"));
                ConfigurationReportsDTO configcolumnValuesIndPola = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("IndPol1"));

                int[] confInAbsAfter = GetRowColOfWorbook(configcolumnValuesIndAbs.Celda);
                int[] confIndPolariAfter = GetRowColOfWorbook(configcolumnValuesIndPola.Celda);

                cellIndex = confInAbsAfter[1];
                //Test type is SA

                #region Before
                ResultRADTestsDetailsDTO resultRAD = resultRADTestsDTO.results[0];

                for (int i = 0; i < resultRAD.AbsorptionIndexs.Count; i++)
                {
                    officialWorkbook.Sheets[0].Rows[confInAbsAfter[0]].Cells[cellIndex + i].Value = workbook.Sheets[0].Rows[confInAbsAfter[0]].Cells[cellIndex + i].Value;
                }
                for (int i = 0; i < resultRAD.PolarizationIndexs.Count; i++)
                {
                    officialWorkbook.Sheets[0].Rows[confIndPolariAfter[0]].Cells[cellIndex + i].Value = workbook.Sheets[0].Rows[confIndPolariAfter[0]].Cells[cellIndex + i].Value;
                }
                #endregion

                ConfigurationReportsDTO resultLocationConfig = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado"));
                int[] resultLocation = GetRowColOfWorbook(resultLocationConfig.Celda);

                bool resultReport = true;
                foreach (ResultRADTestsDetailsDTO item in resultRADTestsDTO.results)
                {
                    resultReport = !item.MessageErrors.Any();
                    if (!resultReport)
                        break;
                }

                officialWorkbook.Sheets[0].Rows[resultLocation[0]].Cells[resultLocation[1]].Value = resultReport ?

                    (ketLanguage.Equals("EN") ? reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptadoEN")).Formato : reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptadoES")).Formato) :

                    (ketLanguage.Equals("EN") ? reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorRechazadoEN")).Formato : reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorRechazadoES")).Formato);

            }
        }

        #region Private Methods
        private static int[] GetRowColOfWorbook(string cell)
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
