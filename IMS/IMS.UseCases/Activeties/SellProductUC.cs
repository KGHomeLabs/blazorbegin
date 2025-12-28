using IMS.CoreBusiness;
using IMS.UseCases.Activeties.Interfaces;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.UseCases.Activeties
{
    public class SellProductUC : ISellProductUC
    {
        private readonly IProductTransactionRepo prodTransactionRepo;
        private readonly IProductRepository prodRepo;

        public SellProductUC(IProductTransactionRepo prodTransactionRepo,
                             IProductRepository prodRepo)
        {
            this.prodTransactionRepo = prodTransactionRepo;
            this.prodRepo = prodRepo;
        }

        public async Task ExecuteAsync(string salesOrderNumber, Product product, int quantity, double unitPrice,string doneBy)
        {
            prodTransactionRepo.SellProductAsync(salesOrderNumber, product, quantity,unitPrice, doneBy);

            product.Quantity -= quantity;
            await prodRepo.UpdateProductAsync(product);

        }
    }
}
