using BotApi.Entities;
using System.Threading.Tasks;

namespace BotApi.Helpers.Interfaces
{
    public interface ISendMessage
    {
        /// <summary>
        /// Сообщение Геннадию 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="row"></param>
        public Task SendMessageForAppointee(string message, Row row); //1

        /// <summary>
        /// Сообщение исполнителю\ответсвенному 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="row"></param>
        public Task SendMessageForExecutor(string message, Row row); //2

        /// <summary>
        /// Сообщение заявителю задачи 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="row"></param>
        public Task SendMessageForApplicant(string message, Row row); //3
    }
}
