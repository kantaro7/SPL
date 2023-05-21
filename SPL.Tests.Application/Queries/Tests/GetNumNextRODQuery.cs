namespace SPL.Tests.Application.Queries.Tests
{
    using MediatR;

    using SPL.Domain;

    public class GetNumNextRODQuery : IRequest<ApiResponse<long>>
    {
        public GetNumNextRODQuery(string noSerie, string keyTest, string lenguage, string connection, string unitType, string material, string unitOfMeasurement)
        {
            this.NroSerie = noSerie;
            this.KeyTests = keyTest;
            this.Connection = connection;
            this.TypeUnit = unitType;
            this.Material = material;
            this.UnitOfMeasurement = unitOfMeasurement;
            this.Lenguage = lenguage;

        }
        #region Constructor

        public string NroSerie { get; }
        public string KeyTests { get; }
        public string TypeUnit { get; }
        public string Connection { get; }
        public string Material { get; }
        public string UnitOfMeasurement { get; }
        public string Lenguage { get; }
   

        #endregion
    }
}
