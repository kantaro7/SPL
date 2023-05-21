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

    public class ArfService : IArfService
    {
        #region Error message
        private readonly ICorrectionFactorService _correcctionFactor;
        private readonly INozzleInformationService _nozzleInformationService;
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion
        public ArfService(ICorrectionFactorService correcctionFactor , INozzleInformationService nozzleInformationService)
        {
            this._correcctionFactor = correcctionFactor;
            this._nozzleInformationService = nozzleInformationService;
        }
        public void PrepareTemplate(ref SettingsToDisplayARFReportsDTO reportsDTO, ref Workbook workbook, string keyTest, string languaje, string voltageLevels,string team, int columnas, string ter2da,
            ref string nivelAceiteLab, ref string nivelAceitePla, ref string boquillasLab, ref string boquillasPla, ref string nucleoLab, ref string nucleoPla, ref string terciarioLab, ref string terciarioPla )
        {
            try
            {
                #region Update Readonly all cells
                workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));
                #endregion

                int[] _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cliente")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.Client;

                //_positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoSerie")).Celda);
                //workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.NoSerie;

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


                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado") && c.Seccion == 1).Celda);
                //workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                //workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = languaje is "ES" ? "Seleccione..." : "Select...";
                //workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                //{
                //    DataType = "list",
                //    ShowButton = true,
                //    ComparerType = "list",
                //    From = languaje is "ES" ? "\"Aceptado,Rechazado\"" : "\"Accepted,Rejected\"",
                //    AllowNulls = false,
                //    Type = "reject"
                //};

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelTension")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = voltageLevels;

                _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Equipo")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value + " " +team;







                if (keyTest == "LYP")
                {
                    if (nivelAceiteLab.ToUpper() != "Lleno".ToUpper())
                    {
                        reportsDTO.CeldaUmTemp1 = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTempAceite") && c.Seccion == 1).Celda;
                    }

                    if (nivelAceitePla.ToUpper() != "Lleno".ToUpper())
                    {
                        reportsDTO.CeldaUmTemp2 = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTempAceite") && c.Seccion == 2).Celda;
                    }


                    if (languaje == "EN")
                    {
                        
                        if (nivelAceiteLab.ToUpper() == "Lleno".ToUpper())
                        {
                            nivelAceiteLab = "Full";
                        }
                        else
                        {
                            nivelAceiteLab = "Empty";
                        }
                    }
                    else
                    {
                        if (nivelAceiteLab.ToUpper() == "Lleno".ToUpper())
                        {
                            nivelAceiteLab = "Lleno";
                        }
                        else
                        {
                            nivelAceiteLab = "Vacío";
                        }
                    }

                    if (languaje == "EN")
                    {
                        if (nivelAceitePla.ToUpper() == "Lleno".ToUpper())
                        {
                            nivelAceitePla = "Full";
                        }
                        else
                        {
                            nivelAceitePla = "Empty";
                        }
                    }
                    else
                    {
                        if (nivelAceitePla.ToUpper() == "Lleno".ToUpper())
                        {
                            nivelAceitePla = "Lleno";
                        }
                        else
                        {
                            nivelAceitePla = "Vacío";
                        }
                    }

                    if (languaje == "EN")
                    {
                        if (boquillasLab.ToUpper() == "Operación".ToUpper())
                        {
                            boquillasLab = "Operation";
                        }
                        else
                        {
                            boquillasLab = "Spark";
                        }
                    }
                    else
                    {
                        if (boquillasLab.ToUpper() == "Operación".ToUpper())
                        {
                            boquillasLab = "Operación";
                        }
                        else
                        {
                            boquillasLab = "Tipo Pozo";
                        }
                    }

                    if (languaje == "EN")
                    {
                        if (boquillasPla.ToUpper() == "Operación".ToUpper())
                        {
                            boquillasPla = "Operation";
                        }
                        else
                        {
                            boquillasPla = "Spark";
                        }
                    }
                    else
                    {
                        if (boquillasPla.ToUpper() == "Operación".ToUpper())
                        {
                            boquillasPla = "Operación";
                        }
                        else
                        {
                            boquillasPla = "Tipo Pozo";
                        }
                    }

                    if (languaje == "EN")
                    {
                        if (nucleoLab.ToUpper() == "Aterrizado".ToUpper())
                        {
                            nucleoLab = "Grounded";
                        }
                        else
                        {
                            nucleoLab = "Floated";
                        }
                    }
                    else
                    {
                        if (nucleoLab.ToUpper() == "Aterrizado".ToUpper())
                        {
                            nucleoLab = "Aterrizado";
                        }
                        else
                        {
                            nucleoLab = "Flotado";
                        }
                    }


                    if (languaje == "EN")
                    {
                        if (nucleoPla.ToUpper() == "Aterrizado".ToUpper())
                        {
                            nucleoPla = "Grounded";
                        }
                        else
                        {
                            nucleoPla = "Floated";
                        }
                    }
                    else
                    {
                        if (nucleoPla.ToUpper() == "Aterrizado".ToUpper())
                        {
                            nucleoPla = "Aterrizado";
                        }
                        else
                        {
                            nucleoPla = "Flotado";
                        }
                    }


                    if (ter2da == "CT" || ter2da == "2B")
                    {
                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terciario") && c.Seccion == 1).Celda);//LAB
                        if (languaje == "EN")
                        {
                            if (terciarioLab.ToUpper() == "Aterrizado".ToUpper())
                            {
                                terciarioLab = "Grounded";
                            }
                            else
                            {
                                terciarioLab = "Floated";
                            }
                        }
                        else
                        {
                            if (terciarioLab.ToUpper() == "Aterrizado".ToUpper())
                            {
                                terciarioLab = "Aterrizado";
                            }
                            else
                            {
                                terciarioLab = "Flotado";
                            }
                        }
                        reportsDTO.CeldaTer1 = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terciario") && c.Seccion == 1).Celda;

                        _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terciario") && c.Seccion == 2).Celda);//LAB
                        if (languaje == "EN")
                        {
                            if (terciarioPla.ToUpper() == "Aterrizado".ToUpper())
                            {
                                terciarioPla = "Grounded";
                            }
                            else
                            {
                                terciarioPla = "Floated";
                            }
                        }
                        else
                        {
                            if (terciarioPla.ToUpper() == "Aterrizado".ToUpper())
                            {
                                terciarioPla = "Aterrizado";
                            }
                            else
                            {
                                terciarioPla = "Flotado";
                            }
                        }
                        reportsDTO.CeldaTer2 = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terciario") && c.Seccion == 2).Celda;
                    }
                    else
                    {
                    }




                }
                else if (keyTest == "LAB")
                {
                    if (nivelAceiteLab.ToUpper() != "Lleno".ToUpper())
                    {
                        reportsDTO.CeldaUmTemp1 = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTempAceite") && c.Seccion == 1).Celda;
                    }

                    if (languaje == "EN")
                    {
                        if (nivelAceiteLab.ToUpper() == "Lleno".ToUpper())
                        {
                            nivelAceiteLab = "Full";
                        }
                        else
                        {
                            nivelAceiteLab = "Empty";
                        }
                    }
                    else
                    {
                        if (nivelAceiteLab.ToUpper() == "Lleno".ToUpper())
                        {
                            nivelAceiteLab = "Lleno";
                        }
                        else
                        {
                            nivelAceiteLab = "Vacío";
                        }
                    }

                    if (languaje == "EN")
                    {
                        if (boquillasLab.ToUpper() == "Operación".ToUpper())
                        {
                            boquillasLab = "Operation";
                        }
                        else
                        {
                            boquillasLab = "Spark";
                        }
                    }
                    else
                    {
                        if (boquillasLab.ToUpper() == "Operación".ToUpper())
                        {
                            boquillasLab = "Operación";
                        }
                        else
                        {
                            boquillasLab = "Tipo Pozo";
                        }
                    }

                    if (languaje == "EN")
                    {
                        if (nucleoLab.ToUpper() == "Aterrizado".ToUpper())
                        {
                            nucleoLab = "Grounded";
                        }
                        else
                        {
                            nucleoLab = "Floated";
                        }
                    }
                    else
                    {
                        if (nucleoLab.ToUpper() == "Aterrizado".ToUpper())
                        {
                            nucleoLab = "Aterrizado";
                        }
                        else
                        {
                            nucleoLab = "Flotado";
                        }
                    }


                    if (ter2da == "CT" || ter2da == "2B")
                    {
                        if (languaje == "EN")
                        {
                            if (terciarioLab.ToUpper() == "Aterrizado".ToUpper())
                            {
                                terciarioLab = "Grounded";
                            }
                            else
                            {
                                terciarioLab = "Floated";
                            }
                        }
                        else
                        {
                            if (terciarioLab.ToUpper() == "Aterrizado".ToUpper())
                            {
                                terciarioLab = "Aterrizado";
                            }
                            else
                            {
                                terciarioLab = "Flotado";
                            }
                        }

                    }
                    else // No tiene terciario
                    {
                    }

                }
                else if (keyTest == "PLA")
                {
                    if (nivelAceitePla.ToUpper() != "Lleno".ToUpper())
                    {
                    }

                    if (languaje == "EN")
                    {
                        if (nivelAceitePla.ToUpper() == "Lleno".ToUpper())
                        {
                            nivelAceitePla = "Full";
                        }
                        else
                        {
                            nivelAceitePla = "Empty";
                        }
                    }
                    else
                    {
                        if (nivelAceitePla.ToUpper() == "Lleno".ToUpper())
                        {
                            nivelAceitePla = "Lleno";
                        }
                        else
                        {
                            nivelAceitePla = "Vacío";
                        }
                    }


                    if (languaje == "EN")
                    {
                        if (boquillasPla.ToUpper() == "Operación".ToUpper())
                        {
                            boquillasPla = "Operation";
                        }
                        else
                        {
                            boquillasPla = "Spark";
                        }
                    }
                    else
                    {
                        if (boquillasPla.ToUpper() == "Operación".ToUpper())
                        {
                            boquillasPla = "Operación";
                        }
                        else
                        {
                            boquillasPla = "Tipo Pozo";
                        }
                    }
                    reportsDTO.CeldaBoquilla1 = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Boquillas") && c.Seccion == 1).Celda;

                    if (languaje == "EN")
                    {
                        if (nucleoPla.ToUpper() == "Aterrizado".ToUpper())
                        {
                            nucleoPla = "Grounded";
                        }
                        else
                        {
                            nucleoPla = "Floated";
                        }
                    }
                    else
                    {
                        if (nucleoPla.ToUpper() == "Aterrizado".ToUpper())
                        {
                            nucleoPla = "Aterrizado";
                        }
                        else
                        {
                            nucleoPla = "Flotado";
                        }
                    }

                    if (ter2da == "CT" || ter2da == "2B")
                    {
                        if (languaje == "EN")
                        {
                            if (terciarioPla.ToUpper() == "Aterrizado".ToUpper())
                            {
                                terciarioPla = "Grounded";
                            }
                            else
                            {
                                terciarioPla = "Floated";
                            }
                        }
                        else
                        {
                            if (terciarioPla.ToUpper() == "Aterrizado".ToUpper())
                            {
                                terciarioPla = "Aterrizado";
                            }
                            else
                            {
                                terciarioPla = "Flotado";
                            }
                        }
                    }
                    else // No tiene terciario
                    {
                        reportsDTO.TitTer1 = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitTerciario") && c.Seccion == 1).Celda;
                    }
                }




            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string ValidateTemplateTDP(SettingsToDisplayTDPReportsDTO reportsDTO, Workbook workbook)
        {
            try
            {
                int[] _positionWB;
                string valor = string.Empty;
                

                if (reportsDTO.BaseTemplate.ColumnasConfigurables == 3)
                {  /***********VALIDAR QUE TODOS LOS DATOS ESTEN INTRODUCIDOS EN EL EXCEL ************************************************************/
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal1")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if(valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 1";
                        }
                        else
                        {
                          
                        }
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal2")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 2";
                        }
                        else
                        {
                          
                        }
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal3")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 3";
                        }
                        else
                        {
                         
                        }
                    }
                    /******************************************************************************************************************************************/
                }
                else
                {
                    /***********VALIDAR QUE TODOS LOS DATOS ESTEN INTRODUCIDOS EN EL EXCEL ************************************************************/
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal1")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 1";
                        }
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal2")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 2";
                        }
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal3")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 3";
                        }
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal4")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 4";
                        }
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal5")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 5";
                        }
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal6")).Celda);
                    for (int i = 0; i < reportsDTO.Times.Count(); i++)
                    {
                        valor = workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        if (valor == null)
                        {
                            return "Debe llenar todas las posiciones asociadas a la Terminal 6";
                        }
                    }
                    /******************************************************************************************************************************************/
                }


                for(int i = 1; i <= reportsDTO.BaseTemplate.ColumnasConfigurables; i++)
                {
                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelCalibracion")).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0] ].Cells[_positionWB[1] + (i - 1)]?.Value?.ToString();
                    if (valor == null)
                    {
                        return "Los valores de Nivel de calibracion deben estar llenos";
                    }

                    _positionWB = this.GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelMedido")).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + (i - 1)]?.Value?.ToString();
                    if (valor == null)
                    {
                        return "Los valores de Nivel de medicion deben estar llenos";
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
