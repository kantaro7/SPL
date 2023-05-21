namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.NRA;

    using Telerik.Web.Spreadsheet;

    public class NraService : INraService
    {
        #region Error message
        private readonly ICorrectionFactorService _correcctionFactor;
        private readonly INozzleInformationService _nozzleInformationService;
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion
        public NraService(ICorrectionFactorService correcctionFactor, INozzleInformationService nozzleInformationService)
        {
            _correcctionFactor = correcctionFactor;
            _nozzleInformationService = nozzleInformationService;
        }
        public void PrepareTemplateNRA(SettingsToDisplayNRAReportsDTO reportsDTO, ref string tituloAlimentacion, ref List<MatrixThreeDTO> matrizUnion, 
            ref bool activarSegundasValidaciones, ref Workbook workbook, string ClavePrueba, int cantidadColumnas,
            int cantidadValidaDeMediciones, string altura, bool esDataExistente, string lenguaje, string alimentacion
            , string cantidadAlimentacion ,string tipoEnfriamiento, ref string medidaCorriente)
        {
            try
            {
                if (string.IsNullOrEmpty(altura))
                {
                    altura = "1/3";
                }
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
                    MessageTemplate = $"Fecha es requerida y debe estar dentro del rango 1/1/1900 - {DateTime.Now.ToString("MM/dd/yyyy")}",
                    ComparerType = "between",
                    From = "DATEVALUE(\"1/1/1900\")",
                    To = $"DATEVALUE(\"{DateTime.Now.ToString("MM/dd/yyyy")}\")",
                    Type = "reject",
                    TitleTemplate = "Error",
                    ShowButton = true
                };

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TipoEnfriamiento")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = tipoEnfriamiento;

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAlimentacion")).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] - 1].Value = cantidadAlimentacion;



                if (ClavePrueba.ToUpper() == "OCT")
                {

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cliente") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.Client;

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NoSerie") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.HeadboardReport.NoSerie;

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Capacidad") && c.Seccion == 2).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = string.IsNullOrEmpty(reportsDTO.HeadboardReport.Capacity) ? string.Empty : $"{reportsDTO.HeadboardReport.Capacity} MVA";

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha") && c.Seccion == 2).Celda);
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

                if (lenguaje == "EN")
                {
                    if (alimentacion == "Tensión")
                    {
                        tituloAlimentacion = "Test Voltage:";

                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAlimentacion")).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = cantidadAlimentacion;

                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMAlimentacion")).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "kV";
                        medidaCorriente = "kV";

                        if (activarSegundasValidaciones)
                        {
                            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAlimentacion") && c.Seccion == 2).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = cantidadAlimentacion;

                            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMAlimentacion") && c.Seccion == 2).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "kV";
                            medidaCorriente = "kV";
                        }
                    }
                    else if (alimentacion == "Corriente")
                    {
                        tituloAlimentacion = "Current:";

                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAlimentacion")).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = cantidadAlimentacion;

                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMAlimentacion")).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "Amps";
                        medidaCorriente = "Amps";
                    }
                    else if (alimentacion == "Pérdidas")
                    {
                        tituloAlimentacion = "Losses:";

                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAlimentacion")).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = cantidadAlimentacion;

                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMAlimentacion")).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "kW";
                        medidaCorriente = "kW";
                    }
                }
                else
                {
                    if (alimentacion == "Tensión")
                    {
                        tituloAlimentacion = "Tensión:";

                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAlimentacion")).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = cantidadAlimentacion;

                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMAlimentacion")).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "kV";
                        medidaCorriente = "kV";

                        if (activarSegundasValidaciones)
                        {
                            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAlimentacion") && c.Seccion == 2).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = cantidadAlimentacion;

                            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMAlimentacion") && c.Seccion == 2).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "kV";
                        }
                    }
                    else if (alimentacion == "Corriente")
                    {
                        tituloAlimentacion = "Corriente:";

                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAlimentacion")).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = cantidadAlimentacion;

                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMAlimentacion")).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "Amps";
                        medidaCorriente = "Amps";
                    }
                    else if (alimentacion == "Pérdidas")
                    {
                        tituloAlimentacion = "Pérdidas:";

                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAlimentacion")).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = cantidadAlimentacion;

                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMAlimentacion")).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "kW";
                        medidaCorriente = "kW";
                    }
                }

                if (ClavePrueba == "RUI")
                {
                    if (esDataExistente)
                    {

                        #region RUIDO
                        int maximaMediciones = cantidadColumnas == 10 ? 60 : 70;
                        int breakSesiones = cantidadColumnas == 10 ? 30 : 35;
                        int i = 0;
                        int seccion = 1;
                        bool flag = false;
                        int seccion1 = cantidadColumnas == 4 ? 1 : 3;
                        int seccion2 = cantidadColumnas == 4 ? 2 : 4;

                        if (altura is "1/3" or "2/3")
                        {
                            IEnumerable<MatrixThree1323HDTO> lista = reportsDTO.MatrixHeight13.Take(cantidadValidaDeMediciones);
                            foreach (MatrixThree1323HDTO item in lista)
                            {
                                if (seccion <= breakSesiones)
                                {
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos13") && c.Seccion == seccion1).Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Position.ToString();

                                    Pintar(_positionWB, i, ref workbook);

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13") && c.Seccion == seccion1).Celda); ;
                                    Pintar(_positionWB, i, ref workbook);

                                    if (esDataExistente)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Dba.ToString("F1");
                                    }

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion1).Celda);
                                    Pintar(_positionWB, i, ref workbook);

                                    if (esDataExistente)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.CorDba.ToString("F1");

                                    }
                                }

                                if (seccion > breakSesiones)
                                {
                                    if (!flag)
                                    {
                                        i = 0;
                                        flag = true;
                                    }

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos13") && c.Seccion == seccion2).Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Position.ToString();

                                    Pintar(_positionWB, i, ref workbook);
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13") && c.Seccion == seccion2).Celda);

                                    if (esDataExistente)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Dba.ToString("F1");
                                    }

                                    Pintar(_positionWB, i, ref workbook);

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion2).Celda);

                                    if (esDataExistente)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.CorDba.ToString("F1");
                                    }

                                    Pintar(_positionWB, i, ref workbook);

                                }

                                if (seccion == cantidadValidaDeMediciones)
                                {
                                    break;
                                }

                                if (seccion > maximaMediciones)
                                {
                                    break;
                                }

                                i++;
                                seccion++;

                            }

                            seccion = 1;
                            i = 0;
                            flag = false;
                            IEnumerable<MatrixThree1323HDTO> lista2 = reportsDTO.MatrixHeight23.Take(cantidadValidaDeMediciones);

                            foreach (MatrixThree1323HDTO item in lista2)
                            {
                                if (seccion <= breakSesiones)
                                {
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos23") && c.Seccion == seccion1).Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Position.ToString();

                                    Pintar(_positionWB, i, ref workbook);
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23") && c.Seccion == seccion1).Celda);

                                    if (esDataExistente)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Dba.ToString("F1");
                                    }
                                    Pintar(_positionWB, i, ref workbook);
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23Corr") && c.Seccion == seccion1).Celda);

                                    if (esDataExistente)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.CorDba.ToString("F1");
                                    }

                                    Pintar(_positionWB, i, ref workbook);

                                }

                                if (seccion > breakSesiones)
                                {
                                    if (!flag)
                                    {
                                        i = 0;
                                        flag = true;
                                    }

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos23") && c.Seccion == seccion2).Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Position.ToString();

                                    Pintar(_positionWB, i, ref workbook);
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23") && c.Seccion == seccion2).Celda);

                                    if (esDataExistente)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Dba.ToString("F1");
                                    }
                                    Pintar(_positionWB, i, ref workbook);

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23Corr") && c.Seccion == seccion2).Celda);

                                    if (esDataExistente)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.CorDba.ToString("F1");
                                    }

                                    Pintar(_positionWB, i, ref workbook);

                                }

                                if (seccion == cantidadValidaDeMediciones)
                                {
                                    break;
                                }

                                if (seccion > maximaMediciones)
                                {
                                    break;
                                }

                                i++;
                                seccion++;

                            }
                        }

                        if (altura == "1/2")
                        {
                            IEnumerable<MatrixThree1323HDTO> lista = reportsDTO.MatrixHeight12.Take(cantidadValidaDeMediciones);
                            foreach (MatrixThree1323HDTO item in lista)
                            {
                                if (seccion <= breakSesiones)
                                {
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos13") && c.Seccion == seccion1).Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Position.ToString();

                                    Pintar(_positionWB, i, ref workbook);

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13") && c.Seccion == seccion1).Celda); ;
                                    Pintar(_positionWB, i, ref workbook);

                                    if (esDataExistente)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Dba.ToString("F1");
                                    }

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion1).Celda);
                                    Pintar(_positionWB, i, ref workbook);

                                    if (esDataExistente)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.CorDba.ToString("F1");

                                    }
                                }

                                if (seccion > breakSesiones)
                                {
                                    if (!flag)
                                    {
                                        i = 0;
                                        flag = true;
                                    }

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos13") && c.Seccion == seccion2).Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Position.ToString();

                                    Pintar(_positionWB, i, ref workbook);
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13") && c.Seccion == seccion2).Celda);

                                    if (esDataExistente)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Dba.ToString("F1");
                                    }

                                    Pintar(_positionWB, i, ref workbook);

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion2).Celda);

                                    if (esDataExistente)
                                    {
                                        workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.CorDba.ToString("F1");
                                    }

                                    Pintar(_positionWB, i, ref workbook);

                                }

                                if (seccion == cantidadValidaDeMediciones)
                                {
                                    break;
                                }

                                if (seccion > maximaMediciones)
                                {
                                    break;
                                }

                                i++;
                                seccion++;

                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region RUIDO
                        int maximaMediciones = cantidadColumnas == 10 ? 60 : 70;
                        int breakSesiones = cantidadColumnas == 10 ? 30 : 35;
                        int i = 0;
                        int seccion = 1;
                        bool flag = false;
                        int seccion1 = cantidadColumnas == 4 ? 1 : 3;
                        int seccion2 = cantidadColumnas == 4 ? 2 : 4;

                        if (altura is "1/3" or "2/3")
                        {
                            IEnumerable<MatrixThree1323HDTO> lista = reportsDTO.MatrixHeight13.Take(cantidadValidaDeMediciones);
                            foreach (MatrixThree1323HDTO item in lista)
                            {
                                if (seccion <= breakSesiones)
                                {
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos13") && c.Seccion == seccion1).Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Position.ToString();

                                    Pintar(_positionWB, i, ref workbook);

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13") && c.Seccion == seccion1).Celda); ;
                                    Pintar(_positionWB, i, ref workbook);

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion1).Celda);
                                    Pintar(_positionWB, i, ref workbook);
                                }

                                if (seccion > breakSesiones)
                                {
                                    if (!flag)
                                    {
                                        i = 0;
                                        flag = true;
                                    }

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos13") && c.Seccion == seccion2).Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Position.ToString();

                                    Pintar(_positionWB, i, ref workbook);
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13") && c.Seccion == seccion2).Celda);

                                    Pintar(_positionWB, i, ref workbook);

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion2).Celda);

                                    Pintar(_positionWB, i, ref workbook);

                                }

                                if (seccion == cantidadValidaDeMediciones)
                                {
                                    break;
                                }

                                if (seccion > maximaMediciones)
                                {
                                    break;
                                }

                                i++;
                                seccion++;

                            }

                            seccion = 1;
                            i = 0;
                            flag = false;
                            IEnumerable<MatrixThree1323HDTO> lista2 = reportsDTO.MatrixHeight23.Take(cantidadValidaDeMediciones);

                            foreach (MatrixThree1323HDTO item in lista2)
                            {
                                if (seccion <= breakSesiones)
                                {
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos23") && c.Seccion == seccion1).Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Position.ToString();

                                    Pintar(_positionWB, i, ref workbook);
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23") && c.Seccion == seccion1).Celda);

                                    Pintar(_positionWB, i, ref workbook);
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23Corr") && c.Seccion == seccion1).Celda);

                                    Pintar(_positionWB, i, ref workbook);

                                }

                                if (seccion > breakSesiones)
                                {
                                    if (!flag)
                                    {
                                        i = 0;
                                        flag = true;
                                    }

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos23") && c.Seccion == seccion2).Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Position.ToString();

                                    Pintar(_positionWB, i, ref workbook);
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23") && c.Seccion == seccion2).Celda);

                                    Pintar(_positionWB, i, ref workbook);

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23Corr") && c.Seccion == seccion2).Celda);
                                    Pintar(_positionWB, i, ref workbook);

                                }

                                if (seccion == cantidadValidaDeMediciones)
                                {
                                    break;
                                }

                                if (seccion > maximaMediciones)
                                {
                                    break;
                                }

                                i++;
                                seccion++;

                            }
                        }

                        if (altura == "1/2")
                        {
                            IEnumerable<MatrixThree1323HDTO> lista = reportsDTO.MatrixHeight12.Take(cantidadValidaDeMediciones);
                            foreach (MatrixThree1323HDTO item in lista)
                            {
                                if (seccion <= breakSesiones)
                                {
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos13") && c.Seccion == seccion1).Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Position.ToString();

                                    Pintar(_positionWB, i, ref workbook);

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13") && c.Seccion == seccion1).Celda); ;
                                    Pintar(_positionWB, i, ref workbook);

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion1).Celda);
                                    Pintar(_positionWB, i, ref workbook);

                                }

                                if (seccion > breakSesiones)
                                {
                                    if (!flag)
                                    {
                                        i = 0;
                                        flag = true;
                                    }

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos13") && c.Seccion == seccion2).Celda);
                                    workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Position.ToString();

                                    Pintar(_positionWB, i, ref workbook);
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13") && c.Seccion == seccion2).Celda);

                                    Pintar(_positionWB, i, ref workbook);

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion2).Celda);

                                    Pintar(_positionWB, i, ref workbook);

                                }

                                if (seccion == cantidadValidaDeMediciones)
                                {
                                    break;
                                }

                                if (seccion > maximaMediciones)
                                {
                                    break;
                                }

                                i++;
                                seccion++;

                            }
                        }
                        #endregion
                    }
                }

                if (ClavePrueba == "OCT")
                {

                    int rowbreak = cantidadColumnas == 4 ? 36 : 24;
                    int row = cantidadColumnas == 4 ? 24 : 36;
                    bool flag = false;
                    int i = 0;
                    int[] inicial1 = cantidadColumnas == 4 ? GetRowColOfWorbook("B24") : GetRowColOfWorbook("B36");
                    int[] inicial2 = GetRowColOfWorbook("B83");
                    string expre;

                    List<MatrixThreeDTO> union = new();
                    if (altura is "1/3" or "2/3")
                    {
                    

                        List<MatrixThreeDTO> matrix13 = reportsDTO.matrixThreeCoolingType.Where(x => x.Height == "1/3").Take(cantidadValidaDeMediciones).ToList();
                        List<MatrixThreeDTO> matrix23 = reportsDTO.matrixThreeCoolingType.Where(x => x.Height == "2/3").Take(cantidadValidaDeMediciones).ToList();
                        matrix13.AddRange(matrix23);
                        union = matrix13;

                    }
                    else
                    {
                        List<MatrixThreeDTO> matrix12 = reportsDTO.matrixThreeCoolingType.Where(x => x.Height == "1/2").Take(cantidadValidaDeMediciones).ToList();
                        union = matrix12;
                    }
                    matrizUnion = union;

                    activarSegundasValidaciones = union.Count() > rowbreak;

                    if (activarSegundasValidaciones)
                    {
                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TipoEnfriamiento") && c.Seccion == 2).Celda);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = tipoEnfriamiento;

                        if (lenguaje == "EN")
                        {
                            if (alimentacion == "Tensión")
                            {
                                tituloAlimentacion = "Test Voltage:";

                                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAlimentacion") && c.Seccion == 2).Celda);
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = cantidadAlimentacion;

                                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMAlimentacion") && c.Seccion == 2).Celda);
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "kV";

                            }
                            else if (alimentacion == "Corriente")
                            {
                                tituloAlimentacion = "Current:";

                                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAlimentacion") && c.Seccion == 2).Celda);
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = cantidadAlimentacion;

                                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMAlimentacion") && c.Seccion == 2).Celda);
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "Amps";
                            }
                            else if (alimentacion == "Pérdidas")
                            {
                                tituloAlimentacion = "Losses:";

                                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAlimentacion") && c.Seccion == 2).Celda);
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = cantidadAlimentacion;

                                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMAlimentacion") && c.Seccion == 2).Celda);
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "kW";
                            }
                        }
                        else
                        {
                            if (alimentacion == "Tensión")
                            {
                                tituloAlimentacion = "Tensión:";

                                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAlimentacion") && c.Seccion == 2).Celda);
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = cantidadAlimentacion;

                                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMAlimentacion") && c.Seccion == 2).Celda);
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "kV";
                            }
                            else if (alimentacion == "Corriente")
                            {
                                tituloAlimentacion = "Corriente:";

                                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAlimentacion") && c.Seccion == 2).Celda);
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = cantidadAlimentacion;

                                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMAlimentacion") && c.Seccion == 2).Celda);
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "Amps";
                            }
                            else if (alimentacion == "Pérdidas")
                            {
                                tituloAlimentacion = "Pérdidas:";

                                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAlimentacion") && c.Seccion == 2).Celda);
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = cantidadAlimentacion;

                                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("UMAlimentacion") && c.Seccion == 2).Celda);
                                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = "kW";
                            }
                        }
                    }

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA") && c.Seccion == 1).Celda);
                    List<MatrixThreeDTO> unionAntesyDespues = new();
                    unionAntesyDespues.AddRange(reportsDTO.matrixThreeAnt.Where(x=>x.Height == altura).Take(cantidadColumnas).ToList());
                    unionAntesyDespues.AddRange(reportsDTO.matrixThreeDes.Where(x => x.Height == altura).Take(cantidadColumnas).ToList());
                    int col = 13;
                    //**LLENADO DE LAS MATREICES ANTES Y DESPUES*****************************/
                    foreach (MatrixThreeDTO item in unionAntesyDespues)
                    {
                        if (esDataExistente)
                        {
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = item.Dba.ToString("F1");
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1].Value = item.Decibels315.ToString("F1");
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2].Value = item.Decibels63.ToString("F1");
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3].Value = item.Decibels125.ToString("F1");
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 4].Value = item.Decibels250.ToString("F1");
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 5].Value = item.Decibels500.ToString("F1");
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 6].Value = item.Decibels1000.ToString("F1");
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 7].Value = item.Decibels2000.ToString("F1");
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 8].Value = item.Decibels4000.ToString("F1");
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 9].Value = item.Decibels8000.ToString("F1");
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 10].Value = item.Decibels10000.ToString("F1");
                        }
                        else
                        {
                            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA") && c.Seccion == 1).Celda);
                            AgregarValidacionDecibel(_positionWB, i, col, ref workbook);
                            col++;
                        }

                        i++;
                    }
                    /*************************************************************************/

                    i = 0;
                    string celdaAt = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT") && c.Seccion == 1).Celda;
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT") && c.Seccion == 1).Celda);
                    expre = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT") && c.Seccion == 1).Celda + ",@^[a-zA-Z0-9]*$@),LEN({celda})<=5)".Replace("@", "\"").Replace("{celda}", celdaAt);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "custom",
                        ComparerType = "custom",
                        From = expre,
                        AllowNulls = false,
                        ShowButton = true,
                        MessageTemplate = "La posición en AT solo puede contener letras y/o números y no puede excederse de 5 caracteres.",
                        Type = "reject",
                        TitleTemplate = "Error"

                    };

                    string celdaBt = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT") && c.Seccion == 1).Celda;
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT") && c.Seccion == 1).Celda);
                    expre = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT") && c.Seccion == 1).Celda + ",@^[a-zA-Z0-9]*$@),LEN({celda})<=5)".Replace("@", "\"").Replace("{celda}", celdaBt); ;
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                    {
                        DataType = "custom",
                        ComparerType = "custom",
                        From = expre,
                        AllowNulls = false,
                        ShowButton = true,
                        MessageTemplate = "La posición en BT solo puede contener letras y/o números y no puede excederse de 5 caracteres.",
                        Type = "reject",
                        TitleTemplate = "Error"

                    };

                    string celdaTer = string.Empty;
                    if (reportsDTO.Posiciones.Terciario.Count() > 0)
                    {
                        celdaTer = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosTer") && c.Seccion == 1).Celda;
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosTer") && c.Seccion == 1).Celda);
                        expre = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosTer") && c.Seccion == 1).Celda + ",@^[a-zA-Z0-9]*$@),LEN({celda})<=5)".Replace("@", "\"").Replace("{celda}", celdaTer);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            ComparerType = "custom",
                            From = expre,
                            AllowNulls = false,
                            ShowButton = true,
                            MessageTemplate = "La posición en Ter solo puede contener letras y/o números y no puede excederse de 5 caracteres.",
                            Type = "reject",
                            TitleTemplate = "Error"

                        };
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                    }

                    if (union.Count() > rowbreak)
                    {
                        celdaAt = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT") && c.Seccion == 2).Celda;
                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT") && c.Seccion == 2).Celda);
                        expre = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT") && c.Seccion == 2).Celda + ",@^[a-zA-Z0-9]*$@),LEN({celda})<=5)".Replace("@", "\"").Replace("{celda}", celdaAt);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            ComparerType = "custom",
                            From = expre,
                            AllowNulls = false,
                            ShowButton = true,
                            MessageTemplate = "La posición en AT solo puede contener letras y/o números y no puede excederse de 5 caracteres.",
                            Type = "reject",
                            TitleTemplate = "Error"

                        };
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;

                        celdaBt = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT") && c.Seccion == 2).Celda;
                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT") && c.Seccion == 2).Celda);
                        expre = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT") && c.Seccion == 2).Celda + ",@^[a-zA-Z0-9]*$@),LEN({celda})<=5)".Replace("@", "\"").Replace("{celda}", celdaBt);
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                        {
                            DataType = "custom",
                            ComparerType = "custom",
                            From = expre,
                            AllowNulls = false,
                            ShowButton = true,
                            MessageTemplate = "La posición en BT solo puede contener letras y/o números y no puede excederse de 5 caracteres.",
                            Type = "reject",
                            TitleTemplate = "Error"

                        };
                        workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;

                        if (reportsDTO.Posiciones.Terciario.Count() > 0)
                        {
                            celdaTer = reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosTer") && c.Seccion == 2).Celda;
                            _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosTer") && c.Seccion == 2).Celda);
                            expre = "AND(REGEXP_MATCH(" + reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosTer") && c.Seccion == 2).Celda + ",@^[a-zA-Z0-9]*$@),LEN({celda})<=5)".Replace("@", "\"").Replace("{celda}", celdaTer);
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = new Validation()
                            {
                                DataType = "custom",
                                ComparerType = "custom",
                                From = expre,
                                AllowNulls = false,
                                ShowButton = true,
                                MessageTemplate = "La posición en Ter solo puede contener letras y/o números y no puede excederse de 5 caracteres.",
                                Type = "reject",
                                TitleTemplate = "Error"

                            };
                            workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Enable = true;
                        }
                    }

                    /********************************LLENADO DE LAS SECCIONES***************************************************************************/
                    int columnas = cantidadColumnas == 4 ? 24 : 36;
                    foreach (MatrixThreeDTO item in union)
                    {

                        for (int j = 0; j < 13; j++)
                        {
                            BorderStyle boder = new()
                            {
                                Color = "Black",
                                Size = 1
                            };

                            workbook.Sheets[0].Rows[inicial1[0] + i].Cells[inicial1[1] + j].BorderBottom = boder;
                            workbook.Sheets[0].Rows[inicial1[0] + i].Cells[inicial1[1] + j].BorderLeft = boder;
                            workbook.Sheets[0].Rows[inicial1[0] + i].Cells[inicial1[1] + j].BorderRight = boder;
                            workbook.Sheets[0].Rows[inicial1[0] + i].Cells[inicial1[1] + j].BorderTop = boder;
                        }

                        workbook.Sheets[0].Rows[inicial1[0] + i].Cells[inicial1[1]].Value = item.Position;
                        workbook.Sheets[0].Rows[inicial1[0] + i].Cells[inicial1[1] + 1].Value = item.Height;
                        if (esDataExistente)
                        {
                            workbook.Sheets[0].Rows[inicial1[0] + i].Cells[inicial1[1] + 2].Value = item.Dba.ToString("F1");
                            workbook.Sheets[0].Rows[inicial1[0] + i].Cells[inicial1[1] + 3].Value = item.Decibels315.ToString("F1");
                            workbook.Sheets[0].Rows[inicial1[0] + i].Cells[inicial1[1] + 4].Value = item.Decibels63.ToString("F1");
                            workbook.Sheets[0].Rows[inicial1[0] + i].Cells[inicial1[1] + 5].Value = item.Decibels125.ToString("F1");
                            workbook.Sheets[0].Rows[inicial1[0] + i].Cells[inicial1[1] + 6].Value = item.Decibels250.ToString("F1");
                            workbook.Sheets[0].Rows[inicial1[0] + i].Cells[inicial1[1] + 7].Value = item.Decibels500.ToString("F1");
                            workbook.Sheets[0].Rows[inicial1[0] + i].Cells[inicial1[1] + 8].Value = item.Decibels1000.ToString("F1");
                            workbook.Sheets[0].Rows[inicial1[0] + i].Cells[inicial1[1] + 9].Value = item.Decibels2000.ToString("F1");
                            workbook.Sheets[0].Rows[inicial1[0] + i].Cells[inicial1[1] + 10].Value = item.Decibels4000.ToString("F1");
                            workbook.Sheets[0].Rows[inicial1[0] + i].Cells[inicial1[1] + 11].Value = item.Decibels8000.ToString("F1");
                            workbook.Sheets[0].Rows[inicial1[0] + i].Cells[inicial1[1] + 12].Value = item.Decibels10000.ToString("F1");
                        }
                        else
                        {
                            AgregarValidacionDecibel2(inicial1, i, columnas, ref workbook);
                        }

                        i++;
                        row++;
                        columnas++;

                        if (i >= rowbreak && !flag)
                        {
                            i = 0;
                            flag = true;
                            inicial1 = inicial2;
                            row = 83;
                            columnas = 83;
                        }
                    }
                    /*************************************************************************************************************************************/

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Garantia")).Celda);
                    workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = reportsDTO.Warranty;

                    if (!esDataExistente)
                    {
                        _positionWB = cantidadColumnas == 4 ? GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AmbienteProm")).Celda) : GetRowColOfWorbook("D33");
                        AgregarValidacionDecibel(_positionWB, 0, cantidadColumnas == 4 ? 21 : 33, ref workbook);
                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Amb+Trans")).Celda);
                        AgregarValidacionDecibel3(_positionWB, 0, 126, ref workbook);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string ValidateTemplate(SettingsToDisplayNRAReportsDTO reportsDTO, Workbook workbook, bool esCargarData, int CantidadMediciones, string pruebas, int columnas, string altura, string norma, string enfriamiento,
            List<InformationLaboratoriesDTO> info, string laboratorio, List<MatrixThreeDTO> matrizAnt, List<MatrixThreeDTO> matrizDes, List<MatrixThree1323HDTO> matriz12, List<MatrixThree1323HDTO> matriz13, List<MatrixThree1323HDTO> matriz23, PositionsDTO posiciones, bool tieneSegundaSeccion, List<MatrixThreeDTO> UnionMatrices, ref NRATestsGeneralDTO salida)
        {
            try
            {
                #region Posiciones y campos de arriba
                int[] _positionWB;
                string valor = string.Empty;
                List<string> vaciado13DbaCorr = new();
                List<string> vaciado13Dba = new();
                List<string> vaciado23DbaCorr = new();
                List<string> vaciado23Dba = new();
                List<string> vaciado12Dba = new();
                List<string> vaciado12DbaCorr = new();

                List<string> vaciadoAntes = new();
                List<string> vaciadoDespues = new();

                NRATestsDTO data = new();
                NRATestsDetailsRuiDTO matrizInfoRuido = new();
                data.Diferencia = reportsDTO.Diferencia;
                data.FactorCoreccion = reportsDTO.FactorCoreccion;

                if (string.IsNullOrEmpty(altura))
                {
                    altura = "1/3";
                }

                InformationLaboratoriesDTO labSeleccionado = info.Where(x => x.Laboratorio == laboratorio).FirstOrDefault();
                if (labSeleccionado != null)
                {
                    data.SV = labSeleccionado.Sv;
                    data.Alfa = labSeleccionado.Alfa;
                }
                else
                {
                    return "No se han encontrado valores SV/Alfa para el laboratorio seleccionado";

                }

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT")).Celda);
                valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                if (string.IsNullOrEmpty(valor))
                {
                    return "Debe llenar la posicion AT";
                }
                else
                {
                    if (posiciones.AltaTension.Contains(valor))
                    {
                        data.HV = valor;

                    }
                    else
                    {
                        return "Debe seleccionar una posicion valida para AT";
                    }
                }

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT")).Celda);
                valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                if (string.IsNullOrEmpty(valor))
                {
                    return "Debe llenar la posicion BT";
                }
                else
                {
                    if (posiciones.BajaTension.Contains(valor))
                    {
                        data.LV = valor;

                    }
                    else
                    {
                        return "Debe seleccionar una posicion valida para BT";
                    }
                }

                if (reportsDTO.Posiciones.TerNom is not null and not "")
                {
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosTer")).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();
                    if (string.IsNullOrEmpty(valor))
                    {
                        return "Debe llenar la posicion Ter";
                    }
                    else
                    {
                        if (posiciones.Terciario.Contains(valor))
                        {
                            data.TV = valor;

                        }
                        else
                        {
                            return "Debe seleccionar una posicion valida para Ter";
                        }
                    }
                }

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AlturaH")).Celda);
                valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                if (string.IsNullOrEmpty(valor))
                {
                    return "La altura no puede estar vacia";
                }
                else
                {
                    data.Height = decimal.Parse(valor);
                }

                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerimetroPM")).Celda);
                valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                if (string.IsNullOrEmpty(valor))
                {
                    return "El perimetro no puede estar vacio";
                }
                else
                {
                    data.Length = decimal.Parse(valor);
                }

                if (pruebas == "OCT" && tieneSegundaSeccion)
                {
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AlturaH") && c.Seccion == 2).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(valor))
                    {
                        return "La altura de la segunda seccion no puede estar vacia";
                    }
                    else
                    {
                        data.Height = decimal.Parse(valor);
                    }

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerimetroPM") && c.Seccion == 2).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(valor))
                    {
                        return "El perimetro de la segunda seccions no puede estar vacio";
                    }
                    else
                    {
                        data.Length = decimal.Parse(valor);
                    }

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT") && c.Seccion == 2).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(valor))
                    {
                        return "Debe llenar la posicion AT de la segunda seccion";
                    }
                    else
                    {
                        if (posiciones.AltaTension.Contains(valor))
                        {
                            data.HV = valor;

                        }
                        else
                        {
                            return "Debe seleccionar una posicion valida para AT de la segunda seccion";
                        }
                    }

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT") && c.Seccion == 2).Celda);
                    valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(valor))
                    {
                        return "Debe llenar la posicion BT de la segunda seccion";
                    }
                    else
                    {
                        if (posiciones.BajaTension.Contains(valor))
                        {
                            data.LV = valor;

                        }
                        else
                        {
                            return "Debe seleccionar una posicion valida para BT de la segunda seccion";
                        }
                    }

                    if (reportsDTO.Posiciones.TerNom is not null and not "")
                    {
                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosTer") && c.Seccion == 2).Celda);
                        valor = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();
                        if (string.IsNullOrEmpty(valor))
                        {
                            return "Debe llenar la posicion Ter de la segunda seccion";
                        }
                        else
                        {
                            if (posiciones.Terciario.Contains(valor))
                            {
                                data.TV = valor;

                            }
                            else
                            {
                                return "Debe seleccionar una posicion valida para Ter de la segunda seccion";
                            }
                        }
                    }

                    string AT1 = string.Empty;
                    string AT2 = string.Empty;
                    string BT1 = string.Empty;
                    string BT2 = string.Empty;
                    string Ter1 = string.Empty;
                    string Ter2 = string.Empty;

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT")).Celda);
                    AT1 = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT") && c.Seccion == 2).Celda);
                    AT2 = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (AT1.Trim() != AT2.Trim())
                    {
                        return "Las posiciones AT para ambas secciones deben ser iguales";
                    }

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT")).Celda);
                    BT1 = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT") && c.Seccion == 2).Celda);
                    BT2 = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                    if (BT1.Trim() != BT2.Trim())
                    {
                        return "Las posiciones BT para ambas secciones deben ser iguales";
                    }

                    if (reportsDTO.Posiciones.Terciario.Count() > 0)
                    {
                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosTer")).Celda);
                        Ter1 = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosTer") && c.Seccion == 2).Celda);
                        Ter2 = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                        if (Ter1.Trim() != Ter2.Trim())
                        {
                            return "Las posiciones Ter para ambas secciones deben ser iguales";
                        }
                    }
                }

#endregion 

                int seccion = 1;
                int i = 0;
                bool flag = false;
                int valorSeccionComparar = columnas == 10 ? 30 : 35;
                int breakComparasion = columnas == 10 ? 60 : 70;
                int seccion1 = columnas == 4 ? 1 : 3;
                int seccion2 = columnas == 4 ? 2 : 4;
                if (pruebas == "RUI")
                {
                    string str = null;
                    decimal convert;

                    for (int u = 0; u < columnas; u++)
                    {
                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBAAnt") && c.Seccion == 1).Celda);
                        vaciadoAntes.Add(workbook.Sheets[0].Rows[_positionWB[0] + u].Cells[_positionWB[1]]?.Value?.ToString());

                        _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBADes") && c.Seccion == 1).Celda);
                        vaciadoDespues.Add(workbook.Sheets[0].Rows[_positionWB[0] + u].Cells[_positionWB[1]]?.Value?.ToString());
                    }

                    if (vaciadoAntes.Any(x => string.IsNullOrEmpty(x)) && vaciadoAntes.Any(x => string.IsNullOrEmpty(x)) &&
                        vaciadoDespues.Any(x => string.IsNullOrEmpty(x)) && vaciadoDespues.Any(x => string.IsNullOrEmpty(x)))
                    {
                        return "Debe llenar todas las posiciones para la matriz antes y/o despues";
                    }

                    if (esCargarData)
                    {
                        #region RUIDO CON INFORMACION
                        if (altura is "1/3" or "2/3")
                        {

                            for (int o = 0; o < CantidadMediciones; o++)
                            {
                                if (seccion <= valorSeccionComparar)
                                {

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13") && c.Seccion == seccion1).Celda);
                                    vaciado13Dba.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion1).Celda);
                                    vaciado13DbaCorr.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23") && c.Seccion == seccion1).Celda);
                                    vaciado23Dba.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23Corr") && c.Seccion == seccion1).Celda);
                                    vaciado23DbaCorr.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());
                                }

                                if (seccion > valorSeccionComparar)
                                {
                                    if (!flag)
                                    {
                                        i = 0;
                                        flag = true;
                                    }

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13") && c.Seccion == seccion2).Celda);
                                    vaciado13Dba.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion2).Celda);
                                    vaciado13DbaCorr.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23") && c.Seccion == seccion2).Celda);
                                    vaciado23Dba.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23Corr") && c.Seccion == seccion2).Celda);
                                    vaciado23DbaCorr.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());

                                }

                                if (seccion == CantidadMediciones)
                                {
                                    break;
                                }

                                if (seccion > breakComparasion)
                                {
                                    break;
                                }

                                seccion++;
                                i++;
                            }

                            if (vaciado13Dba.Any(x => string.IsNullOrEmpty(x)) && vaciado13DbaCorr.Any(x => string.IsNullOrEmpty(x)) &&
                                vaciado23Dba.Any(x => string.IsNullOrEmpty(x)) && vaciado23DbaCorr.Any(x => string.IsNullOrEmpty(x)))
                            {
                                return "Debe llenar todos los campos de dba y dbaCor";
                            }
                            else
                            {
                                salida.KeyTest = pruebas;
                                salida.Rule = norma;
                                salida.CoolingType = enfriamiento;
                                salida.LoadInfo = esCargarData;
                                matrizInfoRuido.MatrixNRA13Tests = new List<MatrixNRATestsDTO>();
                                matrizInfoRuido.MatrixNRA23Tests = new List<MatrixNRATestsDTO>();
                                matrizInfoRuido.MatrixNRAAntTests = new List<MatrixNRATestsDTO>();
                                matrizInfoRuido.MatrixNRADesTests = new List<MatrixNRATestsDTO>();
                                List<string> abc = new() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" }; ;

                                foreach (var item in vaciado13Dba.Select((value, i) => new { value, i }))
                                {
                                    matrizInfoRuido.MatrixNRA13Tests.Add(new MatrixNRATestsDTO
                                    {
                                        Position = (item.i + 1).ToString(),
                                        Dba = decimal.Parse(item.value),
                                        DbaCor = decimal.Parse(vaciado13DbaCorr[item.i]),
                                        TypeInformation = matriz13[item.i].TypeInformation,
                                        Height = matriz13[item.i].Height,

                                    });
                                }

                                foreach (var item in vaciado23Dba.Select((value, i) => new { value, i }))
                                {
                                    matrizInfoRuido.MatrixNRA23Tests.Add(new MatrixNRATestsDTO
                                    {
                                        Position = (item.i + 1).ToString(),
                                        Dba = decimal.Parse(item.value),
                                        DbaCor = decimal.Parse(vaciado23DbaCorr[item.i]),
                                        TypeInformation = matriz23[item.i].TypeInformation,
                                        Height = matriz23[item.i].Height,

                                    });
                                }

                                foreach (var item in matrizAnt.Select((value, i) => new { value, i }).Take(columnas))
                                {
                                    matrizInfoRuido.MatrixNRAAntTests.Add(new MatrixNRATestsDTO
                                    {
                                        Position = abc[item.i],
                                        Dba = decimal.Parse(vaciadoAntes[item.i]),
                                        TypeInformation = item.value.TypeInformation,
                                        Height = item.value.Height

                                    });
                                }

                                foreach (var item in matrizDes.Select((value, i) => new { value, i }).Take(columnas))
                                {
                                    matrizInfoRuido.MatrixNRADesTests.Add(new MatrixNRATestsDTO
                                    {
                                        Position = abc[item.i],
                                        Dba = decimal.Parse(vaciadoDespues[item.i]),
                                        TypeInformation = item.value.TypeInformation,
                                        Height = item.value.Height

                                    });
                                }

                                matrizInfoRuido.MatrixNRADesTests.ForEach(x => x.Section = 1);
                                matrizInfoRuido.MatrixNRAAntTests.ForEach(x => x.Section = 1);
                                matrizInfoRuido.MatrixNRA13Tests.ForEach(x => x.Section = 2);
                                matrizInfoRuido.MatrixNRA23Tests.ForEach(x => x.Section = 2);

                        
                                /*
                                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AmbienteProm")).Celda);
                                str = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                                if (string.IsNullOrEmpty(str))
                                {
                                    return "El Ambiente promedio no puede estar vacio";
                                }


                                if(decimal.TryParse(str, out convert))
                                {
                                    matrizInfoRuido.AverageAmb = convert;
                                }


                                str = null;
                                convert = 0;

                                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Amb+Trans")).Celda);
                                str = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                                if (string.IsNullOrEmpty(str))
                                {
                                    return "El Amb+Trans no puede estar vacio";
                                }


                                if (decimal.TryParse(str, out convert))
                                {
                                    matrizInfoRuido.AmbTrans = convert;
                                }
                                */

                                data.NRATestsDetailsRuis = matrizInfoRuido;
                                salida.Data = data;
                                //data.
                            }
                        }

                        if (altura == "1/2")
                        {
                            for (int o = 0; o < CantidadMediciones; o++)
                            {
                                if (seccion <= valorSeccionComparar)
                                {

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13") && c.Seccion == seccion1).Celda);
                                    vaciado12Dba.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion1).Celda);
                                    vaciado12DbaCorr.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());

                                }

                                if (seccion > valorSeccionComparar)
                                {
                                    if (!flag)
                                    {
                                        i = 0;
                                        flag = true;
                                    }

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13") && c.Seccion == seccion2).Celda);
                                    vaciado12Dba.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());
                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion2).Celda);
                                    vaciado12DbaCorr.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());

                                }

                                if (seccion == CantidadMediciones)
                                {
                                    break;
                                }

                                if (seccion > breakComparasion)
                                {
                                    break;
                                }

                                seccion++;
                                i++;
                            }

                            if (vaciado12Dba.Any(x => string.IsNullOrEmpty(x)) && vaciado12DbaCorr.Any(x => string.IsNullOrEmpty(x)))
                            {
                                return "Debe llenar todos los campos de dba y dbaCor";
                            }
                            else
                            {
                                salida.KeyTest = pruebas;
                                salida.Rule = norma;
                                salida.CoolingType = enfriamiento;
                                salida.LoadInfo = esCargarData;
                                matrizInfoRuido.MatrixNRA13Tests = new List<MatrixNRATestsDTO>();
                                matrizInfoRuido.MatrixNRAAntTests = new List<MatrixNRATestsDTO>();
                                matrizInfoRuido.MatrixNRADesTests = new List<MatrixNRATestsDTO>();

                                foreach (var item in vaciado12Dba.Select((value, i) => new { value, i }))
                                {
                                    matrizInfoRuido.MatrixNRA13Tests.Add(new MatrixNRATestsDTO
                                    {
                                        Position = (item.i + 1).ToString(),
                                        Dba = decimal.Parse(item.value),
                                        DbaCor = decimal.Parse(vaciado12DbaCorr[item.i]),
                                        TypeInformation = matriz12[item.i].TypeInformation,
                                        Height = matriz12[item.i].Height,

                                    });
                                }

                                foreach (var item in matrizAnt.Select((value, i) => new { value, i }).Take(columnas))
                                {
                                    matrizInfoRuido.MatrixNRAAntTests.Add(new MatrixNRATestsDTO
                                    {
                                        Position = (item.i + 1).ToString(),
                                        Dba = decimal.Parse(vaciadoAntes[item.i]),
                                        TypeInformation = item.value.TypeInformation,
                                        Height = item.value.Height

                                    });
                                }

                                foreach (var item in matrizDes.Select((value, i) => new { value, i }).Take(columnas))
                                {
                                    matrizInfoRuido.MatrixNRADesTests.Add(new MatrixNRATestsDTO
                                    {
                                        Position = (item.i + 1).ToString(),
                                        Dba = decimal.Parse(vaciadoDespues[item.i]),
                                        TypeInformation = item.value.TypeInformation,
                                        Height = item.value.Height

                                    });
                                }

                                matrizInfoRuido.MatrixNRADesTests.ForEach(x => x.Section = 1);
                                matrizInfoRuido.MatrixNRAAntTests.ForEach(x => x.Section = 1);
                                matrizInfoRuido.MatrixNRA13Tests.ForEach(x => x.Section = 2);


                               /* _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AmbienteProm") ).Celda);
                                str = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                                if (string.IsNullOrEmpty(str))
                                {
                                    return "El Ambiente promedio no puede estar vacio";
                                }


                                if (decimal.TryParse(str, out convert))
                                {
                                    matrizInfoRuido.AverageAmb = convert;
                                }


                                str = null;
                                convert = 0;

                                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Amb+Trans") ).Celda);
                                str = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                                if (string.IsNullOrEmpty(str))
                                {
                                    return "El Amb+Trans no puede estar vacio";
                                }


                                if (decimal.TryParse(str, out convert))
                                {
                                    matrizInfoRuido.AmbTrans = convert;
                                }*/


                                data.NRATestsDetailsRuis = matrizInfoRuido;
                                salida.Data = data;
                                //data.
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        if (altura is "1/3" or "2/3")
                        {

                            for (int o = 0; o < CantidadMediciones; o++)
                            {
                                if (seccion <= valorSeccionComparar)
                                {

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13") && c.Seccion == seccion1).Celda);
                                    vaciado13Dba.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());

                                    /*ositionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion1).Celda);
                                    vaciado13DbaCorr.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());*/

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23") && c.Seccion == seccion1).Celda);
                                    vaciado23Dba.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());
                                   /* _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23Corr") && c.Seccion == seccion1).Celda);
                                    vaciado23DbaCorr.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());*/
                                }

                                if (seccion > valorSeccionComparar)
                                {
                                    if (!flag)
                                    {
                                        i = 0;
                                        flag = true;
                                    }

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13") && c.Seccion == seccion2).Celda);
                                    vaciado13Dba.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());
                                   /* _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion2).Celda);
                                    vaciado13DbaCorr.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());*/

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23") && c.Seccion == seccion2).Celda);
                                    vaciado23Dba.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());
                                  /*  _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23Corr") && c.Seccion == seccion2).Celda);
                                    vaciado23DbaCorr.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());*/

                                }

                                if (seccion == CantidadMediciones)
                                {
                                    break;
                                }

                                if (seccion > breakComparasion)
                                {
                                    break;
                                }

                                seccion++;
                                i++;
                            }

                            if (vaciado13Dba.Any(x => string.IsNullOrEmpty(x)) && vaciado13DbaCorr.Any(x => string.IsNullOrEmpty(x)) &&
                                vaciado23Dba.Any(x => string.IsNullOrEmpty(x)) && vaciado23DbaCorr.Any(x => string.IsNullOrEmpty(x)))
                            {
                                return "Debe llenar todos los campos de dba y dbaCor";
                            }
                            else
                            {
                                salida.KeyTest = pruebas;
                                salida.Rule = norma;
                                salida.CoolingType = enfriamiento;
                                salida.LoadInfo = esCargarData;
                                matrizInfoRuido.MatrixNRA13Tests = new List<MatrixNRATestsDTO>();
                                matrizInfoRuido.MatrixNRA23Tests = new List<MatrixNRATestsDTO>();
                                matrizInfoRuido.MatrixNRAAntTests = new List<MatrixNRATestsDTO>();
                                matrizInfoRuido.MatrixNRADesTests = new List<MatrixNRATestsDTO>();
                                List<string> abc = new() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

                                foreach (var item in vaciado13Dba.Select((value, i) => new { value, i }))
                                {
                                    matrizInfoRuido.MatrixNRA13Tests.Add(new MatrixNRATestsDTO
                                    {
                                        Position = (item.i + 1).ToString(),
                                        Dba = decimal.Parse(item.value),
                                        DbaCor =0,// decimal.Parse(vaciado13DbaCorr[item.i]),
                                        TypeInformation = enfriamiento,
                                        Height = "1/3",

                                    });
                                }

                                foreach (var item in vaciado23Dba.Select((value, i) => new { value, i }))
                                {
                                    matrizInfoRuido.MatrixNRA23Tests.Add(new MatrixNRATestsDTO
                                    {
                                        Position = (item.i + 1).ToString(),
                                        Dba = decimal.Parse(item.value),
                                        DbaCor = 0,//decimal.Parse(vaciado23DbaCorr[item.i]),
                                        TypeInformation = enfriamiento,
                                        Height = "2/3",

                                    });
                                }

                                foreach (var item in matrizAnt.Select((value, i) => new { value, i }).Take(columnas))
                                {
                                    matrizInfoRuido.MatrixNRAAntTests.Add(new MatrixNRATestsDTO
                                    {
                                        Position = abc[item.i],
                                        Dba = decimal.Parse(vaciadoAntes[item.i]),
                                        TypeInformation = "ANT",
                                        Height = altura

                                    });
                                }

                                foreach (var item in matrizDes.Select((value, i) => new { value, i }).Take(columnas))
                                {
                                    matrizInfoRuido.MatrixNRADesTests.Add(new MatrixNRATestsDTO
                                    {
                                        Position = abc[item.i],
                                        Dba = decimal.Parse(vaciadoDespues[item.i]),
                                        TypeInformation = "DES",
                                        Height = altura

                                    });
                                }

                                
                                matrizInfoRuido.MatrixNRADesTests.ForEach(x => x.Section = 1);
                                 matrizInfoRuido.MatrixNRAAntTests.ForEach(x => x.Section = 1);

                                matrizInfoRuido.MatrixNRA13Tests.ForEach(x => x.Section = 2);
                                matrizInfoRuido.MatrixNRA23Tests.ForEach(x => x.Section = 2);


                               /* _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AmbienteProm")).Celda);
                                str = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                                if (string.IsNullOrEmpty(str))
                                {
                                    return "El Ambiente promedio no puede estar vacio";
                                }


                                if (decimal.TryParse(str, out convert))
                                {
                                    matrizInfoRuido.AverageAmb = convert;
                                }


                                str = null;
                                convert = 0;

                                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Amb+Trans")).Celda);
                                str = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                                if (string.IsNullOrEmpty(str))
                                {
                                    return "El Amb+Trans no puede estar vacio";
                                }


                                if (decimal.TryParse(str, out convert))
                                {
                                    matrizInfoRuido.AmbTrans = convert;
                                }

                                */

                                data.NRATestsDetailsRuis = matrizInfoRuido;
                                salida.Data = data;
                                //data.
                            }
                        }

                        if (altura == "1/2")
                        {
                            for (int o = 0; o < CantidadMediciones; o++)
                            {
                                if (seccion <= valorSeccionComparar)
                                {

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13") && c.Seccion == seccion1).Celda);
                                    vaciado12Dba.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());

                                 /*   _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion1).Celda);
                                    vaciado12DbaCorr.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());*/

                                }

                                if (seccion > valorSeccionComparar)
                                {
                                    if (!flag)
                                    {
                                        i = 0;
                                        flag = true;
                                    }

                                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13") && c.Seccion == seccion2).Celda);
                                    vaciado12Dba.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());
                                  /*  _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion2).Celda);
                                    vaciado12DbaCorr.Add(workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());*/

                                }

                                if (seccion == CantidadMediciones)
                                {
                                    break;
                                }

                                if (seccion > breakComparasion)
                                {
                                    break;
                                }

                                seccion++;
                                i++;
                            }

                            if (vaciado12Dba.Any(x => string.IsNullOrEmpty(x)) && vaciado12DbaCorr.Any(x => string.IsNullOrEmpty(x)))
                            {
                                return "Debe llenar todos los campos de dba y dbaCor";
                            }
                            else
                            {
                                salida.KeyTest = pruebas;
                                salida.Rule = norma;
                                salida.CoolingType = enfriamiento;
                                salida.LoadInfo = esCargarData;
                                matrizInfoRuido.MatrixNRA13Tests = new List<MatrixNRATestsDTO>();
                                matrizInfoRuido.MatrixNRAAntTests = new List<MatrixNRATestsDTO>();
                                matrizInfoRuido.MatrixNRADesTests = new List<MatrixNRATestsDTO>();
                                List<string> abc = new() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

                                foreach (var item in vaciado12Dba.Select((value, i) => new { value, i }))
                                {
                                    matrizInfoRuido.MatrixNRA13Tests.Add(new MatrixNRATestsDTO
                                    {
                                        Position = (item.i + 1).ToString(),
                                        Dba = decimal.Parse(item.value),
                                        DbaCor = 0,//decimal.Parse(vaciado12DbaCorr[item.i]),
                                        TypeInformation = enfriamiento,
                                        Height ="1/2"

                                    });
                                }

                                foreach (var item in matrizAnt.Select((value, i) => new { value, i }).Take(columnas))
                                {
                                    matrizInfoRuido.MatrixNRAAntTests.Add(new MatrixNRATestsDTO
                                    {
                                        Position = abc[item.i],
                                        Dba = decimal.Parse(vaciadoAntes[item.i]),
                                        TypeInformation = "ANT",
                                        Height = altura

                                    });
                                }

                                foreach (var item in matrizDes.Select((value, i) => new { value, i }).Take(columnas))
                                {
                                    matrizInfoRuido.MatrixNRADesTests.Add(new MatrixNRATestsDTO
                                    {
                                        Position = abc[item.i],
                                        Dba = decimal.Parse(vaciadoDespues[item.i]),
                                        TypeInformation = "DES",
                                        Height = altura

                                    });
                                }

                                matrizInfoRuido.MatrixNRADesTests.ForEach(x => x.Section = 1);
                                matrizInfoRuido.MatrixNRAAntTests.ForEach(x => x.Section = 1);
                                matrizInfoRuido.MatrixNRA13Tests.ForEach(x => x.Section = 2);


                                /*_positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AmbienteProm")).Celda);
                                str = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                                if (string.IsNullOrEmpty(str))
                                {
                                    return "El Ambiente promedio no puede estar vacio";
                                }


                                if (decimal.TryParse(str, out convert))
                                {
                                    matrizInfoRuido.AverageAmb = convert;
                                }


                                str = null;
                                convert = 0;

                                _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Amb+Trans")).Celda);
                                str = workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                                if (string.IsNullOrEmpty(str))
                                {
                                    return "El Amb+Trans no puede estar vacio";
                                }


                                if (decimal.TryParse(str, out convert))
                                {
                                    matrizInfoRuido.AmbTrans = convert;
                                }
                                */

                                data.NRATestsDetailsRuis = matrizInfoRuido;
                                salida.Data = data;
                                //data.
                            }
                        }

                    }

                }

                if (pruebas == "OCT")
                {
                    NRATestsDetailsOctDTO objGeneral = new();

                    List<MatrixNRATestsDTO> MatrixNRAAntTests = new(); ;
                    List<MatrixNRATestsDTO> MatrixNRADesTests = new(); ;
                    List<MatrixNRATestsDTO> MatrixNRACoolingTypeTests = new();

                    MatrixNRATestsDTO MatrixNRAAmbProm = new();
                    MatrixNRATestsDTO MatrixNRAAmbTrans = new();

                    List<MatrixOneDTO> MatrizAntesDespuesPura = new();
                    List<MatrixTwoDTO> MatrizDeEnfriamientoPura = new();


                    var test11 = reportsDTO.MatrixOneDto.Where(x => x.Height == altura && x.TypeInformation == "ANT").Take(columnas).ToList();
                    var test22 = reportsDTO.MatrixOneDto.Where(x => x.Height == altura && x.TypeInformation == "DES").Take(columnas).ToList();
                    MatrizAntesDespuesPura.AddRange(test11);
                    MatrizAntesDespuesPura.AddRange(test22);
                    if (altura is "1/3" or "2/3" )
                    {
                        var test3 = reportsDTO.MatrixTwoDto.Where(x => x.Height == "1/3" && x.TypeInformation == enfriamiento).Take(CantidadMediciones).ToList();
                        MatrizDeEnfriamientoPura.AddRange(test3);

                        test3 = reportsDTO.MatrixTwoDto.Where(x => x.Height == "2/3" && x.TypeInformation == enfriamiento).Take(CantidadMediciones).ToList();
                        MatrizDeEnfriamientoPura.AddRange(test3);
                    }
                    else
                    {
                        var test12 = reportsDTO.MatrixTwoDto.Where(x => x.Height == "1/2" && x.TypeInformation == enfriamiento).Take(CantidadMediciones).ToList();
                        MatrizDeEnfriamientoPura.AddRange(test12);
                    }

                 


                    salida.KeyTest = pruebas;
                    salida.Rule = norma;
                    salida.CoolingType = enfriamiento;
                    salida.LoadInfo = esCargarData;

                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA")).Celda);
                    string result = "";
                    //*******************************obtiene las posiciones ambiente Antes****************************************************/
                    for (int ii = 0; ii < (columnas * 2) + 1; ii++)
                    {
                        if (ii < columnas)
                        {
                            result = ReturnObjetoAntDes(_positionWB, workbook, ref MatrixNRAAntTests, altura, "ANT", ii);
                            if (!string.IsNullOrEmpty(result))
                            {
                                return result;
                            }
                        }

                        if (ii >= columnas)
                        {
                            result = ReturnObjetoAntDes(_positionWB, workbook, ref MatrixNRADesTests, altura, "DES", ii);
                            if (!string.IsNullOrEmpty(result))
                            {
                                return result;
                            }
                        }
                    }
                    /**********************************************************************************************************************
                    //**********MATRIZ AMBIENTE PROMEDIO *******************************/
                    MatrixNRAAmbProm.Dba = MatrixNRADesTests.LastOrDefault().Dba;
                    MatrixNRAAmbProm.Section = 1;
                    MatrixNRAAmbProm.Decibels315 = MatrixNRADesTests.LastOrDefault().Decibels315;
                    MatrixNRAAmbProm.Decibels63 = MatrixNRADesTests.LastOrDefault().Decibels63;
                    MatrixNRAAmbProm.Decibels125 = MatrixNRADesTests.LastOrDefault().Decibels125;
                    MatrixNRAAmbProm.Decibels250 = MatrixNRADesTests.LastOrDefault().Decibels250;
                    MatrixNRAAmbProm.Decibels500 = MatrixNRADesTests.LastOrDefault().Decibels500;
                    MatrixNRAAmbProm.Decibels1000 = MatrixNRADesTests.LastOrDefault().Decibels1000;
                    MatrixNRAAmbProm.Decibels2000 = MatrixNRADesTests.LastOrDefault().Decibels2000;
                    MatrixNRAAmbProm.Decibels4000 = MatrixNRADesTests.LastOrDefault().Decibels4000;
                    MatrixNRAAmbProm.Decibels8000 = MatrixNRADesTests.LastOrDefault().Decibels8000;
                    MatrixNRAAmbProm.Decibels10000 = MatrixNRADesTests.LastOrDefault().Decibels10000;
                    MatrixNRAAmbProm.Height = altura;
                    MatrixNRAAmbProm.TypeInformation = enfriamiento;
                    /*****************************************************************************************/

                    /*******************************AMBIENTE TRANS *******************************************/
                    List<MatrixNRATestsDTO> ambTrans = new();
                    _positionWB = GetRowColOfWorbook(reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Amb+Trans") && c.Seccion == 2).Celda);
                    result = ReturnObjetoAntDes(_positionWB, workbook, ref ambTrans, altura, enfriamiento, 0);
                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }
                    MatrixNRAAmbTrans = ambTrans.FirstOrDefault();

                    /*********************************************************************************************/

                    int rowbreak = columnas == 10 ? 24 : 36;
                    i = 0;
                    flag = false;

                    _positionWB = columnas == 10 ? GetRowColOfWorbook("D36") : GetRowColOfWorbook("D24");
                    seccion = 2;
                    for (int u = 0; u < UnionMatrices.Count(); u++)
                    {
                        result = GetMatrizCoolingType(_positionWB, i, ref workbook, ref MatrixNRACoolingTypeTests, altura, enfriamiento, seccion);
                        i++;
                        if (i >= rowbreak && !flag)
                        {
                            i = 0;
                            flag = true;
                            _positionWB = GetRowColOfWorbook("D83");
                        }
                    }
                    MatrixNRADesTests.RemoveAt(MatrixNRADesTests.Count() - 1);
                    objGeneral.MatrixNRACoolingTypeTests = MatrixNRACoolingTypeTests;
                    objGeneral.MatrixNRAAntTests = MatrixNRAAntTests;
                    objGeneral.MatrixNRADesTests = MatrixNRADesTests;
                    objGeneral.MatrixNRAAmbProm = MatrixNRAAmbProm;
                    objGeneral.MatrixNRAAmbProm = MatrixNRAAmbProm;
                    objGeneral.MatrixNRAAmbTrans = MatrixNRAAmbTrans;
                    objGeneral.MatrizAntesDespuesPura = MatrizAntesDespuesPura;
                    objGeneral.MatrizDeEnfriamientoPura = MatrizDeEnfriamientoPura;

                    data.NRATestsDetailsOcts = objGeneral;
                    salida.Data = data;

                }

                return string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Private Methods

        private void Pintar(int[] _positionWB, int i, ref Workbook workbook)
        {

            BorderStyle boder = new()
            {
                Color = "Black",
                Size = 1
            };

            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderBottom = boder;
            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderLeft = boder;
            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderRight = boder;
            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].BorderTop = boder;

        }
        private void AgregarValidacionDecibel(int[] _positionWB, int i, int col, ref Workbook workbook)
        {
            string expre = "AND(REGEXP_MATCH({celda}, @^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,1})?%?$@))";
            List<string> abc = new() { "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N" }; ;
            foreach (var letra in abc.Select((value, i) => new { value, i }))
            {
                workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + letra.i].Validation = new Validation()
                {
                    DataType = "custom",
                    ComparerType = "custom",
                    From = expre.Replace("{celda}", letra.value + col).Replace("@", "\""),
                    AllowNulls = false,
                    ShowButton = true,
                    MessageTemplate = "Los decibeles y dba deben ser numericos mayores a 0 contemplando 3 enteros y un decimal",
                    Type = "reject",
                    TitleTemplate = "Error"

                };
            }
        }

        private void AgregarValidacionDecibel2(int[] _positionWB, int i, int col, ref Workbook workbook)
        {
            string expre = "AND(REGEXP_MATCH({celda}, @^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,1})?%?$@))";
            List<string> abc = new() { "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N" }; ;
            foreach (var letra in abc.Select((value, i) => new { value, i }))
            {
                workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2 + letra.i].Validation = new Validation()
                {
                    DataType = "custom",
                    ComparerType = "custom",
                    From = expre.Replace("{celda}", letra.value + col).Replace("@", "\""),
                    AllowNulls = false,
                    ShowButton = true,
                    MessageTemplate = "Los decibeles y dba deben ser numericos mayores a 0 contemplando 3 enteros y un decimal",
                    Type = "reject",
                    TitleTemplate = "Error"

                };
            }
        }

        private void AgregarValidacionDecibel3(int[] _positionWB, int i, int col, ref Workbook workbook)
        {
            string expre = "AND(REGEXP_MATCH({celda}, @^[0-9]\\\\d{0,2}(\\\\.\\\\d{1,1})?%?$@))";
            List<string> abc = new() { "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N" }; ;
            foreach (var letra in abc.Select((value, i) => new { value, i }))
            {
                workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + letra.i].Validation = new Validation()
                {
                    DataType = "custom",
                    ComparerType = "custom",
                    From = expre.Replace("{celda}", letra.value + col).Replace("@", "\""),
                    AllowNulls = false,
                    ShowButton = true,
                    MessageTemplate = "Los decibeles y dba deben ser numericos mayores a 0 contemplando 3 enteros y un decimal",
                    Type = "reject",
                    TitleTemplate = "Error"

                };
                workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + letra.i].Enable = true;
            }
        }

        private string ReturnObjetoAntDes(int[] _positionWB, Workbook workbook, ref List<MatrixNRATestsDTO> matrizOutput, string altura, string tipoInformacion, int row)
        {

            string valor = "";
            MatrixNRATestsDTO salida = new();
            List<string> valores = new();

            for (int i = 0; i < 11; i++)
            {
                valor = workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + i]?.Value?.ToString();
                valores.Add(valor);
            }

            if (valores.Any(x => string.IsNullOrEmpty(x)))
            {
                return "Debe llenar todas las posiciones asociadas a los decibeles/dba";
            }
            else
            {
                salida.Section = 1;
                salida.Position = workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] - 1]?.Value?.ToString();
                salida.Height = altura;
                salida.TypeInformation = tipoInformacion;
                salida.Dba = decimal.Parse(valores[0]);
                salida.Decibels315 = decimal.Parse(valores[1]);
                salida.Decibels63 = decimal.Parse(valores[2]);
                salida.Decibels125 = decimal.Parse(valores[3]);
                salida.Decibels250 = decimal.Parse(valores[4]);
                salida.Decibels500 = decimal.Parse(valores[5]);
                salida.Decibels1000 = decimal.Parse(valores[6]);
                salida.Decibels2000 = decimal.Parse(valores[7]);
                salida.Decibels4000 = decimal.Parse(valores[8]);
                salida.Decibels8000 = decimal.Parse(valores[9]);
                salida.Decibels10000 = decimal.Parse(valores[10]);

                matrizOutput.Add(salida);
            }

            return "";
        }

        private string GetMatrizCoolingType(int[] _positionWB, int row, ref Workbook workbook, ref List<MatrixNRATestsDTO> matrizOutput, string altura, string tipoInformacion, int seccion)
        {
            string valor = "";
            List<string> abc = new() { "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N" }; ;
            MatrixNRATestsDTO salida = new();
            List<string> valores = new();

            foreach (var letra in abc.Select((value, i) => new { value, i }))
            {
                valor = workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] + letra.i]?.Value?.ToString();
                valores.Add(valor);

            }

            if (valores.Any(x => string.IsNullOrEmpty(x)))
            {
                return "Debe llenar todas las posiciones asociadas a los decibeles/dba";
            }
            else
            {
                salida.Section = seccion;
                salida.Position = workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] - 2]?.Value?.ToString();
                salida.Height = workbook.Sheets[0].Rows[_positionWB[0] + row].Cells[_positionWB[1] - 1]?.Value?.ToString(); ;
                salida.TypeInformation = tipoInformacion;
                salida.Dba = decimal.Parse(valores[0]);
                salida.Decibels315 = decimal.Parse(valores[1]);
                salida.Decibels63 = decimal.Parse(valores[2]);
                salida.Decibels125 = decimal.Parse(valores[3]);
                salida.Decibels250 = decimal.Parse(valores[4]);
                salida.Decibels500 = decimal.Parse(valores[5]);
                salida.Decibels1000 = decimal.Parse(valores[6]);
                salida.Decibels2000 = decimal.Parse(valores[7]);
                salida.Decibels4000 = decimal.Parse(valores[8]);
                salida.Decibels8000 = decimal.Parse(valores[9]);
                salida.Decibels10000 = decimal.Parse(valores[10]);

                matrizOutput.Add(salida);
            }

            return "";
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
