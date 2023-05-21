namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;

    using Telerik.Web.Spreadsheet;
    using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;

    public class IndService : IIndService
    {
        #region Error message
        private readonly ICorrectionFactorService _correcctionFactor;
        private readonly INozzleInformationService _nozzleInformationService;
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion
        public IndService(ICorrectionFactorService correcctionFactor , INozzleInformationService nozzleInformationService)
        {
            this._correcctionFactor = correcctionFactor;
            this._nozzleInformationService = nozzleInformationService;
        }
        public void PrepareTemplate(SettingsToDisplayINDReportsDTO reportsDTO, ref Workbook workbook, string keyTest, string languaje, string tcBuyers)
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

                string val = string.Empty;

                if(keyTest == "ALA" || keyTest == "TER" || keyTest == "CBO")
                {
                    val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotPaginas") && c.ClavePrueba == keyTest)?.Celda + ",@^[a-zA-Z0-9]*$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotPaginas") && c.ClavePrueba == keyTest)?.Celda + "')<=10)";
                    val = val.Replace('@', '"');
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotPaginas") && c.ClavePrueba == keyTest)?.Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "custom",
                        AllowNulls = false,
                        MessageTemplate = $"El total de páginas no puede excederse de 10 caracteres",
                        From = val,
                        ComparerType = "custom",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };


                    if(keyTest == "ALA")
                    {
                        var paginas = reportsDTO.ConfigurationReports.Where(c => c.Dato.Contains("NoPagina") && c.ClavePrueba == keyTest).ToList();

                        foreach(var pagina in paginas)
                        {

                            val = "AND(REGEXP_MATCH(" + pagina.Celda + ",@^[a-zA-Z0-9]*$@),LEN('" +pagina.Celda + "')<=10)";
                            val = val.Replace('@', '"');
                            _positionWB = this.GetRowColOfWorbook(pagina.Celda);
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                            {
                                DataType = "custom",
                                AllowNulls = false,
                                MessageTemplate = $"El número de páginas no puede excederse de 10 caracteres",
                                From = val,
                                ComparerType = "custom",
                                Type = "reject",
                                TitleTemplate = "Error",
                                ShowButton = true
                            };
                        }


                    }
                    else if(keyTest == "CBO")
                    {
                        val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotPaginas") && c.ClavePrueba == keyTest)?.Celda + ",@^[a-zA-Z0-9]*$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotPaginas") && c.ClavePrueba == keyTest)?.Celda + "')<=10)";
                        val = val.Replace('@', '"');
                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotPaginas") && c.ClavePrueba == keyTest)?.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            AllowNulls = false,
                            MessageTemplate = $"El total de páginas no puede excederse de 10 caracteres",
                            From = val,
                            ComparerType = "custom",
                            Type = "reject",
                            TitleTemplate = "Error",
                            ShowButton = true
                        };
                        val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Anexo") && c.ClavePrueba == keyTest)?.Celda + ",@^[a-zA-Z0-9]*$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Anexo") && c.ClavePrueba == keyTest)?.Celda + "')<=3)";
                        val = val.Replace('@', '"');
                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Anexo") && c.ClavePrueba == keyTest)?.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            AllowNulls = false,
                            MessageTemplate = $"El valor del anexo solo puede contener letras y/o números y no puede excederse de 3 caracteres",
                            From = val,
                            ComparerType = "custom",
                            Type = "reject",
                            TitleTemplate = "Error",
                            ShowButton = true
                        };



                    }
                    else if(keyTest == "TER")
                    {
                        val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Anexo") && c.ClavePrueba == keyTest)?.Celda + ",@^[a-zA-Z0-9]*$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Anexo") && c.ClavePrueba == keyTest)?.Celda + "')<=3)";
                        val = val.Replace('@', '"');
                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Anexo") && c.ClavePrueba == keyTest)?.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            AllowNulls = false,
                            MessageTemplate = $"El valor del anexo solo puede contener letras y/o números y no puede excederse de 3 caracteres",
                            From = val,
                            ComparerType = "custom",
                            Type = "reject",
                            TitleTemplate = "Error",
                            ShowButton = true
                        };


                        val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotPaginas") && c.ClavePrueba == keyTest)?.Celda + ",@^[a-zA-Z0-9]*$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotPaginas") && c.ClavePrueba == keyTest)?.Celda + "')<=10)";
                        val = val.Replace('@', '"');
                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotPaginas") && c.ClavePrueba == keyTest)?.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            AllowNulls = false,
                            MessageTemplate = $"El total de páginas no puede excederse de 10 caracteres",
                            From = val,
                            ComparerType = "custom",
                            Type = "reject",
                            TitleTemplate = "Error",
                            ShowButton = true
                        };


                        val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorKW") && c.ClavePrueba == keyTest)?.Celda + ",@^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,3})?%?$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotPaginas") && c.ClavePrueba == keyTest)?.Celda + "')<=10)";
                        val = val.Replace('@', '"');
                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorKW") && c.ClavePrueba == keyTest)?.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            AllowNulls = false,
                            MessageTemplate = $"El valor KW debe ser mayor a cero considerando 3 enteros con 3 decimales",
                            From = val,
                            ComparerType = "custom",
                            Type = "reject",
                            TitleTemplate = "Error",
                            ShowButton = true
                        };


                        val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorMVA") && c.ClavePrueba == keyTest)?.Celda + ",@^\\\\d+$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorMVA") && c.ClavePrueba == keyTest)?.Celda + "')<=6)";
                        val = val.Replace('@', '"');
                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorMVA") && c.ClavePrueba == keyTest)?.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            AllowNulls = false,
                            MessageTemplate = $"El valor MVA debe ser mayor a cero considerando 6 enteros con cero decimales",
                            From = val,
                            ComparerType = "custom",
                            Type = "reject",
                            TitleTemplate = "Error",
                            ShowButton = true
                        };
                    }




                }

                if (keyTest == "ACT")
                {
                    val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPagina1") && c.ClavePrueba == keyTest)?.Celda + ",@^[a-zA-Z0-9]*$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPagina1") && c.ClavePrueba == keyTest)?.Celda + "')<=10)";
                    val = val.Replace('@', '"');
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPagina1") && c.ClavePrueba == keyTest)?.Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "custom",
                        AllowNulls = false,
                        MessageTemplate = $"El número de páginas no puede excederse de 10 caracteres",
                        From = val,
                        ComparerType = "custom",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };

                    val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPagina2") && c.ClavePrueba == keyTest)?.Celda + ",@^[a-zA-Z0-9]*$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPagina2") && c.ClavePrueba == keyTest)?.Celda + "')<=10)";
                    val = val.Replace('@', '"');
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPagina2") && c.ClavePrueba == keyTest)?.Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "custom",
                        AllowNulls = false,
                        MessageTemplate = $"El número de páginas no puede excederse de 10 caracteres",
                        From = val,
                        ComparerType = "custom",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };

                    val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Anexo") && c.ClavePrueba == keyTest)?.Celda + ",@^[a-zA-Z0-9]*$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Anexo") && c.ClavePrueba == keyTest)?.Celda + "')<=3)";
                    val = val.Replace('@', '"');
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Anexo") && c.ClavePrueba == keyTest)?.Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "custom",
                        AllowNulls = false,
                        MessageTemplate = $"El valor del anexo solo puede contener letras y/o números y no puede excederse de 3 caracteres",
                        From = val,
                        ComparerType = "custom",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };

                }

                if (keyTest == "CTC")
                {
                    val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Anexo") && c.ClavePrueba == keyTest)?.Celda + ",@^[a-zA-Z0-9]*$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Anexo") && c.ClavePrueba == keyTest)?.Celda + "')<=3)";
                    val = val.Replace('@', '"');
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Anexo") && c.ClavePrueba == keyTest)?.Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "custom",
                        AllowNulls = false,
                        MessageTemplate = $"El valor del anexo solo puede contener letras y/o números y no puede excederse de 3 caracteres",
                        From = val,
                        ComparerType = "custom",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };


                    if (tcBuyers.ToUpper() == "SI")
                    {
                        val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaIni") && c.ClavePrueba == keyTest)?.Celda + ",@^[a-zA-Z0-9]*$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaIni") && c.ClavePrueba == keyTest)?.Celda + "')<=10)";
                        val = val.Replace('@', '"');
                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaIni") && c.ClavePrueba == keyTest)?.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            AllowNulls = false,
                            MessageTemplate = $"El número de páginas inicial no puede excederse de 10 caracteres",
                            From = val,
                            ComparerType = "custom",
                            Type = "reject",
                            TitleTemplate = "Error",
                            ShowButton = true
                        };

                        val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaFin") && c.ClavePrueba == keyTest)?.Celda + ",@^[a-zA-Z0-9]*$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaFin") && c.ClavePrueba == keyTest)?.Celda + "')<=10)";
                        val = val.Replace('@', '"');
                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaFin") && c.ClavePrueba == keyTest)?.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            AllowNulls = false,
                            MessageTemplate = $"El número de páginas final no puede excederse de 10 caracteres",
                            From = val,
                            ComparerType = "custom",
                            Type = "reject",
                            TitleTemplate = "Error",
                            ShowButton = true
                        };


                        val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaIni") && c.Seccion == 2 && c.ClavePrueba == keyTest)?.Celda + ",@^[a-zA-Z0-9]*$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaIni") && c.Seccion == 2 && c.ClavePrueba == keyTest)?.Celda + "')<=10)";
                        val = val.Replace('@', '"');
                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaIni") && c.Seccion == 2 && c.ClavePrueba == keyTest)?.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            AllowNulls = false,
                            MessageTemplate = $"El número de páginas inicial no puede excederse de 10 caracteres",
                            From = val,
                            ComparerType = "custom",
                            Type = "reject",
                            TitleTemplate = "Error",
                            ShowButton = true
                        };

                        val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaFin") && c.Seccion == 2 && c.ClavePrueba == keyTest)?.Celda + ",@^[a-zA-Z0-9]*$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaFin") && c.Seccion == 2 && c.ClavePrueba == keyTest)?.Celda + "')<=10)";
                        val = val.Replace('@', '"');
                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaFin") && c.Seccion == 2 && c.ClavePrueba == keyTest)?.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            AllowNulls = false,
                            MessageTemplate = $"El número de páginas final no puede excederse de 10 caracteres",
                            From = val,
                            ComparerType = "custom",
                            Type = "reject",
                            TitleTemplate = "Error",
                            ShowButton = true
                        };

                    }
                    else
                    {
                        val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaIni") && c.ClavePrueba == keyTest)?.Celda + ",@^[a-zA-Z0-9]*$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaIni") && c.ClavePrueba == keyTest)?.Celda + "')<=10)";
                        val = val.Replace('@', '"');
                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaIni") && c.ClavePrueba == keyTest)?.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            AllowNulls = false,
                            MessageTemplate = $"El número de páginas inicial no puede excederse de 10 caracteres",
                            From = val,
                            ComparerType = "custom",
                            Type = "reject",
                            TitleTemplate = "Error",
                            ShowButton = true
                        };

                        val = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaFin") && c.ClavePrueba == keyTest)?.Celda + ",@^[a-zA-Z0-9]*$@),LEN('" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaFin") && c.ClavePrueba == keyTest)?.Celda + "')<=10)";
                        val = val.Replace('@', '"');
                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaFin") && c.ClavePrueba == keyTest)?.Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            AllowNulls = false,
                            MessageTemplate = $"El número de páginas final no puede excederse de 10 caracteres",
                            From = val,
                            ComparerType = "custom",
                            Type = "reject",
                            TitleTemplate = "Error",
                            ShowButton = true
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string ValidateTemplateIND(SettingsToDisplayINDReportsDTO reportsDTO, Workbook workbook, string keyTest , string tieneTC, ref List<INDTestsDetailsDTO> arreglo , ref int totalPag,ref string anexo)
        {
            try
            {
                int[] _positionWB;
                string valor = string.Empty;
                INDTestsDetailsDTO obj = new INDTestsDetailsDTO();

                if (keyTest == "ACT")
                {
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPagina1") && c.ClavePrueba == keyTest).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(valor))
                    {
                        return "ERR";
                    }
                    else
                    {
                        obj.NoPage = valor;
                    }
                    arreglo.Add(obj);
                    obj = new INDTestsDetailsDTO();
                    
                    
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPagina2") && c.ClavePrueba == keyTest).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(valor))
                    {
                        return "ERR";
                    }
                    else
                    {
                        obj.NoPage = valor;
                    }
                    arreglo.Add(obj);

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Anexo") && c.ClavePrueba == keyTest).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(valor))
                    {
                        return "ERR";
                    }
                    else
                    {
                      anexo = valor;
                    }



                }
                else if(keyTest == "ALA")
                {
                    var data = reportsDTO.ConfigurationReports.Where(c => c.Dato.Contains("NoPagina") && c.ClavePrueba == keyTest).ToList();

                    foreach(var item in data)
                    {
                        obj = new INDTestsDetailsDTO();

                        _positionWB = this.GetRowColOfWorbook(item.Celda);
                        valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                        if (string.IsNullOrEmpty(valor))
                        {
                            return "ERR";
                        }
                        else
                        {
                            obj.NoPage = valor;
                        }

                        arreglo.Add(obj);
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotPaginas") && c.ClavePrueba == keyTest).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(valor))
                    {
                        return "ERR";
                    }
                    else
                    {
                        totalPag = int.Parse(valor);
                    }

                }
                else if (keyTest == "CBO")
                {
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotPaginas") && c.ClavePrueba == keyTest).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(valor))
                    {
                        return "ERR";
                    }
                    else
                    {
                       totalPag = int.Parse(valor);
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Anexo") && c.ClavePrueba == keyTest).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(valor))
                    {
                        return "ERR";
                    }
                    else
                    {
                        anexo = valor;
                    }
                    // arreglo.Add(obj);
                }
                else if (keyTest == "TER")
                {
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotPaginas") && c.ClavePrueba == keyTest).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(valor))
                    {
                        return "ERR";
                    }
                    else
                    {
                        totalPag= int.Parse(valor);
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorKW") && c.ClavePrueba == keyTest).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(valor))
                    {
                        return "ERR";
                    }
                    else
                    {
                        obj.KwValue = decimal.Parse(valor);
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorMVA") && c.ClavePrueba == keyTest).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(valor))
                    {
                        return "ERR";
                    }
                    else
                    {
                        obj.MvaValue = decimal.Parse(valor);
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Anexo") && c.ClavePrueba == keyTest).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(valor))
                    {
                        return "ERR";
                    }
                    else
                    {
                        anexo = valor;
                    }

                    arreglo.Add(obj);
                }
                else if (keyTest == "CTC")
                {
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaIni") && c.ClavePrueba == keyTest).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(valor))
                    {
                        return "ERR";
                    }
                    else
                    {
                        obj.NoInitPage = valor;
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaFin") && c.ClavePrueba == keyTest).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(valor))
                    {
                        return "ERR";
                    }
                    else
                    {
                        obj.NoPagEnd = valor;
                    }

                    arreglo.Add(obj);

                    if(tieneTC.ToUpper() == "SI")
                    {
                        INDTestsDetailsDTO obj2 = new INDTestsDetailsDTO();
                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaIni") && c.Seccion == 2 && c.ClavePrueba == keyTest).Celda);
                        valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                        if (string.IsNullOrEmpty(valor))
                        {
                            return "ERR";
                        }
                        else
                        {
                            obj2.NoInitPage = valor;
                        }

                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPaginaFin") && c.Seccion == 2 && c.ClavePrueba == keyTest).Celda);
                        valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                        if (string.IsNullOrEmpty(valor))
                        {
                            return "ERR";
                        }
                        else
                        {
                            obj2.NoPagEnd = valor;
                        }

                        arreglo.Add(obj2);
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Anexo") && c.ClavePrueba == keyTest).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(valor))
                    {
                        return "ERR";
                    }
                    else
                    {
                        anexo = valor;
                    }


                }


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
