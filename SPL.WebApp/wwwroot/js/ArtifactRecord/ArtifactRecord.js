
class TapBaanViewModel {
    constructor(data) {
        this.OrderCode = '';
        this.ComboNumericSc = 0;
        this.CantidadSupSc = 0;
        this.PorcentajeSupSc = 0;
        this.CantidadInfSc = 0;
        this.PorcentajeInfSc = 0;
        this.NominalSc = 0;
        this.IdentificacionSc = 0;
        this.InvertidoSc = false;
        this.ComboNumericBc = 0;
        this.CantidadSupBc = 0;
        this.PorcentajeSupBc = 0;
        this.CantidadInfBc = 0;
        this.PorcentajeInfBc = 0;
        this.NominalBc = 0;
        this.IdentificacionBc = 0;
        this.InvertidoBc = '';
        this.CreadoPor = '';
        this.FechaCreacion = '';
        this.ModificadoPor = '';
        this.FechaModificacion = '';
    }
    map(data) {
        if (data !== null && data !== undefined) {
            this.OrderCode = data.OrderCode;
            this.ComboNumericSc = data.ComboNumericSc;
            this.CantidadSupSc = data.CantidadSupSc;
            this.PorcentajeSupSc = data.PorcentajeSupSc;
            this.CantidadInfSc = data.CantidadInfSc;
            this.PorcentajeInfSc = data.PorcentajeInfSc;
            this.NominalSc = data.NominalSc;
            this.IdentificacionSc = data.IdentificacionSc;
            this.InvertidoSc = (data.InvertidoSc) == null ? false : (data.InvertidoSc);
            this.ComboNumericBc = data.ComboNumericBc;
            this.CantidadSupBc = data.CantidadSupBc;
            this.PorcentajeSupBc = data.PorcentajeSupBc;
            this.CantidadInfBc = data.CantidadInfBc;
            this.PorcentajeInfBc = data.PorcentajeInfBc;
            this.NominalBc = data.NominalBc;
            this.IdentificacionBc = data.IdentificacionBc;
            this.InvertidoBc = (data.InvertidoBc) == null ? false : (data.InvertidoBc);
            this.CreadoPor = data.CreadoPor;
            this.FechaCreacion = data.FechaCreacion;
            this.ModificadoPor = data.ModificadoPor;
            this.FechaModificacion = data.FechaModificacion;
        }
    }
}

class LabTestsArtifactViewModel {
    constructor() {
        this.OrderCode = '';
        this.TextTestRoutine = 0;
        this.TextTestDielectric = 0;
        this.TextTestPrototype = 0;
        this.CreadoPor = 0;
        this.FechaCreacion = 0;
        this.ModificadoPor = 0;
        this.FechaModificacion = 0;
    }

    map(data) {
        if (data !== null && data !== undefined) {
            this.OrderCode = data.OrderCode;
            this.TextTestRoutine = data.TextTestRoutine;
            this.TextTestDielectric = data.TextTestDielectric;
            this.TextTestPrototype = data.TextTestPrototype;
            this.CreadoPor = data.CreadoPor;
            this.FechaCreacion = data.FechaCreacion;
            this.ModificadoPor = data.ModificadoPor;
            this.FechaModificacion = data.FechaModificacion;
        }
    }
}


class ArtifactRecordViewModel {

    constructor() {
        //General Properties
        this.NoSerie = '';
        this.OrderCode = '';
        this.Creadopor = '';
        this.Fechacreacion = '';
        this.Modificadopor = '';
        this.Fechamodificacion = '';
        this.OperationsItems = [];
        this.IsFromSPL = false;
        this.Update = '';


        //Properties Tab General
        this.Descripcion = '';
        this.Phases = 0;
        this.CustomerName = '';
        this.Frecuency = 0;
        this.PoNumeric = '';
        this.AltitudeF1 = 0;
        this.AltitudeF2 = '';
        this.Typetrafoid = 0;
        this.TipoUnidad = '';
        this.ApplicationId = 0;
        this.StandardId = 0;
        this.Norma = '';
        this.NormaEquivalente = '';
        this.LanguageId = 0;
        this.ClaveIdioma = '';
        this.PolarityId = 0;
        this.PolarityOther = '';
        this.DesplazamientoAngular = '';
        this.OilBrand = '';
        this.OilType = '';


        //Properties Tab Characteristics
        this.CharacteristicsArtifacts = [];
        this.TensionKvAltaTension1 = 0;
        this.TensionKvAltaTension2 = 0;
        this.TensionKvAltaTension3 = 0;
        this.TensionKvAltaTension4 = 0;
        this.TensionKvBajaTension1 = 0;
        this.TensionKvBajaTension2 = 0;
        this.TensionKvBajaTension3 = 0;
        this.TensionKvBajaTension4 = 0;
        this.TensionKvSegundaBaja1 = 0;
        this.TensionKvSegundaBaja2 = 0;
        this.TensionKvSegundaBaja3 = 0;
        this.TensionKvSegundaBaja4 = 0;
        this.TensionKvTerciario1 = 0;
        this.TensionKvTerciario2 = 0;
        this.TensionKvTerciario3 = 0;
        this.TensionKvTerciario4 = 0;
        this.NbaiAltaTension = 0;
        this.NbaiBajaTension = 0;
        this.NbaiSegundaBaja = 0;
        this.NabaiTercera = 0;
        this.IdConexionAltaTension = 0;
        this.ConexionAltaTension = '';
        this.OtraConexionAltaTension = '';
        this.IdConexionBajaTension = 0;
        this.ConexionBajaTension = '';
        this.OtraConexionBajaTension = '';
        this.IdConexionSegundaBaja = 0;
        this.ConexionSegundaBaja = '';
        this.OtraConexionSegundaBaja = '';
        this.IdConexionTercera = 0;
        this.ConexionTercera = '';
        this.OtraConexionTercera = '';
        this.TipoDerivacionAltaTension = '';
        this.ValorDerivacionUpAltaTension = 0;
        this.ValorDerivacionDownAltaTension = 0;
        this.TipoDerivacionAltaTension_2 = '';
        this.ValorDerivacionUpAltaTension_2 = 0;
        this.ValorDerivacionDownAltaTension_2 = 0;
        this.TipoDerivacionBajaTension = '';
        this.ValorDerivacionUpBajaTension = 0;
        this.ValorDerivacionDownBajaTension = 0;
        this.TipoDerivacionSegundaTension = '';
        this.ValorDerivacionUpSegundaTension = 0;
        this.ValorDerivacionDownSegundaTension = 0;
        this.TipoDerivacionTerceraTension = '';
        this.ValorDerivacionUpTerceraTension = 0;
        this.ValorDerivacionDownTerceraTension = 0;
        this.ConexionEquivalente = '';
        this.ConexionEquivalente_2 = '';
        this.ConexionEquivalente_3 = '';
        this.ConexionEquivalente_4 = '';
        this.IdConexionEquivalente = 0;
        this.IdConexionEquivalente2 = 0;
        this.IdConexionEquivalente3 = 0;
        this.IdConexionEquivalente4 = 0;
        this.TapsAltaTension = 0;
        this.TapsAltaTension_2 = 0;
        this.TapsBajaTension = 0;
        this.TapsSegundaBaja = 0;
        this.TapsTerciario = 0;
        this.ValorNbaiNeutroAltaTension = 0;
        this.ValorNbaiNeutroBajaTension = 0;
        this.ValorNbaiNeutroSegundaBaja = 0;
        this.ValorNbaiNeutroTercera = 0;

        //Properties Tab Warranties

        this.Iexc100 = 0;
        this.Iexc110 = 0;
        this.Kwfe100 = 0;
        this.Kwfe110 = 0;
        this.TolerancyKwfe = 0;
        this.KwcuMva = 0;
        this.KwcuKv = 0;
        this.Kwcu = 0;
        this.TolerancyKwCu = 0;
        this.Kwaux3 = 0;
        this.Kwaux4 = 0;
        this.Kwaux1 = 0;
        this.Kwaux2 = 0;
        this.TolerancyKwAux = 0;
        this.Kwtot100 = 0;
        this.Kwtot110 = 0;
        this.TolerancyKwtot = 0;
        this.ZPositiveMva = 0;
        this.ZPositiveHx = 0;
        this.ZPositiveHy = 0;
        this.ZPositiveXy = 0;
        this.TolerancyZpositive = 0;
        this.TolerancyZpositive2 = 0;
        this.NoiseOa = 0;
        this.NoiseFa1 = 0;
        this.NoiseFa2 = 0;

        //Properties Tab Nozzles

        this.NozzlesArtifacts = [];

        //Properties Tab LightningRod

        this.LightningRodArtifacts = [];

        //Properties Tab Changes

        this.ChangingTablesArtifacs = [];

        //this.TapBaan = new TapBaanViewModel();

        //Properties Test Labs
        this.LabTestsArtifact = new LabTestsArtifactViewModel();

        //Properties norms

        this.RulesArtifacts = [];

    }

