namespace BAS_Project.DTOs
{
    public class ResponseDTO
    {
        public string RequestTime { get; set; }

        public string WriteTime { get; set; }

        public long ProcessingTime { get; set; }

        public string message { get; set; }

        public ResponseDTO(string requestTime, string writeTime, long processingTime, string message)
        {
            RequestTime = requestTime;
            WriteTime = writeTime;
            ProcessingTime = processingTime;
            this.message = message;
        }
    }
}

