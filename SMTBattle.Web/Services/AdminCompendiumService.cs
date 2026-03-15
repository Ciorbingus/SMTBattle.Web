using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Components.Forms;
using SMTBattle.Web.Models;

public class AdminCompendiumService
{
    private readonly IWebHostEnvironment _env;
    private readonly string _path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "compendium.json");
    private readonly string _imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "demons");

    public AdminCompendiumService(IWebHostEnvironment env)
    {
        _env = env;
        if (!Directory.Exists(_imagesPath)) Directory.CreateDirectory(_imagesPath);
    }

    public async Task SaveToCompendium(Demon newDemon)
    {
        List<Demon> compendium = new();

        if (File.Exists(_path))
        {
            var json = await File.ReadAllTextAsync(_path);
            if (!string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    compendium = JsonSerializer.Deserialize<List<Demon>>(json) ?? new();
                }
                catch {  }
            }
        }

        var existing = compendium.FirstOrDefault(d => d.Name == newDemon.Name);
        if (existing != null) compendium.Remove(existing);
        compendium.Add(newDemon);

        var finalJson = JsonSerializer.Serialize(compendium, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_path, finalJson);
    }

    public async Task<string> UploadDemonImage(IBrowserFile file, string demonName)
    {
        var safeName = demonName.Replace(" ", "_").ToLower();
        var extension = Path.GetExtension(file.Name);
        var fileName = $"{safeName}{extension}";
        var physicalPath = Path.Combine(_imagesPath, fileName);

        using var stream = file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 2);
        using var fileStream = new FileStream(physicalPath, FileMode.Create);
        await stream.CopyToAsync(fileStream);

        return $"/images/demons/{fileName}";
    }

    public async Task<List<Demon>> GetFullCompendium()
    {
        if (!File.Exists(_path)) return new List<Demon>();

        var json = await File.ReadAllTextAsync(_path);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Demon>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<Demon>>(json) ?? new List<Demon>();
        }
        catch (JsonException)
        {
            return new List<Demon>();
        }
    }
}