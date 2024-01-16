using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.WSA;
using Application = UnityEngine.Application;

namespace GoodHub.Core.Runtime.Persistence
{
    public class PersistentData<T> where T : new()
    {
        private static T _persistentData;

        private static string FolderPath => $"{Application.persistentDataPath}{Path.DirectorySeparatorChar}PersistentData";

        private static string FileName => $"{typeof(T)}.json";

        private static string FilePath => $"{FolderPath}{Path.DirectorySeparatorChar}{FileName}";

        /// <summary>
        /// Get the currently loaded copy of the PersistentData object from memory.
        /// If none is loaded a new one is created, saved and returned instead.
        /// </summary>
        public static T Get()
        {
            if (_persistentData != null)
                return _persistentData;

            LoadOrCreateData();
            
            return _persistentData;
        }

        /// <summary>
        /// Save the copy of the PersistentData object from memory to file.
        /// </summary>
        public static bool Save()
        {
            try
            {
                string jsonText = JsonUtility.ToJson(_persistentData);

                if (Directory.Exists(FolderPath) == false)
                {
                    Directory.CreateDirectory(FolderPath);
                }

                File.WriteAllText(FilePath, jsonText);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private static void LoadOrCreateData()
        {
            if (File.Exists(FilePath) == false)
            {
                _persistentData = new T();
                Save();

                return;
            }

            try
            {
                string jsonText = File.ReadAllText(FilePath);
                _persistentData = JsonUtility.FromJson<T>(jsonText);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}