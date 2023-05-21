namespace SPL.Domain
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public T Structure { get; set; }

        public ApiResponse()
        {
        }

        public ApiResponse(int code, string description, T structure)
        {
            this.Code = code;
            this.Description = description;
            this.Structure = structure;
        }
    }
    public enum ResponsesID
    {
        exitoso = 1, fallido = -1, exception = 5
    }
}
