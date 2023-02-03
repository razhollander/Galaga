using System.Collections.Generic;
using Modules.Analytics.Parameters;

namespace Modules.Analytics.Analytics
{
    public interface IAnalyticTransport
    {
        void Trigger(IAnalyticEvent analyticEvent, IEnumerable<IAnalyticParameter> eventParameters, IEnumerable<IAnalyticParameter> generalParameters);
    }
}