using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
public class GameManager : MonoBehaviour
{
    public GameObject gameOverCanvas;       // Oyun bittikten sonra arayuzu gosterir
    public GameObject boruSpawn;            // Borulari yollayan nesnenin referansi tutulur.
    public GameObject FB_prefab;            // Olusturulacak kusun nesnesi tutulur.
    public List<GameObject> birds;          // Olusturulacak kuslarin nesnesi tutulur.
    private List<int> movement;             // Hareket dizisi tutulur.
    private float timeScale=2;              // Oyun default hiz bilgisi tutulur.

    // Default degerler atanir.
    void Start()
    {
        Time.timeScale = 0f;
        // Eger populasyon atamasi yapilmamis ise atamayi yap.
        if(GeneticAlgorithm.population == null)
            GeneticAlgorithm.assignMovements();

        // Eger en iyi oyun izlenmek isteniyor ise
        if (GeneticAlgorithm.bestPlayShow)
        {
            bestPlayDemo(GeneticAlgorithm.bestPlayArr);
        }
        else
        {
            // Jenerasyonu olusturup oynatmaya baslar.
            createGeneration();
        }
        
    }

    // Yeni jenerasyon olusturmak icin kullanilir.
    public void createGeneration()
    {
        // UI kaldir.
        gameOverCanvas.SetActive(false);
        // Zamani normale cek.
        Time.timeScale = timeScale;

        // Populasyondaki tum kromozomlar icin bir kus olusturulur ve takip edilir.
        for (int i = 0; i < GeneticAlgorithm.populationCount; i++)
        {
            GameObject bird = Instantiate(FB_prefab, transform);
            // Kusun indexini belirt.
            FB_script birdScrpt = bird.GetComponent<FB_script>();
            birdScrpt.setBirdIndex(i);
            // Populasyonda kalinan yerden devam et.
            List<int> mov = GeneticAlgorithm.population[i];
            // Otopilotun input formatina donustur ve gonder.
            int[] res = mov.OfType<int>().ToArray();
            birdScrpt.playGame(res);

            // Olusturulan kusun referansini ekle.
            birds.Add(bird);
        } 
    }

    // Kus oldugunde fitness degeri hesaplanir
    public void kusDead(int idx, int score, int jumpCount)
    {
        // Eger en iyi oyun gosterilmis ise
        if (GeneticAlgorithm.bestPlayShow)
        {
            // Tum borulari yok et.
            GameObject[] borular = GameObject.FindGameObjectsWithTag("boru");
            foreach(GameObject boru in borular)
            {
                Destroy(boru);
            }
            // Zamani durdur. UI goster. Degerleri default cek.
            Time.timeScale = 0;
            gameOverCanvas.SetActive(true);
            GeneticAlgorithm.bestPlayShow = false;
            boruSpawn.GetComponent<Boru_Spawn>().setState(false);
            return;
        }
        // Iyilik degerini hesapla.
        int fitness = GeneticAlgorithm.getFitnessValue(jumpCount, score);
        FitnessIdx ftn = new FitnessIdx(fitness, idx);
        // Butun iyilik degerlerinin tutuldugu yere ekle.
        GeneticAlgorithm.fitnessValues.Add(ftn);

        // Demek ki butun kuslar olmustur. Yeni jenerasyonun olusturulmasi gerekir.
        if(GeneticAlgorithm.fitnessValues.Count == GeneticAlgorithm.populationCount)
        {
            GeneticAlgorithm.iterationCount += 1;
            // Fitness degerlerini buyukten kucuge sirala.
            List<FitnessIdx> a = GeneticAlgorithm.fitnessValues;
            List<FitnessIdx> SortedList = a.OrderBy(o => -o.fitness).ToList();

            Debug.Log("Best score in iteration " + GeneticAlgorithm.iterationCount + " : " + SortedList[0].fitness.ToString());
            // Bir onceki iterasyonda maksimum degere sahip olan kromozomu ata.
            GeneticAlgorithm.iterMaxScore = SortedList[0].fitness;

            // Max iterasyona ulasilmis mi kontrolu yapilacak.
            if (GeneticAlgorithm.iterationCount >= GeneticAlgorithm.maxIteration)
            {
                // Max iterasyona ulastiysam oyun bitsin.
                GamePause();
                // En iyi oyun gosterilmek istenirse oyun ataniyor.
                movement = GeneticAlgorithm.population[SortedList[0].idx];
                GeneticAlgorithm.bestPlayArr = movement;
                boruSpawn.GetComponent<Boru_Spawn>().setState(false);
                return;
            }

            // Yeni jenerasyonu al.
            GeneticAlgorithm.createNewGeneration();
            // Yeni populasyon icin degerler sifirlanacak.
            GeneticAlgorithm.resetVariables();
            // Yeni jenerasyonu oynatmaya baslat.
            Replay();

        }
        // Yanan kusu ekrandan yok et.
        Destroy(birds[idx]);
        
    }

    // Iterasyonlar bittiginde son durumda en iyi oyunu gostermek icin kullanilir.
    public void showBestPlay()
    {
        GeneticAlgorithm.bestPlayShow = true;
        Replay();
    }

    // Oyun ekranini yeniden yukleyerek yeniden oynanmasini saglar.
    public void Replay()
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    // Oyunu durdurur.
    public void GamePause()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    // Oyunu devam ettirir.
    public void GameContinue()
    {
        gameOverCanvas.SetActive(false);
        Time.timeScale = timeScale;
    }

    // Oyunu baslatir.
    public void startGame()
    {
        GeneticAlgorithm.resetVariables();
        GeneticAlgorithm.iterMaxScore = 0;
        GeneticAlgorithm.iterationCount = 0;
        // UI kaldir.
        gameOverCanvas.SetActive(false);

        // Zamani normale cek.
        Time.timeScale = timeScale;
        // Ilk random degerleri olustur.
        GeneticAlgorithm.population = null;
        // Ekrani yeniden yukle.
        SceneManager.LoadScene("LoginScene");
        //Replay();
    }

    // En iyi olan kromozomu tekrardan oynatir.
    public void bestPlayDemo(List<int> movement)
    {
        // UI kaldir.
        gameOverCanvas.SetActive(false);
        // Zamani normale cek.
        Time.timeScale = timeScale;
        GameObject bird = Instantiate(FB_prefab, transform);
        // Kusun indexini belirt.
        FB_script birdScrpt = bird.GetComponent<FB_script>();
        birdScrpt.setBirdIndex(-1);
        int[] res = movement.OfType<int>().ToArray();
        birdScrpt.playGame(res);
    }

    public void onQuitClick()
    {
        startGame();
    }

    // Debug mesaji yazdirir.
    public static void debugMessage(string msg)
    {
        Debug.Log(msg);
    }
}
