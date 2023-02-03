namespace Modules.Analytics.Parameters
{
    public class FloatAnalyticParameter : IAnalyticParameter
    {
        public string Name { get; }
        public object Value { get; }

        public FloatAnalyticParameter(string name, float value)
        {
            Name = name;
            Value = value;
        }
    }
}