using System.Collections.Generic;

namespace SPL.WebApp.ViewModels
{
    public class DevanadoEnergizadoViewModel
    {
        public string Devanado { get; set; }
        public bool Select { get; set; }
    }

    public class ListaDevanados
    {
        public List<DevanadoEnergizadoViewModel> DevanadosAT { get; set; }
        public List<DevanadoEnergizadoViewModel> DevanadosBT { get; set; }
        public List<DevanadoEnergizadoViewModel> DevanadosTer { get; set; }

        public int SeleccionadoAT { get; set; }
        public int SeleccionadoBT { get; set; }
        public int SeleccionadoTer { get; set; }

        public ListaDevanados()
        {
            this.DevanadosAT = new();
            this.DevanadosBT = new();
            this.DevanadosTer = new();
            this.SeleccionadoAT = this.SeleccionadoBT = this.SeleccionadoTer = 0;
        }
    }
}
