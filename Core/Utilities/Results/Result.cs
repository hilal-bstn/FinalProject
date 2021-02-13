using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {
       

        public Result(bool success, string message):this(success)//mesaj gönderilmezse diğer constructor ı çalıştırır hale getirdik
        {
            Message = message;
            
        }
        public Result(bool success) 
        {
           
            Success = success;
        }

        public bool Success { get; }

        public string Message { get; }//constructor dışında set edilemez
    }
}
