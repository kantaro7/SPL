namespace SPL.WebApp.Domain.Services.Imp.PIR
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public class PirService : IPirService
    {
        #region Error message
        private readonly ICorrectionFactorService _correcctionFactor;
        private readonly INozzleInformationService _nozzleInformationService;
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion
        public PirService(ICorrectionFactorService correcctionFactor, INozzleInformationService nozzleInformationService)
        {
            _correcctionFactor = correcctionFactor;
            _nozzleInformationService = nozzleInformationService;
        }
        public void PrepareTemplate_Pir(SettingsToDisplayPIRReportsDTO reportsDTO, ref Workbook workbook, string ClavePrueba, string idioma, DerivationsDTO derivationsDTO, string connectionAt, string connectionBt, string connectionTer, ref int count)
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

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotalPagina")).Celda);
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

                List<string> terminalsAT = new();
                List<string> terminalsBT = new();
                List<string> terminalsTer = new();
                List<string> terminals = new();

                if (derivationsDTO.IdConexionEquivalente is not 0 && connectionAt != string.Empty)
                {
                    if (derivationsDTO.ConexionEquivalente.ToUpper().Contains("DELTA"))
                    {
                        terminalsAT.Add("H1");
                        terminalsAT.Add("H2");
                        terminalsAT.Add("H3");
                    }

                    if (derivationsDTO.ConexionEquivalente.ToUpper().Contains("ESTRELLA") || derivationsDTO.ConexionEquivalente.ToUpper().Contains("WYE"))
                    {
                        terminalsAT.Add("H1");
                        terminalsAT.Add("H2");
                        terminalsAT.Add("H3");
                        terminalsAT.Add("H0");
                    }
                }

                if (derivationsDTO.IdConexionEquivalente2 is not 0 && connectionBt != string.Empty)
                {
                    if (derivationsDTO.ConexionEquivalente_2.ToUpper().Contains("DELTA"))
                    {
                        terminalsBT.Add("X1");
                        terminalsBT.Add("X2");
                        terminalsBT.Add("X3");
                    }

                    if (derivationsDTO.ConexionEquivalente_2.ToUpper().Contains("ESTRELLA") || derivationsDTO.ConexionEquivalente_2.ToUpper().Contains("WYE"))
                    {
                        terminalsBT.Add("X1");
                        terminalsBT.Add("X2");
                        terminalsBT.Add("X3");
                        terminalsBT.Add("X0");
                    }
                }

                if (derivationsDTO.IdConexionEquivalente4 is not 0 && connectionTer != string.Empty)
                {
                    if (derivationsDTO.ConexionEquivalente_4.ToUpper().Contains("DELTA"))
                    {
                        terminalsTer.Add("Y1");
                        terminalsTer.Add("Y2");
                        terminalsTer.Add("Y3");
                    }

                    if (derivationsDTO.ConexionEquivalente_4.ToUpper().Contains("ESTRELLA") || derivationsDTO.ConexionEquivalente_4.ToUpper().Contains("WYE"))
                    {
                        terminalsTer.Add("Y1");
                        terminalsTer.Add("Y2");
                        terminalsTer.Add("Y3");
                        terminalsTer.Add("Y4");
                    }
                }

                if (connectionAt != string.Empty)
                {
                    foreach (string item in connectionAt.Split(','))
                    {
                        if (item.Split('-').Length <= 1)
                        {
                            terminals.AddRange(terminalsAT.Select(x => $"{x}"));
                        }
                        else
                        {
                            terminals.AddRange(terminalsAT.Select(x => $"{x} - {Convert.ToDecimal(item.Split('-')[1]):##0.000}"));
                        }
                    }
                }

                if (connectionBt != string.Empty)
                {
                    foreach (string item in connectionBt.Split(','))
                    {
                        if (item.Split('-').Length <= 1)
                        {
                            terminals.AddRange(terminalsBT.Select(x => $"{x}"));
                        }
                        else
                        {
                            terminals.AddRange(terminalsBT.Select(x => $"{x} - {Convert.ToDecimal(item.Split('-')[1]):##0.000}"));
                        }
                    }
                }

                if (connectionTer != string.Empty)
                {
                    foreach (string item in connectionTer.Split(','))
                    {
                        if (item.Split('-').Length <= 1)
                        {
                            terminals.AddRange(terminalsTer.Select(x => $"{x}"));
                        }
                        else
                        {
                            terminals.AddRange(terminalsTer.Select(x => $"{x} - {Convert.ToDecimal(item.Split('-')[1]):##0.000}"));
                        }
                    }
                }

                int counter = 0;
                foreach (string terminal in terminals)
                {
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal")).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0] + counter].Cells[_positionWB[1]].Value = terminal;
                    counter++;
                }

                BorderStyle boder = new()
                {
                    Color = "Black",
                    Size = 1
                };

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal")).Celda);
                for (int i = 0; i < counter; i++)
                {
                    string a = "AND(LEN(G" + _positionWB[0] + i + ") < 3";
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderBottom = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderLeft = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderTop = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderRight = boder;

                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].BorderBottom = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].BorderLeft = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].BorderTop = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].BorderRight = boder;

                    /*workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Validation = new Validation()
                    {
                        DataType = "custom",
                        AllowNulls = false,
                        MessageTemplate = $"La página no puede excederse de 10 caracteres",
                        From = "REGEXP_MATCH(G"+(_positionWB[0] + i)+", '^\\d+\\s(?:-\\s\\d+)$')",
                        Type = "reject",
                        TitleTemplate = "Error",
                        ShowButton = true
                    };*/
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Enable = true;

                }

                count = counter;

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

    public class GenericPirPrueba
    {
        public List<string> Terminales { get; set; }
        public PirPruebasDTO PirPrueba { get; set; }
    }
}
