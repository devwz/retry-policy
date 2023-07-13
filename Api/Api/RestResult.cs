using System.Net;

namespace Api
{
    public class RestResult<T>
    {
        public RestResult(T result, HttpStatusCode statusCode)
            => (Data, StatusCode) = (result, statusCode);

        public RestResult(HttpStatusCode statusCode)
            => (StatusCode) = (statusCode);

        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
