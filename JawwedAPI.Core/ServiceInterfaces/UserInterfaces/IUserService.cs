using System;

namespace JawwedAPI.Core.ServiceInterfaces.UserInterfaces;

public interface IUserService
{
    public Task UpgradeUserToPremiumAsync(string email);
}
