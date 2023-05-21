namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public class CemService : ICemService
    {
        #region Error message
        private readonly ICorrectionFactorService _correcctionFactor;
        private readonly INozzleInformationService _nozzleInformationService;
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion
        public CemService(ICorrectionFactorService correcctionFactor , INozzleInformationService nozzleInformationService)
        {
            this._correcctionFactor = correcctionFactor;
            this._nozzleInformationService = nozzleInformationService;
        }
        public void PrepareTemplate_CEM(SettingsToDisplayCEMReportsDTO reportsDTO, ref Workbook workbook, string tensionPrimaria, string tensionSecundaria, string posicionPrimaria, string posicionesSecundarias, string voltage,string idioma)
        {
            try
            {
                #region Update Readonly all cells
                workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));
                #endregion

                List<string> secPositions = posicionesSecundarias.Split(',').ToList();

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

                // Titulo primaria
                string nombrePospri = string.Empty;
                if (idioma.Equals("EN"))
                    nombrePospri = tensionPrimaria.Equals("AT") ? "HV" : tensionPrimaria.Equals("BT") ? "LV" : "TV";

                if (idioma.Equals("ES"))
                    nombrePospri = tensionPrimaria;


                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPosPrim")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = nombrePospri;

                // Posicion primaria
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosPrim")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = posicionPrimaria;

                // Titulo secundaria
                /*if (idioma.Equals("ES"))
                {
                    _positionWB = this.GetRowColOfWorbook("E13");
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = $"Prueba de {reportsDTO.SecondaryPositions.FirstOrDefault().TitPos1}";
        
                }
                else
                {
                    _positionWB = this.GetRowColOfWorbook("E12");
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = $"{reportsDTO.SecondaryPositions.FirstOrDefault().TitPos1} Tap";
                  
                }*/

                // Voltaje de Prueba
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VoltajePrueba")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = voltage;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Color = "Black";
                //workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "###,##0.0";
                //workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                //{
                //    DataType = "number",
                //    ComparerType = "greaterThan",
                //    From = "0",
                //    To = "999999.9",
                //    AllowNulls = false,
                //    MessageTemplate = $"{messageErrorNumeric}",
                //    Type = "reject",
                //    TitleTemplate = "Error"
                //};

                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "#,##0.0##";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "between",
                    From = "0.1",
                    To = "999999.9",
                    AllowNulls = false,
                    MessageTemplate = "El valor del voltaje debe ser numérico mayor a cero considerando 6 enteros con 1 decimal",
                    Type = "reject",
                    TitleTemplate = "Error"

                };



                // Titulos columnas
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitColumna1")).Celda);
                var columnas = reportsDTO.SecondaryPositions.OrderBy(x => x.PosColumna).ToList();

                if((tensionPrimaria == "AT" || tensionPrimaria == "BT") &&(tensionSecundaria == "AT" ||tensionSecundaria =="BT"))
                {
                    if (tensionSecundaria == "AT")
                    {

                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = columnas[0].PrimerRenglon;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Value = columnas[1].PrimerRenglon;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Value = columnas[2].PrimerRenglon;

                        if (idioma.Equals("ES"))
                        {
                            _positionWB = this.GetRowColOfWorbook("E13");
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = $"Prueba de {tensionSecundaria}";
                        }
                        else
                        {
                            _positionWB = this.GetRowColOfWorbook("E12");
                            string nom = tensionSecundaria.Equals("AT") ? "HV" : tensionSecundaria.Equals("BT") ? "LV" : "TV";
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = $"{nom} Tap";

                        }
                    }
                    else if (tensionSecundaria == "BT")
                    {

                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = columnas[0].SegundoRenglon;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Value = columnas[1].SegundoRenglon;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Value = columnas[2].SegundoRenglon;

                        if (idioma.Equals("ES"))
                        {
                            _positionWB = this.GetRowColOfWorbook("E13");
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = $"Prueba de {tensionSecundaria}";

                        }
                        else
                        {
                            _positionWB = this.GetRowColOfWorbook("E12");
                            string nom = tensionSecundaria.Equals("AT") ? "HV" : tensionSecundaria.Equals("BT") ? "LV" : "TV";
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = $"{nom} Tap";

                        }
                    }
                }
                else if ((tensionPrimaria == "AT" || tensionPrimaria.ToUpper() == "TER") && (tensionSecundaria == "AT" || tensionSecundaria.ToUpper() == "TER"))
                {
                    if (tensionSecundaria == "AT")
                    {

                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = columnas[0].PrimerRenglon;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Value = columnas[1].PrimerRenglon;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Value = columnas[2].PrimerRenglon;

                        if (idioma.Equals("ES"))
                        {
                            _positionWB = this.GetRowColOfWorbook("E13");
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = $"Prueba de {tensionSecundaria}";
                        }
                        else
                        {
                            _positionWB = this.GetRowColOfWorbook("E12");
                            string nom = tensionSecundaria.Equals("AT") ? "HV" : tensionSecundaria.Equals("BT") ? "LV" : "TV";
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = $"{nom} Tap";

                        }
                    }
                    else if (tensionSecundaria.ToUpper() == "TER")
                    {

                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = columnas[0].SegundoRenglon;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Value = columnas[1].SegundoRenglon;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Value = columnas[2].SegundoRenglon;

                        if (idioma.Equals("ES"))
                        {
                            _positionWB = this.GetRowColOfWorbook("E13");
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = $"Prueba de {tensionSecundaria}";

                        }
                        else
                        {
                            _positionWB = this.GetRowColOfWorbook("E12");
                            string nom = tensionSecundaria.Equals("AT") ? "HV" : tensionSecundaria.Equals("BT") ? "LV" : "TV";
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = $"{nom} Tap";

                        }
                    }
                }
                else if ((tensionPrimaria == "BT" || tensionPrimaria.ToUpper() == "TER") && (tensionSecundaria == "BT" || tensionSecundaria.ToUpper() == "TER"))
                {
                    if (tensionSecundaria == "BT")
                    {

                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = columnas[0].PrimerRenglon;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Value = columnas[1].PrimerRenglon;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Value = columnas[2].PrimerRenglon;

                        if (idioma.Equals("ES"))
                        {
                            _positionWB = this.GetRowColOfWorbook("E13");
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = $"Prueba de {tensionSecundaria}";
                        }
                        else
                        {
                            _positionWB = this.GetRowColOfWorbook("E12");
                            string nom = tensionSecundaria.Equals("AT") ? "HV" : tensionSecundaria.Equals("BT") ? "LV" : "TV";
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = $"{nom} Tap";

                        }
                    }
                    else if (tensionSecundaria.ToUpper() == "TER")
                    {

                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = columnas[0].SegundoRenglon;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Value = columnas[1].SegundoRenglon;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Value = columnas[2].SegundoRenglon;

                        if (idioma.Equals("ES"))
                        {
                            _positionWB = this.GetRowColOfWorbook("E13");
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = $"Prueba de {tensionSecundaria}";

                        }
                        else
                        {
                            _positionWB = this.GetRowColOfWorbook("E12");
                            string nom = tensionSecundaria.Equals("AT") ? "HV" : tensionSecundaria.Equals("BT") ? "LV" : "TV";
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = $"{nom} Tap";

                        }
                    }
                }
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Validation = new Validation()
                {
                    DataType = "custom",
                    ComparerType = "lessThan",
                    From = "AND(LEN(D" + (_positionWB[0] + 1) + ") <=15)",
                    AllowNulls = false,
                    MessageTemplate = "La primer terminal no puede excederse de 15 caracteres",
                    Type = "reject",
                    TitleTemplate = "Error"
                };
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Validation = new Validation()
                {
                    DataType = "custom",
                    ComparerType = "lessThan",
                    From = "AND(LEN(D" + (_positionWB[0] + 1) + ") <=15)",
                    AllowNulls = false,
                    MessageTemplate = "La segunda terminal no puede excederse de 15 caracteres",
                    Type = "reject",
                    TitleTemplate = "Error"
                };

                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3].Validation = new Validation()
                {
                    DataType = "custom",
                    ComparerType = "lessThan",
                    From = "AND(LEN(D" + (_positionWB[0] + 1) + ") <=15)",
                    AllowNulls = false,
                    MessageTemplate = "La tercer terminal no puede excederse de 15 caracteres",
                    Type = "reject",
                    TitleTemplate = "Error"
                };

                // Data Fill
                BorderStyle boder = new()
                {
                    Color = "Black",
                    Size = 1
                };
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosSec")).Celda);

                for (int i = 0; i < secPositions.Count; i++)
                {
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = secPositions[i].ToUpper();
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderTop = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderRight = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderBottom = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderLeft = boder;
                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].FontFamily = "Arial Unicode MS";

                    for (int j = 0; j < 3; j++)
                    {
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j + 1].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j + 1].BorderTop = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j + 1].BorderRight = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j + 1].BorderBottom = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j + 1].BorderLeft = boder;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j + 1].Format = "###,##0.000";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j + 1].FontFamily = "Arial Unicode MS";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + j + 1].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "between",
                            From = "1",
                            To = "999999.999",
                            AllowNulls = false,
                            MessageTemplate = "La corriente de la terminal debe ser numérica mayor a cero considerando 6 enteros con 3 decimales",
                            Type = "reject",
                            TitleTemplate = "Error"

                        };
                    }
                }

                string finals =  idioma.Equals("ES") ?
                    "Aceptado,Rechazado" : "Accepted,Rejected";
                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "Seleccione...";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                {
                    DataType = "list",
                    ShowButton = true,
                    ComparerType = "list",
                    From = "\"" + finals + "\"",
                    AllowNulls = false,
                    Type = "reject"
                };
            }
            catch (Exception ex) {
                throw;
            }
        }

        public DateTime GetDate(Workbook origin, SettingsToDisplayRCTReportsDTO reportsDTO)
        {
            ConfigurationReportsDTO fechaCelda = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha"));
            int[] _positionWB = this.GetRowColOfWorbook(fechaCelda.Celda);
            string fecha = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();
            DateTime date = DateTime.TryParseExact(fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime objDT)
                ? objDT
                : DateTime.Now.Date;
            return date;
        }

        public bool Verify_CEM_Columns(SettingsToDisplayCEMReportsDTO reportsDTO, Workbook workbook, int cols)
        {
            ConfigurationReportsDTO starts = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("CorrienteTerm1"));
            int[] _positionWB;
            _positionWB = this.GetRowColOfWorbook(starts.Celda);
            for (int i = 0; i < cols; i++)
            {
                int rowsReaded = 0;
                string cell = "NOM";
                int count = _positionWB[0];
                while (cell is not "" and not null)
                {
                    cell = workbook.Sheets[0].Rows[count].Cells[_positionWB[1] + i].Value?.ToString();
                    if (cell is not "" and not null)
                    {
                        rowsReaded++;
                    }
                    count++;
                }
                if (rowsReaded != 3)
                {
                    return false;
                }
            }

            IEnumerable<ConfigurationReportsDTO> aux = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha") || c.Dato.Equals("Resultado"));

            foreach (ConfigurationReportsDTO column in aux)
            {
                _positionWB = this.GetRowColOfWorbook(column.Celda);
                if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString()))
                {
                    return false;
                }
            }

            return true;
        }

        public void Prepare_CEM_Test(SettingsToDisplayCEMReportsDTO reportsDTO, Workbook workbook, ref CEMTestsGeneralDTO _cemTestDTOs, int cols,string idioma,string tensionSecundaria)
        {
            // Tension primaria
            int[] _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPosPrim")).Celda);
            _cemTestDTOs.IdPosPrimary = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();

            // Tension secundaria
            if (idioma.Equals("ES"))
            {
                _cemTestDTOs.IdPosSecundary = tensionSecundaria;
            }
            else
            {
                string nom = tensionSecundaria.Equals("AT") ? "HV" : tensionSecundaria.Equals("BT") ? "LV" : "TV";
                _cemTestDTOs.IdPosSecundary = nom;

            }

            // Posicion Primaria
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosPrim")).Celda);
            _cemTestDTOs.PosPrimary = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();

            // Voltaje de Prueba
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VoltajePrueba")).Celda);
            _cemTestDTOs.TestsVoltage = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString());

            // Titulos
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitColumna1")).Celda);
            _cemTestDTOs.TitTerm1 = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();
            _cemTestDTOs.TitTerm2 = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Value.ToString();
            _cemTestDTOs.TitTerm3 = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2].Value.ToString();

            _cemTestDTOs.CEMTestsDetails = new List<CEMTestsDetailsDTO>();

            #region Reading Columns
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosSec")).Celda);
            for (int i = 0; i < cols; i++)
            {
                _cemTestDTOs.CEMTestsDetails.Add(new CEMTestsDetailsDTO()
                {
                    PosSec = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value.ToString(),
                    CurrentTerm1 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Value.ToString()),
                    CurrentTerm2 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Value.ToString()),
                    CurrentTerm3 = Convert.ToDecimal(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value.ToString())
                });
            
            }
            #endregion

            // Resultado
            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
            _cemTestDTOs.Result = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString() is "Aceptado" or "Accepted";
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
