namespace SPL.Tests.Application.Handlers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.PCE;
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.FPB;
    using SPL.Domain.SPL.Tests.FPC;

    using SPL.Domain.SPL.Tests.PCI;
    using SPL.Domain.SPL.Tests.PLR;
    using SPL.Domain.SPL.Tests.PRD;
    using SPL.Tests.Application.Commands.Tests;

    public class PRDTestsHandler : IRequestHandler<PRDTestsCommand, ApiResponse<ResultPRDTests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public PRDTestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultPRDTests>> Handle(PRDTestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ResultPRDTests result = new();

                PRDTests ObjTests = new();

                List<PRDTestsDetails> ResultPRDTestsDetails = new();
         

             

                List<ErrorColumns> listErrors = new();
              
                ObjTests = request.Data;

                if (request.Data == null)
                {
                    return new ApiResponse<ResultPRDTests>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Faltan datos para realizar el cálculo",
                        Structure = null
                    };
                }

                 
                decimal V = Math.Round( (request.Data.KeyTest.ToUpper().Equals("100")) ? ((request.Data.NominalVoltage / Convert.ToDecimal(Math.Sqrt(3))) * 1000) : ((request.Data.NominalVoltage * Convert.ToDecimal(1.1) / Convert.ToDecimal(Math.Sqrt(3))) * 1000),0);
                   request.Data.PRDTestsDetails.V = V;

                    decimal I = Math.Round((request.Data.PRDTestsDetails.Cap / V) * 1000, 3);
                   request.Data.PRDTestsDetails.I= I;

                decimal Lxp = Math.Round(1 / request.Data.PRDTestsDetails.U * request.Data.PRDTestsDetails.M3 * (request.Data.PRDTestsDetails.C4 / request.Data.PRDTestsDetails.Cn + 1), 2);
                request.Data.PRDTestsDetails.Lxp = Lxp;

                decimal Rxp = Math.Round(1 / request.Data.PRDTestsDetails.U * request.Data.PRDTestsDetails.M3 / (request.Data.PRDTestsDetails.Cn * request.Data.PRDTestsDetails.R4s) * (request.Data.PRDTestsDetails.Cn / request.Data.PRDTestsDetails.C4 + 1), 3);
                request.Data.PRDTestsDetails.Rxp = Rxp;

                decimal P = Math.Round((V * V) / Rxp, 0);
                request.Data.PRDTestsDetails.P = P;

                decimal Xm = Math.Round(request.Data.PRDTestsDetails.Vm / request.Data.PRDTestsDetails.Im, 2);
                request.Data.PRDTestsDetails.Xm =Xm;

                decimal Xc = Math.Round(V / I, 2);
                request.Data.PRDTestsDetails.Xc = Xc;

                decimal PorcDev = Math.Round((Xm / Xc - 1), 6);
                request.Data.PRDTestsDetails.PorcDesv = Math.Round( PorcDev * 100,2);

                decimal Pjm = Math.Round((I * I) * request.Data.PRDTestsDetails.Rm, 0);
                request.Data.PRDTestsDetails.Pjm = Pjm;

                decimal Fc = Math.Round((Convert.ToDecimal(234.5) + request.Data.PRDTestsDetails.Tmp) / (Convert.ToDecimal(234.5) + request.Data.PRDTestsDetails.Tm), 5);
                request.Data.PRDTestsDetails.Fc = Fc;

                decimal Pjmc = Math.Round(Pjm * Fc, 0);
                request.Data.PRDTestsDetails.Pjmc = Pjmc;

                decimal Pe = Math.Round(P - Pjmc - request.Data.PRDTestsDetails.Pfe, 0);
                request.Data.PRDTestsDetails.Pe = Pe;

                decimal Fc2 = Math.Round((Convert.ToDecimal( 234.5) + request.Data.PRDTestsDetails.Tr) / (Convert.ToDecimal(234.5) + request.Data.PRDTestsDetails.Tmp), 0);
                request.Data.PRDTestsDetails.Fc2 = Fc2;

                decimal Pt = Math.Round(Pjmc * Fc2 + (Pe / Fc2) + request.Data.PRDTestsDetails.Pfe, 0);
                request.Data.PRDTestsDetails.Pt = Pt;

                result.PRDTests = request.Data;
                result.Results = listErrors.ToList();
             
           
                return new ApiResponse<ResultPRDTests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                };
            }
            catch (Exception ex)
             {
            return new ApiResponse<ResultPRDTests>()
            {
                Code = (int)ResponsesID.fallido,
                Description = ex.Message,
                Structure = null
            };
            }

           
       
          
        }
      
        #endregion
    }
}
