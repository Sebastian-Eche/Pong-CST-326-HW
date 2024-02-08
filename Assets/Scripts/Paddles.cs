using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddles : MonoBehaviour
{

    public float paddleMoveSpeed = 5;

    public GameObject leftPaddle;

    public GameObject rightPaddle;

    private void FixedUpdate()
    {
        float leftPaddleInput = Input.GetAxis("LeftPaddle");

        float rightPaddleInput = Input.GetAxis("RightPaddle");

        Vector3 leftPaddleMovement = paddleMoveSpeed * Time.deltaTime * new Vector3(0,0, leftPaddleInput);

        leftPaddle.transform.Translate(leftPaddleMovement);

        Vector3 rightPaddleMovement = paddleMoveSpeed * Time.deltaTime * new Vector3(0,0, rightPaddleInput);

        rightPaddle.transform.Translate(rightPaddleMovement);

        //ADDING FORCES CODE
        // Vector3 leftPaddleMovement = Vector3.forward * leftPaddleInput * paddleMoveSpeed;

        // Rigidbody leftRB = leftPaddle.GetComponent<Rigidbody>();

        // leftRB.AddForce(leftPaddleMovement, ForceMode.Force);
    }
}
