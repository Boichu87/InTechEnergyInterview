using System.Text.Json.Serialization;

namespace ExampleApp.Api.Controllers.Models;

public record CourseModel
{
    public CourseModel(string id, string description, KeyNameModel semester, KeyNameModel professor)
    {
        Id = id;
        Description = description;
        Semester = semester;
        Professor = professor;
    }

    public string Id { get; set; }
    public string Description { get; set; }
    public KeyNameModel Semester { get; set; }

    [JsonPropertyName("Lecturer")] ///Task 5
    public KeyNameModel Professor { get; set; }
}
