namespace FluentValidation
{
    public abstract class RespectNullValidator<T> :
        AbstractValidator<T>
    {
        protected RespectNullValidator()
        {
            this.AddNullableRules();
        }
    }
}
