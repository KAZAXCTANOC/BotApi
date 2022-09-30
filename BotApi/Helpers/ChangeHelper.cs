using BotApi.Entities;
using BotApi.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace BotApi.Helpers
{
    public class ChangeHelper : IDeterminingSourceChange, IValidate, IStatusValidate
    {
        public List<MyUsersDatum> Users { get; set; } = new List<MyUsersDatum>();
        private Row Row { get; set; }
        private int Collumn { get; set; } = default;
        public ChangeHelper(int collumn, Row row, List<MyUsersDatum> _users) 
        {
            foreach (var user in _users)
            {
                if (user.UserName != null && user.TelegramId != null)
                {
                    Users.Add(user);
                }
            }

            Collumn = collumn;
            Row = row;
        }
        public PlaceholderType DeterminingSourceChange()
        {
            int collumn;

            if (Collumn == default)
            {
                return PlaceholderType.Error;
            }
            else
            {
                collumn = Collumn;
                if (collumn >= 1 && collumn <= 4)
                {
                    return PlaceholderType.Applicant;
                }

                if (collumn >= 4 && collumn <= 6)
                {
                    return PlaceholderType.Assignee_Assignee;

                }

                if (collumn >= 7 && collumn <= 8)
                {
                    return PlaceholderType.Executor;

                }

                if (collumn >= 9 && collumn <= 10)
                {
                    return PlaceholderType.Assignee_Acceptance;

                }

                return PlaceholderType.Error;
            }
        }

        public bool Validate(PlaceholderType type)
        {
#pragma warning disable CS0252 // Возможно, использовано непреднамеренное сравнение ссылок: для левой стороны требуется приведение☻
            switch (type)
            {
                case PlaceholderType.Applicant:
                    if (Row.B == "" || Row.C == "" || Row.D == "" || Row.E == ""  || 
                        Row.B == null || Row.C == null || Row.D == null || Row.E == null)
                        return false;
                    else
                        return true;

                case PlaceholderType.Assignee_Acceptance:
                    if (Row.J == null || Row.J == "")
                        return false;
                    else 
                        return true;

                case PlaceholderType.Executor:
                    if (Row.H == null || Row.I == null ||
                        Row.H == "" || Row.I == "")
                        return false;
                    else
                        return true;

                case PlaceholderType.Assignee_Assignee:
                    if (Row.F == null || Row.G == null ||
                        Row.F == "" || Row.G == "")
                        return false;
                    else
                        return true;
#pragma warning restore CS0252 // Возможно, использовано непреднамеренное сравнение ссылок: для левой стороны требуется приведение

                case PlaceholderType.Error:
                    return false;
            }

            return false;
        }

        public bool StatusValidate(string status)
        {
            if (string.IsNullOrEmpty(status)) return false;

            switch (status)
            {
                case "Не принято":
                    return false;

                case "Ответ принят":
                    return false;

                case "Срочно":
                    return true;

                case "Требуется уточнение":
                    return true;

                case "Ожидаем ответа":
                    return true;
            }

            return false;
        }

        #region async

        #region async create task
        /// <summary>
        /// Сообщение для Геннадия
        /// </summary>
        /// <param name="message"></param>
        /// <param name="row"></param>
        public void SendMessageForAppointeeAsync(Row row, object telegramId, int type = 0)
        {
            long userId = 718347004;
            if (telegramId != null)
            {
                userId = (long)Convert.ToDouble(telegramId);
            }
            switch (type)
            {
                case 0: 
                    {
                        try
                        {
                            ITelegramBotClient botClient = new TelegramBotClient("5388294357:AAEjA6aihEG1ZHBgbqcfrOEr9Z2owosI3iE");
                            botClient.SendTextMessageAsync((long)Convert.ToDouble(userId),
                                $"Была поставленна новая задача: \"{row.B}\"" +
                                $"\nот заявителя: {row.C} " +
                                $"\nдля задачи установлен срок ответа: {row.D} " +
                                $"\nЗайдите в форму обратной связи и назначьте отвественного " +
                                $"\nСсылка но форму обратной связи:\nhttps://docs.google.com/spreadsheets/d/1_JyRpD_rrZYKs7uMwWXmmw3zEAI1owmY4D7XNXorw04/edit#gid=0");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    }

                case 1:
                    {
                        try
                        {
                            ITelegramBotClient botClient = new TelegramBotClient("5388294357:AAEjA6aihEG1ZHBgbqcfrOEr9Z2owosI3iE");
                            botClient.SendTextMessageAsync((long)Convert.ToDouble(userId),
                                $"На запрос №{row.A} \"{row.B}\" был дан ответ \n\"{row.I}\"");
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;

                    }
            }
        }

        /// <summary>
        /// Сообщения для отвественного
        /// </summary>
        /// <param name="row"></param>
        /// <param name="message"></param>
        public void SendMessageForExecutorAsync(Row row)
        {
            ITelegramBotClient botClient = new TelegramBotClient("5388294357:AAEjA6aihEG1ZHBgbqcfrOEr9Z2owosI3iE");
            var user = Users.FirstOrDefault(el => el.UserName.ToString() == row.G.ToString());
            if (user != null)
            {
                try
                {
                    botClient.SendTextMessageAsync((long)Convert.ToDouble(user.TelegramId),
                        $"Вы были назначили ответственным за задачу \"{row.B}\"" +
                        $"\nПожалуйста зайдите в форму обратной сзяви и уточните информацию по задаче" +
                        $"\nПо завршению выполнения задачи пожалуйста заполните \"ответ на запрос\"" +
                        $"\nСсылка но форму обратной связи:\nhttps://docs.google.com/spreadsheets/d/1_JyRpD_rrZYKs7uMwWXmmw3zEAI1owmY4D7XNXorw04/edit#gid=0");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Сообщение для заявителя задачи
        /// </summary>
        /// <param name="row"></param>
        /// <param name="message"></param>
        public void SendMessageForApplicantAsync(Row row)
        {
            ITelegramBotClient botClient = new TelegramBotClient("5388294357:AAEjA6aihEG1ZHBgbqcfrOEr9Z2owosI3iE");
            var user = Users.FirstOrDefault(el => el.UserName.ToString() == row.C.ToString());
            if (user != null)
            {
                try
                {
                    botClient.SendTextMessageAsync((long)Convert.ToDouble(user.TelegramId),
                            $"У поставленной вами задачи №{row.A} \"{row.B}\" был изменён статус выполнения на \"{Row.J}\"" +
                            $"\nСсылка но форму обратной связи:\nhttps://docs.google.com/spreadsheets/d/1_JyRpD_rrZYKs7uMwWXmmw3zEAI1owmY4D7XNXorw04/edit#gid=0");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        #endregion

        #region async change task
        /// <summary>
        /// Сообщение об измениях (вопрос, срок ответа, объект, ответ от исполнителя) в ФОС для Геннадия
        /// </summary>
        /// <param name="message"></param>
        /// <param name="row"></param>
        public void SendCahngeMessageForAppointeeAsync(Row row, string oldValue, int column, object telegramId)
        {
            long userId = 718347004;
            if (telegramId != null)
            {
                userId = (long)Convert.ToDouble(telegramId);
            }

            ITelegramBotClient botClient = new TelegramBotClient("5388294357:AAEjA6aihEG1ZHBgbqcfrOEr9Z2owosI3iE");
            switch (column)
            {
                case 1: 
                    {
                        try
                        {
                            botClient.SendTextMessageAsync((long)Convert.ToDouble(userId),
                                $"Постановка задачи {oldValue} была изменена на {row.B} " +
                                "\nДля уточнения информации зайдите в форму обратной связи" +
                                $"\nСсылка но форму обратной связи:\nhttps://docs.google.com/spreadsheets/d/1_JyRpD_rrZYKs7uMwWXmmw3zEAI1owmY4D7XNXorw04/edit#gid=0");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    }

                case 3:
                    {
                        try
                        {
                            botClient.SendTextMessageAsync((long)Convert.ToDouble(userId),
                               $"Требуемый срок ответа задачи №{row.A} \"{row.B}\" был изменена на {row.D} " +
                               "\nДля уточнения информации зайдите в форму обратной связи" +
                               $"\nСсылка но форму обратной связи:\nhttps://docs.google.com/spreadsheets/d/1_JyRpD_rrZYKs7uMwWXmmw3zEAI1owmY4D7XNXorw04/edit#gid=0");
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    }

                case 4:
                    {
                        try
                        {
                            botClient.SendTextMessageAsync((long)Convert.ToDouble(userId),
                               $"Объект {oldValue} была изменена на {row.E} " +
                               "\nДля уточнения информации зайдите в форму обратной связи" +
                               $"\nСсылка но форму обратной связи:\nhttps://docs.google.com/spreadsheets/d/1_JyRpD_rrZYKs7uMwWXmmw3zEAI1owmY4D7XNXorw04/edit#gid=0");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;

                    }

            }
        }

        /// <summary>
        /// Сообщения для отвественного
        /// </summary>
        /// <param name="row"></param>
        /// <param name="message"></param>
        public void SendCahngeMessageForExecutorAsync(Row row, string oldValue)
        {
            ITelegramBotClient botClient = new TelegramBotClient("5388294357:AAEjA6aihEG1ZHBgbqcfrOEr9Z2owosI3iE");
            var newUser = Users.FirstOrDefault(el => el.UserName.ToString() == row.G.ToString());
            var oldUser = Users.FirstOrDefault(el => el.UserName.ToString() == oldValue);

            if (newUser != null && oldUser != null)
            {
                try
                {
                    botClient.SendTextMessageAsync((long)Convert.ToDouble(oldUser.TelegramId),
                        $"С вас убрали ответсвенность за задачу {row.B}" +
                        $"\nСсылка но форму обратной связи:\nhttps://docs.google.com/spreadsheets/d/1_JyRpD_rrZYKs7uMwWXmmw3zEAI1owmY4D7XNXorw04/edit#gid=0");

                    SendMessageForExecutorAsync(row);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Сообщение для заявителя задачи
        /// </summary>
        /// <param name="row"></param>
        /// <param name="message"></param>
        public void SendCahngeMessageForApplicantAsync(Row row)
        {
            ITelegramBotClient botClient = new TelegramBotClient("5388294357:AAEjA6aihEG1ZHBgbqcfrOEr9Z2owosI3iE");
            var user = Users.FirstOrDefault(el => el.UserName.ToString() == row.D.ToString());
            if (user != null)
            {
                try
                {
                    botClient.SendTextMessageAsync((long)Convert.ToDouble(user.TelegramId),
                        $"У поставленной вами задачи \"{row.B}\" был изменён статус выполнения на \"{Row.J}\"");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        #endregion

        #endregion
    }
}
