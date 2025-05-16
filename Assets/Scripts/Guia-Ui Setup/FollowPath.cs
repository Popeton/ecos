using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
   
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, endPoint.position, Time.deltaTime/10f);
    }
}
