namespace Modules.Analytics.Parameters
{
    public interface IAnalyticParameter
    {
        string Name { get; }
        object Value { get; }
    }
}