    map(ArtifactRecordViewModel) {

        //General Properties
        this.NoSerie = ArtifactRecordViewModel.NoSerie;
        this.OrderCode = ArtifactRecordViewModel.OrderCode;
        this.Creadopor = ArtifactRecordViewModel.Creadopor;
        this.Fechacreacion = ArtifactRecordViewModel.Fechacreacion;
        this.Modificadopor = ArtifactRecordViewModel.Modificadopor;
        this.Fechamodificacion = ArtifactRecordViewModel.Fechamodificacion;
        this.OperationsItems = ArtifactRecordViewModel.OperationsItems;
        this.Descripcion = ArtifactRecordViewModel.Descripcion;
        this.IsFromSPL = ArtifactRecordViewModel.IsFromSPL;


        //Properties Tab General

        this.Descripcion = ArtifactRecordViewModel.Descripcion;
        this.Phases = ArtifactRecordViewModel.Phases;
        this.CustomerName = ArtifactRecordViewModel.CustomerName;
        this.Frecuency = ArtifactRecordViewModel.Frecuency;
        this.PoNumeric = ArtifactRecordViewModel.PoNumeric;
        this.AltitudeF1 = ArtifactRecordViewModel.AltitudeF1;
        this.AltitudeF2 = ArtifactRecordViewModel.AltitudeF2;
        this.Typetrafoid = ArtifactRecordViewModel.Typetrafoid;
        this.TipoUnidad = ArtifactRecordViewModel.TipoUnidad;
        this.ApplicationId = ArtifactRecordViewModel.ApplicationId;
        this.StandardId = ArtifactRecordViewModel.StandardId;
        this.Norma = ArtifactRecordViewModel.Norma;
        this.NormaEquivalente = ArtifactRecordViewModel.NormaEquivalente;
        this.LanguageId = ArtifactRecordViewModel.LanguageId;
        this.ClaveIdioma = ArtifactRecordViewModel.ClaveIdioma;
        this.PolarityId = ArtifactRecordViewModel.PolarityId;
        this.PolarityOther = ArtifactRecordViewModel.PolarityOther;
        this.DesplazamientoAngular = ArtifactRecordViewModel.DesplazamientoAngular;
        this.OilBrand = ArtifactRecordViewModel.OilBrand;
        this.OilType = ArtifactRecordViewModel.OilType;

        //Properties Tab Characteristics

        this.CharacteristicsArtifacts = ArtifactRecordViewModel.CharacteristicsArtifacts;
        this.TensionKvAltaTension1 = ArtifactRecordViewModel.TensionKvAltaTension1;
        this.TensionKvAltaTension2 = ArtifactRecordViewModel.TensionKvAltaTension2;
        this.TensionKvAltaTension3 = ArtifactRecordViewModel.TensionKvAltaTension3;
        this.TensionKvAltaTension4 = ArtifactRecordViewModel.TensionKvAltaTension4;
        this.TensionKvBajaTension1 = ArtifactRecordViewModel.TensionKvBajaTension1;
        this.TensionKvBajaTension2 = ArtifactRecordViewModel.TensionKvBajaTension2;
        this.TensionKvBajaTension3 = ArtifactRecordViewModel.TensionKvBajaTension3;
        this.TensionKvBajaTension4 = ArtifactRecordViewModel.TensionKvBajaTension4;
        this.TensionKvSegundaBaja1 = ArtifactRecordViewModel.TensionKvSegundaBaja1;
        this.TensionKvSegundaBaja2 = ArtifactRecordViewModel.TensionKvSegundaBaja2;
        this.TensionKvSegundaBaja3 = ArtifactRecordViewModel.TensionKvSegundaBaja3;
        this.TensionKvSegundaBaja4 = ArtifactRecordViewModel.TensionKvSegundaBaja4;
        this.TensionKvTerciario1 = ArtifactRecordViewModel.TensionKvTerciario1;
        this.TensionKvTerciario2 = ArtifactRecordViewModel.TensionKvTerciario2;
        this.TensionKvTerciario3 = ArtifactRecordViewModel.TensionKvTerciario3;
        this.TensionKvTerciario4 = ArtifactRecordViewModel.TensionKvTerciario4;
        this.NbaiAltaTension = ArtifactRecordViewModel.NbaiAltaTension;
        this.NbaiBajaTension = ArtifactRecordViewModel.NbaiBajaTension;
        this.NbaiSegundaBaja = ArtifactRecordViewModel.NbaiSegundaBaja;
        this.NabaiTercera = ArtifactRecordViewModel.NabaiTercera;
        this.IdConexionAltaTension = ArtifactRecordViewModel.IdConexionAltaTension;
        this.ConexionAltaTension = ArtifactRecordViewModel.ConexionAltaTension;
        this.OtraConexionAltaTension = ArtifactRecordViewModel.OtraConexionAltaTension;
        this.IdConexionBajaTension = ArtifactRecordViewModel.IdConexionBajaTension;
        this.ConexionBajaTension = ArtifactRecordViewModel.ConexionBajaTension;
        this.OtraConexionBajaTension = ArtifactRecordViewModel.OtraConexionBajaTension;
        this.IdConexionSegundaBaja = ArtifactRecordViewModel.IdConexionSegundaBaja;
        this.ConexionSegundaBaja = ArtifactRecordViewModel.ConexionSegundaBaja;
        this.OtraConexionSegundaBaja = ArtifactRecordViewModel.OtraConexionSegundaBaja;
        this.IdConexionTercera = ArtifactRecordViewModel.IdConexionTercera;
        this.ConexionTercera = ArtifactRecordViewModel.ConexionTercera;
        this.OtraConexionTercera = ArtifactRecordViewModel.OtraConexionTercera;
        this.TipoDerivacionAltaTension = ArtifactRecordViewModel.TipoDerivacionAltaTension;
        this.ValorDerivacionUpAltaTension = ArtifactRecordViewModel.ValorDerivacionUpAltaTension;
        this.ValorDerivacionDownAltaTension = ArtifactRecordViewModel.ValorDerivacionDownAltaTension;
        this.TipoDerivacionAltaTension_2 = ArtifactRecordViewModel.TipoDerivacionAltaTension_2;
        this.ValorDerivacionUpAltaTension_2 = ArtifactRecordViewModel.ValorDerivacionUpAltaTension_2;
        this.ValorDerivacionDownAltaTension_2 = ArtifactRecordViewModel.ValorDerivacionDownAltaTension_2;
        this.TipoDerivacionBajaTension = ArtifactRecordViewModel.TipoDerivacionBajaTension;
        this.ValorDerivacionUpBajaTension = ArtifactRecordViewModel.ValorDerivacionUpBajaTension;
        this.ValorDerivacionDownBajaTension = ArtifactRecordViewModel.ValorDerivacionDownBajaTension;
        this.TipoDerivacionSegundaTension = ArtifactRecordViewModel.TipoDerivacionSegundaTension;
        this.ValorDerivacionUpSegundaTension = ArtifactRecordViewModel.ValorDerivacionUpSegundaTension;
        this.ValorDerivacionDownSegundaTension = ArtifactRecordViewModel.ValorDerivacionDownSegundaTension;
        this.TipoDerivacionTerceraTension = ArtifactRecordViewModel.TipoDerivacionTerceraTension;
        this.ValorDerivacionUpTerceraTension = ArtifactRecordViewModel.ValorDerivacionUpTerceraTension;
        this.ValorDerivacionDownTerceraTension = ArtifactRecordViewModel.ValorDerivacionDownTerceraTension;
        this.ConexionEquivalente = ArtifactRecordViewModel.ConexionEquivalente;
        this.ConexionEquivalente_2 = ArtifactRecordViewModel.ConexionEquivalente_2;
        this.ConexionEquivalente_3 = ArtifactRecordViewModel.ConexionEquivalente_3;
        this.ConexionEquivalente_4 = ArtifactRecordViewModel.ConexionEquivalente_4;
        this.IdConexionEquivalente = ArtifactRecordViewModel.IdConexionEquivalente;
        this.IdConexionEquivalente2 = ArtifactRecordViewModel.IdConexionEquivalente2;
        this.IdConexionEquivalente3 = ArtifactRecordViewModel.IdConexionEquivalente3;
        this.IdConexionEquivalente4 = ArtifactRecordViewModel.IdConexionEquivalente4;
        this.TapsAltaTension = ArtifactRecordViewModel.TapsAltaTension;
        this.TapsAltaTension_2 = ArtifactRecordViewModel.TapsAltaTension_2;
        this.TapsBajaTension = ArtifactRecordViewModel.TapsBajaTension;
        this.TapsSegundaBaja = ArtifactRecordViewModel.TapsSegundaBaja;
        this.TapsTerciario = ArtifactRecordViewModel.TapsTerciario;
        this.ValorNbaiNeutroAltaTension = ArtifactRecordViewModel.ValorNbaiNeutroAltaTension;
        this.ValorNbaiNeutroBajaTension = ArtifactRecordViewModel.ValorNbaiNeutroBajaTension;
        this.ValorNbaiNeutroSegundaBaja = ArtifactRecordViewModel.ValorNbaiNeutroSegundaBaja;
        this.ValorNbaiNeutroTercera = ArtifactRecordViewModel.ValorNbaiNeutroTercera;

        //Properties Tab Warranties

        this.Iexc100 = ArtifactRecordViewModel.Iexc100;
        this.Iexc110 = ArtifactRecordViewModel.Iexc110;
        this.Kwfe100 = ArtifactRecordViewModel.Kwfe100;
        this.Kwfe110 = ArtifactRecordViewModel.Kwfe110;
        this.TolerancyKwfe = ArtifactRecordViewModel.TolerancyKwfe;
        this.KwcuMva = ArtifactRecordViewModel.KwcuMva;
        this.KwcuKv = ArtifactRecordViewModel.KwcuKv;
        this.Kwcu = ArtifactRecordViewModel.Kwcu;
        this.TolerancyKwCu = ArtifactRecordViewModel.TolerancyKwCu;
        this.Kwaux3 = ArtifactRecordViewModel.Kwaux3;
        this.Kwaux4 = ArtifactRecordViewModel.Kwaux4;
        this.Kwaux1 = ArtifactRecordViewModel.Kwaux1;
        this.Kwaux2 = ArtifactRecordViewModel.Kwaux2;
        this.TolerancyKwAux = ArtifactRecordViewModel.TolerancyKwAux;
        this.Kwtot100 = ArtifactRecordViewModel.Kwtot100;
        this.Kwtot110 = ArtifactRecordViewModel.Kwtot110;
        this.TolerancyKwtot = ArtifactRecordViewModel.TolerancyKwtot;
        this.ZPositiveMva = ArtifactRecordViewModel.ZPositiveMva;
        this.ZPositiveHx = ArtifactRecordViewModel.ZPositiveHx;
        this.ZPositiveHy = ArtifactRecordViewModel.ZPositiveHy;
        this.ZPositiveXy = ArtifactRecordViewModel.ZPositiveXy;
        this.TolerancyZpositive = ArtifactRecordViewModel.TolerancyZpositive;
        this.TolerancyZpositive2 = ArtifactRecordViewModel.TolerancyZpositive2;
        this.NoiseOa = ArtifactRecordViewModel.NoiseOa;
        this.NoiseFa1 = ArtifactRecordViewModel.NoiseFa1;
        this.NoiseFa2 = ArtifactRecordViewModel.NoiseFa2;

        //Properties Tab Nozzles

        this.NozzlesArtifacts = ArtifactRecordViewModel.NozzlesArtifacts;

        //Properties Tab LightningRod

        this.LightningRodArtifacts = ArtifactRecordViewModel.LightningRodArtifacts;

        //Properties Tab Changes

        this.ChangingTablesArtifacs = ArtifactRecordViewModel.ChangingTablesArtifacs;

        //this.TapBaan.map(ArtifactRecordViewModel.TapBaan);

        this.ComboNumericScInput = ArtifactRecordViewModel.ComboNumericSc;
        this.CantidadSupScInput = ArtifactRecordViewModel.CantidadSupSc;
        this.PorcentajeSupScInput = ArtifactRecordViewModel.PorcentajeSupSc;
        this.CantidadInfScInput = ArtifactRecordViewModel.CantidadInfSc;
        this.PorcentajeInfScInput = ArtifactRecordViewModel.PorcentajeInfSc;
        this.NominalScInput = ArtifactRecordViewModel.NominalSc;
        this.IdentificacionScInput = ArtifactRecordViewModel.IdentificacionSc;
        this.InvertidoSc_ValueInput = ArtifactRecordViewModel.InvertidoSc_Value;

        this.ComboNumericBcInput = ArtifactRecordViewModel.ComboNumericBc;
        this.CantidadSupBcInput = ArtifactRecordViewModel.CantidadSupBc;
        this.PorcentajeSupBcInput = ArtifactRecordViewModel.PorcentajeSupBc;
        this.CantidadInfBcInput = ArtifactRecordViewModel.CantidadInfBc;
        this.PorcentajeInfBcInput = ArtifactRecordViewModel.PorcentajeInfBc;
        this.NominalBcInput = ArtifactRecordViewModel.NominalBc;
        this.IdentificacionBcInput = ArtifactRecordViewModel.IdentificacionBc;
        this.InvertidoBc_ValueInput = ArtifactRecordViewModel.InvertidoBc_Value;

        //Properties Test Labs
        this.LabTestsArtifact.map(ArtifactRecordViewModel.LabTestsArtifact);

        //Properties norms

        this.RulesArtifacts = ArtifactRecordViewModel.RulesArtifacts;
    }
}

var colorTabsSelect = "#80808026";


$("#NoSerie").focus();



$("#NoSerie").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $("#btnRequest").click();
    }
});

$("#GENERAL").css({ "background": colorTabsSelect });

$('.k-link').attr('style', 'padding: 10px 15px;text-transform: none !important;font-weight: 600; ')
$('.k-icon').attr('style', 'width: 1.5rem')






let artifactViewModel; //= new ArtifactRecordViewModel();

//btn
let btnRequest = document.getElementById("btnRequest");
let btnSave = document.getElementById("btnSave");
let btnLoadDataSidco = document.getElementById("btnLoadDataSidco");
let btnClear = document.getElementById("btnClear");

let btnAddNorm = document.getElementById("btnAddNorm");
let btnClearNorm = document.getElementById("btnClearNorm");

//inputs
let noSerieInput = document.getElementById("NoSerie");
let descripcionInput = document.getElementById("Descripcion");
let customerNameInput = document.getElementById("CustomerName");
let poNumericInput = document.getElementById("PoNumeric");
let typetrafoidInput = document.getElementById("Typetrafoid");
let applicationIdInput = document.getElementById("ApplicationId");
let standardIdInput = document.getElementById("StandardId");
let normaInput = document.getElementById("Norma");
let languageIdInput = document.getElementById("LanguageId");
let phasesInput = document.getElementById("Phases");
let frecuencyInput = document.getElementById("Frecuency");
let altitudeF1Input = document.getElementById("AltitudeF1");
let altitudeF2Input = document.getElementById("AltitudeF2");
let tipoUnidadInput = document.getElementById("TipoUnidad");
let normaEquivalenteInput = document.getElementById("NormaEquivalente");
let claveIdiomaInput = document.getElementById("ClaveIdioma");
let polarityIdInput = document.getElementById("PolarityId");
let polarityOtherInput = document.getElementById("PolarityOther");
let desplazamientoAngularInput = document.getElementById("DesplazamientoAngular");


let oilBrandInput = document.getElementById("OilBrand");
let oilTypeInput = document.getElementById("OilType");

let tensionKvAltaTension1Input = document.getElementById("TensionKvAltaTension1");
let tensionKvAltaTension2Input = document.getElementById("TensionKvAltaTension2");
let tensionKvAltaTension3Input = document.getElementById("TensionKvAltaTension3");
let tensionKvAltaTension4Input = document.getElementById("TensionKvAltaTension4");

let tensionKvBajaTension1Input = document.getElementById("TensionKvBajaTension1");
let tensionKvBajaTension2Input = document.getElementById("TensionKvBajaTension2");
let tensionKvBajaTension3Input = document.getElementById("TensionKvBajaTension3");
let tensionKvBajaTension4Input = document.getElementById("TensionKvBajaTension4");

let tensionKvSegundaBaja1Input = document.getElementById("TensionKvSegundaBaja1");
let tensionKvSegundaBaja2Input = document.getElementById("TensionKvSegundaBaja2");
let tensionKvSegundaBaja3Input = document.getElementById("TensionKvSegundaBaja3");
let tensionKvSegundaBaja4Input = document.getElementById("TensionKvSegundaBaja4");

let tensionKvTerciario1Input = document.getElementById("TensionKvTerciario1");
let tensionKvTerciario2Input = document.getElementById("TensionKvTerciario2");
let tensionKvTerciario3Input = document.getElementById("TensionKvTerciario3");
let tensionKvTerciario4Input = document.getElementById("TensionKvTerciario4");

let nbaiAltaTensionInput = document.getElementById("NbaiAltaTension");
let nbaiBajaTensionInput = document.getElementById("NbaiBajaTension");
let nbaiSegundaBajaInput = document.getElementById("NbaiSegundaBaja");
let nabaiTerceraInput = document.getElementById("NabaiTercera");

let ConexionAltaTensionInput = document.getElementById("ConexionAltaTension");
let ConexionBajaTensionInput = document.getElementById("ConexionBajaTension");
let ConexionSegundaBajaInput = document.getElementById("ConexionSegundaBaja");
let ConexionTerceraInput = document.getElementById("ConexionTercera");

let IdConexionAltaTensionInput = document.getElementById("IdConexionAltaTension");
let IdConexionBajaTensionInput = document.getElementById("IdConexionBajaTension");
let IdConexionSegundaBajaInput = document.getElementById("IdConexionSegundaBaja");
let IdConexionTerceraInput = document.getElementById("IdConexionTercera");

let otraConexionAltaTensionInput = document.getElementById("OtraConexionAltaTension");
let otraConexionBajaTensionInput = document.getElementById("OtraConexionBajaTension");
let otraConexionSegundaBajaInput = document.getElementById("OtraConexionSegundaBaja");
let otraConexionTerceraInput = document.getElementById("OtraConexionTercera");

