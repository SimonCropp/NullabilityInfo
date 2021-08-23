using System.Reflection;
using Xunit;

public class Samples
{
    #region Usage

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
    #endregion
}