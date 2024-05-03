
using System.Collections.Generic;
using System.Threading.Tasks;

public class TestPurchaseBUS
{
    private readonly TestPurchaseDAO _testPurchaseDao = new(); 
    
    public async Task<BaseDTO<List<TestPurchaseDTO>>> GetByUserId(int user_id)
    {
        return await _testPurchaseDao.GetByUserID(user_id);
    }
    public async Task<BaseDTO<TestPurchaseDTO>> GetById(int id)
    {
        return await _testPurchaseDao.GetById(id);
    }
}