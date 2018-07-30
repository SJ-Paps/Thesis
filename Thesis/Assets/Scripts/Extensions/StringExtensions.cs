

public static class StringExtensions
{
    public static string FirstLetterToUpper(this string str)
    {
        if(str != null)
        {
            if (str.Length > 1)
            {
                return char.ToUpper(str[0]) + str.Substring(1);
            }
            else if(str.Length > 0)
            {
                return str.ToUpper();
            }
            else
            {
                return str;
            }
        }

        return null;
    }
}
