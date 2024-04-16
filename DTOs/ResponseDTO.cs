namespace BAS_Project.DTOs
{
    public class ResponseDTO
    {
        public string RequestTime { get; set; }

        public string WriteTime { get; set; }

        public long ProcessingTime { get; set; }

        public ResponseDTO(string requestTime, string writeTime, long processingTime)
        {
            RequestTime = requestTime;
            WriteTime = writeTime;
            ProcessingTime = processingTime;
        }
    }
}

