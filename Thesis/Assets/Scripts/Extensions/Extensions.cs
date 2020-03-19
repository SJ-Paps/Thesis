using System.Collections.Generic;
using System.Globalization;

public static class Extensions
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

    public static bool ContainsType<T>(this IEnumerable<object> enumerable)
    {
        return ContainsType<T>(enumerable, out T temp);
    }

    public static bool ContainsType<T>(this IEnumerable<object> enumerable, out T value)
    {
        value = default;

        foreach (object obj in enumerable)
        {
            if(obj is T TValue)
            {
                value = TValue;
                return true;
            }
        }

        return false;
    }

    public static bool ContainsType<T, TEnumerableType>(this IEnumerable<TEnumerableType> enumerable)
    {
        return ContainsType<T, TEnumerableType>(enumerable, out T temp);
    }

    public static bool ContainsType<T, TEnumerableType>(this IEnumerable<TEnumerableType> enumerable, out T value)
    {
        value = default;

        foreach(TEnumerableType obj in enumerable)
        {
            if(obj is T TValue)
            {
                value = TValue;
                return true;
            }
        }

        return false;
    }

}
