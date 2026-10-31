using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class CollapseState : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool isDestroy;

    public static CollapseState instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void SetState(bool val)
    {
        isDestroy = val;
    }

    public bool getState()
    {
        return isDestroy;
    }
}
