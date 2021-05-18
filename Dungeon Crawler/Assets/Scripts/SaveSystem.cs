using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
/// <summary>
/// Created using Brackeys 'Save & Load System in Unity' https://www.youtube.com/watch?v=XOjd_qU2Ido
/// </summary>
public class SaveSystem
{
    /// <summary>
    /// Saves the player hotbar.
    /// </summary>
    /// <param name="playerHotbar"></param>
    public static void SavePlayer(PlayerHotbar playerHotbar)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.hot";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(playerHotbar);

        binaryFormatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("here");
    }

    /// <summary>
    /// Saves playerStats to a binary file, player.stats.
    /// </summary>
    /// <param name="playerStats"></param>
    public static void SavePlayer(PlayerStats playerStats)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.stats";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(playerStats);

        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }
    /// <summary>
    /// Loads the players stats.
    /// </summary>
    /// <returns></returns>
    public static PlayerData LoadPlayerStats()
    {
        string path = Application.persistentDataPath + "/player.stats";
        Debug.Log(path);

        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

            PlayerData data = binaryFormatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            SavePlayer(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>());
            //Debug.LogError("Save file not found in: " + path);
            return null;
        }
    }

    /// <summary>
    /// Saves playerStats to a binary file, player.stats.
    /// </summary>
    /// <param name="playerStats"></param>
    public static void DeletePlayer()
    {
        string path = Application.persistentDataPath;
        string pathHotBar = path + "/player.hot";
        string pathStats = path + "/player.stats";
        if (File.Exists(pathHotBar))
        {
            File.Delete(pathHotBar);
        }
        if (File.Exists(pathStats))
        {
            File.Delete(pathStats);
        }

    }
    /// <summary>
    /// Loads the player hotbar.
    /// </summary>
    /// <returns></returns>
    public static PlayerData LoadPlayerHotbar()
    {
        string path = Application.persistentDataPath + "/player.hot";
        
        if (File.Exists(path))
        {
            Debug.Log("oops");
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

            PlayerData data = binaryFormatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }

        else
        {

            SavePlayer(GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerHotbar>());

            //Debug.LogError("Save file not found in: " + path);

            return null;
        }

    }
}
