﻿using System.Net.Http;
using VerifyTests;
using Xunit;

public class Sync
{
    static string solutionDir = AttributeReader.GetSolutionDirectory();
    static string sourceOnlyDir = Path.Combine(solutionDir, "Nullability.Source");
    static string libDir = Path.Combine(solutionDir, "Nullability");

    [Fact]
    public async Task Run()
    {
        var client = new HttpClient();
        var infoContext = await client.GetStringAsync("https://raw.githubusercontent.com/dotnet/runtime/main/src/libraries/System.Private.CoreLib/src/System/Reflection/NullabilityInfoContext.cs");

        infoContext = infoContext
            .Replace("[^1]", ".Last()")
            .Replace(".IsGenericMethodParameter", ".IsGenericMethodParameter()")
            .Replace("SR.NullabilityInfoContext_NotSupported", "\"NullabilityInfoContext is not supported\"")
            .Replace(
                "if (info.HasSameMetadataDefinitionAs(member))",
                "if (NullabilityInfoExtensions.HasSameMetadataDefinitionAs(info,member))")
            .Replace(
                "return type.GetGenericTypeDefinition().GetMemberWithSameMetadataDefinitionAs(member);",
                "return NullabilityInfoExtensions.GetMemberWithSameMetadataDefinitionAs(type.GetGenericTypeDefinition(), member);");

        infoContext = infoContext.Replace("!!", "");

        var info = await client.GetStringAsync("https://raw.githubusercontent.com/dotnet/runtime/main/src/libraries/System.Private.CoreLib/src/System/Reflection/NullabilityInfo.cs");
        info = info.Replace("!!", "");

        WriteSourceOnlyFiles(infoContext, info);
        WriteSourceLibFiles(infoContext, info);
    }

    static void WriteSourceLibFiles(string infoContext, string info)
    {
        infoContext= FixNames(infoContext)
            .Replace("class NullabilityInfoContext", "class NullabilityInfoContextEx");
        OverWriteLib(infoContext, "NullabilityInfoContext.cs");

        var extensions = File.ReadAllText(Path.Combine(sourceOnlyDir, "NullabilityInfoExtensions.cs.pp"));
        extensions = FixNames(extensions)
            .Replace("    internal", "    public");
        OverWriteLib(extensions, "NullabilityInfoExtensions.cs");

        info = FixNames(info)
            .Replace("class NullabilityInfo", "class NullabilityInfoEx")
            .Replace("internal NullabilityInfo(", "internal NullabilityInfoEx(")
            .Replace("public enum NullabilityState", "public enum NullabilityStateEx");

        OverWriteLib(info, "NullabilityInfo.cs");
    }

    static string FixNames(string extensions) =>
        extensions
            .Replace(
                "namespace System.Reflection",
                @"
using NullabilityInfoContext= Nullability.NullabilityInfoContextEx;
using NullabilityInfo = Nullability.NullabilityInfoEx;
using NullabilityState = Nullability.NullabilityStateEx;
namespace Nullability");

    static void OverWriteLib(string? content, string file)
    {
        var path = Path.Combine(libDir, file);
        File.Delete(path);
        File.WriteAllText(path, content);
    }

    static void WriteSourceOnlyFiles(string infoContext, string info)
    {
        infoContext = $@"#nullable enable
using System.Linq;
{infoContext}";
        infoContext = MakeInternal(infoContext);
        OverWriteSourceOnly(infoContext, "NullabilityInfoContext.cs.pp");

        info = $@"#nullable enable
using System.Linq;
{info}";
        info = MakeInternal(info);
        OverWriteSourceOnly(info, "NullabilityInfo.cs.pp");
    }

    static string MakeInternal(string source) =>
        source
            .Replace("public enum", "enum")
            .Replace("public sealed class", "sealed class");

    static void OverWriteSourceOnly(string? content, string file)
    {
        var path = Path.Combine(sourceOnlyDir, file);
        File.Delete(path);
        File.WriteAllText(path, content);
    }
}