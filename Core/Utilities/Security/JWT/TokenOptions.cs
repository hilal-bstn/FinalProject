using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public class TokenOptions
    {//web api katmanındaki appsettings.json configurasyonu için oluşturuldu
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int AccessTokenExpiration { get; set; }//dakika cinsinden kullanıcı kaç dakika login kalabilir.
        public string SecurityKey { get; set; }
    }
}
