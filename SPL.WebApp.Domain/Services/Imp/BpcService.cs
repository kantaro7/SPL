namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public class BpcService : IBpcService
    {
        #region Error message
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion

        public void PrepareTemplate_BPC(SettingsToDisplayBPCReportsDTO reportsDTO, ref Workbook workbook, string claveIdioma)
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


                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorObtenido")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = claveIdioma is "ES" ? "Seleccione..." : "Select...";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "list",
                    ShowButton = true,
                    ComparerType = "list",
                    From = claveIdioma is "ES" ? "\"Detectado,No Detectado\"" : "\"Detected,Not Detected\"",
                    AllowNulls = false,
                    Type = "reject"
                };

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("MetodoUsado")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Notas")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j].Enable = true;
                    }
                }



                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Temperatura")).Celda);

                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "##0.0";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "999",
                    AllowNulls = false,
                    MessageTemplate = "La temperatura en °C debe ser mayor a cero considerando 3 enteros con 1 decimal",
                    Type = "reject",
                    TitleTemplate = "Error"

                };


                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = claveIdioma is "ES" ? "Seleccione..." : "Select...";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "list",
                    ShowButton = true,
                    ComparerType = "list",
                    From = claveIdioma is "ES" ? "\"Aceptado,Rechazado\"" : "\"Accepted,Rejected\"",
                    AllowNulls = false,
                    Type = "reject"
                };
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string Verify_BPC_ColumnsToCalculate(SettingsToDisplayBPCReportsDTO reportsDTO, Workbook workbook)
        {
            string result = string.Empty;
            int[] _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Temperatura")).Celda);
            if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString()))
            {
                result += " La temperatura es requerida. ";
            }
            else
            {
                decimal temp = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());
                if (temp > 999.9m)
                {
                    result += " La temperatura solo ppuede tener 3 enteros. ";
                }
            }
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorObtenido")).Celda);
            if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString()) || (workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString() is "Selecciones.." or "Select..."))
            {
                result += " El Valor Obtenido es requerida. ";
            }
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("MetodoUsado")).Celda);
            string metodo = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
            if (string.IsNullOrEmpty(metodo))
            {
                result += " El Metodo Usado es requerido. ";
            }
            else
            {
                if (metodo.Length <= 15)
                {
                    if (!Regex.IsMatch(metodo, @"^[a-zA-Z0-9\-]+$"))
                    {
                        result += " El Metodo Usado debe contener solo letras, numeros y guiones. ";
                    }
                }
                else
                {
                    result += " El Metodo Usado debe contener menos de 15 caracteres. ";
                }
            }

            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Notas")).Celda);
            string nota = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString() ?? string.Empty + " " + workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].Value?.ToString() ?? string.Empty;
            string nota1 = workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].Value?.ToString() ?? string.Empty + " " + workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].Value?.ToString() ?? string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(nota);
            stringBuilder.AppendLine(nota1);

            nota = stringBuilder.ToString();

            if (nota.Length > 100)
            {
                result += "Notas no puede exceder de 100 caracteres. ";
            }

            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
            if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString()) || (workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString() is "Seleccione..." or "Select..."))
            {
                result += " El Resultado es requerido. ";
            }

            return result;
        }

        public void Prepare_BPC_Test(SettingsToDisplayBPCReportsDTO reportsDTO, Workbook workbook, ref BPCTestsGeneralDTO _bpcTestDTO)
        {
            int[] _positionWB;
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Temperatura")).Celda);

            _bpcTestDTO.Temperature = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());

            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorObtenido")).Celda);

            _bpcTestDTO.ObtainedValue = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();

            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("MetodoUsado")).Celda);

            _bpcTestDTO.MethodologyUsed = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();

            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Notas")).Celda);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString());
            stringBuilder.AppendLine(workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].Value?.ToString());

            _bpcTestDTO.Grades = stringBuilder.ToString();
        }

        public void PrepareIndexOfBPC(SettingsToDisplayBPCReportsDTO reportsDTO, ref Workbook workbook, string idioma)
        {
            int[] resultLocation = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
            bool resultReport = true;

            workbook.Sheets[0].Rows[resultLocation[0]].Cells[resultLocation[1]].Value = resultReport ?
                reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorAceptado{idioma}")).Formato :
                reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorRechazado{idioma}")).Formato;
        }

        public DateTime GetDate(Workbook origin, SettingsToDisplayBPCReportsDTO reportsDTO)
        {
            ConfigurationReportsDTO fechaCelda = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha"));
            int[] _positionWB = this.GetRowColOfWorbook(fechaCelda.Celda);
            string fecha = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
            if (DateTime.TryParseExact(fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime objDT))
            {
                return objDT;
            }
            else
            {
                double value = double.Parse(fecha);
                DateTime date1 = DateTime.FromOADate(value);
                return date1;
            }
        }

        public bool GetResult(Workbook origin, SettingsToDisplayBPCReportsDTO reportsDTO)
        {
            bool result = false;
            int[] _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
            string data = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();

            result = data.ToUpper().Equals("ACEPTADO") || data.ToUpper().Equals("ACCEPTED");

            return result;
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
