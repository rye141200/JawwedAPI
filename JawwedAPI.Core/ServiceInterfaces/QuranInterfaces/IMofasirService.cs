using System;
using JawwedAPI.Core.Domain.Entities;

namespace JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;

/// <summary>
/// Service for accessing Mofasir (Quranic commentator) information
/// </summary>
public interface IMofasirService
{
    /// <summary>
    /// Gets all available Mofasirs
    /// </summary>
    /// <returns>List of all Mofasir entities</returns>
    Task<List<Mofasir>> GetMofasirsInfo();

    /// <summary>
    /// Gets a specific Mofasir by ID
    /// </summary>
    /// <param name="mofasirID">The Mofasir ID to retrieve</param>
    /// <returns>The requested Mofasir entity</returns>
    Task<Mofasir> GetMofasirInfo(string mofasirID);
}