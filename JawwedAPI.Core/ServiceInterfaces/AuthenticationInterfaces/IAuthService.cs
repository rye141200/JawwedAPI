using System;
using JawwedAPI.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JawwedAPI.Core.ServiceInterfaces.AuthenticationInterfaces;

public interface IAuthService
{
    Task<string> LoginAndGenerateToken(GoogleLoginRequest request);
}
