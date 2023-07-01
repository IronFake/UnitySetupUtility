using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Storm.UnitySetupUtility.Editor
{
    [CustomPropertyDrawer(typeof(PackageInfo))]
    public class PackageInfoDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;
            return (lineHeight + verticalSpacing);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty packageDisplayNameProperty = property.FindPropertyRelative("DisplayName");
            SerializedProperty packageNameProperty = property.FindPropertyRelative("Name");
            SerializedProperty packageUrlProperty = property.FindPropertyRelative("Url");
            SerializedProperty isInstalledProperty = property.FindPropertyRelative("IsInstalled");
            SerializedProperty isUpdatedProperty = property.FindPropertyRelative("IsUpdated");
            SerializedProperty isAssetProperty = property.FindPropertyRelative("IsAsset");

            Rect labelRect = new Rect(position.x, position.y, position.width - 80, EditorGUIUtility.singleLineHeight);
            Rect buttonRect = new Rect(position.x + position.width - 70, position.y, 70,
                EditorGUIUtility.singleLineHeight);

            EditorGUI.LabelField(labelRect, packageDisplayNameProperty.stringValue);
            
            bool previousGUIState = GUI.enabled;
            if (isAssetProperty.boolValue)
            {
                GUI.enabled = !isUpdatedProperty.boolValue;
                if (GUI.Button(buttonRect, "Import"))
                {
                    DownloadPackage(packageUrlProperty.stringValue, isUpdatedProperty);
                }
                
                GUI.enabled = previousGUIState;
            }
            else
            {
                if (isUpdatedProperty.boolValue == false)
                {
                    isInstalledProperty.boolValue = PackageUtils.CheckIfPackageInstall(packageNameProperty.stringValue);
                    isUpdatedProperty.boolValue = true;
                }
            
                GUI.backgroundColor = isInstalledProperty.boolValue ? EditorGuiStyle.RedColor : EditorGuiStyle.GreenColor;

                if (isInstalledProperty.boolValue)
                {
                    if (GUI.Button(buttonRect, "Remove"))
                    {
                        RemovePackage(packageNameProperty.stringValue, isUpdatedProperty);
                    }
                }
                else
                {
                    if (GUI.Button(buttonRect, "Add"))
                    {
                        AddPackage(packageNameProperty.stringValue, packageUrlProperty.stringValue, isUpdatedProperty);
                    }
                }
            
                GUI.backgroundColor = Color.white;
            }
            
            EditorGUI.EndProperty();
        }
        
        private async Task DownloadPackage(string packageUrl, SerializedProperty isUpdatedProperty)
        {
            isUpdatedProperty.boolValue = true;
            await PackageUtils.DownloadPackageAsync(packageUrl);
            isUpdatedProperty.boolValue = false;
            isUpdatedProperty.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private async Task AddPackage(string packageName, string packageUrl, SerializedProperty isUpdatedProperty)
        {
            await PackageUtils.AddPackageAsync(packageName, packageUrl);
            isUpdatedProperty.boolValue = false;
            isUpdatedProperty.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private async Task RemovePackage(string packageName, SerializedProperty isUpdatedProperty)
        {
            await PackageUtils.RemovePackageAsync(packageName);
            isUpdatedProperty.boolValue = false;
            isUpdatedProperty.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}