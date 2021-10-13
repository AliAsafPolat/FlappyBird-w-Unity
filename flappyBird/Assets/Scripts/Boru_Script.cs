using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boru_Script : MonoBehaviour
{
    public float speed;
    
    // Update is called once per frame
    void Update()
    {
        // Borulari spawn edip belli hizda sola dogru yollar.
        transform.position += Vector3.left * speed * Time.fixedDeltaTime;
    }
}
