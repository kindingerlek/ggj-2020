using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoadingViewController : MonoBehaviour
{

    [SerializeField] private Slider progressBar;
    private void OnEnable()
    {
        progressBar.value = 0;
        //StartCoroutine(FakeLoadingProgress());
    }

    private IEnumerator FakeLoadingProgress()
    {
        int stops = Random.Range(8, 20);
        int steps = 0;
        while (progressBar.value < 1f)
        {
            var newProgress = Random.Range(.2f, 1f) * ((1f - progressBar.value) / (stops - steps++));
            progressBar.value += newProgress;
            yield return new WaitForSeconds(Random.value * 1f);
        }
        yield return null;
    }

    public void UpdateUI(float progress)
    {
        string stringProgress = string.Format("{0:0.0%}", progress);
        progressBar.value = progress;
    }
}
