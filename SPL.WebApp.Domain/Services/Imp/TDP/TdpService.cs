namespace SPL.WebApp.Domain.Services.Imp.TDP
{
    using System;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public class TdpService : ITdpService
    {
        #region Error message
        private readonly ICorrectionFactorService _correcctionFactor;
        private readonly INozzleInformationService _nozzleInformationService;
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion
        public TdpService(ICorrectionFactorService correcctionFactor, INozzleInformationService nozzleInformationService)
        {
            _correcctionFactor = correcctionFactor;
            _nozzleInformationService = nozzleInformationService;
        }
        public void PrepareTemplate(SettingsToDisplayTDPReportsDTO reportsDTO, ref Workbook workbook, string keyTest, string languaje, string voltageLevels, int noCycles)
        {
            try
            {
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

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelTension")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = voltageLevels;

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Frecuencia")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.Frequency;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].TextAlign = "Left";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    AllowNulls = false,
                    MessageTemplate = $"La Frecuencia debe ser mayor a cero considerando 6 enteros",
                    ComparerType = "between",
                    From = "1",
                    To = "999999",
                    Type = "reject",
                    TitleTemplate = "Error",
                    ShowButton = true
                };

                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    AllowNulls = false,
                    MessageTemplate = $"El valor de la Frecuencia debe ser mayor a cero considerando 6 enteros sin decimales",
                    ComparerType = "between",
                    From = "1",
                    To = "999999",
                    Type = "reject",
                    TitleTemplate = "Error",
                    ShowButton = true
                };

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo")).Celda);

                for (int i = 0; i < reportsDTO.Times.Count; i++)
                {
                    if (i == 2)
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Formula = "=CONCATENATE(" + noCycles + @"/J9,"" Sec."")";
                    else
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = i == 1 ? reportsDTO.Times[i].ToString() + " Min." : (object)reportsDTO.Times[i].ToString();
                    }
                }

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension")).Celda);

                for (int i = 0; i < reportsDTO.Voltages.Count; i++)
                {
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = reportsDTO.Voltages[i].ToString();
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "###,##0.000";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "number",
                        AllowNulls = false,
                        MessageTemplate = $"La tensión de prueba debe ser numérica mayor o igual a cero considerando 9 enteros con 3 decimales",
                        ComparerType = "between",
                        From = "0.0",
                        To = "999999999.999",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };
                }

                BorderStyle boder = new()
                {
                    Color = "Black",
                    Size = 1
                };

                for (int i = 0; i < reportsDTO.Times.Count; i++)
                {

                    for (int j = 0; j <= reportsDTO.BaseTemplate.ColumnasConfigurables + 1; j++)
                    {

                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo")).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].BorderBottom = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].BorderLeft = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].BorderTop = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].BorderRight = boder;
                    }
                }

                for (int i = 0; i < reportsDTO.Times.Count; i++)
                {
                    for (int j = 0; j < reportsDTO.BaseTemplate.ColumnasConfigurables; j++)
                    {

                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal1")).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].Color = "Black";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].Format = "###,##0.0";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].Validation = new Validation()
                        {
                            DataType = "number",
                            AllowNulls = false,
                            MessageTemplate = $"El valor de la terminal debe ser mayor a cero considerando 5 enteros con un decimal",
                            ComparerType = "between",
                            From = "0.1",
                            To = "99999.9",
                            Type = "reject",
                            TitleTemplate = "Error",
                            ShowButton = true
                        };

                    }
                }

                for (int i = 1; i <= reportsDTO.BaseTemplate.ColumnasConfigurables; i++)
                {
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelMedido")).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + (i - 1)].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + (i - 1)].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + (i - 1)].Validation = new Validation()
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

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelCalibracion")).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + (i - 1)].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + (i - 1)].Color = "Black";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + (i - 1)].Validation = new Validation()
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
                }

                if (reportsDTO.BaseTemplate.ColumnasConfigurables == 3)
                {

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal1") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.TitTerminal1.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal2") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.TitTerminal2.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal3") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.TitTerminal3.Trim());

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed1") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.UMed1.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed2") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.UMed2.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed3") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.UMed3.Trim());

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal1") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.TitTerminal1.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal2") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.TitTerminal2.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal3") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.TitTerminal3.Trim());

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed1") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.UMed1.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed2") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.UMed2.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed3") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString().Replace("XX", reportsDTO.UMed3.Trim());

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelCalibracion")).Celda);
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

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelMedido")).Celda);
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

                }
                else
                {
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal1") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal1.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal2") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal2.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal3") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal3.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal4") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal1.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal5") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal2.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal6") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal3.Trim());

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed1") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed1.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed2") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed2.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed3") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed3.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed4") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed4.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed5") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed5.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed6") && c.Seccion == 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed6.Trim());

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal1") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal1.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal2") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal2.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal3") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal3.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal4") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal1.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal5") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal2.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerminal6") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.TitTerminal3.Trim());

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed1") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed1.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed2") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed2.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed3") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed3.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed4") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed4.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed5") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed5.Trim());
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMed6") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString()?.Replace("XX", reportsDTO.UMed6.Trim());

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string ValidateTemplateTDP(SettingsToDisplayTDPReportsDTO reportsDTO, Workbook workbook)
        {
            try
            {
                int[] _positionWB;
                string valor = string.Empty;

                if (reportsDTO.BaseTemplate.ColumnasConfigurables == 3)
                {  /***********VALIDAR QUE TODOS LOS DATOS ESTEN INTRODUCIDOS EN EL EXCEL ************************************************************/
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal1")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count; i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                            return "Debe llenar todas las posiciones asociadas a la Terminal 1";
                        else
                        {

                        }
                    }

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal2")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count; i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                            return "Debe llenar todas las posiciones asociadas a la Terminal 2";
                        else
                        {

                        }
                    }

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal3")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count; i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                            return "Debe llenar todas las posiciones asociadas a la Terminal 3";
                        else
                        {

                        }
                    }
                    /******************************************************************************************************************************************/
                }
                else
                {
                    /***********VALIDAR QUE TODOS LOS DATOS ESTEN INTRODUCIDOS EN EL EXCEL ************************************************************/
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal1")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count; i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                            return "Debe llenar todas las posiciones asociadas a la Terminal 1";
                    }

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal2")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count; i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                            return "Debe llenar todas las posiciones asociadas a la Terminal 2";
                    }

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal3")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count; i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                            return "Debe llenar todas las posiciones asociadas a la Terminal 3";
                    }

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal4")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count; i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                            return "Debe llenar todas las posiciones asociadas a la Terminal 4";
                    }

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal5")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count; i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                            return "Debe llenar todas las posiciones asociadas a la Terminal 5";
                    }

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal6")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count; i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                            return "Debe llenar todas las posiciones asociadas a la Terminal 6";
                    }
                    /******************************************************************************************************************************************/
                }

                for (int i = 1; i <= reportsDTO.BaseTemplate.ColumnasConfigurables; i++)
                {
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelCalibracion")).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + (i - 1)]?.Value?.ToString();
                    if (valor == null)
                        return "Los valores de Nivel de calibracion deben estar llenos";

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelMedido")).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + (i - 1)]?.Value?.ToString();
                    if (valor == null)
                        return "Los valores de Nivel de medicion deben estar llenos";
                }

                return string.Empty;
            }
            catch (Exception)
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
                    col += cell[i];
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
