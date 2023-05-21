namespace SPL.WebApp.Domain.DTOs
{

    public class FileExtensionsDTO
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public long TipoArchivo { get; set; }
        public bool Active { get; set; }
    }
}
