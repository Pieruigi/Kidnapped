using EvolveGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Kidnapped.SaveSystem
{
    public class SaveManager : Singleton<SaveManager>
    {
        public static UnityAction OnGameSaved;

        static Dictionary<string, string> data = new Dictionary<string, string>();

        [SerializeField]
        List<GameObject> savables;

        string fileName = "save_v0_5.txt"; // Change it with the next update

        protected override void Awake()
        {
            base.Awake();

            CheckSaveVersion();

            SceneManager.sceneLoaded += HandleOnSceneLoaded;
        }

      

        private void Start()
        {
//#if UNITY_EDITOR
//            var list = new List<MonoBehaviour>(FindObjectsOfType<MonoBehaviour>(true)).Where(m=>m is ISavable);
//            savables = new List<GameObject>();
//            foreach (var l in list)
//                savables.Add(l.gameObject);


//            foreach (var l in list)
//            {
//                if (string.IsNullOrEmpty(l.GetComponent<ISavable>().GetCode()))
//                    Debug.LogError($"[SaveManager - ISavable with no code found: {l.name}");
//            }
//#endif
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.O))
            {
                SaveGame();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                GameManager.Instance.LoadSavedGame();
            }
#endif
        }


        private void HandleOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            savables.Clear();
            if (GameManager.Instance.IsGameScene())
            {
                var list = new List<MonoBehaviour>(FindObjectsOfType<MonoBehaviour>(true)).Where(m => m is ISavable);
                savables = new List<GameObject>();
                foreach (var l in list)
                    savables.Add(l.gameObject);
            }
        }


        void WriteToFile()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in data.Keys)
            {
                sb.AppendLine($"{key}:{data[key]}");
            }

            string filePath = System.IO.Path.Combine(Application.persistentDataPath, fileName);
            System.IO.File.WriteAllText(System.IO.Path.Combine(Application.persistentDataPath, fileName), sb.ToString());

           
        }

        void ReadFromFile()
        {
            string[] lines = System.IO.File.ReadAllLines(System.IO.Path.Combine(Application.persistentDataPath, fileName));
            foreach (string line in lines)
            {
                string[] s = line.Split(":");
                string key = s[0];
                string value = s[1];    
                if(!data.ContainsKey(key))
                    data.Add(key, "");
                data[key] = value;
            }
        }

        public void SaveGame()
        {
#if UNITY_EDITOR
            //return;
#endif
            foreach (var savable in savables)
            {
                if (savable == null)
                    continue;
                
                // Get the interface
                ISavable s = savable.GetComponent<ISavable>();
                // Get the key
                string code = s.GetCode();
                // Create a new element in the dictionary if it doesn't exists
                if (!data.ContainsKey(code))
                    data.Add(code, "");
                // Set value
                data[code] = s.GetData();
            }

            Debug.Log("saving file");
            WriteToFile();

            OnGameSaved?.Invoke();
        }

        public void LoadGame()
        {
            ReadFromFile();

            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


        }
            

        public bool SaveGameExists()
        {
            return System.IO.File.Exists(System.IO.Path.Combine(Application.persistentDataPath, fileName));
        }

        public void DeleteSaveGame()
        {
            if (SaveGameExists())
                System.IO.File.Delete(System.IO.Path.Combine(Application.persistentDataPath, fileName));
        }

        public void ClearCache()
        {
            data.Clear();
        }

        public void CheckSaveVersion()
        {
            var files = System.IO.Directory.GetFiles(Application.persistentDataPath);
            Debug.Log("Files.Length:" + files.Length);

            if (files == null || files.Length == 0)
                return; // No save game found

            int foundIndex = -1;
            for (int i = 0; i < files.Length && foundIndex < 0; i++)
            {
                Debug.Log("Check file:" + files[i].Substring(files[i].LastIndexOf("\\") + 1));
                if (files[i].Substring(files[i].LastIndexOf("\\")+1).StartsWith("save"))
                    foundIndex = i;
            }

            Debug.Log("Found index:" + foundIndex);

            if (foundIndex < 0)
                return;

            var foundName = files[foundIndex].Substring(files[foundIndex].LastIndexOf("\\") + 1);

            // Check version
            if (!fileName.Equals(foundName))
            {
                // Delete file
                System.IO.File.Delete(System.IO.Path.Combine(Application.persistentDataPath, foundName));
            }
            
        }

        #region utilities


        public static string GetCachedValue(string key)
        {
            if (!data.ContainsKey(key))
                return "";
            return data[key];
        }

        public static string ParseVector3ToString(Vector3 vector)
        {
            return $"{vector.x.ToString(CultureInfo.InvariantCulture)},{vector.y.ToString(CultureInfo.InvariantCulture)},{vector.z.ToString(CultureInfo.InvariantCulture)}";
        }

        public static Vector3 ParseStringToVector3(string data)
        {
            string[] s = data.Split(",");
            return new Vector3(float.Parse(s[0], CultureInfo.InvariantCulture), float.Parse(s[1], CultureInfo.InvariantCulture), float.Parse(s[2], CultureInfo.InvariantCulture));
        }

        public static string ParseQuaternionToString(Quaternion q)
        {
            return $"{q.x.ToString(CultureInfo.InvariantCulture)},{q.y.ToString(CultureInfo.InvariantCulture)},{q.z.ToString(CultureInfo.InvariantCulture)},{q.w.ToString(CultureInfo.InvariantCulture)}";
        }

        public static Quaternion ParseStringToQuaternion(string data)
        {
            string[] s = data.Split(",");
            return new Quaternion(float.Parse(s[0], CultureInfo.InvariantCulture), float.Parse(s[1], CultureInfo.InvariantCulture), float.Parse(s[2], CultureInfo.InvariantCulture), float.Parse(s[3], CultureInfo.InvariantCulture));
        }

        #endregion
    }


}

