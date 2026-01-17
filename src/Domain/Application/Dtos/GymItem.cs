using Domain.Entities;

namespace Domain.Application.Dtos;

public class GymItem
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public static GymItem FromDomain(Gym gym)
    {
        return new GymItem
        {
            Id = gym.Id,
            Name = gym.Name
        };
    }
}