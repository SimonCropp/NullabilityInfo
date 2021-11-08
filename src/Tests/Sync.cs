using System.Linq;
using System.Net.Http;
using VerifyTests;
using Xunit;

public class Sync
{
    static string dir = Path.Combine(AttributeReader.GetSolutionDirectory(typeof(Sync).Assembly), "NullabilityInfo");
    static Dictionary<string, string> replacements = new()
    {
        ["public enum"] = "enum",
        ["public sealed class"] = "sealed class",
    };

    [Theory]
    [InlineData("https://raw.githubusercontent.com/dotnet/runtime/main/src/libraries/System.Private.CoreLib/src/System/Reflection/NullabilityInfoContext.cs")]
    [InlineData("https://raw.githubusercontent.com/dotnet/runtime/main/src/libraries/System.Private.CoreLib/src/System/Reflection/NullabilityInfo.cs")]
    public async Task Run(string source)
    {
        var uri = new Uri(source);
        var lines = await ReadLinesAsync(uri);

        lines.Insert(3, "");
        lines.Insert(3, "#nullable enable");

        var path = Path.Combine(dir, uri.Segments.Last() + ".pp");
        File.Delete(path);
        const string lineEnding = "\n";
        await File.WriteAllTextAsync(path, string.Join(lineEnding, lines) + lineEnding);
    }

    static async Task<List<string>> ReadLinesAsync(Uri uri)
    {
        using var client = new HttpClient();
        await using var stream = await client.GetStreamAsync(uri);
        using var streamReader = new StreamReader(stream);
        string line;
        var lines = new List<string>();
        while ((line = await streamReader.ReadLineAsync()) != null)
        {
            foreach (var (original, replacement) in replacements)
            {
                line = line.Replace(original, replacement);
            }
            lines.Add(line);
        }
        return lines;
    }
}