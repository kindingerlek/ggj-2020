using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUIController : MonoBehaviour
{
    List<TextMeshProUGUI> labels = new List<TextMeshProUGUI>();


    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in this.transform)
        {
            labels.Add(child.GetComponent<TextMeshProUGUI>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < labels.Count; i++)
        {
            TextMeshProUGUI label = (TextMeshProUGUI) labels[i];
            label.text = GameManager.Instance.scores[i].ToString();
        }

    }
}
