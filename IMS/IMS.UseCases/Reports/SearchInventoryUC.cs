using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.UseCases.Reports
{
    public class SearchInventoryUC : ISearchInventoryUC
    {
        private readonly IInventoryTransactionRepository invTransactionRepo;

        public SearchInventoryUC(IInventoryTransactionRepository invTransactionRepo)
        {
            this.invTransactionRepo = invTransactionRepo;
        }

        public async Task<IEnumerable<InventoryTransaction>> ExectuteAsync(
            string inventoryName,
            DateTime? dateFrom,
            DateTime? dateTo,
            InventoryTransactionType? transactionType)
        {
            if (dateTo.HasValue) dateTo = dateTo.Value.AddDays(1);

            return await invTransactionRepo.GetInventroyTransactions(
                inventoryName, 
                dateFrom,
                dateTo,
                transactionType);
        }
    }
}
