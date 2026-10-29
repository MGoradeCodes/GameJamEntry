using System;
using Mono.Cecil.Cil;
using UnityEditor.Search;
using UnityEngine;

public class AnimateShard : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private double height;
    private float speed = 1.3f;

    [SerializeField] private float offsetY;

    [SerializeField] private float divideFactor;
    private float angle;
    private double theta;
    private Vector3 newPos;
    void Start()
    {
        height = 0;
    }
    void FixedUpdate()
    {
        angle += speed*Time.deltaTime;
        theta = angle;
        height = Math.Sin(theta) / divideFactor;
        newPos = transform.position;
        newPos.y = (float)height + offsetY;
        transform.position = newPos;

    }   
}
