using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandle dataHandle;

    [SerializeField] private string fileName;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        dataHandle = new FileDataHandle(Application.persistentDataPath,fileName);
        saveManagers = FindAllSaveManager();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandle.Load();
        if(gameData == null)
        {
            Debug.LogWarning("game data not found ");
            NewGame();
        }

        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame() {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        dataHandle.Save(gameData);
        //Debug.Log("game was saved "+ gameData.currency);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManager()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List <ISaveManager>(saveManagers);
    }
}
