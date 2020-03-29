using System.Globalization;

public static class StringExtensions
{
    public static string FirstLetterToUpper(this string str)
    {
        if (str != null)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsLetter(str[i]))
                {
                    if (i == 0)
                        return char.ToUpper(str[0]) + str.Substring(1);
                    else
                        return str.Substring(0, i) + char.ToUpper(str[i]) + str.Substring(i + 1);
                }
            }
        }

        return str;
    }

    public static string ToTitleCase(this string str)
    {
        if(str != null)
        {
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(str);
        }

        return str;
    }

    public static bool HasSubfix(this string str, string subfix)
    {
        if(str.Length < subfix.Length)
        {
            return false;
        }

        for(int i = 0, j = str.Length - subfix.Length; i < subfix.Length; i++, j++)
        {
            if(str[j] != subfix[i])
            {
                return false;
            }
        }

        return true;
    }
}