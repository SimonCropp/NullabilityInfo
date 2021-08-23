using System.Collections.Generic;
using System.Reflection;
using Xunit;

public class Samples
{
    #region Usage

    class Target
    {
        public string?[] ArrayField;
        public Dictionary<string, object?> GenericField;
    }

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
    #endregion
}