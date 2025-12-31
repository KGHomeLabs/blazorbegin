using IMS.CoreBusiness;

namespace IMS.UseCases.Reports
{
    public interface ISearchInventoryUC
    {
        Task<IEnumerable<InventoryTransaction>> ExectuteAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? transactionType);
    }
}