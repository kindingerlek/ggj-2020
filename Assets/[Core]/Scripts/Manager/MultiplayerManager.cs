using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class MultiplayerManager : MonoBehaviour
{
    //PlayerInputManager inputManager;

    private void OnPlayerJoined()
    {
        Debug.Log($"New Player Joined!");
    }

    private void OnPlayerLeft()
    {
        Debug.Log($"Player Left!");
    }
}
