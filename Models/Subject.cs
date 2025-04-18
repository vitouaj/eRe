namespace ERE.Models;


public class Subject
{
    public SubjectId Id { get; set; }
    public string Name {get; set;}
    public Subject(SubjectId id)
    {
        Id = id;
        Name = id.ToString();
    }
    public Subject() { }
}

public enum SubjectId
{
    MATH = 1, SCIENCE, ENGLISH, HISTORY, GEOGRAPHY, PHYSICAL_EDUCATION, ART, MUSIC
}