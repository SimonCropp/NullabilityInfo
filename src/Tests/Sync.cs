using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using VerifyTests;
using Xunit;

public class Sync
{
    static string dir = Path.Combine(AttributeReader.GetSolutionDirectory(typeof(Sync).Assembly), "NullabilityInfo");

    [Fact]
    public async Task Run()
    {
        var client = new HttpClient();
        var nullabilityInfoContext = await client.GetStringAsync("https://raw.githubusercontent.com/dotnet/runtime/main/src/libraries/System.Private.CoreLib/src/System/Reflection/NullabilityInfoContext.cs");
        nullabilityInfoContext = nullabilityInfoContext.Replace("public sealed class", "sealed class");
        await OverWrite(nullabilityInfoContext, "NullabilityInfoContext.cs.pp");
        var nullabilityInfo = await client.GetStringAsync("https://raw.githubusercontent.com/dotnet/runtime/main/src/libraries/System.Private.CoreLib/src/System/Reflection/NullabilityInfo.cs");
        nullabilityInfo = nullabilityInfo
            .Replace("public enum", "enum")
            .Replace("public sealed class", "sealed class");
        await OverWrite(nullabilityInfo, "NullabilityInfo.cs.pp");
    }

    static async Task OverWrite(string? content, string file)
    {
        var path = Path.Combine(dir, file);
        File.Delete(path);
        await File.WriteAllTextAsync(path, content);
    }
}