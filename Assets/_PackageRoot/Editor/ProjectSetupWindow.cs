using System.Collections.Generic;
using System.IO;
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

        private ProjectSetupSettingsProvider _settingsProvider;
        private SerializedObject _settingProviderSerializedObject;

        private SerializedObject _windowSerializedObject;
        private Vector2 _scrollPosition;

        [MenuItem("Window/Project Setup Window")]
        public static void ShowWindow()
        {
            GetWindow<ProjectSetupWindow>("Project Setup");
        }

        private void OnEnable()
        {
            _settingsProvider = Resources.Load<ProjectSetupSettingsProvider>("ProjectSetupSettingsProvider");
  
            _settingProviderSerializedObject = new SerializedObject(_settingsProvider);
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
                Folders.CreateDirectories(_settingsProvider.Settings.MainFolders);
            }

            if (GUILayout.Button("Download manifest"))
            {
                ReplaceManifest();
            }
            
            EditorGUILayout.Space(10);
            if (GUILayout.Button("Refresh packaged"))
            {
                RequestPackages();
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
            var settingsProperty = _settingProviderSerializedObject.FindProperty("_settings");
            EditorUtils.DrawAllProperties(settingsProperty);
            
            if (GUILayout.Button("Load from JSON"))
            {
                var selectedFilePath = EditorUtility.OpenFilePanel(
                    "Open JSON File", Application.dataPath, "json");

                if (!string.IsNullOrEmpty(selectedFilePath))
                {
                    string jsonContent = File.ReadAllText(selectedFilePath);
                    _settingsProvider.Settings = ParseJsonToSettings(jsonContent);
                    _settingProviderSerializedObject.Update();
                    
                    RequestPackages();
                }
            }
            
            if (GUILayout.Button("Save to JSON"))
            {
                var selectedFilePath = EditorUtility.SaveFilePanel(
                    "Save settings", Application.dataPath, "ProjectSetupSettings","json");

                if (selectedFilePath.Length != 0)
                {
                    string jsonContent = JsonUtility.ToJson(_settingsProvider.Settings, true);
                    if (jsonContent != null)
                    {
                        File.WriteAllText(selectedFilePath, jsonContent);
                    }
                }
            }
            
            _settingProviderSerializedObject.ApplyModifiedProperties();
        }

        private async Task RequestPackages()
        {
            _packagesList = await PackageUtils.GetPackagesFromGist(_settingsProvider.Settings.PackagesGist);
        }

        private async Task ReplaceManifest()
        {
            await PackageUtils.ReplaceManifestFromGist(_settingsProvider.Settings.ManifestGist);
            foreach (var packageInfo in _packagesList)
            {
                packageInfo.IsUpdated = false;
            }
        }

        private ProjectSetupSettingsModel ParseJsonToSettings(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return new ProjectSetupSettingsModel();
            }

            ProjectSetupSettingsModel projectSetupSettingsModel = JsonUtility.FromJson<ProjectSetupSettingsModel>(json);
            return projectSetupSettingsModel;
        }
    }
}