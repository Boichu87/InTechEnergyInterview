using System.Text.Json.Serialization;

namespace ExampleApp.Api.Controllers.Models;

public class CourseResponseModel
{
    public string Key { get; set; }
    public string Name { get; set; }

    [JsonPropertyName("Lecturer")] ///Task 5
    public ProfessorResponseModel? Professor { get; set; }
}
