using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class Controller : MonoBehaviour
{
    //public GameObject mouseTracker;
    public GameObject golfclub;
    public GameObject ball;
    public GameObject playerCamera;
    public AudioSource hitSound;

    static public bool gameOver = false;

    private float zoominT = 0.0f;
    private float zoomoutT = 0.0f;

    CinemachineVirtualCamera Vcam;

    [SerializeField] Animator mainAnimator;

    //[SerializeField, Range(0.01f, 20f)] float animSpeedControl = 1f;

    private bool canPutt = true;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "GameController")
        {
        golfclub.SetActive(false);
        hitSound.time = 2.22f;
        hitSound.Play();
        hitSound.SetScheduledEndTime(AudioSettings.dspTime + 0.5f);
        }

    }

    // Update is called once per frame
    void Update()
    {

        mainAnimator.SetFloat("speed", GetComponent<Rigidbody2D>().velocity.magnitude/5);  

        Rigidbody2D ballrb = GetComponent<Rigidbody2D>();

        Vector3 v3Velocity = (ballrb.velocity);

        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (GameIntro.gameStart == false)
        {
            if (v3Velocity.magnitude > 20) //allows player to use the club while ball is still moving a little
            {
                canPutt = false;
                zoomoutT = 0.0f;

                Vcam = playerCamera.GetComponent<CinemachineVirtualCamera>();
                zoominT += 0.01f * Time.deltaTime;
                Vcam.m_Lens.OrthographicSize = Mathf.Lerp(Vcam.m_Lens.OrthographicSize, 40, zoominT);
                

            }
            else
            {
                canPutt = true;
                zoominT = 0.0f;
                
                Vcam = playerCamera.GetComponent<CinemachineVirtualCamera>();
                zoomoutT += 0.001f * Time.deltaTime;
                Vcam.m_Lens.OrthographicSize = Mathf.Lerp(Vcam.m_Lens.OrthographicSize, 7, zoomoutT);
                
                
            }
        }
        //the golf club controlls
        if (Input.GetMouseButtonDown(0) && canPutt && gameOver != true && (Vector2.Distance(mouseWorldPos,ball.transform.position)>1.7f)) 
        {
            
            GameIntro.playerInput = true; //stops intro so it doesnt interfere with the zoom

            golfclub.SetActive(true);            

            Vector3 targetDirection = ball.transform.position - mouseWorldPos;

            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            golfclub.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            golfclub.transform.position = (new Vector2(mouseWorldPos.x, mouseWorldPos.y));

        }

        if (Input.GetMouseButton(0) && canPutt)
        {      

            TargetJoint2D clubTarget = golfclub.GetComponent<TargetJoint2D>();
            clubTarget.target = (new Vector2(mouseWorldPos.x, mouseWorldPos.y));

        }
        else
        {
            golfclub.SetActive(false);
        }

        //reset scene
        if (Input.GetKey(KeyCode.X))
        {
            if (gameOver == true)
            {
                GameIntro.gameStart = true;
                GameIntro.playerInput = false;
                gameOver = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene
            }
        }

        if (Input.GetKey(KeyCode.R) && gameOver != true) //respawn ball to home
        {
            ball.transform.position =  new Vector3(0,0,0);
            ball.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
        }

    }

}
