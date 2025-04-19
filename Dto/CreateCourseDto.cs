using ERE.Models;

namespace ERE.DTO;

public class CreateCourseDto
{
    public string TeacherId { get; set; }
    public LevelId Level { get; set; }
    public float MaxScore { get; set; }
    public float PassingRate { get; set; }
}