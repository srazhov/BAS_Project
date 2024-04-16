using BAS_Project.DTOs;

namespace BAS_Project.Services
{
    public interface IFileProcessingService
    {
        /// <summary>
        /// Add the new message string to the queue to write in the file
        /// </summary>
        /// <param name="message">Message to write in a file</param>
        /// <param name="requestTime">Time when the request was made</param>
        /// <returns>Message Id associated with this request</returns>
        string EnqueueMessage(string message, DateTime requestTime);

        /// <summary>
        /// Writes messages in bulk and get the requested message back using messageId
        /// </summary>
        /// <param name="messageId">Message Id associated with the request</param>
        /// <returns>MessageDTO object</returns>
        MessageDTO WriteMessage(string messageId);
    }
}

