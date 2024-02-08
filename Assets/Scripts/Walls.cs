using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{
    public float ballForce = 2;
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.name == "Ball"){
            if(other.transform.position.z > 5){
                Debug.Log($"hit {other.gameObject.name}");
                Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(Vector3.back * ballForce, ForceMode.Impulse);
            }else if(other.transform.position.z < 5){
                Debug.Log($"hit {other.gameObject.name}");
                Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(Vector3.forward * ballForce, ForceMode.Impulse);
            }
        }
    }
}
