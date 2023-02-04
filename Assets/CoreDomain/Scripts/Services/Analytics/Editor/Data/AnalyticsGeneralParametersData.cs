using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Modules.Analytics.Editor.Data
{
    [Serializable]
    public class AnalyticsGeneralParametersData
    {
        [JsonProperty("general_parameters")] public List<AnalyticPropertyData> GeneralParameters;
    }
}