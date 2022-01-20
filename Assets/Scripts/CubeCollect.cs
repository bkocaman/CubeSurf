using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubeCollect : MonoBehaviour
{
    public float speed;

    public Transform target;
    //public Transform intial;
    public GameObject CubePrefab;
    public Camera cam;

    public void Start()
    {
        if(cam ==null)
        {
            cam = Camera.main;
        }
    }
    // Kübü býraktýktan sonra score a dogru ilerletme 
    public void StartCubeMove(Vector3 _intial, Action onComplate)
    {
        //Vector3 intialpos = cam.ScreenToWorldPoint(new Vector3(intial.position.x, intial.position.y, cam.transform.position.z * -1));
        Vector3 targetPos = cam.ScreenToWorldPoint(new Vector3(target.position.x, target.position.y, cam.transform.position.z * -1));
        GameObject _cube = Instantiate(CubePrefab, transform);

        StartCoroutine(MoveCube(_cube.transform, _intial, targetPos, onComplate));
    }


    IEnumerator MoveCube (Transform obj, Vector3 startPos, Vector3 endPos, Action onComplate)
    {
        float time = 0;

        while (time<1)
        {
            time += speed * Time.deltaTime;
            obj.position = Vector3.Lerp(startPos, endPos, time);

            yield return new WaitForEndOfFrame();
        }

        onComplate.Invoke();
    }


}

