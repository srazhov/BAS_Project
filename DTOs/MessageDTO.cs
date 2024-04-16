namespace BAS_Project.DTOs
{
    public class MessageDTO
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// Start time of the request
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Time when the writing in the file was started
        /// </summary>
        public DateTime WriteTime { get; set; }

        /// <summary>
        /// Message string
        /// </summary>
        public string Message { get; set; }
    }
}

