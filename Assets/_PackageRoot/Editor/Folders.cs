using System.IO;
using UnityEditor;
using UnityEngine;

namespace Storm.UnitySetupUtility.Editor
{
    public static class Folders
    {
        public static void CreateDirectories(string root, params string[] dir)
        {
            Dir(root, dir);
            AssetDatabase.Refresh();
        }

        private static void Dir(string root, params string[] dir)
        {
            var fullPath = Path.Combine(Application.dataPath, root);
            foreach (var newDirectory in dir)
            {
                Directory.CreateDirectory(Path.Combine(fullPath, newDirectory));
            }
        }
    }
}