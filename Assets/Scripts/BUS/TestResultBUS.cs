
using System.Collections.Generic;
using System.Threading.Tasks;

public class TestResultBUS
{
    private readonly TestResultDAO _testResultDao = new(); 
    
    public async Task<BaseDTO<List<TestResultDTO>>> GetTestsResultByUserId(int id)
    {
        return await _testResultDao.GetTestsResultByUserId(id);
    }
    public async Task<BaseDTO<TestResultDTO>> AddTestResult(TestResultDTO testResult)
    {
        return await _testResultDao.AddTestResult(testResult);
    }
    public async Task<BaseDTO<TestResultDTO>> GetById(int id)
    {
        return await _testResultDao.GetById(id);
    }
    public async Task<BaseDTO<TestResultDTO>> GetByUserIdAndTestId( int testId)
    {
        return await _testResultDao.GetByUserIdAndTestId( testId);
    }
}