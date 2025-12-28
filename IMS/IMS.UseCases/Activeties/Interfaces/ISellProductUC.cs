using IMS.CoreBusiness;

namespace IMS.UseCases.Activeties.Interfaces
{
    public interface ISellProductUC
    {
        Task ExecuteAsync(string salesOrderNumber, Product product, int quantity,double unitPrice, string doneBy);
    }
}