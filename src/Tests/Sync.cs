using System.Net.Http;
using VerifyTests;
using Xunit;

public class Sync
{
    static string solutionDir = AttributeReader.GetSolutionDirectory();
    static string dir = Path.Combine(solutionDir, "NullabilityInfo");

    [Fact]
    public async Task Run()
    {
        var client = new HttpClient();
        var infoContext = await client.GetStringAsync("https://raw.githubusercontent.com/dotnet/runtime/main/src/libraries/System.Private.CoreLib/src/System/Reflection/NullabilityInfoContext.cs");

        infoContext = infoContext
            .Replace("[^1]", ".Last()")
            .Replace(".IsGenericMethodParameter", ".IsGenericMethodParameter()");

        var info = await client.GetStringAsync("https://raw.githubusercontent.com/dotnet/runtime/main/src/libraries/System.Private.CoreLib/src/System/Reflection/NullabilityInfo.cs");

        WriteSourceOnlyFiles(infoContext, info);
    }

    static void WriteSourceOnlyFiles(string infoContext, string info)
    {
        infoContext = $@"#nullable enable
using System.Linq;
{infoContext}";
        infoContext = MakeInternal(infoContext);
        OverWrite(infoContext, "NullabilityInfoContext.cs.pp");

        info = $@"#nullable enable
using System.Linq;
{info}";
        info = MakeInternal(info);
        OverWrite(info, "NullabilityInfo.cs.pp");
    }

    static string MakeInternal(string source)
    {
        return source
            .Replace("public enum", "enum")
            .Replace("public sealed class", "sealed class");
    }

    static void OverWrite(string? content, string file)
    {
        var path = Path.Combine(dir, file);
        File.Delete(path);
        File.WriteAllText(path, content);
    }
}