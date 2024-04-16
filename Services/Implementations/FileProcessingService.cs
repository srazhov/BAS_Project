using System.Text;
using BAS_Project.DTOs;
using BAS_Project.Helpers;

namespace BAS_Project.Services.Implementations
{
    public class FileProcessingService : IFileProcessingService
    {
        private static readonly object queueLocker = new();

        private static readonly object mainLocker = new();

        private Queue<MessageDTO> Messages { get; set; }

        private List<MessageDTO> FinishedMessages { get; set; }

        private IWebHostEnvironment AppEnvironment { get; set; }



        public FileProcessingService(IWebHostEnvironment appEnvironment)
        {
            Messages = new Queue<MessageDTO>();
            FinishedMessages = new List<MessageDTO>();
            AppEnvironment = appEnvironment;
        }

        public string EnqueueMessage(string message)
        {
            lock (queueLocker)
            {
                var messageDTO = new MessageDTO()
                {
                    MessageId = Guid.NewGuid().ToString(),
                    StartTime = DateTime.Now,
                    Message = message
                };

                Messages.Enqueue(messageDTO);

                return messageDTO.MessageId;
            }
        }

        public MessageDTO WriteMessage(string messageId)
        {
            lock (mainLocker)
            {
                var result = GetAndDeleteItem(messageId);
                if (result != null)
                {
                    return result;
                }

                var path = Path.Combine(AppEnvironment.WebRootPath, "data.txt");
                using (var writer = new StreamWriter(new FileStream(path, FileMode.Append)))
                {
                    var msgsCount = Messages.Count;
                    var sb = new StringBuilder();
                    for (int i = 0; i < 5 && Messages.TryDequeue(out var messageDTO); i++)
                    {
                        messageDTO.WriteTime = DateTime.Now;

                        var formattedText = $"{msgsCount} | {DateHelpers.GetISO_8601String(messageDTO.StartTime)} | " +
                            $"{DateHelpers.GetISO_8601String(messageDTO.WriteTime)} | {messageDTO.Message}";
                        sb.AppendLine(formattedText);

                        FinishedMessages.Add(messageDTO);
                    }

                    writer.Write(sb.ToString());
                    writer.Flush();
                }

                Thread.Sleep(1500);

                return GetAndDeleteItem(messageId); // Always not null
            }
        }

        private MessageDTO? GetAndDeleteItem(string messageId)
        {
            var result = FinishedMessages.FirstOrDefault(x => x.MessageId == messageId);
            if (result != null)
            {
                FinishedMessages.RemoveAll(x => x.MessageId == messageId);
            }

            return result;
        }
    }
}

