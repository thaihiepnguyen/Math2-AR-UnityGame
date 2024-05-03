
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestPurchaseDAO
{
    public async Task<BaseDTO<List<TestPurchaseDTO>>> GetByUserID(int userId)
    {
        return await API.Get<List<TestPurchaseDTO>>($"{GlobalVariable.server_url}/test_purchase/user/{userId}");
    }
    public async Task<BaseDTO<TestPurchaseDTO>> GetById(int id)
    {
        return await API.Get<TestPurchaseDTO>($"{GlobalVariable.server_url}/test_purchase/{id}");
    }
}