using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.UseCases.Reports
{
    public class SearchProductUC : ISearchProductUC
    {
        private readonly IProductTransactionRepo prodTransactionRepo;

        public SearchProductUC(IProductTransactionRepo invTransactionRepo)
        {
            this.prodTransactionRepo = invTransactionRepo;
        }

        public async Task<IEnumerable<ProductTransaction>> ExectuteAsync(
            string productName,
            DateTime? dateFrom,
            DateTime? dateTo,
            ProductTransactionType? transactionType)
        {
            if (dateTo.HasValue) dateTo = dateTo.Value.AddDays(1);

            return await prodTransactionRepo.GetProductTransactions(
                productName,
                dateFrom,
                dateTo,
                transactionType);
        }

       
    }
}
