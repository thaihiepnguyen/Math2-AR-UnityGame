using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;


public class AuthDAO
{
    public AuthDAO()
    {
    }

    public async Task<BaseDTO<TestDTO>> Test()
    {
        return await API.Get<TestDTO>($"{GlobalVariable.server_url}/account/test");
    }

    public async Task<BaseDTO<object>> LoginByEmail(LoginDTO loginDto)
    {
        return await API.Post<LoginDTO, object>($"{GlobalVariable.server_url}/account/login-by-email", loginDto);
    }

    public async Task<BaseDTO<object>> RegisterByEmail(RegisterDTO registerDto)
    {
        return await API.Post<RegisterDTO, object>($"{GlobalVariable.server_url}/account/register-by-email", registerDto);
    }
}