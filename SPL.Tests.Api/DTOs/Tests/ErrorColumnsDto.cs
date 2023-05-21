namespace SPL.Tests.Api.DTOs.Tests
{
    public class ErrorColumnsDto
    {
        public int Column { get; set; }
        public int Fila { get; set; }
        public string Message { get; set; }
        public ErrorColumnsDto()
        {
        }

        public ErrorColumnsDto(int pColumn, int pFila, string pMessage)
        {
            this.Column = pColumn;
            this.Fila = pFila;
            this.Message = pMessage;
        }
    }
}
