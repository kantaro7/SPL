namespace SPL.Tests.Application.Handlers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;

    using SPL.Domain.SPL.Tests.PCI;
    using SPL.Tests.Application.Commands.Tests;

    public class PCITestsHandler : IRequestHandler<PCITestsCommand, ApiResponse<PCITestResponse>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public PCITestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods

        public async Task<ApiResponse<PCITestResponse>> Handle(PCITestsCommand request, CancellationToken cancellationToken)
        {
            PCITestResponse result = new();

            try
            {
                if (request.Ratings.Count == 0)
                {
                    return new ApiResponse<PCITestResponse>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Faltan datos para realizar el cálculo",
                        Structure = null
                    };
                }

                List<List<SecondaryPositionPCITestResult>> results = new();

                for (int i = 0; i < request.Ratings.Count; i++)
                {
                    PCIRating rating = request.Ratings[i];

                    List<SecondaryPositionPCITestResult> secondaryResults = new();

                    for (int r = 0; r < rating.SecondaryPositions.Count; r++)
                    {
                        double overvoltage = (double)rating.TemperatureElevation + 20d;
                        double elevationCorrectionFactor = 0;

                        if (request.WindingMaterial.ToUpper().Equals("COBRE"))
                        {
                            elevationCorrectionFactor = Math.Round((234.5d + overvoltage) / (234.5d + (double)rating.Temperature), 7);
                        }
                        else if (request.WindingMaterial.ToUpper().Equals("ALUMINIO"))
                        {
                            elevationCorrectionFactor = Math.Round((225d + overvoltage) / (225d + (double)rating.Temperature), 7);
                        }

                        PCISecondaryPosition detailsPCI = rating.SecondaryPositions[r];

                        SecondaryCalculate secondaryCalculate = new(request.Autotransformer, request.Monofasico, request.WindingMaterial, detailsPCI.TapPosition, detailsPCI.Position)
                        {
                            BaseRating = (double)rating.BaseRating,
                            Frequency = (double)rating.Frequency,
                            Temperature = (double)rating.Temperature,
                            CurrentIrms = (double)detailsPCI.CurrentIrms,
                            Voltage = (double)detailsPCI.VoltagekVrms,
                            Power = (double)detailsPCI.PowerKW,
                            AverageResistance = (double)detailsPCI.AverageResistance,
                            ResistanceTemperature = (double)detailsPCI.ResistanceTemperature,
                            Corrected20KW = (double)detailsPCI.Corregidas20KW,
                            Tension = (double)detailsPCI.Tension,
                        };

                        PrimaryCalculate primaryCalculate = new(request.Autotransformer, request.Monofasico, request.WindingMaterial, rating.TapPosition, rating.Position, secondaryCalculate)
                        {
                            ReducedCapacityAtLowVoltage = request.ReducedCapacityAtLowVoltage,
                            BaseRating = (double)rating.BaseRating,
                            Frequency = (double)rating.Frequency,
                            Temperature = (double)rating.Temperature,
                            AverageResistance = (double)rating.AverageResistance,
                            ResistanceTemperature = (double)rating.ResistanceTemperature,
                            Tension = (double)rating.Tension,
                        };

                        secondaryResults.Add(Calculate(primaryCalculate, elevationCorrectionFactor));
                    }

                    result.Results.Add(new RatingPCITestResult()
                    {
                        BaseRating = rating.BaseRating,
                        TemperatureElevation = rating.TemperatureElevation,
                        Frequency = rating.Frequency,
                        Temperature = rating.Temperature,
                        TapPosition = rating.TapPosition,
                        Position = rating.Position,
                        AverageResistance = rating.AverageResistance,
                        ResistanceTemperature = rating.ResistanceTemperature,
                        Tension = rating.Tension,
                        SecondaryPositions = new List<SecondaryPositionPCITestResult>(secondaryResults)
                    });
                }

                result.IsApproved = false;
                result.Kwtot100M = request.Kwtot100 * (1 + (request.TolerancyKwtot / 100m));

                RatingPCITestResult ratingtest = result.Results.FirstOrDefault(r => (r.BaseRating / 1000m) == request.KwcuMva);

                if (string.IsNullOrWhiteSpace(request.NominalSecondaryPosition))
                {
                    result.Messages.Add(new ResponseMessage(MessageType.Warning, "No se encontró la posición nominal secundaria."));
                    result.IsApproved = true;
                }
                else if (request.KwcuMva == 0m)
                {
                    result.Messages.Add(new ResponseMessage(MessageType.Warning, "No se usó la capacidad de pérdidas de cobre."));
                    result.IsApproved = true;
                }
                else if (ratingtest is null)
                {
                    result.Messages.Add(new ResponseMessage(MessageType.Warning, $"No se encontró la capacidad de pérdidas de cobre {request.KwcuMva}."));
                    result.IsApproved = true;
                }
                else
                {
                    if (ratingtest != null)
                    {
                        SecondaryPositionPCITestResult secondaryResult = ratingtest.SecondaryPositions
                            .FirstOrDefault(p => p.Position == request.NominalSecondaryPosition);

                        if (secondaryResult != null)
                        {
                            if (secondaryResult.LossesCorrected > request.Kwcu)
                            {
                                result.Messages.Add(new ResponseMessage(MessageType.Warning, "Revisar pérdidas de cobre vs pérdidas corregidas."));
                            }

                            if (secondaryResult.LossesTotal > request.Kwtot100)
                            {
                                result.Messages.Add(new ResponseMessage(MessageType.Warning, "Revisar pérdidas totales vs pérdidas totales de garantias."));
                            }

                            if (secondaryResult.LossesTotal <= result.Kwtot100M)
                            {
                                result.IsApproved = true;
                            }
                        }
                        else
                        {
                            result.Messages.Add(new ResponseMessage(MessageType.Warning, $"No se encontró la posición nominal secundaria {request.NominalSecondaryPosition}."));
                            result.IsApproved = true;
                        }
                    }
                }

                return new ApiResponse<PCITestResponse>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PCITestResponse>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message,
                    Structure = null
                };
            }
        }

        private SecondaryPositionPCITestResult Calculate(PrimaryCalculate primaryCalculate, double elevationCorrectionFactor)
        {
            SecondaryCalculate secondaryCalculate = primaryCalculate.SecondaryCalculate;

            double i2RTotal = Math.Round(secondaryCalculate.I2R + primaryCalculate.I2R, 7);
            double i2RCorrectedTotal = Math.Round(i2RTotal * elevationCorrectionFactor, 7);
            double wcuCor = Math.Round(Math.Pow(secondaryCalculate.Inom / secondaryCalculate.CurrentIrms, 2) * secondaryCalculate.PowerKW, 7);

            double wind = Math.Round(wcuCor - i2RTotal, 7);
            double windCorregido = Math.Round(wind / elevationCorrectionFactor, 7);

            double wCu = Math.Round((i2RCorrectedTotal + windCorregido) / 1000d, 7);
            double lossesCorrected = Math.Round((i2RCorrectedTotal + windCorregido) / 1000d, 7);

            double percentageR = Math.Round(wCu / secondaryCalculate.BaseRating * 100d, 7);
            double percentageZ = Math.Round((secondaryCalculate.VoltageKVRMS) / secondaryCalculate.Vnom * (secondaryCalculate.Inom / secondaryCalculate.CurrentIrms) * 100d, 7);

            double powRPercent = Math.Round(Math.Pow(percentageR, 2), 7);
            double powZPercent = Math.Round(Math.Pow(percentageZ, 2), 6);

            double percentageX = (powZPercent - powRPercent) < 0
                ? throw new Exception("Porcentaje R no puede ser mayor a Porcentaje Z (Raiz negativa)")
                : Math.Round(Math.Sqrt(powZPercent - powRPercent), 7);

            double lossesTotal = Math.Round(lossesCorrected + secondaryCalculate.Corrected20KW, 7);

            return new SecondaryPositionPCITestResult()
            {
                TapPosition = secondaryCalculate.TapPosition,
                Position = secondaryCalculate.Position,
                CurrentIrms = (decimal)Math.Round(secondaryCalculate.CurrentIrms, 7),
                Voltage = (decimal)Math.Round(secondaryCalculate.Voltage, 7),
                VoltagekVrms = (decimal)Math.Round(secondaryCalculate.VoltageKVRMS, 7),
                Power = (decimal)Math.Round(secondaryCalculate.Power, 7),
                PowerKW = (decimal)Math.Round(secondaryCalculate.PowerKW, 7),
                AverageResistance = (decimal)Math.Round(secondaryCalculate.AverageResistance, 7),
                ResistanceTemperature = (decimal)Math.Round(secondaryCalculate.ResistanceTemperature, 7),
                Tension = (decimal)Math.Round(secondaryCalculate.Tension, 7),
                VnomPrimary = (decimal)Math.Round(primaryCalculate.Vnom, 7),
                VnomSecondary = (decimal)Math.Round(secondaryCalculate.Vnom, 7),
                InomPrimary = (decimal)Math.Round(primaryCalculate.Inom, 7),
                InomSecondary = (decimal)Math.Round(secondaryCalculate.Inom, 7),
                WcuCor = (decimal)Math.Round(wcuCor, 7),
                PrimaryReductionCorrectionFactor = (decimal)Math.Round(primaryCalculate.ReductionCorrectionFactor, 7),
                SecondaryReductionCorrectionFactor = (decimal)Math.Round(secondaryCalculate.ReductionCorrectionFactor, 7),
                I2RPrimary = (decimal)Math.Round(primaryCalculate.I2R, 7),
                I2RSecondary = (decimal)Math.Round(secondaryCalculate.I2R, 7),
                I2RTotal = (decimal)Math.Round(i2RTotal, 7),
                ElevationCorrectionFactor = (decimal)Math.Round(elevationCorrectionFactor, 7),
                I2RCorrectedTotal = (decimal)Math.Round(i2RCorrectedTotal, 7),
                Wind = (decimal)Math.Round(wind, 7),
                WindCorr = (decimal)Math.Round(windCorregido, 7),
                WCu = (decimal)Math.Round(wCu, 7),
                PercentageR = (decimal)Math.Round(percentageR, 7),
                PercentageZ = (decimal)Math.Round(percentageZ, 7),
                PercentageX = (decimal)Math.Round(percentageX, 7),
                Corregidas20KW = (decimal)Math.Round(secondaryCalculate.Corrected20KW, 7),
                LossesCorrected = (decimal)Math.Round(lossesCorrected, 7),
                LossesTotal = (decimal)Math.Round(lossesTotal, 7)
            };
        }

        #endregion
    }

    public abstract class WindingCalculate
    {
        #region Fields

        private double voltagekVrms;
        private double powerKW;

        #endregion

        #region Constructor

        public WindingCalculate(
            bool autotransformer,
            bool monofasico,
            string tapPosition,
            string position,
            string windingMaterial)
        {
            Autotransformer = autotransformer;
            Monofasico = monofasico;
            TapPosition = tapPosition;
            Position = position;
            WindingMaterial = windingMaterial;
        }

        #endregion

        #region Properties

        public bool Autotransformer { get; }

        public bool Monofasico { get; }

        public string WindingMaterial { get; }

        public bool ReducedCapacityAtLowVoltage { get; set; }

        public string TapPosition { get; }

        public string Position { get; }

        public double BaseRating { get; internal set; }

        public double Frequency { get; internal set; }

        public double Temperature { get; internal set; }

        public double ResistanceTemperature { get; internal set; }

        public double AverageResistance { get; internal set; }

        public double Tension { get; internal set; }

        public abstract double Vnom { get; }

        public abstract double Inom { get; }

        public double ReductionCorrectionFactor => WindingMaterial.ToUpper() == "COBRE"
            ? Math.Round((234.5d + Temperature) / (234.5d + ResistanceTemperature), 7)
            : Math.Round((225 + Temperature) / (225d + ResistanceTemperature), 7);

        public double I2R => Monofasico
                    ? Math.Round(Math.Pow(Inom, 2) * AverageResistance * ReductionCorrectionFactor, 7)
                    : Autotransformer
                        ? Math.Round(3d * Math.Pow(Inom, 2) * AverageResistance * ReductionCorrectionFactor, 7)
                        : Math.Round(1.5d * Math.Pow(Inom, 2) * AverageResistance * ReductionCorrectionFactor, 7);

        #endregion
    }

    public class SecondaryCalculate : WindingCalculate
    {
        #region Constructor

        public SecondaryCalculate(bool autotransformer, bool monofasico, string windingMaterial, string tapPosition, string position)
            : base(autotransformer, monofasico, tapPosition, position, windingMaterial)
        {
        }

        #endregion

        #region Properties

        public double CurrentIrms { get; set; }

        public double Voltage { get; set; }

        public double VoltageKVRMS => Voltage * 1000d;

        public double Power { get; set; }

        public double PowerKW => Power * 1000d;

        public double Corrected20KW { get; internal set; }

        public override double Vnom => Monofasico ? Math.Round(Tension * 1000d / Math.Sqrt(3), 7) : Tension * 1000d;

        public override double Inom => Monofasico ? Math.Round(BaseRating / Vnom * 1000d, 7) : Math.Round(BaseRating / Vnom / Math.Sqrt(3) * 1000d, 7);


        #endregion
    }

    public class PrimaryCalculate : WindingCalculate
    {
        #region Constructor

        public PrimaryCalculate(bool autotransformer, bool monofasico, string windingMaterial, string tapPosition, string position, SecondaryCalculate secondaryCalculate)
            : base(autotransformer, monofasico, tapPosition, position, windingMaterial)
        {
            SecondaryCalculate = secondaryCalculate;
        }

        #endregion

        #region Properties

        public SecondaryCalculate SecondaryCalculate { get; private set; }

        public override double Vnom => Tension * 1000d;

        public override double Inom
        {
            get
            {
                double inom = ReducedCapacityAtLowVoltage
                         ? Math.Round(Vnom / 1000d, 7) < Tension
                             ? Math.Round(BaseRating / Tension / Math.Sqrt(3), 7)
                             : Math.Round(BaseRating / Vnom / Math.Sqrt(3) * 1000d, 7)
                         : Math.Round(BaseRating / Vnom / Math.Sqrt(3) * 1000d, 7);

                if (Autotransformer)
                {
                    inom -= SecondaryCalculate.Inom;
                }

                if (Monofasico)
                {
                    inom *= Math.Sqrt(3);
                }

                return Math.Round(inom, 7);
            }
        }

        #endregion
    }
}