using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSaveManager : MonoBehaviour
{
    


    static Dictionary<string, string> data = new Dictionary<string, string>();


    string fileName = "save.txt";

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadGame();
        }
#endif
    }

    void SaveGame()
    {
        SaveScene();
        SavePlayer();

        WriteToFile();
    }

    void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        foreach(string key in data.Keys)
        {
            Debug.Log($"{key}:{data[key]}");
        }
    }

    void CheckKey(string key)
    {
        if(!data.ContainsKey(key))
            data.Add(key, "");
    }

    void SavePlayer()
    {
        CheckKey("player");
        data["player"] = $"{PlayerController.Instance.HasFlashlight} {PlayerController.Instance.CanRunning} {PlayerController.Instance.CanCrouch} " +
                         $"{PlayerController.Instance.transform.position} {PlayerController.Instance.transform.rotation}";


    }

    void LoadPlayer()
    {
        
    }

    void SaveScene()
    {
        CheckKey("scene");
        data["scene"] = $"{SceneManager.GetActiveScene().buildIndex}";
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

    }
}
