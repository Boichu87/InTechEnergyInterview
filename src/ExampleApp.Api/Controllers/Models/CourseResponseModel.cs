using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ExampleApp.Api.Controllers.Models;

public class CourseResponseModel
{
    public string Key { get; set; }
    public string Name { get; set; }
 
    [JsonProperty("Lecturer")]
    public ProfessorResponseModel? Professor { get; set; }
}
