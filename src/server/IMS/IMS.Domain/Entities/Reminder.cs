namespace IMS.Domain.Entities;

public class Reminder : BaseEntity
{
    public required string Email { get; set; }
   
    public DateTime ScheduleAt { get; set; }
    
    public required string BackgroundJobId { get; set; }
    
    public required string Title { get; set; }
}