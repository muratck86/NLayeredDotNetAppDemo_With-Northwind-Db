using Northwind.Business.Abstract;
using Northwind.DataAccess.Abstract;
using Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public void Add(Product product)
        {
            _productDal.Add(product);
        }

        public void Update(Product product)
        {
            _productDal.Update(product);
        }

        public void Delete(Product product)
        {
            _productDal.Delete(product);
        }

        public List<Product> GetAll()
        {
            //Business code...
            var products = _productDal.GetAll();
            //Business code...
            return products;
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            return _productDal.GetAll(p => p.CategoryId == categoryId);
        }

        public List<Product> SearchProductsByName(string key)
        {
            return _productDal.GetAll(p => p.ProductName.Contains(key));
        }

        public List<Product> SearchProductsByNameInSelectedCategory(string key, int categoryId)
        {
            return _productDal.GetAll(p =>p.ProductName.Contains(key) && p.CategoryId == categoryId);
        }

    }
}
