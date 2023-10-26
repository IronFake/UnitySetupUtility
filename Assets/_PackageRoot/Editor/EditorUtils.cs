using UnityEditor;

namespace Storm.UnitySetupUtility.Editor
{
    public static class EditorUtils
    {
        public static void DrawAllProperties(SerializedObject serializedObject, bool skipScriptName = true)
        {
            SerializedProperty property = serializedObject.GetIterator();

            bool next = property.NextVisible(true);
            if (skipScriptName)
            {
                next = property.NextVisible(true);
            }
            
            while (next)
            {
                EditorGUILayout.PropertyField(property, true);
                next = property.NextVisible(false);
            }

            serializedObject.ApplyModifiedProperties();
        }
        
        public static void DrawAllProperties(SerializedProperty property)
        {
            bool next = property.NextVisible(true);
            while (next)
            {
                EditorGUILayout.PropertyField(property, true);
                next = property.NextVisible(false);
            }
        }
    }
}