namespace EmployeeManagementAPI.Models.Entities
{
    public class Note
    {
        public int NoteId { get; set; }
        public int TaskId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
