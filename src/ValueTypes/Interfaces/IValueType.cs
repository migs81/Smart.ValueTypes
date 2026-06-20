namespace Migs.ValueTypes.Interfaces
{
    public interface IValueType<in TValue, TThis>
        where TValue : notnull
        where TThis : IValueType<TValue, TThis>
    {
        bool Equals(TValue value);
    }

    public interface IValueType<in T0, in T1, TThis>
        where T0 : notnull
        where T1 : notnull
        where TThis : IValueType<T0, T1, TThis>
    {
        bool Equals(T0 arg0, T1 arg1);
    }

    public interface IValueType<in T0, in T1, in T2, TThis>
        where T0 : notnull
        where T1 : notnull
        where T2 : notnull
        where TThis : IValueType<T0, T1,T2, TThis>
    {
        bool Equals(T0 arg0, T1 arg1, T2 arg2);
    }

    public interface IValueType<in T0, in T1, in T2, in T3, TThis>
        where T0 : notnull
        where T1 : notnull
        where T2 : notnull
    where TThis : IValueType<T0, T1, T2, T3, TThis>
    {
        bool Equals(T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    }

    public interface IValueType<in T0, in T1, in T2, in T3, in T4, TThis>
        where T0 : notnull
        where T1 : notnull
        where T2 : notnull
        where TThis : IValueType<T0, T1, T2, T3, T4, TThis>
    {
        bool Equals(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    }
}
