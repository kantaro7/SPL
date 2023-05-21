namespace SPL.Masters.Api.DTOs.Configuration
{
    public class FileWeightDto
    {
        public long Id { get; set; }
        public long? ExtensionArchivo { get; set; }
        public string MaximoPeso { get; set; }
        public long IdModulo { get; set; }

        public FileExtensionsDto ExtensionArchivoNavigation { get; set; }
    }
}
