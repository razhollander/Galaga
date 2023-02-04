namespace Modules.Analytics.Parameters
{
    public class StringAnalyticParameter : IAnalyticParameter
    {
        public string Name { get; }
        public object Value { get; }

        public StringAnalyticParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}