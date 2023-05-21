namespace SPL.WebApp.Domain.Services.Imp.RAN
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;

    using Telerik.Web.Spreadsheet;

    public class RanService : IRanService
    {
        #region Error message

        private const string messageErrorNumericES = "Debe Insertar un valor numérico.";
        private const string messageErrorNumericEN = "The value must be numeric.";
        private const string messageErrorText = "Este campo es requerido";

        #endregion

        public void PrepareTemplate_RAN_AYD(int measuring, string languageKey, SettingsToDisplayRANReportsDTO reportsDTO, ref Workbook workbook)
        {
            #region Update Readonly all cells

            workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));

            #endregion

            #region Head

            int[] _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cliente")).Celda);

            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.Client;
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = false;

            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoSerie")).Celda);

            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.NoSerie;
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = false;

            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Capacidad")).Celda);

            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.Capacity) ? string.Empty : $"{reportsDTO.HeadboardReport.Capacity} MVA";
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = false;

            #endregion

            #region Body
            IOrderedEnumerable<ConfigurationReportsDTO> configDate = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha_1") || c.Dato.Equals("Fecha_2") || c.Dato.Equals("Fecha")).OrderBy(c => c.Dato);

            foreach (ConfigurationReportsDTO item in configDate)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);

                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "MM/dd/yyyy";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.NoteFc) ? DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : reportsDTO.HeadboardReport.NoteFc;
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
            }

            #region Before, InsultaionResistence
            int[] descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_1")).Celda);
            int[] meditionResistenceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Medicion_1")).Celda);
            int[] unitMeditionResistanceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMMedicion_1")).Celda);
            int[] vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_1")).Celda);
            int[] limitlesLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Limite_1")).Celda);
            int[] durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_1")).Celda);
            int[] timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_1")).Celda);
            int[] unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_1")).Celda);

            for (int i = 0; i < measuring; i++)
            {
                InitializeBody(i, languageKey, descriptionLocation, meditionResistenceLocation, unitMeditionResistanceLocation, vcdLocation, limitlesLocation, durationLocation, timeLocation, unitTimeLocation, null, ref workbook);
            }
            #endregion

            #region Before, Applied Potential
            descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_2")).Celda);
            vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_2")).Celda);
            durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_2")).Celda);
            timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_2")).Celda);
            unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_2")).Celda);
            int[] validLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valido_2")).Celda);

            for (int i = 0; i < measuring; i++)
            {
                InitializeBody(i, languageKey, descriptionLocation, null, null,
                    vcdLocation, null, durationLocation, timeLocation, unitTimeLocation, validLocation, ref workbook);
            }
            #endregion

            #region After, INSULATION RESISTANCE
            descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_3")).Celda);
            meditionResistenceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Medicion_3")).Celda);
            unitMeditionResistanceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMMedicion_3")).Celda);
            vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_3")).Celda);
            limitlesLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Limite_3")).Celda);
            durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_3")).Celda);
            timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_3")).Celda);
            unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_3")).Celda);

            for (int i = 0; i < measuring; i++)
            {
                InitializeBody(i, languageKey, descriptionLocation, meditionResistenceLocation, unitMeditionResistanceLocation, vcdLocation, limitlesLocation, durationLocation, timeLocation, unitTimeLocation, null, ref workbook);
            }
            #endregion

            #region After, APPLIED POTENTIAL
            descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_4")).Celda);
            vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_4")).Celda);
            durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_4")).Celda);
            timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_4")).Celda);
            unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_4")).Celda);
            validLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valido_4")).Celda);

            for (int i = 0; i < measuring; i++)
            {
                InitializeBody(i, languageKey, descriptionLocation, null, null, vcdLocation, null, durationLocation, timeLocation, unitTimeLocation, validLocation, ref workbook);
            }
            #endregion

            #endregion
        }

        public void PrepareTemplate_RAN_APD(int measuring, string languageKey, SettingsToDisplayRANReportsDTO reportsDTO, ref Workbook workbook)
        {

            #region Update Readonly all cells
            workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Enable = false));
            #endregion

            #region Head

            int[] _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cliente")).Celda);

            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.Client;
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = false;

            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoSerie")).Celda);

            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.NoSerie;
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = false;

            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Capacidad")).Celda);

            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.Capacity) ? string.Empty : $"{reportsDTO.HeadboardReport.Capacity} MVA";
            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = false;

            #endregion

            #region Body
            IOrderedEnumerable<ConfigurationReportsDTO> configDate = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha_1") || c.Dato.Equals("Fecha_2") || c.Dato.Equals("Fecha")).OrderBy(c => c.Dato);

            foreach (ConfigurationReportsDTO item in configDate)
            {
                _positionWB = GetRowColOfWorbook(item.Celda);

                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "MM/dd/yyyy";
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.NoteFc) ? DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : reportsDTO.HeadboardReport.NoteFc;
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
            }

            #region Before, InsultaionResistence
            int[] descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_1")).Celda);
            int[] meditionResistenceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Medicion_1")).Celda);
            int[] unitMeditionResistanceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMMedicion_1")).Celda);
            int[] vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_1")).Celda);
            int[] limitlesLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Limite_1")).Celda);
            int[] durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_1")).Celda);
            int[] timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_1")).Celda);
            int[] unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_1")).Celda);

            for (int i = 0; i < measuring; i++)
            {
                InitializeBody(i, languageKey, descriptionLocation, meditionResistenceLocation, unitMeditionResistanceLocation, vcdLocation, limitlesLocation, durationLocation, timeLocation, unitTimeLocation, null, ref workbook);
            }

            #endregion

            #region Before, Applied Potential
            descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_2")).Celda);
            vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_2")).Celda);
            durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_2")).Celda);
            timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_2")).Celda);
            unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_2")).Celda);
            int[] validLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valido_2")).Celda);

            for (int i = 0; i < measuring; i++)
            {
                InitializeBody(i, languageKey, descriptionLocation, null, null, vcdLocation, null, durationLocation, timeLocation, unitTimeLocation, validLocation, ref workbook);
            }

            #endregion

            #endregion
        }

        public void Prepare_RAN_Test(string testType, int measuring, SettingsToDisplayRANReportsDTO reportsDTO, Workbook workbook, ref RANTestsDetailsDTO _ranTestDTO)
        {
            IOrderedEnumerable<ConfigurationReportsDTO> configDate = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha_1") || c.Dato.Equals("Fecha_2") || c.Dato.Equals("Fecha")).OrderBy(c => c.Dato);

            foreach (ConfigurationReportsDTO item in configDate)
            {
                int[] _positionWB = GetRowColOfWorbook(item.Celda);

                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.NoteFc) ? workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value : reportsDTO.HeadboardReport.NoteFc;
            }

            if (testType.Equals(TestType.AYD.ToString()))
            {
                _ranTestDTO.RANTestsDetailsRAs = new List<RANTestsDetailsRADTO>();
                _ranTestDTO.RANTestsDetailsTAs = new List<RANTestsDetailsTADTO>();

                #region Before, InsultaionResistence
                int[] descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_1")).Celda);
                int[] meditionResistenceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Medicion_1")).Celda);
                int[] unitMeditionResistanceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMMedicion_1")).Celda);
                int[] vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_1")).Celda);
                int[] limitlesLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Limite_1")).Celda);
                int[] durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_1")).Celda);
                int[] timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_1")).Celda);
                int[] unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_1")).Celda);

                for (int i = 0; i < measuring; i++)
                {
                    RANTestsDetailsRADTO result = GetValueFromWorkbook(i, descriptionLocation, meditionResistenceLocation, unitMeditionResistanceLocation, vcdLocation, limitlesLocation, durationLocation, timeLocation, unitTimeLocation, workbook);
                    result.Section = 1;
                    _ranTestDTO.RANTestsDetailsRAs.Add(result);
                }
                #endregion

                #region Before, Applied Potential
                descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_2")).Celda);
                vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_2")).Celda);
                durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_2")).Celda);
                timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_2")).Celda);
                unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_2")).Celda);
                int[] validLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valido_2")).Celda);

                for (int i = 0; i < measuring; i++)
                {
                    RANTestsDetailsTADTO result = GetValueFromWorkbook(i, descriptionLocation, meditionResistenceLocation, unitMeditionResistanceLocation, vcdLocation, limitlesLocation, durationLocation, timeLocation, unitTimeLocation, validLocation, workbook);
                    result.Section = 2;
                    _ranTestDTO.RANTestsDetailsTAs.Add(result);
                }
                #endregion

                #region After, INSULATION RESISTANCE
                descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_3")).Celda);
                meditionResistenceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Medicion_3")).Celda);
                unitMeditionResistanceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMMedicion_3")).Celda);
                vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_3")).Celda);
                limitlesLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Limite_3")).Celda);
                durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_3")).Celda);
                timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_3")).Celda);
                unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_3")).Celda);

                for (int i = 0; i < measuring; i++)
                {
                    RANTestsDetailsRADTO result = GetValueFromWorkbook(i, descriptionLocation, meditionResistenceLocation, unitMeditionResistanceLocation, vcdLocation, limitlesLocation, durationLocation, timeLocation, unitTimeLocation, workbook);
                    result.Section = 3;
                    _ranTestDTO.RANTestsDetailsRAs.Add(result);
                }
                #endregion

                #region After, APPLIED POTENTIAL
                descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_4")).Celda);
                vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_4")).Celda);
                durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_4")).Celda);
                timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_4")).Celda);
                unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_4")).Celda);
                validLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valido_4")).Celda);

                for (int i = 0; i < measuring; i++)
                {
                    RANTestsDetailsTADTO result = GetValueFromWorkbook(i, descriptionLocation, meditionResistenceLocation, unitMeditionResistanceLocation, vcdLocation, limitlesLocation, durationLocation, timeLocation, unitTimeLocation, validLocation, workbook);
                    result.Section = 4;
                    _ranTestDTO.RANTestsDetailsTAs.Add(result);
                }
                #endregion
            }
            else
            {
                _ranTestDTO.RANTestsDetailsRAs = new List<RANTestsDetailsRADTO>();
                _ranTestDTO.RANTestsDetailsTAs = new List<RANTestsDetailsTADTO>();
                #region Before, InsultaionResistence
                int[] descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_1")).Celda);
                int[] meditionResistenceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Medicion_1")).Celda);
                int[] unitMeditionResistanceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMMedicion_1")).Celda);
                int[] vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_1")).Celda);
                int[] limitlesLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Limite_1")).Celda);
                int[] durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_1")).Celda);
                int[] timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_1")).Celda);
                int[] unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_1")).Celda);

                for (int i = 0; i < measuring; i++)
                {
                    RANTestsDetailsRADTO result = GetValueFromWorkbook(i, descriptionLocation, meditionResistenceLocation, unitMeditionResistanceLocation, vcdLocation, limitlesLocation, durationLocation, timeLocation, unitTimeLocation, workbook);
                    result.Section = 1;
                    _ranTestDTO.RANTestsDetailsRAs.Add(result);
                }
                #endregion

                #region Before, Applied Potential
                descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_2")).Celda);
                vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_2")).Celda);
                durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_2")).Celda);
                timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_2")).Celda);
                unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_2")).Celda);
                int[] validLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valido_2")).Celda);

                for (int i = 0; i < measuring; i++)
                {
                    RANTestsDetailsTADTO result = GetValueFromWorkbook(i, descriptionLocation, meditionResistenceLocation, unitMeditionResistanceLocation, vcdLocation, limitlesLocation, durationLocation, timeLocation, unitTimeLocation, validLocation, workbook);
                    result.Section = 2;
                    _ranTestDTO.RANTestsDetailsTAs.Add(result);
                }
                #endregion
            }
        }

        public void PrepareIndexOfRAN(ResultRANTestsDTO resultRANTestsDTO, SettingsToDisplayRANReportsDTO reportsDTO, string keyLenguage, ref Workbook workbook, string ClavePrueba)
        {
            int[] resultLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
            bool result = !resultRANTestsDTO.MessageErrors.Any();
            workbook.Sheets[0].Rows[resultLocation[0]].Cells[resultLocation[1]].Value = keyLenguage switch
            {
                "ES" => result ? "Aceptado" : "Rechazado",
                "EN" => result ? "Accepted" : "Rejected",
                _ => result ? "Aceptado" : "Rechazado",
            };
        }

        public void DeleteValid(string testType, int measuring, SettingsToDisplayRANReportsDTO reportsDTO, ref Workbook workbook)
        {
            if (TestType.AYD.ToString().Equals(testType))
            {
                int[] validLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valido_2")).Celda);
                workbook.Sheets[0].Rows[validLocation[0] - 1].Cells[validLocation[1]].Value = string.Empty;
                for (int i = 0; i < measuring; i++)
                {
                    workbook.Sheets[0].Rows[validLocation[0] + i].Cells[validLocation[1]].Validation = null;
                    workbook.Sheets[0].Rows[validLocation[0] + i].Cells[validLocation[1]].Color = "#FFFFFF";
                    workbook.Sheets[0].Rows[validLocation[0] + i].Cells[validLocation[1]].Value = string.Empty;
                }

                validLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valido_4")).Celda);
                workbook.Sheets[0].Rows[validLocation[0] - 1].Cells[validLocation[1]].Value = string.Empty;
                for (int i = 0; i < measuring; i++)
                {
                    workbook.Sheets[0].Rows[validLocation[0] + i].Cells[validLocation[1]].Validation = null;
                    workbook.Sheets[0].Rows[validLocation[0] + i].Cells[validLocation[1]].Color = "#FFFFFF";
                    workbook.Sheets[0].Rows[validLocation[0] + i].Cells[validLocation[1]].Value = string.Empty;
                }
            }
            else
            {
                int[] validLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valido_2")).Celda);
                workbook.Sheets[0].Rows[validLocation[0] - 1].Cells[validLocation[1]].Value = string.Empty;
                for (int i = 0; i < measuring; i++)
                {
                    workbook.Sheets[0].Rows[validLocation[0] + i].Cells[validLocation[1]].Validation = null;
                    workbook.Sheets[0].Rows[validLocation[0] + i].Cells[validLocation[1]].Color = "#FFFFFF";
                    workbook.Sheets[0].Rows[validLocation[0] + i].Cells[validLocation[1]].Value = string.Empty;
                }
            }
        }

        public void CloneWorkbook(string testType, string ketLanguage, Workbook workbook, SettingsToDisplayRANReportsDTO reportsDTO, int measuring, ref Workbook officialWorkbook)
        {
            IOrderedEnumerable<ConfigurationReportsDTO> configDate = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha_1") || c.Dato.Equals("Fecha_2") || c.Dato.Equals("Fecha")).OrderBy(c => c.Dato);

            foreach (ConfigurationReportsDTO item in configDate)
            {
                int[] _positionWB = GetRowColOfWorbook(item.Celda);
                officialWorkbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value;
            }

            if (testType.Equals(TestType.AYD.ToString()))
            {
                #region Before, InsultaionResistence
                int[] descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_1")).Celda);
                int[] meditionResistenceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Medicion_1")).Celda);
                int[] unitMeditionResistanceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMMedicion_1")).Celda);
                int[] vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_1")).Celda);
                int[] limitlesLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Limite_1")).Celda);
                int[] durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_1")).Celda);
                int[] timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_1")).Celda);
                int[] unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_1")).Celda);

                for (int i = 0; i < measuring; i++)
                {
                    CopyRanRadSectionToWorkbookOfficial(i, descriptionLocation, meditionResistenceLocation, unitMeditionResistanceLocation, vcdLocation, limitlesLocation, durationLocation, timeLocation, unitTimeLocation, workbook, ref officialWorkbook);
                }
                #endregion

                #region Before, Applied Potential
                descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_2")).Celda);
                vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_2")).Celda);
                durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_2")).Celda);
                timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_2")).Celda);
                unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_2")).Celda);
                int[] validLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valido_2")).Celda);

                for (int i = 0; i < measuring; i++)
                {
                    _ = CopyRanTASectionToWorkbookOfficial(i, descriptionLocation, vcdLocation, limitlesLocation, durationLocation, timeLocation, unitTimeLocation, validLocation, workbook, ref officialWorkbook);
                }
                #endregion

                #region After, INSULATION RESISTANCE
                descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_3")).Celda);
                meditionResistenceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Medicion_3")).Celda);
                unitMeditionResistanceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMMedicion_3")).Celda);
                vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_3")).Celda);
                limitlesLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Limite_3")).Celda);
                durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_3")).Celda);
                timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_3")).Celda);
                unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_3")).Celda);

                for (int i = 0; i < measuring; i++)
                {
                    CopyRanRadSectionToWorkbookOfficial(i, descriptionLocation, meditionResistenceLocation, unitMeditionResistanceLocation, vcdLocation, limitlesLocation, durationLocation, timeLocation, unitTimeLocation, workbook, ref officialWorkbook);
                }
                #endregion

                #region After, APPLIED POTENTIAL
                descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_4")).Celda);
                vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_4")).Celda);
                durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_4")).Celda);
                timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_4")).Celda);
                unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_4")).Celda);
                validLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valido_4")).Celda);

                for (int i = 0; i < measuring; i++)
                {
                    _ = CopyRanTASectionToWorkbookOfficial(i, descriptionLocation, vcdLocation, limitlesLocation, durationLocation, timeLocation, unitTimeLocation, validLocation, workbook, ref officialWorkbook);
                }
                #endregion
            }
            else
            {
                #region Before, InsultaionResistence
                int[] descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_1")).Celda);
                int[] meditionResistenceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Medicion_1")).Celda);
                int[] unitMeditionResistanceLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMMedicion_1")).Celda);
                int[] vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_1")).Celda);
                int[] limitlesLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Limite_1")).Celda);
                int[] durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_1")).Celda);
                int[] timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_1")).Celda);
                int[] unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_1")).Celda);

                for (int i = 0; i < measuring; i++)
                {
                    CopyRanRadSectionToWorkbookOfficial(i, descriptionLocation, meditionResistenceLocation, unitMeditionResistanceLocation, vcdLocation, limitlesLocation, durationLocation, timeLocation, unitTimeLocation, workbook, ref officialWorkbook);
                }
                #endregion

                #region Before, Applied Potential
                descriptionLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Descripcion_2")).Celda);
                vcdLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("VCD_2")).Celda);
                durationLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Duracion_2")).Celda);
                timeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo_2")).Celda);
                unitTimeLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMTiempo_2")).Celda);
                int[] validLocation = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valido_2")).Celda);

                for (int i = 0; i < measuring; i++)
                {
                    _ = CopyRanTASectionToWorkbookOfficial(i, descriptionLocation, vcdLocation, limitlesLocation, durationLocation, timeLocation, unitTimeLocation, validLocation, workbook, ref officialWorkbook);
                }
                #endregion
            }

            ConfigurationReportsDTO resultOption = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado"));
            int[] resultLocation = GetRowColOfWorbook(resultOption.Celda);
            officialWorkbook.Sheets[0].Rows[resultLocation[0]].Cells[resultLocation[1]].Value = workbook.Sheets[0].Rows[resultLocation[0]].Cells[resultLocation[1]].Value;

            if (testType == "AYD")
            {
                int[] valid1 = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valido_2") && c.ClavePrueba == "AYD").Celda);
                int[] valid2 = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valido_4") && c.ClavePrueba == "AYD").Celda);

                workbook.Sheets[0].Rows[valid1[0]].Cells[valid1[1]].Value = string.Empty;
                workbook.Sheets[0].Rows[valid1[0] + 1].Cells[valid1[1]].Value = string.Empty;

                workbook.Sheets[0].Rows[valid2[0]].Cells[valid2[1]].Value = string.Empty;
                workbook.Sheets[0].Rows[valid2[0] + 1].Cells[valid2[1]].Value = string.Empty;
            }
            else
            {
                int[] valid1 = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valido_2") && c.ClavePrueba == "APD").Celda);
                workbook.Sheets[0].Rows[valid1[0]].Cells[valid1[1]].Value = string.Empty;
                workbook.Sheets[0].Rows[valid1[0] + 1].Cells[valid1[1]].Value = string.Empty;

            }
        }

        public List<DateTime> GetDate(Workbook origin, SettingsToDisplayRANReportsDTO reportsDTO, string keyTest)
        {
            List<DateTime> dates = new();
            IEnumerable<ConfigurationReportsDTO> fechaCelda = keyTest.ToUpper().Equals("APD")
                ? reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha"))
                : reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha_1") || c.Dato.Equals("Fecha_2"));
            foreach (ConfigurationReportsDTO item in fechaCelda)
            {
                int[] _positionWB = GetRowColOfWorbook(item.Celda);
                string fecha = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();

                if (DateTime.TryParseExact(fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime objDT))
                {
                    dates.Add(objDT);
                }
                else
                {
                    double value = double.Parse(fecha);
                    DateTime date1 = DateTime.FromOADate(value);
                    dates.Add(date1);
                }
            }

            return dates;
        }

        #region Private Methods
        private RANTestsDetailsRADTO GetValueFromWorkbook(
            int index,
            int[] descriptionLocation,
            int[] meditionResistenceLocation,
            int[] unitMeditionResistanceLocation,
            int[] vcdLocation,
            int[] limitlesLocation,
            int[] durationLocation,
            int[] timeLocation,
            int[] unitTimeLocation,
            Workbook workbook)
        {

            RANTestsDetailsRADTO ranTestsDetailsRADTO = new();

            if (descriptionLocation != null)
            {
                string descriptionString = workbook.Sheets[0].Rows[descriptionLocation[0] + index].Cells[descriptionLocation[1]].Value is null ? string.Empty : workbook.Sheets[0].Rows[descriptionLocation[0] + index].Cells[descriptionLocation[1]].Value.ToString();
                ranTestsDetailsRADTO.Description = descriptionString.Length >= 25 ? descriptionString[..25] : descriptionString;
            }

            if (meditionResistenceLocation != null)
                ranTestsDetailsRADTO.Measurement = workbook.Sheets[0].Rows[meditionResistenceLocation[0] + index].Cells[meditionResistenceLocation[1]].Value is null ? 0 : Convert.ToInt32(workbook.Sheets[0].Rows[meditionResistenceLocation[0] + index].Cells[meditionResistenceLocation[1]].Value);

            if (unitMeditionResistanceLocation != null)
                ranTestsDetailsRADTO.UMMeasurement = workbook.Sheets[0].Rows[unitMeditionResistanceLocation[0] + index].Cells[unitMeditionResistanceLocation[1]].Value is null ? string.Empty : workbook.Sheets[0].Rows[unitMeditionResistanceLocation[0] + index].Cells[unitMeditionResistanceLocation[1]].Value.ToString();

            if (vcdLocation != null)
                ranTestsDetailsRADTO.VCD = workbook.Sheets[0].Rows[vcdLocation[0] + index].Cells[vcdLocation[1]].Value is null ? 0 : Convert.ToInt32(workbook.Sheets[0].Rows[vcdLocation[0] + index].Cells[vcdLocation[1]].Value);

            if (limitlesLocation != null)
                ranTestsDetailsRADTO.Limit = workbook.Sheets[0].Rows[limitlesLocation[0] + index].Cells[limitlesLocation[1]].Value is null ? 0 : Convert.ToInt32(workbook.Sheets[0].Rows[limitlesLocation[0] + index].Cells[limitlesLocation[1]].Value);

            if (durationLocation != null)
                ranTestsDetailsRADTO.Duration = workbook.Sheets[0].Rows[durationLocation[0] + index].Cells[durationLocation[1]].Value is null ? string.Empty : Convert.ToString(workbook.Sheets[0].Rows[durationLocation[0] + index].Cells[durationLocation[1]].Value);

            if (timeLocation != null)
                ranTestsDetailsRADTO.Time = workbook.Sheets[0].Rows[timeLocation[0] + index].Cells[timeLocation[1]].Value is null ? 0 : Convert.ToInt32(workbook.Sheets[0].Rows[timeLocation[0] + index].Cells[timeLocation[1]].Value);

            if (unitTimeLocation != null)
                ranTestsDetailsRADTO.UMTime = workbook.Sheets[0].Rows[unitTimeLocation[0] + index].Cells[unitTimeLocation[1]].Value is null ? string.Empty : Convert.ToString(workbook.Sheets[0].Rows[unitTimeLocation[0] + index].Cells[unitTimeLocation[1]].Value);
            return ranTestsDetailsRADTO;
        }

        private void CopyRanRadSectionToWorkbookOfficial(
           int index,
           int[] descriptionLocation,
           int[] meditionResistenceLocation,
           int[] unitMeditionResistanceLocation,
           int[] vcdLocation,
           int[] limitlesLocation,
           int[] durationLocation,
           int[] timeLocation,
           int[] unitTimeLocation,
           Workbook workbook,
           ref Workbook officialWorkbook)
        {

            if (descriptionLocation != null)
            {
                string descriptionString = workbook.Sheets[0].Rows[descriptionLocation[0] + index].Cells[descriptionLocation[1]].Value is null ? string.Empty : workbook.Sheets[0].Rows[descriptionLocation[0] + index].Cells[descriptionLocation[1]].Value.ToString();
                officialWorkbook.Sheets[0].Rows[descriptionLocation[0] + index].Cells[descriptionLocation[1]].Value = descriptionString.Length > 25 ? descriptionString[..25] : descriptionString;
            }

            if (meditionResistenceLocation != null)
                officialWorkbook.Sheets[0].Rows[meditionResistenceLocation[0] + index].Cells[meditionResistenceLocation[1]].Value = workbook.Sheets[0].Rows[meditionResistenceLocation[0] + index].Cells[meditionResistenceLocation[1]].Value is null ? 0 : Convert.ToInt32(workbook.Sheets[0].Rows[meditionResistenceLocation[0] + index].Cells[meditionResistenceLocation[1]].Value);

            if (unitMeditionResistanceLocation != null)
                officialWorkbook.Sheets[0].Rows[unitMeditionResistanceLocation[0] + index].Cells[unitMeditionResistanceLocation[1]].Value = workbook.Sheets[0].Rows[unitMeditionResistanceLocation[0] + index].Cells[unitMeditionResistanceLocation[1]].Value is null ? string.Empty : workbook.Sheets[0].Rows[unitMeditionResistanceLocation[0] + index].Cells[unitMeditionResistanceLocation[1]].Value.ToString();

            if (vcdLocation != null)
                officialWorkbook.Sheets[0].Rows[vcdLocation[0] + index].Cells[vcdLocation[1]].Value = workbook.Sheets[0].Rows[vcdLocation[0] + index].Cells[vcdLocation[1]].Value is null ? 0 : Convert.ToInt32(workbook.Sheets[0].Rows[vcdLocation[0] + index].Cells[vcdLocation[1]].Value);

            if (limitlesLocation != null)
                officialWorkbook.Sheets[0].Rows[limitlesLocation[0] + index].Cells[limitlesLocation[1]].Value = workbook.Sheets[0].Rows[limitlesLocation[0] + index].Cells[limitlesLocation[1]].Value is null ? 0 : Convert.ToInt32(workbook.Sheets[0].Rows[limitlesLocation[0] + index].Cells[limitlesLocation[1]].Value);

            if (durationLocation != null)
                officialWorkbook.Sheets[0].Rows[durationLocation[0] + index].Cells[durationLocation[1]].Value = workbook.Sheets[0].Rows[durationLocation[0] + index].Cells[durationLocation[1]].Value is null ? string.Empty : Convert.ToString(workbook.Sheets[0].Rows[durationLocation[0] + index].Cells[durationLocation[1]].Value);

            if (timeLocation != null)
                officialWorkbook.Sheets[0].Rows[timeLocation[0] + index].Cells[timeLocation[1]].Value = workbook.Sheets[0].Rows[timeLocation[0] + index].Cells[timeLocation[1]].Value is null ? 0 : Convert.ToInt32(workbook.Sheets[0].Rows[timeLocation[0] + index].Cells[timeLocation[1]].Value);

            if (unitTimeLocation != null)
                officialWorkbook.Sheets[0].Rows[unitTimeLocation[0] + index].Cells[unitTimeLocation[1]].Value = workbook.Sheets[0].Rows[unitTimeLocation[0] + index].Cells[unitTimeLocation[1]].Value is null ? string.Empty : Convert.ToString(workbook.Sheets[0].Rows[unitTimeLocation[0] + index].Cells[unitTimeLocation[1]].Value);
        }

        private RANTestsDetailsTADTO GetValueFromWorkbook(
            int index,
            int[] descriptionLocation,
            int[] meditionResistenceLocation,
            int[] unitMeditionResistanceLocation,
            int[] vcdLocation,
            int[] limitlesLocation,
            int[] durationLocation,
            int[] timeLocation,
            int[] unitTimeLocation,
            int[] validLocation,
            Workbook workbook)
        {

            RANTestsDetailsTADTO ranTestsDetailsTADTO = new();

            if (descriptionLocation != null)
            {
                string descriptionString = workbook.Sheets[0].Rows[descriptionLocation[0] + index].Cells[descriptionLocation[1]].Value is null ? string.Empty : workbook.Sheets[0].Rows[descriptionLocation[0] + index].Cells[descriptionLocation[1]].Value.ToString();
                ranTestsDetailsTADTO.Description = descriptionString.Length > 25 ? descriptionString[..25] : descriptionString;
            }

            if (vcdLocation != null)
                ranTestsDetailsTADTO.VCD = workbook.Sheets[0].Rows[vcdLocation[0] + index].Cells[vcdLocation[1]].Value is null ? 0 : Convert.ToInt32(workbook.Sheets[0].Rows[vcdLocation[0] + index].Cells[vcdLocation[1]].Value);

            if (limitlesLocation != null)
                ranTestsDetailsTADTO.Limit = workbook.Sheets[0].Rows[limitlesLocation[0] + index].Cells[limitlesLocation[1]].Value is null ? 0 : Convert.ToInt32(workbook.Sheets[0].Rows[limitlesLocation[0] + index].Cells[limitlesLocation[1]].Value);

            if (durationLocation != null)
                ranTestsDetailsTADTO.Duration = workbook.Sheets[0].Rows[durationLocation[0] + index].Cells[durationLocation[1]].Value is null ? string.Empty : Convert.ToString(workbook.Sheets[0].Rows[durationLocation[0] + index].Cells[durationLocation[1]].Value);

            if (timeLocation != null)
                ranTestsDetailsTADTO.Time = workbook.Sheets[0].Rows[timeLocation[0] + index].Cells[timeLocation[1]].Value is null ? 0 : Convert.ToInt32(workbook.Sheets[0].Rows[timeLocation[0] + index].Cells[timeLocation[1]].Value);

            if (unitTimeLocation != null)
                ranTestsDetailsTADTO.UMTime = workbook.Sheets[0].Rows[unitTimeLocation[0] + index].Cells[unitTimeLocation[1]].Value is null ? string.Empty : Convert.ToString(workbook.Sheets[0].Rows[unitTimeLocation[0] + index].Cells[unitTimeLocation[1]].Value);

            if (validLocation != null)
            {
                ranTestsDetailsTADTO.Valid = (workbook.Sheets[0].Rows[validLocation[0] + index].Cells[validLocation[1]].Value is null ? "" : workbook.Sheets[0].Rows[validLocation[0] + index].Cells[validLocation[1]].Value.ToString()) switch
                {
                    "Yes" => true,
                    "Si" => true,
                    "No" => false,
                    _ => false,
                };
            }
            return ranTestsDetailsTADTO;
        }

        private RANTestsDetailsTADTO CopyRanTASectionToWorkbookOfficial(
            int index,
            int[] descriptionLocation,
            int[] vcdLocation,
            int[] limitlesLocation,
            int[] durationLocation,
            int[] timeLocation,
            int[] unitTimeLocation,
            int[] validLocation,
           Workbook workbook,
           ref Workbook officialWorkbook)
        {
            RANTestsDetailsTADTO ranTestsDetailsTADTO = new();

            if (descriptionLocation != null)
            {
                string descriptionString = workbook.Sheets[0].Rows[descriptionLocation[0] + index].Cells[descriptionLocation[1]].Value is null ? string.Empty : workbook.Sheets[0].Rows[descriptionLocation[0] + index].Cells[descriptionLocation[1]].Value.ToString();

                officialWorkbook.Sheets[0].Rows[descriptionLocation[0] + index].Cells[descriptionLocation[1]].Value = descriptionString.Length > 25 ? descriptionString[..25] : descriptionString;
            }

            if (vcdLocation != null)
                officialWorkbook.Sheets[0].Rows[vcdLocation[0] + index].Cells[vcdLocation[1]].Value = workbook.Sheets[0].Rows[vcdLocation[0] + index].Cells[vcdLocation[1]].Value is null ? 0 : Convert.ToInt32(workbook.Sheets[0].Rows[vcdLocation[0] + index].Cells[vcdLocation[1]].Value);

            if (limitlesLocation != null)
                officialWorkbook.Sheets[0].Rows[limitlesLocation[0] + index].Cells[limitlesLocation[1]].Value = workbook.Sheets[0].Rows[limitlesLocation[0] + index].Cells[limitlesLocation[1]].Value is null ? 0 : Convert.ToInt32(workbook.Sheets[0].Rows[limitlesLocation[0] + index].Cells[limitlesLocation[1]].Value);

            if (durationLocation != null)
                officialWorkbook.Sheets[0].Rows[durationLocation[0] + index].Cells[durationLocation[1]].Value = workbook.Sheets[0].Rows[durationLocation[0] + index].Cells[durationLocation[1]].Value is null ? string.Empty : Convert.ToString(workbook.Sheets[0].Rows[durationLocation[0] + index].Cells[durationLocation[1]].Value);

            if (timeLocation != null)
                officialWorkbook.Sheets[0].Rows[timeLocation[0] + index].Cells[timeLocation[1]].Value = workbook.Sheets[0].Rows[timeLocation[0] + index].Cells[timeLocation[1]].Value is null ? 0 : Convert.ToInt32(workbook.Sheets[0].Rows[timeLocation[0] + index].Cells[timeLocation[1]].Value);

            if (unitTimeLocation != null)
                officialWorkbook.Sheets[0].Rows[unitTimeLocation[0] + index].Cells[unitTimeLocation[1]].Value = workbook.Sheets[0].Rows[unitTimeLocation[0] + index].Cells[unitTimeLocation[1]].Value is null ? string.Empty : Convert.ToString(workbook.Sheets[0].Rows[unitTimeLocation[0] + index].Cells[unitTimeLocation[1]].Value);

            if (validLocation != null)
                officialWorkbook.Sheets[0].Rows[validLocation[0] + index].Cells[validLocation[1]].Value = workbook.Sheets[0].Rows[validLocation[0] + index].Cells[validLocation[1]].Value is null ? "" : workbook.Sheets[0].Rows[validLocation[0] + index].Cells[validLocation[1]].Value.ToString();
            return ranTestsDetailsTADTO;
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

        private static void InitializeBody(
            int index,
            string keyLanguage,
            int[] descriptionLocation,
            int[] measuringLocation,
            int[] meditionResistenceLocation,
            int[] vcdLocation,
            int[] limitLocation,
            int[] durationLocation,
            int[] timeLocation,
            int[] unitTimeLocation,
            int[] validLocation,
            ref Workbook workbook)
        {
            if (descriptionLocation != null)
                workbook.Sheets[0].Rows[descriptionLocation[0] + index].Cells[descriptionLocation[1]].Enable = true;

            if (measuringLocation != null)
            {
                workbook.Sheets[0].Rows[measuringLocation[0] + index].Cells[measuringLocation[1]].Enable = true;
                workbook.Sheets[0].Rows[measuringLocation[0] + index].Cells[measuringLocation[1]].Format = "#,#######";
                workbook.Sheets[0].Rows[measuringLocation[0] + index].Cells[measuringLocation[1]].TextAlign = "center";
                workbook.Sheets[0].Rows[measuringLocation[0] + index].Cells[measuringLocation[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "-1",
                    To = "999999999",
                    AllowNulls = true,
                    MessageTemplate = keyLanguage.Equals("ES") ? $"{messageErrorNumericES}" : $"{messageErrorNumericEN}",
                    Type = "reject",
                    TitleTemplate = "Error"
                };
            }

            if (meditionResistenceLocation != null)
            {
                workbook.Sheets[0].Rows[meditionResistenceLocation[0] + index].Cells[meditionResistenceLocation[1]].Enable = true;
                workbook.Sheets[0].Rows[meditionResistenceLocation[0] + index].Cells[meditionResistenceLocation[1]].Value = "Mohmios";
                workbook.Sheets[0].Rows[meditionResistenceLocation[0] + index].Cells[meditionResistenceLocation[1]].TextAlign = "center";
            }

            if (vcdLocation != null)
            {
                workbook.Sheets[0].Rows[vcdLocation[0] + index].Cells[vcdLocation[1]].Enable = true;
                workbook.Sheets[0].Rows[vcdLocation[0] + index].Cells[vcdLocation[1]].Format = "#,#######";
                workbook.Sheets[0].Rows[vcdLocation[0] + index].Cells[vcdLocation[1]].TextAlign = "center";
                workbook.Sheets[0].Rows[vcdLocation[0] + index].Cells[vcdLocation[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "-1",
                    To = "999999999",
                    AllowNulls = true,
                    MessageTemplate = keyLanguage.Equals("ES") ? $"{messageErrorNumericES}" : $"{messageErrorNumericEN}",
                    Type = "reject",
                    TitleTemplate = "Error"
                };
            }

            if (limitLocation != null)
            {
                workbook.Sheets[0].Rows[limitLocation[0] + index].Cells[limitLocation[1]].Enable = true;
                workbook.Sheets[0].Rows[limitLocation[0] + index].Cells[limitLocation[1]].Format = "#,#######";
                workbook.Sheets[0].Rows[limitLocation[0] + index].Cells[limitLocation[1]].Value = 200;
                workbook.Sheets[0].Rows[limitLocation[0] + index].Cells[limitLocation[1]].TextAlign = "center";
                workbook.Sheets[0].Rows[limitLocation[0] + index].Cells[limitLocation[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "-1",
                    To = "999999999",
                    AllowNulls = true,
                    MessageTemplate = keyLanguage.Equals("ES") ? $"{messageErrorNumericES}" : $"{messageErrorNumericEN}",
                    Type = "reject",
                    TitleTemplate = "Error"
                };

            }

            if (durationLocation != null)
            {
                workbook.Sheets[0].Rows[durationLocation[0] + index].Cells[durationLocation[1]].Enable = true;
                workbook.Sheets[0].Rows[durationLocation[0] + index].Cells[durationLocation[1]].Value = keyLanguage.Equals("ES") ? "Durante" : "During";
                workbook.Sheets[0].Rows[durationLocation[0] + index].Cells[durationLocation[1]].TextAlign = "center";
            }

            if (timeLocation != null)
            {
                workbook.Sheets[0].Rows[timeLocation[0] + index].Cells[timeLocation[1]].Enable = true;
                workbook.Sheets[0].Rows[timeLocation[0] + index].Cells[timeLocation[1]].Format = "#,#######";
                workbook.Sheets[0].Rows[timeLocation[0] + index].Cells[timeLocation[1]].Value = 1;
                workbook.Sheets[0].Rows[timeLocation[0] + index].Cells[timeLocation[1]].TextAlign = "center";
                workbook.Sheets[0].Rows[timeLocation[0] + index].Cells[timeLocation[1]].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "-1",
                    To = "999999999",
                    AllowNulls = true,
                    MessageTemplate = keyLanguage.Equals("ES") ? $"{messageErrorNumericES}" : $"{messageErrorNumericEN}",
                    Type = "reject",
                    TitleTemplate = "Error"
                };
            }

            if (unitTimeLocation != null)
            {
                workbook.Sheets[0].Rows[unitTimeLocation[0] + index].Cells[unitTimeLocation[1]].Enable = true;
                workbook.Sheets[0].Rows[unitTimeLocation[0] + index].Cells[unitTimeLocation[1]].Value = keyLanguage.Equals("ES") ? "Minuto" : "Minute";
                workbook.Sheets[0].Rows[unitTimeLocation[0] + index].Cells[unitTimeLocation[1]].TextAlign = "center";
            }

            if (validLocation != null)
            {
                workbook.Sheets[0].Rows[validLocation[0] + index].Cells[validLocation[1]].Enable = true;

                if (keyLanguage.Equals("ES"))
                {
                    workbook.Sheets[0].Rows[validLocation[0] + index].Cells[validLocation[1]].Value = "Si";
                    workbook.Sheets[0].Rows[validLocation[0] + index].Cells[validLocation[1]].Validation = new Validation
                    {
                        DataType = "list",
                        ShowButton = true,
                        ComparerType = "list",
                        From = "\"Si,No\"",
                        AllowNulls = false,
                        Type = "reject"
                    };
                }
                else
                {
                    workbook.Sheets[0].Rows[validLocation[0] + index].Cells[validLocation[1]].Value = "Yes";
                    workbook.Sheets[0].Rows[validLocation[0] + index].Cells[validLocation[1]].Validation = new Validation
                    {
                        DataType = "list",
                        ShowButton = true,
                        ComparerType = "list",
                        From = "\"Yes,No\"",
                        AllowNulls = false,
                        Type = "reject"
                    };
                }
            }
        }
        #endregion
    }
}
