namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public class FpbService : IFpbService
    {
        #region Error message
        private readonly ICorrectionFactorService _correcctionFactor;
        private readonly INozzleInformationService _nozzleInformationService;
        private const string messageErrorNumeric = "Debe Insertar un valor válido igual o mayor a 0.";
        #endregion
        public FpbService(ICorrectionFactorService correcctionFactor, INozzleInformationService nozzleInformationService)
        {
            _correcctionFactor = correcctionFactor;
            _nozzleInformationService = nozzleInformationService;
        }

        public void PrepareTemplate_FPB(SettingsToDisplayFPBReportsDTO reportsDTO, ref Workbook workbook, string ClavePrueba, string tangtDelta)
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

                workbook.Sheets[0].Rows[9].Cells[8].Enable = true;
                workbook.Sheets[0].Rows[9].Cells[8].Format = "MM/dd/yyyy";
                workbook.Sheets[0].Rows[9].Cells[8].Value = DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

                workbook.Sheets[0].Rows[9].Cells[8].Validation = new Validation()
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

                workbook.Sheets[0].Rows[10].Cells[3].Enable = true;
                workbook.Sheets[0].Rows[10].Cells[3].Format = "###,####0.000";
                workbook.Sheets[0].Rows[10].Cells[4].Enable = true;
                workbook.Sheets[0].Rows[10].Cells[4].Format = "###,####0.000";
                workbook.Sheets[0].Rows[10].Cells[3].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "999999.9",
                    AllowNulls = false,
                    MessageTemplate = "La tensión de prueba en kV debe ser mayor a cero considerando 6 enteros con 3 decimales",
                    Type = "reject",
                    TitleTemplate = "Error"

                };

                workbook.Sheets[0].Rows[10].Cells[8].Enable = true;
                workbook.Sheets[0].Rows[10].Cells[8].Format = "#,##0.0";
                workbook.Sheets[0].Rows[10].Cells[8].Validation = new Validation()
                {
                    DataType = "number",
                    ComparerType = "greaterThan",
                    From = "0",
                    To = "999.9",
                    AllowNulls = false,
                    MessageTemplate = "La temperatura en °C debe ser mayor a cero considerando 3 enteros con 1 decimal",
                    Type = "reject",
                    TitleTemplate = "Error"

                };
                if (ClavePrueba.ToUpper().Equals("AYD"))
                {
                    int[] posit = GetRowColOfWorbook("G14");
                    if (tangtDelta is "Con")
                    {
                        workbook.Sheets[0].Rows[posit[0]].Cells[posit[1]].Value = "20.0 °C";
                    }
                    else
                    {
                        workbook.Sheets[0].Rows[posit[0]].Cells[posit[1]].Formula = "=CONCATENATE(I11,J11)";
                    }

                    posit = GetRowColOfWorbook("H13");
                    if (tangtDelta is "Sin")
                    {
                        workbook.Sheets[0].Rows[posit[0]].Cells[posit[1]].Formula = "=G13";
                    }
                    else
                    {
                        workbook.Sheets[0].Rows[posit[0]].Cells[posit[1]].Value = "% Tan d";
                    }

                    int[] posit2 = GetRowColOfWorbook("G35");
                    if (tangtDelta is "Con")
                    {
                        workbook.Sheets[0].Rows[posit2[0]].Cells[posit2[1]].Value = "20.0 °C";
                    }
                    else
                    {
                        workbook.Sheets[0].Rows[posit2[0]].Cells[posit2[1]].Formula = "=CONCATENATE(I32,J11)";
                    }

                    posit = GetRowColOfWorbook("H34");
                    if (tangtDelta is "Sin")
                    {
                        workbook.Sheets[0].Rows[posit[0]].Cells[posit[1]].Formula = "=G34";
                    }
                    else
                    {
                        workbook.Sheets[0].Rows[posit[0]].Cells[posit[1]].Value = "% Tan d";
                    }
                }
                else
                {
                    int[] posit = GetRowColOfWorbook("G14");
                    if (tangtDelta is "Con")
                    {
                        workbook.Sheets[0].Rows[posit[0]].Cells[posit[1]].Value = "20.0 °C";
                    }
                    else
                    {
                        workbook.Sheets[0].Rows[posit[0]].Cells[posit[1]].Formula = "=CONCATENATE(I11,J11)";
                    }

                    posit = GetRowColOfWorbook("H13");
                    if (tangtDelta is "Sin")
                    {
                        workbook.Sheets[0].Rows[posit[0]].Cells[posit[1]].Formula = "=G13";
                    }
                    else
                    {
                        workbook.Sheets[0].Rows[posit[0]].Cells[posit[1]].Value = "% Tan d";
                    }
                }
              
                BorderStyle boder = new()
                {
                    Color = "Black",
                    Size = 1
                };

                int primeraTablaIndex = 14;
                List<RecordNozzleInformationDTO> lista = reportsDTO.NozzlesByDesignDtos.NozzleInformation.OrderBy(x => x.Orden).ToList();
                for (int i = 0; i < lista.Count; i++)
                {
                    workbook.Sheets[0].Rows[primeraTablaIndex].Cells[1].Value = reportsDTO.NozzlesByDesignDtos.NozzleInformation[i].Posicion;
                    workbook.Sheets[0].Rows[primeraTablaIndex].Cells[2].Value = reportsDTO.NozzlesByDesignDtos.NozzleInformation[i].NoSerieBoq;
                    workbook.Sheets[0].Rows[primeraTablaIndex].Cells[3].Enable = true;
                    workbook.Sheets[0].Rows[primeraTablaIndex].Cells[4].Enable = true;
                    workbook.Sheets[0].Rows[primeraTablaIndex].Cells[5].Enable = true;
                    workbook.Sheets[0].Rows[primeraTablaIndex].Cells[8].Enable = true;

                    for (int y = 1; y <= 8; y++)
                    {
                        workbook.Sheets[0].Rows[primeraTablaIndex].Cells[y].BorderTop = boder;
                        workbook.Sheets[0].Rows[primeraTablaIndex].Cells[y].BorderRight = boder;
                        workbook.Sheets[0].Rows[primeraTablaIndex].Cells[y].BorderBottom = boder;
                        workbook.Sheets[0].Rows[primeraTablaIndex].Cells[y].BorderLeft = boder;

                    }

                    for (int p = 3; p <= 5; p++)
                    {

                        if (p == 3)
                        {
                            workbook.Sheets[0].Rows[primeraTablaIndex].Cells[p].Validation = new Validation()
                            {
                                DataType = "custom",
                                ComparerType = "lessThan",
                                From = "AND(LEN(D" + (primeraTablaIndex + 1) + ") <=2)",
                                AllowNulls = false,
                                MessageTemplate = "La columna T no puede excederse de 2 caracteres",
                                Type = "reject",
                                TitleTemplate = "Error"
                            };
                        }
                        else
                        {

                            workbook.Sheets[0].Rows[primeraTablaIndex].Cells[p].Format = "#,##0.0##";
                            workbook.Sheets[0].Rows[primeraTablaIndex].Cells[p].Validation = new Validation()
                            {
                                DataType = "number",
                                ComparerType = "greaterThan",
                                From = "0",
                                To = "999.9",
                                AllowNulls = false,
                                MessageTemplate = $"{messageErrorNumeric}",
                                Type = "reject",
                                TitleTemplate = "Error"
                            };
                        }
                    }

                    workbook.Sheets[0].Rows[primeraTablaIndex].Cells[8].Format = "#,##0.0##";
                    workbook.Sheets[0].Rows[primeraTablaIndex].Cells[8].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "greaterThan",
                        From = "0",
                        To = "999.9",
                        AllowNulls = false,
                        MessageTemplate = $"{messageErrorNumeric}",
                        Type = "reject",
                        TitleTemplate = "Error"
                    };

                    primeraTablaIndex++;
                }

                if (ClavePrueba == "AYD")
                {

                    int segundaTablaIndex = 35;
                    for (int i = 0; i < lista.Count; i++)
                    {
                        workbook.Sheets[0].Rows[segundaTablaIndex].Cells[1].Value = reportsDTO.NozzlesByDesignDtos.NozzleInformation[i].Posicion;
                        workbook.Sheets[0].Rows[segundaTablaIndex].Cells[2].Value = reportsDTO.NozzlesByDesignDtos.NozzleInformation[i].NoSerieBoq;
                        workbook.Sheets[0].Rows[segundaTablaIndex].Cells[3].Enable = true;
                        workbook.Sheets[0].Rows[segundaTablaIndex].Cells[4].Enable = true;
                        workbook.Sheets[0].Rows[segundaTablaIndex].Cells[5].Enable = true;
                        workbook.Sheets[0].Rows[segundaTablaIndex].Cells[8].Enable = true;

                        for (int y = 1; y <= 8; y++)
                        {
                            workbook.Sheets[0].Rows[segundaTablaIndex].Cells[y].BorderTop = boder;
                            workbook.Sheets[0].Rows[segundaTablaIndex].Cells[y].BorderRight = boder;
                            workbook.Sheets[0].Rows[segundaTablaIndex].Cells[y].BorderBottom = boder;
                            workbook.Sheets[0].Rows[segundaTablaIndex].Cells[y].BorderLeft = boder;
                        }

                        for (int p = 3; p <= 5; p++)
                        {
                            if (p == 3)
                            {
                                workbook.Sheets[0].Rows[segundaTablaIndex].Cells[p].Validation = new Validation()
                                {
                                    DataType = "custom",
                                    ComparerType = "lessThan",
                                    From = "AND(LEN(D" + (segundaTablaIndex + 1) + ") <=2)",
                                    AllowNulls = false,
                                    MessageTemplate = $"La columna T no puede excederse de 2 caracteres",
                                    Type = "reject",
                                    TitleTemplate = "Error"
                                };
                            }
                            else
                            {
                                workbook.Sheets[0].Rows[segundaTablaIndex].Cells[p].Format = "#,##0.0##";
                                workbook.Sheets[0].Rows[segundaTablaIndex].Cells[p].Validation = new Validation()
                                {
                                    DataType = "number",
                                    ComparerType = "greaterThan",
                                    From = "0",
                                    To = "999.9",
                                    AllowNulls = false,
                                    MessageTemplate = $"{messageErrorNumeric}",
                                    Type = "reject",
                                    TitleTemplate = "Error"
                                };
                            }
                        }

                        workbook.Sheets[0].Rows[segundaTablaIndex].Cells[8].Format = "#,##0.0##";
                        workbook.Sheets[0].Rows[segundaTablaIndex].Cells[8].Validation = new Validation()
                        {
                            DataType = "number",
                            ComparerType = "greaterThan",
                            From = "0",
                            To = "999.9",
                            AllowNulls = false,
                            MessageTemplate = $"{messageErrorNumeric}",
                            Type = "reject",
                            TitleTemplate = "Error"
                        };

                        segundaTablaIndex++;
                    }

                    workbook.Sheets[0].Rows[30].Cells[8].Enable = true;
                    workbook.Sheets[0].Rows[30].Cells[8].Format = "MM/dd/yyyy";
                    workbook.Sheets[0].Rows[30].Cells[8].Value = DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    workbook.Sheets[0].Rows[30].Cells[8].Validation = new Validation()
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

                    workbook.Sheets[0].Rows[31].Cells[3].Enable = true;
                    workbook.Sheets[0].Rows[31].Cells[3].Format = "#,##0.000";
                    workbook.Sheets[0].Rows[31].Cells[4].Enable = true;
                    workbook.Sheets[0].Rows[31].Cells[4].Format = "#,##0.000";
                    workbook.Sheets[0].Rows[31].Cells[3].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "greaterThan",
                        From = "0",
                        To = "999999.999",
                        AllowNulls = false,
                        MessageTemplate = "La tensión de prueba en kV debe ser mayor a cero considerando 6 enteros con 3 decimales",
                        Type = "reject",
                        TitleTemplate = "Error"

                    };

                    workbook.Sheets[0].Rows[31].Cells[8].Enable = true;
                    workbook.Sheets[0].Rows[31].Cells[8].Format = "#,##0.0##";
                    workbook.Sheets[0].Rows[31].Cells[8].Validation = new Validation()
                    {
                        DataType = "number",
                        ComparerType = "greaterThan",
                        From = "0",
                        To = "999999.999",
                        AllowNulls = false,
                        MessageTemplate = "La temperatura en °C debe ser mayor a cero considerando 3 enteros con 1 decimal",
                        Type = "reject",
                        TitleTemplate = "Error"

                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Prepare_FPB_Test(Workbook workbook, ref List<FPBTestsDTO> _fpbTestDTOs, SettingsToDisplayFPBReportsDTO fpbReport, string clavePrueba)
        {
            try
            {
                SPL.Domain.ApiResponse<List<CorrectionFactorsXMarksXTypesDTO>> getCorrectionFactors = Task.Run(() => _correcctionFactor.GetCorrectionFactorsXMarksXTypes()).Result;

                int indexPrimeraTabla = 13;
                int indexSegundaTabla = 33;

                if (clavePrueba == "AYD")
                {
                    _fpbTestDTOs[0].TempFP = decimal.Parse(workbook.Sheets[0].Rows[12].Cells[6].Value.ToString().Split('°')[0]);
                    _fpbTestDTOs[1].TempFP = decimal.Parse(workbook.Sheets[0].Rows[32].Cells[6].Value.ToString().Split('°')[0]);

                    string tensionPrueba1 = workbook.Sheets[0].Rows[9].Cells[3].Value.ToString();
                    string tipoTensionPrueba1 = workbook.Sheets[0].Rows[9].Cells[5].Value.ToString();
                    string gradosCentigrados1 = workbook.Sheets[0].Rows[9].Cells[8].Value.ToString();
                    string tipoGradosCentigrados1 = workbook.Sheets[0].Rows[9].Cells[9].Value.ToString();

                    _fpbTestDTOs[0].Tension = decimal.Parse(tensionPrueba1);
                    _fpbTestDTOs[0].UmTension = tipoTensionPrueba1;
                    _fpbTestDTOs[0].UmTemperature = tipoGradosCentigrados1;
                    _fpbTestDTOs[0].Temperature = decimal.Parse(gradosCentigrados1);

                    string tensionPrueba2 = workbook.Sheets[0].Rows[29].Cells[3].Value.ToString();
                    string tipoTensionPrueba2 = workbook.Sheets[0].Rows[29].Cells[5].Value.ToString();
                    string gradosCentigrados2 = workbook.Sheets[0].Rows[29].Cells[8].Value.ToString();
                    string tipoGradosCentigrados2 = workbook.Sheets[0].Rows[29].Cells[9].Value.ToString();

                    _fpbTestDTOs[1].Tension = decimal.Parse(tensionPrueba2);
                    _fpbTestDTOs[1].UmTension = tipoTensionPrueba2;
                    _fpbTestDTOs[1].UmTemperature = tipoGradosCentigrados2;
                    _fpbTestDTOs[1].Temperature = decimal.Parse(gradosCentigrados2);

                    for (int i = 0; i < fpbReport.NozzlesByDesignDtos.NozzleInformation.OrderBy(x => x.Posicion).ToList().Count; i++)
                    {
                        decimal temp1 = _fpbTestDTOs[0].Temperature;
                        decimal temp2 = _fpbTestDTOs[1].Temperature;

                        _fpbTestDTOs[0].FPBTestsDetails.Add(new FPBTestsDetailsDTO
                        {
                            Capacitance = decimal.Parse(workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[8].Value.ToString()),
                            Id = workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[1].Value.ToString(),
                            NroSerie = workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[2].Value.ToString(),
                            T = workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[3].Value.ToString(),
                            Current = decimal.Parse(workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[4].Value.ToString()),
                            Power = decimal.Parse(workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[5].Value.ToString()),
                            CorrectionFactorSpecificationsTemperature = getCorrectionFactors.Structure.Where(
                                x => x.Temperatura == Math.Round(temp1, 0, MidpointRounding.AwayFromZero) &&
                                x.IdMarca == fpbReport.NozzlesByDesignDtos.NozzleInformation[i].IdMarca &&
                                x.IdTipo == fpbReport.NozzlesByDesignDtos.NozzleInformation[i].IdTipo
                             ).FirstOrDefault(),
                            CorrectionFactorSpecifications20Grados = getCorrectionFactors.Structure.Where(
                                x => x.Temperatura == Math.Round(20m, 0, MidpointRounding.AwayFromZero) &&
                                x.IdMarca == fpbReport.NozzlesByDesignDtos.NozzleInformation[i].IdMarca &&
                                x.IdTipo == fpbReport.NozzlesByDesignDtos.NozzleInformation[i].IdTipo
                             ).FirstOrDefault(),
                            NozzlesByDesign = fpbReport.NozzlesByDesignDtos.NozzleInformation.Where(x => x.Posicion == workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[1].Value.ToString() &&
                            x.NoSerieBoq == workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[2].Value.ToString()).FirstOrDefault()

                        }); ;

                        var r = getCorrectionFactors.Structure.Where(
                                x => x.Temperatura == Math.Round(temp1, 0, MidpointRounding.AwayFromZero) &&
                                x.IdMarca == fpbReport.NozzlesByDesignDtos.NozzleInformation[i].IdMarca &&
                                x.IdTipo == fpbReport.NozzlesByDesignDtos.NozzleInformation[i].IdTipo
                             ).FirstOrDefault();

                        var r2 = getCorrectionFactors.Structure.Where(
                                x => x.Temperatura == Math.Round(temp2, 0, MidpointRounding.AwayFromZero) &&
                                x.IdMarca == fpbReport.NozzlesByDesignDtos.NozzleInformation[i].IdMarca &&
                                x.IdTipo == fpbReport.NozzlesByDesignDtos.NozzleInformation[i].IdTipo
                             ).FirstOrDefault();


                        _fpbTestDTOs[1].FPBTestsDetails.Add(new FPBTestsDetailsDTO
                        {
                            Capacitance = decimal.Parse(workbook.Sheets[0].Rows[indexSegundaTabla].Cells[8].Value.ToString()),
                            Id = workbook.Sheets[0].Rows[indexSegundaTabla].Cells[1].Value.ToString(),
                            NroSerie = workbook.Sheets[0].Rows[indexSegundaTabla].Cells[2].Value.ToString(),
                            T = workbook.Sheets[0].Rows[indexSegundaTabla].Cells[3].Value.ToString(),
                            Current = decimal.Parse(workbook.Sheets[0].Rows[indexSegundaTabla].Cells[4].Value.ToString()),
                            Power = decimal.Parse(workbook.Sheets[0].Rows[indexSegundaTabla].Cells[5].Value.ToString()),
                            CorrectionFactorSpecificationsTemperature = getCorrectionFactors.Structure.Where(
                                x => x.Temperatura == Math.Round(temp2, 0, MidpointRounding.AwayFromZero) &&
                                x.IdMarca == fpbReport.NozzlesByDesignDtos.NozzleInformation[i].IdMarca &&
                                x.IdTipo == fpbReport.NozzlesByDesignDtos.NozzleInformation[i].IdTipo
                             ).FirstOrDefault(),
                            CorrectionFactorSpecifications20Grados = getCorrectionFactors.Structure.Where(
                                x => x.Temperatura == Math.Round(20m, 0, MidpointRounding.AwayFromZero) &&
                                x.IdMarca == fpbReport.NozzlesByDesignDtos.NozzleInformation[i].IdMarca &&
                                x.IdTipo == fpbReport.NozzlesByDesignDtos.NozzleInformation[i].IdTipo
                             ).FirstOrDefault(),
                            NozzlesByDesign = fpbReport.NozzlesByDesignDtos.NozzleInformation.Where(x => x.Posicion == workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[1].Value.ToString() &&
                           x.NoSerieBoq == workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[2].Value.ToString()).FirstOrDefault()

                        });

                        indexPrimeraTabla++;
                        indexSegundaTabla++;
                    }
                }
                else
                {
                    _fpbTestDTOs[0].TempFP = decimal.Parse(workbook.Sheets[0].Rows[12].Cells[6].Value.ToString().Split(' ')[0]);
                    string tensionPrueba1 = workbook.Sheets[0].Rows[9].Cells[3].Value.ToString();
                    string tipoTensionPrueba1 = workbook.Sheets[0].Rows[9].Cells[5].Value.ToString();
                    string gradosCentigrados1 = workbook.Sheets[0].Rows[9].Cells[8].Value.ToString();
                    string tipoGradosCentigrados1 = workbook.Sheets[0].Rows[9].Cells[9].Value.ToString();

                    _fpbTestDTOs[0].Tension = decimal.Parse(tensionPrueba1);
                    _fpbTestDTOs[0].UmTension = tipoTensionPrueba1;
                    _fpbTestDTOs[0].UmTemperature = tipoGradosCentigrados1;
                    _fpbTestDTOs[0].Temperature = decimal.Parse(gradosCentigrados1);

                    for (int i = 0; i < fpbReport.NozzlesByDesignDtos.NozzleInformation.OrderBy(x => x.Posicion).ToList().Count; i++)
                    {
                        decimal temp1 = _fpbTestDTOs[0].Temperature;

                        _fpbTestDTOs[0].FPBTestsDetails.Add(new FPBTestsDetailsDTO
                        {
                            Capacitance = decimal.Parse(workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[8].Value.ToString()),
                            Id = workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[1].Value.ToString(),
                            NroSerie = workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[2].Value.ToString(),
                            T = workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[3].Value.ToString(),
                            Current = decimal.Parse(workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[4].Value.ToString()),
                            Power = decimal.Parse(workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[5].Value.ToString()),
                            CorrectionFactorSpecificationsTemperature = getCorrectionFactors.Structure.Where(
                                x => x.Temperatura == Math.Round(temp1, 0, MidpointRounding.AwayFromZero) &&
                                x.IdMarca == fpbReport.NozzlesByDesignDtos.NozzleInformation[i].IdMarca &&
                                x.IdTipo == fpbReport.NozzlesByDesignDtos.NozzleInformation[i].IdTipo
                             ).FirstOrDefault(),
                            CorrectionFactorSpecifications20Grados = getCorrectionFactors.Structure.Where(
                                x => x.Temperatura == Math.Round(20m, 0, MidpointRounding.AwayFromZero) &&
                                x.IdMarca == fpbReport.NozzlesByDesignDtos.NozzleInformation[i].IdMarca &&
                                x.IdTipo == fpbReport.NozzlesByDesignDtos.NozzleInformation[i].IdTipo
                             ).FirstOrDefault(),
                            NozzlesByDesign = fpbReport.NozzlesByDesignDtos.NozzleInformation.Where(x => x.Posicion == workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[1].Value.ToString() &&
                            x.NoSerieBoq == workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[2].Value.ToString()).FirstOrDefault()

                        }); ;

                        indexPrimeraTabla++;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public void PrepareIndexOfFPB(ResultFPBTestsDTO resultFPBTestsDTO, SettingsToDisplayFPBReportsDTO reportsDTO, string idioma, ref Workbook workbook, bool resultReport, string ClavePrueba, string Tandelta)
        {

            int indexPrimeraTabla = 13;
            int indexSegundaTabla = 33;

            for (int i = 0; i < reportsDTO.NozzlesByDesignDtos.NozzleInformation.Count; i++)
            {

                workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[6].Value = resultFPBTestsDTO.FPBTests[0].FPBTestsDetails[i].PercentageA;
                workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[7].Value = resultFPBTestsDTO.FPBTests[0].FPBTestsDetails[i].PercentageB;

                if (ClavePrueba == "AYD")
                {
                    workbook.Sheets[0].Rows[indexSegundaTabla].Cells[6].Value = resultFPBTestsDTO.FPBTests[1].FPBTestsDetails[i].PercentageA;
                    workbook.Sheets[0].Rows[indexSegundaTabla].Cells[7].Value = resultFPBTestsDTO.FPBTests[1].FPBTestsDetails[i].PercentageB;
                }

                indexPrimeraTabla++;
                indexSegundaTabla++;
            }

            workbook.Sheets[0].Rows[49].Cells[8].Value = resultReport ?
                reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorAceptado{idioma}")).Formato :
                reportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals($"ValorRechazado{idioma}")).Formato;
        }

        public void CloneWorkbook(Workbook origin, SettingsToDisplayFPCReportsDTO reportsDTO, ref Workbook official, out List<DateTime> dates)
        {
            dates = new List<DateTime>();
            IEnumerable<ConfigurationReportsDTO> starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Corriente"));
            int[] _positionWB;

            // Copiando Todas las columnas
            foreach (ConfigurationReportsDTO column in starts)
            {
                _positionWB = GetRowColOfWorbook(column.Celda);
                for (int i = 0; i < 5; i++)
                {
                    CopyColumn(origin, _positionWB, ref official);
                    _positionWB[1]++;
                }
            }

            // Copiando fecha
            starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha"));
            foreach (ConfigurationReportsDTO column in starts)
            {
                _positionWB = GetRowColOfWorbook(column.Celda);
                official.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value;
                string fecha = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value?.ToString();

                if (DateTime.TryParseExact(fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime objDT))
                {
                    dates.Add(objDT);
                }
                else
                {
                    double value = double.Parse(fecha);
                    DateTime date = DateTime.FromOADate(value);
                    dates.Add(date);
                }
            }

            //Copiando el resto
            starts = reportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TempAceiteInf") || c.Dato.Equals("TempAceiteSup") || c.Dato.Equals("TempFP") || c.Dato.Equals("TempTanD") || c.Dato.Equals("TitColumna9") || c.Dato.Equals("TensionPrueba") || c.Dato.Equals("UMTempInf") || c.Dato.Equals("UMTempSup") || c.Dato.Equals("UMTension") || c.Dato.Equals("Resultado"));

            foreach (ConfigurationReportsDTO column in starts)
            {
                _positionWB = GetRowColOfWorbook(column.Celda);
                official.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = origin.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value;
            }
        }

        public bool Verify_FPB_Columns(Workbook workbook, SettingsToDisplayFPBReportsDTO result, string ClavePrueba)
        {
            int indexPrimeraTabla = 13;
            int indexSegundaTabla = 33;
            for (int i = 0; i < result.NozzlesByDesignDtos.NozzleInformation.Count; i++)
            {
                object t = workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[3]?.Value;
                object current = workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[4]?.Value;
                object power = workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[5]?.Value;
                object cap = workbook.Sheets[0].Rows[indexPrimeraTabla].Cells[8]?.Value;

                if (ClavePrueba == "AYD")
                {
                    object t2 = workbook.Sheets[0].Rows[indexSegundaTabla].Cells[3]?.Value;
                    object current2 = workbook.Sheets[0].Rows[indexSegundaTabla].Cells[4]?.Value;
                    object power2 = workbook.Sheets[0].Rows[indexSegundaTabla].Cells[5]?.Value;
                    object cap2 = workbook.Sheets[0].Rows[indexSegundaTabla].Cells[8]?.Value;

                    if (t2 == null || current2 == null || power2 == null || cap2 == null)
                    {
                        return false;
                    }
                }

                if (t == null || current == null || power == null || cap == null)
                {
                    return false;
                }

                indexSegundaTabla++;
                indexPrimeraTabla++;
            }

            object tensionPrueba1 = workbook.Sheets[0].Rows[9].Cells[3]?.Value;
            object gradosCentigrados1 = workbook.Sheets[0].Rows[9].Cells[8]?.Value;

            object tensionPrueba2 = workbook.Sheets[0].Rows[29].Cells[3]?.Value;
            object gradosCentigrados2 = workbook.Sheets[0].Rows[29].Cells[8]?.Value;

            if (tensionPrueba1 == null || gradosCentigrados1 == null)
            {
                return false;
            }

            if (ClavePrueba == "AYD")
            {
                if (tensionPrueba2 == null || gradosCentigrados2 == null)
                {
                    return false;
                }
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

            position[0] = System.Convert.ToInt32(col);

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
