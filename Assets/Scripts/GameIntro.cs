using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameIntro : MonoBehaviour
{
    public AudioSource music;



    public GameObject startScreen;
    static public bool gameStart = true;
    public GameObject playerCamera;

    public AudioSource menuSound;

    static public bool playerInput = false;
    private float startTime = 0; 
    private float scrollspeed = 0.4f;
    private float zoominT = 0.0f;
    CinemachineVirtualCamera Vcam;

    // Start is called before the first frame update
    void Start()
    {
        Vcam = playerCamera.GetComponent<CinemachineVirtualCamera>();
        startTime = Time.time;
        Vcam.m_Lens.OrthographicSize = 70;
        menuSound.Play();   
    }

    // Update is called once per frame
    void Update()
    {

        if (playerInput == false)
        {
            if (Time.time>(startTime+3)) //start panning after a few seconds
            {
                zoominT += scrollspeed * Time.deltaTime;
                Vcam.m_Lens.OrthographicSize = Mathf.Lerp(70, 7, zoominT); 

            }

            if (Input.GetMouseButtonDown(0))
            {
                startScreen.SetActive(false);
                scrollspeed = 5.0f;
                gameStart = false;
                music.time = 78.8f;
                menuSound.Stop();  
                music.Play(); 
            }

            //if (Mathf.Lerp(70, 7, zoominT) <= 7)
           // {
            //    gameStart = false;
            //}
        }

    }
}
