using SideBySideAPI.Models;

namespace SideBySideAPI.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
        bool ValidateToken(string token, out string userId);
    }
} 