
using System.Threading.Tasks;


public class StoreDAO
{
    public async Task<BaseDTO<StoreDTO>> GetAll()
    {
       return await API.Get<StoreDTO>($"{GlobalVariable.server_url}/shop/get-all");

    }


}