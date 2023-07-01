using System;

namespace Storm.UnitySetupUtility.Editor
{
    [Serializable]
    public class PackageInfo
    {
        public string DisplayName;
        public string Name;
        public string Url;
        public bool IsAsset;

        public bool IsInstalled;
        public bool IsUpdated;

        public PackageInfo(string displayName, string name, string path, bool isAsset = false)
        {
            DisplayName = displayName;
            Name = name;
            Url = path;
            IsAsset = isAsset;
        }
        
        public PackageInfo(string displayName, string path, bool isAsset = true)
        {
            DisplayName = displayName;
            Name = displayName;
            Url = path;
            IsAsset = isAsset;
        }
    }
}