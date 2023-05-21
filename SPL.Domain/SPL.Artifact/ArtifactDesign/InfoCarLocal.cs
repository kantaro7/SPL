using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Artifact.ArtifactDesign
{
    public class InfoCarLocal
    {
        public InfoCarLocal() { }
        public string OrderCode { get; set; }
        public decimal? Mvaf1Voltage1 { get; set; }
        public decimal? Mvaf1Voltage2 { get; set; }
        public decimal? Mvaf2Voltage1 { get; set; }
        public decimal? Mvaf2Voltage2 { get; set; }
        public decimal? Mvaf3Voltage1 { get; set; }
        public decimal? Mvaf3Voltage2 { get; set; }
        public decimal? Mvaf4Voltage1 { get; set; }
        public decimal? Mvaf4Voltage2 { get; set; }
        public decimal? Mvaf1Nbai1 { get; set; }
        public decimal? Mvaf2Nbai1 { get; set; }
        public decimal? Mvaf3Nbai1 { get; set; }
        public decimal? Mvaf4Nbai1 { get; set; }
        public decimal? Mvaf1NbaiNeutro { get; set; }
        public decimal? Mvaf2NbaiNeutro { get; set; }
        public decimal? Mvaf3NbaiNeutro { get; set; }
        public decimal? Mvaf4NbaiNeutro { get; set; }
        public decimal? Mvaf1ConnectionId { get; set; }
        public string Mvaf1ConnectionOther { get; set; }
        public string ConexionEq { get; set; }
        public decimal? Mvaf2ConnectionId { get; set; }
        public string Mvaf2ConnectionOther { get; set; }
        public string ConexionEq2 { get; set; }
        public decimal? Mvaf3ConnectionId { get; set; }
        public string Mvaf3ConnectionOther { get; set; }
        public string ConexionEq3 { get; set; }
        public decimal? Mvaf4ConnectionId { get; set; }
        public string Mvaf4ConnectionOther { get; set; }
        public string ConexionEq4 { get; set; }
        public decimal? Mvaf1Taps { get; set; }
        public decimal? Mvaf2Taps { get; set; }
        public decimal? Mvaf3Taps { get; set; }
        public decimal? Mvaf4Taps { get; set; }
        public decimal? RcbnFcbn1 { get; set; }
        public decimal? Mvaf1DerUp { get; set; }
        public decimal? Mvaf1DerDown { get; set; }
        public decimal? RcbnFcbn2 { get; set; }
        public decimal? Mvaf2DerUp { get; set; }
        public decimal? Mvaf2DerDown { get; set; }
        public decimal? RcbnFcbn3 { get; set; }
        public decimal? Mvaf3DerUp { get; set; }
        public decimal? Mvaf3DerDown { get; set; }
        public decimal? RcbnFcbn4 { get; set; }
        public decimal? Mvaf4DerUp { get; set; }
        public decimal? Mvaf4DerDown { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
        public decimal? Mvaf1Voltage3 { get; set; }
        public decimal? Mvaf1Voltage4 { get; set; }
        public decimal? Mvaf2Voltage3 { get; set; }
        public decimal? Mvaf2Voltage4 { get; set; }
        public decimal? Mvaf3Voltage3 { get; set; }
        public decimal? Mvaf3Voltage4 { get; set; }
        public decimal? Mvaf4Voltage3 { get; set; }
        public decimal? Mvaf4Voltage4 { get; set; }

        public InfoCarLocal(bool status,string pOrderCode,VoltageKV pVoltageKV, NBAIBilKv pNBAI, ConnectionTypes pConnections, Derivations pDerivations, Taps pTaps, NBAINeutro pNBAINeutro, string creadoPor) {
            setInfoCarLocal(status, pOrderCode, pVoltageKV, pNBAI, pConnections,  pDerivations,  pTaps,  pNBAINeutro, new Nullable<DateTime>(), creadoPor, "");
        }

        public InfoCarLocal(bool status, string pOrderCode, VoltageKV pVoltageKV, NBAIBilKv pNBAI, ConnectionTypes pConnections, Derivations pDerivations, Taps pTaps, NBAINeutro pNBAINeutro, DateTime pDateCreacion, string creadoPor, string modificadoPor)
        {
            setInfoCarLocal(status, pOrderCode, pVoltageKV, pNBAI, pConnections, pDerivations, pTaps, pNBAINeutro, pDateCreacion, creadoPor, modificadoPor);
        }

        public void setInfoCarLocal(bool status, string pOrderCode, VoltageKV pVoltageKV, NBAIBilKv pNBAI, ConnectionTypes pConnections, Derivations pDerivations, Taps pTaps, NBAINeutro pNBAINeutro, DateTime? pDateCreacion, string creadoPor, string modificadoPor)
        {

            if (pDateCreacion != null)
            {
                this.Fechacreacion = pDateCreacion;
            }
            this.OrderCode = pOrderCode;

            if (status)
            {
                this.Creadopor = creadoPor;
                this.Fechacreacion = DateTime.Now;
                this.Fechamodificacion = null;
                this.Modificadopor = null;
            }
            else {
                this.Creadopor = creadoPor;
                this.Modificadopor = modificadoPor;
                this.Fechamodificacion = DateTime.Now;
            }
         
            this.Mvaf1NbaiNeutro = pNBAINeutro?.valornbaineutroaltatension;
            this.Mvaf2NbaiNeutro = pNBAINeutro?.valornbaineutrobajatension;
            this.Mvaf3NbaiNeutro = pNBAINeutro?.valornbaineutrosegundabaja;
            this.Mvaf4NbaiNeutro = pNBAINeutro?.valornbaineutrotercera;

            this.Mvaf1ConnectionOther = pConnections?.otraconexionaltatension;
            this.Mvaf2ConnectionOther = pConnections?.otraconexionbajatension;
            this.Mvaf3ConnectionOther = pConnections?.otraconexionsegundabaja;
            this.Mvaf4ConnectionOther = pConnections?.otraconexiontercera;

            this.Mvaf1ConnectionId = pConnections?.idconexionaltatension;
            this.Mvaf2ConnectionId = pConnections?.idconexionbajatension;
            this.Mvaf3ConnectionId = pConnections?.idconexionsegundabaja;
            this.Mvaf4ConnectionId = pConnections?.idconexiontercera;

            this.Mvaf1DerUp = pDerivations?.valorderivacionupaltatension;
            this.Mvaf1DerDown = pDerivations?.valorderivaciondownaltatension;
            this.Mvaf2DerUp = pDerivations?.valorderivacionupbajatension;
            this.Mvaf2DerDown = pDerivations?.valorderivaciondownbajatension;

            this.Mvaf3DerUp = pDerivations?.valorderivacionupsegundatension;
            this.Mvaf3DerDown = pDerivations?.valorderivaciondownsegundatension;

            this.Mvaf4DerUp = pDerivations?.valorderivacionupterceratension;
            this.Mvaf4DerDown = pDerivations?.valorderivaciondownterceratension;

            this.RcbnFcbn1 = Convert.ToDecimal(pDerivations?.tipoderivacionaltatension);
            this.RcbnFcbn2 = Convert.ToDecimal(pDerivations?.tipoderivacionbajatension);
            this.RcbnFcbn3 = Convert.ToDecimal(pDerivations?.tipoderivacionsegundatension);
            this.RcbnFcbn4 = Convert.ToDecimal(pDerivations?.tipoderivacionterceratension);

            ConexionEq = pDerivations?.conexionequivalente;
            ConexionEq2 = pDerivations?.conexionequivalente_2;
            ConexionEq3 = pDerivations?.conexionequivalente_3;
            ConexionEq4 = pDerivations?.conexionequivalente_4;
            //Mvaf1ConnectionId = pDerivations.idConexionEquivalente;
            //Mvaf2ConnectionId = pDerivations.idConexionEquivalente2;
            //Mvaf3ConnectionId = pDerivations.idConexionEquivalente3;
            //Mvaf4ConnectionId = pDerivations.idConexionEquivalente4;

            this.Mvaf1Nbai1 = pNBAI?.nbaialtatension;
            this.Mvaf2Nbai1 = pNBAI?.nbaibajatension;
            this.Mvaf3Nbai1 = pNBAI?.nbaisegundabaja;
            this.Mvaf4Nbai1 = pNBAI?.nabaitercera;

            this.Mvaf1Taps = pTaps?.tapsaltatension;
            this.Mvaf2Taps = pTaps?.tapsbajatension;
            this.Mvaf3Taps = pTaps?.tapssegundabaja;
            this.Mvaf4Taps = pTaps?.tapsterciario;

            this.Mvaf1Voltage1 = pVoltageKV?.tensionkvaltatension1;
            this.Mvaf1Voltage2 = pVoltageKV?.tensionkvaltatension2;
            this.Mvaf2Voltage1 = pVoltageKV?.tensionkvbajatension1;
            this.Mvaf2Voltage2 = pVoltageKV?.tensionkvbajatension2;
            this.Mvaf3Voltage1 = pVoltageKV?.tensionkvsegundabaja1;
            this.Mvaf3Voltage2 = pVoltageKV?.tensionkvsegundabaja2;
            this.Mvaf4Voltage1 = pVoltageKV?.tensionkvterciario1;
            this.Mvaf4Voltage2 = pVoltageKV?.tensionkvterciario2;
            this.Mvaf1Voltage3 = pVoltageKV?.tensionkvaltatension3;
            this.Mvaf1Voltage4 = pVoltageKV?.tensionkvaltatension4;
            this.Mvaf2Voltage3 = pVoltageKV?.tensionkvbajatension3;
            this.Mvaf2Voltage4 = pVoltageKV?.tensionkvbajatension4;
            this.Mvaf3Voltage3 = pVoltageKV?.tensionkvsegundabaja3;
            this.Mvaf3Voltage4 = pVoltageKV?.tensionkvsegundabaja4;
            this.Mvaf4Voltage3 = pVoltageKV?.tensionkvterciario3;
            this.Mvaf4Voltage4 = pVoltageKV?.tensionkvterciario4;
        }

    }
}
