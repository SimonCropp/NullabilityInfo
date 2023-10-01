using Xunit;
// ReSharper disable NotAccessedField.Local

public class Samples
{
    #region SourceUsage

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