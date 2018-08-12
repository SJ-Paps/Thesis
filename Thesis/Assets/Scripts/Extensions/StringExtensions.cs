

public static class StringExtensions
{
    public static string FirstLetterToUpper(this string str)
    {
        if(str != null)
        {
            for(int i = 0; i < str.Length; i++)
            {
                if(char.IsLetter(str[i]))
                {
                    if(i == 0)
                    {
                        return char.ToUpper(str[0]) + str.Substring(1);
                    }
                    else
                    {
                        return str.Substring(0, i) + char.ToUpper(str[i]) + str.Substring(i + 1);
                    }
                }
            }
        }

        return str;
    }
}
