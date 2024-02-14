using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    void Start()
    {
        transform.SetPositionAndRotation(new Vector3(Random.Range(-3,3), 0.5f, Random.Range(-3,3)), new Quaternion(0f,0f,0f,0f));
        // transform.gameObject.SetActive(false);
    }
    void Update()
    {
        transform.Rotate(new Vector3(15,20,45) * Time.deltaTime);
    }

}
