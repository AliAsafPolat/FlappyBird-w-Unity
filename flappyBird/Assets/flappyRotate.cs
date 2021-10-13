using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flappyRotate : MonoBehaviour
{

    private Vector3 rotationEuler;
    // Start is called before the first frame update
    void Start()
    {
        rotationEuler = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        rotationEuler += Vector3.forward * -30 * Time.deltaTime; //increment 30 degrees every second
        transform.rotation = Quaternion.Euler(rotationEuler);
    }
}
