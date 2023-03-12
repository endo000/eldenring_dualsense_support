using System;
using System.Net;
using System.Text.Json;
using dualsense_support;

public static class Triggers
{
    public static IPAddress localhost = new IPAddress(new byte[] { 127, 0, 0, 1 });

    public static string PacketToJson(Packet packet)
    {
        return JsonSerializer.Serialize(packet);
    }

    public static Packet JsonToPacket(string json)
    {
        return JsonSerializer.Deserialize<Packet>(json);
    }
}


public enum TriggerMode
{
    Normal = 0,
    GameCube = 1,
    VerySoft = 2,
    Soft = 3,
    Hard = 4,
    VeryHard = 5,
    Hardest = 6,
    Rigid = 7,
    VibrateTrigger = 8,
    Choppy = 9,
    Medium = 10,
    VibrateTriggerPulse = 11,
    CustomTriggerValue = 12,
    Resistance = 13,
    Bow = 14,
    Galloping = 15,
    SemiAutomaticGun = 16,
    AutomaticGun = 17,
    Machine = 18
}

public enum CustomTriggerValueMode
{
    OFF = 0,
    Rigid = 1,
    RigidA = 2,
    RigidB = 3,
    RigidAB = 4,
    Pulse = 5,
    PulseA = 6,
    PulseB = 7,
    PulseAB = 8,
    VibrateResistance = 9,
    VibrateResistanceA = 10,
    VibrateResistanceB = 11,
    VibrateResistanceAB = 12,
    VibratePulse = 13,
    VibratePulseA = 14,
    VibratePulsB = 15,
    VibratePulseAB = 16
}

public enum Trigger
{
    Invalid,
    Left,
    Right
}

public enum InstructionType
{
    Invalid,
    TriggerUpdate,
    RGBUpdate,
    PlayerLED,
    TriggerThreshold
}

public struct Instruction
{
    public InstructionType type { get; set; }
    public object[] parameters { get; set; }
}

public class Packet
{
    public MyInstruction[] Instructions { get; set; }

    public Packet()
    {
        // instructions = new Instruction[4];
        //
        // instructions[0].type = InstructionType.TriggerUpdate;
        // instructions[0].parameters = new object[] { 0, Trigger.Left, TriggerMode.Normal };
        //
        // instructions[1].type = InstructionType.RGBUpdate;
        // instructions[1].parameters = new object[] { 0, 255, 0, 0 };
        //
        // instructions[2].type = InstructionType.PlayerLED;
        // instructions[2].parameters = new object[] { 0, true, false, true, false, true };
        //
        // instructions[3].type = InstructionType.TriggerThreshold;
        // instructions[3].parameters = new object[] { 0, Trigger.Right, 255 };
    }
}