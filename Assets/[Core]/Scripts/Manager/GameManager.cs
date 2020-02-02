using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Collection;

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
    public GameObject prefabBall;

    public PlayerSpawnPoint[] playerData;


    private GameObject[] Balls = new GameObject[4];

    public List<PlayerSpawnPoint> team1;
    public List<PlayerSpawnPoint> team2;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");

        int[] pairs = Enumerable.Range(0, 4).Shuffle().ToArray();

        for (int i = 0; i < Balls.Length; i++)
        {
            Balls[i] = Instantiate(prefabBall);
            Balls[i].transform.position = GetNewSpawnBallPosition();
            Balls[i].GetComponent<Grabbable>().playersIndexes = new int[] { i, (int) Mathf.Repeat(i+1,4) };

        }

        team1.Add(playerData[0]);
        team1.Add(playerData[1]);
        team2.Add(playerData[2]);
        team2.Add(playerData[3]);
    }

    public Vector3 GetNewSpawnBallPosition()
    {
        var randomPos = Random.insideUnitCircle * 9;

        if (randomPos.magnitude < 2f)
            randomPos = randomPos.normalized * 2f;

        return new Vector3(randomPos.x, 5f, randomPos.y);
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
