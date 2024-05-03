
using System.Collections.Generic;
using System.Threading.Tasks;

public class TestBUS
{
    private readonly TestDAO _testDao = new(); 
    
    public async Task<BaseDTO<List<TestDTO>>> GetTestsBySemester(int id)
    {
        return await _testDao.GetTestsBySemester(id);
    }
    public async Task<BaseDTO<TestDTO>> GetById(int id)
    {
        return await _testDao.GetById(id);
    }
}