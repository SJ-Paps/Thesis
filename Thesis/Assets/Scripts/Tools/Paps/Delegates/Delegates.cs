namespace Paps.Delegates
{
    public delegate void RefAction<T>(ref T param);
    public delegate void RefAction<T1, T2>(ref T1 param1, ref T2 param2);
    public delegate void RefAction<T1, T2, T3>(ref T1 param1, ref T2 param2, ref T3 param3);
    public delegate void RefAction<T1, T2, T3, T4>(ref T1 param1, ref T2 param2, ref T3 param3, ref T4 param4);

    public delegate void InAction<T>(in T param);
    public delegate void InAction<T1, T2>(in T1 param1, in T2 param2);
    public delegate void InAction<T1, T2, T3>(in T1 param1, in T2 param2, in T3 param3);
    public delegate void InAction<T1, T2, T3, T4>(in T1 param1, in T2 param2, in T3 param3, in T4 param4);

    public delegate void OutAction<T>(out T param);
    public delegate void OutAction<T1, T2>(out T1 param1, out T2 param2);
    public delegate void OutAction<T1, T2, T3>(out T1 param1, out T2 param2, out T3 param3);
    public delegate void OutAction<T1, T2, T3, T4>(out T1 param1, out T2 param2, out T3 param3, out T4 param4);
}
