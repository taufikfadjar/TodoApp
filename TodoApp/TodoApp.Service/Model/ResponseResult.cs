using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Service.Model
{
    public class ResponseResult<T> where T : class
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool Result { get; set; }
        public ResponseResult(T data, bool result, string message ="")
        {
            Data = data;
            Message = message;
            Result = result;
        }
    }
}
