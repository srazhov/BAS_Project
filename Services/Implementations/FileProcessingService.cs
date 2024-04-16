using System.Text;
using BAS_Project.DTOs;
using BAS_Project.Helpers;
using Microsoft.Extensions.Options;

namespace BAS_Project.Services.Implementations
{
    public class FileProcessingService : IFileProcessingService
    {
        private static readonly object queueLocker = new();

        private static readonly object mainLocker = new();

        private Queue<MessageDTO> Messages { get; set; }

        private List<MessageDTO> FinishedMessages { get; set; }

        private IWebHostEnvironment AppEnvironment { get; set; }

        private MyConfigs MyConfigs { get; set; }



        public FileProcessingService(IWebHostEnvironment appEnvironment, IOptions<MyConfigs> myConfigs)
        {
            Messages = new Queue<MessageDTO>();
            FinishedMessages = new List<MessageDTO>();
            AppEnvironment = appEnvironment;
            MyConfigs = myConfigs.Value;
        }

        public string EnqueueMessage(string message, DateTime requestTime)
        {
            lock (queueLocker)
            {
                var messageDTO = new MessageDTO()
                {
                    MessageId = Guid.NewGuid().ToString(),
                    StartTime = requestTime,
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
                    for (int i = 0; i < MyConfigs.MaxSimultaneousOperations && Messages.TryDequeue(out var messageDTO); i++)
                    {
                        messageDTO.WriteTime = DateTime.Now;

                        var formattedText = GetLineText(msgsCount, messageDTO);
                        sb.AppendLine(formattedText);

                        FinishedMessages.Add(messageDTO);
                    }

                    writer.Write(sb.ToString());
                }

                Thread.Sleep(MyConfigs.SleepTime);

                return GetAndDeleteItem(messageId); // Always not null
            }
        }

        private static string GetLineText(int msgsCount, MessageDTO messageDTO)
        {
            return $"{msgsCount} | {DateHelpers.GetISO_8601String(messageDTO.StartTime)} | " +
                $"{DateHelpers.GetISO_8601String(messageDTO.WriteTime)} | {messageDTO.Message}";
        }

        private MessageDTO? GetAndDeleteItem(string messageId)
        {
            var result = FinishedMessages.FirstOrDefault(x => x.MessageId == messageId);
            if (result != null)
            {
                FinishedMessages.Remove(result);
            }

            return result;
        }
    }
}

