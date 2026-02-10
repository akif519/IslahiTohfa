using IslahiTohfa.Domain.Common;

namespace IslahiTohfa.Domain.Entities;

public class Like : BaseEntity
{
    // Foreign Keys
    public int BookId { get; set; }
    public string UserId { get; set; } = string.Empty;
    
    // Navigation Properties
    public virtual Book Book { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
}
