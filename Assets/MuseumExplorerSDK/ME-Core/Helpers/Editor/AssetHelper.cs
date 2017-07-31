using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Helpers;

public class AssetHelper : MonoBehaviour
{
    /// <summary>
    /// UnityEngine.LoadAssetsAtPath does not work as it should. Moreover, it's not generic. Therefore this method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<T> LoadAssetsAtPath<T>(string path) where T : UnityEngine.Object
    {
        path = HandleAssetPath(path);
        Debug.Log("[AssetHelper] Loading assets from " + path);
        var filePaths = Directory.GetFiles(Application.dataPath + path);
        var filteredPaths = filePaths.Where(p => !p.Contains(".meta"));
        List<T> assets = new List<T>();
        foreach (var p in filteredPaths)
        {
            var assetPath = FileManager.GetRightPartOfPath(p, "Assets", '/');
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        Debug.Log("[AssetHelper] Assets found: " + assets.Count);
        return assets;
    }

    private static string HandleAssetPath(string path)
    {
        if (path[0] != '/')
        {
            path = "/" + path;
        }
        var parts = path.Split('/').ToList();
        var firstPart = parts.First();
        if (firstPart.ToLowerInvariant().Equals("assets"))
        {
            parts.Remove(firstPart);
        }
        return string.Join("/", parts.ToArray());
    }
}
