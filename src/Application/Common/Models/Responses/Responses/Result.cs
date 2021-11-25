using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class Result
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public Result(string message)
        {
            Success = true;
            Message = message;
        }
        public Result()
        {
            Success = true;
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; set; }

        public Result()
        {
            Success = true;
        }

        public Result(T value)
        {
            Success = true;
            Value = value;
        }

        public Result(T value, string message)
        {
           Success = true;
           Value = value;
           Message = message;
        }
    }
}
