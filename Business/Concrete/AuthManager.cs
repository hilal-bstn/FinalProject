using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Entities.DTOs;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;//kullanıcı veri tabanında var mı gibi kontroller felan yapmak için
        private ITokenHelper _tokenHelper;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);//Girdiğimiz passwordu Hash ve Salta çeviriyoruz 
            var user = new User
            {
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash,//Passwordu Hashleyip ve Salt yaptıktan sonra veri tabanına kayıt ediyoruz.
                PasswordSalt = passwordSalt,
                Status = true
            };
            _userService.Add(user);
            return new SuccessDataResult<User>(user, "kayıt oldu");
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(userForLoginDto.Email);//Bu mail adresi kayıtlı ise bu kullanıcın bilgilerini userToCheck e at
            if (userToCheck == null)//userToCheck null sa Böyle bir kullanıcı kaydı yok demektir.
            {
                return new ErrorDataResult<User>("kullanıcı bulunamadı");
            }
            //Şifreyi kontrol etmek için hashing helper gerekli.Bu noktada Core katmanında Hashing klasöründe HashingHelper classını oluşturyoruz.
            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))//Kullanıcının girdiği şifre ile veritabanındaki hash ve salt kısımları eşleşiyormu
            {
                return new ErrorDataResult<User>("parola hatası");
            }

            return new SuccessDataResult<User>(userToCheck,"Kullanıcı oluştu");
        }

        public IResult UserExists(string email)
        {//Bir kullanıcı kayıtlı mail adresiyle yeniden kaydolmaya çalışyorsa hata verir.
            if (_userService.GetByMail(email) != null)
            {
                return new ErrorResult();
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {//Kullanıcı Login olduktan sonra biz kullanıca bir token veriyoruz ve sistemde yapacağı işlemleri bu token vasıtasıyla yapıyor
            var claims = _userService.GetClaims(user);//Claim kişinin rolleri neler
            var accessToken = _tokenHelper.CreateToken(user, claims);//Kişinin rollerine göre token oluşturuyoruz yetkilendirme için 
            return new SuccessDataResult<AccessToken>(accessToken,"Token oluşturuldu");
        }
        //=>Web apide Controller oluşturyoruz bu aşamadan sonra
    }
}
