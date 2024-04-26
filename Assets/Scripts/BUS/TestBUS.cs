
using System.Collections.Generic;
using System.Threading.Tasks;

public class TestBUS
{
    private readonly TestDAO _testDao = new(); 
    
    public async Task<BaseDTO<List<TestDTO>>> GetUserById(int id)
    {
        return await _testDao.GetTestsBySemester(id);
    }
}