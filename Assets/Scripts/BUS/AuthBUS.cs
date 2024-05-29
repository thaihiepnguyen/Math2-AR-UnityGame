using System.Threading.Tasks;
using Unity.VisualScripting;

public class AuthBUS
{
    private readonly AuthDAO _authDao = new();
    
    public async Task<BaseDTO<int>> LoginByEmail(LoginEmailDTO loginDto)
    {
        return await _authDao.LoginByEmail(loginDto);
    }

    public async Task<BaseDTO<int>> LoginExternalParty(LoginExternalDTO loginDto)
    {
        return await _authDao.LoginExternalParty(loginDto);
    }

    public async Task<BaseDTO<object>> RegisterByEmail(RegisterEmailDTO registerDto)
    {
        return await _authDao.RegisterByEmail(registerDto);
    }
    public async Task<BaseDTO<object>> RegisterByPhone(RegisterPhoneDTO registerDto)
    {
        return await _authDao.RegisterByPhone(registerDto);
    }
    public async Task<BaseDTO<object>> VerifyByPhone(RegisterPhoneDTO phoneNumber)
    {
        return await _authDao.VerifyByPhone(phoneNumber);
    }
}