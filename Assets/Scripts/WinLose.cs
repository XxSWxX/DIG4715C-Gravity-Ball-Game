using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WinLose : MonoBehaviour
{
    
    public GameObject winScreen;
    public GameObject loseScreen;
    
    private float timer = 0f;

    public GameObject timeUI;
    public TMP_Text timerTEXT;
    public AudioSource winSound;
    public AudioSource looseSound;

    // Start is called before the first frame update
    void Start()
    {
        timerTEXT = timeUI.GetComponent<TextMeshProUGUI>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Finish")
        {
            winSound.Play(); 
            winScreen.SetActive(true);
            Debug.Log("WINNER");
            Controller.gameOver = true; 
        }

        if (collision.gameObject.tag == "Respawn")
        {
            Invoke("FailFunction", 2);
            //slight delay before starting fail sequence
        }
    }

    void FailFunction()
    {
        looseSound.Play(); 
        loseScreen.SetActive(true);
        Debug.Log("FAILED");
        Controller.gameOver = true;  
    }

    void Update() {

        if (GameIntro.gameStart == false) //if start sequence is over
        {
            timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);

            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

            //GUI.Label(new Rect(10,10,250,100), niceTime);
            timerTEXT.text=(niceTime);
        }
    }

}
