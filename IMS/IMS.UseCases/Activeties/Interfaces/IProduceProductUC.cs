using IMS.CoreBusiness;

namespace IMS.UseCases.Activeties.Interfaces
{
    public interface IProduceProductUC
    {
        Task ExecuteAsync(string productionNumber, Product product, int quantity, string doneBy);
    }
}