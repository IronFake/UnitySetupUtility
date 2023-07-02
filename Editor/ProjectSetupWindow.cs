using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Storm.UnitySetupUtility.Editor
{
    public class ProjectSetupWindow : EditorWindow
    {
        [SerializeField] private List<PackageInfo> _packagesList = new List<PackageInfo>();

        private int _selectedTab;
        private string[] _tabNames = { "General", "Settings" };
        
        private ProjectSetupSettings _settings;
        private SerializedObject _settingSerializedObject;
        
        private SerializedObject _windowSerializedObject;
        private Vector2 _scrollPosition;

        [MenuItem("Window/Project Setup Window")]
        public static void ShowWindow()
        {
            GetWindow<ProjectSetupWindow>("Project Setup");
        }

        private void OnEnable()
        {
            _settings = Resources.Load<ProjectSetupSettings>("ProjectSetupSettings");
            _settingSerializedObject = new SerializedObject(_settings);
            _windowSerializedObject = new SerializedObject(this);

            RequestPackages();
        }
        
        private void OnGUI()
        {
            DrawTabButtons();
            switch (_selectedTab)
            {
                case 0:
                    DrawGeneralTab();
                    break;
                case 1:
                    DrawSettingsTab();
                    break;
            }
        }
        
        private void DrawTabButtons()
        {
            GUILayout.BeginHorizontal();

            for (int i = 0; i < _tabNames.Length; i++)
            {
                if (GUILayout.Toggle(_selectedTab == i, _tabNames[i], EditorStyles.toolbarButton))
                {
                    _selectedTab = i;
                }
            }

            GUILayout.EndHorizontal();
        }
        
        private void DrawGeneralTab()
        {
            _windowSerializedObject.Update();
            
            if (GUILayout.Button("Create Main Folders"))
            {
                Folders.CreateDirectories(_settings.MainFolders);
            }

            if (GUILayout.Button("Download manifest"))
            {
                ReplaceManifest();
            }

            EditorGUILayout.LabelField("Package List:", EditorStyles.boldLabel);
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandHeight(true));
            
            var packageListProperty = _windowSerializedObject.FindProperty("_packagesList");
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
            _windowSerializedObject.ApplyModifiedProperties();
        }
        
        private void DrawSettingsTab()
        {
            EditorUtils.DrawAllProperties(_settingSerializedObject);
        }

        private async Task RequestPackages()
        {
            _packagesList = await PackageUtils.GetPackagesFromGist(_settings.PackagesGist);
        }


        private async Task ReplaceManifest()
        {
            await PackageUtils.ReplaceManifestFromGist(_settings.ManifestGist);
            foreach (var packageInfo in _packagesList)
            {
                packageInfo.IsUpdated = false;
            }
        }
    }
}