let conexionEquivalenteInput = document.getElementById("ConexionEquivalente");
let conexionEquivalente_2Input = document.getElementById("ConexionEquivalente_2");
let conexionEquivalente_3Input = document.getElementById("ConexionEquivalente_3");
let conexionEquivalente_4Input = document.getElementById("ConexionEquivalente_4");

let tipoDerivacionAltaTensionInput = document.getElementById("TipoDerivacionAltaTension");
let valorDerivacionUpAltaTensionInput = document.getElementById("ValorDerivacionUpAltaTension");
let valorDerivacionDownAltaTensionInput = document.getElementById("ValorDerivacionDownAltaTension");

let tipoDerivacionBajaTensionInput = document.getElementById("TipoDerivacionBajaTension");
let valorDerivacionUpBajaTensionInput = document.getElementById("ValorDerivacionUpBajaTension");
let valorDerivacionDownBajaTensionInput = document.getElementById("ValorDerivacionDownBajaTension");

let tipoDerivacionSegundaTensionInput = document.getElementById("TipoDerivacionSegundaTension");
let valorDerivacionUpSegundaTensionInput = document.getElementById("ValorDerivacionUpSegundaTension");
let valorDerivacionDownSegundaTensionInput = document.getElementById("ValorDerivacionDownSegundaTension");

let tipoDerivacionTerceraTensionInput = document.getElementById("TipoDerivacionTerceraTension");
let valorDerivacionUpTerceraTensionInput = document.getElementById("ValorDerivacionUpTerceraTension");
let valorDerivacionDownTerceraTensionInput = document.getElementById("ValorDerivacionDownTerceraTension");

let tapsAltaTensionInput = document.getElementById("TapsAltaTension");
let tapsBajaTensionInput = document.getElementById("TapsBajaTension");
let tapsSegundaBajaInput = document.getElementById("TapsSegundaBaja");
let tapsTerciarioInput = document.getElementById("TapsTerciario");

let valorNbaiNeutroAltaTensionInput = document.getElementById("ValorNbaiNeutroAltaTension");
let valorNbaiNeutroBajaTensionInput = document.getElementById("ValorNbaiNeutroBajaTension");
let valorNbaiNeutroSegundaBajaInput = document.getElementById("ValorNbaiNeutroSegundaBaja");
let valorNbaiNeutroTerceraInput = document.getElementById("ValorNbaiNeutroTercera");


let Iexc100Input = document.getElementById("Iexc100");
let Kwfe100Input = document.getElementById("Kwfe100");
let KwcuInput = document.getElementById("Kwcu");
let Kwaux2Input = document.getElementById("Kwaux2");
let Kwtot100Input = document.getElementById("Kwtot100");
let ZPositiveHxInput = document.getElementById("ZPositiveHx");

let Iexc110Input = document.getElementById("Iexc110");
let Kwfe110Input = document.getElementById("Kwfe110");
let KwcuMvaInput = document.getElementById("KwcuMva");
let Kwaux4Input = document.getElementById("Kwaux4");
let Kwtot110Input = document.getElementById("Kwtot110");
let ZPositiveMvaInput = document.getElementById("ZPositiveMva");

let CoolingTypesInput = document.getElementById("CoolingTypes");

let TolerancyKwfeInput = document.getElementById("TolerancyKwfe");
let KwcuKvInput = document.getElementById("KwcuKv");
let TolerancyKwCuInput = document.getElementById("TolerancyKwCu");
let Kwaux1Input = document.getElementById("Kwaux1");
let Kwaux3Input = document.getElementById("Kwaux3");
let TolerancyKwAuxInput = document.getElementById("TolerancyKwAux");
let TolerancyKwtotInput = document.getElementById("TolerancyKwtot");
let ZPositiveHyInput = document.getElementById("ZPositiveHy");
let ZPositiveXyInput = document.getElementById("ZPositiveXy");
let TolerancyZpositiveInput = document.getElementById("TolerancyZpositive");
let TolerancyZpositive2Input = document.getElementById("TolerancyZpositive2");
let NoiseOaInput = document.getElementById("NoiseOa");
let NoiseFa1Input = document.getElementById("NoiseFa1");
let NoiseFa2Input = document.getElementById("NoiseFa2");


let ComboNumericScInput = document.getElementById("ComboNumericSc");
let CantidadSupScInput = document.getElementById("CantidadSupSc");
let PorcentajeSupScInput = document.getElementById("PorcentajeSupSc");
let CantidadInfScInput = document.getElementById("CantidadInfSc");
let PorcentajeInfScInput = document.getElementById("PorcentajeInfSc");
let NominalScInput = document.getElementById("NominalSc");
let IdentificacionScInput = document.getElementById("IdentificacionSc");
let InvertidoSc_ValueInput = document.getElementById("InvertidoSc");

let ComboNumericBcInput = document.getElementById("ComboNumericBc");
let CantidadSupBcInput = document.getElementById("CantidadSupBc");
let PorcentajeSupBcInput = document.getElementById("PorcentajeSupBc");
let CantidadInfBcInput = document.getElementById("CantidadInfBc");
let PorcentajeInfBcInput = document.getElementById("PorcentajeInfBc");
let NominalBcInput = document.getElementById("NominalBc");
let IdentificacionBcInput = document.getElementById("IdentificacionBc");
let InvertidoBc_ValueInput = document.getElementById("InvertidoBc");

let LabTestsArtifact_TextTestRoutineInput = document.getElementById("LabTestsArtifact_TextTestRoutine");
let LabTestsArtifact_TextTestDielectricInput = document.getElementById("LabTestsArtifact_TextTestDielectric");
let LabTestsArtifact_TextTestPrototypeInput = document.getElementById("LabTestsArtifact_TextTestPrototype");

let newNormaInput = document.getElementById("newNorma");

let tableNorma = $("#tableNorma").DataTable({

    "language": setSpanishTable(),

    select: {
        style: 'single'
    },
    processing: false,
    serverSide: false,
    searching: false,
    ordering: true,
    paging: true,
    "lengthChange": false,
    "columnDefs": [{
        className: "text-center",
        "targets": 1,
        "data": null,
        "defaultContent": '<button type="button" class="btn btn-danger"><i class="bi-trash" style="font-size: 1.3rem;"></i></button>'
    }]

});

let positionIdInput = document.getElementById("positionId");
let invertidoCkInput = document.getElementById("invertidoCk");
let identificationChangesInput = document.getElementById("identificationChanges");
let positionTAInput = document.getElementById("positionId");
let tapScBtnInput = document.getElementById("tapScBtn");
let tapBcBtnInput = document.getElementById("tapBcBtn");
let changerValueTableBodyInput = document.getElementById("changerValueTableBody");




function validations(generalData, charanterisData, garantiesData, boquillasData, apartarrayosData, cambiadoresData, pruebasLabData, rulesData) {

    var bandera = true;


    if (generalData) {
        /* ***************General*/
        var validator_general_data = $("#form_general_data").data("kendoValidator");

        if (!validator_general_data.validate()) {
            bandera = false;
            document.getElementById('GENERAL').style.background = 'red';
        }
    }

    if (charanterisData) {
        /* ***************Characteris*/


        for (var i = 0; i < artifactViewModel.CharacteristicsArtifacts.length; i++) {

            var getValueColingType = $(`#CharacteristicsArtifacts_${i}__CoolingType`).val();
            var getValueOrderElevation = $(`#CharacteristicsArtifacts_${i}__OverElevation`).val();
            var getValueMvaf1 = $(`#CharacteristicsArtifacts_${i}__Mvaf1`).val();

            if (getValueColingType == null || getValueColingType == "" || getValueColingType == undefined) {
                bandera = false;

                $(`#CharacteristicsArtifacts_${i}__CoolingTypeSpan`).text("Requerido");
                document.getElementById('CHARACTERISTIC').style.background = 'red';
            }
            else {

                $(`#CharacteristicsArtifacts_${i}__CoolingTypeSpan`).text("");
            }


            if (getValueOrderElevation == null || getValueOrderElevation == "" || getValueOrderElevation == undefined) {
                bandera = false;
                $(`#CharacteristicsArtifacts_${i}__OverElevationSpan`).text("Requerido");
                document.getElementById('CHARACTERISTIC').style.background = 'red';
            }
            else {

                $(`#CharacteristicsArtifacts_${i}__OverElevationSpan`).text("");
            }


            if (getValueMvaf1 == null || getValueMvaf1 == "" || getValueMvaf1 == undefined) {
                bandera = false;
                $(`#CharacteristicsArtifacts_${i}__Mvaf1Span`).text("Requerido");
                document.getElementById('CHARACTERISTIC').style.background = 'red';
            }
            else {

                $(`#CharacteristicsArtifacts_${i}__Mvaf1Span`).text("");
            }


        };


        if ($("#IdConexionAltaTension").val() == 0 && (CheckConnectionOfCharacteristic("ALTA_TENSION"))) {
            $(`#IdConexionAltaTensionSpan`).text("Requerido");
            bandera = false;
            document.getElementById('CHARACTERISTIC').style.background = 'red';
        }
        else
            $(`#IdConexionAltaTensionSpan`).text("");

        if ($("#IdConexionBajaTension").val() == 0 && (CheckConnectionOfCharacteristic("BAJA_TENSION"))) {
            $(`#IdConexionBajaTensionSpan`).text("Requerido");
            bandera = false;
            document.getElementById('CHARACTERISTIC').style.background = 'red';
        }
        else $(`#IdConexionBajaTensionSpan`).text("");

        if ($("#IdConexionSegundaBaja").val() == 0 && (CheckConnectionOfCharacteristic("2BAJA_TENSION"))) {
            $(`#IdConexionSegundaBajaSpan`).text("Requerido");
            bandera = false;
            document.getElementById('CHARACTERISTIC').style.background = 'red';
        }
        else $(`#IdConexionSegundaBajaSpan`).text("");

        if ($("#IdConexionTercera").val() == 0 && (CheckConnectionOfCharacteristic("TERCIARIO"))) {
            $(`#IdConexionTerceraSpan`).text("Requerido");
            bandera = false;
            document.getElementById('CHARACTERISTIC').style.background = 'red';
        }
        else $(`#IdConexionTerceraSpan`).text("");



        var validator_characteris_data = $("#form_characteris_data").data("kendoValidator");
        if (!validator_characteris_data.validate()) {
            bandera = false;
            document.getElementById('CHARACTERISTIC').style.background = 'red';
        }

    }


    if (garantiesData) {
        //******garantias

        var validator_garanties_data = $("#form_garanties_data").data("kendoValidator");

        if (!validator_garanties_data.validate()) {
            bandera = false;
            document.getElementById('WARANTIES').style.background = 'red';
        }
    }

    if (boquillasData) {

        //******boquillas

        var validator_boq_data = $("#form_boq_data").data("kendoValidator");

        if (!validator_boq_data.validate()) {
            bandera = false;
            document.getElementById('NOZZLESS').style.background = 'red';
        }
    }




    if (apartarrayosData) {
        //******apartarrayos

        var validator_apart_data = $("#form_apart_data").data("kendoValidator");

        if (!validator_apart_data.validate()) {
            bandera = false;
            document.getElementById('LIGHTNING_ROD').style.background = 'red';
        }
    }


    if (cambiadoresData) {
        /* ***************cambiadores*/
        var validator_cambiadores_data = $("#form_cambiadores_data").data("kendoValidator");
        if (!validator_cambiadores_data.validate()) {
            bandera = false;
            document.getElementById('CHANGENS').style.background = 'red';
        }
    }

    if (pruebasLabData) {
        /* ***************preubas lab*/
        var validator_pruebaslab_data = $("#form_pruebaslab_data").data("kendoValidator");
        if (!validator_pruebaslab_data.validate()) {
            bandera = false;
            document.getElementById('TEST_LABS').style.background = 'red';
        }
    }

    if (rulesData) {

        /* ***************Normas*/
        if (tableNorma.data().count() <= 0) {
            ShowWarningMessage("Debe agregar al menos una norma.");
            bandera = false;
            document.getElementById('NORMS').style.background = 'red';
        }


    }

    return bandera;
}


//event

/*
let positionIdInput = document.getElementById("positionId");
let invertidoCkInput = document.getElementById("invertidoCk");
let identificationChangesInput = document.getElementById("identificationChanges");
let positionTAInput = document.getElementById("positionId");
 */

tapScBtnInput.addEventListener("click", function () {
    GetChangersJSON(CantidadSupScInput.value, CantidadInfScInput.value, NominalScInput.value, IdentificacionSc.value, InvertidoSc_ValueInput.checked).then(
        data => {
            if (data !== null && data !== undefined) {

                positionIdInput.value = data.response.Position;
                invertidoCkInput.checked = data.response.Reversed;
                identificationChangesInput.value = data.response.Identification;

                document.getElementById("changerValueTableBody").innerHTML = '';

                data.response.Values.forEach(function (model, key) {
                    let content = '';

                    if (model.Selected) {
                        content =
                            `<tr>
                                            <th class="table-primary">${model.Value}</th>
                                        </tr>`;
                    }
                    else {
                        content = `<tr>
                                            <th scope="row">${model.Value}</th>
                                        </tr>`;
                    }
                    let list = document.getElementById("changerValueTableBody");

                    list.innerHTML += content;
                });
            }
        }
    );
});

tapBcBtnInput.addEventListener("click", function () {
    GetChangersJSON(CantidadSupBcInput.value, CantidadInfBcInput.value, NominalBcInput.value, IdentificacionBc.value, InvertidoBc_ValueInput.checked).then(
        data => {
            if (data !== null && data !== undefined) {

                positionIdInput.value = data.response.Position;
                invertidoCkInput.checked = data.response.Reversed;
                identificationChangesInput.value = data.response.Identification;

                document.getElementById("changerValueTableBody").innerHTML = '';
                data.response.Values.forEach(function (model, key) {
                    let content = '';

                    if (model.Selected) {
                        content =
                            `<tr>
                                            <th class="table-primary">${model.Value}</th>
                                        </tr>`;
                    }
                    else {
                        content = `<tr>
                                            <th scope="row">${model.Value}</th>
                                        </tr>`;
                    }
                    let list = document.getElementById("changerValueTableBody");

                    list.innerHTML += content;
                });
            }
        }
    );
});

