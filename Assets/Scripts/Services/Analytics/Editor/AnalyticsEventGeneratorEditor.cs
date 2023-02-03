using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Handlers.Serializers.Serializer;
using Modules.Analytics.Editor.Data;
using Modules.Analytics.Parameters;
using Services.SerializerService;
using UnityEditor;
using UnityEngine;

namespace Modules.Analytics.Editor
{
 internal class AnalyticsEventGeneratorEditor
    {
        private static readonly string _generalParamsPath = Path.Combine(Application.dataPath, "Scripts", "Modules", "Analytics", "Generated");
        private static readonly string _eventsPath = Path.Combine(Application.dataPath, "Scripts", "Modules", "Analytics", "Generated", "Events");
        private static readonly string _eventsConfigPath = Path.Combine(Application.dataPath, "Scripts", "Modules", "Analytics", "Editor", "Jsons", "Events");
        private static readonly string _generalParametersPath = Path.Combine(Application.dataPath, "Scripts", "Modules", "Analytics", "Editor", "Jsons", "general_parameters.json");
        
        private const string AnalyticEventPostfix = "AnalyticEvent";
        private const string ParameterStillNotInUsedComment = "Parameter still not in used in analytics systems, because there is no default value. This feature come from product side.";
        private const string AnalyticParametersCollectionName = "IAnalyticParametersCollection";
        
        [MenuItem("Tools/Analytics/Generate scripts")]
        public static void Generate()
        {
            CreatePathForGeneratedScripts();
            
            var generatedScriptsFolderPath = Path.Combine(_generalParamsPath, $"{AnalyticParametersCollectionName}.cs");
            
            var parametersBuilder = new StringBuilder();
            parametersBuilder.AppendLine($"// This code autogenerated by {nameof(AnalyticsEventGeneratorEditor)}");
            parametersBuilder.AppendLine($"");
            parametersBuilder.AppendLine($"namespace Modules.Analytics.Generated");
            parametersBuilder.AppendLine($"{{");
            parametersBuilder.AppendLine($"   public interface {AnalyticParametersCollectionName} "); 
            parametersBuilder.AppendLine($"   {{");
            
            GenerateGeneralParams(parametersBuilder);
            GenerateEventsAndParametersInterface(parametersBuilder);
            
            parametersBuilder.AppendLine($"   }}");
            parametersBuilder.AppendLine($"}}");
            
            SaveCode(parametersBuilder, generatedScriptsFolderPath);
            
            AssetDatabase.Refresh();
        }
        
        [MenuItem("Tools/Analytics/Clear Generated")]
        public static void ClearGenerated()
        {
            CreatePathForGeneratedScripts();
        }

        private static void GenerateEventsAndParametersInterface(StringBuilder parametersBuilder)
        {
            var dir = new DirectoryInfo(_eventsConfigPath);
            var filesList = dir.GetFiles("*.json");
            var globalParamsList = new Dictionary<string, string>();
            
            foreach (var item in filesList)
            {
                var eventData = GetEventData(item.FullName);
                GenerateInterfaceParameter(eventData, globalParamsList, parametersBuilder);
                GenerateEvent(eventData);
            }
        }

        private static void GenerateInterfaceParameter(
            AnalyticEventData data, 
            Dictionary<string, string> createdParam,
            StringBuilder codeBuilder)
        {
            foreach (var parameter in data.Parameters)
            {
                if(createdParam.ContainsKey(parameter.Name))
                {
                    if (createdParam[parameter.Name] != parameter.Type)
                    {
                        throw new Exception($"Few parameters with same name but with different type. name: {parameter.Name}, type: {parameter.Type} in event {data.Name}");
                    }
                }
                else
                {
                    createdParam.Add(parameter.Name, parameter.Type);
                    codeBuilder.AppendLine($"       {ConvertType(parameter.Type)} Get{ConvertToMainLetterUpperWord(parameter.Name)} ({ConvertType(parameter.Type)} defaultValue);");
                }
            }
        }

        private static void CreatePathForGeneratedScripts()
        {
            RecreatePath(_generalParamsPath);
            RecreatePath(_eventsPath);
        }

