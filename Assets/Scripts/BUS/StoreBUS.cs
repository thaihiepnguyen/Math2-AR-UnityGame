
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
public class StoreBUS
{
    private readonly StoreDAO _storeDao = new(); 
    
    public async Task<BaseDTO<StoreDTO>> GetAll ()
    {
        return await _storeDao.GetAll();
    }

}