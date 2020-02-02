using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.InputSystem;

public class MultiplayerManager : MonoBehaviour
{
    //PlayerInputManager inputManager;

    private void OnPlayerJoined(PlayerInput player)
    {
        var playerSpawn = GameManager.Instance.SpawnPoints;

        var mat = player.GetComponentInChildren<SkinnedMeshRenderer>().material;
        mat.SetColor("_BaseColor", playerSpawn[player.playerIndex].Color);

        player.gameObject.transform.position = playerSpawn[player.playerIndex].transform.position;

        Debug.Log($"New Player Joined!");
    }

    private void OnPlayerLeft()
    {
        Debug.Log($"Player Left!");
    }
}