        private static void RecreatePath(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);
        }

        private static AnalyticsGeneralParametersData GetGeneralParameterData(string path)
        {
            var serializer = new SerializerService();
            var dataText = File.ReadAllText(path);
            return serializer.DeserializeJson<AnalyticsGeneralParametersData>(dataText);
        }

        private static void GenerateEvent(AnalyticEventData data)
        {
            var className = $"{ConvertToMainLetterUpperWord(data.Name)}{AnalyticEventPostfix}";

            var generatedScriptsFolderPath = Path.Combine(_eventsPath, $"{className}.cs");

            var eventBuilder = new StringBuilder();
            eventBuilder.AppendLine($"// This code autogenerated by {nameof(AnalyticsEventGeneratorEditor)}");
            eventBuilder.AppendLine($"using Modules.Analytics.Parameters;");
            eventBuilder.AppendLine($"using System.Collections.Generic;");
            eventBuilder.AppendLine($"");
            eventBuilder.AppendLine($"namespace Modules.Analytics.Generated.Events");
            eventBuilder.AppendLine($"{{");
            eventBuilder.AppendLine($"   public class {className} : {nameof(IAnalyticEvent)}");
            eventBuilder.AppendLine($"   {{");
            eventBuilder.AppendLine($"       public string {nameof(IAnalyticEvent.Name)} => \"{data.Name}\";");

            eventBuilder.AppendLine($"       private readonly List<IAnalyticParameter> _parameters = new();");
            eventBuilder.AppendLine($"       private readonly {AnalyticParametersCollectionName} _analyticParametersCollectionName;");

            var constructorArguments = string.Empty;
            var eventBody = string.Empty;
            
            foreach (var parameter in data.Parameters)
            {
                if(!parameter.ConstructorProperty)
                {
                    continue;
                }
                
                if (constructorArguments.Length > 0)
                {
                    constructorArguments += ", ";
                    eventBody += "\n";
                }
                constructorArguments += $"{ConvertType(parameter.Type)} {parameter.Name}";

                eventBody += $"           _parameters.Add(new GenericAnalyticParameter(\"{parameter.Name}\", {parameter.Name}));";
            }
            
            eventBuilder.AppendLine($"");
            eventBuilder.AppendLine($"       public {className} ({constructorArguments})");
            eventBuilder.AppendLine($"       {{");
            eventBuilder.AppendLine(eventBody);
            eventBuilder.AppendLine($"       }}");
            eventBuilder.AppendLine($"");
            
            eventBuilder.AppendLine($"       public List<{nameof(IAnalyticParameter)}> Process ({AnalyticParametersCollectionName} _analyticParametersCollectionName)");
            eventBuilder.AppendLine($"       {{");
            eventBuilder.AppendLine($"           var list = new List<{nameof(IAnalyticParameter)}>();");

            foreach (var parameter in data.Parameters)
            {
                if (parameter.ConstructorProperty)
                {
                    continue;
                }

                AppendParameter(parameter, eventBuilder);
            }

            eventBuilder.AppendLine($"           list.AddRange(_parameters);");
            eventBuilder.AppendLine($"           return list;");
            eventBuilder.AppendLine($"       }}");
            
            eventBuilder.AppendLine($"   }}");
            eventBuilder.AppendLine($"}}");

            SaveCode(eventBuilder, generatedScriptsFolderPath);
        }

        private static AnalyticEventData GetEventData(string path)
        {
            Debug.Log($"GetAnalyticsData path: {path}");
            
            var serializer = new SerializerService();
            var dataText = File.ReadAllText(path);
            return serializer.DeserializeJson<AnalyticEventData>(dataText);
        }

