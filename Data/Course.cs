namespace SchoolApp.Data;

public class Course
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public int? TeacherId { get; set; }

    public virtual Teacher? Teacher { get; set; }
    public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
}
