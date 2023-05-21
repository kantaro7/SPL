namespace SPL.WebApp.Domain.DTOs
{

    public class FileWeightDTO
    {
        public long Id { get; set; }
        public long? ExtensionArchivo { get; set; }
        public string MaximoPeso { get; set; }
        public long IdModulo { get; set; }

        public FileExtensionsDTO ExtensionArchivoNavigation { get; set; }
    }
}
