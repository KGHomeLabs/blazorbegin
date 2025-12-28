using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;


namespace IMS.Plugins.InMemory
{
    public class ProductTransactionRepository : IProductTransactionRepo
    {
        private readonly IProductRepository productRepo;
        private readonly IInventoryTransactionRepository invTransactionRepo;
        private readonly IInventoryRepository invRepo;
        private List<ProductTransaction> _productTransactions =  new List<ProductTransaction>();

        public ProductTransactionRepository(IProductRepository productRepo, IInventoryTransactionRepository invTransactionRepo, IInventoryRepository invRepo)
        {
            this.productRepo = productRepo;
            this.invTransactionRepo = invTransactionRepo;
            this.invRepo = invRepo;
        }

        public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            var prod = await this.productRepo.GetProductByIdAsync(product.Id);
            
            //add inventory transaction
            if (prod != null)
            {
                foreach(var pi in prod.ProductInventories)
                {
                    if(pi.Inventory != null)
                    {
                        this.invTransactionRepo.ProduceAsync(productionNumber,
                            pi.Inventory,
                            pi.InventoryQuantity * quantity,
                            doneBy,
                            -1);
                        var inv = await invRepo.GetInventoryByIdAsync(pi.InventoryId);
                        inv.Quantity -= pi.InventoryQuantity * quantity;
                        await invRepo.UpdateInventoryAsync(inv);
                            
                    }
                    
                }
            }
            //add product transaction
            var newProductTransaction = new ProductTransaction
            {
                ProductionNumber = productionNumber,
                ProductId = product.Id,
                QuantityBefore = product.Quantity,
                QuantityAfter = product.Quantity + quantity,
                TransDate = DateTime.Now,
                DoneBy = doneBy

            };
            _productTransactions.Add(newProductTransaction);
        }

        public Task SellProductAsync(string salesOrderNumber, Product product, int quantity,double unitPrice, string doneBy)
        {
            var newTransaction = new ProductTransaction();
            newTransaction.ActivityType = ProductTransactionType.SellProduct;
            newTransaction.SONumber = salesOrderNumber;
            newTransaction.ProductId = product.Id;
            newTransaction.QuantityBefore = product.Quantity;
            newTransaction.QuantityAfter = product.Quantity-quantity;
            newTransaction.TransDate = DateTime.Now;
            newTransaction.DoneBy = doneBy;
            newTransaction.UnitPrice = unitPrice;


            _productTransactions.Add(newTransaction);

            return Task.CompletedTask;
        }

    }
}
