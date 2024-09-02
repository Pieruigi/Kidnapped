using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kidnapped.SaveSystem
{
    public class SaveManager : Singleton<SaveManager>
    {

        static Dictionary<string, string> data = new Dictionary<string, string>();

        [SerializeField]
        List<GameObject> savables;

        string fileName = "save.txt";

        protected override void Awake()
        {
            base.Awake();

            InitSavables();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.O))
            {
                SaveGame();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
#endif
        }

        

        void LoadGame()
        {
            ReadFromFile();

            //InitSavables();
            
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //foreach (string key in data.Keys)
            //{
            //    Debug.Log($"{key}:{data[key]}");
            //}
        }

        void InitSavables()
        {
            foreach (var savable in savables)
            {
                ISavable s = savable.GetComponent<ISavable>();
                // Get the code
                string code = s.GetCode();
                // Look for the code in the dictionary
                if (data.ContainsKey(code))
                    s.Init(data[code]);
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
            //SaveScene();
            //SavePlayer();
            foreach (var savable in savables)
            {
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

            WriteToFile();
        }

        #region utilities
        public static string ParseVector3ToString(Vector3 vector)
        {
            return $"{vector.x},{vector.y},{vector.z}";
        }

        public static Vector3 ParseStringToVector3(string data)
        {
            string[] s = data.Split(",");
            return new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
        }

        public static string ParseQuaternionToString(Quaternion q)
        {
            return $"{q.x},{q.y},{q.z},{q.w}";
        }

        public static Quaternion ParseStringToQuaternion(string data)
        {
            string[] s = data.Split(",");
            return new Quaternion(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]), float.Parse(s[3]));
        }

        #endregion
    }


}

