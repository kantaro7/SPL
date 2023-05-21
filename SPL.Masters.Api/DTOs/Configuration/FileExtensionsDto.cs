namespace SPL.Masters.Api.DTOs.Configuration
{
    public class FileExtensionsDto
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public long TipoArchivo { get; set; }
        public bool Active { get; set; }
    }
}
