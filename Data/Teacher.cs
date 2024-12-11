using UsersStudentsMVCApp.Data;

namespace SchoolApp.Data;

public class Teacher : BaseEntity
{
    public int Id { get; set; }
    public string Institution { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
}
