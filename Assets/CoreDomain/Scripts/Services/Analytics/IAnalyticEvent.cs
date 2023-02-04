using System.Collections.Generic;
using Modules.Analytics.Generated;
using Modules.Analytics.Parameters;

namespace Modules.Analytics
{
    public interface IAnalyticEvent
    {
        string Name { get; }
        List<IAnalyticParameter> Process(IAnalyticParametersCollection _analyticParametersCollectionName);
    }
}