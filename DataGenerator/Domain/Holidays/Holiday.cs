namespace DataGenerator.Domain.Holidays;

public class Holiday
{
    public int Year => Date.Year;

    public DateOnly Date { get; set; }

    public string Name { get; set; }
}
