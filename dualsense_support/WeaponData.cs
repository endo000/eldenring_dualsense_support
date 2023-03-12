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

    public int Id { get; set; }
    public string Name { get; set; }

    public string? Instructions { get; set; }
}

public class MyInstruction
{
    public int Type { get; set; }
    public int[] Parameters { get; set; }
}