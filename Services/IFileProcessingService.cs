using BAS_Project.DTOs;

namespace BAS_Project.Services
{
    public interface IFileProcessingService
    {
        string EnqueueMessage(string message);

        MessageDTO WriteMessage(string messageId);
    }
}

