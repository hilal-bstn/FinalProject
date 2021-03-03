using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Concrete
{
    public class User:IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }//passwordu şifreleme algoritmasıyla hashliyoruz
        public bool Status { get; set; }//Durum :kullanıcı aktif mi aktifse true değilse false
        //Kullanıcının basit bir şifre girme ihtimaline karşın Passwordu Saltlıyoruz
    }

}