btnSave.addEventListener("click", function () {
    $("#loader").css("display", "block");
    var ResultValidations = true;
    if (artifactViewModel.IsFromSPL) {
        artifactViewModel.Update = $("#tabstrip").data("kendoTabStrip").select()[0].id;
        if (artifactViewModel.Update == "GENERAL") {
            ResultValidations = validations(true, false, false, false, false, false, false, false);
            if (!ResultValidations)
                ShowFailedMessage('Faltan campos requeridos en Generales');
        }
        else if (artifactViewModel.Update == "CHARACTERISTIC") {
            ResultValidations = validations(false, true, false, false, false, false, false, false);
            if (!ResultValidations)
                ShowFailedMessage('Faltan campos requeridos en Características');
        }
        else if (artifactViewModel.Update == "WARANTIES") {
            ResultValidations = validations(false, false, true, false, false, false, false, false);
            if (!ResultValidations)
                ShowFailedMessage('Faltan campos requeridos en Garantías');
        }
        else if (artifactViewModel.Update == "NOZZLESS") {
            ResultValidations = validations(false, false, false, true, false, false, false, false);
            if (!ResultValidations)
                ShowFailedMessage('Faltan campos requeridos en Boquillas');
        }
        else if (artifactViewModel.Update == "LIGHTNING_ROD") {
            ResultValidations = validations(false, false, false, false, true, false, false, false);
            if (!ResultValidations)
                ShowFailedMessage('Faltan campos requeridos en Apartarayos');
        }
        else if (artifactViewModel.Update == "CHANGENS") {
            ResultValidations = validations(false, false, false, false, false, true, false, false);
            if (!ResultValidations)
                ShowFailedMessage('Faltan campos requeridos en Cambiadores');
        }
        else if (artifactViewModel.Update == "TEST_LABS") {
            ResultValidations = validations(false, false, false, false, false, false, true, false);
            if (!ResultValidations)
                ShowFailedMessage('Faltan campos requeridos en Pruebas Labs');
        }
        else {
            ResultValidations = validations(false, false, false, false, false, false, false, true);
            if (!ResultValidations)
                ShowFailedMessage('Faltan campos requeridos en Normas');
        }
    }
    else {
        ResultValidations = validations(true, true, true, true, true, true, true, true);
        if (!ResultValidations)
            ShowFailedMessage('Faltan campos requeridos');
    }


    if (ComboNumericScInput.value != 6 && ComboNumericScInput.value != 0 && NominalScInput.value == 0) {
        ResultValidations = false;
        ShowFailedMessage('Error, debe agregar al menos una nominal para las posiciones sin carga en cambiadores');

    }
    if (ComboNumericBcInput.value != 6 && ComboNumericBcInput.value != 0 && NominalBcInput.value == 0) {
        ResultValidations = false;
        ShowFailedMessage('Error, debe agregar al menos una nominal para las posiciones bajo carga en cambiadores');
    }

    //validate nozzles
    var nozzlesInputs = document.querySelectorAll('.nozzlesInput');
    var totalBoquillas = 0;
    nozzlesInputs.forEach(input => {
        var nozzleValue = parseInt(input.value);
        totalBoquillas = totalBoquillas + nozzleValue;
    });

    if (totalBoquillas > 15) {
        ResultValidations = false;
        ShowFailedMessage('Error, cantidad de boquillas excede el máximo de 15, favor de corregirlas');
    }
    else if (totalBoquillas < 2) {
        ShowFailedMessage('Error, cantidad de boquillas no llega al mínimo de 2, favor de corregirlas');
        ResultValidations = false;
    }

    if (ResultValidations) {


      
        //Update all artifact or partial
        if (artifactViewModel.IsFromSPL) {
            artifactViewModel.Update = $("#tabstrip").data("kendoTabStrip").select()[0].id;

        }
        else
            artifactViewModel.Update = 'ALL';

        MapFormToViewModel();

        postData(domain + "/ArtifactRecord/Post/", artifactViewModel)
            .then(data => {
                if (data !== null && data.response === 200) {
                    if (artifactViewModel.IsFromSPL) {

                        ShowTabNameUpdated(artifactViewModel.Update);
                    }
                    else {
                        artifactViewModel.IsFromSPL = true;
                        ShowSuccessMessage('Diseño guardado en S.P.L.');
                        let lbIsFromSIDCO = document.getElementById("lbIsFromSIDCO");
                        lbIsFromSIDCO.style.display = "none";
                        let lbIsFromSPL = document.getElementById("lbIsFromSPL");
                        lbIsFromSPL.style.display = "block";
                    }
                }
                else {
                    ShowFailedMessage('Error, por favor contacte al administrador del sistema.');
                }
                $("#loader").css("display", "none");
            });

    }
    else { $("#loader").css("display", "none"); }

});


//event
btnClear.addEventListener("click", function () {

    clearAll();

});

function ShowTabNameUpdated(tabUpdated) {
    switch (tabUpdated) {
        case 'GENERAL':
            {
                ShowSuccessMessage('Actualizado Generales.');
                break;
            }
        case 'CHARACTERISTIC':
            {
                ShowSuccessMessage('Actualizado Características.');
                break;
            }
        case 'WARANTIES':
            {
                ShowSuccessMessage('Actualizado Garantías.');
                break;
            }
        case 'NOZZLESS':
            {
                ShowSuccessMessage('Actualizado Boquillas.');
                break;
            }
        case 'LIGHTNING_ROD':
            {
                ShowSuccessMessage('Actualizado Apartarayos.');
                break;
            }
        case 'CHANGENS':
            {
                ShowSuccessMessage('Actualizado Cambiadores.');
                break;
            }
        case 'TEST_LABS':
            {
                ShowSuccessMessage('Actualizado Pruebas de Laboratorio.');
                break;
            }
        case 'NORMS':
            {
                ShowSuccessMessage('Actualizado Normas.');
                break;
            }
        default:
            {
                ShowSuccessMessage('Diseño guardado.');
                break;
            }
    }
}

function callBackClear(arg) {
    noSerieInput.disabled = false;
    let lbIsFromSIDCO = document.getElementById("lbIsFromSIDCO");
    lbIsFromSIDCO.style.display = "none";
    let lbIsFromSPL = document.getElementById("lbIsFromSPL");
    lbIsFromSPL.style.display = "none";

    noSerieInput.value = '';

    descripcionInput.value = '';
    customerNameInput.value = '';
    poNumericInput.value = '';
    typetrafoidInput.value = null;
    applicationIdInput.value = null;
    standardIdInput.value = null;
    normaInput.value = '';
    languageIdInput.value = null;
    phasesInput.value = '';
    frecuencyInput.value = '';
    altitudeF1Input.value = '';
    altitudeF2Input.value = '';
    tipoUnidadInput.value = '';
    normaEquivalenteInput.value = '';
    claveIdiomaInput.value = '';
    polarityIdInput.value = null;
    polarityOtherInput.value = '';
    desplazamientoAngularInput.value = '';
    oilBrandInput.value = '';
    oilTypeInput.value = '';

    //CHARACTERISTICS
    let list = document.getElementById("CharacteristicList");
    list.innerHTML = '';


    tensionKvAltaTension1Input.value = '';
    tensionKvAltaTension2Input.value = '';
    tensionKvAltaTension3Input.value = '';
    tensionKvAltaTension4Input.value = '';

    tensionKvBajaTension1Input.value = '';
    tensionKvBajaTension2Input.value = '';
    tensionKvBajaTension3Input.value = '';
    tensionKvBajaTension4Input.value = '';

    tensionKvSegundaBaja1Input.value = '';
    tensionKvSegundaBaja2Input.value = '';
    tensionKvSegundaBaja3Input.value = '';
    tensionKvSegundaBaja4Input.value = '';

    tensionKvTerciario1Input.value = '';
    tensionKvTerciario2Input.value = '';
    tensionKvTerciario3Input.value = '';
    tensionKvTerciario4Input.value = '';

    nbaiAltaTensionInput.value = '';
    nbaiBajaTensionInput.value = '';
    nbaiSegundaBajaInput.value = '';
    nabaiTerceraInput.value = '';

    ConexionAltaTensionInput.value = '';
    ConexionBajaTensionInput.value = '';
    ConexionSegundaBajaInput.value = '';
    ConexionTerceraInput.value = '';

    IdConexionAltaTensionInput.value = '';
    IdConexionBajaTensionInput.value = '';
    IdConexionSegundaBajaInput.value = '';
    IdConexionTerceraInput.value = '';

    otraConexionAltaTensionInput.value = '';
    otraConexionBajaTensionInput.value = '';
    otraConexionSegundaBajaInput.value = '';
    otraConexionTerceraInput.value = '';

    conexionEquivalenteInput.value = '';
    conexionEquivalente_2Input.value = '';
    conexionEquivalente_3Input.value = '';
    conexionEquivalente_4Input.value = '';

    tipoDerivacionAltaTensionInput.value = '';
    valorDerivacionUpAltaTensionInput.value = '';
    valorDerivacionDownAltaTensionInput.value = '';

    tipoDerivacionBajaTensionInput.value = '';
    valorDerivacionUpBajaTensionInput.value = '';
    valorDerivacionDownBajaTensionInput.value = '';

    tipoDerivacionSegundaTensionInput.value = '';
    valorDerivacionUpSegundaTensionInput.value = '';
    valorDerivacionDownSegundaTensionInput.value = '';

    tipoDerivacionTerceraTensionInput.value = '';
    valorDerivacionUpTerceraTensionInput.value = '';
    valorDerivacionDownTerceraTensionInput.value = '';

    tapsAltaTensionInput.value = '';
    tapsBajaTensionInput.value = '';
    tapsSegundaBajaInput.value = '';
    tapsTerciarioInput.value = '';

    valorNbaiNeutroAltaTensionInput.value = '';
    valorNbaiNeutroBajaTensionInput.value = '';
    valorNbaiNeutroSegundaBajaInput.value = '';
    valorNbaiNeutroTerceraInput.value = '';

    //Waranties
    Iexc100Input.value = '';
    Kwfe100Input.value = '';
    KwcuInput.value = '';
    Kwaux2Input.value = '';
    Kwtot100Input.value = '';
    ZPositiveHxInput.value = '';

    Iexc110Input.value = '';
    Kwfe110Input.value = '';
    KwcuMvaInput.value = '';
    Kwaux4Input.value = '';
    Kwtot110Input.value = '';
    ZPositiveMvaInput.value = '';



    CoolingTypesInput.value = '';


    TolerancyKwfeInput.value = '';
    KwcuKvInput.value = '';
    TolerancyKwCuInput.value = '';
    Kwaux1Input.value = '';
    Kwaux3Input.value = '';
    TolerancyKwAuxInput.value = '';
    TolerancyKwtotInput.value = '';
    ZPositiveHyInput.value = '';
    ZPositiveXyInput.value = '';
    TolerancyZpositiveInput.value = '';
    TolerancyZpositive2Input.value = '';
    NoiseOaInput.value = '';
    NoiseFa1Input.value = '';
    NoiseFa2Input.value = '';

    //Nozzless

    let nozzlesList = document.getElementById("nozzlesList");
    nozzlesList.innerHTML = '';

    //lightningRod
    let lightningRodList = document.getElementById("lightningRodList");
    lightningRodList.innerHTML = '';

    //Changins
    let operationConditionTitle = document.getElementById("titleOperationConditionTable");
    operationConditionTitle.innerHTML = '';


    let rowConditionOperation = document.getElementById("rowConditionOperation");
    rowConditionOperation.innerHTML = '';

    let rowDerivations = document.getElementById("rowDerivations");
    rowDerivations.innerHTML = '';

    let rowSteps = document.getElementById("rowSteps");
    rowSteps.innerHTML = '';




    ComboNumericScInput.value = '';
    CantidadSupScInput.value = '';
    PorcentajeSupScInput.value = '';
    CantidadInfScInput.value = '';
    PorcentajeInfScInput.value = '';
    NominalScInput.value = '';
    IdentificacionScInput.value = '';
    InvertidoSc_ValueInput.value = '';

    ComboNumericBcInput.value = '';
    CantidadSupBcInput.value = '';
    PorcentajeSupBcInput.value = '';
    CantidadInfBcInput.value = '';
    PorcentajeInfBcInput.value = '';
    NominalBcInput.value = '';
    IdentificacionBcInput.value = '';
    InvertidoBc_ValueInput.value = '';



    LabTestsArtifact_TextTestRoutineInput.value = '';
    LabTestsArtifact_TextTestDielectricInput.value = '';
    LabTestsArtifact_TextTestPrototypeInput.value = '';



    this.RulesArtifacts = [];
    tableNorma.row($(this).parents('tr'))
        .remove()
        .draw();
    $('#tableNorma tbody > tr').remove();




    newNormaInput.value = '';





    btnSave.disabled = true;
    btnLoadDataSidco.disabled = true;
    $("#NoSerie").focus();
    //Norms
}


function clearAll() {

    var width, height = "150px";
    var imgUrl = "";
    var title = "Alerta"
    var text = "¿Está seguro de limpiar todos los campos?";

    callBackClear(text, callBackClear, width, height, null, title, imgUrl);

}

