using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintViewController : MonoBehaviour
{
    [SerializeField] private float minDeletionDelay = 0.0f;
    [SerializeField] private float maxDeletionDelay = 0.05f;
    [SerializeField] private float minInsertionDelay = 0.01f;
    [SerializeField] private float maxInsertionDelay = 0.025f;

    [SerializeField] private TMP_Text label;
    [SerializeField] private float readTime = 2f;
    [SerializeField] private bool shuffle = true;

    public void OnEnable()
    {
        label.text = "";
        StartCoroutine(LoopHint());
    }

    public void OnDisable()
    {
        StopCoroutine(LoopHint());
    }

    public void OnDestroy()
    {
        StopCoroutine(LoopHint());
    }

    private IEnumerator LoopHint()
    {
        while(true)
        {
            var tempList = new List<string>(HintsDataModel.Instance.hintList);

            while (tempList.Count > 0)
            {
                int hintIndex = shuffle ? Random.Range(0, tempList.Count) : 0;
                var nextHint = tempList[hintIndex];
                tempList.RemoveAt(hintIndex);
                yield return StartCoroutine(NextHint(nextHint));


                yield return new WaitForSeconds(readTime);
            }
        }
    }

    private IEnumerator NextHint(string nextHint)
    {
        while(label.text.Length > 0)
        {
            label.text = label.text.Remove(label.text.Length - 1);
            yield return new WaitForSeconds(Random.Range(minDeletionDelay, maxDeletionDelay));
        }

        foreach(var character in nextHint)
        {
            label.text += character;
            yield return new WaitForSeconds(Random.Range(minInsertionDelay, maxInsertionDelay));
        }
    }
}
