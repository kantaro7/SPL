namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public class PciService : IPciService
    {
        #region Error message
        private readonly ICorrectionFactorService _correcctionFactor;
        private readonly INozzleInformationService _nozzleInformationService;
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion
        public PciService(ICorrectionFactorService correcctionFactor, INozzleInformationService nozzleInformationService)
        {
            this._correcctionFactor = correcctionFactor;
            this._nozzleInformationService = nozzleInformationService;
        }
        public void PrepareTemplate_PCI(SettingsToDisplayPCIReportsDTO reportsDTO, ref Workbook workbook, string ClavePrueba, int cantidadPosicionesPrimarias, int catidadPosicionesSecundarias, string PosicionesPrimarias, string PosicionesSecundarias)
        {
            try
            {
                string[] posiPrimarias = PosicionesPrimarias.Split(",");
                string[] posiSecundarias = PosicionesSecundarias.Split(",");

                #region Update Readonly all cells
                workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));
                #endregion
                string[] capacidades = reportsDTO.CapacidadP.Split(',');

                bool onePage = posiSecundarias.Length <= 7;

                int secciones = 0;

                foreach (string capacidad in capacidades)
                {
                    decimal capacity = decimal.Parse(capacidad);

                    decimal temperatureElevation = reportsDTO.InfotmationArtifact.CharacteristicsArtifact
                        .Where(e =>
                            e.Mvaf1 == capacity
                            || e.Mvaf2 == capacity
                            || e.Mvaf3 == capacity
                            || e.Mvaf4 == capacity)
                        .OrderBy(e => e.Secuencia)
                        .Select(e => e.OverElevation)
                        .FirstOrDefault(0m) ?? 0m;

                    int[] _positionWB;
                    if ((onePage && secciones == 0) || !onePage)
                    {
                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cliente") && c.Seccion == secciones + 1).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.Client;

                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoSerie") && c.Seccion == secciones + 1).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.NoSerie;

                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Capacidad") && c.Seccion == secciones + 1).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.Capacity) ? string.Empty : $"{reportsDTO.HeadboardReport.Capacity} MVA";

                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha") && c.Seccion == secciones + 1).Celda);
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
                    }


                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("CapacidadP") && c.Seccion == secciones + 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "between",
                        From = "1",
                        To = "9999999.999",
                        AllowNulls = false,
                        MessageTemplate = "Capacidad de prueba debe ser mayor a cero considerando 7 enteros con 3 decimales",
                        Type = "reject",
                        TitleTemplate = "Error"

                    };
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "###,###";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = decimal.Parse(capacidades[secciones]) * 1000;

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPosPrim") && c.Seccion == secciones + 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.TitPosPrim;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Value = posiPrimarias[0];

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Temperatura") && c.Seccion == secciones + 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    //workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "#,##0";
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "between",
                        From = "1",
                        To = "99",
                        AllowNulls = false,
                        MessageTemplate = "Temperatura debe ser mayor a cero considerando 2 enteros sin decimales",
                        Type = "reject",
                        TitleTemplate = "Error"

                    };

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Frecuencia") && c.Seccion == secciones + 1).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.Frecuency;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "between",
                        From = "0",
                        To = "999999",
                        AllowNulls = false,
                        MessageTemplate = "Frecuencia debe ser mayor a cero considerando 6 enteros sin decimales",
                        Type = "reject",
                        TitleTemplate = "Error"

                    };

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidaCorregida") && c.Seccion == secciones + 1).Celda);
                    string peridaCorr = workbook.Sheets[0].Rows[_positionWB[0] - 1].Cells[_positionWB[1]].Value.ToString();
                    workbook.Sheets[0].Rows[_positionWB[0] - 1].Cells[_positionWB[1]].Value = peridaCorr.Replace("XX", (temperatureElevation + 20m).ToString());

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidaTotal") && c.Seccion == secciones + 1).Celda);
                    string peridaTol = workbook.Sheets[0].Rows[_positionWB[0] - 1].Cells[_positionWB[1]].Value.ToString();
                    workbook.Sheets[0].Rows[_positionWB[0] - 1].Cells[_positionWB[1]].Value = peridaTol.Replace("XX", (temperatureElevation +20m).ToString());
                    string tipPosSec;

                    if (reportsDTO.BaseTemplate.ClaveIdioma == "EN")
                    {
                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPosSec") && c.Seccion == secciones + 1).Celda);
                        tipPosSec = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = tipPosSec.Replace("XX", reportsDTO.TitPosSec);
                    }
                    else
                    {
                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPosSec") && c.Seccion == secciones + 1).Celda);
                        tipPosSec = workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].Value.ToString();
                        workbook.Sheets[0].Rows[_positionWB[0] + 1].Cells[_positionWB[1]].Value = tipPosSec.Replace("XX", reportsDTO.TitPosSec);
                    }

                    BorderStyle boder = new()
                    {
                        Color = "Black",
                        Size = 1
                    };

                    for (int i = 0; i < posiSecundarias.Length; i++)
                    {

                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Corriente") && c.Seccion == secciones + 1).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "###,##0.000";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "between",
                            From = "1",
                            To = "999999.999",
                            AllowNulls = false,
                            MessageTemplate = "El valor de la corriente i rms debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                            Type = "reject",
                            TitleTemplate = "Error"

                        };

                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension") && c.Seccion == secciones + 1).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "###,##0.000";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "between",
                            From = "1",
                            To = "999999.999",
                            AllowNulls = false,
                            MessageTemplate = "El valor de la tensión kv rms debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                            Type = "reject",
                            TitleTemplate = "Error"

                        };

                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Potencia") && c.Seccion == secciones + 1).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Format = "###,##0.000";
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "between",
                            From = "1",
                            To = "999999.999",
                            AllowNulls = false,
                            MessageTemplate = "El valor de la potencia kw debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                            Type = "reject",
                            TitleTemplate = "Error"

                        };

                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Corriente") && c.Seccion == secciones + 1).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] - 1].Value = posiSecundarias[i];

                        for (int o = 0; o < 9; o++)
                        {
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] - 1 + o].BorderBottom = boder;
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] - 1 + o].BorderLeft = boder;
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] - 1 + o].BorderRight = boder;
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] - 1 + o].BorderTop = boder;

                        }
                    }
                    secciones++;
                }



                //--llenado de las posciiones primarias
                //if (cantidadPosicionesPrimarias == 3)
                //{
                //    for(int i = 2; i <=3; i++)
                //    {
                //        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidaCorregida") && c.Seccion == i ).Celda);
                //        peridaCorr = workbook.Sheets[0].Rows[_positionWB[0]-1].Cells[_positionWB[1]].Value.ToString();
                //        workbook.Sheets[0].Rows[_positionWB[0]-1].Cells[_positionWB[1]].Value = peridaCorr.Replace("XX", reportsDTO.TitPerdCorr.ToString());

                //        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidaTotal") && c.Seccion == i).Celda);
                //        peridaTol = workbook.Sheets[0].Rows[_positionWB[0]-1].Cells[_positionWB[1]].Value.ToString();
                //        workbook.Sheets[0].Rows[_positionWB[0]-1].Cells[_positionWB[1]].Value = peridaTol.Replace("XX", reportsDTO.TitPerdTot.ToString());

                //        if(reportsDTO.BaseTemplate.ClaveIdioma == "EN")
                //        {
                //            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPosSec") && c.Seccion == i).Celda);
                //            tipPosSec = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();
                //            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = tipPosSec.Replace("XX", reportsDTO.TitPosSec);
                //        }
                //        else
                //        {
                //            _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPosSec") && c.Seccion == i).Celda);
                //            tipPosSec = workbook.Sheets[0].Rows[_positionWB[0]+1].Cells[_positionWB[1]].Value.ToString();
                //            workbook.Sheets[0].Rows[_positionWB[0]+1].Cells[_positionWB[1]].Value = tipPosSec.Replace("XX", reportsDTO.TitPosSec);
                //        }
                //    }

                //    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPosPrim") && c.Seccion == 2).Celda);
                //    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.TitPosPrim;
                //    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Value = posiPrimarias[1];

                //    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPosPrim") && c.Seccion == 3).Celda);
                //    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.TitPosPrim;
                //    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1].Value = posiPrimarias[2];

                //    for (int i = 0; i < posiSecundarias.Length; i++)
                //    {

                //        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Corriente") && c.Seccion == 2).Celda);
                //        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                //        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                //        {
                //            DataType = "number",
                //            ComparerType = "between",
                //            From = "1",
                //            To = "999999.999",
                //            AllowNulls = false,
                //            MessageTemplate = "El valor de la corriente i rms debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                //            Type = "reject",
                //            TitleTemplate = "Error"

                //        };

                //        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension") && c.Seccion == 2).Celda);
                //        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                //        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                //        {
                //            DataType = "number",
                //            ComparerType = "between",
                //            From = "1",
                //            To = "999999.999",
                //            AllowNulls = false,
                //            MessageTemplate = "El valor de la tensión kv rms debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                //            Type = "reject",
                //            TitleTemplate = "Error"

                //        };

                //        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Potencia") && c.Seccion == 2).Celda);
                //        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                //        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                //        {
                //            DataType = "number",
                //            ComparerType = "between",
                //            From = "1",
                //            To = "999999.999",
                //            AllowNulls = false,
                //            MessageTemplate = "El valor de la potencia kw debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                //            Type = "reject",
                //            TitleTemplate = "Error"

                //        };

                //        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Corriente") && c.Seccion == 3).Celda);
                //        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                //        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                //        {
                //            DataType = "number",
                //            ComparerType = "between",
                //            From = "1",
                //            To = "999999.999",
                //            AllowNulls = false,
                //            MessageTemplate = "El valor de la corriente i rms debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                //            Type = "reject",
                //            TitleTemplate = "Error"

                //        };

                //        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension") && c.Seccion == 3).Celda);
                //        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                //        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                //        {
                //            DataType = "number",
                //            ComparerType = "between",
                //            From = "1",
                //            To = "999999.999",
                //            AllowNulls = false,
                //            MessageTemplate = "El valor de la tensión kv rms debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                //            Type = "reject",
                //            TitleTemplate = "Error"

                //        };

                //        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Potencia") && c.Seccion == 3).Celda);
                //        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Enable = true;
                //        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Validation = new Validation()
                //        {
                //            DataType = "number",
                //            ComparerType = "between",
                //            From = "1",
                //            To = "999999.999",
                //            AllowNulls = false,
                //            MessageTemplate = "El valor de la potencia kw debe ser numérico mayor a cero considerando 6 enteros con 3 decimales",
                //            Type = "reject",
                //            TitleTemplate = "Error"

                //        };

                //        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Corriente") && c.Seccion == 2).Celda);
                //        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] - 1].Value = posiSecundarias[i];

                //        for (int o = 0; o < 9; o++)
                //        {
                //            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]-1 + o].BorderBottom = boder;
                //            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]-1 + o].BorderLeft = boder;
                //            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]-1 + o].BorderRight = boder;
                //            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]-1 + o].BorderTop = boder;

                //        }

                //        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Corriente") && c.Seccion == 3).Celda);
                //        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] - 1].Value = posiSecundarias[i];

                //        for (int o = 0; o < 9; o++)
                //        {
                //            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]-1 + o].BorderBottom = boder;
                //            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]-1 + o].BorderLeft = boder;
                //            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]-1 + o].BorderRight = boder;
                //            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]-1 + o].BorderTop = boder;

                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ValidateTemplatePCI(SettingsToDisplayPCIReportsDTO reportsDTO, Workbook workbook, string clavePrueba, string claveIdioma, string capacidadPrueba, int cantidadPosicionesPrimarias, int cantidadPosicionesSecundarias)
        {
            int[] positions = null;


            /******** VALIDAR CAMPOS del EXCE ****************/
            string fecha;

            positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("Fecha")).Celda);
            fecha = workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString();

            if (string.IsNullOrEmpty(fecha))
                return "La fecha no puede estar vacia.";

            string[] capacities = capacidadPrueba.Split(",");

            int cantidadCap = capacities.Length;

            for (int o = 0; o < cantidadCap; o++)
            {
                for (int i = 1; i <= cantidadPosicionesPrimarias; i++)
                {
                    positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("CapacidadP") && c.Seccion == o + 1).Celda);
                    _ = decimal.TryParse(workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out decimal baseRating);

                    if (baseRating == 0)
                        return "El valor de la capacidad no puede ser 0 o no puede estar vacio";

                    if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[positions[0]].Cells[positions[1] + 1].Value.ToString()))
                        return "La unidad de capacidad no puede estar vacia";

                    positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("Frecuencia") && c.Seccion == o + 1).Celda);
                    _ = decimal.TryParse(workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out decimal frecuencia);

                    if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[positions[0]].Cells[positions[1] + 1].Value.ToString()))
                    {
                        return "La unidad de frecuencia no puede estar vacia";
                    }

                    if (frecuencia == 0)
                    {
                        return "El valor de la frecuencia no puede ser 0 o no puede estar vacio";
                    }

                    positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("Temperatura") && c.Seccion == o + 1).Celda);
                    _ = decimal.TryParse(workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out decimal temp);
                    if (temp == 0)
                        return "El valor de la temperatura no puede ser 0 o no puede estar vacio";

                    if (string.IsNullOrEmpty(workbook.Sheets[0].Rows[positions[0]].Cells[positions[1] + 1].Value.ToString()))
                        return "La unidad de temperatura no puede estar vacia";

                    string tapPosPri;
                    string tapPosSec;
                    if (claveIdioma == "EN")
                    {
                        positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPosSec") && c.Seccion == o + 1).Celda);
                        tapPosSec = workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString().Split(" ")[0];
                    }
                    else
                    {
                        positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPosSec") && c.Seccion == o + 1).Celda);
                        tapPosSec = workbook.Sheets[0].Rows[positions[0] + 1].Cells[positions[1]]?.Value?.ToString().Split(" ")[1];
                    }
                    positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPosPrim")).Celda);
                    tapPosPri = workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString();

                    for (int j = 1; j <= cantidadPosicionesSecundarias; j++)
                    {
                        positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Corriente") && c.Seccion == o + 1).Celda);
                        object currentValue = workbook.Sheets[0].Rows[positions[0] + (j - 1)].Cells[positions[1]].Value;

                        positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension") && c.Seccion == o + 1).Celda);
                        object tensionValue = workbook.Sheets[0].Rows[positions[0] + (j - 1)].Cells[positions[1]].Value;

                        positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Potencia") && c.Seccion == o + 1).Celda);
                        object potenciaValue = workbook.Sheets[0].Rows[positions[0] + (j - 1)].Cells[positions[1]].Value;

                        if (currentValue == null || tensionValue == null || potenciaValue == null)
                        {
                            return "Debe llenar todas las posiciones de corriente, potencia y tension";
                        }
                    }
                }
            }

            return string.Empty;
        }

        public string GetDatePCI(SettingsToDisplayPCIReportsDTO reportsDTO, Workbook workbook)
        {
            int[] positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("Fecha")).Celda);
            string fecha = workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString();

            DateTime basedate = new(1899, 12, 30);

            return fecha.Contains("/") ? (DateTime.Now.Date - basedate.Date).TotalDays.ToString() : fecha;
        }

        public string Prepare_PCI_Test(
            SettingsToDisplayPCIReportsDTO reportsDTO,
            Workbook workbook,
            string claveIdioma,
            int cantidadPosicionesPrimarias,
            int cantidadPosicionesSecundarias,
            string capacidad,
            string posicionPrimaria,
            string[] RegistrosPosicionesPrimarias,
            string posicionSecundaria,
            string[] RegistrosPosicionesSecundarias,
            List<PlateTensionDTO> plateTension,
            IEnumerable<PCIParameters> parameters,
            ref PCIInputTestDTO testOut)
        {
            int[] positions;

            #region obteniendo data

            DateTime fecha = DateTime.Today;

            positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("Fecha")).Celda);
            if (double.TryParse(workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]].Value.ToString(), out double oaDate))
            {
                fecha = DateTime.FromOADate(oaDate);
            }

            #endregion

            #region resto

            decimal current = 0;
            decimal voltaje = 0;
            decimal power = 0;
            string valuetap;

            string[] capacities = capacidad.Split(",");

            int cantidadCap = capacities.Length;

            for (int o = 0; o < cantidadCap; o++)
            {
                decimal temperatureElevation = reportsDTO.InfotmationArtifact.CharacteristicsArtifact
                    .Where(e =>
                        e.Mvaf1 == decimal.Parse(capacities[o])
                        || e.Mvaf2 == decimal.Parse(capacities[o])
                        || e.Mvaf3 == decimal.Parse(capacities[o])
                        || e.Mvaf4 == decimal.Parse(capacities[o]))
                    .OrderBy(e => e.Secuencia)
                    .Select(e => e.OverElevation)
                    .FirstOrDefault(0m) ?? 0m;

                for (int i = 1; i <= cantidadPosicionesPrimarias; i++)
                {
                    positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("CapacidadP") && c.Seccion == o + 1).Celda);
                    decimal baseRating = Convert.ToDecimal(workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]].Value.ToString());

                    positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("Frecuencia") && c.Seccion == o + 1).Celda);
                    decimal frecuencia = Convert.ToDecimal(workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]].Value.ToString());

                    positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("Temperatura") && c.Seccion == o + 1).Celda);
                    decimal temp = Convert.ToDecimal(workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]].Value.ToString());

                    string tapPosPri;
                    positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPosPrim") && c.Seccion == o + 1).Celda);
                    tapPosPri = workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString();

                    positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosPrim") && c.Seccion == o + 1).Celda);
                    valuetap = workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString();

                    PCIParameters primaryParameter = parameters
                        .FirstOrDefault(e => e.PrimaryWinding == posicionPrimaria.ToUpper());

                    PCIRating ratings = new()
                    {
                        BaseRating = baseRating,
                        TemperatureElevation = temperatureElevation,
                        Frequency = frecuencia,
                        Temperature = temp,
                        TapPosition = primaryParameter?.PrimaryWinding ?? "",
                        Position = primaryParameter?.PrimaryPosition ?? "",
                        Tension = plateTension
                            .Where(c =>
                                c.TipoTension == (primaryParameter?.PrimaryWinding ?? "")
                                && c.Posicion == (primaryParameter?.PrimaryPosition ?? ""))
                            .Select(e => e.Tension)
                            .FirstOrDefault(),
                        AverageResistance = primaryParameter?.PrimaryAverageResistance ?? 0m,
                        ResistanceTemperature = primaryParameter?.PrimaryTemperature ?? 0m,
                    };

                    if (ratings.Tension == 0m)
                    {
                        return $"No ha sido encontrado el Plate Tension para la {primaryParameter?.PrimaryWinding ?? ""} - {primaryParameter?.PrimaryPosition ?? ""}";
                    }

                    string tapPosSec;
                    if (claveIdioma == "EN")
                    {
                        positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPosSec") && c.Seccion == o + 1).Celda);
                        tapPosSec = workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString().Split(" ")[0];
                    }
                    else
                    {
                        positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitPosSec") && c.Seccion == o + 1).Celda);
                        tapPosSec = workbook.Sheets[0].Rows[positions[0] + 1].Cells[positions[1]]?.Value?.ToString().Split(" ")[1];
                    }

                    for (int j = 1; j <= cantidadPosicionesSecundarias; j++)
                    {
                        positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Corriente") && c.Seccion == (o + 1)).Celda);
                        _ = decimal.TryParse(workbook.Sheets[0].Rows[positions[0] + (j - 1)].Cells[positions[1]]?.Value?.ToString(), out current);
                        string posicion = workbook.Sheets[0].Rows[positions[0] + (j - 1)].Cells[positions[1] - 1]?.Value?.ToString();

                        positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension") && c.Seccion == (o + 1)).Celda);
                        _ = decimal.TryParse(workbook.Sheets[0].Rows[positions[0] + (j - 1)].Cells[positions[1]]?.Value?.ToString(), out voltaje);

                        positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Potencia") && c.Seccion == (o + 1)).Celda);
                        _ = decimal.TryParse(workbook.Sheets[0].Rows[positions[0] + (j - 1)].Cells[positions[1]]?.Value?.ToString(), out power);

                        string[] registrosPosPrimarias = RegistrosPosicionesPrimarias;
                        string[] registrosPosSecundarias = RegistrosPosicionesSecundarias;

                        PCIParameters parameter = parameters
                            .FirstOrDefault(e =>
                                e.SecondaryWinding == posicionSecundaria.ToUpper()
                                && e.SecondaryPosition == posicion);

                        PlateTensionDTO plateSecond = plateTension
                            .Where(c =>
                                c.TipoTension == (parameter?.SecondaryWinding ?? "")
                                && c.Posicion == (parameter?.SecondaryPosition ?? ""))
                            .FirstOrDefault();

                        if (plateSecond == null)
                        {
                            return $"No se encontro tensión de placa para la posición {primaryParameter?.SecondaryWinding ?? ""} - {primaryParameter?.SecondaryPosition ?? ""}";
                        }

                        ratings.SecondaryPositions.Add(new PCISecondaryPosition()
                        {
                            TapPosition = parameter?.SecondaryWinding,
                            Position = parameter?.SecondaryPosition,
                            CurrentIrms = current,
                            VoltagekVrms = voltaje,
                            PowerKW = power,
                            ResistanceTemperature = parameter?.SecondaryTemperature ?? 0m,
                            AverageResistance = parameter?.SecondaryAverageResistance ?? 0m,
                            Corregidas20KW = parameter?.SecondaryCorrection20 ?? 0m,
                            Tension = plateSecond.Tension,
                        });
                    }

                    testOut.Ratings.Add(ratings);
                }
            }

            #endregion

            return string.Empty;
        }

        public void PrepareIndexOfPCI(PCITestResponseDTO result, SettingsToDisplayPCIReportsDTO reportsDTO, string claveIdioma, int cantidadPosicionesPrimarias, int cantidadPosicionesSecundarias, ref Workbook workbook)
        {
            int[] positions;
            for (int i = 1; i <= cantidadPosicionesPrimarias; i++)
            {
                for (int j = 1; j <= cantidadPosicionesSecundarias; j++)
                {
                    positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Porc_R") && c.Seccion == i).Celda);
                    workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]].Value = "";
                }
            }

            foreach (var item in result.Results.Select((value, i) => new { i, value }))
            {
                positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("CapacidadP") && c.Seccion == item.i + 1).Celda);
                string unit = workbook.Sheets[0].Rows[positions[0]].Cells[positions[1] + 1].Value.ToString();

                positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("UMFrec") && c.Seccion == item.i + 1).Celda);
                string unitFrecuencia = workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]].Value.ToString();

                positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("UMTemp") && c.Seccion == item.i + 1).Celda);

                string unitTemp = workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]].Value.ToString();

                item.value.UmBaseRating = unit;
                item.value.UmFrequency = unitFrecuencia;
                item.value.UmTemperature = unitTemp;

                foreach (var item2 in item.value.SecondaryPositions.Select((value, i) => new { i, value }))
                {
                    //filas del reporte
                    positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Porc_R") && c.Seccion == (item.i + 1)).Celda);
                    workbook.Sheets[0].Rows[positions[0] + item2.i].Cells[positions[1]].Value = item2.value.PercentageR;

                    positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Porc_X") && c.Seccion == (item.i + 1)).Celda);
                    workbook.Sheets[0].Rows[positions[0] + item2.i].Cells[positions[1]].Value = item2.value.PercentageX;

                    positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Porc_Z") && c.Seccion == (item.i + 1)).Celda);
                    workbook.Sheets[0].Rows[positions[0] + item2.i].Cells[positions[1]].Value = item2.value.PercentageZ;

                    positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidaCorregida") && c.Seccion == (item.i + 1)).Celda);
                    workbook.Sheets[0].Rows[positions[0] + item2.i].Cells[positions[1]].Value = item2.value.LossesCorrected;

                    positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidaTotal") && c.Seccion == (item.i + 1)).Celda);
                    workbook.Sheets[0].Rows[positions[0] + item2.i].Cells[positions[1]].Value = item2.value.LossesTotal;
                }
            }

            string resultadoString = string.Empty;
            resultadoString = result.Messages.Any()
                ? claveIdioma == "EN"
                    ? reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorRechazadoEN")).Formato
                    : reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorRechazadoES")).Formato
                : claveIdioma == "EN"
                    ? reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptadoEN")).Formato
                    : reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptadoES")).Formato;

            positions = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
            workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]].Value = resultadoString;
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
