namespace ExampleApp.Api.Controllers.Models;

public class StudentModel
{
    public StudentModel()
    {

    }

    public StudentModel(int id, string fullName, string badge, string residenceStatus, int courseCount)
    {
        Id = id;
        FullName = fullName;
        Badge = badge;
        ResidenceStatus = residenceStatus;
        CourseCount = courseCount;
    }

    public int Id { get; set; }
    public  string FullName { get; set; }
    public  string Badge { get; set; }
    public  string ResidenceStatus { get; set; }
    public int CourseCount { get; set; }
}
