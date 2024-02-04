namespace Core.Models
{
    public class AdrSettings
    {
        public string Name { get; set; }
        public string Template { get; set; }
        public string[] AvailableStatus { get; set; }
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}
