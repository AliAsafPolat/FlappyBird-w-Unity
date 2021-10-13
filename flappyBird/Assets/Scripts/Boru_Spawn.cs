using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Oyuna belirli araliklarla borular olusturup yollar.
public class Boru_Spawn : MonoBehaviour
{
    public float maxTime;       // Borunun yollanma sikligi.
    private float timer = 500;  // Sayac
    public GameObject boru;     // Yollanacak boru nesnesi

    public bool state;          // Eger true ise boru yollamaya devam eder. False ise boru yollamaz.

    // Boru yollanma durumlarini kontrol eder.
    public void setState(bool val)
    {
        this.state = val;
    }
    // Default degerler atanir.
    void Start()
    {
        state = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (state)
        {
            if (timer > maxTime * Time.fixedDeltaTime)
            {
                GameObject yeniBoru = Instantiate(boru);
                yeniBoru.transform.position = transform.position;
                
                timer = 0;
                // Belirli bir sure gectikten sonra borulari yok eder.
                Destroy(yeniBoru, 20f);
            }
            timer += Time.fixedDeltaTime;
        }
        
    }
}
