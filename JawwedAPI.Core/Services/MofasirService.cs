using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.Exceptions.CustomExceptions;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
using Microsoft.Extensions.Logging;

namespace JawwedAPI.Core.Services;

public class MofasirService(IGenericRepository<Mofasir> mofasirRepository, ILogger<MofasirService> logger) : IMofasirService
{

    /// <summary>
    /// Gets a specific Mofasir by ID or name
    /// </summary>
    public async Task<Mofasir> GetMofasirInfo(string mofasirID)
    {
        logger?.LogDebug("Getting Mofasir with ID/name {MofasirID}", mofasirID);

        // Validate input
        if (string.IsNullOrWhiteSpace(mofasirID))
        {
            logger?.LogWarning("Null or empty mofasirID provided");
            throw new GlobalErrorThrower(400, "Invalid Mofasir ID",
                "A valid Mofasir ID or name must be provided.");
        }

        try
        {
            // Try to parse ID as integer
            if (int.TryParse(mofasirID, out int id))
            {
                // Find mofasir by numeric ID
                var mofasir = await mofasirRepository.FindOne(m => m.MofasirID == id);
                if (mofasir != null)
                {
                    return mofasir;
                }
            }
            else
            {
                // If not numeric, try to find by author name in either language
                var mofasirByName = await mofasirRepository.FindOne(
                    m => m.AuthorNameArabic == mofasirID ||
                         m.AuthorNameEnglish == mofasirID);

                if (mofasirByName != null)
                {
                    return mofasirByName;
                }
            }

            // If we get here, no mofasir was found
            logger?.LogWarning("Mofasir with identifier {MofasirID} not found", mofasirID);
            throw new GlobalErrorThrower(404, "Mofasir Not Found",
                $"No Mofasir with identifier '{mofasirID}' was found in the database.");
        }
        catch (Exception ex) when (!(ex is GlobalErrorThrower))
        {
            // Log unexpected exceptions but wrap them in our custom exception
            logger?.LogError(ex, "Error retrieving Mofasir with ID {MofasirID}", mofasirID);
            throw new GlobalErrorThrower(500, "Server Error",
                "An error occurred while retrieving the Mofasir data.");
        }
    }

    /// <summary>
    /// Gets all available Mofasirs
    /// </summary>
    public async Task<List<Mofasir>> GetMofasirsInfo()
    {
        logger?.LogDebug("Getting all Mofasirs");

        try
        {
            // Get all mofasirs from the repository
            List<Mofasir> mofasirs = (List<Mofasir>)await mofasirRepository.GetAll();

            // Even if empty, we return the list (not an error condition)
            if (mofasirs.Count == 0)
            {
                logger?.LogInformation("No Mofasirs found in the database");
            }

            return mofasirs;
        }
        catch (Exception ex)
        {
            // Log and wrap unexpected exceptions
            logger?.LogError(ex, "Error retrieving all Mofasirs");
            throw new GlobalErrorThrower(500, "Server Error",
                "An error occurred while retrieving the list of Mofasirs.");
        }
    }
}