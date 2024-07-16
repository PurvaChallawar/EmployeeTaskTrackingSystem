﻿namespace EmployeeManagementAPI.Models
{
    public class TaskDTO
    {
        public string AssignedEmployeeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
    }
}