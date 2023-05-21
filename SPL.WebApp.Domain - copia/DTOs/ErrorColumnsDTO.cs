namespace SPL.WebApp.Domain.DTOs
{

    public class ErrorColumnsDTO
    {
        public int Column { get; set; }
        public int Fila { get; set; }
        public string Message { get; set; }
        public ErrorColumnsDTO()
        {

        }

        public ErrorColumnsDTO(int pColumn, int pFila, string pMessage)
        {
            this.Column = pColumn;
            this.Fila = pFila;
            this.Message = pMessage;
        }
    }
}
