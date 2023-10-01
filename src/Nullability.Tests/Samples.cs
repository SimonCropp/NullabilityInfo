using Nullability;
using Xunit;
// ReSharper disable NotAccessedField.Local

public class Samples
{
    #region Usage

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

    #endregion

    #region ExtensionEx

    [Fact]
    public void ExtensionTests()
    {
        var type = typeof(Target);
        var field = type.GetField("StringField");
        Assert.True(field.IsNullable());
        Assert.Equal(NullabilityStateEx.Nullable, field.GetNullability());
        Assert.Equal(NullabilityStateEx.Nullable, field.GetNullabilityInfo().ReadState);
    }

    #endregion


    class PropertyTarget
    {
        string? write;
        public string? ReadWrite { get; set; }
        public string? Read { get; }

        public string? Write
        {
            set => write = value;
        }
    }

    [Fact]
    public void Property()
    {
        var type = typeof(PropertyTarget);
        var readWrite = type.GetProperty("ReadWrite");
        var write = type.GetProperty("Write");
        var read = type.GetProperty("Read");
        Assert.True(readWrite.IsNullable());
        Assert.True(write.IsNullable());
        Assert.True(read.IsNullable());
    }
}