// https://raw.githubusercontent.com/MaxTheMiracle/Dark-Souls-3-Parts-Files/master/Elden%20Ring

using System.Text.Encodings.Web;
using System.Text.Json;
using dualsense_support;

const string eldenRingWeaponsUri =
    "https://raw.githubusercontent.com/MaxTheMiracle/Dark-Souls-3-Parts-Files/master/Elden%20Ring";
const string outputFilename = "weapon_data.json";

var cli = new HttpClient();
var rawData = await
    cli.GetStringAsync(eldenRingWeaponsUri);

var rawDataLines = rawData.Split('\n');

var weapons = new List<WeaponData>();

var isWeaponSection = false;
foreach (var rawLine in rawDataLines)
{
    if (rawLine.Contains("Weapons"))
    {
        isWeaponSection = true;
        continue;
    }

    if (isWeaponSection)
    {
        var splitRawLine = rawLine.Split('\t');
        var escapedRawLine = splitRawLine.Where(splitElement => splitElement != "").ToList();

        if (escapedRawLine.Count != 3) continue;

        var weapon = new WeaponData(int.Parse(escapedRawLine[0]), escapedRawLine[2]);
        weapons.Add(weapon);
    }
}

var jsonString = JsonSerializer.Serialize(weapons,
    new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
File.WriteAllText(outputFilename, jsonString);