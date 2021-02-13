using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
   public class SuccessDataResult<T>:DataResult<T>
    {
        public SuccessDataResult(T data,string message):base(data,true,message)//data ve mesaj döndürmek istersek
        {

        }
        public SuccessDataResult(T data):base(data,true)//sadece data döndürmek istersek
        {

        }
        public SuccessDataResult(string message):base(default,true,message)//data döndürmek istemezsek(çok fazla kullanılmaz)
        {

        }
        public SuccessDataResult():base(default,true)//hiçbir şey döndürmek istmezsek(çok fazla kullanılmaz)
        {

        }
    }
}
