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
    }

    public int timeToPair = 15;
    public float timeToPairRemaining = 15.0f;
    public GameObject prefabBall;

    public PlayerSpawnPoint[] playerData;


    private GameObject[] Balls = new GameObject[4];


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
