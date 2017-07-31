using Helpers.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Helpers
{
    public static class FileManager
    {
        #region Main methods
        public static bool SaveAsText(string source, string path, string fileName, bool isPersistentPath, bool overWriteOldFile = false)
        {
            var paths = ParsePath(fileName, path, isPersistentPath);
            Debug.Log("[FileManager] Saving as text to " + paths.fullPath);
            if (File.Exists(paths.fullPath))
            {
                if (overWriteOldFile)
                {
                    Debug.Log("[FileManager] File already found, overwriting.");
                    File.WriteAllText(paths.fullPath, source);
                }
                else
                {
                    Debug.Log("[FileManager] File already found, appending.");
                    File.AppendAllText(paths.fullPath, source);
                }
                return true;
            }
            else
            {
                var dInfo = Directory.CreateDirectory(paths.pathWithoutFileName);
                if (dInfo.Exists)
                {
                    File.WriteAllText(paths.fullPath, source);
                    Debug.Log("[FileManager] File created at " + paths.fullPath);
                    return true;
                }
                else
                {
                    Debug.LogWarning("[FileManager] Cannot create directories for path: " + paths.pathWithoutFileName);
                    return false;
                }
            }
        }

        /// <summary>
        /// Creates a copy of the file and saves it into a file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public static bool Save<T>(T source, string path, string fileName, bool isPersistentPath, bool overWriteOldFile = false) where T : new()
        {
            var paths = ParsePath(fileName, path, isPersistentPath);
            Debug.Log("[FileManager] Saving as binary to " + paths.fullPath);
            if (!overWriteOldFile && File.Exists(paths.fullPath))
            {
                Debug.LogWarning("[FileManager] A file with the same path already exists. Overwriting is disabled.");
                return false;
            }
            else
            {
                var dInfo = Directory.CreateDirectory(paths.pathWithoutFileName);
                if (dInfo.Exists)
                {
                    var file = File.Create(paths.fullPath);
                    T copy = CreateCopyOf(source);
                    var bf = new BinaryFormatter();
                    try
                    {
                        bf.Serialize(file, copy);
                    }
                    catch (SerializationException e)
                    {
                        Debug.LogError("[FileManager] Cannot serialize: " + e);
                        return false;
                    }
                    file.Close();
                    Debug.Log("[FileManager] File created at " + paths.fullPath);
                    return true;
                }
                else
                {
                    Debug.LogError("[FileManager] Cannot create directories for path: " + paths.pathWithoutFileName);
                    return false;
                }
            }
        }

        public static T Load<T>(T classToLoadDataInto, string path, bool isPersistentPath) where T : new()
        {
            path = HandlePath(path, isPersistentPath);
            Debug.Log("[FileManager] loading from " + path);
            if (File.Exists(path))
            {
                try
                {
                    var bf = new BinaryFormatter();
                    var file = File.Open(path, FileMode.Open);
                    T data = (T)bf.Deserialize(file);
                    file.Close();
                    return data;
                }
                catch (SerializationException e)
                {
                    Debug.LogError("[FileManager] The file found but it is probably not of binary format! SerializationException was thrown: " + e);
                    return default(T);
                }
            }
            else
            {
                Debug.LogWarning("[FileManager] The file does not exist!");
                return default(T);
            }
        }

        public static string LoadText(string path, bool isPersistentPath)
        {
            path = HandlePath(path, isPersistentPath);
            Debug.Log("[FileManager] loading text file from " + path);
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            else
            {
                Debug.LogWarning("[FileManager] The file does not exist!");
                return string.Empty;
            }
        }

        public static bool DeleteFile(string path, bool isPersistentPath)
        {
            path = HandlePath(path, isPersistentPath);   
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.LogFormat("[FileManager] File found at {0} successfully deleted", path);
                return true;
            }
            else
            {
                Debug.LogWarning("[FileManager] The file does not exist!");
                return false;
            }
        }
        #endregion

        #region Public helpers
        /// <summary>
        /// Returns a new instance of the class with all public properties and fields copied.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T CreateCopyOf<T>(T source) where T : new()
        {
            return CopyValuesOf(source, new T());
        }

        /// <summary>
        /// Copies the values of the source to the destination. Both objects have to be of the same type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static T CopyValuesOf<T>(T source, T destination)
        {
            Type type = destination.GetType();
            if (type != source.GetType()) return default(T);
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            var properties = type.GetProperties(flags);
            foreach (var property in properties)
            {
                if (property.CanWrite)
                {
                    try
                    {
                        property.SetValue(destination, property.GetValue(source, null), null);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e);
                    }
                }
            }
            var fields = type.GetFields(flags);
            fields.ForEach(f => f.SetValue(destination, f.GetValue(source)));
            if (fields.Any(f => !f.GetValue(destination).Equals(f.GetValue(source))))
            {
                Debug.LogWarning("[FileManager] Failed to copy some of the fields.");
            }
            return destination;
        }

        public struct SeparatedPathPair
        {
            public string pathWithoutFileName;
            public string fullPath;
        }

        public static SeparatedPathPair SeparateFileNameFromPath(string fileName, string path, char separatorCharacter)
        {
            var paths = new SeparatedPathPair();
            if (string.IsNullOrEmpty(path))
            {
                paths.pathWithoutFileName = string.Empty;
                paths.fullPath = separatorCharacter + fileName;
            }
            else
            {
                paths.pathWithoutFileName = path;
                paths.fullPath = path;
                if (path.Contains(fileName) && !path[path.Length - 1].Equals(separatorCharacter))
                {
                    Debug.Log("[FileManager] Filtering the file name from the path");
                    var parsedPath = path.Split(separatorCharacter);
                    var ignorable = parsedPath.Last(p => p == fileName);
                    paths.pathWithoutFileName = string.Join(separatorCharacter.ToString(), parsedPath.Where(p => p != ignorable).ToArray());
                }
                else
                {
                    paths.fullPath = paths.pathWithoutFileName[paths.pathWithoutFileName.Length - 1].Equals(separatorCharacter) ? paths.pathWithoutFileName + fileName : paths.pathWithoutFileName + separatorCharacter + fileName;
                }
            }
            return paths;
        }

        public static string GetRightPartOfPath(string path, string after, char separatorCharacter)
        {
            var parts = path.Split(separatorCharacter);
            int afterIndex = Array.IndexOf(parts, after);
            if (afterIndex < 0)
            {
                throw new Exception(string.Format("[FileManager] Cannot find the right side of path {0} from {1}", path, after));
            }
            return string.Join(separatorCharacter.ToString(), parts, afterIndex, parts.Length - afterIndex);
        }
        #endregion

        #region Private methods
        private static string HandlePath(string path, bool isPersistentPath)
        {
            path = TrimPath(path);
            if (isPersistentPath)
            {
                return Path.Combine(Application.persistentDataPath, path);
            }
            else
            {
                return Application.dataPath + "/" + path;
            }
        }

        private static string TrimPath(string path)
        {
            return path.Replace("//", "/").TrimStart('/', ' ');
        }

        private static SeparatedPathPair ParsePath(string fileName, string path, bool isPersistentPath)
        {
            path = TrimPath(path);
            var paths = SeparateFileNameFromPath(fileName, path, '/');
            if (isPersistentPath)
            {
                paths.fullPath = Path.Combine(Application.persistentDataPath, paths.fullPath);
                paths.pathWithoutFileName = Path.Combine(Application.persistentDataPath, paths.pathWithoutFileName);
            }
            else
            {
                paths.fullPath = Application.dataPath + "/" + paths.fullPath;
                paths.pathWithoutFileName = Application.dataPath + "/" + paths.pathWithoutFileName;
            }
            return paths;
        }
        #endregion
    }
}
