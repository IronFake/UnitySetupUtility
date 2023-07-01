using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Storm.UnitySetupUtility.Editor
{
    public class ProjectSetupWindow : EditorWindow
    {
        [SerializeField] private List<PackageInfo> _packagesList = new List<PackageInfo>();

        private SerializedObject _serializedObject;
        private Vector2 _scrollPosition;

        [MenuItem("Window/Project Setup Window")]
        public static void ShowWindow()
        {
            GetWindow<ProjectSetupWindow>("Project Setup");
        }

        private void OnEnable()
        {
            RequestPackages();
            
            _serializedObject = new SerializedObject(this);
        }
        
        private void OnGUI()
        {
            _serializedObject.Update();

            if (GUILayout.Button("Create Default Folders"))
            {
                Folders.CreateDirectories("_Project", "Scripts", "Art", "Scenes", "Prefabs");
            }

            if (GUILayout.Button("Replace Manifest from gist"))
            {
                ReplaceManifest();
            }

            EditorGUILayout.LabelField("Package List:", EditorStyles.boldLabel);
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandHeight(true));
            
            var packageListProperty = _serializedObject.FindProperty("_packagesList");
            for (int i = 0; i < packageListProperty.arraySize; i++)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUI.indentLevel++;

                SerializedProperty packageProperty = packageListProperty.GetArrayElementAtIndex(i);

                EditorGUILayout.PropertyField(packageProperty, true);

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }

            GUILayout.EndScrollView();
            _serializedObject.ApplyModifiedProperties();
        }
        
        private async Task RequestPackages()
        {
            _packagesList = await PackageUtils.GetPackagesFromGist("07ad9070801dbf3aab326e967683ae32", "IronFake");
        }


        private async Task ReplaceManifest()
        {
            await PackageUtils.ReplaceManifestFromGist("e534972ee09ec6f940b3b1a44574d699", "IronFake");
            foreach (var packageInfo in _packagesList)
            {
                packageInfo.IsUpdated = false;
            }
        }
    }
}