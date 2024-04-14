
using System.Threading.Tasks;

public class UserBUS
{
    private readonly UserDAO _userDao = new(); 
    
    public async Task<BaseDTO<UserDTO>> GetUserById(int id)
    {
        return await _userDao.GetUserById(id);
    }
}