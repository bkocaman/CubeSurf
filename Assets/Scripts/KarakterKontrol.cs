using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class KarakterKontrol : MonoBehaviour
{
    // Attributes

    public static KarakterKontrol instance;
    public Rigidbody rb;
    public float speed;
    public float lerpValue;
    public GameObject prev;
    public Material karakterMat;
    public List<GameObject> cubes = new List<GameObject>();
    public Text finishText;
    public Text scoreText;
    public CamControl camControl;
    public Transform target;
    private Camera cam;
    private int score = 0;
    private bool isGameEnded=false;
   



    private void Awake()
    {

        if (instance==null)
        {
            instance = this;
        }
    }

    void Start()
    {
        cam = Camera.main;

       
    }

    void FixedUpdate()
    {

        //cubes.Add(GameObject.FindGameObjectWithTag("Stack"));
            if (Input.GetButton("Fire1"))
            {

                Movement();
            }

            if (!isGameEnded)
            {

                rb.velocity = Vector3.forward * Time.deltaTime * speed;
            }


        }


    // Karakter Kontrolü
    
    private void Movement()
    {

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,1000))
        {
            Vector3 movementVec = new Vector3(hit.point.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, movementVec,lerpValue);
            
            
        }

    }

    // Tekli Toplama 

    public void Topla(GameObject gameObject)
    {
       
        Vector3 karakterPos = transform.localPosition;
        
        karakterPos.y += 0.44f;
         transform.position = Vector3.Lerp(transform.position, karakterPos, 0.095f);
         transform.position = karakterPos;

        gameObject.tag = "Stack";

        gameObject.transform.SetParent(transform);

        Vector3 pos = prev.transform.position;
        pos.y -= 1;
      
        camControl.offset.y += 0.1f;

        gameObject.GetComponent<MeshRenderer>().material = karakterMat;
        prev = gameObject;

        cubes.Add(gameObject);
        prev.GetComponent<BoxCollider>().isTrigger = false;
        Score += 1;



    }


     // İkili toplama
    public void CokluTopla(List<GameObject> gameObjects )
    {
        
        for (int i = 0; i < gameObjects.Count; i++)
        {
            GameObject gameObject = gameObjects[i];
            Vector3 karakterPos = transform.localPosition;
            karakterPos.y += 0.44f;
            transform.position = karakterPos;
            gameObject.tag = "Stack";


            gameObject.transform.SetParent(transform);

            Vector3 pos = prev.transform.localPosition;
            pos.y -= 1;
            gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition,pos,4);
            camControl.offset.y += 0.1f;
            gameObjects[i].gameObject.GetComponent<MeshRenderer>().material = karakterMat;
            cubes.Add(gameObject);

            prev = gameObject;

            prev.GetComponent<BoxCollider>().isTrigger = false;

            Score += 1;

            
           
        }
    }

    // Kübü bırakma 

    public IEnumerator Azalt(List<GameObject> gameObjects)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            transform.GetChild(transform.childCount - 1).SetParent(null);


            Vector3 karakterPos = transform.localPosition;
            karakterPos.y -= 0.44f;
            transform.transform.position = karakterPos;



            if (cubes.Count > 0)
            {

                cubes.RemoveAt(cubes.Count - 1);


            }

            // Skoru arttırma
            Score += 1;

  

            yield return new WaitForSeconds(0.5f);




        }

        if (cubes.Count > 0 && cubes.Count > gameObjects.Count - 1)
        {
            prev = cubes[cubes.Count - 1];
            cubes[cubes.Count - 1].AddComponent<Control>();


        }
        else
        {
            // Bırakacak küp kalmazsa oyunu bitirme
            speed = 0;
            finishText.text = "You Failed!";

        }
    }

    // Skor get/set
         public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            scoreText.text = score.ToString();

        }
    }

    // Kübü bıraktıktan sonra score a dogru ilerletme 
    public void StartCubeMove(Vector3 _intial, Action onComplate)
    {
        //Vector3 intialpos = cam.ScreenToWorldPoint(new Vector3(intial.position.x, intial.position.y, cam.transform.position.z * -1));


        Vector3 targetPos = cam.ScreenToWorldPoint(new Vector3(target.position.x, target.position.y, cam.transform.position.z * -1));
        //GameObject _cube = Instantiate(prev,transform);


        cubes.Add(gameObject);

       // StartCoroutine(MoveCube(stack[stack.Length].transform, _intial, targetPos, onComplate));
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



