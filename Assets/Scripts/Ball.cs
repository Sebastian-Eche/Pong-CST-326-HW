using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    private Quaternion rotation;
    private Vector3 bounceDirection;

    public TextMeshProUGUI leftScore;
    public TextMeshProUGUI rightScore;
    public TextMeshProUGUI isOnFire;
    public GameObject winnerL;
    public GameObject winnerR;
    public GameObject isTheWinner;
    public GameObject BallPowerUp;
    public GameObject InvertPowerUp;
    public AudioSource cheer;
    public AudioSource goal;
    public AudioSource backgroundMusic;
    public AudioSource pickUp;
    public AudioSource otherCollisions;
    public AudioSource lowThwack;

    private bool gameEnd = false;

    private int countL;
    private int countR;
    
    public float ballSpeed = 1;
    float threeInARowL = 0;
    float threeInARowR = 0;

    void Start()
    {
        countL = 0;
        countR = 0;
        int coinFlip = UnityEngine.Random.Range(0,2);
        Rigidbody rb = GetComponent<Rigidbody>();

        Debug.Log($"FLIP {coinFlip}");

        isTheWinner.SetActive(false);
        winnerL.SetActive(false);
        winnerR.SetActive(false);
        BallPowerUp.SetActive(false);
        InvertPowerUp.SetActive(false);

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

        float height;
        float remap;

        if(other.gameObject.CompareTag("RightPaddle")){//RIGHT PADDLE
            ballSpeed += 0.2f;
            height = (ballZ - otherminZ)/(othermaxZ- otherminZ);
            remap = (height - 0.5f) / 0.5f;
            if(remap > 0){
                GetComponent<AudioSource>().Play();
            }else{
                lowThwack.Play();
            }
            // Debug.Log($"Z pos of ball is {ballZ}");
            // Debug.Log($" MAX = {othermaxZ} MIN = {otherminZ}");
            // if(ballZ <= othermaxZ && ballZ > 0){
                rotation = Quaternion.Euler(0f, 10f * remap, 0f);
                bounceDirection = rotation * Vector3.left;
                rb.AddForce(bounceDirection * 1100f * ballSpeed, ForceMode.Force);
            // }else if(ballZ >= otherminZ && ballZ < 0){
            //     lowThwack.Play();
            //     rotation = Quaternion.Euler(0f, 10f * remap, 0f);
            //     bounceDirection = rotation * Vector3.left;
            //     rb.AddForce(bounceDirection * 1100f * ballSpeed, ForceMode.Force);
            // }
        }else if(other.gameObject.CompareTag("LeftPaddle")){//LEFT PADDLE
            ballSpeed += 0.2f;
            height = (ballZ - otherminZ)/(othermaxZ- otherminZ);
            remap = (height/2) + 0.5f;
            Debug.Log($"Z pos of ball is {ballZ}");
            Debug.Log($" MAX = {othermaxZ} MIN = {otherminZ}");
            if(remap >= 0.6){
                GetComponent<AudioSource>().Play();
                rotation = Quaternion.Euler(0f, -10f * remap, 0f);
            }else if (remap < 0.6){
                // remap = -remap;
                Debug.Log($"Remapped: {remap}");
                rotation = Quaternion.Euler(0f, 10f, 0f);
                lowThwack.Play();
            }
            // if(ballZ <= othermaxZ && ballZ > 0){
                GetComponent<AudioSource>().Play();

                bounceDirection = rotation * Vector3.right;
                rb.AddForce(bounceDirection * 1000f * ballSpeed, ForceMode.Force);
            // }else if(ballZ >= otherminZ && ballZ < 0){
                // lowThwack.Play();
                // rotation = Quaternion.Euler(0f, 10f, 0f);
                // bounceDirection = rotation * Vector3.right;
                // rb.AddForce(1000f * ballSpeed * bounceDirection, ForceMode.Force);
        }else{
            otherCollisions.Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if(countL % 2 == 1 || countR % 2 == 1){
            BallPowerUp.SetActive(true);
        }else{
            InvertPowerUp.SetActive(true);
        }

        if(other.gameObject.name == "BallSize"){
            transform.localScale = new Vector3(1.5f,1.5f,1.5f);
            pickUp.Play();
            BallPowerUp.SetActive(false);
        }

        if(other.gameObject.name == "InvertDirection"){
            rb.velocity = Quaternion.Euler(0f, -10f, 0f) * -rb.velocity;
            pickUp.Play();
            InvertPowerUp.SetActive(false);
        }



        
            

        ballSpeed = 1;
        if(other.gameObject.name == "Left Trigger" && !gameEnd){
            transform.localScale = new Vector3(1f,1f,1f);
            ++countR;
            threeInARowL = 0;
            threeInARowR++;
            rightScore.text = countR.ToString();
            rightScore.fontSize = 30;
            leftScore.fontSize = 24;
            rb.velocity = Vector3.zero;
            transform.SetPositionAndRotation(new Vector3(UnityEngine.Random.Range(-3f, 0f), 0f, UnityEngine.Random.Range(-4f, 4f)), new Quaternion(-0f, 0f, 0f, 0f));
            rb.AddForce(500f * ballSpeed * Vector3.right, ForceMode.Force);
            Debug.Log($"point for right paddle");
            if(threeInARowR == 3){
                isOnFire.text = "RIGHT IS ON FIRE";
                isOnFire.color = Color.red;
            }else{
                isOnFire.text = "";
            }
            goal.Play();
        }else if(other.gameObject.name == "Right Trigger" && !gameEnd){
            transform.localScale = new Vector3(1f,1f,1f);
            ++countL;
            threeInARowL++;
            threeInARowR = 0;
            leftScore.text = countL.ToString();
            leftScore.fontSize = 30;
            rightScore.fontSize = 24;
            rb.velocity = Vector3.zero;
            transform.SetPositionAndRotation(new Vector3(UnityEngine.Random.Range(0f, 3f), 0f,UnityEngine.Random.Range(-4f, 4f)), new Quaternion(0f, 0f, 0f, 0f));
            rb.AddForce(500f * ballSpeed * Vector3.left, ForceMode.Force);
            Debug.Log($"point for left paddle");
            if(threeInARowL == 3){
                isOnFire.text = "LEFT IS ON FIRE";
                isOnFire.color = Color.red;
            }else{
                isOnFire.text = "";
            }
            goal.Play();
        }
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
            transform.SetPositionAndRotation(new Vector3 (0f, 0.5f, 0f), new Quaternion(0f, 0f, 0f, 0f));
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
