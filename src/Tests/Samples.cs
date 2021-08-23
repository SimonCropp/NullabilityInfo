using System.Reflection;
using Xunit;

public class Samples
{
    #region Usage

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

    #region Extension

    [Fact]
    public void ExtensionTests()
    {
        var type = typeof(Target);
        var field = type.GetField("StringField");
        Assert.True(field.IsNullable());
        Assert.Equal(NullabilityState.Nullable, field.GetNullability());
        Assert.Equal(NullabilityState.Nullable, field.GetNullabilityInfo().ReadState);
    }

    #endregion
}