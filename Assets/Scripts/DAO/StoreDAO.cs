
using System.Threading.Tasks;


public class StoreDAO
{
    public async Task<BaseDTO<StoreDTO>> GetAll()
    {
       return await API.Get<StoreDTO>($"{GlobalVariable.server_url}/shop/get-all");

    }

    public async Task<BaseDTO<object>> Purchase(PurchaseDTO purchaseDTO) 
    {
        return await API.Post<PurchaseDTO, object>($"{GlobalVariable.server_url}/shop/purchase", purchaseDTO);
    }
}