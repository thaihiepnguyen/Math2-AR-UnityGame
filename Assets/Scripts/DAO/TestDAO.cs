
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestDAO
{
    public async Task<BaseDTO<List<TestDTO>>> GetTestsBySemester(int semester)
    {
        return await API.Get<List<TestDTO>>($"{GlobalVariable.server_url}/test/semester/{semester}");
    }
    public async Task<BaseDTO<TestDTO>> GetById(int id)
    {
        return await API.Get<TestDTO>($"{GlobalVariable.server_url}/test/{id}");
    }
}