using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int speed = 10;

    void Update()
    {
        transform.Rotate(0, 0, speed);
    }
}