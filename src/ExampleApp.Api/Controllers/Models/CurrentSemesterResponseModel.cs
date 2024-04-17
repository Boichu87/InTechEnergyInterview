namespace ExampleApp.Api.Controllers.Models;
/// <summary>
/// TASK 4 Models
/// </summary>
public class CurrentSemesterResponseModel
{
    public SemesterResponseModel semester { get; set; }
}

public class SemesterResponseModel
{
    public string Key { get; set; }
    public string Name { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public List<CourseResponseModel>? Courses { get; set; }
}


public class CourseResponseModel
{
    public string Key { get; set; }
    public string Name { get; set; }
    public ProfessorResponseModel? Professor { get; set; }
}

public class ProfessorResponseModel
{
    public string Key { get; set; }
    public string Name { get; set; }
}
