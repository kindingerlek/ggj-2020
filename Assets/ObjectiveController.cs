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


        Debug.Log("ÉEEEE GOOOOOOOLLL!!!!");        
        other.GetComponent<Grabbable>().Respawn();
    }
}
