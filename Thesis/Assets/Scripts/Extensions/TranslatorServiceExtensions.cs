using SJ.Management.Localization;

public static class TranslatorServiceExtensions
{
    public static string GetLineByTagOfCurrentLanguage(this ITranslatorService translatorService, string tag, TextOptions option)
    {
        var line = translatorService.GetLineByTagOfCurrentLanguage(tag);

        switch(option)
        {
            case TextOptions.ToLower:
                return line.ToLower();
            case TextOptions.ToUpper:
                return line.ToUpper();
            case TextOptions.FirstLetterToUpper:
                return line.FirstLetterToUpper();
            case TextOptions.TitleCase:
                return line.ToTitleCase();
        }

        return line;
    }
}
