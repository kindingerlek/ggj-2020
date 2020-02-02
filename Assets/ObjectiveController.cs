using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveController : MonoBehaviour
{
    public new Collider collider;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Grabbable")
            return;

        var grabblable = other.GetComponent<Grabbable>();

        int points = grabblable.FirstPlayerCarried && grabblable.SecondPlayerCarried ? 3 : 1;

        if(grabblable.FirstPlayerCarried)
            GameManager.Instance.scores[grabblable.playersIndexes[0]] += points;
        
        if(grabblable.SecondPlayerCarried)
            GameManager.Instance.scores[grabblable.playersIndexes[1]] += points;

        Debug.Log("ÉEEEE GOOOOOOOLLL!!!!");        
        other.GetComponent<Grabbable>().Respawn();
    }
}
