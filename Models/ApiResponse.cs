namespace ZUSContributionImporter.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Model { get; set; }
    }
}