namespace ecommerse_api.Common.Repository.Validation
{
    public interface IValidation
    {
        Task CheckValidation(object request);
    }
}
