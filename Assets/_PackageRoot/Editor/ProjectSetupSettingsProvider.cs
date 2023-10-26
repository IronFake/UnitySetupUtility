using System;
using UnityEngine;

namespace Storm.UnitySetupUtility.Editor
{
    public class ProjectSetupSettingsProvider : ScriptableObject
    {
        [SerializeField] private ProjectSetupSettingsModel _settings;

        public ProjectSetupSettingsModel Settings
        {
            get => _settings;
            set => _settings = value;
        }
    }
}