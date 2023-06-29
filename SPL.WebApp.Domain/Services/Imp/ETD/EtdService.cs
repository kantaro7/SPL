namespace SPL.WebApp.Domain.Services.Imp.ETD
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ETD;

    using Telerik.Windows.Documents.Spreadsheet.Model;
    using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;

    using Workbook = Telerik.Web.Spreadsheet.Workbook;

    public class EtdService : IEtdService
    {
        #region Error message
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion
        public EtdService()
        {

        }
        public void PrepareTemplate_ETD(SettingsToDisplayETDReportsDTO reportsDTO, bool lVDifferentCapacity, decimal capacity1, bool terReducedCapacity, decimal capacity2, string claveIdioma, string overload, string typeTest, ref Workbook workbook)
        { }

        public bool Verify_ETD_ColumnsToCalculate(SettingsToDisplayETDReportsDTO reportsDTO, Telerik.Web.Spreadsheet.Workbook workbook) => true;

        public void PrepareIndexOfETD(ResultETDTestsDTO resultETDTestsDTO, SettingsToDisplayETDReportsDTO reportsDTO, ref Telerik.Web.Spreadsheet.Workbook workbook, string idioma)
        { }

        public DateTime GetDate(Telerik.Web.Spreadsheet.Workbook origin, SettingsToDisplayETDReportsDTO reportsDTO) => DateTime.Now;

        public bool Verify_ETD_Columns(SettingsToDisplayETDReportsDTO reportsDTO, Telerik.Web.Spreadsheet.Workbook workbook) => true;

        public string PrepareDownloadTemplate_ETD(SettingsToDisplayETDReportsDTO reportsDTO, Dictionary<string, ParamETD> parameters, ref Telerik.Windows.Documents.Spreadsheet.Model.Workbook workbook)
        {
            try
            {
                #region Update Readonly all cells 
                //workbook.Worksheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));
                #endregion 
                int[] _positionWB; string notFound = string.Empty; List<string> pagesNames = reportsDTO.ConfigurationReports.Select(x => x.Hoja).Distinct().ToList();
                foreach (string page in pagesNames)
                {
                    if (!workbook.Worksheets.Contains(page))
                    {
                        notFound += $"La hoja {page} no se encontró en el archivo. ";
                    }
                    else
                    {

                        //string data = "";
                        //for (int i = 0; i < 60; i++)
                        //{
                        //    if (workbook.Worksheets[0].[i] != null)
                        //    {
                        //        if (workbook.Worksheets[0].Cells[i].Cells != null)
                        //        {
                        //            foreach (Cell cell in workbook.Worksheets[0].Cells[i].Cells)
                        //            {
                        //                if (cell != null)
                        //                {
                        //                    data += cell.Value?.ToString() ?? "null";
                        //                    data += ",";
                        //                }
                        //                else
                        //                {
                        //                    data += "null,";
                        //                }
                        //            }
                        //        }
                        //        else
                        //        {
                        //            data += "null";
                        //        }
                        //    }
                        //    data += Environment.NewLine;
                        //}

                        Telerik.Windows.Documents.Spreadsheet.Model.Worksheet hoja = workbook.Worksheets.GetByName(page); IEnumerable<ConfigurationETDReportsDTO> configPage = reportsDTO.ConfigurationReports.Where(x => x.Hoja.Equals(page)).OrderBy(x => x.Orden);

                        foreach (ConfigurationETDReportsDTO item in configPage.Where(x => x.Proceso.ToUpper() == "DESCARGAR"))
                        {
                            if (item.Validacion.Equals("Igual"))
                            {
                                int[] ini = this.GetRowColOfWorbook(item.IniEtiqueta);
                                int[] fin = this.GetRowColOfWorbook(item.FinEtiqueta);

                                if (item.IniEtiqueta.Equals(item.FinEtiqueta))
                                {
                                    if (workbook.Worksheets[0].Cells[ini[0], ini[1]].GetValue().Value.RawValue.ToString().Contains(item.Etiqueta))
                                    {
                                        _positionWB = this.GetRowColOfWorbook(item.IniDato);
                                        ParamETD valor = parameters[item.Campo1];
                                        workbook.Worksheets[0].Cells[_positionWB[0], _positionWB[1]].SetValue(valor.GetValue().ToString());
                                    }
                                    else
                                    {
                                        notFound += $"“No se encontró la etiqueta {item.Etiqueta} en el archivo";
                                    }
                                }
                                else
                                {
                                    bool igual = false;
                                    for (int i = ini[1]; i <= fin[1]; i++)
                                    {
                                        string dato = workbook.Worksheets[index: 0].Cells[rowIndex: ini[0], columnIndex: i].GetValue().Value.RawValue.ToString();

                                        if (dato.Contains(item.Etiqueta))
                                        {
                                            igual = true;
                                            break;
                                        }
                                    }
                                    if (!igual)
                                    {
                                        notFound += $"“No se encontró la etiqueta {item.Etiqueta} en el archivo";
                                    }
                                    else
                                    {
                                        _positionWB = this.GetRowColOfWorbook(item.IniDato);

                                        ParamETD valor = parameters[key: item.Campo1];
                                        workbook.Worksheets[0].Cells[_positionWB[0], _positionWB[1]].SetValue(valor.GetValue().ToString());

                                    }
                                }
                            }
                            else if (item.Validacion.Equals("PI"))
                            {
                                _positionWB = this.GetRowColOfWorbook(item.IniDato);
                                ParamETD valor = parameters[item.Campo1];
                                workbook.Worksheets[0].Cells[_positionWB[0], _positionWB[1]].SetValue(valor.GetValue().ToString());
                            }
                            else
                            {
                                _positionWB = this.GetRowColOfWorbook(item.IniDato); ParamETD valor = parameters[item.Campo1]; workbook.Worksheets[0].Cells[_positionWB[0], _positionWB[1]].SetValue(valor.GetValue().ToString());
                            }
                        }
                    }
                }
                CellIndex dataValidationRuleCellIndex = new(1, 1);

                ListDataValidationRuleContext context = new(workbook.Worksheets[0], dataValidationRuleCellIndex)
                {
                    InputMessageTitle = "Restricted input",
                    InputMessageContent = "The input is restricted to the week days.",
                    ErrorStyle = ErrorStyle.Stop,
                    ErrorAlertTitle = "Wrong value",
                    ErrorAlertContent = "The entered value is not valid. Allowed values are the week days!",
                    InCellDropdown = true,
                    Argument1 = "Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday"
                };

                ListDataValidationRule rule = new(context);

                workbook.Worksheets[0].Cells[dataValidationRuleCellIndex].SetDataValidationRule(rule);
                return notFound;
            }
            catch (Exception ex)
            {
                return "Error en preparar excel" + ex.Message + ex.InnerException;
            }
        }

        public ETDUploadResultDTO PrepareUploadConfiguration_ETD(SettingsToDisplayETDReportsDTO reportsDTO, List<bool> listHojas, Workbook workbook, string claveIdioma)
        {
            List<ErrorColumnsDTO> listErrors = new();
            ETDUploadResultDTO eTDUploadResult = new();
            try
            {

                //SPL_DATOSGRAL_EST Fijo se devuelve
                StabilizationDataDTO stabilizationDataDTO = new();

                //SPL_CORTEGRAL_EST Son la cabecera de cada Corte
                List<HeaderCuttingDataDTO> headerCuttingDatas = new();

                //SPL_INFO_GENERAL_ETD lista de los reportes
                List<ETDReportDTO> eTDReports = new();

                //SPL_INFO_GRAFICA_ETD
                List<GraphicETDTestsDTO> graphicsETDTests = new();

                //SPL_DATOSGRAL_EST config
                List<ConfigurationETDReportsDTO> ConfigurationReports = reportsDTO.ConfigurationReports;

                IEnumerable<ConfigurationETDReportsDTO> configF1 = new List<ConfigurationETDReportsDTO>();
                IEnumerable<ConfigurationETDReportsDTO> configF2 = new List<ConfigurationETDReportsDTO>();
                IEnumerable<ConfigurationETDReportsDTO> configF3 = new List<ConfigurationETDReportsDTO>();

                foreach (bool item in listHojas)
                {
                    if (listHojas.IndexOf(item) == 0 && item)
                    {
                        configF1 = reportsDTO.ConfigurationReports.Where(x => x.Hoja.Equals("Rep.F1") || x.Hoja.Equals("CortesF1")).AsEnumerable();
                    }
                    else if (listHojas.IndexOf(item) == 1 && item)
                    {
                        configF2 = reportsDTO.ConfigurationReports.Where(x => x.Hoja.Equals("Rep.F2") || x.Hoja.Equals("CortesF2")).AsEnumerable();
                    }
                    else
                    {
                        if (item)
                            configF3 = reportsDTO.ConfigurationReports.Where(x => x.Hoja.Equals("Rep.F3") || x.Hoja.Equals("CortesF3")).AsEnumerable();
                    }
                }

                IEnumerable<ConfigurationETDReportsDTO> configEstab = reportsDTO.ConfigurationReports.Where(x => x.Hoja.Equals("Estabilización")).AsEnumerable();

                int stabilizationSheet = workbook.Sheets.IndexOf(workbook.Sheets.First(x => x.Name.Equals("Estabilización")));
                int corteF1Sheet = workbook.Sheets.IndexOf(workbook.Sheets.First(x => x.Name.Equals("CortesF1")));

                #region Estabilizacion

                int[] altPos = this.GetRowColOfWorbook(configEstab.First(predicate: x => x.Campo1.Equals("ALTITUDE_F1")).IniDato);
                int[] altPos2 = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("ALTITUDE_F2")).IniDato);
                int[] btDifCapPos = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("BT_DIF_CAP")).IniDato);
                int[] CapBtPos = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("CAPACIDAD_BT")).IniDato);
                int[] CapTerPos = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("CAPACIDAD_TER")).IniDato);
                int[] claveIdiomaPos = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("CLAVE_IDIOMA")).IniDato);
                int[] conexionPos = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("CONEXION")).IniDato);
                int[] devanadoSplitPos = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("DEVANADO_SPLIT")).IniDato);
                int[] estPos = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("ESTATUS")).IniDato);
                int[] factPos = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("FACT_ALT")).IniDato);
                int[] intervaloPos = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("INTERVALO")).IniDato);
                int[] materialPos = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("MATERIAL")).IniDato);
                int[] atPos = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("POS_AT")).IniDato);
                int[] btPos = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("POS_BT")).IniDato);
                int[] terPos = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("POS_TER")).IniDato);
                int[] sobrecargaPos = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("SOBRECARGA")).IniDato);
                int[] terbt2Pos = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("TER_BT2")).IniDato);
                int[] terCapRedPos = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("TER_CAP_RED")).IniDato);
                int[] umIntervaloPos = this.GetRowColOfWorbook(configEstab.First(x => x.Campo1.Equals("UM_INTERVALO")).IniDato);

                stabilizationDataDTO = new()
                {
                    AltitudeF1 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[altPos[0]].Cells[altPos[1]].Value),
                    AltitudeF2 = workbook.Sheets[stabilizationSheet].Rows[altPos2[0]].Cells[altPos2[1]].Value.ToString(),
                    BtDifCap = Convert.ToInt32(workbook.Sheets[stabilizationSheet].Rows[btDifCapPos[0]].Cells[btDifCapPos[1]].Value.ToString())
                };

                if (workbook.Sheets[stabilizationSheet].Rows[CapBtPos[0]].Cells[CapBtPos[1]].Value == null)
                {
                    listErrors.Add(new ErrorColumnsDTO(claveIdiomaPos[0], claveIdiomaPos[1], "Capacidad BT es vacio o nulo en pestaña de estabilización por favor agregar valor"));
                }
                else { stabilizationDataDTO.CapacidadBT = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[CapBtPos[0]].Cells[CapBtPos[1]].Value.ToString()); }
                if (workbook.Sheets[stabilizationSheet].Rows[CapBtPos[0]].Cells[CapBtPos[1]].Value == null)
                {
                    listErrors.Add(new ErrorColumnsDTO(claveIdiomaPos[0], claveIdiomaPos[1], "Capacidad TER es vacio o nulo en pestaña de estabilización por favor agregar valor"));
                }
                else
                {
                    bool ok = decimal.TryParse(workbook.Sheets[stabilizationSheet].Rows[CapTerPos[0]].Cells[CapTerPos[1]].Value.ToString(), out decimal result);
                    stabilizationDataDTO.CapacidadTER = ok ? result : 0;
                }

                if (string.IsNullOrEmpty(workbook.Sheets[stabilizationSheet].Rows[claveIdiomaPos[0]].Cells[claveIdiomaPos[1]].Value.ToString()))
                {
                    listErrors.Add(new ErrorColumnsDTO(claveIdiomaPos[0], claveIdiomaPos[1], "Clave idioma es requerido en la hoja llamada Estabilización"));
                }
                else
                {
                    string data = SinTildes(workbook.Sheets[stabilizationSheet].Rows[claveIdiomaPos[0]].Cells[claveIdiomaPos[1]].Value.ToString().ToUpper().Trim());
                    stabilizationDataDTO.ClaveIdioma = workbook.Sheets[stabilizationSheet].Rows[claveIdiomaPos[0]].Cells[claveIdiomaPos[1]].Value.ToString();
                }

               stabilizationDataDTO.Conexion = workbook.Sheets[stabilizationSheet].Rows[conexionPos[0]].Cells[conexionPos[1]].Value.ToString();
                stabilizationDataDTO.DevanadoSplit = workbook.Sheets[stabilizationSheet].Rows[devanadoSplitPos[0]].Cells[devanadoSplitPos[1]].Value.ToString();
                stabilizationDataDTO.Estatus = workbook.Sheets[stabilizationSheet].Rows[estPos[0]].Cells[estPos[1]].Value.ToString().Equals("ESTABILIZADO");
                stabilizationDataDTO.FactAlt = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[factPos[0]].Cells[factPos[1]].Value.ToString());
                stabilizationDataDTO.Intervalo = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[intervaloPos[0]].Cells[intervaloPos[1]].Value.ToString());
                stabilizationDataDTO.Material = workbook.Sheets[stabilizationSheet].Rows[materialPos[0]].Cells[materialPos[1]].Value.ToString();
                stabilizationDataDTO.PosAt = workbook.Sheets[stabilizationSheet].Rows[atPos[0]].Cells[atPos[1]].Value.ToString();
                stabilizationDataDTO.PosBt = workbook.Sheets[stabilizationSheet].Rows[btPos[0]].Cells[btPos[1]].Value.ToString();
                stabilizationDataDTO.PosTer = workbook.Sheets[stabilizationSheet].Rows[terPos[0]].Cells[terPos[1]].Value.ToString();
                stabilizationDataDTO.Sobrecarga = workbook.Sheets[stabilizationSheet].Rows[sobrecargaPos[0]].Cells[sobrecargaPos[1]].Value.ToString();
                stabilizationDataDTO.TerBt2 = workbook.Sheets[stabilizationSheet].Rows[terbt2Pos[0]].Cells[terbt2Pos[1]].Value.ToString();
                stabilizationDataDTO.TerCarRed = Convert.ToInt32(workbook.Sheets[stabilizationSheet].Rows[terCapRedPos[0]].Cells[terCapRedPos[1]].Value.ToString());
                stabilizationDataDTO.UmIntervalo = workbook.Sheets[stabilizationSheet].Rows[umIntervaloPos[0]].Cells[umIntervaloPos[1]].Value.ToString();
                IEnumerable<ConfigurationETDReportsDTO> datosDatosDetEstConfig = reportsDTO.ConfigurationReports.Where(x => x.Tabla1.Equals("SPL_DATOSDET_EST") || x.Tabla2.Equals("SPL_DATOSDET_EST"));

                int[] fechaHora_1Pos = null;
                int[] fechaHora_11Pos = null;
                int[] fechaHora_111Pos = null;
                int[] fechaHora_1111Pos = null;

                foreach (ConfigurationETDReportsDTO item39 in datosDatosDetEstConfig.Where(x => x.Campo1 == "FECHA_HORA").OrderBy(x => x.Orden))
                {
                    int pos = datosDatosDetEstConfig.ToList().IndexOf(item39);
                    switch (pos)
                    {
                        case 0:
                            fechaHora_1Pos = this.GetRowColOfWorbook(configEstab.First(predicate: x => x.Campo1.Equals("FECHA_HORA") && x.Orden == item39.Orden).IniDato);
                            break;
                        case 1:
                            fechaHora_11Pos = this.GetRowColOfWorbook(configEstab.First(predicate: x => x.Campo1.Equals("FECHA_HORA") && x.Orden == item39.Orden).IniDato);
                            break;
                        case 2:
                            fechaHora_111Pos = this.GetRowColOfWorbook(configEstab.First(predicate: x => x.Campo1.Equals("FECHA_HORA") && x.Orden == item39.Orden).IniDato);
                            break;
                        case 3:
                            fechaHora_1111Pos = this.GetRowColOfWorbook(configEstab.First(predicate: x => x.Campo1.Equals("FECHA_HORA") && x.Orden == item39.Orden).IniDato);
                            break;

                    }
                }

                DateTime FechaHora1 = Convert.ToDateTime(workbook.Sheets[stabilizationSheet].Rows[fechaHora_1Pos[0]].Cells[fechaHora_1Pos[1]].Value.ToString());
                DateTime FechaHora2 = Convert.ToDateTime(workbook.Sheets[stabilizationSheet].Rows[fechaHora_11Pos[0]].Cells[fechaHora_11Pos[1]].Value.ToString());
                DateTime FechaHora3 = Convert.ToDateTime(workbook.Sheets[stabilizationSheet].Rows[fechaHora_111Pos[0]].Cells[fechaHora_111Pos[1]].Value.ToString());
                DateTime FechaHora4 = Convert.ToDateTime(workbook.Sheets[stabilizationSheet].Rows[fechaHora_1111Pos[0]].Cells[fechaHora_1111Pos[1]].Value.ToString());

                int[] canalSup1Pos = null;
                int[] canalSup11Pos = null;
                int[] canalSup111Pos = null;
                int[] canalSup1111Pos = null;

                foreach (ConfigurationETDReportsDTO item33 in datosDatosDetEstConfig.Where(x => x.Campo1 == "CANAL_SUP_1").OrderBy(x => x.Orden))
                {
                    int pos = datosDatosDetEstConfig.ToList().IndexOf(item33);
                    switch (pos)
                    {
                        case 0:
                            canalSup1Pos = this.GetRowColOfWorbook(configEstab.First(predicate: x => x.Campo1.Equals("CANAL_SUP_1") && x.Orden == item33.Orden).IniDato);
                            break;
                        case 1:
                            canalSup11Pos = this.GetRowColOfWorbook(configEstab.First(predicate: x => x.Campo1.Equals("CANAL_SUP_1") && x.Orden == item33.Orden).IniDato);
                            break;
                        case 2:
                            canalSup111Pos = this.GetRowColOfWorbook(configEstab.First(predicate: x => x.Campo1.Equals("CANAL_SUP_1") && x.Orden == item33.Orden).IniDato);
                            break;
                        case 3:
                            canalSup1111Pos = this.GetRowColOfWorbook(configEstab.First(predicate: x => x.Campo1.Equals("CANAL_SUP_1") && x.Orden == item33.Orden).IniDato);
                            break;

                    }
                }

                List<StabilizationDetailsDataDTO> listaDetalles = new();

                int[] kwmedidosPos = null;
                int[] canal;
                foreach (ConfigurationETDReportsDTO item40 in datosDatosDetEstConfig.Where(x => x.Campo1 == "KW_MEDIDOS").OrderBy(x => x.Orden))
                {
                    int pos = datosDatosDetEstConfig.ToList().IndexOf(item40);
                    kwmedidosPos = this.GetRowColOfWorbook(configEstab.First(predicate: x => x.Campo1.Equals("KW_MEDIDOS") && x.Orden == item40.Orden).IniDato);

                    canal = pos == 0 ? canalSup1Pos : pos == 1 ? canalSup11Pos : pos == 2 ? canalSup111Pos : canalSup1111Pos;

                    for (int i = 0; i < 13; i++)
                    {
                        listaDetalles.Add(new StabilizationDetailsDataDTO()
                        {
                            FechaHora = pos == 0 ? FechaHora1.AddHours(i) : pos == 1 ? FechaHora2.AddHours(i) : pos == 2 ? FechaHora3.AddHours(i) : FechaHora4.AddHours(i),
                            KwMedidos = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0]].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            AmpMedidos = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 1].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            CanalSup1 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[canal[0]].Cells[canal[1] + i].Value.ToString()),
                            CabSupRadBco1 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 2].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            CanalSup2 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[canal[0] + 1].Cells[canal[1] + i].Value.ToString()),
                            CabSupRadBco2 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 3].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            CanalSup3 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[canal[0] + 2].Cells[canal[1] + i].Value.ToString()),
                            CabSupRadBco3 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 4].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            CanalSup4 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[canal[0] + 3].Cells[canal[1] + i].Value.ToString()),
                            CabSupRadBco4 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 5].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            CanalSup5 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[canal[0] + 4].Cells[canal[1] + i].Value.ToString()),
                            CabSupRadBco5 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 6].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            PromRadSup = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 7].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            CanalInf1 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[canal[0] + 6].Cells[canal[1] + i].Value.ToString()),
                            CabInfRadBco1 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 8].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            CanalInf2 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[canal[0] + 7].Cells[canal[1] + i].Value.ToString()),
                            CabInfRadBco2 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 9].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            CanalInf3 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[canal[0] + 8].Cells[canal[1] + i].Value.ToString()),
                            CabInfRadBco3 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 10].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            CanalInf4 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[canal[0] + 9].Cells[canal[1] + i].Value.ToString()),
                            CabInfRadBco4 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 11].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            CanalInf5 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[canal[0] + 10].Cells[canal[1] + i].Value.ToString()),
                            CabInfRadBco5 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 12].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            PromRadInf = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 13].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            CanalAmb1 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[canal[0] + 12].Cells[canal[1] + i].Value.ToString()),
                            Ambiente1 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 14].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            CanalAmb2 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[canal[0] + 13].Cells[canal[1] + i].Value.ToString()),
                            Ambiente2 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 15].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            CanalAmb3 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[canal[0] + 14].Cells[canal[1] + i].Value.ToString()),
                            Ambiente3 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 16].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            AmbienteProm = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 17].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            CanalTtapa = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[canal[0] + 16].Cells[canal[1] + i].Value.ToString()),
                            TempTapa = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 18].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            Aor = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 19].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            Tor = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 20].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            Bor = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 21].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            AorCorr = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 22].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            TorCorr = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 23].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            CanalInst1 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[canal[0] + 22].Cells[canal[1] + i].Value.ToString()),
                            TempInstr1 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 24].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            CanalInst2 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[canal[0] + 23].Cells[canal[1] + i].Value.ToString()),
                            TempInstr2 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 25].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            CanalInst3 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[canal[0] + 24].Cells[canal[1] + i].Value.ToString()),
                            TempInstr3 = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 26].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            VerifVent1 = Convert.ToBoolean(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 27].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            VerifVent2 = Convert.ToBoolean(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 28].Cells[kwmedidosPos[1] + i].Value.ToString()),
                            Presion = Convert.ToDecimal(workbook.Sheets[stabilizationSheet].Rows[kwmedidosPos[0] + 29].Cells[kwmedidosPos[1] + i].Value.ToString()),

                        });
                    }
                }

                stabilizationDataDTO.StabilizationDataDetails = listaDetalles;

                #endregion

                if (configF1.Any())
                {
                    #region CorteF1
                    HeaderCuttingDataDTO headerCuttingDataDTO = new()
                    {
                        SectionCuttingData = new List<SectionCuttingDataDTO>()
                    };
                    IEnumerable<ConfigurationETDReportsDTO> datosHeaderConfig = configF1.Where(x => x.Tabla1.Equals("SPL_CORTEGRAL_EST") || (x.Tabla2.Equals("SPL_CORTEGRAL_EST") && x.ClaveIdioma == claveIdioma));

                    int[] limitEstPos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("LIMITE_EST")).IniDato);
                    int[] nroserielimitEstPos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("NO_SERIE")).IniDato);
                    int[] primercortePos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("PRIMER_CORTE")).IniDato);
                    int[] segundocortePos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("SEGUNDO_CORTE")).IniDato);
                    int[] tercercortePos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("TERCER_CORTE")).IniDato);
                    int[] tiporegresionPos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("TIPO_REGRESION")).IniDato);
                    int[] ultimahoraPos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("ULTIMA_HORA")).IniDato);

                    headerCuttingDataDTO.LimitEst = Convert.ToInt32(workbook.Sheets[0].Rows[limitEstPos[0]].Cells[limitEstPos[1]].Value.ToString());
                    headerCuttingDataDTO.NoSerie = Convert.ToString(workbook.Sheets[0].Rows[nroserielimitEstPos[0]].Cells[nroserielimitEstPos[1]].Value);
                    headerCuttingDataDTO.PrimerCorte = Convert.ToDateTime(workbook.Sheets[0].Rows[primercortePos[0]].Cells[primercortePos[1]].Value.ToString());
                    headerCuttingDataDTO.SegundoCorte = Convert.ToDateTime(workbook.Sheets[0].Rows[segundocortePos[0]].Cells[segundocortePos[1]].Value.ToString());
                    headerCuttingDataDTO.TercerCorte = Convert.ToDateTime(workbook.Sheets[0].Rows[tercercortePos[0]].Cells[tercercortePos[1]].Value.ToString());
                    headerCuttingDataDTO.TipoRegresion = Convert.ToInt32(workbook.Sheets[0].Rows[tiporegresionPos[0]].Cells[tiporegresionPos[1]].Value.ToString());
                    headerCuttingDataDTO.UltimaHora = Convert.ToDateTime(workbook.Sheets[0].Rows[ultimahoraPos[0]].Cells[ultimahoraPos[1]].Value.ToString());

                    IEnumerable<ConfigurationETDReportsDTO> datosCortesSecConfig = configF1.Where(x => x.Tabla1.Equals("SPL_CORTESECC_EST") || (x.Tabla2.Equals("SPL_CORTESECC_EST") && x.ClaveIdioma == claveIdioma));

                    int[] tempPosC = this.GetRowColOfWorbook(datosCortesSecConfig.First(predicate: x => x.Campo1.Equals("TEMP_DEV_C") && x.Consecutivo == 1).IniDato);
                    int[] tempPosE = this.GetRowColOfWorbook(datosCortesSecConfig.First(predicate: x => x.Campo1.Equals("TEMP_DEV_C") && x.Consecutivo == 2).IniDato);

                    if (headerCuttingDataDTO.TipoRegresion is 1)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            SectionCuttingDataDTO sectionCuttingData = new()
                            {
                                Terminal = workbook.Sheets[index: 0].Rows[tempPosC[0] - 2].Cells[tempPosC[1] + (i * 2)].Value.ToString(),
                                ResistZeroC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] - 1].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                ResistZeroE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] - 1].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                TempDevC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0]].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                TempDevE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0]].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                GradienteCaC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 1].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                GradienteCaE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 1].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                AwrC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 2].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                HsrC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 3].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                HstC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 4].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                AwrE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 2].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                HsrE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 3].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                HstE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 4].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                LimiteEst = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 5].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                            };
                            headerCuttingDataDTO.SectionCuttingData.Add(sectionCuttingData);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            SectionCuttingDataDTO sectionCuttingData = new()
                            {
                                Terminal = workbook.Sheets[index: 0].Rows[tempPosE[0] - 2].Cells[tempPosE[1] + (i * 2)].Value.ToString(),
                                ResistZeroC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] - 1].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                ResistZeroE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] - 1].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                TempDevC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0]].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                TempDevE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0]].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                GradienteCaC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 1].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                GradienteCaE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 1].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                AwrC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 2].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                HsrC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 3].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                HstC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 4].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                AwrE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 2].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                HsrE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 3].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                HstE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 4].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                LimiteEst = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 5].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                            };
                            headerCuttingDataDTO.SectionCuttingData.Add(sectionCuttingData);
                        }
                    }

                    IEnumerable<ConfigurationETDReportsDTO> datosCortesSecDetailsConfig = configF1.Where(x => x.Tabla1.Equals("SPL_CORTEDETA_EST") || (x.Tabla2.Equals("SPL_CORTEDETA_EST") && x.ClaveIdioma == claveIdioma));

                    int[] TiempPos1 = this.GetRowColOfWorbook(datosCortesSecConfig.First(predicate: x => x.Campo1.Equals("TEMP_DEV_C") && x.Seccion == 1).IniDato);
                    int[] TiempPos2 = this.GetRowColOfWorbook(datosCortesSecConfig.First(predicate: x => x.Campo1.Equals("TEMP_DEV_C") && x.Seccion == 2).IniDato);
                    int[] TiempPos3 = this.GetRowColOfWorbook(datosCortesSecConfig.First(predicate: x => x.Campo1.Equals("TEMP_DEV_C") && x.Seccion == 3).IniDato);

                    for (int i = 0; i < 3; i++)
                    {
                        headerCuttingDataDTO.SectionCuttingData[i].DetailCuttingData = new List<DetailCuttingDataDTO>();
                        if (i is 0)
                        {
                            for (int j = 0; j < 29; j++)
                            {
                                headerCuttingDataDTO.SectionCuttingData[i].DetailCuttingData.Add(new DetailCuttingDataDTO()
                                {
                                    Tiempo = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos1[0] + j].Cells[TiempPos1[1]].Value.ToString()),
                                    Resistencia = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos1[0] + j].Cells[TiempPos1[1] + 1].Value.ToString()),
                                    TempR = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos1[0] + j].Cells[TiempPos1[1] + 2].Value.ToString()),
                                    Seccion = 1,
                                    Renglon = j + 1
                                });
                            }
                        }
                        else if (i is 1)
                        {
                            for (int j = 0; j < 29; j++)
                            {
                                headerCuttingDataDTO.SectionCuttingData[i].DetailCuttingData.Add(new DetailCuttingDataDTO()
                                {
                                    Tiempo = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos2[0] + j].Cells[TiempPos2[1]].Value.ToString()),
                                    Resistencia = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos2[0] + j].Cells[TiempPos2[1] + 1].Value.ToString()),
                                    TempR = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos2[0] + j].Cells[TiempPos2[1] + 2].Value.ToString()),
                                    Seccion = 1,
                                    Renglon = j + 1
                                });
                            }
                        }
                        else
                        {
                            for (int j = 0; j < 29; j++)
                            {
                                headerCuttingDataDTO.SectionCuttingData[i].DetailCuttingData.Add(new DetailCuttingDataDTO()
                                {
                                    Tiempo = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos3[0] + j].Cells[TiempPos3[1]].Value.ToString()),
                                    Resistencia = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos3[0] + j].Cells[TiempPos3[1] + 1].Value.ToString()),
                                    TempR = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos3[0] + j].Cells[TiempPos3[1] + 2].Value.ToString()),
                                    Seccion = 1,
                                    Renglon = j + 1
                                });
                            }
                        }
                    }
                    headerCuttingDatas.Add(headerCuttingDataDTO);
                    #endregion
                    #region RepF1
                    #region Seccion 1
                    #region Cabecera
                    ETDReportDTO eTDReportDTO = new();
                    IEnumerable<ConfigurationETDReportsDTO> configRep1 = configF1.Where(x => x.Hoja.Equals("Rep.F1") && x.Seccion == 1);
                    //Datos de la cabecera
                    int[] cliPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("CLIENTE")).IniDato);
                    int[] seriePos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("NO_SERIE")).IniDato);
                    int[] ratingPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("CAPACIDAD") && x.Tabla1.Equals("SPL_INFO_GENERAL_ETD")).IniDato);
                    //Datos de la seccion 1
                    int[] fechaPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("FECHA_PRUEBA") && x.Tabla1.Equals("SPL_INFO_SECCION_ETD")).IniDato);
                    int[] capPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("CAPACIDAD") && x.Tabla1.Equals("SPL_INFO_SECCION_ETD")).IniDato);
                    //la posicion de perdida esta debajo de capPos
                    int[] coolPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("COOLING_TYPE")).IniDato);
                    int[] alPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("ALTITUDE_F1")).IniDato);
                    int[] alPos2 = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("ALTITUDE_F2")).IniDato);
                    int[] atPo = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("POS_AT")).IniDato);
                    int[] btPo = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("POS_BT")).IniDato);
                    int[] terPo = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("POS_TER")).IniDato);

                    eTDReportDTO.Cliente = workbook.Sheets[0].Rows[cliPos[0]].Cells[cliPos[1]].Value.ToString();
                    eTDReportDTO.NoSerie = workbook.Sheets[0].Rows[seriePos[0]].Cells[seriePos[1]].Value.ToString();
                    eTDReportDTO.Capacidad = workbook.Sheets[0].Rows[ratingPos[0]].Cells[ratingPos[1]].Value.ToString();
                    #endregion

                    #region Datos sueltos
                    eTDReportDTO.ETDTestsGeneral = new ETDTestsGeneralDTO() { ETDTests = new List<ETDTestsDTO>() };

                    ETDTestsDTO secc1 = new()
                    {
                        FechaPrueba = Convert.ToDateTime(workbook.Sheets[0].Rows[fechaPos[0]].Cells[fechaPos[1]].Value.ToString()),
                        Capacidad = Convert.ToDecimal(workbook.Sheets[0].Rows[capPos[0]].Cells[capPos[1]].Value.ToString()),
                        CoolingType = workbook.Sheets[0].Rows[coolPos[0]].Cells[coolPos[1]].Value.ToString(),
                        AltitudeF1 = Convert.ToDecimal(workbook.Sheets[0].Rows[alPos[0]].Cells[alPos[1]].Value.ToString()),
                        AltitudeF2 = workbook.Sheets[0].Rows[alPos2[0]].Cells[alPos2[1]].Value.ToString(),
                        PosAt = workbook.Sheets[0].Rows[atPo[0]].Cells[atPo[1]].Value.ToString(),
                        PosBt = workbook.Sheets[0].Rows[btPo[0]].Cells[btPo[1]].Value.ToString(),
                        PosTer = stabilizationDataDTO.CapacidadTER is 0 ? "" : workbook.Sheets[0].Rows[terPo[0]].Cells[terPo[1]].Value.ToString(),
                        Perdidas = Convert.ToDecimal(workbook.Sheets[0].Rows[capPos[0] + 1].Cells[capPos[1]].Value.ToString()),
                        ETDTestsDetails = new List<ETDTestsDetailsDTO>(),
                        GraphicETDTests = new List<GraphicETDTestsDTO>()
                    };
                    #endregion

                    #region Tabla detalles

                    int[] horaPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("FECHA_HORA") && x.Consecutivo == 1).IniDato);

                    for (int i = 0; i < 4; i++)
                    {
                        secc1.ETDTestsDetails.Add(new ETDTestsDetailsDTO()
                        {
                            FechaHora = Convert.ToDateTime(workbook.Sheets[0].Rows[horaPos[0]].Cells[horaPos[1] + i].Value.ToString()),
                            PromRadSup = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 1].Cells[horaPos[1] + i].Value.ToString()),
                            PromRadInf = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 2].Cells[horaPos[1] + i].Value.ToString()),
                            Ambiente1 = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 3].Cells[horaPos[1] + i].Value.ToString()),
                            Ambiente2 = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 4].Cells[horaPos[1] + i].Value.ToString()),
                            Ambiente3 = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 5].Cells[horaPos[1] + i].Value.ToString()),
                            AmbienteProm = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 6].Cells[horaPos[1] + i].Value.ToString()),
                            TempTapa = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 7].Cells[horaPos[1] + i].Value.ToString()),
                            Tor = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 8].Cells[horaPos[1] + i].Value.ToString()),
                            Aor = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 9].Cells[horaPos[1] + i].Value.ToString()),
                            Bor = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 10].Cells[horaPos[1] + i].Value.ToString()),
                            ElevAceiteSup = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 16].Cells[horaPos[1] + i].Value.ToString()),
                            ElevAceiteProm = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 17].Cells[horaPos[1] + i].Value.ToString()),
                            ElevAceiteInf = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 18].Cells[horaPos[1] + i].Value.ToString())
                        });
                    }

                    eTDReportDTO.ETDTestsGeneral.ETDTests.Add(secc1);
                    #endregion

                    #endregion

                    #region Seccion 2 3 y posible 4

                    int secNum = stabilizationDataDTO.CapacidadTER is 0 ? 2 : 3;

                    for (int sec = 1; sec <= secNum; sec++)
                    {
                        #region Datos Sueltos
                        IEnumerable<ConfigurationETDReportsDTO> configRep2 = configF1.Where(x => x.Hoja.Equals("Rep.F1") && x.Seccion == sec + 1);
                        fechaPos = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("FECHA_PRUEBA") && x.Tabla1.Equals("SPL_INFO_SECCION_ETD")).IniDato);

                        capPos = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("CAPACIDAD") && x.Tabla1.Equals("SPL_INFO_SECCION_ETD")).IniDato);
                        atPo = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("POS_AT")).IniDato);

                        int[] resisPos = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("RESIST_CORTE")).IniDato);
                        int[] tempPromPos = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("TEMP_PROM_ACEITE")).IniDato);
                        int[] termPos = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("TERMINAL")).IniDato);

                        ETDTestsDTO secc234 = new()
                        {
                            FechaPrueba = Convert.ToDateTime(workbook.Sheets[0].Rows[fechaPos[0]].Cells[fechaPos[1]].Value.ToString()),
                            Capacidad = Convert.ToDecimal(workbook.Sheets[0].Rows[capPos[0]].Cells[capPos[1]].Value.ToString()),
                            CoolingType = workbook.Sheets[0].Rows[capPos[0] + 1].Cells[capPos[1]].Value.ToString(),
                            AltitudeF1 = Convert.ToDecimal(workbook.Sheets[0].Rows[capPos[0] + 3].Cells[capPos[1]].Value.ToString()),
                            AltitudeF2 = workbook.Sheets[0].Rows[capPos[0] + 3].Cells[capPos[1] + 1].Value.ToString(),
                            PosAt = workbook.Sheets[0].Rows[atPo[0]].Cells[atPo[1]].Value.ToString(),
                            PosBt = workbook.Sheets[0].Rows[atPo[0] + 1].Cells[atPo[1]].Value.ToString(),
                            PosTer = stabilizationDataDTO.CapacidadTER is 0 ? "" : workbook.Sheets[0].Rows[atPo[0] + 2].Cells[atPo[1]].Value.ToString(),
                            ResistCorte = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0]].Cells[resisPos[1]].Value.ToString()),
                            TempResistCorte = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 1].Cells[resisPos[1]].Value.ToString()),
                            FactorK = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 2].Cells[resisPos[1]].Value.ToString()),
                            ResistTcero = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 3].Cells[resisPos[1]].Value.ToString()),
                            TempDev = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 5].Cells[resisPos[1]].Value.ToString()),
                            GradienteDev = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 6].Cells[resisPos[1]].Value.ToString()),
                            ElevPromDev = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 7].Cells[resisPos[1]].Value.ToString()),
                            ElevPtoMasCal = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 8].Cells[resisPos[1]].Value.ToString()),
                            TempPromAceite = Convert.ToDecimal(workbook.Sheets[0].Rows[tempPromPos[0]].Cells[tempPromPos[1]].Value.ToString()),
                            Terminal = workbook.Sheets[0].Rows[termPos[0]].Cells[termPos[1]].Value.ToString(),
                            ETDTestsDetails = new List<ETDTestsDetailsDTO>(),
                            Seccion = 1 + sec
                        };
                        #endregion

                        #region Tabla detalles

                        int[] tiemPos = this.GetRowColOfWorbook(configRep2.First(x => x.Campo1.Equals("TIEMPO")).IniDato);

                        for (int i = 0; i < 4; i++)
                        {
                            secc234.ETDTestsDetails.Add(new ETDTestsDetailsDTO()
                            {
                                Tiempo = Convert.ToDecimal(workbook.Sheets[0].Rows[tiemPos[0] + i].Cells[tiemPos[1]].Value.ToString()),
                                Resistencia = Convert.ToDecimal(workbook.Sheets[0].Rows[tiemPos[0] + i].Cells[tiemPos[1] + 2].Value.ToString()),
                                Seccion = 1 + sec,
                                Renglon = i + 1
                            });
                        }

                        #endregion
                        #region Datos Grafica
                        int[] grafXPos = this.GetRowColOfWorbook(configRep2.First(x => x.Campo1.Equals("VALOR_X") && x.Seccion == sec + 1).IniDato);

                        for (int i = 0; i < 35; i++)
                        {
                            secc234.GraphicETDTests.Add(new()
                            {
                                ValorX = Convert.ToDecimal(workbook.Sheets[0].Rows[grafXPos[0] + i].Cells[grafXPos[1]].Value.ToString()),
                                ValorY = Convert.ToDecimal(workbook.Sheets[0].Rows[grafXPos[0] + i].Cells[grafXPos[1] + 1].Value.ToString()),
                                ValorZ = Convert.ToDecimal(workbook.Sheets[0].Rows[grafXPos[0] + i].Cells[grafXPos[1] + 2].Value.ToString()),
                                Seccion = 1 + sec,
                                Renglon = i + 1
                            });
                        }

                        eTDReportDTO.ETDTestsGeneral.ETDTests.Add(secc234);
                        #endregion
                    }
                    #endregion
                    eTDReports.Add(eTDReportDTO);
                    #endregion
                }

                if (configF2.Any())
                {
                    #region CorteF2
                    HeaderCuttingDataDTO headerCuttingDataDTO = new()
                    {
                        SectionCuttingData = new List<SectionCuttingDataDTO>()
                    };
                    IEnumerable<ConfigurationETDReportsDTO> datosHeaderConfig = configF2.Where(x => x.Tabla1.Equals("SPL_CORTEGRAL_EST") || (x.Tabla2.Equals("SPL_CORTEGRAL_EST") && x.ClaveIdioma == claveIdioma));

                    int[] limitEstPos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("LIMITE_EST")).IniDato);
                    int[] nroserielimitEstPos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("NO_SERIE")).IniDato);
                    int[] primercortePos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("PRIMER_CORTE")).IniDato);
                    int[] segundocortePos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("SEGUNDO_CORTE")).IniDato);
                    int[] tercercortePos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("TERCER_CORTE")).IniDato);
                    int[] tiporegresionPos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("TIPO_REGRESION")).IniDato);
                    int[] ultimahoraPos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("ULTIMA_HORA")).IniDato);

                    headerCuttingDataDTO.LimitEst = Convert.ToInt32(workbook.Sheets[0].Rows[limitEstPos[0]].Cells[limitEstPos[1]].Value.ToString());
                    headerCuttingDataDTO.NoSerie = Convert.ToString(workbook.Sheets[0].Rows[nroserielimitEstPos[0]].Cells[nroserielimitEstPos[1]].Value);
                    headerCuttingDataDTO.PrimerCorte = Convert.ToDateTime(workbook.Sheets[0].Rows[primercortePos[0]].Cells[primercortePos[1]].Value.ToString());
                    headerCuttingDataDTO.SegundoCorte = Convert.ToDateTime(workbook.Sheets[0].Rows[segundocortePos[0]].Cells[segundocortePos[1]].Value.ToString());
                    headerCuttingDataDTO.TercerCorte = Convert.ToDateTime(workbook.Sheets[0].Rows[tercercortePos[0]].Cells[tercercortePos[1]].Value.ToString());
                    headerCuttingDataDTO.TipoRegresion = Convert.ToInt32(workbook.Sheets[0].Rows[tiporegresionPos[0]].Cells[tiporegresionPos[1]].Value.ToString());
                    headerCuttingDataDTO.UltimaHora = Convert.ToDateTime(workbook.Sheets[0].Rows[ultimahoraPos[0]].Cells[ultimahoraPos[1]].Value.ToString());

                    IEnumerable<ConfigurationETDReportsDTO> datosCortesSecConfig = configF2.Where(x => x.Tabla1.Equals("SPL_CORTESECC_EST") || (x.Tabla2.Equals("SPL_CORTESECC_EST") && x.ClaveIdioma == claveIdioma));

                    int[] tempPosC = this.GetRowColOfWorbook(datosCortesSecConfig.First(predicate: x => x.Campo1.Equals("TEMP_DEV_C") && x.Consecutivo == 1).IniDato);
                    int[] tempPosE = this.GetRowColOfWorbook(datosCortesSecConfig.First(predicate: x => x.Campo1.Equals("TEMP_DEV_C") && x.Consecutivo == 2).IniDato);

                    if (headerCuttingDataDTO.TipoRegresion is 1)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            SectionCuttingDataDTO sectionDetailsCDataDTO = new()
                            {
                                Terminal = workbook.Sheets[index: 0].Rows[tempPosC[0] - 2].Cells[tempPosC[1] + (i * 2)].Value.ToString(),
                                ResistZeroC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] - 1].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                ResistZeroE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] - 1].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                TempDevC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0]].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                TempDevE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0]].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                GradienteCaC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 1].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                GradienteCaE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 1].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                AwrC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 2].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                HsrC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 3].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                HstC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 4].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                AwrE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 2].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                HsrE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 3].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                HstE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 4].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                LimiteEst = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 5].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                            };
                            headerCuttingDataDTO.SectionCuttingData.Add(sectionDetailsCDataDTO);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            SectionCuttingDataDTO sectionDetailsCDataDTO = new()
                            {
                                Terminal = workbook.Sheets[index: 0].Rows[tempPosE[0] - 2].Cells[tempPosE[1] + (i * 2)].Value.ToString(),
                                ResistZeroC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] - 1].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                ResistZeroE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] - 1].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                TempDevC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0]].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                TempDevE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0]].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                GradienteCaC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 1].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                GradienteCaE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 1].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                AwrC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 2].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                HsrC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 3].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                HstC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 4].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                AwrE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 2].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                HsrE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 3].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                HstE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 4].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                LimiteEst = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 5].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                            };
                            headerCuttingDataDTO.SectionCuttingData.Add(sectionDetailsCDataDTO);
                        }
                    }

                    IEnumerable<ConfigurationETDReportsDTO> datosCortesSecDetailsConfig = configF2.Where(x => x.Tabla1.Equals("SPL_CORTEDETA_EST") || (x.Tabla2.Equals("SPL_CORTEDETA_EST") && x.ClaveIdioma == claveIdioma));

                    int[] TiempPos1 = this.GetRowColOfWorbook(datosCortesSecConfig.First(predicate: x => x.Campo1.Equals("TEMP_DEV_C") && x.Seccion == 1).IniDato);
                    int[] TiempPos2 = this.GetRowColOfWorbook(datosCortesSecConfig.First(predicate: x => x.Campo1.Equals("TEMP_DEV_C") && x.Seccion == 2).IniDato);
                    int[] TiempPos3 = this.GetRowColOfWorbook(datosCortesSecConfig.First(predicate: x => x.Campo1.Equals("TEMP_DEV_C") && x.Seccion == 3).IniDato);

                    for (int i = 0; i < 3; i++)
                    {
                        headerCuttingDataDTO.SectionCuttingData[i].DetailCuttingData = new List<DetailCuttingDataDTO>();
                        if (i is 0)
                        {
                            for (int j = 0; j < 29; j++)
                            {
                                headerCuttingDataDTO.SectionCuttingData[i].DetailCuttingData.Add(new DetailCuttingDataDTO()
                                {
                                    Tiempo = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos1[0] + j].Cells[TiempPos1[1]].Value.ToString()),
                                    Resistencia = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos1[0] + j].Cells[TiempPos1[1] + 1].Value.ToString()),
                                    TempR = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos1[0] + j].Cells[TiempPos1[1] + 2].Value.ToString()),
                                    Seccion = 1,
                                    Renglon = j + 1
                                });
                            }
                        }
                        else if (i is 1)
                        {
                            for (int j = 0; j < 29; j++)
                            {
                                headerCuttingDataDTO.SectionCuttingData[i].DetailCuttingData.Add(new DetailCuttingDataDTO()
                                {
                                    Tiempo = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos2[0] + j].Cells[TiempPos2[1]].Value.ToString()),
                                    Resistencia = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos2[0] + j].Cells[TiempPos2[1] + 1].Value.ToString()),
                                    TempR = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos2[0] + j].Cells[TiempPos2[1] + 2].Value.ToString()),
                                    Seccion = 1,
                                    Renglon = j + 1
                                });
                            }
                        }
                        else
                        {
                            for (int j = 0; j < 29; j++)
                            {
                                headerCuttingDataDTO.SectionCuttingData[i].DetailCuttingData.Add(new DetailCuttingDataDTO()
                                {
                                    Tiempo = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos3[0] + j].Cells[TiempPos3[1]].Value.ToString()),
                                    Resistencia = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos3[0] + j].Cells[TiempPos3[1] + 1].Value.ToString()),
                                    TempR = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos3[0] + j].Cells[TiempPos3[1] + 2].Value.ToString()),
                                    Seccion = 1,
                                    Renglon = j + 1
                                });
                            }
                        }
                    }
                    headerCuttingDatas.Add(headerCuttingDataDTO);
                    #endregion
                    #region RepF2
                    #region Seccion 1
                    #region Cabecera
                    ETDReportDTO eTDReportDTO = new();
                    IEnumerable<ConfigurationETDReportsDTO> configRep1 = configF2.Where(x => x.Hoja.Equals("Rep.F2") && x.Seccion == 1);
                    //Datos de la cabecera
                    int[] cliPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("CLIENTE")).IniDato);
                    int[] seriePos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("NO_SERIE")).IniDato);
                    int[] ratingPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("CAPACIDAD") && x.Tabla1.Equals("SPL_INFO_GENERAL_ETD")).IniDato);
                    //Datos de la seccion 1
                    int[] fechaPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("FECHA_PRUEBA") && x.Tabla1.Equals("SPL_INFO_SECCION_ETD")).IniDato);
                    int[] capPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("CAPACIDAD") && x.Tabla1.Equals("SPL_INFO_SECCION_ETD")).IniDato);
                    //la posicion de perdida esta debajo de capPos
                    int[] coolPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("COOLING_TYPE")).IniDato);
                    int[] alPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("ALTITUDE_F1")).IniDato);
                    int[] alPos2 = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("ALTITUDE_F2")).IniDato);
                    int[] atPo = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("POS_AT")).IniDato);
                    int[] btPo = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("POS_BT")).IniDato);
                    int[] terPo = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("POS_TER")).IniDato);

                    eTDReportDTO.Cliente = workbook.Sheets[0].Rows[cliPos[0]].Cells[cliPos[1]].Value.ToString();
                    eTDReportDTO.NoSerie = workbook.Sheets[0].Rows[seriePos[0]].Cells[seriePos[1]].Value.ToString();
                    eTDReportDTO.Capacidad = workbook.Sheets[0].Rows[ratingPos[0]].Cells[ratingPos[1]].Value.ToString();
                    #endregion

                    #region Datos sueltos
                    eTDReportDTO.ETDTestsGeneral = new ETDTestsGeneralDTO() { ETDTests = new List<ETDTestsDTO>() };

                    ETDTestsDTO secc1 = new()
                    {
                        FechaPrueba = Convert.ToDateTime(workbook.Sheets[0].Rows[fechaPos[0]].Cells[fechaPos[1]].Value.ToString()),
                        Capacidad = Convert.ToDecimal(workbook.Sheets[0].Rows[capPos[0]].Cells[capPos[1]].Value.ToString()),
                        CoolingType = workbook.Sheets[0].Rows[coolPos[0]].Cells[coolPos[1]].Value.ToString(),
                        AltitudeF1 = Convert.ToDecimal(workbook.Sheets[0].Rows[alPos[0]].Cells[alPos[1]].Value.ToString()),
                        AltitudeF2 = workbook.Sheets[0].Rows[alPos2[0]].Cells[alPos2[1]].Value.ToString(),
                        PosAt = workbook.Sheets[0].Rows[atPo[0]].Cells[atPo[1]].Value.ToString(),
                        PosBt = workbook.Sheets[0].Rows[btPo[0]].Cells[btPo[1]].Value.ToString(),
                        PosTer = stabilizationDataDTO.CapacidadTER is 0 ? "" : workbook.Sheets[0].Rows[terPo[0]].Cells[terPo[1]].Value.ToString(),
                        Perdidas = Convert.ToDecimal(workbook.Sheets[0].Rows[capPos[0] + 1].Cells[capPos[1]].Value.ToString()),
                        ETDTestsDetails = new List<ETDTestsDetailsDTO>(),
                        Seccion = 1,
                        GraphicETDTests = new List<GraphicETDTestsDTO>()
                    };
                    #endregion

                    #region Tabla detalles

                    int[] horaPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("FECHA_HORA") && x.Consecutivo == 1).IniDato);

                    for (int i = 0; i < 4; i++)
                    {
                        secc1.ETDTestsDetails.Add(new ETDTestsDetailsDTO()
                        {
                            FechaHora = Convert.ToDateTime(workbook.Sheets[0].Rows[horaPos[0]].Cells[horaPos[1] + i].Value.ToString()),
                            PromRadSup = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 1].Cells[horaPos[1] + i].Value.ToString()),
                            PromRadInf = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 2].Cells[horaPos[1] + i].Value.ToString()),
                            Ambiente1 = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 3].Cells[horaPos[1] + i].Value.ToString()),
                            Ambiente2 = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 4].Cells[horaPos[1] + i].Value.ToString()),
                            Ambiente3 = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 5].Cells[horaPos[1] + i].Value.ToString()),
                            AmbienteProm = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 6].Cells[horaPos[1] + i].Value.ToString()),
                            TempTapa = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 7].Cells[horaPos[1] + i].Value.ToString()),
                            Tor = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 8].Cells[horaPos[1] + i].Value.ToString()),
                            Aor = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 9].Cells[horaPos[1] + i].Value.ToString()),
                            Bor = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 10].Cells[horaPos[1] + i].Value.ToString()),
                            ElevAceiteSup = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 16].Cells[horaPos[1] + i].Value.ToString()),
                            ElevAceiteProm = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 17].Cells[horaPos[1] + i].Value.ToString()),
                            ElevAceiteInf = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 18].Cells[horaPos[1] + i].Value.ToString())
                        });
                    }

                    eTDReportDTO.ETDTestsGeneral.ETDTests.Add(secc1);
                    #endregion

                    #endregion

                    #region Seccion 2 3 y posible 4

                    int secNum = stabilizationDataDTO.CapacidadTER is 0 ? 2 : 3;

                    for (int sec = 1; sec <= secNum; sec++)
                    {
                        #region Datos Sueltos
                        IEnumerable<ConfigurationETDReportsDTO> configRep2 = configF2.Where(x => x.Hoja.Equals("Rep.F2") && x.Seccion == sec + 1);
                        fechaPos = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("FECHA_PRUEBA") && x.Tabla1.Equals("SPL_INFO_SECCION_ETD")).IniDato);

                        capPos = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("CAPACIDAD") && x.Tabla1.Equals("SPL_INFO_SECCION_ETD")).IniDato);
                        atPo = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("POS_AT")).IniDato);

                        int[] resisPos = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("RESIST_CORTE")).IniDato);
                        int[] tempPromPos = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("TEMP_PROM_ACEITE")).IniDato);
                        int[] termPos = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("TERMINAL")).IniDato);

                        ETDTestsDTO secc234 = new()
                        {
                            FechaPrueba = Convert.ToDateTime(workbook.Sheets[0].Rows[fechaPos[0]].Cells[fechaPos[1]].Value.ToString()),
                            Capacidad = Convert.ToDecimal(workbook.Sheets[0].Rows[capPos[0]].Cells[capPos[1]].Value.ToString()),
                            CoolingType = workbook.Sheets[0].Rows[capPos[0] + 1].Cells[capPos[1]].Value.ToString(),
                            AltitudeF1 = Convert.ToDecimal(workbook.Sheets[0].Rows[capPos[0] + 3].Cells[capPos[1]].Value.ToString()),
                            AltitudeF2 = workbook.Sheets[0].Rows[capPos[0] + 3].Cells[capPos[1] + 1].Value.ToString(),
                            PosAt = workbook.Sheets[0].Rows[atPo[0]].Cells[atPo[1]].Value.ToString(),
                            PosBt = workbook.Sheets[0].Rows[atPo[0] + 1].Cells[atPo[1]].Value.ToString(),
                            PosTer = stabilizationDataDTO.CapacidadTER is 0 ? "" : workbook.Sheets[0].Rows[atPo[0] + 2].Cells[atPo[1]].Value.ToString(),
                            ResistCorte = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0]].Cells[resisPos[1]].Value.ToString()),
                            TempResistCorte = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 1].Cells[resisPos[1]].Value.ToString()),
                            FactorK = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 2].Cells[resisPos[1]].Value.ToString()),
                            ResistTcero = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 3].Cells[resisPos[1]].Value.ToString()),
                            TempDev = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 5].Cells[resisPos[1]].Value.ToString()),
                            GradienteDev = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 6].Cells[resisPos[1]].Value.ToString()),
                            ElevPromDev = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 7].Cells[resisPos[1]].Value.ToString()),
                            ElevPtoMasCal = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 8].Cells[resisPos[1]].Value.ToString()),
                            TempPromAceite = Convert.ToDecimal(workbook.Sheets[0].Rows[tempPromPos[0]].Cells[tempPromPos[1]].Value.ToString()),
                            Terminal = workbook.Sheets[0].Rows[termPos[0]].Cells[termPos[1]].Value.ToString(),
                            ETDTestsDetails = new List<ETDTestsDetailsDTO>(),
                            Seccion = 1 + sec
                        };
                        #endregion

                        #region Tabla detalles

                        int[] tiemPos = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("TIEMPO")).IniDato);

                        for (int i = 0; i < 4; i++)
                        {
                            secc234.ETDTestsDetails.Add(new ETDTestsDetailsDTO()
                            {
                                Tiempo = Convert.ToDecimal(workbook.Sheets[0].Rows[tiemPos[0] + i].Cells[tiemPos[1]].Value.ToString()),
                                Resistencia = Convert.ToDecimal(workbook.Sheets[0].Rows[tiemPos[0] + i].Cells[tiemPos[1] + 2].Value.ToString()),
                                Seccion = 1 + sec,
                                Renglon = i + 1
                            });
                        }

                        #endregion
                        #region Datos Grafica
                        int[] grafXPos = this.GetRowColOfWorbook(configRep2.First(x => x.Campo1.Equals("VALOR_X") && x.Seccion == sec + 1).IniDato);

                        for (int i = 0; i < 35; i++)
                        {
                            secc234.GraphicETDTests.Add(new()
                            {
                                ValorX = Convert.ToDecimal(workbook.Sheets[0].Rows[grafXPos[0] + i].Cells[grafXPos[1]].Value.ToString()),
                                ValorY = Convert.ToDecimal(workbook.Sheets[0].Rows[grafXPos[0] + i].Cells[grafXPos[1] + 1].Value.ToString()),
                                ValorZ = Convert.ToDecimal(workbook.Sheets[0].Rows[grafXPos[0] + i].Cells[grafXPos[1] + 2].Value.ToString()),
                                Seccion = 1 + sec,
                                Renglon = i + 1
                            });
                        }

                        eTDReportDTO.ETDTestsGeneral.ETDTests.Add(secc234);
                        #endregion
                    }
                    #endregion
                    eTDReports.Add(eTDReportDTO);
                    #endregion
                }

                if (configF3.Any())
                {
                    #region CorteF3
                    HeaderCuttingDataDTO headerCuttingDataDTO = new()
                    {
                        SectionCuttingData = new List<SectionCuttingDataDTO>()
                    };
                    IEnumerable<ConfigurationETDReportsDTO> datosHeaderConfig = configF3.Where(x => x.Tabla1.Equals("SPL_CORTEGRAL_EST") || (x.Tabla2.Equals("SPL_CORTEGRAL_EST") && x.ClaveIdioma == claveIdioma));

                    int[] limitEstPos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("LIMITE_EST")).IniDato);
                    int[] nroserielimitEstPos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("NO_SERIE")).IniDato);
                    int[] primercortePos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("PRIMER_CORTE")).IniDato);
                    int[] segundocortePos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("SEGUNDO_CORTE")).IniDato);
                    int[] tercercortePos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("TERCER_CORTE")).IniDato);
                    int[] tiporegresionPos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("TIPO_REGRESION")).IniDato);
                    int[] ultimahoraPos = this.GetRowColOfWorbook(datosHeaderConfig.First(x => x.Campo1.Equals("ULTIMA_HORA")).IniDato);

                    headerCuttingDataDTO.LimitEst = Convert.ToInt32(workbook.Sheets[0].Rows[limitEstPos[0]].Cells[limitEstPos[1]].Value.ToString());
                    headerCuttingDataDTO.NoSerie = Convert.ToString(workbook.Sheets[0].Rows[nroserielimitEstPos[0]].Cells[nroserielimitEstPos[1]].Value);
                    headerCuttingDataDTO.PrimerCorte = Convert.ToDateTime(workbook.Sheets[0].Rows[primercortePos[0]].Cells[primercortePos[1]].Value.ToString());
                    headerCuttingDataDTO.SegundoCorte = Convert.ToDateTime(workbook.Sheets[0].Rows[segundocortePos[0]].Cells[segundocortePos[1]].Value.ToString());
                    headerCuttingDataDTO.TercerCorte = Convert.ToDateTime(workbook.Sheets[0].Rows[tercercortePos[0]].Cells[tercercortePos[1]].Value.ToString());
                    headerCuttingDataDTO.TipoRegresion = Convert.ToInt32(workbook.Sheets[0].Rows[tiporegresionPos[0]].Cells[tiporegresionPos[1]].Value.ToString());
                    headerCuttingDataDTO.UltimaHora = Convert.ToDateTime(workbook.Sheets[0].Rows[ultimahoraPos[0]].Cells[ultimahoraPos[1]].Value.ToString());

                    IEnumerable<ConfigurationETDReportsDTO> datosCortesSecConfig = configF3.Where(x => x.Tabla1.Equals("SPL_CORTESECC_EST") || (x.Tabla2.Equals("SPL_CORTESECC_EST") && x.ClaveIdioma == claveIdioma));

                    int[] tempPosC = this.GetRowColOfWorbook(datosCortesSecConfig.First(predicate: x => x.Campo1.Equals("TEMP_DEV_C") && x.Consecutivo == 1).IniDato);
                    int[] tempPosE = this.GetRowColOfWorbook(datosCortesSecConfig.First(predicate: x => x.Campo1.Equals("TEMP_DEV_C") && x.Consecutivo == 2).IniDato);

                    if (headerCuttingDataDTO.TipoRegresion is 1)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            SectionCuttingDataDTO sectionDetailsCDataDTO = new()
                            {
                                Terminal = workbook.Sheets[index: 0].Rows[tempPosC[0] - 2].Cells[tempPosC[1] + (i * 2)].Value.ToString(),
                                ResistZeroC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] - 1].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                ResistZeroE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] - 1].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                TempDevC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0]].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                TempDevE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0]].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                GradienteCaC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 1].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                GradienteCaE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 1].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                AwrC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 2].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                HsrC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 3].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                HstC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 4].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                AwrE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 2].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                HsrE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 3].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                HstE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 4].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                LimiteEst = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 5].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                            };
                            headerCuttingDataDTO.SectionCuttingData.Add(sectionDetailsCDataDTO);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            SectionCuttingDataDTO sectionDetailsCDataDTO = new()
                            {
                                Terminal = workbook.Sheets[index: 0].Rows[tempPosE[0] - 2].Cells[tempPosE[1] + (i * 2)].Value.ToString(),
                                ResistZeroC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] - 1].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                ResistZeroE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] - 1].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                TempDevC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0]].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                TempDevE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0]].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                GradienteCaC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 1].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                GradienteCaE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 1].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                AwrC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 2].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                HsrC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 3].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                HstC = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 4].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                                AwrE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 2].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                HsrE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 3].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                HstE = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosC[0] + 4].Cells[tempPosC[1] + (i * 2)].Value.ToString()),
                                LimiteEst = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[tempPosE[0] + 5].Cells[tempPosE[1] + (i * 2)].Value.ToString()),
                            };
                            headerCuttingDataDTO.SectionCuttingData.Add(sectionDetailsCDataDTO);
                        }
                    }

                    IEnumerable<ConfigurationETDReportsDTO> datosCortesSecDetailsConfig = configF3.Where(x => x.Tabla1.Equals("SPL_CORTEDETA_EST") || (x.Tabla2.Equals("SPL_CORTEDETA_EST") && x.ClaveIdioma == claveIdioma));

                    int[] TiempPos1 = this.GetRowColOfWorbook(datosCortesSecConfig.First(predicate: x => x.Campo1.Equals("TEMP_DEV_C") && x.Seccion == 1).IniDato);
                    int[] TiempPos2 = this.GetRowColOfWorbook(datosCortesSecConfig.First(predicate: x => x.Campo1.Equals("TEMP_DEV_C") && x.Seccion == 2).IniDato);
                    int[] TiempPos3 = this.GetRowColOfWorbook(datosCortesSecConfig.First(predicate: x => x.Campo1.Equals("TEMP_DEV_C") && x.Seccion == 3).IniDato);

                    for (int i = 0; i < 3; i++)
                    {
                        headerCuttingDataDTO.SectionCuttingData[i].DetailCuttingData = new List<DetailCuttingDataDTO>();
                        if (i is 0)
                        {
                            for (int j = 0; j < 29; j++)
                            {
                                headerCuttingDataDTO.SectionCuttingData[i].DetailCuttingData.Add(new DetailCuttingDataDTO()
                                {
                                    Tiempo = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos1[0] + j].Cells[TiempPos1[1]].Value.ToString()),
                                    Resistencia = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos1[0] + j].Cells[TiempPos1[1] + 1].Value.ToString()),
                                    TempR = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos1[0] + j].Cells[TiempPos1[1] + 2].Value.ToString()),
                                    Seccion = 1,
                                    Renglon = j + 1
                                });
                            }
                        }
                        else if (i is 1)
                        {
                            for (int j = 0; j < 29; j++)
                            {
                                headerCuttingDataDTO.SectionCuttingData[i].DetailCuttingData.Add(new DetailCuttingDataDTO()
                                {
                                    Tiempo = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos2[0] + j].Cells[TiempPos2[1]].Value.ToString()),
                                    Resistencia = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos2[0] + j].Cells[TiempPos2[1] + 1].Value.ToString()),
                                    TempR = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos2[0] + j].Cells[TiempPos2[1] + 2].Value.ToString()),
                                    Seccion = 1,
                                    Renglon = j + 1
                                });
                            }
                        }
                        else
                        {
                            for (int j = 0; j < 29; j++)
                            {
                                headerCuttingDataDTO.SectionCuttingData[i].DetailCuttingData.Add(new DetailCuttingDataDTO()
                                {
                                    Tiempo = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos3[0] + j].Cells[TiempPos3[1]].Value.ToString()),
                                    Resistencia = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos3[0] + j].Cells[TiempPos3[1] + 1].Value.ToString()),
                                    TempR = Convert.ToDecimal(workbook.Sheets[index: 0].Rows[TiempPos3[0] + j].Cells[TiempPos3[1] + 2].Value.ToString()),
                                    Seccion = 1,
                                    Renglon = j + 1
                                });
                            }
                        }
                    }
                    headerCuttingDatas.Add(headerCuttingDataDTO);
                    #endregion
                    #region RepF3
                    #region Seccion 1
                    #region Cabecera
                    ETDReportDTO eTDReportDTO = new();
                    IEnumerable<ConfigurationETDReportsDTO> configRep1 = configF3.Where(x => x.Hoja.Equals("Rep.F3") && x.Seccion == 1);
                    //Datos de la cabecera
                    int[] cliPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("CLIENTE")).IniDato);
                    int[] seriePos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("NO_SERIE")).IniDato);
                    int[] ratingPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("CAPACIDAD") && x.Tabla1.Equals("SPL_INFO_GENERAL_ETD")).IniDato);
                    //Datos de la seccion 1
                    int[] fechaPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("FECHA_PRUEBA") && x.Tabla1.Equals("SPL_INFO_SECCION_ETD")).IniDato);
                    int[] capPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("CAPACIDAD") && x.Tabla1.Equals("SPL_INFO_SECCION_ETD")).IniDato);
                    //la posicion de perdida esta debajo de capPos
                    int[] coolPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("COOLING_TYPE")).IniDato);
                    int[] alPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("ALTITUDE_F1")).IniDato);
                    int[] alPos2 = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("ALTITUDE_F2")).IniDato);
                    int[] atPo = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("POS_AT")).IniDato);
                    int[] btPo = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("POS_BT")).IniDato);
                    int[] terPo = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("POS_TER")).IniDato);

                    eTDReportDTO.Cliente = workbook.Sheets[0].Rows[cliPos[0]].Cells[cliPos[1]].Value.ToString();
                    eTDReportDTO.NoSerie = workbook.Sheets[0].Rows[seriePos[0]].Cells[seriePos[1]].Value.ToString();
                    eTDReportDTO.Capacidad = workbook.Sheets[0].Rows[ratingPos[0]].Cells[ratingPos[1]].Value.ToString();
                    #endregion

                    #region Datos sueltos
                    eTDReportDTO.ETDTestsGeneral = new ETDTestsGeneralDTO() { ETDTests = new List<ETDTestsDTO>() };

                    ETDTestsDTO secc1 = new()
                    {
                        FechaPrueba = Convert.ToDateTime(workbook.Sheets[0].Rows[fechaPos[0]].Cells[fechaPos[1]].Value.ToString()),
                        Capacidad = Convert.ToDecimal(workbook.Sheets[0].Rows[capPos[0]].Cells[capPos[1]].Value.ToString()),
                        CoolingType = workbook.Sheets[0].Rows[coolPos[0]].Cells[coolPos[1]].Value.ToString(),
                        AltitudeF1 = Convert.ToDecimal(workbook.Sheets[0].Rows[alPos[0]].Cells[alPos[1]].Value.ToString()),
                        AltitudeF2 = workbook.Sheets[0].Rows[alPos2[0]].Cells[alPos2[1]].Value.ToString(),
                        PosAt = workbook.Sheets[0].Rows[atPo[0]].Cells[atPo[1]].Value.ToString(),
                        PosBt = workbook.Sheets[0].Rows[btPo[0]].Cells[btPo[1]].Value.ToString(),
                        PosTer = stabilizationDataDTO.CapacidadTER is 0 ? "" : workbook.Sheets[0].Rows[terPo[0]].Cells[terPo[1]].Value.ToString(),
                        Perdidas = Convert.ToDecimal(workbook.Sheets[0].Rows[capPos[0] + 1].Cells[capPos[1]].Value.ToString()),
                        ETDTestsDetails = new List<ETDTestsDetailsDTO>(),
                        GraphicETDTests = new List<GraphicETDTestsDTO>(),
                        Seccion = 1
                    };
                    #endregion

                    #region Tabla detalles

                    int[] horaPos = this.GetRowColOfWorbook(configRep1.First(predicate: x => x.Campo1.Equals("FECHA_HORA") && x.Consecutivo == 1).IniDato);

                    for (int i = 0; i < 4; i++)
                    {
                        secc1.ETDTestsDetails.Add(new ETDTestsDetailsDTO()
                        {
                            FechaHora = Convert.ToDateTime(workbook.Sheets[0].Rows[horaPos[0]].Cells[horaPos[1] + i].Value.ToString()),
                            PromRadSup = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 1].Cells[horaPos[1] + i].Value.ToString()),
                            PromRadInf = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 2].Cells[horaPos[1] + i].Value.ToString()),
                            Ambiente1 = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 3].Cells[horaPos[1] + i].Value.ToString()),
                            Ambiente2 = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 4].Cells[horaPos[1] + i].Value.ToString()),
                            Ambiente3 = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 5].Cells[horaPos[1] + i].Value.ToString()),
                            AmbienteProm = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 6].Cells[horaPos[1] + i].Value.ToString()),
                            TempTapa = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 7].Cells[horaPos[1] + i].Value.ToString()),
                            Tor = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 8].Cells[horaPos[1] + i].Value.ToString()),
                            Aor = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 9].Cells[horaPos[1] + i].Value.ToString()),
                            Bor = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 10].Cells[horaPos[1] + i].Value.ToString()),
                            ElevAceiteSup = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 16].Cells[horaPos[1] + i].Value.ToString()),
                            ElevAceiteProm = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 17].Cells[horaPos[1] + i].Value.ToString()),
                            ElevAceiteInf = Convert.ToDecimal(workbook.Sheets[0].Rows[horaPos[0] + 18].Cells[horaPos[1] + i].Value.ToString())
                        });
                    }

                    eTDReportDTO.ETDTestsGeneral.ETDTests.Add(secc1);
                    #endregion

                    #endregion

                    #region Seccion 2 3 y posible 4

                    int secNum = stabilizationDataDTO.CapacidadTER is 0 ? 2 : 3;

                    for (int sec = 1; sec <= secNum; sec++)
                    {
                        #region Datos Sueltos
                        IEnumerable<ConfigurationETDReportsDTO> configRep2 = configF3.Where(x => x.Hoja.Equals("Rep.F3") && x.Seccion == sec + 1);
                        fechaPos = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("FECHA_PRUEBA") && x.Tabla1.Equals("SPL_INFO_SECCION_ETD")).IniDato);

                        capPos = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("CAPACIDAD") && x.Tabla1.Equals("SPL_INFO_SECCION_ETD")).IniDato);
                        atPo = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("POS_AT")).IniDato);

                        int[] resisPos = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("RESIST_CORTE")).IniDato);
                        int[] tempPromPos = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("TEMP_PROM_ACEITE")).IniDato);
                        int[] termPos = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("TERMINAL")).IniDato);

                        ETDTestsDTO secc234 = new()
                        {
                            FechaPrueba = Convert.ToDateTime(workbook.Sheets[0].Rows[fechaPos[0]].Cells[fechaPos[1]].Value.ToString()),
                            Capacidad = Convert.ToDecimal(workbook.Sheets[0].Rows[capPos[0]].Cells[capPos[1]].Value.ToString()),
                            CoolingType = workbook.Sheets[0].Rows[capPos[0] + 1].Cells[capPos[1]].Value.ToString(),
                            AltitudeF1 = Convert.ToDecimal(workbook.Sheets[0].Rows[capPos[0] + 3].Cells[capPos[1]].Value.ToString()),
                            AltitudeF2 = workbook.Sheets[0].Rows[capPos[0] + 3].Cells[capPos[1] + 1].Value.ToString(),
                            PosAt = workbook.Sheets[0].Rows[atPo[0]].Cells[atPo[1]].Value.ToString(),
                            PosBt = workbook.Sheets[0].Rows[atPo[0] + 1].Cells[atPo[1]].Value.ToString(),
                            PosTer = stabilizationDataDTO.CapacidadTER is 0 ? "" : workbook.Sheets[0].Rows[atPo[0] + 2].Cells[atPo[1]].Value.ToString(),
                            ResistCorte = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0]].Cells[resisPos[1]].Value.ToString()),
                            TempResistCorte = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 1].Cells[resisPos[1]].Value.ToString()),
                            FactorK = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 2].Cells[resisPos[1]].Value.ToString()),
                            ResistTcero = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 3].Cells[resisPos[1]].Value.ToString()),
                            TempDev = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 5].Cells[resisPos[1]].Value.ToString()),
                            GradienteDev = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 6].Cells[resisPos[1]].Value.ToString()),
                            ElevPromDev = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 7].Cells[resisPos[1]].Value.ToString()),
                            ElevPtoMasCal = Convert.ToDecimal(workbook.Sheets[0].Rows[resisPos[0] + 8].Cells[resisPos[1]].Value.ToString()),
                            TempPromAceite = Convert.ToDecimal(workbook.Sheets[0].Rows[tempPromPos[0]].Cells[tempPromPos[1]].Value.ToString()),
                            Terminal = workbook.Sheets[0].Rows[termPos[0]].Cells[termPos[1]].Value.ToString(),
                            ETDTestsDetails = new List<ETDTestsDetailsDTO>(),
                            Seccion = 1 + sec
                        };
                        #endregion

                        #region Tabla detalles

                        int[] tiemPos = this.GetRowColOfWorbook(configRep2.First(predicate: x => x.Campo1.Equals("TIEMPO")).IniDato);

                        for (int i = 0; i < 4; i++)
                        {
                            secc234.ETDTestsDetails.Add(new ETDTestsDetailsDTO()
                            {
                                Tiempo = Convert.ToDecimal(workbook.Sheets[0].Rows[tiemPos[0] + i].Cells[tiemPos[1]].Value.ToString()),
                                Resistencia = Convert.ToDecimal(workbook.Sheets[0].Rows[tiemPos[0] + i].Cells[tiemPos[1] + 2].Value.ToString()),
                                Seccion = 1 + sec,
                                Renglon = i + 1
                            });
                        }
                        #endregion
                        #region Datos Grafica
                        int[] grafXPos = this.GetRowColOfWorbook(configRep2.First(x => x.Campo1.Equals("VALOR_X") && x.Seccion == sec + 1).IniDato);

                        for (int i = 0; i < 35; i++)
                        {
                            secc234.GraphicETDTests.Add(new()
                            {
                                ValorX = Convert.ToDecimal(workbook.Sheets[0].Rows[grafXPos[0] + i].Cells[grafXPos[1]].Value.ToString()),
                                ValorY = Convert.ToDecimal(workbook.Sheets[0].Rows[grafXPos[0] + i].Cells[grafXPos[1] + 1].Value.ToString()),
                                ValorZ = Convert.ToDecimal(workbook.Sheets[0].Rows[grafXPos[0] + i].Cells[grafXPos[1] + 2].Value.ToString()),
                                Seccion = 1 + sec,
                                Renglon = i + 1
                            });
                        }

                        eTDReportDTO.ETDTestsGeneral.ETDTests.Add(secc234);
                        #endregion
                    }
                    #endregion
                    eTDReports.Add(eTDReportDTO);
                    #endregion
                }
                eTDUploadResult.HeaderCuttingDatas = headerCuttingDatas;
                eTDUploadResult.ETDReports = eTDReports;
                eTDUploadResult.Errors = listErrors;

            }
            catch (Exception ex)
            {
                listErrors.Add(new ErrorColumnsDTO(0, 0, ex.Message));
                return eTDUploadResult;
            }

            return eTDUploadResult;
        }

        #region Private Methods

        public static string SinTildes(string texto) =>
           new string(
          texto.Normalize(NormalizationForm.FormD)
          .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
          .ToArray()).Normalize(NormalizationForm.FormC);
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
}
