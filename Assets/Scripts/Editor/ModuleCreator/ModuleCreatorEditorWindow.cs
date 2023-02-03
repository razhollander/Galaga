using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Modules.Editor
{
    public class ModuleCreatorEditorWindow : OdinEditorWindow
    {
        private string _templatesFolder;
        private const string MODULE_WILDCARD ="xxmodule_namexx";
        private const string MODULE_WILDCARD_LOWER_CASE = "<module_name_lower>";
        private const string GAME_CLIENT_PATH = "Scripts/Game/GameClient.cs";

        [MenuItem("Tools/Module Generator")]
        public static void ShowWindow()
        {
            var window = GetWindow<ModuleCreatorEditorWindow>("");
            // Sets a minimum size to the window.
            window.minSize = new Vector2(340f, 250f);
            window.Show();
        }

        [VerticalGroup(1)] [InspectorName("Name Module")]
        public string _moduleName;

        [VerticalGroup(2)]
        [Button(ButtonSizes.Large)]
        public void Create()
        {
            if (string.IsNullOrWhiteSpace(_moduleName))
            {
                Debug.LogError("Module name is empty");
            }
            else
            {
                CreateModule();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _templatesFolder = Path.Combine(Application.dataPath, "Scripts/Templates/ModulesTemplate");
        }

        private void CreateModule()
        {
            string cleanedModuleName = _moduleName.Trim();
            cleanedModuleName = cleanedModuleName.Replace("module", "");
            cleanedModuleName = cleanedModuleName.Replace("Module", "");
            if (string.IsNullOrWhiteSpace(cleanedModuleName))
            {
                Debug.LogError("Module name is empty");
                return;
            }

            string moduleFolderPath = $"{Application.dataPath}/Scripts/Modules/{cleanedModuleName}/";
            Directory.CreateDirectory(moduleFolderPath);
            foreach (string file in Directory.GetFiles(_templatesFolder, "*.txt"))
            {
                Debug.Log($"Copying: {file}");
                CSFileEditUtils.CopyFileAndReplaceWildcard(file, moduleFolderPath, MODULE_WILDCARD, cleanedModuleName);
            }

            string gameClientPath = Path.Combine(Application.dataPath, GAME_CLIENT_PATH);
            CSFileEditUtils.AddLineToFunction(gameClientPath, "InitModules()",
                $"            _modules.Add<I{cleanedModuleName}Module>(new {cleanedModuleName}Module(this));");
            CSFileEditUtils.AddUsingToFile(gameClientPath, $"using Modules.{cleanedModuleName};");
            AssetDatabase.Refresh();
        }
    }
}