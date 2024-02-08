using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    private Quaternion rotation;
    private Vector3 bounceDirection;

    public TextMeshProUGUI leftScore;
    public TextMeshProUGUI rightScore;
    public GameObject winnerL;
    public GameObject winnerR;
    public GameObject isTheWinner;
    public AudioSource cheer;
    public AudioSource goal;
    public AudioSource backgroundMusic;

    private bool gameEnd = false;

    private int countL;
    private int countR;
    
    public float ballSpeed = 1;

    void Start()
    {
        countL = 0;
        countR = 0;
        int coinFlip = Random.Range(0,2);
        Rigidbody rb = GetComponent<Rigidbody>();

        Debug.Log($"FLIP {coinFlip}");

        isTheWinner.SetActive(false);
        winnerL.SetActive(false);
        winnerR.SetActive(false);

        //ZERO == Left
        //ONE == Right

        if(coinFlip != 0){
            rotation = Quaternion.Euler(-60f, -10f, 0f);
            bounceDirection = rotation * Vector3.right;
            rb.AddForce(bounceDirection * 800f, ForceMode.Force);
        }else{
           rotation = Quaternion.Euler(60f, -10f, 0f);
            bounceDirection = rotation * Vector3.left;
            rb.AddForce(bounceDirection * 800f, ForceMode.Force); 
        }
        
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log($"{other.gameObject.name} hit");
        Rigidbody rb = GetComponent<Rigidbody>();

        BoxCollider otherBC = other.gameObject.GetComponent<BoxCollider>();

        Bounds otherBounds = otherBC.bounds;
        float ballZ = transform.position.z;
        float othermaxZ = otherBounds.max.z;
        float otherminZ = otherBounds.min.z;

        if(other.gameObject.CompareTag("RightPaddle"))
        {//RIGHT PADDLE
            ballSpeed += 0.2f;
            GetComponent<AudioSource>().Play();
            if(ballZ <= othermaxZ && ballZ > 0){
                rotation = Quaternion.Euler(-60f, 12f, 0f);
                bounceDirection = rotation * Vector3.left;
                rb.AddForce(bounceDirection * 1100f * ballSpeed, ForceMode.Force);
            }else if(ballZ >= otherminZ && ballZ < 0){
                rotation = Quaternion.Euler(-60f, -12f, 0f);
                bounceDirection = rotation * Vector3.left;
                rb.AddForce(bounceDirection * 1100f * ballSpeed, ForceMode.Force);
            }else{

            }
        }

        if(other.gameObject.CompareTag("LeftPaddle"))
        {//LEFT PADDLE
            ballSpeed += 0.2f;
            GetComponent<AudioSource>().Play();
            if(ballZ <= othermaxZ && ballZ > 0){
                rotation = Quaternion.Euler(60f, 12f, 0f);
                bounceDirection = rotation * Vector3.right;
                rb.AddForce(bounceDirection * 1000f * ballSpeed, ForceMode.Force);
            }else if(ballZ >= otherminZ && ballZ < 0){
                rotation = Quaternion.Euler(60f, -12f, 0f);
                bounceDirection = rotation * Vector3.right;
                rb.AddForce(1000f * ballSpeed * bounceDirection, ForceMode.Force);
            }else{

            }
        }
        Debug.Log($"Z pos of ball is {ballZ}");
        Debug.Log($" MAX = {othermaxZ} MIN = {otherminZ}");
    }

    void OnTriggerEnter(Collider other)
    {
        ballSpeed = 1;
        Rigidbody rb = GetComponent<Rigidbody>();
        if(other.gameObject.name == "Left Trigger" && !gameEnd){
            ++countR;
            rightScore.text = countR.ToString();
            rb.velocity = Vector3.zero;
            transform.SetPositionAndRotation(new Vector3(Random.Range(-3f, 0f), 0f, Random.Range(-4f, 4f)), new Quaternion(-0f, 0f, 0f, 0f));
            
            rb.AddForce(500f * ballSpeed * Vector3.right, ForceMode.Force);
            Debug.Log($"point for right paddle");
        }else if(other.gameObject.name == "Right Trigger" && !gameEnd){
            ++countL;
            leftScore.text = countL.ToString();
            rb.velocity = Vector3.zero;
            transform.SetPositionAndRotation(new Vector3(Random.Range(0f, 3f), 0f,Random.Range(-4f, 4f)), new Quaternion(0f, 0f, 0f, 0f));
            rb.AddForce(500f * ballSpeed * Vector3.left, ForceMode.Force);
            Debug.Log($"point for left paddle");
        }
        goal.Play();
        WinnerWinnerChickenDinner();
    }

    void WinnerWinnerChickenDinner()
    {
        if(countL == 11){
            // winner.text = "Left";
            winnerL.SetActive(true);
            isTheWinner.SetActive(true);
            gameEnd = true;
            cheer.Play();
            backgroundMusic.Stop();
            Debug.Log("Left Wins");
        }else if(countR == 11){
            // winner.text = "Right";
            // rightScore.text = "0";
            winnerR.SetActive(true);
            isTheWinner.SetActive(true);
            gameEnd = true;
            cheer.Play();
            backgroundMusic.Stop();
            Debug.Log("Right Wins");
        }
    }

    void Update()
    {
        WinnerWinnerChickenDinner();
        if(gameEnd){
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.SetPositionAndRotation(new Vector3 (0f, 0f, 0f), new Quaternion(0f, 0f, 0f, 0f));
            countL = 0;
            countR = 0;
            leftScore.text = countL.ToString();
            rightScore.text = countR.ToString();
            gameEnd = false;
        }
    }

    void endGame()
    {
        if(gameEnd){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
