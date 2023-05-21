namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Helpers;

    using Telerik.Web.Spreadsheet;

    public class PimService : IPimService
    {
        #region Error message
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion

        public void PrepareTemplate_PIM(SettingsToDisplayPIMReportsDTO reportsDTO, ref Workbook workbook)
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
                // Terminals
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal")).Celda);
                BorderStyle boder = new()
                {
                    Color = "Black",
                    Size = 1
                };
                for (int i = 0; i < reportsDTO.Terminals.Count; i++)
                {
                    // Terminal
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = reportsDTO.Terminals.ElementAt(i);
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderTop = boder;
                   
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderRight = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderBottom = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderLeft = boder;

                    // Pagina
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].FontFamily = "Arial Unicode MS";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].BorderTop = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].BorderRight = boder;
                
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].BorderBottom = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].BorderLeft = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Enable = true;
                }

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotalPagina")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    AllowNulls = false,
                    MessageTemplate = $"El total de páginas debe ser mayor a cero considerando 3 enteros sin decimales",
                    ComparerType = "between",
                    From = "1",
                    To = "999",
                    Type = "reject",
                    TitleTemplate = "Error",
                    ShowButton = true
                };

                #endregion
            }
            catch (Exception ex) {
                throw;
            }
        }

        public void Prepare_PIM_Test(SettingsToDisplayPIMReportsDTO reportsDTO, Workbook workbook, ref PIMTestsGeneralDTO _pimTestDTO)
        {
            int[] _positionWB;
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal")).Celda);
            string coolingType = "Frio";
            int row = 0;
            while (!string.IsNullOrEmpty(coolingType))
            {
                _pimTestDTO.Data.Add(new PIMTestsDetailsDTO()
                {
                    Terminal = workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1]].Value.ToString(),
                    Page = workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 1].Value.ToString()
                });
                coolingType = workbook.Sheets[0].Rows[_positionWB[0] + row + 1].Cells[_positionWB[1]].Value?.ToString();
                row++;
            }

            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotalPagina")).Celda);
            _pimTestDTO.TotalPags = Convert.ToInt32(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString());
        }

        public DateTime GetDate(Workbook origin, SettingsToDisplayPIMReportsDTO reportsDTO)
        {
            ConfigurationReportsDTO fechaCelda = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha"));
            int[] _positionWB = this.GetRowColOfWorbook(fechaCelda.Celda);
            string fecha = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
            return DateUtils.getDateFromSheet(fecha);
        }

        public bool Verify_PIM_Columns(SettingsToDisplayPIMReportsDTO reportsDTO, Workbook workbook)
        {
            int[] _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotalPagina")).Celda);
            string cell = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
            if (cell is null or "")
            {
                return false;
            }

            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal")).Celda);
            string coolingType = "Frio";
            int row = 0;
            while (!string.IsNullOrEmpty(coolingType))
            {
                string terminal = workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1]].Value?.ToString();
                string page = workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + 1].Value?.ToString();

                if (string.IsNullOrEmpty(terminal) || string.IsNullOrEmpty(page))
                {
                    return false;
                }

                coolingType = workbook.Sheets[0].Rows[_positionWB[0] + row + 1].Cells[_positionWB[1]].Value?.ToString();
                row++;
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
