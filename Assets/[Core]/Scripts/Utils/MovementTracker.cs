using System.Collections;
using Tools.DebugDraw;
using UnityEngine;

public class MovementTracker : MonoBehaviour
{
    [SerializeField]
    private float frequencyHz = 15;

    [SerializeField]
    private float lifeTime = 2f;

    [SerializeField]
    private Color Color = Color.black;

    private Vector3 _lastPosition;

    private void Start()
    {
        StartCoroutine(ShowTrace());
    }

    // Update is called once per frame
    IEnumerator ShowTrace()
    {
        while (true)
        {
            DebugDraw.Line(_lastPosition, transform.position, lifeTime: lifeTime).Color = Color;
            _lastPosition = transform.position;
            yield return new WaitForSeconds(1f/frequencyHz);
        }
    }
}
