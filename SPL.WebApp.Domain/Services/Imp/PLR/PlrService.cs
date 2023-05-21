namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public class PlrService : IPlrService
    {
        #region Error message
        private readonly ICorrectionFactorService _correcctionFactor;
        private readonly INozzleInformationService _nozzleInformationService;
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion
        public PlrService(ICorrectionFactorService correcctionFactor , INozzleInformationService nozzleInformationService)
        {
            this._correcctionFactor = correcctionFactor;
            this._nozzleInformationService = nozzleInformationService;
        }
        public void PrepareTemplate_PLR(SettingsToDisplayPLRReportsDTO reportsDTO, ref Workbook workbook,  string ClavePrueba, int? cantidadTension = 0 , int? cantidadTiempo = 0, decimal? reactanciaLineal = 0)
        {
            try
            {
               
                string data = "";
                for (int i = 0; i < 60; i++)
                {
                    if (workbook.Sheets[0].Rows[i] != null)
                    {
                        if (workbook.Sheets[0].Rows[i].Cells != null)
                        {
                            foreach (Cell cell in workbook.Sheets[0].Rows[i].Cells)
                            {
                                if (cell != null)
                                {
                                    data += cell.Value?.ToString() ?? "null";
                                    data += ",";
                                }
                                else
                                {
                                    data += "null,";
                                }
                            }
                        }
                        else
                        {
                            data += "null";
                        }
                    }
                    data += Environment.NewLine;
                }

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

                if(cantidadTension > 0)
                {
                    BorderStyle boder = new()
                    {
                        Color = "Black",
                        Size = 1
                    };

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension") && c.ClavePrueba == ClavePrueba).Celda);

                    for(int i = 1; i <= cantidadTension; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Color = "Black";
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Format = "#,##0.0##";
                        workbook.Sheets[0].Rows[_positionWB[0] + (i-1)].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "between",
                            From = "0.001",
                            To = "999999.999",
                            AllowNulls = false,
                            MessageTemplate = "El valor de la tensión kv debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                            Type = "reject",
                            TitleTemplate = "Error"

                        };

                        workbook.Sheets[0].Rows[_positionWB[0] + (i- 1 )].Cells[_positionWB[1]].BorderBottom = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i -1 )].Cells[_positionWB[1]].BorderLeft = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i -1)].Cells[_positionWB[1]].BorderRight = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i -1 )].Cells[_positionWB[1]].BorderTop = boder;
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Corriente") && c.ClavePrueba == ClavePrueba).Celda);

                    for (int i = 1; i <= cantidadTension; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Format = "#,##0.0##";
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Color = "Black";
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "between",
                            From = "0.001",
                            To = "999999.999",
                            AllowNulls = false,
                            MessageTemplate = "El valor de la corriente amps debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                            Type = "reject",
                            TitleTemplate = "Error"

                        };

                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderBottom = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderLeft = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderRight = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderTop = boder;
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Reactancia") && c.ClavePrueba == ClavePrueba).Celda);

                    for (int i = 1; i <= cantidadTension; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderBottom = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderLeft = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderRight = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderTop = boder;
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ReacLineal") && c.ClavePrueba == ClavePrueba).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reactanciaLineal;
                }

                if(cantidadTiempo > 0)
                {
                    BorderStyle boder = new()
                    {
                        Color = "Black",
                        Size = 1
                    };

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo") && c.ClavePrueba == ClavePrueba).Celda);

                    for (int i = 1; i <= cantidadTiempo; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Color = "Black";
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Format = "#,#0.0#";
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "between",
                            From = "0.01",
                            To = "999.99",
                            AllowNulls = false,
                            MessageTemplate = "El valor del tiempo seg debe ser numérico mayor a cero considerando 3 enteros con 2 decimales",
                            Type = "reject",
                            TitleTemplate = "Error"

                        };

                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderBottom = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderLeft = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderRight = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderTop = boder;
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension") && c.ClavePrueba == ClavePrueba).Celda);

                    for (int i = 1; i <= cantidadTiempo; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Color = "Black";
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Format = "#,##0.0##";
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "between",
                            From = "0.001",
                            To = "999999.999",
                            AllowNulls = false,
                            MessageTemplate = "El valor de la tensión kv debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                            Type = "reject",
                            TitleTemplate = "Error"

                        };

                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderBottom = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderLeft = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderRight = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderTop = boder;
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Corriente") && c.ClavePrueba == ClavePrueba).Celda);

                    for (int i = 1; i <= cantidadTiempo; i++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Color = "Black";
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Format = "#,##0.0##";
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "between",
                            From = "0.001",
                            To = "999999.999",
                            AllowNulls = false,
                            MessageTemplate = "El valor de la corriente amps debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                            Type = "reject",
                            TitleTemplate = "Error"

                        };

                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderBottom = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderLeft = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderRight = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].BorderTop = boder;
                    }
                }
            }
            catch (Exception ex) {
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
