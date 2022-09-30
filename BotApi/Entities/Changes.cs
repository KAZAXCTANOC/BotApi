namespace BotApi.Entities
{
    public class Changes
    {
        public object oldValue { get; set; }
        public object newValue { get; set; }
        public object col { get; set; }
        public object row { get; set; }
        public object address { get; set; }
        public object activeSheet { get; set; }
    }
}
