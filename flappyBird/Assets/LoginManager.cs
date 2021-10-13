using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class LoginManager : MonoBehaviour
{

    public TMP_InputField populationField;
    public TMP_InputField iterationField;
    public TMP_InputField mutationField;
    public TMP_Text errorField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void onStartButtonClick()
    {
        try
        {
            errorField.gameObject.SetActive(false);
            int pop = int.Parse(populationField.text);
            int iter = int.Parse(iterationField.text);
            int mut = int.Parse(mutationField.text);

            GeneticAlgorithm.populationCount = pop;
            GeneticAlgorithm.maxIteration = iter;
            GeneticAlgorithm.mutationProbablity = mut;

            SceneManager.LoadScene("SampleScene");
        }
        catch(Exception e)
        {
            Debug.Log("Error" + e.ToString());
            errorField.gameObject.SetActive(true);
        }
        
    }

}
