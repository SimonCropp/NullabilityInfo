# <img src="/src/icon.png" height="30px"> NullabilityInfo

[![Build status](https://ci.appveyor.com/api/projects/status/636i70gvxfuwdq38?svg=true)](https://ci.appveyor.com/project/SimonCropp/NullabilityInfo)
[![NuGet Status](https://img.shields.io/nuget/v/NullabilityInfo.svg)](https://www.nuget.org/packages/NullabilityInfo/)

Code-only package that exposes top-level nullability information from reflection.

This feature is [coming in net6](https://github.com/dotnet/runtime/issues/29723). This package exposes the APIs to lower runtime. It supports `netstandard2.0` and up.

Designed to be compatible for libraries that are targeting multiple frameworks including `net6`.

## NuGet package

https://nuget.org/packages/NullabilityInfo/

## Usage

<!-- snippet: Usage -->
<a id='snippet-usage'></a>
```cs
class Target
{
    public string?[] ArrayField;
    public (string?, object) TupleField;
}

[Fact]
public void Test()
{
    var type = typeof(Target);
    var arrayField = type.GetField("ArrayField");
    var tupleField = type.GetField("TupleField");

    var context = new NullabilityInfoContext();

    var arrayInfo = context.Create(arrayField);

    Assert.Equal(NullabilityState.NotNull, arrayInfo.ReadState);
    Assert.Equal(NullabilityState.Nullable, arrayInfo.ElementType.ReadState);

    var tupleInfo = context.Create(tupleField);

    Assert.Equal(NullabilityState.NotNull, tupleInfo.ReadState);
    Assert.Equal(NullabilityState.Nullable, tupleInfo.GenericTypeArguments[0].ReadState);
    Assert.Equal(NullabilityState.NotNull, tupleInfo.GenericTypeArguments[1].ReadState);
}
```
<sup><a href='/src/Tests/Samples.cs#L6-L34' title='Snippet source file'>snippet source</a> | <a href='#snippet-usage' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


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

<!-- snippet: NullabilityInfo.cs.pp -->
<a id='snippet-NullabilityInfo.cs.pp'></a>
```pp
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;

namespace System.Reflection
{
    /// <summary>
    /// A class that represents nullability info
    /// </summary>
    public sealed class NullabilityInfo
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
    public enum NullabilityState
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
<sup><a href='/src/NullabilityInfo/NullabilityInfo.cs.pp#L1-L64' title='Snippet source file'>snippet source</a> | <a href='#snippet-NullabilityInfo.cs.pp' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## How this is built

On every build the `NullabilityInfo.csproj` downloads `NullabilityInfo.cs` and `NullabilityInfoContext.cs`. This ensures that the bundled files are always up to date on each release.


## Icon

[Reflection](https://thenounproject.com/term/reflection/4087162/) designed by [Yogi Aprelliyanto](https://thenounproject.com/yogiaprelliyanto/) from [The Noun Project](https://thenounproject.com).

