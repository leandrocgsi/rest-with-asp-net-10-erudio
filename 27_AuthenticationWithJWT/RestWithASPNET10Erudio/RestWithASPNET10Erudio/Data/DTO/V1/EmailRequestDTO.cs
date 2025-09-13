namespace RestWithASPNET10Erudio.Data.DTO.V1
{
    public class EmailRequestDTO
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}