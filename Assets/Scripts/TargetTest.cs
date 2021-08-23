using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTest : MonoBehaviour
{
    private int _direction = 1;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 5) _direction = -1;
        if (transform.position.x < -5) _direction = 1;
        
        transform.position += _direction * Time.deltaTime * Vector3.right;
    }
}
