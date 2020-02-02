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
        public int id;
    }

    public int timeToPair = 15;
    public float timeToPairRemaining = 15.0f;

    public PlayerSpawnPoint[] playerData;
    public List<PlayerSpawnPoint> team1;
    public List<PlayerSpawnPoint> team2;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");


        team1.Add(playerData[0]);
        team1.Add(playerData[1]);
        team2.Add(playerData[2]);
        team2.Add(playerData[3]);

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Players :"+playerData.Length);
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
        GetaRandomTeam();


        ResetTimeToPair();
    }


    void ResetTimeToPair()
    {
        timeToPairRemaining = timeToPair;
    }

    void GetaRandomTeam() {
        var coinFlip = Random.Range(0, 2);        
        PlayerSpawnPoint changedPLayer2 = team2[coinFlip];
        PlayerSpawnPoint changedPLayer1 = team1[1];
        team2.Remove(changedPLayer2);
        team1.Remove(changedPLayer1);
        team1.Add(changedPLayer2);
        team2.Add(changedPLayer1);
        Debug.Log("Random team cara:"+ coinFlip);

    }



}
