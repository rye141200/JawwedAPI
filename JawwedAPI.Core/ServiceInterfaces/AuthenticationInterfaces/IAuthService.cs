using System;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JawwedAPI.Core.ServiceInterfaces.AuthenticationInterfaces;

public interface IAuthService
{
    Task<AuthResponse> GoogleLogin(GoogleLoginRequest request);

    Task<List<ApplicationUser>> GetAllUsers();
}
