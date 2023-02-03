using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace CoreDomain.Scripts.Utils.SceneLoadWindow.Editor
{
    public class SceneLoadWindow : OdinEditorWindow
    {

        [TableList(AlwaysExpanded = true, HideToolbar = true)] public SceneLoadButton[] Scenes;

        public void OnBecameVisible()
        {
            Scenes = GenerateSceneLoadButtons();
        }

        [MenuItem("Tools/SceneTool _%g")]
        private static void OpenWindow()
        {
            GetWindow<SceneLoadWindow>().Show();
        }


        private static SceneLoadButton[] GenerateSceneLoadButtons()
        {
            var sceneLoadButtons = new SceneLoadButton[EditorBuildSettings.scenes.Length];

            for (var i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                var editorBuildSettingsScene = EditorBuildSettings.scenes[i];

                sceneLoadButtons[i] = new SceneLoadButton(editorBuildSettingsScene.path);
            }

            return sceneLoadButtons;
        }
    }

    
    public class SceneLoadButton
    {
        private readonly string _path;
        private string _buttonName;

        public SceneLoadButton(string path)
        {
            _path = path;
            _buttonName = Path.GetFileNameWithoutExtension(path);
        }

        [GUIColor(.36f, 0.42f, 0.68f , 1)]
        [Button("$_buttonName", ButtonSizes.Large)]
        public void OpenScene()
        {
            EditorSceneManager.SaveOpenScenes();
            EditorSceneManager.OpenScene(_path);
        }
    }
}