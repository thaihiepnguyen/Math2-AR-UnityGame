using System.Threading.Tasks;
using Unity.VisualScripting;

public class AuthBUS
{
    private AuthDAO _authDao = new ();
    
    public async Task<BaseDTO<object>> LoginByEmail(LoginDTO loginDto)
    {
        return await _authDao.LoginByEmail(loginDto);
    }

    public async Task<BaseDTO<object>> RegisterByEmail(RegisterDTO registerDto)
    {
        return await _authDao.RegisterByEmail(registerDto);
    }
}