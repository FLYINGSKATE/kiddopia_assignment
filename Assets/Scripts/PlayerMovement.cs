using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
   int splittedScreen = Screen.width / 3;
   Vector3 initialPosition,extremeRightPosition,extremeLeftPosition ;
   public float speed = 2.0f;
   public Text timeScoreText;
   public int timeScore;

   public AudioSource happySFX;
   public AudioSource sadSFX;

   public Text currentPlayerNameText;
   private Vector2 fingerDown;
   private Vector2 fingerUp;
   public bool detectSwipeOnlyAfterRelease = false;

   public float SWIPE_THRESHOLD = 20f;
   public int maxTimeSlider = 15;
   public int minTimeSlider = 0;

   public GameObject boostPanel,shieldPanel;


    void checkSwipe()
    {
        //Check if Vertical swipe
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            //Debug.Log("Vertical");
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                OnSwipeUp();
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
        }

        //Check if Horizontal swipe
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            //Debug.Log("Horizontal");
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
        }

        //No Movement at-all
        else
        {
            //Debug.Log("No Swipe!");
        }
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////
    void OnSwipeUp()
    {
        Debug.Log("Swipe UP");
    }

    void OnSwipeDown()
    {
        Debug.Log("Swipe Down");
    }

    void OnSwipeLeft()
    {
        Debug.Log("Swipe Left");
        float step = speed * Time.deltaTime;
        

        if(transform.position==extremeLeftPosition){
            //Already in extreme left
            return;
        }
        else if(transform.position==initialPosition){
            //In Middle then swipe left
            transform.position = Vector3.MoveTowards(transform.position, extremeLeftPosition, step);
        }
        else if(transform.position==extremeRightPosition){
            //In extreme right ..move to middle
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);
        }

        
        
    }

    void OnSwipeRight()
    {
        Debug.Log("Swipe Right");
        float step = speed * Time.deltaTime;
        

        if(transform.position==extremeRightPosition){
            //Already in extreme right
            return;
        }
        else if(transform.position==initialPosition){
            //In Middle then swipe right
            transform.position = Vector3.MoveTowards(transform.position, extremeRightPosition, step);
        }
        else if(transform.position==extremeLeftPosition){
            //In extreme left ..move to middle
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);
        }
    }

 
    void Start(){
        initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        extremeRightPosition =  new Vector3(2.0f,initialPosition.y,initialPosition.z);
        extremeLeftPosition = new Vector3(-1.88f,initialPosition.y,initialPosition.z);
        StartCoroutine("StartTimer");
        currentPlayerNameText.text = PlayerPrefs.GetString("CurrentPlayer");
    }

     // Update is called once per frame
     void Update ()
     {    
 
        //  if (Input.GetMouseButtonDown(0))
        //  {
        //      if (Input.mousePosition.x >= 2 * splittedScreen)
        //      {
        //         float step = speed * Time.deltaTime;
        //         transform.position = Vector3.MoveTowards(transform.position, new Vector3(2.0f,initialPosition.y,initialPosition.z), step);
        //         Debug.Log("Left");
        //      }
 
        //      if (Input.mousePosition.x >= splittedScreen && 
        //          Input.mousePosition.x < 2 * splittedScreen)
        //      {
        //         float step = speed * Time.deltaTime;
        //          transform.position = Vector3.MoveTowards(transform.position, new Vector3(initialPosition.x,initialPosition.y,initialPosition.z), step);
                
        //         Debug.Log("Middle");
        //      }
 
        //      if (Input.mousePosition.x < splittedScreen)
        //      {
        //         float step = speed * Time.deltaTime;
        //         transform.position = Vector3.MoveTowards(transform.position, new Vector3(-1.88f,initialPosition.y,initialPosition.z), step);
        //         Debug.Log("Right");
        //      }
        //  }   


         ////Swipe Code
         foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            //Detects Swipe while finger is still moving
            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeOnlyAfterRelease)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }

            //Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                checkSwipe();
            }
        }

     }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something Entered");
        if (other.CompareTag("Coin")){
            Debug.Log("Coin Found");
            happySFX.Play(0);
            Debug.Log("Bark");
            GameManager.AddCoin();
            Destroy(other.gameObject);
        }
        
        if (other.CompareTag("Enemy") && !GameManager.shieldPlayer){
            //Destroy(other.gameObject);
            
            sadSFX.Play(0);
            Destroy(this.gameObject);
            Debug.Log("Cried");
            GameManager.score = timeScore;
            GameManager.GameOver();
        }

        if (other.CompareTag("Enemy") && GameManager.shieldPlayer){
            Destroy(other.gameObject);
           
        }

        if (other.CompareTag("Shield")){
            Destroy(other.gameObject);
            StartCoroutine("EnableShield");
        }

        if (other.CompareTag("Boost")){
            Destroy(other.gameObject);
            StartCoroutine("BoostGame");
        }
    }


    IEnumerator StartTimer(){
        while(true){
            timeScore++;
            timeScoreText.text = timeScore.ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator EnableShield(){
        GameManager.shieldPlayer = true;
        shieldPanel.SetActive(true);
        yield return new WaitForSeconds(15f);
        shieldPanel.SetActive(false);
        GameManager.shieldPlayer = false;

    }

    IEnumerator BoostGame(){
        GameManager.BoostGame();
        boostPanel.SetActive(true);
        yield return new WaitForSeconds(15f);
        boostPanel.SetActive(false);
        GameManager.UnBoostGame();

    }




    


}
