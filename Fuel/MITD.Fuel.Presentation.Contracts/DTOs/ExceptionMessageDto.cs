namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class ExceptionMessageDto
    {
        public long LogId { get; set; }
        public string Message { get; set; }
        public ExceptionMessageTypeDto ExceptionMessageType { get; set; }
        public string Parameter { get; set; }

        public string GetJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
           
        }
    }
}