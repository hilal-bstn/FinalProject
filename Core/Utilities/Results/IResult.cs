using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{//Temel voidler için başlangıç
   public interface IResult
    {
        bool Success { get; }//yapılan işlemin gerçekleşip gerçekleşmemesi üzerine true false döndürür
        string Message { get; }//true false a göre geri mesaj yollar
    }
}
