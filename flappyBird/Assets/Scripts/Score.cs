using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI iterationText;
    
    
    void Update()
    {
        // Ekrandaki yazilari gunceller.
        scoreText.text = "IterMax : " + GeneticAlgorithm.iterMaxScore.ToString();
        iterationText.text = "Iteration : " + (GeneticAlgorithm.iterationCount+1).ToString();

    }
}
