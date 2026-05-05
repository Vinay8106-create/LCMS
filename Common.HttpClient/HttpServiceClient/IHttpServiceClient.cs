namespace Common.HttpClient
{
    public interface IHttpServiceClient
    {
        Task<T?> GetAsync<T>(HttpServiceRequest request);
        Task<T?> PostAsync<T>(HttpServiceRequest request);
        Task<T?> PutAsync<T>(HttpServiceRequest request);
        Task<T?> DeleteAsync<T>(HttpServiceRequest request);
    }
}
