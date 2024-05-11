
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestResultDAO
{
    public async Task<BaseDTO<List<TestResultDTO>>> GetTestsResultByUserId(int userId)
    {
        return await API.Get<List<TestResultDTO>>($"{GlobalVariable.server_url}/test_result/userId/{userId}");
    }
    public async Task<BaseDTO<TestResultDTO>>AddTestResult(TestResultDTO testResult)
    {
        return await API.Post<TestResultDTO, TestResultDTO>($"{GlobalVariable.server_url}/test_result", testResult);
    }
    public async Task<BaseDTO<TestResultDTO>> GetById(int id)
    {
        return await API.Get<TestResultDTO>($"{GlobalVariable.server_url}/test_result/{id}");
    }
    public async Task<BaseDTO<TestResultDTO>> GetByUserIdAndTestId(int userId, int testId)
    {
        return await API.Get<TestResultDTO>($"{GlobalVariable.server_url}/test_result/user_test?userId={userId}&testId={testId}");
    }
}