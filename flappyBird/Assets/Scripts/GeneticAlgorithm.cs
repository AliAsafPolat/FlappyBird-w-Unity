using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

class FitnessIdx
{
    public int fitness;
    public int idx;
    public FitnessIdx(int f, int i)
    {
        this.fitness = f;
        this.idx = i;
    }
}
class GeneticAlgorithm
{
    public static int iterMaxScore = 0;         // Bir onceki iterasyonda yapilan maksimum skoru getirir.
    public static int maxIteration = 50;       // Yapilacak olan maksimum iterasyonu getirir.
    public static int iterationCount = 0;       // Guncel iterasyon sayisini getirir.
    public static int populationCount = 100;     // Populasyon buyuklugunu saklar.
    public static int seedLength = 150;         // Kusun ziplama sayisi, bir oyun boyunca
    public static int mutationProbablity = 20;  // %20

    public static bool bestPlayShow = false;    // En iyi oyun gosterilme tusuna basildi ise oyun sergilenir.
    public static List<int> bestPlayArr;        // En iyi oyunun hareket dizisini tutar.
    public static Random rnd = new Random();    // Random generator.
    //public static List routes = new List();   // Kuslarin ziplama arrayleri (1-0)
    public static List<FitnessIdx> fitnessValues = new List<FitnessIdx>(); // Rotalarin fitness degerleri
    public static List<List<int>> population;   // Populasyon degiskeni


    // Verilen rotanin iyilik degerini hesaplar.
    public static int getFitnessValue(int jumpCount, int score)
    {
        // Ne kadar fazla ziplama yapilmis ise o kadar ile gidilmistir. Ne kadar boru gecilmis ise o kadar iyidir.
        // Borulardan gecilme sayisi ziplamadan cok daha onemlidir.
        int fitness = jumpCount + 20 * score;
        return fitness;
    }

    // Ilk populasyonun hepsi oynatildiktan sonra yeni populasyon icin degiskenler resetlenir.
    public static void resetVariables()
    {
        fitnessValues = new List<FitnessIdx>();
    }

    // Ilk kez populasyon olusturmak icin kullanilir. Rastgele degerler ile populasyon olusturur.
    public static void assignMovements()
    {
        population = new List<List<int>>();

        for (int j = 0; j < populationCount; j++)
        {
            List<int> movement = new List<int>();
            for (int i = 0; i < seedLength; i++)
            {
                int rand = rnd.Next(0, 100);
                // Ziplama hareketi onemli oldugundan %60 ziplasin dedim.
                if (rand > 40)
                    movement.Add(1);
                else
                    movement.Add(0);
            }
            population.Add(movement);
        }
    }

    // Yeni jenerasyon oluşturmak için kullanılır.
    public static void createNewGeneration()
    {
        List<List<int>> newPopulation = new List<List<int>>();
        // Fitness degerlerine gore hesaplamalar yapilir. Rulet teker yontemi kullanilmaktadir.
        List<FitnessIdx> a = fitnessValues;
        List<FitnessIdx> SortedList = a.OrderBy(o => -o.fitness).ToList();
        for (int i = 0; i<populationCount; i++)
        {
            // Iki tane parent sec.
            int selectedParentX = selectParent(SortedList);
            int selectedParentY = selectParent(SortedList);
            // Secilen parentlardan cross over olustur.
            List<int> child = applyCrossOver(selectedParentX, selectedParentY);
            // Mutasyon ihtimalini uygula.
            applyMutation(child);
            // Yeni jenerasyona ekle.
            newPopulation.Add(child);
        }
        population = newPopulation;
    }

    // Verilen listeden ihtimallere gore parent secer. Rulet teker yontemi kullanir.
    public static int selectParent(List<FitnessIdx> fitnessVals_)
    {
        List<FitnessIdx> fitnessVals = new List<FitnessIdx>(fitnessVals_);
        var converted = new List<FitnessIdx>(fitnessVals.Count);
        var sum = 0;

        // Rulet teker secimi icin olasiliklar dagitilir.
        int sumFit = fitnessVals.Sum(o => o.fitness);
        foreach (var item in fitnessVals.Take(fitnessVals.Count - 1))
        {
            sum += item.fitness;
            converted.Add(new FitnessIdx(sum,item.idx));
        }
        converted.Add(new FitnessIdx(sumFit,fitnessVals.Last().idx));
        
        // Olusturulan ihtimaller dizisinden rastgele deger al.
        int probability = rnd.Next(0,sumFit);
        // Bulunan degeri sec.
        var selected = converted.SkipWhile(i => i.fitness < probability).First();
        // Secilen kromozomun indisini dondur.
        return selected.idx;
    }
    
    // Verilen listeye mutasyon olasiligina gore mutasyon uygular ve dondurur.
    public static void applyMutation(List<int> kromozom)
    {
        for (int i = 0; i<kromozom.Count; i++)
        {
            int rand = rnd.Next(0, 100);
            if(rand < mutationProbablity)
            {
                kromozom[i] = (kromozom[i] + 1) % 2; // 0 ise 1, 1 ise 0 yap.
            }
        }
    }

    // Alinan iki parenti cross over islemi yapar.
    public static List<int> applyCrossOver(int p1, int p2)
    {
        List<int> p1_kromozom = new List<int>(population[p1]);
        List<int> p2_kromozom = new List<int>(population[p2]);
        Random rnd = new Random();
        int randVal = rnd.Next(0,p1_kromozom.Count);

        for (int i = 0; i < randVal; i++)
        {
            int tmp = p1_kromozom[i];
            p1_kromozom[i] = p2_kromozom[i];
            p2_kromozom[i] = tmp;
        }
        // Parentlardan birisini rastgele olarak dondurur.
        if (randVal % 2 == 0)        
            return p1_kromozom;
        else
            return p2_kromozom;

    }
    
}

