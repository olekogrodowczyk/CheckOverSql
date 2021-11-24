using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class ErrorResult
    {
        public string Message { get; set; }
        public IDictionary<string,string[]> Errors { get; set; }
        public bool Success { get; set; }

        public ErrorResult()
        {

        }

        public ErrorResult(string message)
        {
            Message = message;
            Success = false;
        }

        public ErrorResult(string message, IDictionary<string, string[]> errors)
        {
            Success = false;
            Message = message;
            Errors = errors;
        }
    }
}
