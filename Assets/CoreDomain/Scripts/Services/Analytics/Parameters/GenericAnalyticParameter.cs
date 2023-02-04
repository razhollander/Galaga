namespace Modules.Analytics.Parameters
{
    public class GenericAnalyticParameter : IAnalyticParameter
    {
        public string Name { get; }
        public object Value { get; }

        public GenericAnalyticParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}