        private static void GenerateGeneralParams(StringBuilder parametersCollectionBuilder)
        {
            var data = GetGeneralParameterData(_generalParametersPath);
            var className = $"GeneralParametersList";
        
            var generatedScriptsFolderPath = Path.Combine(_generalParamsPath, $"{className}.cs");
            var codeBuilder = new StringBuilder();
            
            codeBuilder.AppendLine($"// This code autogenerated by {nameof(AnalyticsEventGeneratorEditor)}"); 
            codeBuilder.AppendLine($"using System.Collections.Generic;"); 
            codeBuilder.AppendLine($"using Modules.Analytics.Parameters;"); 
            codeBuilder.AppendLine($"");
            codeBuilder.AppendLine($"namespace Modules.Analytics.Generated");
            codeBuilder.AppendLine($"{{");
            codeBuilder.AppendLine($"   public static class {className} "); 
            codeBuilder.AppendLine($"   {{");
            codeBuilder.AppendLine($"       public static List<{nameof(IAnalyticParameter)}> Process ({AnalyticParametersCollectionName} _analyticParametersCollectionName)");
            codeBuilder.AppendLine($"       {{");
            codeBuilder.AppendLine($"           var list = new List<{nameof(IAnalyticParameter)}>();");

            foreach (var parameter in data.GeneralParameters)
            {
                AppendParameter(parameter, codeBuilder);
            }
            
            codeBuilder.AppendLine($"           return list;");
            codeBuilder.AppendLine($"       }}");
            codeBuilder.AppendLine($"   }}");
            codeBuilder.AppendLine($"}}");
            
            SaveCode(codeBuilder, generatedScriptsFolderPath);
            
            foreach (var parameter in data.GeneralParameters)
            {
                if (parameter.DefaultValue == null)
                {
                    parametersCollectionBuilder.AppendLine($"        //\"{parameter.Name}\", {ParameterStillNotInUsedComment}");
                }
                else
                {
                    parametersCollectionBuilder.AppendLine($"       {ConvertType(parameter.Type)} Get{ConvertToMainLetterUpperWord(parameter.Name)} ({ConvertType(parameter.Type)} defaultValue);");
                }
            }
        }
        
        private static void AppendParameter(AnalyticPropertyData parameterData, StringBuilder codeBuilder)
        {
            if (parameterData.DefaultValue == null)
            {
                return;
            }
            
            var nameProperty = ConvertToMainLetterUpperWord(parameterData.Name);
            switch (parameterData.Type)
            {
                case "const":
                    codeBuilder.AppendLine($"           list.Add(  new GenericAnalyticParameter(\"{parameterData.Name}\", _analyticParametersCollectionName.Get{nameProperty}(\"{parameterData.DefaultValue}\")));");

                    break;
                case "int":
                    codeBuilder.AppendLine($"           list.Add(  new GenericAnalyticParameter(\"{parameterData.Name}\", _analyticParametersCollectionName.Get{nameProperty}({parameterData.DefaultValue})));");

                    break;
                case "float":
                    codeBuilder.AppendLine($"           list.Add(  new GenericAnalyticParameter(\"{parameterData.Name}\", _analyticParametersCollectionName.Get{nameProperty}({parameterData.DefaultValue}f)));");

                    break;
                case "string":
                    codeBuilder.AppendLine($"           list.Add(  new GenericAnalyticParameter(\"{parameterData.Name}\", _analyticParametersCollectionName.Get{nameProperty}(\"{parameterData.DefaultValue}\")));");

                    break;
                case "boolean":
                    codeBuilder.AppendLine($"           list.Add(  new GenericAnalyticParameter(\"{parameterData.Name}\", _analyticParametersCollectionName.Get{nameProperty}({parameterData.DefaultValue})));");

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void SaveCode(StringBuilder codeBuilder, string generatedScriptsFolderPath)
        {
            using var fileStream = File.Open(generatedScriptsFolderPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            using var streamWriter = new StreamWriter(fileStream);
            streamWriter.Write(codeBuilder.ToString());
        }
        
        private static string ConvertToMainLetterUpperWord(string str)
        {
            return string.Join("", str.Split("_").Select(i => char.ToUpper(i[0]) + i.Substring(1)).ToArray());
        }

        private static string ConvertType(string inputType)
        {
            var returnType = string.Empty;
            
            switch (inputType)
            {
                case "const":
                    returnType = "string";
                    break;
                case "int":
                    returnType = "int";
                    break;
                case "float":
                    returnType = "float";
                    break;
                case "string":
                    returnType = "string";
                    break;
                case "boolean":
                    returnType = "bool";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return returnType;
        }
    }
}