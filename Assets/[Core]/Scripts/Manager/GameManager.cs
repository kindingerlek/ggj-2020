using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [System.Serializable]
    public class PlayerSpawnPoint
    {
        public Transform transform;
        public Color Color;
    }

    public int timeToPair = 15;
    public float timeToPairRemaining = 15.0f;

    public PlayerSpawnPoint[] SpawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()

    {
        UpdateTimeToPair();
    }




    void UpdateTimeToPair() {
        if (timeToPairRemaining > 0.0)
        {
            timeToPairRemaining -= Time.deltaTime;
        }else{
            this.TimeToPairEvent();
        }
    }
    void TimeToPairEvent() {
        Debug.Log("PAAAAIR TIME!");

        //Do wharever you need to do
        //Don't forget to reset the time to pair:
        //ResetTimeToPair();

        ResetTimeToPair();
    }
    void ResetTimeToPair()
    {
        timeToPairRemaining = timeToPair;
    }





}
