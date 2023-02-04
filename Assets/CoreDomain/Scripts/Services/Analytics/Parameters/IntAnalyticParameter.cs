namespace Modules.Analytics.Parameters
{
    public class IntAnalyticParameter : IAnalyticParameter
    {
        public string Name { get; }
        public object Value { get; }

        public IntAnalyticParameter(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}