using System;
using AutoMapper;
using System.Text.Json;
using JawwedAPI.Core.ServiceInterfaces.SeedInterfaces;
using JawwedAPI.Core.Domain.RepositoryInterfaces;

namespace JawwedAPI.Infrastructure.DataSeeding;
// T : JsonBindedClass
// M : ModelClass
public class GenericSeedService<T, M>(IGenericRepository<M> repository, IMapper mapper) : IGenericSeedService<T, M> where T : class where M : class
{

    public async Task SaveToDatabase(string path)
    {
        Seed(path).ToList().ForEach(line => repository.Create(line));
        await repository.SaveChangesAsync();
    }

    public IEnumerable<M> Seed(string path)
    {

        //? 1) Read json file
        List<T> jsonObjList = ReadFromJson(path);
        //? 2) Mapping json object to model object using auto mapper
        var modelObjList = mapper.Map<List<M>>(jsonObjList);

        return modelObjList;
    }

    private List<T> ReadFromJson(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("The file path cannot be null or empty.", nameof(path));

        if (!File.Exists(path))
            throw new FileNotFoundException($"The file at path '{path}' was not found.", path);

        try
        {
            using var reader = new StreamReader(path, System.Text.Encoding.UTF8);
            string jsonContent = reader.ReadToEnd();

            var dataList = JsonSerializer.Deserialize<List<T>>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (dataList == null || !dataList.Any())
            {
                Console.WriteLine("No data found in the JSON file.");
                return new List<T>(); // Return an empty list if no data is found
            }

            return dataList;
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
            return new List<T>(); // Return an empty list on JSON parsing errors
        }
        catch (IOException ioEx)
        {
            Console.WriteLine($"I/O error reading file: {ioEx.Message}");
            return new List<T>(); // Return an empty list on I/O errors
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return new List<T>(); // Return an empty list on unexpected errors
        }
    }

}
