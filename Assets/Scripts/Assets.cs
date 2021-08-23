using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assets : MonoBehaviour
{
    public static Assets Instance;
    public GameObject Bullet;
    public GameObject HomingBullet;
    public GameObject Ray;
    public GameObject Target;
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
