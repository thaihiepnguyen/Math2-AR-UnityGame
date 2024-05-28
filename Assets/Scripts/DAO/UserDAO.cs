
using System.Threading.Tasks;
using UnityEngine;

public class UserDAO
{
    public async Task<BaseDTO<UserDTO>> GetUserById(int id)
    {
        return await API.Get<UserDTO>($"{GlobalVariable.server_url}/users/get-by-id/{id}");
    }

    public async Task<BaseDTO<UserDTO>> GetMe() {
        return await API.Get<UserDTO>($"{GlobalVariable.server_url}/users/get-me/{GlobalVariable.platform}");
    }

    public async Task<BaseDTO<PersonalDTO>> GetProfile() {
        return await API.Get<PersonalDTO>($"{GlobalVariable.server_url}/users/get-personal-by-id/{GlobalVariable.platform}");
    }
}