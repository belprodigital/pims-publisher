namespace PimsPublisher.Infrastructure.HttpClients
{
    internal static class HttpResponseMessageExtension
    {
        internal static bool IsSuccess(this HttpResponseMessage httpResponse) 
        {
            if(httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var ensureHttpStatus = httpResponse.EnsureSuccessStatusCode();

                return ensureHttpStatus.IsSuccessStatusCode == true;
            }

            return false;
        }

        internal static bool IsApplicationJson(this HttpResponseMessage httpResponse)
        {
            if(httpResponse == null ||
                httpResponse.Content == null || 
                httpResponse.Content.Headers == null || 
                httpResponse.Content.Headers.ContentType == null)
                return false;

            return httpResponse.Content.Headers.ContentType.IsApplicationJson();
        }
            
    }
}
