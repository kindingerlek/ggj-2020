﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]


public class FollowPlayers : MonoBehaviour
{


    public List<Transform> targets;
    public Vector3 velocity;
    public Vector3 offSet;
    public float smoothTime = 0.5f;
    public float minZoom = 2f;
    public float maxZoom = 20f;
    public float zoomLimiter = 20f;
    private Camera cam;
    [SerializeField]
    private float camSize;
    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        UpdateList();
        //Move();
        Zoom();
    }

    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint(); ;

        Vector3 newPos = centerPoint + offSet;

        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);


    }
    private void UpdateList()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Grabbable");
        GameObject[] anchors = GameObject.FindGameObjectsWithTag("archor_point");

        this.targets = new List<Transform>();

        foreach (GameObject player in players)
        {
            this.targets.Add(player.transform);
        }

        foreach (GameObject obj in objects)
        {
            this.targets.Add(obj.transform);
        }

        foreach (GameObject obj in anchors)
        {
            this.targets.Add(obj.transform);
        }
    }

    private void Zoom()
    {
        Debug.Log("greaat distance:"+GetGreaatestDistance());
        float newZoom = Mathf.Lerp(minZoom, maxZoom, GetGreaatestDistance() / zoomLimiter);
        Debug.Log("new zoom"+newZoom);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }


    private Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;

        }


        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);

        }

        return bounds.center;
    }

    private float GetGreaatestDistance()
    {


        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);

        }
        Debug.Log("diagonal:" + Mathf.Sqrt((bounds.size.x * bounds.size.x) + (bounds.size.z * bounds.size.z)));
        Debug.Log("x:" + bounds.size.x);
        Debug.Log("y:" + bounds.size.z);
        return Mathf.Sqrt((bounds.size.x * bounds.size.x) + (bounds.size.z * bounds.size.z));
    }
}
