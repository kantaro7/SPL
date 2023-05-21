namespace SPL.Domain.SPL.Tests.PCE
{
    using System;
    using System.Collections.Generic;

    using global::SPL.Domain.SPL.Configuration;

    public class PCETests
    {

        public decimal ToleranciaExec { get; set; }
        public decimal ToleranciaPer { get; set; }

        public decimal VoltajeBase { get; set; }
        public string UmVoltajeBase { get; set; } 
        public string KeyTest { get; set; } 
        
        public decimal Capacidad { get; set; }
        public string UmCapacidad { get; set; }

        public decimal Frecuencia { get; set; }
        public string UmFrecuencia { get; set; }

        public string PosPruebaAT { get; set; }
        public string PosPruebaBT { get; set; }
        public string PosPruebaTER { get; set; }

        public decimal Temperatura { get; set; }
        public string UmTemperatura { get; set; } 
        
        public decimal GarPerdidas { get; set; }
        public string UmGarPerdidas { get; set; }

        public decimal GarCExcitacion { get; set; }
        public string UmGarCExcitacion { get; set; }

        public decimal PorGarPerdidasTolerancy { get; set; }

        public long IInicio { get; set; }
        public long IFin { get; set; }
        public long Intervalo { get; set; }
      

        public DateTime Date { get; set; }

      
        public List<PCETestsDetails> PCETestsDetails { get; set; }
    }
}
