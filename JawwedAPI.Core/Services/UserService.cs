using System;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.Exceptions.CustomExceptions;
using JawwedAPI.Core.ServiceInterfaces.UserInterfaces;

namespace JawwedAPI.Core.Services;

public class UserService(IGenericRepository<ApplicationUser> userRepository) : IUserService
{
    public async Task UpgradeUserToPremiumAsync(string email)
    {
        var user =
            await userRepository.FindOne(user => user.Email == email)
            ?? throw new GlobalErrorThrower(
                404,
                "User with this email is not found, maybe the user got deleted after issuing the token!"
            );
        user.UserRole = ApplicationRoles.Premium;
        userRepository.Update(user);
        await userRepository.SaveChangesAsync();
    }
}