typetrafoidInput.addEventListener("change", function () {
    if (typetrafoidInput.value !== null && typetrafoidInput.value !== undefined && typetrafoidInput.value !== '') {
        MapFormToViewModel();
        GetTypeAfroidJSON(domain + "/ArtifactRecord/GetTypeAfroid/", artifactViewModel).then(
            data => {
                if (data !== null && data !== undefined) {
                    tipoUnidadInput.value = data.response;
                }
                $("#loader").css("display", "none");
            }
        );
    }
});

standardIdInput.addEventListener("change", function () {
    if (standardIdInput.value !== null && standardIdInput.value !== undefined && standardIdInput.value !== '') {
        MapFormToViewModel();
        GetTypeAfroidJSON(domain + "/ArtifactRecord/GetNorm/", artifactViewModel).then(
            data => {
                if (data !== null && data !== undefined) {
                    normaEquivalenteInput.value = data.response;
                }
                $("#loader").css("display", "none");
            }
        );
    }
});
polarityIdInput.addEventListener("change", function () {
    if (polarityIdInput.value !== null && polarityIdInput.value !== undefined && polarityIdInput.value !== '') {
        MapFormToViewModel();
        GetTypeAfroidJSON(domain + "/ArtifactRecord/GetDisplacementEquivalent/", artifactViewModel).then(
            data => {
                if (data !== null && data !== undefined) {
                    desplazamientoAngularInput.value = data.response;
                }
                $("#loader").css("display", "none");
            }
        );
    }
}); 


languageIdInput.addEventListener("change", function () {
    if (languageIdInput.value !== null && languageIdInput.value !== undefined && languageIdInput.value !== '') {

        switch (languageIdInput.value) {
            case '1323':
                claveIdiomaInput.value = 'ES';
                break;
            case '1324':
                claveIdiomaInput.value = 'EN';
                break;
            case '1325':
                claveIdiomaInput.value = 'BI';
                break;
            default:
                claveIdiomaInput.value = '';
                break;
        }
    }
});
btnRequest.addEventListener("click", function () {

    if ($("#NoSerie").val() == undefined || $("#NoSerie").val() == "" || $("#NoSerie").val() == null) {
        $(`#NoSerieSpand`).text("Requerido");
        return;
    }
    else {
        $(`#NoSerieSpand`).text("");
    }

    $("#loader").css("display", "block");
    if (noSerieInput.value) {
        this.RulesArtifacts = [];
        GetArtifactJSON().then(
            data => {
                if (data !== null && data !== undefined) {
                    //artifactViewModel.map(data.response);
                    artifactViewModel = data.response;
                    LoadForm(data.response);
                    btnSave.disabled = false;
                    btnLoadDataSidco.disabled = false;
                    noSerieInput.disabled = true;
                   
                }
                $("#loader").css("display", "none");
            }
        );
    }
    else {
        $("#loader").css("display", "none");

        ShowSuccessMessage('Por favor ingrese un No. Serie.');
        btnSave.disabled = true;
        btnLoadDataSidco.disabled = true;
    }
});
btnLoadDataSidco.addEventListener("click", function () {
    $("#loader").css("display", "block");
    if (noSerieInput.value) {

        GetArtifactJSON(true).then(
            data => {
                if (data !== null && data !== undefined) {
                    //artifactViewModel.map(data.response);
                    artifactViewModel = data.response;
                    LoadForm(data.response);
                    btnSave.disabled = false;
                    btnLoadDataSidco.disabled = false;
                }
                $("#loader").css("display", "none");
            }
        );
    }
    else {
        $("#loader").css("display", "none");

        btnSave.disabled = true;
        btnLoadDataSidco.disabled = true;
    }
});

btnAddNorm.addEventListener("click", function () {

    if (newNormaInput.value !== '') {

        tableNorma.row.add([
            newNormaInput.value
        ]).draw();
        newNormaInput.value = "";

    }
    else {

        console.log("FUCK");
    }

});

btnClearNorm.addEventListener("click", function () {

    newNormaInput.value = ""

});

$('#tableNorma tbody').on('click', 'button', function () {
    tableNorma.row($(this).parents('tr'))
        .remove()
        .draw();
});

//functions

function AddNorm(rulesArtifactsModel) {
    tableNorma.row.add([
        rulesArtifactsModel.Descripcion
    ]).draw();
    newNormaInput.value = ""
}

function CheckConnectionOfCharacteristic(columnTitle) {

    let validate = false;

    switch (columnTitle) {
        case "ALTA_TENSION":
            artifactViewModel.CharacteristicsArtifacts.forEach(function (characteristic, index) {
                if ($(`#CharacteristicsArtifacts_${index}__Mvaf1`).val() !== undefined && $(`#CharacteristicsArtifacts_${index}__Mvaf1`).val() !== null && !isNaN($(`#CharacteristicsArtifacts_${index}__Mvaf1`).val()) && parseFloat($(`#CharacteristicsArtifacts_${index}__Mvaf1`).val()) > 0)
                    validate = true;
            });
            break;
        case "BAJA_TENSION":
            artifactViewModel.CharacteristicsArtifacts.forEach(function (characteristic, index) {
                if ($(`#CharacteristicsArtifacts_${index}__Mvaf2`).val() !== undefined && $(`#CharacteristicsArtifacts_${index}__Mvaf2`).val() !== null && !isNaN($(`#CharacteristicsArtifacts_${index}__Mvaf2`).val()) && parseFloat($(`#CharacteristicsArtifacts_${index}__Mvaf2`).val()) > 0)
                    validate = true;
            });
            break;
        case "2BAJA_TENSION":
            artifactViewModel.CharacteristicsArtifacts.forEach(function (characteristic, index) {
                if ($(`#CharacteristicsArtifacts_${index}__Mvaf3`).val() !== undefined && $(`#CharacteristicsArtifacts_${index}__Mvaf3`).val() !== null && !isNaN($(`#CharacteristicsArtifacts_${index}__Mvaf3`).val()) && parseFloat($(`#CharacteristicsArtifacts_${index}__Mvaf3`).val()) > 0)
                    validate = true;
            });
            break;
        case "TERCIARIO":
            artifactViewModel.CharacteristicsArtifacts.forEach(function (characteristic, index) {
                if ($(`#CharacteristicsArtifacts_${index}__Mvaf4`).val() !== undefined && $(`#CharacteristicsArtifacts_${index}__Mvaf4`).val() !== null && !isNaN($(`#CharacteristicsArtifacts_${index}__Mvaf4`).val()) && parseFloat($(`#CharacteristicsArtifacts_${index}__Mvaf4`).val()) > 0)
                    validate = true;
            });
            break;
        default:
    }

    return validate;
}

async function GetChangersJSON(cantPos, cantNeg, nominal, identificationId, reversed) {

    if (parseFloat(cantPos) + parseFloat(cantNeg) + 1 >= 26 && identificationId === "1") {
        ShowWarningMessage('El número de posiciones no pueden ser mayor a 26 cuando el tipo de identificación es Letras.')
    }
    else {
        var path = '/ArtifactRecord/GetChangers/';

        var url = new URL(domain + path),
            params = {
                cantPos: cantPos,
                cantNeg: cantNeg,
                nominal: nominal,
                identificationId: identificationId,
                reversed: reversed
            }

        Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

        const response = await fetch(url);

        if (response.ok && response.status === 200) {

            const data = await response.json();
            return data;
        }

        else {

            return null;
        }
    }

}



async function GetArtifactJSON(loadDataSidco = false) {
    var path = '';
    if (loadDataSidco)
        path = "/ArtifactRecord/GetFromSidco/"; //load from sidco
    else
        path = "/ArtifactRecord/Get/"; //check if exist in spl, if not exist then load from sidco

    var url = new URL(domain + path),
        params = { noSerie: noSerieInput.value }
    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

    const response = await fetch(url);

    if (response.ok && response.status === 200) {

        const artifact = await response.json();
        ShowSuccessMessage('Consulta Exitosa.');
        return artifact;
    }

    else if (response.ok && response.status === 204) {
        ShowWarningMessage('Diseño no encontrado.');
        return null;
    }

    else {
        ShowFailedMessage('Error, por favor contacte al administrador del sistema.');
        return null;
    }
}


async function postData(url = '', data = {}) {
    // Opciones por defecto estan marcadas con un *
    const response = await fetch(url, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        mode: 'same-origin', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
            'Content-Type': 'application/json'
            // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'same-origin', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: JSON.stringify(data) // body data type must match "Content-Type" header
    });
    if (response.ok) {
        return response.json(); // parses JSON response into native JavaScript objects
    }
    else {
        return null
    }

}

async function GetTypeAfroidJSON(url = '', data = {}) {
    // Opciones por defecto estan marcadas con un *
    const response = await fetch(url, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        mode: 'same-origin', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
            'Content-Type': 'application/json'
            // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'same-origin', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: JSON.stringify(data) // body data type must match "Content-Type" header
    });
    if (response.ok) {
        return response.json(); // parses JSON response into native JavaScript objects
    }
    else {
        return null
    }
}



