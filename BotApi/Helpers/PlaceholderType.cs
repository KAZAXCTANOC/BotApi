namespace BotApi.Helpers
{
    public enum PlaceholderType
    {
        /// <summary>
        /// Заявитель
        /// </summary>
        Applicant,

        /// <summary>
        /// Назначенец
        /// </summary>
        Appointee,

        /// <summary>
        /// Исполнитель
        /// </summary>
        Executor,

        /// <summary>
        /// Назначенец_Назначенец
        /// </summary>
        Assignee_Assignee,

        /// <summary>
        /// Назначенец_Приемка
        /// </summary>
        Assignee_Acceptance,

        /// <summary>
        /// Ошибка
        /// </summary>
        Error 
    }
}
