using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Collection;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public int[,] combinations = new int[,]
    {
        { 0, 1, 2, 3 },
        { 0, 2, 3, 1 },
        { 0, 3, 1, 2 }
    };

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


    private GameObject[] Balls = new GameObject[2];

    public List<int> team1;
    public List<int> team2;


    private int r;
    internal int[] scores = new int[4];


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");

        r = Mathf.Clamp(Random.Range(0, 3), 0, 3);

        team1.Add(combinations[r,0]);
        team1.Add(combinations[r,1]);
        team2.Add(combinations[r,2]);
        team2.Add(combinations[r,3]);

        Balls[0] = Instantiate(prefabBall);
        Balls[1] = Instantiate(prefabBall);

        Balls[0].GetComponent<Grabbable>().playersIndexes = team1.ToArray();
        Balls[1].GetComponent<Grabbable>().playersIndexes = team2.ToArray();

        Balls[0].GetComponent<Grabbable>().Respawn();
        Balls[1].GetComponent<Grabbable>().Respawn();
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
        GetaRandomTeam();


        ResetTimeToPair();
    }


    void ResetTimeToPair()
    {
        timeToPairRemaining = timeToPair;
    }

    void GetaRandomTeam() {
        var aux = Mathf.Clamp(Random.Range(0, 3), 0,3) ;

        while(aux == r)
            aux = Mathf.Clamp(Random.Range(0, 3), 0, 3);

        r = aux;

        team1 = new List<int>();
        team2 = new List<int>();

        team1.Add(combinations[r, 0]);
        team1.Add(combinations[r, 1]);
        team2.Add(combinations[r, 2]);
        team2.Add(combinations[r, 3]);

        Balls[0].GetComponent<Grabbable>().playersIndexes = team1.ToArray();
        Balls[1].GetComponent<Grabbable>().playersIndexes = team2.ToArray();

        Balls[0].GetComponent<Grabbable>().Respawn();
        Balls[1].GetComponent<Grabbable>().Respawn();
    }



}
