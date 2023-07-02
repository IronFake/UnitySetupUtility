using System.IO;
using UnityEditor;
using UnityEngine;

namespace Storm.UnitySetupUtility.Editor
{
    public static class Folders
    {
        public static void CreateDirectories(params string[] dir)
        {
            Dir(dir);
            AssetDatabase.Refresh();
        }

        private static void Dir(params string[] dir)
        {
            foreach (var newDirectory in dir)
            {
                Directory.CreateDirectory(Path.Combine(Application.dataPath, newDirectory));
            }
        }
    }
}