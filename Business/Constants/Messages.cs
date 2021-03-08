using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Constants
{
    public static class Messages//new lemeye gerek yok
    {//Constants =Sabitler
        public static string ProductAdded = "Ürün eklendi";
        public static string ProductNameInvalid = "Ürün ismi geçersiz";
        public static string MaintenanceTime = "Sistem bakımda";
        public static string ProductsListed = "Ürünler listelendi";
        public static string ProductNameAlreadyExists = "Bu isimde başka bir ürün bulunmaktadır";
        public static string CategoryLimitExceded = "kategori limiti aşıldığı yeni ürün eklenemiyor..";
        public static string AuthorizationDenied="yetkiniz yok";
        public static string ProductUpdated="ürün güncellendi";
    }
}
