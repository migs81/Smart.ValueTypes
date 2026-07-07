namespace Smart.ValueTypes.Interfaces
{
    public interface IValueType<in TValue, TThis>
        where TValue : notnull
        where TThis : IValueType<TValue, TThis>;

    public interface IValueType<in T0, in T1, TThis>
        where T0 : notnull
        where T1 : notnull
        where TThis : IValueType<T0, T1, TThis>;

    public interface IValueType<in T0, in T1, in T2, TThis>
        where T0 : notnull
        where T1 : notnull
        where T2 : notnull
        where TThis : IValueType<T0, T1,T2, TThis>;

    public interface IValueType<in T0, in T1, in T2, in T3, TThis>
        where T0 : notnull
        where T1 : notnull
        where T2 : notnull
        where TThis : IValueType<T0, T1, T2, T3, TThis>;

    public interface IValueType<in T0, in T1, in T2, in T3, in T4, TThis>
        where T0 : notnull
        where T1 : notnull
        where T2 : notnull
        where TThis : IValueType<T0, T1, T2, T3, T4, TThis>;
}
