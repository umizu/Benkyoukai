namespace Benkyoukai.Models;

public class Session
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public DateTime LastModifiedDateTime { get; set; }
}
