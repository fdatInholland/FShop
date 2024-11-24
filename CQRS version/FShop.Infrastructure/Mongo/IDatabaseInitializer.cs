namespace FShop.Infrastructure.Mongo
{
    public interface IDatabaseInitializer
    {
        Task InitializingAsync();
    }
}
