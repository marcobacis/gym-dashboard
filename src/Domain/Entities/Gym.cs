namespace Domain.Entities;

public class Gym
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    
    public string Name { get; set; } = string.Empty;
    
    public string Username { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public string ApiPrefix { get; set; } = string.Empty;
    
    public bool CollectEnabled { get; set; } = true;
}