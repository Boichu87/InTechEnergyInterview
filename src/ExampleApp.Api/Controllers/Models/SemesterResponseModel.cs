namespace ExampleApp.Api.Controllers.Models;

public class SemesterResponseModel
{
    public string Key { get; set; }
    public string Name { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public List<CourseResponseModel>? Courses { get; set; }
}
