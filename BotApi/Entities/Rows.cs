using System.Collections.Generic;

namespace BotApi.Entities
{
    public class Rows
    {
        public List<Row> row { get; set; }
        public NeedRow needRow { get; set; }
        public Changes changes { get; set; }
        public List<MyUsersDatum> myUsersData { get; set; }
        public object appointeeName { get; set; }
    }
}
