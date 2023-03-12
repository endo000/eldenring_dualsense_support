namespace dualsense_support;

public class WeaponData
{
    public WeaponData()
    {
        Id = -1;
        Name = "Unknown";
    }

    public WeaponData(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public WeaponData(int id, string name, string instructions)
    {
        Id = id;
        Name = name;
        Instructions = instructions;
    }

    public int Id { get; set; }
    public string Name { get; set; }

    public string? Instructions { get; set; }
}