using IMS.CoreBusiness;

namespace IMS.UseCases.Reports
{
    public interface ISearchProductUC
    {
        Task<IEnumerable<ProductTransaction>> ExectuteAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? transactionType);
    }
}