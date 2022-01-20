using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
   
    public Transform target;
    public Vector3 offset;
    public float lerpValue;


   
    void LateUpdate()
    {
        Vector3 desiredPos = offset + target.position;
        desiredPos.x = 0.47f;
        transform.position = Vector3.Lerp(transform.position, desiredPos, lerpValue);


    }
}
