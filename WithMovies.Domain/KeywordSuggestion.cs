using WithMovies.Domain.Models;

namespace WithMovies.Domain;

public class KeywordSuggestion
{
    private object value;
    private string text;

    /// <summary>
    /// What the user actually typed
    /// </summary>
    public string RawText { get; set; }

    /// <summary>
    /// The suggested keyword
    /// </summary>
    public string Keyword { get; set; }

    /// <summary>
    /// Every matching character in the suggestion.
    ///
    /// <example>
    /// For example (input: "mandrn"):
    /// <code>
    /// RawText = mandrn
    /// Keyword = mandarin
    /// Matches = mand r n
    /// </code>
    /// </example>
    /// </summary>
    public string Matches { get; set; }

    public float Weight { get; set; }

    public KeywordSuggestion(string suggestion, string userInput, float weight)
    {
        RawText = userInput;
        Keyword = suggestion;
        Matches = FindMatches(userInput, suggestion);
        Weight = weight;
    }

    private static string FindMatches(string haystack, string needle)
    {
        string buffer = haystack;
        string matches = "";

        foreach (var ch in needle)
        {
            int index = buffer.IndexOf(ch);

            if (index == -1)
            {
                matches += ' ';
            }
            else
            {
                buffer = buffer.Remove(index, 1);
                matches += ch;
            }

            if (buffer.Length == 0)
                break;
        }

        return matches;
    }
}
