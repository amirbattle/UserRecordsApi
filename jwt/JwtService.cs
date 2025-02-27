
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class JwtService {
    private readonly UserDbContext _dbContext;
    private readonly IConfiguration _configuration; 

    public JwtService(UserDbContext dbContext, IConfiguration configuration) 
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task<LoginResponseModel?> Authenticate(LoginRequestModel request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return null;
        }

        var userAccount = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Username);
        if (userAccount is null || !SecureHasher.Verify(request.Password, userAccount.Password))
        {
            return null;
        }

        var issuer = _configuration["JwtConfig:Issuer"];
        var audience = _configuration["JwtConfig:Audience"];
        var key = _configuration["JwtConfig:Key"];
        var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
        var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, request.Username)
            }),
            Issuer = issuer,
            Expires = tokenExpiryTimeStamp,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                SecurityAlgorithms.HmacSha512Signature),
            
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);

        return new LoginResponseModel
        {
            AccessToken = accessToken,
            Username = request.Username,
            ExpirersIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
        };
    }
}