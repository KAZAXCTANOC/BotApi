using BotApi.Helpers;

namespace BotApi.Entities
{
    public class ViewUser : User
    {
        public ViewUser(User user)
        {
            this.UserRole = user.UserRole;
            this.UserTelegramId = user.UserTelegramId;
            this.UserNameForFOS = user.UserNameForFOS;
            this.Id = user.Id;
        }
        public PlaceholderType PlaceholderType { get; set; }
    } 
}
