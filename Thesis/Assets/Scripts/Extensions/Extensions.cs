using System;
using System.Collections.Generic;

public static class HSMExtensions
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

    public static bool Contains<TState, TTrigger>(this List<HSMTransition<TState, TTrigger>> list, in HSMTransition<TState, TTrigger> transition, Func<TState, TState, bool> stateComparer, Func<TTrigger, TTrigger, bool> triggerComparer) where TState : unmanaged where TTrigger : unmanaged
    {
        for (int i = 0; i < list.Count; i++)
        {
            HSMTransition<TState, TTrigger> current = list[i];

            if (current.Equals(transition, stateComparer, triggerComparer))
            {
                return true;
            }
        }

        return false;
    }

}
