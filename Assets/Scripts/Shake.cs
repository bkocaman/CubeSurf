using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    private bool shaking = false;
    [SerializeField]
    private float shakeAmt;
  
    // Alttaki k?b? titretme
    
    void Update()
    {
        if(shaking)
        {
            Vector3 newPos = Random.insideUnitSphere * (Time.deltaTime * shakeAmt);
            newPos.y = transform.position.y;
            newPos.z = transform.position.z;

            transform.position = newPos;
        }
    }

    public void ShakeCube()
    {
        StartCoroutine("ShakeNow");
    }

    IEnumerator ShakeNow()
    {
        Vector3 originalPos = transform.position;
        if(shaking == false)
        {
            shaking = true;
        }
        yield return new WaitForSeconds(0.5f);

        shaking = false;
        transform.position = originalPos;
    }
}
