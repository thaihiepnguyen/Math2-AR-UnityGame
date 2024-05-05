
using System.Threading.Tasks;

public class UserBUS
{
    private readonly UserDAO _userDao = new(); 
    
    public async Task<BaseDTO<UserDTO>> GetUserById(int id)
    {
        return await _userDao.GetUserById(id);
    }

    public async Task<BaseDTO<UserDTO>> GetMe()
    {
        return await _userDao.GetMe();
    }

    public async Task<BaseDTO<PersonalDTO>> GetProfile()
    {
        return await _userDao.GetProfile();
    }
}