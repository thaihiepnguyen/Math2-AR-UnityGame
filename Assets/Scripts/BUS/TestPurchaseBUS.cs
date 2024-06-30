
using System.Collections.Generic;
using System.Threading.Tasks;

public class TestPurchaseBUS
{
    private readonly TestPurchaseDAO _testPurchaseDao = new(); 
    
    public async Task<BaseDTO<List<TestPurchaseDTO>>> GetByUserId()
    {
        return await _testPurchaseDao.GetByUserID();
    }
    public async Task<BaseDTO<TestPurchaseDTO>> GetById(int id)
    {
        return await _testPurchaseDao.GetById(id);
    }
}