function MapFormToViewModel() {



    //Properties Tab General
    artifactViewModel.NoSerie = noSerieInput.value;
    artifactViewModel.Descripcion = descripcionInput.value;
    artifactViewModel.CustomerName = customerNameInput.value;
    artifactViewModel.PoNumeric = poNumericInput.value;
    artifactViewModel.Typetrafoid = typetrafoidInput.value;
    artifactViewModel.ApplicationId = applicationIdInput.value;
    artifactViewModel.StandardId = standardIdInput.value;
    artifactViewModel.Norma = normaInput.value;
    artifactViewModel.LanguageId = languageIdInput.value;
    artifactViewModel.Phases = phasesInput.value;
    artifactViewModel.Frecuency = frecuencyInput.value;
    artifactViewModel.AltitudeF1 = altitudeF1Input.value;
    artifactViewModel.AltitudeF2 = altitudeF2Input.value;
    artifactViewModel.TipoUnidad = tipoUnidadInput.value;
    artifactViewModel.NormaEquivalente = normaEquivalenteInput.value;
    artifactViewModel.ClaveIdioma = claveIdiomaInput.value;
    artifactViewModel.PolarityId = polarityIdInput.value;
    artifactViewModel.PolarityOther = polarityOtherInput.value;
    artifactViewModel.DesplazamientoAngular = desplazamientoAngularInput.value;
    artifactViewModel.OilBrand = oilBrandInput.value;
    artifactViewModel.OilType = oilTypeInput.value;


    //Properties Tab Characteristics

    GetCharacteristics(artifactViewModel.CharacteristicsArtifacts);

    artifactViewModel.TensionKvAltaTension1 = tensionKvAltaTension1Input.value;
    artifactViewModel.TensionKvAltaTension2 = tensionKvAltaTension2Input.value;
    artifactViewModel.TensionKvAltaTension3 = tensionKvAltaTension3Input.value;
    artifactViewModel.TensionKvAltaTension4 = tensionKvAltaTension4Input.value;

    artifactViewModel.TensionKvBajaTension1 = tensionKvBajaTension1Input.value;
    artifactViewModel.TensionKvBajaTension2 = tensionKvBajaTension2Input.value;
    artifactViewModel.TensionKvBajaTension3 = tensionKvBajaTension3Input.value;
    artifactViewModel.TensionKvBajaTension4 = tensionKvBajaTension4Input.value;

    artifactViewModel.TensionKvSegundaBaja1 = tensionKvSegundaBaja1Input.value;
    artifactViewModel.TensionKvSegundaBaja2 = tensionKvSegundaBaja2Input.value;
    artifactViewModel.TensionKvSegundaBaja3 = tensionKvSegundaBaja3Input.value;
    artifactViewModel.TensionKvSegundaBaja4 = tensionKvSegundaBaja4Input.value;

    artifactViewModel.TensionKvTerciario1 = tensionKvTerciario1Input.value;
    artifactViewModel.TensionKvTerciario2 = tensionKvTerciario2Input.value;
    artifactViewModel.TensionKvTerciario3 = tensionKvTerciario3Input.value;
    artifactViewModel.TensionKvTerciario4 = tensionKvTerciario4Input.value;

    artifactViewModel.NbaiAltaTension = nbaiAltaTensionInput.value;
    artifactViewModel.NbaiBajaTension = nbaiBajaTensionInput.value;
    artifactViewModel.NbaiSegundaBaja = nbaiSegundaBajaInput.value;
    artifactViewModel.NabaiTercera = nabaiTerceraInput.value;

    artifactViewModel.ConexionAltaTension = ConexionAltaTensionInput.value;
    artifactViewModel.ConexionBajaTension = ConexionBajaTensionInput.value;
    artifactViewModel.ConexionSegundaBaja = ConexionSegundaBajaInput.value;
    artifactViewModel.ConexionTercera = ConexionTerceraInput.value;

    artifactViewModel.IdConexionAltaTension = IdConexionAltaTensionInput.value === "" ? "0" : IdConexionAltaTensionInput.value;
    artifactViewModel.IdConexionBajaTension = IdConexionBajaTensionInput.value === "" ? "0" : IdConexionBajaTensionInput.value;
    artifactViewModel.IdConexionSegundaBaja = IdConexionSegundaBajaInput.value === "" ? "0" : IdConexionSegundaBajaInput.value;
    artifactViewModel.IdConexionTercera = IdConexionTerceraInput.value === "" ? "0" : IdConexionTerceraInput.value;

    artifactViewModel.OtraConexionAltaTension = otraConexionAltaTensionInput.value;
    artifactViewModel.OtraConexionBajaTension = otraConexionBajaTensionInput.value;
    artifactViewModel.OtraConexionSegundaBaja = otraConexionSegundaBajaInput.value;
    artifactViewModel.OtraConexionTercera = otraConexionTerceraInput.value;

    artifactViewModel.ConexionEquivalente = conexionEquivalenteInput.value;
    artifactViewModel.ConexionEquivalente_2 = conexionEquivalente_2Input.value;
    artifactViewModel.ConexionEquivalente_3 = conexionEquivalente_3Input.value;
    artifactViewModel.ConexionEquivalente_4 = conexionEquivalente_4Input.value;

    artifactViewModel.TipoDerivacionAltaTension = tipoDerivacionAltaTensionInput.value;
    artifactViewModel.ValorDerivacionUpAltaTension = valorDerivacionUpAltaTensionInput.value;
    artifactViewModel.ValorDerivacionDownAltaTension = valorDerivacionDownAltaTensionInput.value;

    artifactViewModel.TipoDerivacionBajaTension = tipoDerivacionBajaTensionInput.value;
    artifactViewModel.ValorDerivacionUpBajaTension = valorDerivacionUpBajaTensionInput.value;
    artifactViewModel.ValorDerivacionDownBajaTension = valorDerivacionDownBajaTensionInput.value;

    artifactViewModel.TipoDerivacionSegundaTension = tipoDerivacionSegundaTensionInput.value;
    artifactViewModel.ValorDerivacionUpSegundaTension = valorDerivacionUpSegundaTensionInput.value;
    artifactViewModel.ValorDerivacionDownSegundaTension = valorDerivacionDownSegundaTensionInput.value;

    artifactViewModel.TipoDerivacionTerceraTension = tipoDerivacionTerceraTensionInput.value;
    artifactViewModel.ValorDerivacionUpTerceraTension = valorDerivacionUpTerceraTensionInput.value;
    artifactViewModel.ValorDerivacionDownTerceraTension = valorDerivacionDownTerceraTensionInput.value;

    artifactViewModel.TapsAltaTension = tapsAltaTensionInput.value;
    artifactViewModel.TapsBajaTension = tapsBajaTensionInput.value;
    artifactViewModel.TapsSegundaBaja = tapsSegundaBajaInput.value;
    artifactViewModel.TapsTerciario = tapsTerciarioInput.value;

    artifactViewModel.ValorNbaiNeutroAltaTension = valorNbaiNeutroAltaTensionInput.value;
    artifactViewModel.ValorNbaiNeutroBajaTension = valorNbaiNeutroBajaTensionInput.value;
    artifactViewModel.ValorNbaiNeutroSegundaBaja = valorNbaiNeutroSegundaBajaInput.value;
    artifactViewModel.ValorNbaiNeutroTercera = valorNbaiNeutroTerceraInput.value;

    //Properties Tab Warranties

    artifactViewModel.Iexc100 = Iexc100Input.value;
    artifactViewModel.Iexc110 = Iexc110Input.value;
    artifactViewModel.Kwfe100 = Kwfe100Input.value;
    artifactViewModel.Kwfe110 = Kwfe110Input.value;
    artifactViewModel.TolerancyKwfe = TolerancyKwfeInput.value;
    artifactViewModel.KwcuMva = KwcuMvaInput.value;
    artifactViewModel.KwcuKv = KwcuKvInput.value;
    artifactViewModel.Kwcu = KwcuInput.value;
    artifactViewModel.TolerancyKwCu = TolerancyKwCuInput.value;
    artifactViewModel.Kwaux3 = Kwaux3Input.value;
    artifactViewModel.Kwaux4 = Kwaux4Input.value;
    artifactViewModel.Kwaux1 = Kwaux1Input.value;
    artifactViewModel.Kwaux2 = Kwaux2Input.value;
    artifactViewModel.TolerancyKwAux = TolerancyKwAuxInput.value;
    artifactViewModel.Kwtot100 = Kwtot100Input.value;
    artifactViewModel.Kwtot110 = Kwtot110Input.value;
    artifactViewModel.TolerancyKwtot = TolerancyKwtotInput.value;
    artifactViewModel.ZPositiveMva = ZPositiveMvaInput.value;
    artifactViewModel.ZPositiveHx = ZPositiveHxInput.value;
    artifactViewModel.ZPositiveHy = ZPositiveHyInput.value;
    artifactViewModel.ZPositiveXy = ZPositiveXyInput.value;
    artifactViewModel.TolerancyZpositive = TolerancyZpositiveInput.value;
    artifactViewModel.TolerancyZpositive2 = TolerancyZpositive2Input.value;
    artifactViewModel.NoiseOa = NoiseOaInput.value;
    artifactViewModel.NoiseFa1 = NoiseFa1Input.value;
    artifactViewModel.NoiseFa2 = NoiseFa2Input.value;
    artifactViewModel.CoolingTypes = CoolingTypesInput.value;

    //Properties Tab Nozzles

    GetNozzlesArtifacts(artifactViewModel.NozzlesArtifacts);

    //Properties Tab LightningRod

    GetLightningRods(artifactViewModel.LightningRodArtifacts);

    //Properties Tab Changes

    GetChanges(artifactViewModel.ChangingTablesArtifacs);

    artifactViewModel.ComboNumericSc = ComboNumericScInput.value;
    artifactViewModel.CantidadSupSc = CantidadSupScInput.value;
    artifactViewModel.PorcentajeSupSc = PorcentajeSupScInput.value;
    artifactViewModel.CantidadInfSc = CantidadInfScInput.value;
    artifactViewModel.PorcentajeInfSc = PorcentajeInfScInput.value;
    artifactViewModel.NominalSc = NominalScInput.value;
    artifactViewModel.IdentificacionSc = IdentificacionScInput.value;
    artifactViewModel.InvertidoSc = InvertidoSc_ValueInput.value;

    artifactViewModel.ComboNumericBc = ComboNumericBcInput.value;
    artifactViewModel.CantidadSupBc = CantidadSupBcInput.value;
    artifactViewModel.PorcentajeSupBc = PorcentajeSupBcInput.value;
    artifactViewModel.CantidadInfBc = CantidadInfBcInput.value;
    artifactViewModel.PorcentajeInfBc = PorcentajeInfBcInput.value;
    artifactViewModel.NominalBc = NominalBcInput.value;
    artifactViewModel.IdentificacionBc = IdentificacionBcInput.value;
    artifactViewModel.InvertidoBc = InvertidoBc_ValueInput.value;

    //Properties Test Labs

    artifactViewModel.LabTestsArtifact.TextTestRoutine = LabTestsArtifact_TextTestRoutineInput.value;
    artifactViewModel.LabTestsArtifact.TextTestDielectric = LabTestsArtifact_TextTestDielectricInput.value;
    artifactViewModel.LabTestsArtifact.TextTestPrototype = LabTestsArtifact_TextTestPrototypeInput.value;


    //Properties norms
    artifactViewModel.RulesArtifacts = GetNorms();


}

function GetCharacteristics(characteristicModel) {


    let characteristicLength = (document.getElementById("CharacteristicList").childNodes.length - 1) / 2;

    for (var i = 0; i < characteristicLength; i++) {
        characteristicModel[i].CoolingType = document.getElementById(`CharacteristicsArtifacts_${i}__CoolingType`).value
        characteristicModel[i].OverElevation = document.getElementById(`CharacteristicsArtifacts_${i}__OverElevation`).value
        characteristicModel[i].Hstr = document.getElementById(`CharacteristicsArtifacts_${i}__Hstr`).value
        characteristicModel[i].DevAwr = document.getElementById(`CharacteristicsArtifacts_${i}__DevAwr`).value

        characteristicModel[i].Mvaf1 = document.getElementById(`CharacteristicsArtifacts_${i}__Mvaf1`).value
        characteristicModel[i].Mvaf2 = document.getElementById(`CharacteristicsArtifacts_${i}__Mvaf2`).value
        characteristicModel[i].Mvaf3 = document.getElementById(`CharacteristicsArtifacts_${i}__Mvaf3`).value
        characteristicModel[i].Mvaf4 = document.getElementById(`CharacteristicsArtifacts_${i}__Mvaf4`).value
    }

}

function GetNozzlesArtifacts(nozzlesArtifactModel) {

    let nozzlesLength = (document.getElementById("nozzlesList").childNodes.length - 1);

    for (var i = 0; i < nozzlesLength; i++) {
        nozzlesArtifactModel[i].Qty = document.getElementById(`NozzlesArtifacts_${i}__Qty`).value
        nozzlesArtifactModel[i].VoltageClass = document.getElementById(`NozzlesArtifacts_${i}__VoltageClass`).value
        nozzlesArtifactModel[i].BilClass = document.getElementById(`NozzlesArtifacts_${i}__BilClass`).value
        nozzlesArtifactModel[i].BilClassOther = document.getElementById(`NozzlesArtifacts_${i}__BilClassOther`).value
        nozzlesArtifactModel[i].CurrentAmps = document.getElementById(`NozzlesArtifacts_${i}__CurrentAmps`).value
        nozzlesArtifactModel[i].CorrienteUnidad = document.getElementById(`NozzlesArtifacts_${i}__CorrienteUnidad`).value
    }

}

function GetLightningRods(lightningRodArtifactModel) {

    for (var i = 0; i < lightningRodArtifactModel.length; i++) {
        lightningRodArtifactModel[i].Qty = document.getElementById(`LightningRodArtifacts_${i}__Qty`).value
    }

}

function GetChanges(changesModel) {

    for (var i = 0; i < changesModel.length; i++) {
        changesModel[i].OperationId = document.getElementById(`ChangingTablesArtifacs_${i}__OperationId`).value
        changesModel[i].FlagRcbnFcbn = document.getElementById(`ChangingTablesArtifacs_${i}__FlagRcbnFcbn`).value
        changesModel[i].DerivId = document.getElementById(`ChangingTablesArtifacs_${i}__DerivId`).value
        changesModel[i].DerivId2 = document.getElementById(`ChangingTablesArtifacs_${i}__DerivId2`).value
        changesModel[i].Taps = document.getElementById(`ChangingTablesArtifacs_${i}__Taps`).value
    }

}

//not ready
function GetNorms() {

    var data = tableNorma
        .rows()
        .data();

    let norms2Model = [];

    for (var i = 0; i < data.length; i++) {
        norms2Model.push({ OrderCode: noSerieInput.value, Descripcion: data[i][0] })
    }
    return norms2Model;

}







