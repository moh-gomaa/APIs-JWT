namespace ApisJwt.Helpers
{
    public class ApiResponse<Object>
    {
        public ApiResponse(bool status, string message, Object obj)
        {
            this.Status = status;
            this.Message = message;
            this.ResponseObject = obj;
        }
        public bool Status { get; set; }
        public string Message { get; set; }
        public Object ResponseObject { get; set; }
    }
}
