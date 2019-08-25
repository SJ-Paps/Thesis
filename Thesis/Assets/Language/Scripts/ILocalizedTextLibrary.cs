public interface ILocalizedTextLibrary
{
    string GetLineByTagOfCurrentLanguage(string tag);
    string GetLineByTagOfLanguage(string language, string tag);
}
