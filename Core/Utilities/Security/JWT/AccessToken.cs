using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public class AccessToken
    {//AccessToken Nedir?=>Bir kaynağa ulaşabilmek için verilmiş belirteçtir.
        //JWT(Json Web Tokens):Token oluturmak için geliştirilmiş web standartlarına uygun bir token(hash) formatıdır.
        //json:web üzerinde bilgi paylaşımı için kullanılan dosya biçimidir.
        public string Token { get; set; }

        public DateTime Expiration { get; set; }

    }
}
