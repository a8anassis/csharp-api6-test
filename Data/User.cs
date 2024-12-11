using SchoolApp.Core.Enums;
using UsersStudentsMVCApp.Data;

namespace SchoolApp.Data;

public class User : BaseEntity
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public UserRole? UserRole { get; set; } = null!;

    public virtual Student? Student { get; set; }
    public virtual Teacher? Teacher { get; set; }
}
