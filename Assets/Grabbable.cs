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
        public float lastTouchTime;
        public Joint joint;
        public PlayerInput player;
    }

    public AudioSource destroy;

    private playerAttach[] playerAttaches = new playerAttach[2];
    public int[] playersIndexes = { 0, 3 };

    // Start is called before the first frame update
    void Start()
    {
        playerAttaches[0].lastTouchTime = float.MinValue;
        playerAttaches[1].lastTouchTime = float.MinValue;

        Respawn();
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
                this.transform.position += Vector3.up * 0.5f;                
                att.joint = this.gameObject.AddComponent<FixedJoint>();
                att.joint.connectedBody = att.player.GetComponent<Rigidbody>();
                //att.joint.anchor = att.player.transform.forward;
                
                Debug.Log($"Attaching Joint to {att.player.transform.name}");

            }

            if (att.player == null && att.joint != null && att.isGrabbing == false)
            {
                Debug.Log("Destroing Joint");

                att.isAttached = false;
                Destroy(att.joint);
                att.joint = null;
                att.lastTouchTime = Time.time;
            }

            playerAttaches[i] = att;
        }
        
    }

    public bool TryGrab(PlayerInput player)
    {
        //Debug.Log($"Player {player.playerIndex} try grab {this.name}");
        if (playerAttaches[0].player != null && playerAttaches[1].player != null)
            return false;

        if (!playersIndexes.Contains(player.playerIndex))
            return false;


        var id = playerAttaches[0].player == null ? 0 : 1;

        playerAttaches[id].isGrabbing = true;
        playerAttaches[id].player = player;
        return true;
    }

    public bool CancelGrab(PlayerInput player)
    {
        //Debug.Log($"Player {player.playerIndex} release object {this.name}");

        if (playerAttaches[0].player == null && playerAttaches[1].player == null)
            return false;

        if (!playersIndexes.Contains(player.playerIndex))
            return false;


        var id = playerAttaches[0].player != null ? 0 : 1;

        playerAttaches[id].isAttached = false;
        playerAttaches[id].isGrabbing = false;
        playerAttaches[id].player = null;
        return true;
    }

    public bool AllPlayersConnect
    {
        get => playerAttaches[0].player != null && playerAttaches[1].player != null; 
    }

    public void Respawn()
    {
        var randomPos = Random.insideUnitCircle * 9;

        if (randomPos.magnitude < 2f)
            randomPos = randomPos.normalized * 2f;

        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        var mat_1 = meshRenderer.materials[1];
        var mat_2 = meshRenderer.materials[2];

        mat_1.SetColor("_BaseColor", GameManager.Instance.playerData[playersIndexes[0]].Color);
        mat_2.SetColor("_BaseColor", GameManager.Instance.playerData[playersIndexes[1]].Color);

        Destroy(playerAttaches[0].joint);
        Destroy(playerAttaches[1].joint);

        playerAttaches[0].joint = null;
        playerAttaches[1].joint = null;

        playerAttaches[0].player = null;
        playerAttaches[1].player = null;


        transform.position = new Vector3(randomPos.x, 5f, randomPos.y);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        return ;
    }

    public bool FirstPlayerCarried
    {
        get => Time.time - playerAttaches[0].lastTouchTime <= 5f;
    }
    public bool SecondPlayerCarried
    {
        get => Time.time - playerAttaches[1].lastTouchTime <= 5f;
    }
}
