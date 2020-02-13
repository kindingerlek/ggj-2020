using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeToPairUi : MonoBehaviour
{
    Text counter;

    // Start is called before the first frame update
    void Start()
    {
        counter = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        counter.text = "time left: "+(int)GameManager.Instance.timeToPairRemaining;
    }
}
