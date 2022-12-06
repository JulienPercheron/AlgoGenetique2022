using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    public GameObject carPrefab;

    List<GameObject> currentGeneration = new List<GameObject>();
    int currentGenerationAlive = 0;

    [SerializeField]
    int currentGenNum = 0;

    // Start is called before the first frame update
    void Start()
    {

        for(int i = 0; i < 50; i++)
        {
            GameObject car = Instantiate(carPrefab);

            car.transform.position = new Vector3(-12, 0.7f, 17.5f);
            //car.transform.rotation = Quaternion.Euler(0, 90, 0);
            car.GetComponent<CarScript>().InitializeRandomActions(60);
            car.GetComponent<CarScript>().InitializePos();
            currentGeneration.Add(car);
            currentGenerationAlive++;
        }

        Debug.Log("Launching generation number " + ++currentGenNum);
        LaunchCars();
    }


    void LaunchCars()
    {
        for(int i = 0; i < currentGeneration.Count; i++)
        {
            currentGeneration[i].GetComponent<CarScript>().isDead = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentGenerationAlive == 0)
        {
            

            Fitness();
            
            /*
            for(int i = 0; i < currentGeneration.Count; i++)
            {
                Debug.Log(i + " " + currentGeneration[i].GetComponent<CarScript>().distance);
            }
            */

            List<GameObject> newGen = Crossbreed();

            newGen = Mutate(newGen);

            for (int i = 0; i < currentGeneration.Count; i++)
            {
                Destroy(currentGeneration[i]);
            }

            currentGeneration = newGen;

            Debug.Log("Launching generation number " + ++currentGenNum);
            LaunchCars();
        }
    }



    public void IncrementDeathCounter()
    {
        currentGenerationAlive--;
    }

    //Sorting the currentGeneration list by distance travelled by the cars
    void Fitness()
    {
        currentGeneration.Sort(delegate (GameObject a, GameObject b)
        {
            return (b.GetComponent<CarScript>().distance).CompareTo(a.GetComponent<CarScript>().distance);
        });
    }


    //Creating a new generation based on the 5 best cars
    List<GameObject> Crossbreed()
    {
        List<GameObject> newGen = new List<GameObject>();

        for (int i = 0; i < 3; i++)
        {
            GameObject car = Instantiate(carPrefab);

            car.transform.position = new Vector3(-12, 1f, 17.5f);
            car.GetComponent<CarScript>().InitializePos();

            //For mutation of the last steps
            car.GetComponent<CarScript>().lastBestIndex = currentGeneration[i].GetComponent<CarScript>().GetActionIndex();

            car.GetComponent<CarScript>().actions = currentGeneration[i].GetComponent<CarScript>().actions;

            newGen.Add(car);
            currentGenerationAlive++;
        }

            for (int i = 0; i < 47; i++)
        {
            GameObject car = Instantiate(carPrefab);

            car.transform.position = new Vector3(-12, 1f, 17.5f);
            car.GetComponent<CarScript>().InitializePos();

            int index1 = 0, index2 = 0;

            while(index1 != index2)
            {
                index1 = Random.Range(0, 4);
                index2 = Random.Range(0, 4);
            }

            List<int> carActions = new List<int>(); 
            for(int j =0; j < currentGeneration[index1].GetComponent<CarScript>().actions.Count; j++)
            {
                if(Random.Range(1,3) == 1)
                {
                    carActions.Add(currentGeneration[index1].GetComponent<CarScript>().actions[j]);
                }
                else
                {
                    carActions.Add(currentGeneration[index2].GetComponent<CarScript>().actions[j]);
                }
            }

            //For mutation of the last steps
            if(currentGeneration[index1].GetComponent<CarScript>().GetActionIndex() > currentGeneration[index2].GetComponent<CarScript>().GetActionIndex())
            {
                car.GetComponent<CarScript>().lastBestIndex = currentGeneration[index1].GetComponent<CarScript>().GetActionIndex();
            }
            else
            {
                car.GetComponent<CarScript>().lastBestIndex = currentGeneration[index2].GetComponent<CarScript>().GetActionIndex();
            }
            
            car.GetComponent<CarScript>().actions = carActions;

            newGen.Add(car);
            currentGenerationAlive++;
        }

        return newGen;
    }


    List<GameObject> Mutate(List<GameObject> newGen)
    {

        for(int i = 3; i < newGen.Count; i++)
        {
            if(Random.Range(1,20) == 1)
            {
                if(Random.Range(1,currentGenNum) == 1)
                {
                    Debug.Log("mutation full random");
                    newGen[i].GetComponent<CarScript>().InitializeRandomActions(60);
                }
                else
                {
                    Debug.Log("mutation last steps");
                    for (int j = newGen[i].GetComponent<CarScript>().lastBestIndex; j > (newGen[i].GetComponent<CarScript>().lastBestIndex - 3); j--)
                    {
                        newGen[i].GetComponent<CarScript>().actions[j] = Random.Range(0, 5);
                    }
                }

                
            }
        }

        return newGen;
    }
}
