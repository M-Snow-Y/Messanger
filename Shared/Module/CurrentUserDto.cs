namespace Messanger.Shared.Module;

public class CurrentUserDto
{
    public int Id{get;set;}
    public string Name{get;set;} = string.Empty;
    public string numberPhone{get;set;} = string.Empty;
    public string UserName{get;set;} = string.Empty;
    public string? AvatarUrl{get;set;}
}