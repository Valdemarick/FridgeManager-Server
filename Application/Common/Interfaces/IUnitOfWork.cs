using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IFridgeRepository Fridge { get; }
        IProductRepository Product { get; }
        IFridgeModelRepository FridgeModel { get; }
        IFridgeProductRepository FridgeProduct { get; }

        Task SaveAsync();
    }
}