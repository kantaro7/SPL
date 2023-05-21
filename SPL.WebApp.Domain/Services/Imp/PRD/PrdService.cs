namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public class PrdService : IPrdService
    {
        #region Error message
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion

        public void PrepareTemplate_PRD(SettingsToDisplayPRDReportsDTO reportsDTO, ref Workbook workbook)
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
                // Cn
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cn")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "#0.00000000000000";
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

                // M3
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("M3")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "#0.0000000";
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

                // C4
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("C4")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "#0.00000000";
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

                // Vm
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Vm")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##,###,##0";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "99999999",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"

                };

                // U
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("U")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##,##0.00";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "99999",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"

                };

                // Tmp
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tmp")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##0.00";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
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

                // R4s
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("R4s")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##0.00";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
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

                // Im
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Im")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##,##0.00";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "99999",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"

                };

                // Cap
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cap")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##,###,##0";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "99999999",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"

                };

                // Tm
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tm")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##0.00";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
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

                // Rm
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Rm")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##0.0000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
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

                // Tr
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tr")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##0.00";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
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

                // Pfe
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pfe")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##,###,##0";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "99999999",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"

                };

                // GARANTIA
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Garantia")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##,###,##0";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "99999999",
                    AllowNulls = false,
                    MessageTemplate = $"{messageErrorNumeric}",
                    Type = "reject",
                    TitleTemplate = "Error"

                };
                #endregion

                #region DataCalculate
                // V
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("V")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "###,##0";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                // I
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("I")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##0.000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                // Lxp
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Lxp")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "#0.00";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                // Rxp
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Rxp")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##0.000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                // P
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("P")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "###,##0";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                // Xm
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Xm")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##0.00";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                // Xc
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Xc")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##0.00";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                // Porc_Desv
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Porc_Desv")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##0.000000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                // Pjm
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pjm")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##,###,##0";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                // Fc
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fc")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##0.00000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                // Pjmc
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pjmc")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "###,##0";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                // Pe
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pe")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "###,##0";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                // Fc2
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fc2")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "#,##0.000";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                // Pt
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pt")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "###,##0";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                #endregion
            }
            catch (Exception ex) {
                throw;
            }
        }

        public void Prepare_PRD_Test(SettingsToDisplayPRDReportsDTO reportsDTO, Workbook workbook, ref PRDTestsDTO _pceTestDTO)
        {
            #region Reading Columns
            int[] _positionWB;

            // Cn
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cn")).Celda);
            _pceTestDTO.PRDTestsDetails.Cn = decimal.Parse(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString(), NumberStyles.Float);

            // M3
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("M3")).Celda);
            _pceTestDTO.PRDTestsDetails.M3 = decimal.Parse(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString(), NumberStyles.Float);

            // C4
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("C4")).Celda);
            _pceTestDTO.PRDTestsDetails.C4 = decimal.Parse(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString(), NumberStyles.Float);

            // Vm
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Vm")).Celda);
            _pceTestDTO.PRDTestsDetails.Vm = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());

            // U
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("U")).Celda);
            _pceTestDTO.PRDTestsDetails.U = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());

            // Tmp
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tmp")).Celda);
            _pceTestDTO.PRDTestsDetails.Tmp = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());

            // R4s
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("R4s")).Celda);
            _pceTestDTO.PRDTestsDetails.R4s = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());

            // Im
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Im")).Celda);
            _pceTestDTO.PRDTestsDetails.Im = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());

            // Cap
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cap")).Celda);
            _pceTestDTO.PRDTestsDetails.Cap = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());

            // Tm
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tm")).Celda);
            _pceTestDTO.PRDTestsDetails.Tm = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());

            // Rm
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Rm")).Celda);
            _pceTestDTO.PRDTestsDetails.Rm = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());

            // Tr
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tr")).Celda);
            _pceTestDTO.PRDTestsDetails.Tr = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());

            // Pfe
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pfe")).Celda);
            _pceTestDTO.PRDTestsDetails.Pfe = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());

            // GARANTIA
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Garantia")).Celda);
            _pceTestDTO.PRDTestsDetails.Warranty = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());

            #endregion

        }

        public void PrepareIndexOfPRD(ResultPRDTestsDTO resultPRDTestsDTO, SettingsToDisplayPRDReportsDTO reportsDTO, ref Workbook workbook)
        {
            // V
            int[]  _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("V")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultPRDTestsDTO.PRDTests.PRDTestsDetails.V.ToString();

            // I
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("I")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultPRDTestsDTO.PRDTests.PRDTestsDetails.I.ToString();

            // Lxp
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Lxp")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultPRDTestsDTO.PRDTests.PRDTestsDetails.Lxp.ToString();

            // Rxp
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Rxp")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultPRDTestsDTO.PRDTests.PRDTestsDetails.Rxp.ToString();

            // P
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("P")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultPRDTestsDTO.PRDTests.PRDTestsDetails.P.ToString();

            // Xm
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Xm")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultPRDTestsDTO.PRDTests.PRDTestsDetails.Xm.ToString();

            // Xc
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Xc")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultPRDTestsDTO.PRDTests.PRDTestsDetails.Xc.ToString();

            // Porc_Desv
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Porc_Desv")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultPRDTestsDTO.PRDTests.PRDTestsDetails.PorcDesv.ToString();

            // Pjm
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pjm")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultPRDTestsDTO.PRDTests.PRDTestsDetails.Pjm.ToString();

            // Fc
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fc")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultPRDTestsDTO.PRDTests.PRDTestsDetails.Fc.ToString();

            // Pjmc
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pjmc")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultPRDTestsDTO.PRDTests.PRDTestsDetails.Pjmc.ToString();

            // Pe
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pe")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultPRDTestsDTO.PRDTests.PRDTestsDetails.Pe.ToString();

            // Fc2
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fc2")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultPRDTestsDTO.PRDTests.PRDTestsDetails.Fc2.ToString();

            // Pt
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pt")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = resultPRDTestsDTO.PRDTests.PRDTestsDetails.Pt.ToString();
        }

        public DateTime GetDate(Workbook origin, SettingsToDisplayPRDReportsDTO reportsDTO)
        {
            ConfigurationReportsDTO fechaCelda = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha"));
            int[] _positionWB = this.GetRowColOfWorbook(fechaCelda.Celda);
            string fecha = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
            DateTime date = DateTime.TryParseExact(fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime objDT)
                ? objDT
                : DateTime.Now.Date;
            return date;
        }

        public bool Verify_PRD_Columns(SettingsToDisplayPRDReportsDTO reportsDTO, Workbook workbook)
        {
            IEnumerable<ConfigurationReportsDTO> starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Cn") || c.Dato.Equals("M3") || c.Dato.Equals("C4") || c.Dato.Equals("Vm") || c.Dato.Equals("U") || c.Dato.Equals("Tmp") || c.Dato.Equals("R4s") || c.Dato.Equals("Im") || c.Dato.Equals("") || c.Dato.Equals("") || c.Dato.Equals("") || c.Dato.Equals("") || c.Dato.Equals("") || c.Dato.Equals("") || c.Dato.Equals("Cap") || c.Dato.Equals("Tm") || c.Dato.Equals("Rm") || c.Dato.Equals("Tr") || c.Dato.Equals("Pfe") || c.Dato.Equals("Garantia") || c.Dato.Equals("V") || c.Dato.Equals("I") || c.Dato.Equals("Lxp") || c.Dato.Equals("Rxp") || c.Dato.Equals("P") || c.Dato.Equals("Xm") || c.Dato.Equals("Xc") || c.Dato.Equals("Porc_Desv") || c.Dato.Equals("Pjm") || c.Dato.Equals("Fc") || c.Dato.Equals("Pjmc") || c.Dato.Equals("Pe") || c.Dato.Equals("Fc2") || c.Dato.Equals("Pt") || c.Dato.Equals("Fecha"));
            int[] _positionWB;
            foreach (ConfigurationReportsDTO item in starts)
            {
                _positionWB = this.GetRowColOfWorbook(item.Celda);
                string cell = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
                if(cell is null or "")
                {
                    return false;
                }
            }

            return true;
        }

        public bool Verify_PRD_ColumnsToCalculate(SettingsToDisplayPRDReportsDTO reportsDTO, Workbook workbook)
        {
            IEnumerable<ConfigurationReportsDTO> starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Cn") || c.Dato.Equals("M3") || c.Dato.Equals("C4") || c.Dato.Equals("Vm") || c.Dato.Equals("U") || c.Dato.Equals("Tmp") || c.Dato.Equals("R4s") || c.Dato.Equals("Im") || c.Dato.Equals("Cap") || c.Dato.Equals("Tm") || c.Dato.Equals("Rm") || c.Dato.Equals("Tr") || c.Dato.Equals("Pfe") || c.Dato.Equals("Garantia") || c.Dato.Equals("Fecha"));
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
