using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    private const string PLAYER_ID_PREFIX = "Player ";

    public static void RegisterPlayer(string netId, GameObject player)
    {
        string playerID = PLAYER_ID_PREFIX + netId;
        players.Add(playerID, player);
        player.transform.name = playerID;
    }

    public static void UnRegisterPlayer(string playerID)
    {
        players.Remove(playerID);
    }

    public static GameObject GetPlayer(string playerID)
    {
        return players[playerID];
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200,200,200,500));
        GUILayout.BeginVertical();
        foreach(string playerID in players.Keys)
            GUILayout.Label(playerID + "  -  " + players[playerID].transform.name);
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
