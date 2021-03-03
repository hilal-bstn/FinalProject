using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Encryption
{
    public class SigningCredentialsHelper
    {
        public static SigningCredentials CreateSigningCredentials(SecurityKey security)//Signing=imzalama
        {//securitykey yi ve algoritmamızı belirleriz bu kısımda(Hangi şifreleme algoritması=HmacSha512Signature)
            return new SigningCredentials(security, SecurityAlgorithms.HmacSha512Signature);
        }
    }
}
