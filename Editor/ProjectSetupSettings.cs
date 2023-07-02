using UnityEngine;

namespace Storm.UnitySetupUtility.Editor
{
    public class ProjectSetupSettings : ScriptableObject
    {
        [SerializeField] private string[] _mainFolders;
        [SerializeField] [InspectorName("Gist for Manifest")]  private GistInfo _manifestGist;
        [SerializeField] [InspectorName("Gist for List of Packages")] private GistInfo _packagesGist;

        public string[] MainFolders => _mainFolders;

        public GistInfo ManifestGist => _manifestGist;

        public GistInfo PackagesGist => _packagesGist;
    }
}