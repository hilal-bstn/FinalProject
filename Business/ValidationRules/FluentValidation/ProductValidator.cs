using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class ProductValidator:AbstractValidator<Product>
    {
        public ProductValidator()//kurallar(dto larda dahil)
        {//ctrl+k,ctrl+d=kod düzenleme
            //tarayıcının diline göre hata mesajı gelir
            RuleFor(p => p.ProductName).NotEmpty();//Boş olamaz
            RuleFor(p => p.ProductName).MinimumLength(2);//productName minimum 2 karakter olmalıdır
            RuleFor(p => p.UnitPrice).NotEmpty();
            RuleFor(p => p.UnitPrice).GreaterThan(0);//0dan büyük olmalı
            RuleFor(p => p.UnitPrice).GreaterThanOrEqualTo(10).When(p => p.CategoryId == 1);
            RuleFor(p => p.ProductName).Must(StartWithA).WithMessage("Ürünler A harfi ile başlamalı.");

        }

        private bool StartWithA(string arg)
        {
            return arg.StartsWith("A");//true false döner
        }
    }
}
