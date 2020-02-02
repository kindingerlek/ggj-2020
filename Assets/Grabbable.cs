using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grabbable : MonoBehaviour
{
    struct playerAttach
    {
        public bool isGrabbing;
        public bool isAttached;
        public Joint joint;
        public PlayerInput player;
    }

    private playerAttach[] playerAttaches = new playerAttach[2];
    private int[] playersIndexes = { 0, 1 };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < playerAttaches.Length; i++)
        {
            playerAttach att = (playerAttach) playerAttaches[i];
            
            if(att.player != null && att.isGrabbing && !att.isAttached)
            {
                att.isAttached = true;
                att.joint = this.gameObject.AddComponent<FixedJoint>();
                att.joint.connectedBody = att.player.GetComponent<Rigidbody>();
                
                Debug.Log($"Attaching Joint to {att.player.transform.name}");

            }

            if (att.player == null && att.joint != null && att.isGrabbing == false)
            {
                Debug.Log("Destroing Joint");

                att.isAttached = false;
                Destroy(att.joint);
                att.joint = null;
            }

            playerAttaches[i] = att;
        }
        
    }

    public void TryGrab(PlayerInput player)
    {
        Debug.Log($"Player {player.playerIndex} try grab {this.name}");
        if (playerAttaches[0].player != null && playerAttaches[1].player != null)
            return;

        if (!playersIndexes.Contains(player.playerIndex))
            return;


        var id = playerAttaches[0].player == null ? playersIndexes[0] : playersIndexes[1];

        playerAttaches[id].isGrabbing = true;
        playerAttaches[id].player = player;
    }

    public void CancelGrab(PlayerInput player)
    {
        Debug.Log($"Player {player.playerIndex} release object {this.name}");

        if (playerAttaches[0].player == null && playerAttaches[1].player == null)
            return;

        if (!playersIndexes.Contains(player.playerIndex))
            return;


        var id = playerAttaches[0].player != null ? playersIndexes[0] : playersIndexes[1];

        playerAttaches[id].isAttached = false;
        playerAttaches[id].isGrabbing = false;
        playerAttaches[id].player = null;
    }

}
