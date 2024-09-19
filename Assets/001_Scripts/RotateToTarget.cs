using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    Transform target;

    void Start()
    {
        GameObject targetGo = GameObject.FindGameObjectWithTag("Player");
        if (targetGo != null)
        {
            target = targetGo.transform;
        }
    }

    
    void Update()
    {
        if(target != null)
        {
            Vector2 dir = target.position - transform.position;
            float angleZ = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, -angleZ);
        }
    }
}
