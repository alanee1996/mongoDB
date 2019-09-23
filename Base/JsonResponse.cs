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


        public static JsonResponse success(string message, object data)
        {
            return new JsonResponse { message = message, data = data, status = 200 };
        }

        public static JsonResponse successButInvalid(string message)
        {
            return new JsonResponse() { message = message, status = 200 };
        }

        public static JsonResponse failed(string message, int status)
        {
            return new JsonResponse() { message = message, status = status };
        }
    }
}
