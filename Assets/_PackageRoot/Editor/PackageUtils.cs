using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

namespace Storm.UnitySetupUtility.Editor
{
    public static class PackageUtils
    {
        public static bool CheckIfPackageInstall(string packageName)
        {
            ListRequest request = Client.List(true, false);
            while (request.IsCompleted == false);
            return CheckResultIfPackageInstalled(request, packageName);
        }
        
        public static async Task<bool> CheckIfPackageInstalledAsync(string packageName)
        {
            ListRequest request = Client.List(true, false);
            while (request.IsCompleted == false)
            {
                await Task.Yield();
            }
            
            return CheckResultIfPackageInstalled(request, packageName);
        }

        private static bool CheckResultIfPackageInstalled(ListRequest request, string packageName)
        {
            bool isPackageInstalled = false;
            if (request.IsCompleted)
            {
                if (request.Status == StatusCode.Success)
                {
                    isPackageInstalled = request.Result.Any((el) => el.name == packageName);
                    if (isPackageInstalled)
                    {
                        Debug.Log($"Package is installed: " + packageName);
                    }
                    else
                    {
                        Debug.Log($"Package is not installed: " + packageName);
                    }
                }
                else if (request.Status >= StatusCode.Failure)
                {
                    Debug.LogError("Failed to get list of packages: " + request.Error.message);
                }
            }

            return isPackageInstalled;
        }
        
        public static async Task DownloadPackageAsync(string packageUrl)
        {
            if (string.IsNullOrEmpty(packageUrl))
            {
                Debug.LogError("Failed to download package: Package url is invalid" + packageUrl);
                return;
            }
            
            string tempFolderPath = Path.GetTempPath();
            string saveLocation = Path.Combine(tempFolderPath, "package.unitypackage");
            
            if (string.IsNullOrEmpty(saveLocation))
            {
                // User canceled the save dialog
                Debug.LogError("Failed to download package: Save path is invalid" + packageUrl);
                return;
            }
            
            UnityWebRequest request = UnityWebRequest.Get(packageUrl);
            request.SendWebRequest();
            while (request.isDone == false)
            {
                await Task.Yield();
            }
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                await File.WriteAllBytesAsync(saveLocation, request.downloadHandler.data); 
                AssetDatabase.ImportPackage(saveLocation, true);
                Debug.Log("Package downloaded and imported successfully.");
                
                // Delete the downloaded file
                File.Delete(saveLocation);
                Debug.Log("Downloaded package file deleted.");
            }
            else
            {
                Debug.LogError("Package download failed. Error: " + request.error);
            }
        }
        
        public static async Task AddPackageAsync(string packageName, string packageUrl)
        {
            if (!string.IsNullOrEmpty(packageUrl))
            {
                AddRequest request = Client.Add(packageUrl);
                while (request.IsCompleted == false)
                {
                    await Task.Yield();
                }

                if (request.IsCompleted)
                {
                    if (request.Status == StatusCode.Success)
                    {
                        Debug.Log("Package added: " + packageName);
                    }
                    else if (request.Status >= StatusCode.Failure)
                    {
                        Debug.LogError("Failed to add package: " + request.Error.message);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Please enter a package name to add.");
            }
        }
        
        
        public static async Task RemovePackageAsync(string packageName)
        {
            RemoveRequest request = Client.Remove(packageName);
            while (request.IsCompleted == false)
            {
                await Task.Yield();
            }
            
            if (request.IsCompleted)
            {
                if (request.Status == StatusCode.Success)
                {
                    Debug.Log("Package removed: " + packageName);
                }
                else if (request.Status >= StatusCode.Failure)
                {
                    Debug.LogError("Failed to remove package: " + request.Error.message);
                }
            }
        }

        public static async Task ReplaceManifestFromGist(string id, string user)
        {
            var url = GetGistUrl(id, user);
            var contents = await GetContents(url);
            ReplacePackageFile(contents);
        }
        
        private static void ReplacePackageFile(string contents)
        {
            var existing = Path.Combine(Application.dataPath, "../Packages/manifest.json");
            File.WriteAllText(existing, contents);
            Client.Resolve();
        }

        public static async Task<List<PackageInfo>> GetPackagesFromGist(string id, string user)
        {
            List<PackageInfo> result = new List<PackageInfo>();

            var url = GetGistUrl(id, user);
            var contents = await GetContents(url);
            var lines = contents.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                result.Add(ConvertToPackageInfo(line));
            }

            return result;
        }
        
        private static PackageInfo ConvertToPackageInfo(string line)
        {
            Regex pattern = new Regex("[\": ]");
            var parts = line.Split(new [] {',', ' '}, StringSplitOptions.RemoveEmptyEntries);
            
            string displayName = pattern.Replace(parts[0], "");
            string packageName = pattern.Replace(parts[1], "");
            string url = pattern.Replace(parts[2], "");
            
            return new PackageInfo(displayName, packageName, url);
        }

        private static string GetGistUrl(string id, string user) => $"https://gist.githubusercontent.com/{user}/{id}/raw";

        public static async Task<string> GetContents(string url)
        {
            using (var request = UnityWebRequest.Get(url))
            {
                UnityWebRequestAsyncOperation operation = request.SendWebRequest();
                while (operation.isDone == false)
                {
                    await Task.Yield();
                }

                return request.downloadHandler.text;
            }
        }
    }
}