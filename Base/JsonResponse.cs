using System;
using System.Collections.Generic;
using System.Text;

namespace Base
{
    public class JsonResponse
    {
        public string message { get; set; }
        public object data { get; set; }
        public int status { get; set; }
        public bool hasError { get; set; }
        public TokenInfo tokenInfo { get; set; }


        public static JsonResponse success(string message, object data)
        {
            return new JsonResponse { message = message, data = data, status = 200, hasError = false };
        }

        public static JsonResponse successButInvalid(string message)
        {
            return new JsonResponse() { message = message, status = 200, hasError = true };
        }

        public static JsonResponse failed(string message, int status)
        {
            return new JsonResponse() { message = message, status = status, hasError = true };
        }

        public JsonResponse setTokenInfo(string accessToken, string refreshToken)
        {
            tokenInfo = new TokenInfo
            {
                accessToken = accessToken,
                refreshToken = refreshToken
            };
            return this;
        }
    }
}
