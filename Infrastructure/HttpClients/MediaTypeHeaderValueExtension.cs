using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PimsPublisher.Infrastructure.HttpClients
{
    internal static class MediaTypeHeaderValueExtension
    {
        internal static bool IsApplicationJson(this MediaTypeHeaderValue headerValue) => headerValue.MediaType == HttpMediaType.Json;
    }
}
