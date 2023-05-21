using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Configuration
{
    public class StabilizationDetailsDataDto
    {
        public decimal IdReg { get; set; }
        public DateTime FechaHora { get; set; }

        [Range(0.1, 9999.9, ErrorMessage = "El campo KwMedidos ser numérico mayor a cero considerando hasta 4 enteros y 1 decimal")]
        public decimal KwMedidos { get; set; }

        [Range(0.1, 9999.9, ErrorMessage = "El campo AmpMedidos ser numérico mayor a cero considerando hasta 4 enteros y 1 decimal")]
        public decimal AmpMedidos { get; set; }


        [Range(0.1, 999.9, ErrorMessage = "El campo CabSupRadBco1 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabSupRadBco1 { get; set; }


      
        [Range(0, 99, ErrorMessage = "El campo CanalSup1 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalSup1 { get; set; }

        [Range(0.1, 999.9, ErrorMessage = "El campo CabSupRadBco2 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabSupRadBco2 { get; set; }


        [Range(0, 99, ErrorMessage = "El campo CanalSup2 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalSup2 { get; set; }

        [Range(0.1, 999.9, ErrorMessage = "El campo CabSupRadBco3 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabSupRadBco3 { get; set; }

        [Range(0, 99, ErrorMessage = "El campo CanalSup3 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalSup3 { get; set; }



        [Range(0.1, 999.9, ErrorMessage = "El campo CabSupRadBco4 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabSupRadBco4 { get; set; }

        [Range(0, 99, ErrorMessage = "El campo CanalSup4 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalSup4 { get; set; }


        [Range(0.1, 999.9, ErrorMessage = "El campo CabSupRadBco5 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabSupRadBco5 { get; set; }


        [Range(0, 99, ErrorMessage = "El campo CanalSup5 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalSup5 { get; set; }


        [Range(0.1, 999.9, ErrorMessage = "El campo CabSupRadBco6 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabSupRadBco6 { get; set; }


        [Range(0, 99, ErrorMessage = "El campo CanalSup6 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalSup6 { get; set; }


        [Range(0.1, 999.9, ErrorMessage = "El campo CabSupRadBco7 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabSupRadBco7 { get; set; }

        [Range(0, 99, ErrorMessage = "El campo CanalSup7 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalSup7 { get; set; }



        [Range(0.1, 999.9, ErrorMessage = "El campo CabSupRadBco8 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabSupRadBco8 { get; set; }


        [Range(0, 99, ErrorMessage = "El campo CanalSup8 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]

        public decimal? CanalSup8 { get; set; }

        [Range(0.1, 999.9, ErrorMessage = "El campo CabSupRadBco9 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabSupRadBco9 { get; set; }


        [Range(0, 99, ErrorMessage = "El campo CanalSup9 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalSup9 { get; set; }


        [Range(0.1, 999.9, ErrorMessage = "El campo CabSupRadBco10 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabSupRadBco10 { get; set; }



        [Range(0, 99, ErrorMessage = "El campo CanalSup10 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalSup10 { get; set; }


        [Range(0.01, 999.99, ErrorMessage = "El campo PromRadSup ser numérico mayor a cero considerando hasta 3 enteros y 2 decimales")]
        public decimal PromRadSup { get; set; }


        [Range(0.1, 999.9, ErrorMessage = "El campo CabInfRadBco1 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabInfRadBco1 { get; set; }


        [Range(0, 99, ErrorMessage = "El campo CanalInf1 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalInf1 { get; set; }


        [Range(0.1, 999.9, ErrorMessage = "El campo CabInfRadBco2 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabInfRadBco2 { get; set; }

        [Range(0, 99, ErrorMessage = "El campo CanalInf2 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalInf2 { get; set; }


        [Range(0.1, 999.9, ErrorMessage = "El campo CabInfRadBco3 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabInfRadBco3 { get; set; }


        [Range(0, 99, ErrorMessage = "El campo CanalInf3 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalInf3 { get; set; }


        [Range(0.1, 999.9, ErrorMessage = "El campo CabInfRadBco4 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabInfRadBco4 { get; set; }

        [Range(0, 99, ErrorMessage = "El campo CanalInf4 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalInf4 { get; set; }


        [Range(0.1, 999.9, ErrorMessage = "El campo CabInfRadBco5 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabInfRadBco5 { get; set; }


        [Range(0, 99, ErrorMessage = "El campo CanalInf5 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalInf5 { get; set; }


        [Range(0.1, 999.9, ErrorMessage = "El campo CabInfRadBco6 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabInfRadBco6 { get; set; }

        [Range(0, 99, ErrorMessage = "El campo CanalInf6 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalInf6 { get; set; }



        [Range(0.1, 999.9, ErrorMessage = "El campo CabInfRadBco7 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabInfRadBco7 { get; set; }

        [Range(0, 99, ErrorMessage = "El campo CanalInf7 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalInf7 { get; set; }


        [Range(0.1, 999.9, ErrorMessage = "El campo CabInfRadBco8 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabInfRadBco8 { get; set; }

        [Range(0, 99, ErrorMessage = "El campo CanalInf8 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalInf8 { get; set; }


        [Range(0.1, 999.9, ErrorMessage = "El campo CabInfRadBco9 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabInfRadBco9 { get; set; }


        [Range(0, 99, ErrorMessage = "El campo CanalInf9 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalInf9 { get; set; }


        [Range(0.1, 999.9, ErrorMessage = "El campo CabInfRadBco10 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? CabInfRadBco10 { get; set; }


        [Range(0, 99, ErrorMessage = "El campo CanalInf10 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalInf10 { get; set; }



        [Range(0.01, 999.99, ErrorMessage = "El campo PromRadInf ser numérico mayor a cero considerando hasta 3 enteros y 2 decimales")]
        public decimal PromRadInf { get; set; }



        public decimal? Ambiente1 { get; set; }
        public decimal? CanalAmb1 { get; set; }
        public decimal? Ambiente2 { get; set; }
        public decimal? CanalAmb2 { get; set; }
        public decimal? Ambiente3 { get; set; }
        public decimal? CanalAmb3 { get; set; }

        [Range(0.01, 99.99, ErrorMessage = "El campo AmbienteProm ser numérico mayor a cero considerando hasta 2 enteros y 2 decimales")]
        public decimal AmbienteProm { get; set; }


        [Range(0.1, 99.9, ErrorMessage = "El campo TempTapa ser numérico mayor a cero considerando hasta 2 enteros y 1 decimal")]
        public decimal TempTapa { get; set; }

        [Range(0, 99, ErrorMessage = "El campo CanalTtapa debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalTtapa { get; set; }

        [Range(0.01, 99.99, ErrorMessage = "El campo Aor ser numérico mayor a cero considerando hasta 2 enteros y 2 decimales")]
        public decimal Aor { get; set; }

        [Range(0.01, 99.99, ErrorMessage = "El campo Tor ser numérico mayor a cero considerando hasta 2 enteros y 2 decimales")]
        public decimal Tor { get; set; }

        [Range(0.01, 99.99, ErrorMessage = "El campo Bor ser numérico mayor a cero considerando hasta 2 enteros y 2 decimales")]
        public decimal Bor { get; set; }

        [Range(0.01, 99.99, ErrorMessage = "El campo AorCorr ser numérico mayor a cero considerando hasta 2 enteros y 2 decimales")]
        public decimal AorCorr { get; set; }

        [Range(0.01, 99.99, ErrorMessage = "El campo TorCorr ser numérico mayor a cero considerando hasta 2 enteros y 2 decimales")]
        public decimal TorCorr { get; set; }

        [Range(0.01, 99.99, ErrorMessage = "El campo Ao ser numérico mayor a cero considerando hasta 2 enteros y 2 decimales")]
        public decimal Ao { get; set; }

        [Range(0.1, 999.9, ErrorMessage = "El campo TempInstr1 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? TempInstr1 { get; set; }


        [Range(0, 99, ErrorMessage = "El campo CanalInst1 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalInst1 { get; set; }

        [Range(0.1, 999.9, ErrorMessage = "El campo TempInstr2 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? TempInstr2 { get; set; }

        [Range(0, 99, ErrorMessage = "El campo CanalInst2 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalInst2 { get; set; }

        [Range(0.1, 999.9, ErrorMessage = "El campo TempInstr3 ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal? TempInstr3 { get; set; }


        [Range(0, 99, ErrorMessage = "El campo CanalInst3 debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? CanalInst3 { get; set; }



        public bool VerifVent1 { get; set; }
        public bool VerifVent2 { get; set; }

        [Range(0, 99, ErrorMessage = "El campo Presion debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? Presion { get; set; }
        public bool Estable { get; set; }
    }
}
