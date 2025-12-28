using IMS.CoreBusiness;
using IMS.UseCases.Activeties.Interfaces;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.UseCases.Activeties
{
    public class ProduceProductUC : IProduceProductUC
    {
        private readonly IProductTransactionRepo productionRepo;
        private readonly IProductRepository productRep;

        public ProduceProductUC(IProductTransactionRepo productionRepo,
                                IProductRepository productRep)
        {
            this.productionRepo = productionRepo;
            this.productRep = productRep;
        }
        public async Task ExecuteAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            //add a transaction record
            await productionRepo.ProduceAsync(productionNumber, product, quantity, doneBy);
            //reduce inventory
            //update quantity of product
            product.Quantity += quantity;
            await this.productRep.UpdateProductAsync(product);
        }
    }
}
