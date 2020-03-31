﻿public static class ObjectExtensions
{
    public static void Inject<T>(this object obj, string fieldOrPropertyName, T value)
    {
        var field = obj.GetType()
            .GetField(fieldOrPropertyName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

        if(field != null)
            field.SetValue(obj, value);
        else
        {
            obj.GetType()
                .GetProperty(fieldOrPropertyName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(obj, value);
        }
    }

    public static T Inspect<T>(this object obj, string fieldOrPropertyName)
    {
        var field = obj.GetType()
            .GetField(fieldOrPropertyName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

        if (field != null)
            return (T)field.GetValue(obj);
        else
        {
            return (T)obj.GetType()
                .GetProperty(fieldOrPropertyName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .GetValue(obj);
        }
    }
}
