namespace EmployeeManagementAPI.Models
{
    public class DocumentDTO
    {
        public int DocumentId { get; set; }
        public int TaskId { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
