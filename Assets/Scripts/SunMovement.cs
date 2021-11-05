using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMovement : MonoBehaviour
{
    private float TimeInDay = 60;

    void Start()
    {
        
    }

    void Update()
    {
        var elapsedInDay = Time.timeSinceLevelLoad % TimeInDay;
        var percentage = elapsedInDay / TimeInDay;

        var rotation = Quaternion.Euler(90+percentage*360,0,0);
        transform.rotation = rotation;
    }
}
