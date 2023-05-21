namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;

    using Telerik.Web.Spreadsheet;

    public class RdtService : IRdtService
    {
        private readonly List<string> columns = new() { "Columna1",
            "Columna2",
            "Columna3",
            "Desv1",
            "Desv2",
            "Desv3",
            "NoPosAT",
            "ValorNominal"
            };

        #region Error message
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion

        public void PrepareTemplate_RDT(SettingsToDisplayRDTReportsDTO reportsDTO, ref Workbook workbook, string angularDisplacement, string testType, string posAT, string posBT, string posTer, int ConexionSp)
        {
            #region Update Readonly all cells
            workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));
            workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.FontFamily = "Arial"));
            #endregion

            #region Head

            int[] _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cliente")).Celda);

            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.Client;
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = false;

            if (ConexionSp == -1)
            {
                _positionWB = GetRowColOfWorbook("A8");
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]+1].Value = "";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]+2].Value = "";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]+3].Value = "";
            }

            

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
            #endregion

            #region Body

            #region Positions

            int[] _titPositionUnic = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPosUnica")).Celda);
            int[] _Titposition = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPosicion")).Celda);

            _ = Enum.TryParse(testType, out TestType test);
            ColumnTitleRDTReportsDTO titPosUnica = reportsDTO.TitleOfColumns.FirstOrDefault();

            if(titPosUnica == null)
            {
                throw new Exception("No se han encontrado columnas para los filtros seleccionados");
            }

            switch (test)
            {
                case TestType.ATT:

                    if(posAT.ToUpper().Equals("TODAS") && !posTer.ToUpper().Equals("TODAS"))
                    {
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1]].Value = titPosUnica.TitPosUnica1;
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1]].Enable = false;

                        workbook.Sheets[0].Rows[_Titposition[0]].Cells[_Titposition[1]].Value = titPosUnica.TitPos1;
                        workbook.Sheets[0].Rows[_Titposition[0]].Cells[_Titposition[1]].Enable = false;

                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1] + 1].Value = posTer;
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1] + 1].Enable = false;
                    }
                    else if (posTer.ToUpper().Equals("TODAS") && !posAT.ToUpper().Equals("TODAS"))
                    {
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1]].Value = titPosUnica.TitPosUnica2;
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1]].Enable = false;

                        workbook.Sheets[0].Rows[_Titposition[0]].Cells[_Titposition[1]].Value = titPosUnica.TitPos2;
                        workbook.Sheets[0].Rows[_Titposition[0]].Cells[_Titposition[1]].Enable = false;

                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1] + 1].Value = posAT;
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1] + 1].Enable = false;
                    }

                    break;
                case TestType.BTT:

                    if (posBT.ToUpper().Equals("TODAS") && !posTer.ToUpper().Equals("TODAS"))
                    {
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1]].Value = titPosUnica.TitPosUnica1;
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1]].Enable = false;

                        workbook.Sheets[0].Rows[_Titposition[0]].Cells[_Titposition[1]].Value = titPosUnica.TitPos1;
                        workbook.Sheets[0].Rows[_Titposition[0]].Cells[_Titposition[1]].Enable = false;

                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1] + 1].Value = posTer;
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1] + 1].Enable = false;
                    }
                    else if (posTer.ToUpper().Equals("TODAS") && !posBT.ToUpper().Equals("TODAS"))
                    {
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1]].Value = titPosUnica.TitPosUnica2;
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1]].Enable = false;

                        workbook.Sheets[0].Rows[_Titposition[0]].Cells[_Titposition[1]].Value = titPosUnica.TitPos2;
                        workbook.Sheets[0].Rows[_Titposition[0]].Cells[_Titposition[1]].Enable = false;

                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1] + 1].Value = posBT;
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1] + 1].Enable = false;
                    }

                    break;
                case TestType.ABT:
                    if (posAT.ToUpper().Equals("TODAS") && !posBT.ToUpper().Equals("TODAS"))
                    {
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1]].Value = titPosUnica.TitPosUnica1;
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1]].Enable = false;

                        workbook.Sheets[0].Rows[_Titposition[0]].Cells[_Titposition[1]].Value = titPosUnica.TitPos1;
                        workbook.Sheets[0].Rows[_Titposition[0]].Cells[_Titposition[1]].Enable = false;

                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1] + 1].Value = posBT;
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1] + 1].Enable = false;
                    }
                    else if (posBT.ToUpper().Equals("TODAS") && !posAT.ToUpper().Equals("TODAS"))
                    {
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1]].Value = titPosUnica.TitPosUnica2;
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1]].Enable = false;

                        workbook.Sheets[0].Rows[_Titposition[0]].Cells[_Titposition[1]].Value = titPosUnica.TitPos2;
                        workbook.Sheets[0].Rows[_Titposition[0]].Cells[_Titposition[1]].Enable = false;

                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1] + 1].Value = posAT;
                        workbook.Sheets[0].Rows[_titPositionUnic[0]].Cells[_titPositionUnic[1] + 1].Enable = false;
                    }
                    break;
                default:
                    break;
            }
            #endregion

            #region Columns

            for (int i = 0; i < reportsDTO.TitleOfColumns.Count; i++)
            {
                switch (i)
                {
                    case 0: _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitColuma1")).Celda);
                        break;
                    case 1: _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitColuma2")).Celda);
                        break;
                    case 2: _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitColuma3")).Celda);
                        break;
                    default:
                        break;
                }

                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.TitleOfColumns[i].Titulo;
                workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].Value = reportsDTO.TitleOfColumns[i].PrimerRenglon;
                workbook.Sheets[0].Rows[_positionWB[0] + 2].Cells[_positionWB[1]].Value = reportsDTO.TitleOfColumns[i].SegundoRenglon;

                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = false;
                workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].Enable = false;
                workbook.Sheets[0].Rows[_positionWB[0] + 2].Cells[_positionWB[1]].Enable = false;
            }

            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPosAT")).Celda);
            int positionIndex = 0;
            BorderStyle boder = new()
            {
                Color = "Black",
                Size = 1
            };
            foreach (string item in reportsDTO.ValuePositions)
            {
                workbook.Sheets[0].Rows[_positionWB[0] + positionIndex].Cells[_positionWB[1]].Value = item;

                for (int i = 0; i < 8; i++)
                {
                    workbook.Sheets[0].Rows[_positionWB[0] + positionIndex].Cells[_positionWB[1] + i].Enable = i % 2 == 0;
                    workbook.Sheets[0].Rows[_positionWB[0] + positionIndex].Cells[_positionWB[1] + i].BorderTop = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + positionIndex].Cells[_positionWB[1] + i].BorderRight = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + positionIndex].Cells[_positionWB[1] + i].BorderBottom = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + positionIndex].Cells[_positionWB[1] + i].BorderLeft = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + positionIndex].Cells[_positionWB[1] + i].FontFamily = "Arial Unicode MS";
                    if(i != 0)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + positionIndex].Cells[_positionWB[1] + i].Format = i is 1 || i is 2 || i is 4 || i is 6 ? "#,##0.0000" : "#,##0.00";
                        workbook.Sheets[0].Rows[_positionWB[0] + positionIndex].Cells[_positionWB[1] + i].Validation = new Validation()
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

                workbook.Sheets[0].Rows[_positionWB[0] + positionIndex].Cells[_positionWB[1]].Enable = false;
                positionIndex++;
            }
            #endregion

            #region Footer

            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DespAng")).Celda);
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = angularDisplacement;
            #endregion
            #endregion
        }

        public bool Verify_RDT_Columns(SettingsToDisplayRDTReportsDTO reportsDTO, Workbook workbook)
        {
            List<string> positions = new();
            List<ColumnDTO> columns = new();
            columns.Add(new ColumnDTO() { Name = "Column1", Values = new List<decimal>() });
            columns.Add(new ColumnDTO() { Name = "Column2", Values = new List<decimal>() });
            columns.Add(new ColumnDTO() { Name = "Column2", Values = new List<decimal>() });

            // Obteniendo AllTension
            int[] _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPosAT")).Celda);

            string cell = "NOM";
            int count = _positionWB[0];
            while (cell is not "" and not null)
            {
                cell = workbook.Sheets[0].Rows[count].Cells[_positionWB[1]].Value?.ToString();
                if (cell is not "" and not null)
                {
                    positions.Add(cell);
                }
                count++;
            }

            // Obteniendo Column 1
            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Columna1")).Celda);

            cell = "0";
            count = _positionWB[0];
            while (cell is not "" and not null)
            {
                cell = workbook.Sheets[0].Rows[count].Cells[_positionWB[1]].Value?.ToString();
                if (cell is not "" and not null)
                {
                    columns[0].Values.Add(Convert.ToDecimal(cell));
                }
                count++;
            }

            // Obteniendo Column 2
            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Columna2")).Celda);

            cell = "0";
            count = _positionWB[0];
            while (cell is not "" and not null)
            {
                cell = workbook.Sheets[0].Rows[count].Cells[_positionWB[1]].Value?.ToString();
                if (cell is not "" and not null)
                {
                    columns[1].Values.Add(Convert.ToDecimal(cell));
                }
                count++;
            }

            // Obteniendo Column 3
            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Columna3")).Celda);

            cell = "0";
            count = _positionWB[0];
            while (cell is not "" and not null)
            {
                cell = workbook.Sheets[0].Rows[count].Cells[_positionWB[1]].Value?.ToString();
                if (cell is not "" and not null)
                {
                    columns[2].Values.Add(Convert.ToDecimal(cell));
                }
                count++;
            }
            int rows = positions.Count;

            return columns[0].Values.Count == rows && columns[1].Values.Count == rows && columns[2].Values.Count == rows;
        }

        public void Prepare_RDT_Test(string testType, string aT, string bT, string ter, GeneralPropertiesDTO angular, SettingsToDisplayRDTReportsDTO reportsDTO, Workbook workbook, List<PlateTensionDTO> plateTensions, ref RDTTestsDTO _rdtTestDTO)
        {

            #region Reading Columns

            _rdtTestDTO.Positions = new();
            _rdtTestDTO.Columns = new();
            _rdtTestDTO.Columns.Add(new ColumnDTO() { Name = "Column1", Values = new List<decimal>() });
            _rdtTestDTO.Columns.Add(new ColumnDTO() { Name = "Column2", Values = new List<decimal>() });
            _rdtTestDTO.Columns.Add(new ColumnDTO() { Name = "Column2", Values = new List<decimal>() });

            // Obteniendo AllTension
            int[] _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoPosAT")).Celda);

            string cell = "NOM";
            int count = _positionWB[0];
            while (cell is not "" and not null)
            {
                cell = workbook.Sheets[0].Rows[count].Cells[_positionWB[1]].Value?.ToString();
                if(cell is not "" and not null)
                {
                    _rdtTestDTO.Positions.Add(cell);
                }
                count++;
            }

            // Obteniendo Column 1
            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Columna1")).Celda);

            cell = "0";
            count = _positionWB[0];
            while (cell is not "" and not null)
            {
                cell = workbook.Sheets[0].Rows[count].Cells[_positionWB[1]].Value?.ToString();
                if (cell is not "" and not null)
                {
                    _rdtTestDTO.Columns[0].Values.Add(Convert.ToDecimal(cell));
                }
                count++;
            }

            // Obteniendo Column 2
            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Columna2")).Celda);

            cell = "0";
            count = _positionWB[0];
            while (cell is not "" and not null)
            {
                cell = workbook.Sheets[0].Rows[count].Cells[_positionWB[1]].Value?.ToString();
                if (cell is not "" and not null)
                {
                    _rdtTestDTO.Columns[1].Values.Add(Convert.ToDecimal(cell));
                }
                count++;
            }

            // Obteniendo Column 3
            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Columna3")).Celda);

            cell = "0";
            count = _positionWB[0];
            while (cell is not "" and not null)
            {
                cell = workbook.Sheets[0].Rows[count].Cells[_positionWB[1]].Value?.ToString();
                if (cell is not "" and not null)
                {
                    _rdtTestDTO.Columns[2].Values.Add(Convert.ToDecimal(cell));
                }
                count++;
            }

            #endregion

            _rdtTestDTO.Tensions = new();

            if(testType == "ABT")
            {
                _rdtTestDTO.AllTension = aT == "Todas" ? "AT" : "BT";
                _rdtTestDTO.UnitTension = aT == "Todas" ? "BT" : "AT";
            }
            else if(testType == "ATT")
            {
                _rdtTestDTO.AllTension = aT == "Todas" ? "AT" : "Ter";
                _rdtTestDTO.UnitTension = aT == "Todas" ? "Ter" : "AT";
            }
            else
            {
                _rdtTestDTO.AllTension = bT == "Todas" ? "BT" : "Ter";
                _rdtTestDTO.UnitTension = bT == "Todas" ? "Ter" : "BT";
            }

            string all = _rdtTestDTO.AllTension;
            string unitT = _rdtTestDTO.UnitTension;

            _rdtTestDTO.Tensions = plateTensions.FindAll(ten => ten.TipoTension == all).Select(ten => ten.Tension).ToList(); 
            
            _rdtTestDTO.OrderPositions = plateTensions.FindAll(ten => ten.TipoTension == all).Select(ten =>     Convert.ToInt32(ten.Orden)).ToList();

            _rdtTestDTO.UnitValue = plateTensions.Find(ten => ten.TipoTension.ToUpper() == unitT.ToUpper() && ten.Posicion.ToUpper() == (unitT == "AT" ? aT.ToUpper() : unitT == "BT" ? bT.ToUpper() : ter.ToUpper())) is null ? 0 :
            plateTensions.Find(ten => ten.TipoTension.ToUpper() == unitT.ToUpper() && ten.Posicion.ToUpper() == (unitT == "AT" ? aT.ToUpper() : unitT == "BT" ? bT.ToUpper() : ter.ToUpper())).Tension;

            _rdtTestDTO.AcceptanceValue = Convert.ToDecimal(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptacion")).Formato.Replace('.',','));

            _rdtTestDTO.AngularDisplacement = angular;

            _rdtTestDTO.Fechacreacion = DateTime.Now;
        }

        public void PrepareIndexOfRDT(ResultRDTTestsDetailsDTO resultRDTTestsDetailsDTO, SettingsToDisplayRDTReportsDTO reportsDTO, string idioma, ref Workbook workbook)
        {
            int[] nominalPos = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorNominal")).Celda);
            int[] desv1Pos = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Desv1")).Celda);
            int[] desv2Pos = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Desv2")).Celda);
            int[] desv3Pos = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Desv3")).Celda);

            for (int i = 0; i < resultRDTTestsDetailsDTO.NominalValue.Count; i++)
            {
                workbook.Sheets[0].Rows[nominalPos[0] + i].Cells[nominalPos[1]].Value = resultRDTTestsDetailsDTO.NominalValue[i];
                workbook.Sheets[0].Rows[desv1Pos[0] + i].Cells[desv1Pos[1]].Value = resultRDTTestsDetailsDTO.DeviationPhasesA[i];
                workbook.Sheets[0].Rows[desv2Pos[0] + i].Cells[desv2Pos[1]].Value = resultRDTTestsDetailsDTO.DeviationPhasesB[i];
                workbook.Sheets[0].Rows[desv3Pos[0] + i].Cells[desv3Pos[1]].Value = resultRDTTestsDetailsDTO.DeviationPhasesC[i];
            }

            int[] resultLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
            bool resultReport = !resultRDTTestsDetailsDTO.MessageErrors.Any();

            workbook.Sheets[0].Rows[resultLocation[0]].Cells[resultLocation[1]].Value = resultReport ?
                reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorAceptado{idioma}")).Formato :
                reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorRechazado{idioma}")).Formato;
        }

        public void CloneWorkbook(Workbook origin, SettingsToDisplayRDTReportsDTO reportsDTO, ref Workbook official, out DateTime date)
        {
            IEnumerable<ConfigurationReportsDTO> columns = reportsDTO.ConfigurationReports.Where(c => this.columns.Exists(x => x.Equals(c.Dato))).Distinct();
            int[] _positionWB;
            // Copiando Todas las columnas
            foreach (ConfigurationReportsDTO column in columns)
            {
                _positionWB = GetRowColOfWorbook(column.Celda);
                CopyColumn(origin, _positionWB, ref official);
            }

            //Copiando resultado
            // Copiando fecha
            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
            official.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value;

            // Copiando fecha
            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha")).Celda);
            official.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value;
            string fecha = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();

           

            if (DateTime.TryParseExact(fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime objDT))
            {
                date = objDT;
            }
            else
            {
                double value = double.Parse(fecha);
                DateTime date1 = DateTime.FromOADate(value);
                date = date1;
            }

            //official.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = date.ToString("MM/dd/yyyy");


        }

        #region Private Methods

        private static void CopyColumn(Workbook origin, int[] position, ref Workbook official)
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
