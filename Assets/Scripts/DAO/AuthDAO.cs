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
    public async Task<BaseDTO<int>> LoginByEmail(LoginEmailDTO loginDto)
    {
        return await API.Post<LoginEmailDTO, int>($"{GlobalVariable.server_url}/account/login-by-email", loginDto);
    }

    public async Task<BaseDTO<int>> LoginExternalParty(LoginExternalDTO loginDto)
    {
        return await API.Post<LoginExternalDTO, int>($"{GlobalVariable.server_url}/account/login-external-party", loginDto);
    }

    public async Task<BaseDTO<object>> RegisterByEmail(RegisterEmailDTO registerDto)
    {
        return await API.Post<RegisterEmailDTO, object>($"{GlobalVariable.server_url}/account/register-by-email", registerDto);
    }
    public async Task<BaseDTO<object>> RegisterByPhone(RegisterPhoneDTO registerDto)
    {
        return await API.Post<RegisterPhoneDTO, object>($"{GlobalVariable.server_url}/account/register-by-phone", registerDto);
    }
    public async Task<BaseDTO<object>> VerifyByPhone(RegisterPhoneDTO phoneNumber)
    {
        return await API.Post<RegisterPhoneDTO, object>($"{GlobalVariable.server_url}/account/verify-phone", phoneNumber);
    }
}