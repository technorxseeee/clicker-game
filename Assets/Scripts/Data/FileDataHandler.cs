﻿using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        // Use Path#Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        GameData loadedData = null;
        
        if (File.Exists(fullPath))
        {
            try
            {
                // Load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                
                // deserialize the data from JSON back into C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("An error occurred:");
                Debug.LogError("Game failed when attempting to load data from file: " + fullPath);
                Debug.LogError("Error Code: DATA_RETRIEVAL_FAILURE");
                Debug.LogError("Stack trace:" + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        // Use Path#Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        
        try
        {
            // create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            
            // serialize the C# game data object into JSON
            string dataToStore = JsonUtility.ToJson(data, true);
            
            // write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("An error occurred:");
            Debug.LogError("Game failed when attempting to save data to file: " + fullPath);
            Debug.LogError("Error Code: DATA_SAVE_FAILURE");
            Debug.LogError("Stack trace:" + "\n" + e);
        }
    }
}