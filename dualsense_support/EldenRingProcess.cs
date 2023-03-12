using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace dualsense_support;

public class EldenRingProcess
{
    [DllImport("kernel32.dll")]
    private static extern nint OpenProcess(uint processAccess, bool bInheritHandle, uint processId);

    [DllImport("kernel32.dll")]
    private static extern bool ReadProcessMemory(nint hProcess,
        nint lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out nint lpNumberOfBytesRead);

    private const int ProcessWmRead = 0x0010;

    private readonly nint _hProcess;
    private readonly ProcessModule _processModule;

    private const string ProcessName = "eldenring";
    private const string FileName = "weapons_data.json";

    private readonly string[] _pattern = "48 8B 05 ?? ?? ?? ?? 48 85 C0 74 05 48 8B 40 58 C3 C3".Split(' ');

    private nint _gameDataManPointer;
    private nint _gameDataMan;
    private nint _primaryWep;
    private readonly int[] _primaryWepOffsets = { 0x39C, 0x3A0 };

    private WeaponData[] _weaponData = null!;

    public EldenRingProcess()
    {
        var processes = Process.GetProcessesByName(ProcessName);
        if (processes.Length <= 0)
        {
            Console.WriteLine("Can't open Elden Ring process. Is it running?");
            Environment.Exit(1);
        }

        var process = processes[0];
        _hProcess = OpenProcess(ProcessWmRead, false, (uint)process.Id);
        _processModule = process.MainModule ?? throw new InvalidOperationException();

        ReadWeaponsDataFile();
        ReadPointers();
    }

    private void ReadWeaponsDataFile()
    {
        var jsonString = File.ReadAllText(FileName);
        _weaponData = JsonSerializer.Deserialize<WeaponData[]>(jsonString) ?? throw new InvalidOperationException();
    }

    private void ReadPointers()
    {
        var moduleMemory = new byte[_processModule.ModuleMemorySize];
        if (!ReadProcessMemory(_hProcess, _processModule.BaseAddress, moduleMemory,
                _processModule.ModuleMemorySize, out _))
        {
            Console.WriteLine("Can't read main module. Exiting.");
            Environment.Exit(1);
        }

        for (var i = 0; i < moduleMemory.Length; i++)
        {
            var found = true;
            for (var j = 0; j < _pattern.Length; j++)
            {
                found = _pattern[j] == "??" || moduleMemory[i + j] == byte.Parse(_pattern[j], NumberStyles.HexNumber);

                if (!found) break;
            }

            if (!found) continue;

            var finder = _processModule.BaseAddress + i;
            _gameDataManPointer = finder + BitConverter.ToInt32(moduleMemory, i + 3) + 7;
            break;
        }

        var buffer = new byte[8];
        if (!ReadProcessMemory(_hProcess, _gameDataManPointer, buffer, buffer.Length, out _))
        {
            Console.WriteLine("Can't read GameDataMan address. Exiting.");
            Environment.Exit(1);
        }

        _gameDataMan = new nint(BitConverter.ToInt64(buffer, 0));

        var primaryWepAddress = _gameDataMan + 0x08;
        if (!ReadProcessMemory(_hProcess, primaryWepAddress, buffer, buffer.Length, out _))
        {
            Console.WriteLine("Can't read PrimaryWepPointer. Exiting.");
            Environment.Exit(1);
        }

        _primaryWep = new nint(BitConverter.ToInt64(buffer, 0));
    }

    public WeaponData[] GetCurrentWeapons()
    {
        var currentWeapons = new WeaponData[2];
        for (var i = 0; i < 2; i++)
        {
            var baseAddress = new nint(_primaryWep + _primaryWepOffsets[i]);
            var buffer = new byte[4];
            if (!ReadProcessMemory(_hProcess, baseAddress, buffer, buffer.Length,
                    out _))
            {
                Console.WriteLine("Can't read current weapon. Exiting.");
                Environment.Exit(1);
            }

            var weaponId = BitConverter.ToInt32(buffer, 0) / 10000 * 10000;
            currentWeapons[i] = _weaponData.FirstOrDefault(data => data.Id == weaponId, new WeaponData());
        }

        return currentWeapons;
    }
}