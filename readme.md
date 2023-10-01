# <img src="/src/icon.png" height="30px"> NullabilityInfo

[![Build status](https://ci.appveyor.com/api/projects/status/636i70gvxfuwdq38?svg=true)](https://ci.appveyor.com/project/SimonCropp/NullabilityInfo)
[![Nullability.Source NuGet Status](https://img.shields.io/nuget/v/Nullability.Source.svg?label=Nullability.Source)](https://www.nuget.org/packages/Nullability.Source/)
[![Nullability NuGet Status](https://img.shields.io/nuget/v/Nullability.svg?label=Nullability)](https://www.nuget.org/packages/Nullability/)

Exposes top-level nullability information from reflection.

**See [Milestones](../../milestones?state=closed) for release notes.**

This feature is [included in net6](https://github.com/dotnet/runtime/issues/29723). This project exposes the APIs to lower runtime. It supports `netstandard2.0` and up.

This project ships two packages:


## Nullability.Source

https://nuget.org/packages/Nullability.Source/

A source-only nuget designed to be compatible for libraries that are targeting multiple frameworks including `net6`. In `net5` and below the source files shipped in this nuget are used. In `net6` and up the types from `System.Runtime.dll` are used.


## Nullability

https://nuget.org/packages/Nullability/

A traditional nuget that ships a single assembly `Nullability.dll`. Since this project syncs with the current master from https://github.com/dotnet/runtime, it contains fixes that may not be included in the currently `System.Runtime.dll`. Use the Nullability package to get those fixes.

To prevent name conflicts the following has been changed:

 * `System.Reflection.NullabilityState` => `Nullability.NullabilityStateEx`
 * `System.Reflection.NullabilityInfoContext` => `Nullability.NullabilityInfoContextEx`


## Copyright / Licensing

The csproj and nuget config that builds the package is under MIT.

The content of the nugets are also MIT but are [Copyright (c) .NET Foundation and Contributors](https://github.com/dotnet/runtime/blob/main/LICENSE.TXT)


## Usage


### Example target class

Given the following class

<!-- snippet: Target.cs -->
<a id='snippet-Target.cs'></a>
```cs
class Target
{
    public string? StringField;
    public string?[] ArrayField;
    public Dictionary<string, object?> GenericField;
}
```
<sup><a href='/src/Nullability.Source.Tests/Target.cs#L1-L6' title='Snippet source file'>snippet source</a> | <a href='#snippet-Target.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### NullabilityInfoContext

From the Nullability.Source package

<!-- snippet: SourceUsage -->
<a id='snippet-sourceusage'></a>
```cs
[Fact]
public void Test()
{
    var type = typeof(Target);
    var arrayField = type.GetField("ArrayField");
    var genericField = type.GetField("GenericField");

    var context = new NullabilityInfoContext();

    var arrayInfo = context.Create(arrayField);

    Assert.Equal(NullabilityState.NotNull, arrayInfo.ReadState);
    Assert.Equal(NullabilityState.Nullable, arrayInfo.ElementType.ReadState);

    var genericInfo = context.Create(genericField);

    Assert.Equal(NullabilityState.NotNull, genericInfo.ReadState);
    Assert.Equal(NullabilityState.NotNull, genericInfo.GenericTypeArguments[0].ReadState);
    Assert.Equal(NullabilityState.Nullable, genericInfo.GenericTypeArguments[1].ReadState);
}
```
<sup><a href='/src/Nullability.Source.Tests/Samples.cs#L6-L29' title='Snippet source file'>snippet source</a> | <a href='#snippet-sourceusage' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### NullabilityInfoContextEx

From the Nullability package

<!-- snippet: Usage -->
<a id='snippet-usage'></a>
```cs
[Fact]
public void Test()
{
    var type = typeof(Target);
    var arrayField = type.GetField("ArrayField");
    var genericField = type.GetField("GenericField");

    var context = new NullabilityInfoContextEx();

    var arrayInfo = context.Create(arrayField);

    Assert.Equal(NullabilityStateEx.NotNull, arrayInfo.ReadState);
    Assert.Equal(NullabilityStateEx.Nullable, arrayInfo.ElementType.ReadState);

    var genericInfo = context.Create(genericField);

    Assert.Equal(NullabilityStateEx.NotNull, genericInfo.ReadState);
    Assert.Equal(NullabilityStateEx.NotNull, genericInfo.GenericTypeArguments[0].ReadState);
    Assert.Equal(NullabilityStateEx.Nullable, genericInfo.GenericTypeArguments[1].ReadState);
}
```
<sup><a href='/src/Nullability.Tests/Samples.cs#L7-L30' title='Snippet source file'>snippet source</a> | <a href='#snippet-usage' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### NullabilityInfoExtensions

`NullabilityInfoExtensions` provides static and thread safe wrapper around <see cref="NullabilityInfoContext"/>. It adds three extension methods to each of ParameterInfo, PropertyInfo, EventInfo, and FieldInfo.

 * `GetNullabilityInfo`: returns the `NullabilityInfo` for the target info.
 * `GetNullability`: returns the `NullabilityState` for the state (`NullabilityInfo.ReadState` or `NullabilityInfo.WriteState` depending on which has more info) of target info.
 * `IsNullable`: given the state (`NullabilityInfo.ReadState` or `NullabilityInfo.WriteState` depending on which has more info) of the info:
   * Returns true if state is `NullabilityState.Nullable`.
   * Returns false if state is `NullabilityState.NotNull`.
   * Throws an exception if state is `NullabilityState.Unknown`.


## API

```
namespace System.Reflection
{
    public sealed class NullabilityInfoContext
    {
        public NullabilityInfo Create(ParameterInfo parameterInfo);
        public NullabilityInfo Create(PropertyInfo propertyInfo);
        public NullabilityInfo Create(EventInfo eventInfo);
        public NullabilityInfo Create(FieldInfo parameterInfo);
    }
}
```

<!-- snippet: NullabilityInfo.cs -->
<a id='snippet-NullabilityInfo.cs'></a>
```cs
// <auto-generated />
#nullable enable
using System.Linq;
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;

namespace System.Reflection
{
    /// <summary>
    /// A class that represents nullability info
    /// </summary>
    sealed class NullabilityInfo
    {
        internal NullabilityInfo(Type type, NullabilityState readState, NullabilityState writeState,
            NullabilityInfo? elementType, NullabilityInfo[] typeArguments)
        {
            Type = type;
            ReadState = readState;
            WriteState = writeState;
            ElementType = elementType;
            GenericTypeArguments = typeArguments;
        }

        /// <summary>
        /// The <see cref="System.Type" /> of the member or generic parameter
        /// to which this NullabilityInfo belongs
        /// </summary>
        public Type Type { get; }
        /// <summary>
        /// The nullability read state of the member
        /// </summary>
        public NullabilityState ReadState { get; internal set; }
        /// <summary>
        /// The nullability write state of the member
        /// </summary>
        public NullabilityState WriteState { get; internal set; }
        /// <summary>
        /// If the member type is an array, gives the <see cref="NullabilityInfo" /> of the elements of the array, null otherwise
        /// </summary>
        public NullabilityInfo? ElementType { get; }
        /// <summary>
        /// If the member type is a generic type, gives the array of <see cref="NullabilityInfo" /> for each type parameter
        /// </summary>
        public NullabilityInfo[] GenericTypeArguments { get; }
    }

    /// <summary>
    /// An enum that represents nullability state
    /// </summary>
    enum NullabilityState
    {
        /// <summary>
        /// Nullability context not enabled (oblivious)
        /// </summary>
        Unknown,
        /// <summary>
        /// Non nullable value or reference type
        /// </summary>
        NotNull,
        /// <summary>
        /// Nullable value or reference type
        /// </summary>
        Nullable
    }
}
```
<sup><a href='/src/Nullability.Source/NullabilityInfo.cs#L1-L67' title='Snippet source file'>snippet source</a> | <a href='#snippet-NullabilityInfo.cs' title='Start of snippet'>anchor</a></sup>
<a id='snippet-NullabilityInfo.cs-1'></a>
```cs
// <auto-generated />
#nullable enable
using System.Linq;
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;

using NullabilityInfoContext= Nullability.NullabilityInfoContextEx;
using NullabilityInfo = Nullability.NullabilityInfoEx;
using NullabilityState = Nullability.NullabilityStateEx;
namespace Nullability
{
    /// <summary>
    /// A class that represents nullability info
    /// </summary>
    public sealed class NullabilityInfoEx
    {
        internal NullabilityInfoEx(Type type, NullabilityState readState, NullabilityState writeState,
            NullabilityInfo? elementType, NullabilityInfo[] typeArguments)
        {
            Type = type;
            ReadState = readState;
            WriteState = writeState;
            ElementType = elementType;
            GenericTypeArguments = typeArguments;
        }

        /// <summary>
        /// The <see cref="System.Type" /> of the member or generic parameter
        /// to which this NullabilityInfo belongs
        /// </summary>
        public Type Type { get; }
        /// <summary>
        /// The nullability read state of the member
        /// </summary>
        public NullabilityState ReadState { get; internal set; }
        /// <summary>
        /// The nullability write state of the member
        /// </summary>
        public NullabilityState WriteState { get; internal set; }
        /// <summary>
        /// If the member type is an array, gives the <see cref="NullabilityInfo" /> of the elements of the array, null otherwise
        /// </summary>
        public NullabilityInfo? ElementType { get; }
        /// <summary>
        /// If the member type is a generic type, gives the array of <see cref="NullabilityInfo" /> for each type parameter
        /// </summary>
        public NullabilityInfo[] GenericTypeArguments { get; }
    }

    /// <summary>
    /// An enum that represents nullability state
    /// </summary>
    public enum NullabilityStateEx
    {
        /// <summary>
        /// Nullability context not enabled (oblivious)
        /// </summary>
        Unknown,
        /// <summary>
        /// Non nullable value or reference type
        /// </summary>
        NotNull,
        /// <summary>
        /// Nullable value or reference type
        /// </summary>
        Nullable
    }
}
```
<sup><a href='/src/Nullability/NullabilityInfo.cs#L1-L70' title='Snippet source file'>snippet source</a> | <a href='#snippet-NullabilityInfo.cs-1' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## How this is built

On every build the `NullabilityInfo.csproj` downloads `NullabilityInfo.cs` and `NullabilityInfoContext.cs`. This ensures that the bundled files are always up to date on each release.


## Icon

[Reflection](https://thenounproject.com/term/reflection/4087162/) designed by [Yogi Aprelliyanto](https://thenounproject.com/yogiaprelliyanto/) from [The Noun Project](https://thenounproject.com).
