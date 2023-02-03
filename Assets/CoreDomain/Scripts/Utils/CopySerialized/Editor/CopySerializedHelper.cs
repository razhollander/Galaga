using UnityEditor;

namespace CoreDomain.Scripts.Utils.CopySerialized.Editor
{
    public static class CopySerializedHelper
    {
        static SerializedObject source;

        [MenuItem("CONTEXT/Component/CopySerialized")]
        public static void CopySerializedFromBase(MenuCommand command)
        {
            source = new SerializedObject(command.context);
        }

        [MenuItem("CONTEXT/Component/PasteSerialized")]
        public static void PasteSerializedFromBase(MenuCommand command)
        {
            SerializedObject dest = new SerializedObject(command.context);
            SerializedProperty propIterator = source.GetIterator();

            //jump into serialized object, this will skip script type so that we dont override the destination component's type
            if (propIterator.NextVisible(true))
            {
                while (propIterator.NextVisible(true)) //iterate through all serializedProperties
                {
                    //try obtaining the property in destination component
                    SerializedProperty propElement = dest.FindProperty(propIterator.name);

                    //validate that the properties are present in both components, and that they're the same type
                    if (propElement != null && propElement.propertyType == propIterator.propertyType)
                    {
                        //copy value from source to destination component
                        dest.CopyFromSerializedProperty(propIterator);
                    }
                }
            }

            dest.ApplyModifiedProperties();
        }
    }
}