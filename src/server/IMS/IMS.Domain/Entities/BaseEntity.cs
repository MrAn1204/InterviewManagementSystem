

namespace IMS.Domain.Entities;

public class BaseEntity : IBaseEntity
{
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedDate { get; set; }

    public bool IsDelete {get; set;}
}