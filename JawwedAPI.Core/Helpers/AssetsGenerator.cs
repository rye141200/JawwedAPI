
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace JawwedAPI.Core.Helpers;

public static class AssetsGenerator
{
    /// <summary>
    /// Creates a dynamic link by replacing placeholders in the template with provided values.
    /// </summary>
    /// <typeparam name="T">The type of the template and the final result</typeparam>
    /// <param name="template">The template string containing placeholders like {parameterN}</param>
    /// <param name="predicates">An array of functions that return values to replace the placeholders</param>
    /// <returns>A string with the placeholders replaced by the corresponding values from the predicates</returns>
    /// <example>
    /// <code>
    /// string template = "{baselink}/{parameter1}/{parameter2}";
    /// string result = GenerateLink(template, () => "https://example.com", () => "path", () => "file.txt");
    /// </code>
    /// </example>

    public static string GenerateLink<T>(string template, List<Func<T>> predicates)
    {

        //! 1) // Find all placeholders in format {parameterN}
        var matches = Regex.Matches(template, @"\{([^}]+)\}");
        //! 2) Replace each placeholder with corresponding predicate result
        for (int i = 0; i < matches.Count; i++)
        {
            string placeholder = matches[i].Value;
            T parameterValue = predicates[i]();
            template = template.Replace(placeholder, parameterValue.ToString());
        }
        return template;
    }
    /// <summary>
    /// Creates a URL template by replacing parts of a given URL with numbered placeholders.
    /// </summary>
    /// <param name="exampleLink">A complete URL to use as an example</param>
    /// <returns>A template string with placeholders like {baselink} and {parameterN}</returns>
    /// <remarks>
    /// The method:
    /// 1. Extracts the base URL (scheme + host)
    /// 2. Breaks the path into individual segments
    /// 3. Replaces each segment with a placeholder in the format {parameterN}
    /// </remarks>
    /// <example>
    /// Input: "https://verses.quran.com/Shuraym/mp3/002002.mp3"
    /// Output: "{baselink}/{parameter1}/{parameter2}/{parameter3}"
    /// </example>
    public static string GenerateTemplate(string exampleLink)
    {
        string template = "";
        Uri urlReader = new Uri(exampleLink);
        template += "{baselink}/";
        //! 1) get link parts like Shuraym,mp3,002002.mp3
        string[] parameters = urlReader.AbsolutePath.TrimStart('/').Split('/');
        List<string> templateParts = new List<string>();

        //! 2) rearrange the link parts
        for (int i = 0; i < parameters.Length; i++)
        {
            templateParts.Add($"{{parameter{i + 1}}}");
        }
        //! 3) add pieces together
        string templatePath = string.Join('/', templateParts);
        //? return {baseLink}/{parameter1}/{parameter2}/{parameter3}
        return template += templatePath;
    }

}