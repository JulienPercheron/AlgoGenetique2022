using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    public GameObject carPrefab;

    List<GameObject> currentGeneration = new List<GameObject>();
    int currentGenerationAlive = 0;

    // Start is called before the first frame update
    void Start()
    {

        for(int i = 0; i < 20; i++)
        {
            GameObject car = Instantiate(carPrefab);

            car.transform.position = new Vector3(-12, 0.5f, 17.5f);
            //car.transform.rotation = Quaternion.Euler(0, 90, 0);
            car.GetComponent<CarScript>().InitializeRandomActions(60);
            car.GetComponent<CarScript>().InitializePos();
            currentGeneration.Add(car);
            currentGenerationAlive++;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentGenerationAlive == 0)
        {
            Debug.Log("everybody died");
        }
    }



    public void IncrementDeathCounter()
    {
        currentGenerationAlive--;
    }


    List<GameObject> Crossbreed(List<GameObject> currentGenereration)
    {


        return null;
    }
}
