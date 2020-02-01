using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cowboy : MonoBehaviour
{
    public static Cowboy instance;
    public float gameTimer = 60;

    

    private void Awake()
    {
        instance = this;    
    }

    
}
