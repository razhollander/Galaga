namespace Modules.Analytics.Parameters
{
    public class ConstAnalyticParameter : IAnalyticParameter
    {
        public string Name { get; }
        public object Value { get; }

        public ConstAnalyticParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}