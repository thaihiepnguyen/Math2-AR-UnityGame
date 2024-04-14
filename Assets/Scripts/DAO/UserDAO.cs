
using System.Threading.Tasks;
using UnityEngine;

public class UserDAO
{
    public async Task<BaseDTO<UserDTO>> GetUserById(int id)
    {
        return await API.Get<UserDTO>($"{GlobalVariable.server_url}/users/get-by-id/{id}");
    }
}