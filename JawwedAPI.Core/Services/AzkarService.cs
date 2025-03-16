using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Exceptions.CustomExceptions;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JawwedAPI.Core.Services;

public class AzkarService : IAzkarService
{
    private readonly IGenericRepository<Zekr> _azkarRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AzkarService> _logger;
    private readonly string _baseLink;

    public AzkarService(
        IGenericRepository<Zekr> azkarRepository,
        IConfiguration configuration,
        ILogger<AzkarService> logger
    )
    {
        _azkarRepository = azkarRepository;
        _configuration = configuration;
        _logger = logger;
        _baseLink =
            _configuration["AudioAssets:AzkarAudioBaseLink"]
            ?? throw new InvalidOperationException("AzkarAudioBaseLink configuration is missing");
    }

    public async Task<List<ZekrCategoryResponse>> GetAzkarCategories()
    {
        _logger?.LogDebug("Getting all Azkar categories");

        try
        {
            List<ZekrCategoryResponse> categoryResponses =
                await _azkarRepository.GetDistinctAndProjectAsync(z => new ZekrCategoryResponse()
                {
                    CategoryId = z.CategoryId,
                    Category = z.Category,
                    CategoryAudio = $"{_baseLink}/{z.CategoryAudio}",
                });

            if (categoryResponses.Count == 0)
            {
                _logger?.LogInformation("No Azkar categories found in the database");
            }

            return categoryResponses;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error retrieving Azkar categories");
            throw new GlobalErrorThrower(
                500,
                "Server Error",
                "An error occurred while retrieving Azkar categories."
            );
        }
    }

    public async Task<ZekrResponse> GetZekrById(int categoryId)
    {
        _logger?.LogDebug("Getting Azkar for category with ID {CategoryId}", categoryId);

        //! 1) Validate input
        if (categoryId <= 0)
        {
            _logger?.LogWarning("Invalid categoryId provided: {CategoryId}", categoryId);
            throw new GlobalErrorThrower(
                400,
                "Invalid Category ID",
                "A valid category ID must be provided."
            );
        }

        try
        {
            //! 2) Get the category information
            var zekrCategory = await _azkarRepository.FindOne(z => z.CategoryId == categoryId);

            if (zekrCategory == null)
            {
                _logger?.LogWarning("Azkar category with ID {CategoryId} not found", categoryId);
                throw new GlobalErrorThrower(
                    404,
                    "Category Not Found",
                    $"No Azkar category with ID {categoryId} was found in the database."
                );
            }

            // Get all zekr items with this categoryId
            var zekrItems = await _azkarRepository.GetFilteredAndProjectWithoutIncludeAsync(
                z => z.CategoryId == categoryId,
                z => new ZekrItemResponse()
                {
                    ZekrID = z.ZekrID,
                    Content = z.Content,
                    Audio = $"{_baseLink}/{z.Audio}",
                    Count = z.Count,
                }
            );

            if (zekrItems.Count == 0)
            {
                _logger?.LogWarning(
                    "No Azkar items found for category ID {CategoryId}",
                    categoryId
                );
            }

            // Create the response with category information
            var response = new ZekrResponse
            {
                CategoryId = zekrCategory.CategoryId,
                Category = zekrCategory.Category,
                CategoryAudio = $"{_baseLink}/{zekrCategory.CategoryAudio}",
                Items = zekrItems,
            };

            return response;
        }
        catch (Exception ex) when (!(ex is GlobalErrorThrower))
        {
            _logger?.LogError(
                ex,
                "Error retrieving Azkar with category ID {CategoryId}",
                categoryId
            );
            throw new GlobalErrorThrower(
                500,
                "Server Error",
                "An error occurred while retrieving the Azkar data."
            );
        }
    }

    public async Task<RandomZekrResponse> GetRandomZekr()
    {
        _logger?.LogDebug("Getting a random Zekr");

        try
        {
            // Get the total count of Zekr items
            var allZekr = await _azkarRepository.GetAll();
            var totalCount = allZekr.Count();

            if (totalCount == 0)
            {
                _logger?.LogWarning("No Zekr items found in the database");
                throw new GlobalErrorThrower(
                    404,
                    "No Content",
                    "No Azkar are available in the database."
                );
            }

            // Generate a random index
            var random = new Random();
            var randomIndex = random.Next(0, totalCount);

            // Get the random Zekr (convert to list for indexing)
            var randomZekr = allZekr.ToList()[randomIndex];

            // Create the response
            var response = new RandomZekrResponse
            {
                ZekrID = randomZekr.ZekrID,
                CategoryId = randomZekr.CategoryId,
                Category = randomZekr.Category,
                Content = randomZekr.Content,
                Count = randomZekr.Count,
                Audio = $"{_baseLink}/{randomZekr.Audio}",
            };

            _logger?.LogInformation(
                "Successfully retrieved random Zekr with ID {ZekrId}",
                randomZekr.ZekrID
            );
            return response;
        }
        catch (Exception ex) when (!(ex is GlobalErrorThrower))
        {
            _logger?.LogError(ex, "Error retrieving random Zekr");
            throw new GlobalErrorThrower(
                500,
                "Server Error",
                "An error occurred while retrieving a random Zekr."
            );
        }
    }

    public async Task<RandomZekrResponse> GetRandomZekrFromCategory(int categoryId)
    {
        _logger?.LogDebug("Getting a random Zekr from category with ID {CategoryId}", categoryId);

        // Validate input
        if (categoryId <= 0)
        {
            _logger?.LogWarning("Invalid categoryId provided: {CategoryId}", categoryId);
            throw new GlobalErrorThrower(
                400,
                "Invalid Category ID",
                "A valid category ID must be provided."
            );
        }

        try
        {
            // Get all Zekr items for this category
            var zekrItems = await _azkarRepository.Find(z => z.CategoryId == categoryId);
            var zekrList = zekrItems.ToList();
            var totalCount = zekrList.Count;

            if (totalCount == 0)
            {
                _logger?.LogWarning("No Zekr items found for category ID {CategoryId}", categoryId);
                throw new GlobalErrorThrower(
                    404,
                    "No Content",
                    $"No Azkar are available for category with ID {categoryId}."
                );
            }

            // Generate a random index
            var random = new Random();
            var randomIndex = random.Next(0, totalCount);

            // Get the random Zekr
            var randomZekr = zekrList[randomIndex];

            // Create the response
            var response = new RandomZekrResponse
            {
                ZekrID = randomZekr.ZekrID,
                CategoryId = randomZekr.CategoryId,
                Category = randomZekr.Category,
                Content = randomZekr.Content,
                Count = randomZekr.Count,
                Audio = $"{_baseLink}/{randomZekr.Audio}",
            };

            _logger?.LogInformation(
                "Successfully retrieved random Zekr with ID {ZekrId} from category {CategoryId}",
                randomZekr.ZekrID,
                categoryId
            );
            return response;
        }
        catch (Exception ex) when (!(ex is GlobalErrorThrower))
        {
            _logger?.LogError(
                ex,
                "Error retrieving random Zekr from category with ID {CategoryId}",
                categoryId
            );
            throw new GlobalErrorThrower(
                500,
                "Server Error",
                "An error occurred while retrieving a random Zekr from the specified category."
            );
        }
    }
}
