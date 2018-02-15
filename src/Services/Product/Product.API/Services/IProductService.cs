using Product.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.API.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> GetProducts(int startFromIndex = 0, int? totalProducts);
    }
}
