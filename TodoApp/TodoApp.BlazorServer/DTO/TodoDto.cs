namespace TodoApp.BlazorServer.DTO
{
    public class TodoDto
    {
        public Guid Id { get; set; }
        public long ActivitiesNo { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
