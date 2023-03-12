using dualsense_support;

DsxUdpClient client;
EldenRingProcess process;
try
{
    client = new DsxUdpClient();
    process = new EldenRingProcess();
}
catch (Exception e)
{
    Console.WriteLine(e);
    return 1;
}

var packet = string.Empty;

int[] oldPrimaryWep = { -1, -1 };

while (true)
{
    WeaponData[] weapons;
    try
    {
        weapons = process.GetCurrentWeapons();
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return 1;
    }

    for (var i = 0; i < 2; i++)
    {
        if (oldPrimaryWep[i] != weapons[i].Id)
        {
            oldPrimaryWep[i] = weapons[i].Id;

            Console.WriteLine("Changed ID: {0}", weapons[i].Id);

            Console.WriteLine("Current {0} weapon {1}", i == 0 ? "left" : "right", weapons[i].Name);

            var packetInstructions = weapons[i].Instructions;
            if (packetInstructions != null)
            {
                Console.WriteLine("Change Instructions");
                packet = packetInstructions;
            }
        }

        client.Send(packet);
    }
}