function LoadForm(artifactRecord) {

    if (artifactRecord.IsFromSPL) {
        let lbIsFromSPL = document.getElementById("lbIsFromSPL");
        lbIsFromSPL.style.display = "block";
        let lbIsFromSIDCO = document.getElementById("lbIsFromSIDCO");
        lbIsFromSIDCO.style.display = "none";
    }
    else {
        let lbIsFromSIDCO = document.getElementById("lbIsFromSIDCO");
        lbIsFromSIDCO.style.display = "block";
        let lbIsFromSPL = document.getElementById("lbIsFromSPL");
        lbIsFromSPL.style.display = "none";
    }

    //GENERAL
    descripcionInput.value = artifactViewModel.Descripcion;
    customerNameInput.value = artifactRecord.CustomerName;
    poNumericInput.value = artifactRecord.PoNumeric;
    typetrafoidInput.value = artifactRecord.Typetrafoid;
    applicationIdInput.value = artifactRecord.ApplicationId;
    standardIdInput.value = artifactRecord.StandardId;
    normaInput.value = artifactRecord.Norma;
    languageIdInput.value = artifactRecord.LanguageId;
    phasesInput.value = artifactRecord.Phases;
    frecuencyInput.value = artifactRecord.Frecuency;
    altitudeF1Input.value = artifactRecord.AltitudeF1;
    altitudeF2Input.value = artifactRecord.AltitudeF2;
    tipoUnidadInput.value = artifactRecord.TipoUnidad;
    normaEquivalenteInput.value = artifactRecord.NormaEquivalente;
    claveIdiomaInput.value = artifactRecord.ClaveIdioma;
    polarityIdInput.value = artifactRecord.PolarityId;
    polarityOtherInput.value = artifactRecord.PolarityOther;
    desplazamientoAngularInput.value = artifactRecord.DesplazamientoAngular;

    oilBrandInput.value = artifactRecord.OilBrand == undefined ? '' : artifactRecord.OilBrand;


    oilTypeInput.value = artifactRecord.IsFromSPL ? artifactRecord.OilType === null ? 'Mineral' : artifactRecord.OilType : 'Mineral';

    //CHARACTERISTICS
    let list = document.getElementById("CharacteristicList");
    list.innerHTML = '';
    artifactRecord.CharacteristicsArtifacts.forEach(AddCharacteristic);

    tensionKvAltaTension1Input.value = artifactRecord.TensionKvAltaTension1;
    tensionKvAltaTension2Input.value = artifactRecord.TensionKvAltaTension2;
    tensionKvAltaTension3Input.value = artifactRecord.TensionKvAltaTension3;
    tensionKvAltaTension4Input.value = artifactRecord.TensionKvAltaTension4;

    tensionKvBajaTension1Input.value = artifactRecord.TensionKvBajaTension1;
    tensionKvBajaTension2Input.value = artifactRecord.TensionKvBajaTension2;
    tensionKvBajaTension3Input.value = artifactRecord.TensionKvBajaTension3;
    tensionKvBajaTension4Input.value = artifactRecord.TensionKvBajaTension4;

    tensionKvSegundaBaja1Input.value = artifactRecord.TensionKvSegundaBaja1;
    tensionKvSegundaBaja2Input.value = artifactRecord.TensionKvSegundaBaja2;
    tensionKvSegundaBaja3Input.value = artifactRecord.TensionKvSegundaBaja3;
    tensionKvSegundaBaja4Input.value = artifactRecord.TensionKvSegundaBaja4;

    tensionKvTerciario1Input.value = artifactRecord.TensionKvTerciario1;
    tensionKvTerciario2Input.value = artifactRecord.TensionKvTerciario2;
    tensionKvTerciario3Input.value = artifactRecord.TensionKvTerciario3;
    tensionKvTerciario4Input.value = artifactRecord.TensionKvTerciario4;

    nbaiAltaTensionInput.value = artifactRecord.NbaiAltaTension;
    nbaiBajaTensionInput.value = artifactRecord.NbaiBajaTension;
    nbaiSegundaBajaInput.value = artifactRecord.NbaiSegundaBaja;
    nabaiTerceraInput.value = artifactRecord.NabaiTercera;

    ConexionAltaTensionInput.value = artifactRecord.ConexionAltaTension;
    ConexionBajaTensionInput.value = artifactRecord.ConexionBajaTension;
    ConexionSegundaBajaInput.value = artifactRecord.ConexionSegundaBaja;
    ConexionTerceraInput.value = artifactRecord.ConexionTercera;

    IdConexionAltaTensionInput.value = artifactRecord.IdConexionAltaTension;
    IdConexionBajaTensionInput.value = artifactRecord.IdConexionBajaTension;
    IdConexionSegundaBajaInput.value = artifactRecord.IdConexionSegundaBaja;
    IdConexionTerceraInput.value = artifactRecord.IdConexionTercera;

    otraConexionAltaTensionInput.value = artifactRecord.OtraConexionAltaTension;
    otraConexionBajaTensionInput.value = artifactRecord.OtraConexionBajaTension;
    otraConexionSegundaBajaInput.value = artifactRecord.OtraConexionSegundaBaja;
    otraConexionTerceraInput.value = artifactRecord.OtraConexionTercera;

    conexionEquivalenteInput.value = artifactRecord.ConexionEquivalente;
    conexionEquivalente_2Input.value = artifactRecord.ConexionEquivalente_2;
    conexionEquivalente_3Input.value = artifactRecord.ConexionEquivalente_3;
    conexionEquivalente_4Input.value = artifactRecord.ConexionEquivalente_4;

    tipoDerivacionAltaTensionInput.value = artifactRecord.TipoDerivacionAltaTension;
    valorDerivacionUpAltaTensionInput.value = artifactRecord.ValorDerivacionUpAltaTension;
    valorDerivacionDownAltaTensionInput.value = artifactRecord.ValorDerivacionDownAltaTension;

    tipoDerivacionBajaTensionInput.value = artifactRecord.TipoDerivacionBajaTension;
    valorDerivacionUpBajaTensionInput.value = artifactRecord.ValorDerivacionUpBajaTension;
    valorDerivacionDownBajaTensionInput.value = artifactRecord.ValorDerivacionDownBajaTension;

    tipoDerivacionSegundaTensionInput.value = artifactRecord.TipoDerivacionSegundaTension;
    valorDerivacionUpSegundaTensionInput.value = artifactRecord.ValorDerivacionUpSegundaTension;
    valorDerivacionDownSegundaTensionInput.value = artifactRecord.ValorDerivacionDownSegundaTension;

    tipoDerivacionTerceraTensionInput.value = artifactRecord.TipoDerivacionTerceraTension;
    valorDerivacionUpTerceraTensionInput.value = artifactRecord.ValorDerivacionUpTerceraTension;
    valorDerivacionDownTerceraTensionInput.value = artifactRecord.ValorDerivacionDownTerceraTension;

    tapsAltaTensionInput.value = artifactRecord.TapsAltaTension;
    tapsBajaTensionInput.value = artifactRecord.TapsBajaTension;
    tapsSegundaBajaInput.value = artifactRecord.TapsSegundaBaja;
    tapsTerciarioInput.value = artifactRecord.TapsTerciario;

    valorNbaiNeutroAltaTensionInput.value = artifactRecord.ValorNbaiNeutroAltaTension;
    valorNbaiNeutroBajaTensionInput.value = artifactRecord.ValorNbaiNeutroBajaTension;
    valorNbaiNeutroSegundaBajaInput.value = artifactRecord.ValorNbaiNeutroSegundaBaja;
    valorNbaiNeutroTerceraInput.value = artifactRecord.ValorNbaiNeutroTercera;

    //Waranties
    Iexc100Input.value = artifactRecord.Iexc100;
    Kwfe100Input.value = artifactRecord.Kwfe100;
    KwcuInput.value = artifactRecord.Kwcu;
    Kwaux2Input.value = artifactRecord.Kwaux2;
    Kwtot100Input.value = artifactRecord.Kwtot100;
    ZPositiveHxInput.value = artifactRecord.ZPositiveHx;

    Iexc110Input.value = artifactRecord.Iexc110;
    Kwfe110Input.value = artifactRecord.Kwfe110;
    KwcuMvaInput.value = artifactRecord.KwcuMva;
    Kwaux4Input.value = artifactRecord.Kwaux4;
    Kwtot110Input.value = artifactRecord.Kwtot110;
    ZPositiveMvaInput.value = artifactRecord.ZPositiveMva;


    let coolingTypes = '';
    for (var i = 0; i < artifactRecord.CharacteristicsArtifacts.length; i++) {
        if (i + 1 < artifactRecord.CharacteristicsArtifacts.length) {
            coolingTypes += artifactRecord.CharacteristicsArtifacts[i].CoolingType + "/";
        }
        else
            coolingTypes += artifactRecord.CharacteristicsArtifacts[i].CoolingType;
    }

    CoolingTypesInput.value = coolingTypes;


    TolerancyKwfeInput.value = artifactRecord.TolerancyKwfe;
    KwcuKvInput.value = artifactRecord.KwcuKv;
    TolerancyKwCuInput.value = artifactRecord.TolerancyKwCu;
    Kwaux1Input.value = artifactRecord.Kwaux1;
    Kwaux3Input.value = artifactRecord.Kwaux3;
    TolerancyKwAuxInput.value = artifactRecord.TolerancyKwAux;
    TolerancyKwtotInput.value = artifactRecord.TolerancyKwtot;
    ZPositiveHyInput.value = artifactRecord.ZPositiveHy;
    ZPositiveXyInput.value = artifactRecord.ZPositiveXy;
    TolerancyZpositiveInput.value = artifactRecord.TolerancyZpositive;
    TolerancyZpositive2Input.value = artifactRecord.TolerancyZpositive2;
    NoiseOaInput.value = artifactRecord.NoiseOa;
    NoiseFa1Input.value = artifactRecord.NoiseFa1;
    NoiseFa2Input.value = artifactRecord.NoiseFa2;

    //Nozzless

    let nozzlesList = document.getElementById("nozzlesList");
    nozzlesList.innerHTML = `<div style="padding:20px" class="form-group col-lg-2">
                                        <div class="col-sm-12">
                                            <div class="text-center">
                                                <label class="col-form-label"></label>
                                            </div>
                                        </div>
                                        <div class="col-sm-12">
                                            <div class="text-center" style="padding-top: 23px">
                                                <label class="col-form-label">Cantidad</label>
                                            </div>
                                        </div>
                                        <div class="col-sm-12">
                                            <div class="text-center" style="padding-top: 10px ">
                                                <label class="col-form-label">Clase de Votaje (KV)</label>
                                            </div>
                                        </div>
                                        <div class="col-sm-12">
                                            <div class="text-center" style="padding-top: 10px ">
                                                <label class="col-form-label">Bil (KV) de la Boquilla</label>
                                            </div>
                                        </div>
                                          <div class="col-sm-12" style="display:none">
                                            <div class="text-center" style="padding-top: 10px ">
                                                <label class="col-form-label">Bil other</label>
                                            </div>
                                        </div>
                                        <div class="col-sm-12">
                                            <div class="text-center" style="padding-top: 10px ">
                                                <label class="col-form-label">Bil Unidad</label>
                                            </div>
                                        </div>
                                        <div class="col-sm-12">
                                            <div class="text-center" style="padding-top: 10px ">
                                                <label class="col-form-label">Corriente (Amps. Operación)</label>
                                            </div>
                                        </div>
                                        <div class="col-sm-12">
                                            <div class="text-center" style="padding-top: 10px ">
                                                <label class="col-form-label">Corriente de la Unidad</label>
                                            </div>
                                        </div>
                                    </div>`;

    artifactRecord.NozzlesArtifacts.forEach(AddNozzles);

    for (var i = 0; i < artifactRecord.NozzlesArtifacts.length; i++) {
        let currentAmps = document.getElementById(`NozzlesArtifacts_${i}__CurrentAmps`).value;
        let corrienteUnidad = document.getElementById(`NozzlesArtifacts_${i}__CorrienteUnidad`).value;
        if (parseFloat(corrienteUnidad) > parseFloat(currentAmps)) {

            $(`.NozzlesArtifacts_${i}__CorrienteUnidadValid`).attr('style', 'color:red')
        }
    }
    //lightningRod
    let lightningRodList = document.getElementById("lightningRodList");
    lightningRodList.innerHTML = `<div style="padding:20px" class="form-group col-lg-2">
                                        <div class="col-sm-12">
                                            <div class="text-center">
                                                <label class="col-form-label"></label>
                                            </div>
                                        </div>
                                        <div class="col-sm-12">
                                            <div class="text-center" style="padding-top: 23px">
                                                <label class="col-form-label">Cantidad</label>
                                            </div>
                                        </div>
                                    </div>`;

    artifactRecord.LightningRodArtifacts.forEach(AddLightningRodArtifacts);

    //Changins
    let operationConditionTitle = document.getElementById("titleOperationConditionTable");
    operationConditionTitle.innerHTML = `<th>
                                                               <div class="col">
                                                                   <div class="text-center">
                                                                       <label class="col-form-label"></label>
                                                                   </div>
                                                               </div>
                                                           </th>`;

    artifactRecord.ChangingTablesArtifacs.forEach(AddOperationConditionTitle);


    let rowConditionOperation = document.getElementById("rowConditionOperation");
    rowConditionOperation.innerHTML = `<td>
                                                           <div class="col">
                                                               <div class="text-center">
                                                                   <label class="col-form-label">Condición de Operación</label>
                                                               </div>
                                                           </div>
                                                       </td>`;

    AddOperationCondition(artifactRecord);

    let rowDerivations = document.getElementById("rowDerivations");
    rowDerivations.innerHTML = `<td>
                                                           <div class="col">
                                                               <div class="text-center">
                                                                   <label class="col-form-label">Derivaciones (%)</label>
                                                               </div>
                                                           </div>
                                                       </td>`;

    artifactRecord.ChangingTablesArtifacs.forEach(AddDerivations);

    let rowSteps = document.getElementById("rowSteps");
    rowSteps.innerHTML = `                    <td>
                                                           <div class="col">
                                                               <div class="text-center">
                                                                   <label class="col-form-label">Pasos</label>
                                                               </div>
                                                           </div>
                                                       </td>`;

    artifactRecord.ChangingTablesArtifacs.forEach(AddSteps);

    if (artifactRecord.TapBaan !== null) {
        ComboNumericScInput.value = artifactRecord.ComboNumericSc;
        CantidadSupScInput.value = artifactRecord.CantidadSupSc;
        PorcentajeSupScInput.value = artifactRecord.PorcentajeSupSc;
        CantidadInfScInput.value = artifactRecord.CantidadInfSc;
        PorcentajeInfScInput.value = artifactRecord.PorcentajeInfSc;
        NominalScInput.value = artifactRecord.NominalSc;
        IdentificacionScInput.value = artifactRecord.IdentificacionSc;
        InvertidoSc_ValueInput.value = artifactRecord.InvertidoSc;

        ComboNumericBcInput.value = artifactRecord.ComboNumericBc;
        CantidadSupBcInput.value = artifactRecord.CantidadSupBc;
        PorcentajeSupBcInput.value = artifactRecord.PorcentajeSupBc;
        CantidadInfBcInput.value = artifactRecord.CantidadInfBc;
        PorcentajeInfBcInput.value = artifactRecord.PorcentajeInfBc;
        NominalBcInput.value = artifactRecord.NominalBc;
        IdentificacionBcInput.value = artifactRecord.IdentificacionBc;
        InvertidoBc_ValueInput.value = artifactRecord.InvertidoBc;
    }

    //Tests Lab
    if (artifactRecord.LabTestsArtifact !== null) {
        LabTestsArtifact_TextTestRoutineInput.value = artifactRecord.LabTestsArtifact.TextTestRoutine;
        LabTestsArtifact_TextTestDielectricInput.value = artifactRecord.LabTestsArtifact.TextTestDielectric;
        LabTestsArtifact_TextTestPrototypeInput.value = artifactRecord.LabTestsArtifact.TextTestPrototype;
    }

    //Norms
    tableNorma.row($(this).parents('tr'))
        .clear()
        .draw();

    artifactRecord.RulesArtifacts.forEach(AddNorm);

    refreshRules();
}

