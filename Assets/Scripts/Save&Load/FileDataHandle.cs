using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class FileDataHandle 
{
    private string fileDirPath = "";
    private string fileName = "";

    public FileDataHandle(string _fileDirPath, string _fileName)
    {
        this.fileDirPath = _fileDirPath;
        this.fileName = _fileName;
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(fileDirPath,fileName);

        try {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(_data,true);

            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

        }
        catch(Exception e) {
            Debug.LogWarning("Error on trying save data to "+fullPath+ "\n"+ e);
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(fileDirPath,fileName);
        GameData loadData = null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath,FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }catch(Exception e)
            {
                Debug.LogWarning("Error on trying load data on " + fullPath + "\n" + e);

            }
        }

        return loadData;
    }
}
