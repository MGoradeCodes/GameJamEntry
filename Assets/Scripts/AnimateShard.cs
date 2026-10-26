using System;
using Mono.Cecil.Cil;
using UnityEngine;

public class AnimateShard : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private double height;
    private float speed = 1.3f;
    private float angle;
    private double theta;
    private Vector3 newPos;
    void Start()
    {
        height = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        angle += speed*Time.deltaTime;
        theta = angle;
        height = Math.Sin(theta) / 2;
        newPos = transform.position;
        newPos.y = (float)height + 3;
        transform.position = newPos;

    }   
}
