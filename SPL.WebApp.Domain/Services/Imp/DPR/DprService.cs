namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.DPR;

    using Telerik.Web.Spreadsheet;

    public class DprService : IDprService
    {
        #region Error message
        private readonly ICorrectionFactorService _correcctionFactor;
        private readonly INozzleInformationService _nozzleInformationService;
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion
        public DprService(ICorrectionFactorService correcctionFactor , INozzleInformationService nozzleInformationService)
        {
            this._correcctionFactor = correcctionFactor;
            this._nozzleInformationService = nozzleInformationService;
        }
        public void PrepareTemplate(SettingsToDisplayDPRReportsDTO reportsDTO, ref Workbook workbook, string keyTest, string languaje)
        {
            try
            {
                #region Update Readonly all cells
                workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));
                #endregion

                int[] _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cliente")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.Client;

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoSerie")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.NoSerie;

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Capacidad")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.Capacity) ? string.Empty : $"{reportsDTO.HeadboardReport.Capacity} MVA";

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha")).Celda);
               
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Color = "Black";
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



                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionPrueba")).Celda);

              
               
               //string val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionPrueba")).Celda + ",@^[a-zA-Z0-9]*$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionPrueba")).Celda + "')<=20)";
               // val = val.Replace('@', '"');
               
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Color = "Black";
                //workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                //{
                //    DataType = "custom",
                //    AllowNulls = false,
                //    MessageTemplate = $"La tensión de prueba no puede excederse de 20 caracteres",
                //    From = val,
                //    ComparerType = "custom",
                //    Type = "reject",
                //    TitleTemplate = "Error",
                //    ShowButton = true
                //};
                //_positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelTension")).Celda);
                //workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = voltageLevels;

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Frecuencia")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.Frequency.ToString();

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo")).Celda);

                for(int i = 0; i < reportsDTO.Times.Count(); i++)
                {
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = reportsDTO.Times[i].ToString();
                }

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionLN")).Celda);

                for (int i = 0; i < reportsDTO.Voltages.Count(); i++)
                {
                    workbook.Sheets[0].Rows[_positionWB[0]+i].Cells[_positionWB[1]].Value = reportsDTO.Voltages[i].ToString();
                }

                BorderStyle boder = new()
                {
                    Color = "Black",
                    Size = 1
                };

                for (int i = 0; i < reportsDTO.Times.Count(); i++)
                {

                    for (int j = 0; j <= (reportsDTO.BaseTemplate.ColumnasConfigurables + 1); j++)
                    {

                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo")).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].BorderBottom = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].BorderLeft = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].BorderTop = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].BorderRight = boder;
                    }
                }

                for (int i = 0; i < reportsDTO.Times.Count(); i++)
                {
                    for (int j = 0; j < reportsDTO.BaseTemplate.ColumnasConfigurables; j++)
                    {
                        if (reportsDTO.BaseTemplate.ColumnasConfigurables == 3)
                        {

                            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_1")).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].Enable = true;
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].Color = "Black";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].Validation = new Validation()
                            {
                                DataType = "number",
                                AllowNulls = false,
                                MessageTemplate = $"El valor de la terminal debe ser mayor a cero considerando 5 enteros sin decimales",
                                ComparerType = "between",
                                From = "1",
                                To = "99999",
                                Type = "reject",
                                TitleTemplate = "Error",
                                ShowButton = true
                            };
                        }
                        else {

                            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_1_1")).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].Enable = true;
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].Color = "Black";
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].Validation = new Validation()
                            {
                                DataType = "number",
                                AllowNulls = false,
                                MessageTemplate = $"El valor de la terminal debe ser mayor a cero considerando 5 enteros sin decimales",
                                ComparerType = "between",
                                From = "1",
                                To = "99999",
                                Type = "reject",
                                TitleTemplate = "Error",
                                ShowButton = true
                            };

                        }
                    }
                }



                if (reportsDTO.BaseTemplate.ColumnasConfigurables == 3)
                {


                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerm_1") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.TitTerminal1.Trim());
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerm_2") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.TitTerminal2.Trim());
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerm_3") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.TitTerminal3.Trim());

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Unidad_1") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.UMed1.Trim());
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Unidad_2") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.UMed2.Trim());
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Unidad_3") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.UMed3.Trim());












                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerm_1") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.TitTerminal1.Trim());
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerm_2") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.TitTerminal2.Trim());
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerm_3") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.TitTerminal3.Trim());

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Unidad_1") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.UMed1.Trim());
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Unidad_2") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.UMed2.Trim());
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Unidad_3") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.UMed3.Trim());






                    _positionWB = this.GetRowColOfWorbook("H42");
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel de calibración debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel de calibración  debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel de calibración  debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };











                    _positionWB = this.GetRowColOfWorbook("H43");
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel medido debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel medido debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel medido debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };







                    _positionWB = this.GetRowColOfWorbook("E49");
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;


                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;


                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Enable = true;


                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Enable = true;

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4].Enable = true;

                    _positionWB = this.GetRowColOfWorbook("E50");
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;


                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;


                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Enable = true;


                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4].Enable = true;

                }
                else
                {
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerm_1") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal1.Trim());
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerm_2") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal2.Trim());
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerm_3") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal3.Trim());
                   

                    //_positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_1_1") && c.Seccion == 1).Celda);
                    //workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed1.Trim());
                    //_positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_1_2") && c.Seccion == 1).Celda);
                    //workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed2.Trim());
                    //_positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_2_1") && c.Seccion == 1).Celda);
                    //workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed3.Trim());
                    //_positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_2_2") && c.Seccion == 1).Celda);
                    //workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed4.Trim());
                    //_positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_3_1") && c.Seccion == 1).Celda);
                    //workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed5.Trim());
                    //_positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_3_2") && c.Seccion == 1).Celda);
                    //workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed6.Trim());






                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerm_1") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal1.Trim());
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals(value: "TitTerm_2") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal2.Trim());
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerm_3") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal3.Trim());






                    _positionWB = this.GetRowColOfWorbook("E42");
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel de calibración debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel de calibración  debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel de calibración  debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel de calibración  debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel de calibración  debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };


                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel de calibración  debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };













                    _positionWB = this.GetRowColOfWorbook("E43");
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel medido debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel medido debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel medido debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };



                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel medido debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };



                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel medido debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };


                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"Nivel medido debe ser mayor a cero considerando 5 enteros sin decimales",
                        ComparerType = "between",
                        From = "1",
                        To = "99999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };











                    _positionWB = this.GetRowColOfWorbook("D49");
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
             

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                  

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Enable = true;
              

                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Enable = true;



                    _positionWB = this.GetRowColOfWorbook("D50");
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;


                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;


                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Enable = true;


                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Enable = true;



                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string ValidateTemplateDPR(SettingsToDisplayDPRReportsDTO reportsDTO, Workbook workbook)
        {
            try
            {
                int[] _positionWB;
                string valor = string.Empty;
                

                if (reportsDTO.BaseTemplate.ColumnasConfigurables == 3)
                {  /***********VALIDAR QUE TODOS LOS DATOS ESTEN INTRODUCIDOS EN EL EXCEL ************************************************************/
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_1")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if(valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 1";
                        }
                        else
                        {
                          
                        }
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_2")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 2";
                        }
                        else
                        {
                          
                        }
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_3")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 3";
                        }
                        else
                        {
                         
                        }
                    }
                    /******************************************************************************************************************************************/
                }
                else
                {
                    /***********VALIDAR QUE TODOS LOS DATOS ESTEN INTRODUCIDOS EN EL EXCEL ************************************************************/
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_1_1")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 1";
                        }
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_1_2")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 2";
                        }
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_2_1")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 3";
                        }
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_2_2")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 4";
                        }
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_3_1")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 5";
                        }
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_3_2")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 6";
                        }
                    }
                    /******************************************************************************************************************************************/
                }

                //for (int i = 1; i <= reportsDTO.BaseTemplate.ColumnasConfigurables; i++)
                //{
                //    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelCalibracion")).Celda);
                //    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + (i - 1)]?.Value?.ToString();
                //    if (valor == null)
                //    {
                //        return "Los valores de Nivel de calibracion deben estar llenos";
                //    }

                //    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelMedido")).Celda);
                //    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + (i - 1)]?.Value?.ToString();
                //    if (valor == null)
                //    {
                //        return "Los valores de Nivel de medicion deben estar llenos";
                //    }
                //}

                return string.Empty;
            }
            catch (Exception e)
            {
                throw;
            }
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
