using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;
using System.Transactions;
using System.IO;

namespace EFreshStore.Managers
{
    public class ProductUnitManager : CommonManager<ProductUnit>, IProductUnitManager
    {
        public ProductUnitManager() : base(new ProductUnitRepository())
        {
        }

        public ICollection<ProductUnit> GetAll()
        {
            return GetAll(c => c.Product,
                c => c.Product.Category,
                c => c.Product.Brand,
                c => c.ProductImages);
        }

        public ProductUnit GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id,
                c => c.Product.Category,
                c => c.Product.Brand,   
                c => c.ProductImages);
        }

        public ICollection<ProductUnit> GetByBrand(long brandId)
        {
            IProductManager productManager = new ProductManager();
            List<Product> products = productManager.GetByBrand(brandId).ToList();
            List<ProductUnit> productUnits = new List<ProductUnit>();
            foreach (Product product in products)
            {
                productUnits.AddRange(product.ProductUnits);
            }

            return productUnits;
        }
        public ICollection<ProductUnit> GetByCategory(long categorydId)
        {
            IProductManager productManager = new ProductManager();
            List<Product> products = productManager.GetByCategory(categorydId).ToList();
            List<ProductUnit> productUnits = new List<ProductUnit>();
            foreach (Product product in products)
            {
                productUnits.AddRange(product.ProductUnits);
            }

            return productUnits;
        }

        public bool SaveProductDetails(ProductUnit productUnit)
        {
            IProductUnitManager _productUnitManager = new ProductUnitManager();
            IProductImageManager _productImageManager = new ProductImageManager();
            IProductUnitPriceManager _productUnitPriceManager = new ProductUnitPriceManager();        

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    productUnit.CreatedOn = DateTime.Now;
                    productUnit.IsActive = true;
                    
                    bool isSaved = _productUnitManager.Add(productUnit);

                   

                    ProductUnitPrice productUnitPrice = new ProductUnitPrice();
                    productUnitPrice.CreatedOn = DateTime.Now;
                    productUnitPrice.IsActive = true;
                    productUnitPrice.MaximumRetailPrice = productUnit.MaximumRetailPrice;
                    productUnitPrice.TradePricePerCarton = productUnit.TradePricePerCarton;
                    productUnitPrice.DistributorPricePerCarton = productUnit.DistributorPricePerCarton;
                    productUnitPrice.ProductUnitId = productUnit.Id;
                    _productUnitPriceManager.Add(productUnitPrice);
                    //foreach (var item in productUnit.ProductImages)
                    //{
                    //    item.CreatedOn = DateTime.Now;
                    //    item.ProductUnit = null;
                    //    item.ProductUnitId = productUnit.Id;
                    //    _productImageManager.Add(item);
                    //}
                    transactionScope.Complete();
                    return true;
                    //transactionScope.Dispose();

                }
                catch (TransactionException ex)
                {
                    transactionScope.Dispose();                    

                }

            }
            return false;
        }

        public bool EditProductDetails(ProductUnit productUnit)
        {
            IProductUnitManager _productUnitManager = new ProductUnitManager();
            IProductImageManager _productImageManager = new ProductImageManager();
            IProductUnitPriceManager _productUnitPriceManager = new ProductUnitPriceManager();

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    productUnit.CreatedOn = DateTime.Now;
                    productUnit.IsActive = true;
                    var images = _productImageManager.GetAll().Where(c => c.ProductUnitId == productUnit.Id).ToList();
                    if(images!=null)
                    {
                        foreach(var item in images)
                        {
                            _productImageManager.Delete(item);
                        }
                    }
                    bool isUpdate = _productUnitManager.Update(productUnit);



                    ProductUnitPrice productUnitPrice = productUnitPrice = _productUnitPriceManager.GetFirstOrDefault(c => c.ProductUnitId == productUnit.Id);
                    if(productUnitPrice==null)
                    {
                        productUnitPrice = new ProductUnitPrice();
                    }
                    productUnitPrice.CreatedOn = DateTime.Now;
                    productUnitPrice.IsActive = true;
                    productUnitPrice.MaximumRetailPrice = productUnit.MaximumRetailPrice;
                    productUnitPrice.TradePricePerCarton = productUnit.TradePricePerCarton;
                    productUnitPrice.DistributorPricePerCarton = productUnit.DistributorPricePerCarton;
                    productUnitPrice.ProductUnitId = productUnit.Id;
                    _productUnitPriceManager.Update(productUnitPrice);
                    foreach (var item in productUnit.ProductImages)
                    {
                        item.CreatedOn = DateTime.Now;
                        item.ProductUnit = null;
                        item.ProductUnitId = productUnit.Id;
                        _productImageManager.Add(item);
                    }
                    transactionScope.Complete();
                    return true;
                    //transactionScope.Dispose();

                }
                catch (TransactionException ex)
                {
                    transactionScope.Dispose();

                }

            }
            return false;
        }
    }
}