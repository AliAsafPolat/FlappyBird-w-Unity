using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Kuslarin kontrolu yapilir.
public class FB_script : MonoBehaviour
{
    public int birdIndex;
    private GameManager gameManager;    // GameManager ile beraber calismak zorundadir. Iterasyonlar GameManager uzerinden ilerler.
    public float velocity = 0.3f;       // Kusun havaya ziplama hizi ayarlanir.
    public int jumpIdx;                 // Verilen listede dolasmak icin kullanilir.
    public int jumpCount;               // Verilen listede kac kere 1 var. Kac kere ziplamis bilgisi alinir.
    public int score;                   // Her kusun ne kadar ileri gittigi skoru tutulur.
    private Rigidbody2D rb;             // Etrafla temasin algilanmasi icin rigidbody nesnesi kullanilir.
    public float autoControlConst;      // Senkronizasyon sabitidir.
    private ArrayList movement;         // Hareket dizisini tutar.

   
    void Start()
    {
        // Default atamalar yapilir.
        jumpIdx = 0;
        jumpCount = 0;
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Hareket dizisini alip oyunu baslatir.
    public void playGame(int[] res)
    {
        StartCoroutine(AutoControl(res));
    }

    // Kusun populasyondaki hangi kus oldugunu belirtmek icin index atamasi yapilir.
    public void setBirdIndex(int index)
    {
        this.birdIndex = index;
    }

    // Kus checkpointten(boru) gectiginde skoru bir artar.
    public void increaseScore()
    {
        this.score += 1;
    }

    // Kusun yukari sicramasi gerceklestirilir.
    public void Jump()
    {
        if(rb != null)
            rb.velocity = Vector2.up * velocity;
    }
    
    
    void Update()
    {
        // Mousea basildiginda sicrar.
        if (Input.GetMouseButtonDown(0))
        {
            // Jump
            Jump();
        }
        
    }

    // Kusun diger nesneler ile temaslari tespit edilir.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Eger kus bir nesneye carpmissa oyun biter.
        gameManager.kusDead(birdIndex, score, jumpCount);
        jumpIdx = 0;
        jumpCount = 0;
        // En iyi oyun gosteriliyorsa kus yok edilir.
        if (this.birdIndex == -1)
        {
            Destroy(gameObject);
        }
    }

    // Alinan hareket dizisi ile kusu 300 ms de bir hareketini saglar.
    IEnumerator AutoControl(int[] movement)
    {
        while(jumpIdx <= movement.Length-1)
        {
            // Eger sicrama gelmis ise ziplat. Degilse zaten yer cekiminden dolayi asagi cekiliyor.
            if (movement[jumpIdx] == 1)
            {
                Jump();
                jumpCount++;
            }

            jumpIdx++;
            yield return new WaitForSeconds(autoControlConst*Time.fixedDeltaTime);
        }
        
    }

}