function AddCharacteristic(characteristicModel, index) {
    let content =
        `
                                        <div class="col-sm-12">
                                            <div class="row">
                                                <div class="col-sm-3">
                                                    <div class="row">
                                                        <div class="form-group col-lg-3">
                                                            <input readonly maxlength="200" class="form-control form-control-lg" type="text" id="CharacteristicsArtifacts_${index}__CoolingType" name="CharacteristicsArtifacts[${index}].CoolingType" value="${characteristicModel.CoolingType == null ? "" : characteristicModel.CoolingType}">
                                                                   <span id="CharacteristicsArtifacts_${index}__CoolingTypeSpan"   class="text-danger"></span>
                                                        </div>
                                                        <div class="form-group col-lg-3">
                                                            <input onkeypress="return soloNumeros(event)" class="form-control form-control-lg" type="text" data-val="true" data-val-number="Solo se permiten números" id="CharacteristicsArtifacts_${index}__OverElevation" name="CharacteristicsArtifacts[${index}].OverElevation" value="${characteristicModel.OverElevation == null ? "" : characteristicModel.OverElevation}">
                                                         <span id="CharacteristicsArtifacts_${index}__OverElevationSpan"   class="text-danger"></span>
                                                        </div>
                                                        <div class="form-group col-lg-3">
                                                            <input onkeypress="return soloNumeros(event)" class="form-control form-control-lg" type="text" data-val="true" data-val-number="Solo se permiten números" id="CharacteristicsArtifacts_${index}__Hstr" name="CharacteristicsArtifacts[${index}].Hstr" value="${characteristicModel.Hstr == null || characteristicModel.Hstr == -1 ? "" : characteristicModel.Hstr}">
                                                        </div>
                                                        <div class="form-group col-lg-3">
                                                            <input onkeypress="return soloNumeros(event)" class="form-control form-control-lg" type="text" data-val="true" data-val-number="Solo se permiten números" id="CharacteristicsArtifacts_${index}__DevAwr" name="CharacteristicsArtifacts[${index}].DevAwr" value="${characteristicModel.DevAwr == null || characteristicModel.DevAwr == -1 ? "" : characteristicModel.DevAwr}">
                                                        </div>

                                                    </div>
                                                </div>
                                                <div class="text-center col-sm-1">
                                                    <label style="padding-top:10px;">MVA</label>
                                                </div>
                                                <div class="col-sm-8">
                                                    <div class="row">
                                                        <div class="form-group col-lg-3">
                                                            <input onkeypress="return soloNumeros(event)" class="form-control form-control-lg" type="text" data-val="true" data-val-number="Solo se permiten números" id="CharacteristicsArtifacts_${index}__Mvaf1" name="CharacteristicsArtifacts[${index}].Mvaf1" value="${characteristicModel.Mvaf1 == null ? "" : characteristicModel.Mvaf1}">
                                                            <span id="CharacteristicsArtifacts_${index}__Mvaf1Span"   class="text-danger"></span>
                                                        </div>
                                                        <div class="form-group col-lg-3">
                                                            <input onkeypress="return soloNumeros(event)" class="form-control form-control-lg" type="text" data-val="true" data-val-number="Solo se permiten números" id="CharacteristicsArtifacts_${index}__Mvaf2" name="CharacteristicsArtifacts[${index}].Mvaf2" value="${characteristicModel.Mvaf2 == null ? "" : characteristicModel.Mvaf2}">
                                                        </div>
                                                        <div class="form-group col-lg-3">
                                                            <input onkeypress="return soloNumeros(event)" class="form-control form-control-lg" type="text" data-val="true" data-val-number="Solo se permiten números" id="CharacteristicsArtifacts_${index}__Mvaf3" name="CharacteristicsArtifacts[${index}].Mvaf3" value="${characteristicModel.Mvaf3 == null ? "" : characteristicModel.Mvaf3}">
                                                        </div>
                                                        <div class="form-group col-lg-3">
                                                            <input onkeypress="return soloNumeros(event)" class="form-control form-control-lg" type="text" data-val="true" data-val-number="Solo se permiten números" id="CharacteristicsArtifacts_${index}__Mvaf4" name="CharacteristicsArtifacts[${index}].Mvaf4" value="${characteristicModel.Mvaf4 == null ? "" : characteristicModel.Mvaf4}">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
        `;
    let list = document.getElementById("CharacteristicList");

    list.innerHTML += content;
}



function AddNozzles(nozzlesModel, index) {

    const content = `<div class="col-lg-2" style="padding: 20px">
                                            <div class="col-sm-12">
                                                <div class="text-center">
                                                    <label class="col-form-label">${nozzlesModel.ColumnTitle}</label>
                                                </div>
                                            </div>
                                            <div class="col-sm-12">
                                                <div class="col" style="padding-top:10px">
                                                    <input onkeypress="return soloNumeros(event)" class="form-control nozzlesInput" type="text" data-val="true" data-val-number="Solo se permiten números" id="NozzlesArtifacts_${index}__Qty" name="NozzlesArtifacts[${index}].Qty" value="${nozzlesModel.Qty}">
                                                </div>
                                            </div>
                                            <div class="col-sm-12">
                                                <div class="col" style="padding-top:10px">
                                                    <input onkeypress="return soloNumeros(event)" class="form-control" type="text" data-val="true" data-val-number="Solo se permiten números" id="NozzlesArtifacts_${index}__VoltageClass" name="NozzlesArtifacts[${index}].VoltageClass" value="${nozzlesModel.VoltageClass}">
                                                </div>
                                            </div>
                                            <div class="col-sm-12">
                                                <div class="col" style="padding-top:10px">
                                                    <input  onkeypress="return soloNumeros(event)" class="form-control" type="text" data-val="true" data-val-number="Solo se permiten números" id="NozzlesArtifacts_${index}__BilClass" name="NozzlesArtifacts[${index}].BilClass" value="${nozzlesModel.BilClass}">
                                                </div>
                                            </div>
                                            <div class="col-sm-12" style="display:none">
                                                <div class="col" style="padding-top:10px">
                                                    <input readonly onkeypress="return soloNumeros(event)" class="form-control" type="text" id="NozzlesArtifacts_${index}__BilClassOther" name="NozzlesArtifacts[${index}].BilClassOther" value="${nozzlesModel.BilClassOther}">
                                                     <span id="NozzlesArtifacts_${index}__BilClassOtherSpan"   class="text-danger"></span>
                                              </div>
                                            </div>
                                            <div class="col-sm-12">
                                                <div class="col" style="padding-top:10px">
                                                    <input readonly onkeypress="return soloNumeros(event)" class="form-control" type="text" id="NozzlesArtifacts_${index}__ResultBilUnidad" name="NozzlesArtifacts[${index}].ResultBilUnidad" value="${nozzlesModel.ResultBilUnidad}">
                                                     <span id="NozzlesArtifacts_${index}__ResultBilUnidadSpan"   class="text-danger"></span>
                                              </div>
                                            </div>
                                            <div class="col-sm-12">
                                                <div class="col" style="padding-top:10px">
                                                   <input readonly  onkeypress="return soloNumeros(event)" class="form-control" type="text" data-val="true" data-val-number="Solo se permiten números" id="NozzlesArtifacts_${index}__CurrentAmps" name="NozzlesArtifacts[${index}].CurrentAmps" value="${nozzlesModel.CurrentAmps}">
                                                    <span id="NozzlesArtifacts_${index}__CurrentAmpsSpan"   class="text-danger"></span>
                                                </div>
                                            </div>
                                            <div class="col-sm-12">
                                                <div class="col" style="padding-top:10px">
                                                    <input readonly onchange="validarValorCorrienteUnidad(${index})" onkeypress="return soloNumeros(event)" data-count="${index}" class="form-control NozzlesArtifacts_${index}__CorrienteUnidadValid" type="text" data-val="true" data-val-number="Solo se permiten números" id="NozzlesArtifacts_${index}__CorrienteUnidad" name="NozzlesArtifacts[${index}].CorrienteUnidad" value="${nozzlesModel.CorrienteUnidad}">
                                                  <span id="NozzlesArtifacts_${index}__CorrienteUnidadSpan"   class="text-danger"></span>
                                                </div>
                                            </div>
                                        </div>`;

    let list = document.getElementById("nozzlesList");

    list.innerHTML += content;



}


function AddLightningRodArtifacts(lightningRodModel, index) {

    let qty = '';

    if (lightningRodModel.Qty !== null && lightningRodModel.Qty !== undefined) {
        qty = lightningRodModel.Qty;
    }

    const content = `<div class="col-lg-2" style="padding: 20px">
                                            <div class="col-sm-12">
                                                <div class="text-center">
                                                    <label class="col-form-label">${lightningRodModel.ColumnTitle}</label>
                                                </div>
                                            </div>
                                            <div class="col-sm-12">
                                                <div class="col" style="padding-top:10px">
                                                    <input onkeypress="return soloNumeros(event)" class="form-control" type="text" data-val="true" data-val-number="Solo se permiten números" id="LightningRodArtifacts_${index}__Qty" name="LightningRodArtifacts[${index}].Qty" value="${qty}">
                                                </div>
                                            </div>                                            
                                        </div>`;

    let list = document.getElementById("lightningRodList");

    list.innerHTML += content;
}

function AddOperationConditionTitle(changingTablesArtifacsModel, index) {
    const content = `                                   <th>
                                                               <div class="col">
                                                                   <div class="text-center">
                                                                       <label class="col-form-label">${changingTablesArtifacsModel.ColumnTitle}</label>
                                                                   </div>
                                                               </div>
                                                           </th>`;
    let list = document.getElementById("titleOperationConditionTable");

    list.innerHTML += content;
}

function AddOperationCondition(artifactRecord) {

    let items = '';

    for (var x = 0; x < artifactRecord.OperationsItems.length; x++) {
        items += `<option value="${artifactRecord.OperationsItems[x].Value}">${artifactRecord.OperationsItems[x].Text}</option>`;
    }

    for (var y = 0; y < artifactRecord.ChangingTablesArtifacs.length; y++) {
        const content = `<td>
                                                               <div class="col" style="padding: 10px">
                                                                   <select onkeypress="return soloNumeros(event)" class="form-select form-select-sm" data-val="true" data-val-number="Solo se permiten números" id="ChangingTablesArtifacs_${y}__OperationId" name="ChangingTablesArtifacs[${y}].OperationId">
                                                                    ${items}
                                                                    </select>
                                                               </div>
                                                           </td>`;
        let list = document.getElementById("rowConditionOperation");
        list.innerHTML += content;

        let input = document.getElementById(`ChangingTablesArtifacs_${y}__OperationId`);
        input.value = artifactRecord.ChangingTablesArtifacs[y].OperationId;
    }

}

function AddDerivations(changingTablesArtifacsModel, index) {

    const content = `                                   <td>
                                                               <div class="row" style="padding: 10px">
                                                                   <div class="col-sm-6 text-center" style="padding-top: 25px">
                                                                       <input maxlength="50" class="form-control" id="ChangingTablesArtifacs_${index}__FlagRcbnFcbn" name="ChangingTablesArtifacs[${index}].FlagRcbnFcbn" type="text" value="${changingTablesArtifacsModel.FlagRcbnFcbn}">
                                                                   </div>
                                                                   <div class="col-sm-6">
                                                                       <div class="col-sm-12" style="padding:5px;">
                                                                           <input onkeypress="return soloNumeros(event)" class="form-control" data-val="true" data-val-number="Solo se permiten números" id="ChangingTablesArtifacs_${index}__DerivId" name="ChangingTablesArtifacs[${index}].DerivId" type="text" value="${changingTablesArtifacsModel.DerivId}">
                                                                       </div>
                                                                       <div class="col-sm-12" style="padding:5px;">
                                                                           <input onkeypress="return soloNumeros(event)" class="form-control" data-val="true" data-val-number="Solo se permiten números" id="ChangingTablesArtifacs_${index}__DerivId2" name="ChangingTablesArtifacs[${index}].DerivId2" type="text" value="${changingTablesArtifacsModel.DerivId2}">
                                                                       </div>
                                                                   </div>
                                                               </div>
                                                           </td>`;
    let list = document.getElementById("rowDerivations");

    list.innerHTML += content;

}

function AddSteps(changingTablesArtifacsModel, index) {

    const content = `  <td>
                                                               <div class="col" style="padding: 10px">
                                                                   <input onkeypress="return soloNumeros(event)" class="form-control" data-val="true" data-val-number="Solo se permiten números" id="ChangingTablesArtifacs_${index}__Taps" name="ChangingTablesArtifacs[${index}].Taps" type="text" value="${changingTablesArtifacsModel.Taps}">

                                                               </div>
                                                           </td>`;
    let list = document.getElementById("rowSteps");

    list.innerHTML += content;

}

$("#NbaiAltaTension").keypress(function () {

    for (var i = 0; i < artifactViewModel.NozzlesArtifacts.length; i++) {
        if (artifactViewModel.NozzlesArtifacts[i].ColumnTitle == "Alta Tensión" || artifactViewModel.NozzlesArtifacts[i].ColumnTitle == "Dev Serie") {
            $(`#NozzlesArtifacts_${i}__ResultBilUnidad`).val($("#NbaiAltaTension").val());
        }
    };

});
$("#NbaiBajaTension").keypress(function () {
    for (var i = 0; i < artifactViewModel.NozzlesArtifacts.length; i++) {
        if (artifactViewModel.NozzlesArtifacts[i].ColumnTitle == "Baja Tensión" || artifactViewModel.NozzlesArtifacts[i].ColumnTitle == "Dev Común") {
            $(`#NozzlesArtifacts_${i}__ResultBilUnidad`).val($("#NbaiBajaTension").val());
        }
    };
});
$("#NbaiSegundaBaja").keypress(function () {
    for (var i = 0; i < artifactViewModel.NozzlesArtifacts.length; i++) {
        if (artifactViewModel.NozzlesArtifacts[i].ColumnTitle == "Baja Tensión 2") {
            $(`#NozzlesArtifacts_${i}__ResultBilUnidad`).val($("#NbaiSegundaBaja").val());
        }
    };
});
$("#NabaiTercera").keypress(function () {
    for (var i = 0; i < artifactViewModel.NozzlesArtifacts.length; i++) {
        if (artifactViewModel.NozzlesArtifacts[i].ColumnTitle == "Terc") {
            $(`#NozzlesArtifacts_${i}__ResultBilUnidad`).val($("#NabaiTercera").val());
        }
    };
});





