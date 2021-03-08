using Business.Abstract;
using Business.BusinessAspect.Autofac;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Cacheing;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;
        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {//Başka bir dal enjekte edemezsin bu kısıma!!
            _productDal = productDal;
            _categoryService = categoryService;
        }
        //Encrytion=geri dönüşü olan veridir.
        //Hashing (örnek olarak;kullacı şifreleri için Şifreleme algoritmaları kullanılır)
        //Salting=kullanıcının girdiği parolayı biz güçlendiriyoruz.
        //Claim=prpduct.add,admin,editor lere verilen ad
        //[Authorize()]= Sadace login olmak yeterli.Yani elimizde bir token varsa bu işlemi gerçekleştirebilir.
        //[Authorize(Roles="Product.List")]
        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {
            //busiess codes

            IResult result = BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId), CheckIfProductNameExists(product.ProductName),CheckIfCategoryLimitExceded());
            if (result != null)
            { return result; }

            _productDal.Add(product);
            return new SuccessResult("Ürün Eklendi..");


            //validation(doğrulama):nesnenin yapısıyla ilgili olanlar validation kısmına yazılır
            //Loglama: yapılan işlemlerin kaydının tutulması


        }
        //Yetkilendirme test aşamaları
        //login olduysak eğer Potman de Headers kısmana gelip Authorization :  Bearer (login olunca verilen Token ı yapıştır)

        [CacheAspect]//key, value
        public IDataResult<List<Product>> GetAll()
        {
            //İş kodları
            //Yetkisi var mı ?
            if (DateTime.Now.Hour == 22)
            { return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime); }

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);
        }
        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }
        [PerformanceAspect(5)]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult();

        }
        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result <= 10)
            {
                return new ErrorResult();
            }
            return new SuccessResult();
        }
        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Count;
            if (result > 0)//böyle bir data varsa
            { return new ErrorResult(Messages.ProductNameAlreadyExists); }
            return new SuccessResult();
        }
        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count > 15)
            { return new ErrorResult(Messages.CategoryLimitExceded); }
            return new SuccessResult();
        }

        [TransactionScopeAspect]
        public IResult TransactionalOperation(Product product)
        {
            _productDal.Update(product);
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductUpdated);
        }
    }


}