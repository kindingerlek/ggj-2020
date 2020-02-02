using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestController : MonoBehaviour
{
    public float speed = 12;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMove(InputValue value)
    {
        var v = value.Get<Vector2>();
        transform.position += new Vector3(v.x, 0, v.y) * speed * Time.deltaTime;
    }
}
