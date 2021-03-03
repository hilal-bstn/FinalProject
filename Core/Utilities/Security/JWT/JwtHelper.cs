using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;//securitykey i tanımlamak için package kuruldu
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;//jwtsecuritytoken için bu paketi indirdik
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Utilities.Security.JWT
{//Bunu oluşturduktan sonra AuthService kısmını oluşturmaya başlıyoruz.
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }//set edilemez
        private TokenOptions _tokenOptions;//configurasyon kısmındaki(apsettingsdeki) nesneleri buraya aktaracağız için
        private DateTime _accessTokenExpiration;
        public JwtHelper(IConfiguration configuration)//web api deki appsettings kısmındaki bilgiyi burda okuyoruz.
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();//Tokenoptiondaki bilgileri nesneye aktardık.

        }
        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);//biz bir şifreli bir token oluşturacağız bunu yaparken kendi bildiğimiz bir anahtara ihtiyacımız var.Bu noktada encryption klasöründeki securityKeyHelper classına bak
            //var securityKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)) bu kodu yazmak yerine encryption da SecurityKeyHelper oluşturduk ordan çağırdık(yukarıdaki satır.)
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);//Encryption SingingCredentialsHelper kısmınından çağırdık şifreleme algoritması
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);//alt kısımda classı var
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };

        }

        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<OperationClaim> operationClaims)//tokenoptions,User:Hangi kullanıcı için,SingingCretentials,List<OperationClaim>:Kullanıcı hangi operasyonlara sahip
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,//ne saman son bulacak(login olma süresi).Bu şekilde kullanarak tarihe çevirmiş oluyoruz accesstokenexpiration bilgisini.
                notBefore: DateTime.Now,//eğer tokenexpiration bilgisi şu andan önce geçerliyse geçerli değil.Yani bu bir kural
                claims: SetClaims(user, operationClaims),//
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());//bu noktada extensions klasöründeki claimextentions classını incele
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());

            return claims;
        }
    }
}