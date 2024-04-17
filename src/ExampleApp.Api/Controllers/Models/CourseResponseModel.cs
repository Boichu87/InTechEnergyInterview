namespace ExampleApp.Api.Controllers.Models;

public class CourseResponseModel
{
    public string Key { get; set; }
    public string Name { get; set; }
    public ProfessorResponseModel? Professor { get; set; }
}
