using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    [SerializeField]
    private bool shakeCube;
    public CubeCollect cubeCollect;
    [SerializeField]
    GameObject[] stack;
    public Transform target;
    [SerializeField]
    private Camera cam;
    public float speed;
    public GameObject CubePrefab;



    private void Update()
    {
        stack = GameObject.FindGameObjectsWithTag("Stack");
    }

    public void  OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Toplanacak")
        {
            
            other.gameObject.tag = "Stack";
            KarakterKontrol.instance.Topla(other.gameObject);
            other.gameObject.AddComponent<Control>();
            other.gameObject.AddComponent<Rigidbody>();
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.GetComponent<Rigidbody>().useGravity = false;
            Destroy(this);


            
        }

        if (other.gameObject.tag == "Coklu")
        {
            Destroy(this);
            other.gameObject.tag = "Stack";
            List<GameObject> objeler = new List<GameObject>();
            for (int i = 0; i < other.gameObject.transform.childCount; i++)
            {
                objeler.Add(other.gameObject.transform.GetChild(i).gameObject);
                other.gameObject.transform.GetChild(i).gameObject.AddComponent<Control>();
                other.gameObject.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
                other.gameObject.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = true;
                other.gameObject.transform.GetChild(i).GetComponent<Rigidbody>().useGravity = false;

            }
          
            KarakterKontrol.instance.CokluTopla(objeler);

        }


        if (other.gameObject.tag=="Azalt")
        {
            StartCubeMove(stack[stack.Length - 1].gameObject.transform.position, () => { });

            StartCoroutine(waitCouple());

         // Kübü bıraktıktan sonra score a dogru ilerletme 

            //cubeCollect.StartCubeMove(transform.position,()=> {   });

           
            //Destroy(other.gameObject);

        }


        // 0.5 Saniye bekletip kübü bırakma aynı zamanda titremesini sağlama

        IEnumerator waitCouple()
    {

        /*    if (shakeCube)
            {
                ShakingCube(gameObject);
            }
       */ 

            print("0.5 saniye bekleyip küp titriyor");
            //0.5 saniye bekletip küpü fırlatıcaz
        yield return new WaitForSeconds(0.5f);

            other.gameObject.tag = "Untagged";

            List<GameObject> objeler = new List<GameObject>();
            for (int i = 0; i < other.gameObject.transform.childCount; i++)
            {
                objeler.Add(other.transform.GetChild(i).gameObject);
            }

            StartCoroutine(KarakterKontrol.instance.Azalt(objeler));

        }


        if (other.gameObject.tag == "Finish")
        {
            KarakterKontrol.instance.speed = 0;
            KarakterKontrol.instance.finishText.text = "You Won!";
        }


        Debug.Log("0.5 saniye geçti küp fırladı");
    }


    // Alttaki kübü 0.5 saniye titretme

    /* private void ShakingCube(GameObject object1)
     {
         object1.GetComponent<Shake>().ShakeCube();

     }
    */



    public void StartCubeMove(Vector3 _intial, Action onComplate)
    {
        //Vector3 intialpos = cam.ScreenToWorldPoint(new Vector3(intial.position.x, intial.position.y, cam.transform.position.z * -1));
        if(target!= null)
        {
            GameObject _cube = Instantiate(CubePrefab, transform);
            Vector3 targetPos = cam.ScreenToWorldPoint(new Vector3(target.position.x, target.position.y, cam.transform.position.z * -1));
            StartCoroutine(MoveCube(_cube.transform, _intial, targetPos, onComplate));
            
        }
        
        

       // GameObject _cube = stack[stack.Length - 1].gameObject;


        

        //StartCoroutine(MoveCube(_cube.transform, _intial, targetPos, onComplate));
    }


    IEnumerator MoveCube(Transform obj, Vector3 startPos, Vector3 endPos, Action onComplate)
    {
        float time = 0;

        while (time < 1)
        {
            time += speed * Time.deltaTime;
            obj.position = Vector3.Lerp(startPos, endPos, time);

            yield return new WaitForEndOfFrame();
        }

        onComplate.Invoke();
    }






}
