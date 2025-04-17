namespace ERE.Models;


public class Role {
    public RoleId Id {get; set;}
    public string Name {get; set;}

    public Role(RoleId id)
    {
        Id = id;
        Name = id.ToString();
    }
    public Role() { }
}
public enum RoleId
{
    STUDENT = 1, TEACHER, PARENT
}
