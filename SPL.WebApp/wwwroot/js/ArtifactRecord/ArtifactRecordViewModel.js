
class ArtifactRecordViewModel {

    constructor() {
        this.NoSerie = '';
        this.OrderCode = '';
        this.Creadopor = '';
        this.Fechacreacion = '';
        this.Modificadopor = '';
        this.Fechamodificacion = '';
        this.OperationsItems = [];
        this.Descripcion = '';
    }

    map(ArtifactRecordViewModel) {
        this.NoSerie = ArtifactRecordViewModel.NoSerie;
        this.OrderCode = ArtifactRecordViewModel.OrderCode;
        this.Creadopor = ArtifactRecordViewModel.Creadopor;
        this.Fechacreacion = ArtifactRecordViewModel.Fechacreacion;
        this.Modificadopor = ArtifactRecordViewModel.Modificadopor;
        this.Fechamodificacion = ArtifactRecordViewModel.Fechamodificacion;
        this.OperationsItems = ArtifactRecordViewModel.OperationsItems;
    }
}

export default ArtifactRecordViewModel;