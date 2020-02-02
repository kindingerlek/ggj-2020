using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.InputSystem;

public class MultiplayerManager : MonoBehaviour
{
    //PlayerInputManager inputManager;
    public List<PlayerInput> players;
    private void OnPlayerJoined(PlayerInput player)
    {
        var playerSpawn = GameManager.Instance.playerData;

        //var mat = player.GetComponentInChildren<SkinnedMeshRenderer>().material;
        //mat.SetColor("_BaseColor", playerSpawn[player.playerIndex].Color);

        var outline = player.GetComponent<Outline>().OutlineColor = playerSpawn[player.playerIndex].Color;


        var mat = player.GetComponentInChildren<SkinnedMeshRenderer>().material;
        mat.SetColor("_BaseColor", playerSpawn[player.playerIndex].Color);

        player.gameObject.transform.position = playerSpawn[player.playerIndex].transform.position;

        player.DeactivateInput();
        GameManager.Instance.playersInputs.Add(player);
        
        Debug.Log($"New Player Joined!");
    }

    private void OnPlayerLeft(PlayerInput player)
    {
        GameManager.Instance.playersInputs.Remove(player);

        Debug.Log($"Player Left!");

    }
}
