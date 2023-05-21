namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public class TinService : ITinService
    {
        #region Error message
        private readonly ICorrectionFactorService _correcctionFactor;
        private readonly INozzleInformationService _nozzleInformationService;
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion
        public TinService(ICorrectionFactorService correcctionFactor , INozzleInformationService nozzleInformationService)
        {
            this._correcctionFactor = correcctionFactor;
            this._nozzleInformationService = nozzleInformationService;
        }
        public void PrepareTemplate_Tin(SettingsToDisplayTINReportsDTO reportsDTO, ref Workbook workbook, string keyTest, string languaje, string connection, string voltaje,ref CeldasValidate celdas)
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

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitConexion")).Celda);

                if (connection.ToLower() == "serie")
                {
                    var a = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = languaje == "ES" ? "Conexión Serie" : "Series Connection";
                }
                else if (connection.ToLower() == "paralelo")
                {
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = languaje == "ES" ? "Conexión Paralelo" : "Parallel Connection";
                }
                else if (connection.ToLower() == "tensión 1" || connection.ToLower() == "tensión 2")
                {
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = languaje == "ES" ? "Conexión de " + voltaje +" KV" : voltaje+ " kV connection";
                }
                else
                {
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "";
                }

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosTer")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;

                celdas.CeldaAT = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT")).Celda;
                celdas.CeldaBT = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT")).Celda;
                celdas.CeldaTer = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosTer")).Celda;
                celdas.CeldaDevanadoEnergizado = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DevanadoEner")).Celda;
                celdas.CeldaDevanadoInducido = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DevanadoIndu")).Celda;
                celdas.FrecuenciaPrueba = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("FrecPrueba")).Celda;
                celdas.RelTension = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("RelTension")).Celda;
                celdas.CeldaTiempo = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo")).Celda;

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("RelTension")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("FrecPrueba")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DevanadoEner")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DevanadoIndu")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionAplicada")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "between",
                    From = "1",
                    To = "999999.999",
                    AllowNulls = false,
                    MessageTemplate = "La tensión aplicada debe ser numérica mayor a cero considerando 6 enteros con 3 decimales",
                    Type = "reject",
                    TitleTemplate = "Error"

                };

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionInducida")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "between",
                    From = "1",
                    To = "999999.999",
                    AllowNulls = false,
                    MessageTemplate = "La tensión inducida debe ser numérica mayor a cero considerando 6 enteros con 3 decimales",
                    Type = "reject",
                    TitleTemplate = "Error"

                };

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;

            }
            catch (Exception ex)
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
