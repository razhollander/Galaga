namespace Modules.Analytics.Parameters
{
    public class BooleanAnalyticParameter : IAnalyticParameter
    {
        public string Name { get; }
        public object Value { get; }

        public BooleanAnalyticParameter(string name, bool value)
        {
            Name = name;
            Value = value;
        }
    }
}