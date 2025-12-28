using IMS.CoreBusiness;

namespace IMS.UseCases.Activeties.Interfaces
{
    public interface IPurchaseInventoryUC
    {
        Task ExecuteAsync(string poNumber, Inventory inventory, int quantity, string doneBy);
    }
}