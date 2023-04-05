namespace Employee.Models
{
    public class Response<t>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public int UserID { get; set; }
        //public int mobile { get; set; }
        public t Data { get; set; }

    }
}
