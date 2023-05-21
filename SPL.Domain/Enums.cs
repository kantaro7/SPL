using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain
{
    public static class Enums {
        public static class EnumsGen 
        {
            public const int Succes = 1;
            public const int Error = -1;
            public const int FilterAll = -1;
        }

        public static class EnumsFilters
        {
            public const int Input = 1;
            public const int Select = 2;
            public const int Check = 3;
            public const int Table = 4;
        }

        public static class EnumsTables
        {
            public const string PRUEBAS = "SPL_PRUEBAS";
            public const string TIPO_UNIDAD = "SPL_TIPO_UNIDAD";
            public const string TERCER_DEVANADO_TIPO = "SPL_TERCER_DEVANADO_TIPO";
            public const string DESPLAZAMIENTO_ANGULAR = "SPL_DESPLAZAMIENTO_ANGULAR";
            public const string NORMA = "SPL_NORMA";
            public const string LOCAL = "LOCAL";

        }

        public static class  EnumsTypeReport
        {
            public const string RDT = "RDT";
            public const string RAD = "RAD";
            public const string RAN = "RAN";
            public const string FPC = "FPC";


            public const string FPB = "FPB";
            public const string RCT = "RCT";
            public const string ROD = "ROD";

            public const string PCI = "PCI";
            public const string PCE = "PCE";
            public const string PRD = "PRD";
            public const string PLR = "PLR";
            public const string PEE = "PEE";


            public const string ISZ = "ISZ";
            public const string RYE = "RYE";
            public const string PIR = "PIR";
            public const string PIM = "PIM";
            public const string TIN = "TIN";
            public const string TAP = "TAP";
            public const string CEM = "CEM";
            public const string TDP = "TDP";

            public const string CGD = "CGD";
            public const string RDD = "RDD";
            public const string ARF = "ARF";
            public const string IND = "IND";
            public const string FPA = "FPA";
            public const string BPC = "BPC";

            public const string NRA = "NRA";
            public const string ETD = "ETD";
            public const string DPR = "DPR";
        }

        public static class RADTime
        {
            public const string c_15sec = "15 Sec.";
            public const string c_60sec = "60 Sec.";
            public const string c_10min = "10 Min.";
            public const string c_30sec = "30 Sec.";
        }




       public static class CGDKeys
        {
            public const string Hydrogen = "HYDROGEN";
            public const string Oxygen = "OXYGEN";
            public const string Nitrogen = "NITROGEN";
            public const string Methane = "METHANE";
            public const string CarbonMonoxide = "CARBONMONOXIDE";
            public const string CarbonDioxide = "CARBONDIOXIDE";
            public const string Ethylene = "ETHYLENE";
            public const string Ethane = "ETHANE";
            public const string Acetylene = "ACETYLENE";
            public const string TotalGases = "TOTALGASES";
            public const string CombustibleGases = "COMBUSTIBLEGASES";
            public const string PorcCombustibleGas = "PORCCOMBUSTIBLEGAS";
            public const string PorcGasContent = "PORCGASCONTENT";
            public const string AcetyleneTest = "ACETYLENETEST";
            public const string HydrogenTest = "HYDROGENTEST";
            public const string MethaneEthyleneEthaneTest = "METHANEETHYLENEETHANETEST";
            public const string CarbonMonoxideTest = "CARBONMONOXIDETEST";
            public const string CarbonDioxideTest = "CARBONDIOXIDETEST";
          
        }



        public enum MicroservicesEnum
        {
            spldesigninformation,
            splmasters,
            splsidco,
            splreports,
            spltests,
            splconfiguration,
            splsecurity

        }
    }
 
}
