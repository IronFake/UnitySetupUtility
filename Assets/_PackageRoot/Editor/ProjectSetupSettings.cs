using UnityEngine;

namespace Storm.UnitySetupUtility.Editor
{
    public class ProjectSetupSettings : ScriptableObject
    {
        [SerializeField] private string[] _folderPaths;
        [SerializeField] private GistInfo _manifestGist;
        [SerializeField] private GistInfo _packagesGist;

        public string[] FolderPaths => _folderPaths;

        public GistInfo ManifestGist => _manifestGist;

        public GistInfo PackagesGist => _packagesGist;
    }
}