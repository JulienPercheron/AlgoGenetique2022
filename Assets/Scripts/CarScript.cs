using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    public float speed = 50.0f;
    public Vector3 rotation;
    public LayerMask layerMask;
    Rigidbody carRigidbody;

    private int index = 0;
    private float compteur = 0;

    public int lastBestIndex = 0;

    /*
    Liste des actions (representees par des int) que prendra la voiture dans l'ordre.
    
    0 -> avancer tout droit et vitesse normale
    1 -> avancer en tournant a gauche et vitesse normale
    2 -> avancer en tournant a droite et vitesse normale
    3 -> avancer en tournant a gauche et vitesse divisee par 2
    4 -> avancer en tournant a droite et vitesse divisee par 2

     */
    public List<int> actions = new List<int>();

    public bool isDead = true;
    public float distance = 0.0f;
    Vector3 previousPos;


    // Start is called before the first frame update
    void Start()
    {
        carRigidbody = this.GetComponent<Rigidbody>();

        Physics.IgnoreLayerCollision(7, 7);
    }


    public int GetActionIndex()
    {
        return index;
    }

    public void InitializeRandomActions(int count)
    {
        actions.Clear();
        for (int i = 0; i < count; i++)
        {
            actions.Add(Random.Range(0, 5));
        }
    }


    //Used to calc the distance travelled in a frame
    public void InitializePos()
    {
        previousPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            ExcecuteAction(actions[index]);

            compteur += Time.deltaTime;
            if (compteur >= 1)
            {
                index++;
                compteur = 0;
            }

            if (index >= actions.Count)
            {
                actions.Add(Random.Range(0, 5));
            }
        }
        
    }

    private void ExcecuteAction(int action)
    {
        switch (action)
        {
            //1 -> avancer en tournant a gauche et vitesse normale
            case 1:
                this.transform.Rotate(-rotation * Time.deltaTime);
                carRigidbody.velocity = transform.forward * speed;
                break;
            //2 -> avancer en tournant a droite et vitesse normale
            case 2:
                this.transform.Rotate(rotation * Time.deltaTime);
                carRigidbody.velocity = transform.forward * speed;
                break;
            //3 -> avancer en tournant a gauche et vitesse divisee par 2
            case 3:
                this.transform.Rotate(-rotation * Time.deltaTime);
                carRigidbody.velocity = transform.forward * speed * 0.5f;
                break;
            //4 -> avancer en tournant a droite et vitesse divisee par 2
            case 4:
                this.transform.Rotate(rotation * Time.deltaTime);
                carRigidbody.velocity = transform.forward * speed * 0.5f;
                break;
            //0 -> avancer tout droit et vitesse normale
            default:
                carRigidbody.velocity = transform.forward * speed;
                break;
        }

        distance += Vector3.Distance(previousPos, this.transform.position);
        previousPos = this.transform.position;

    }


    void NotifyDeath()
    {
        GameObject.FindObjectOfType<GeneticAlgorithm>().IncrementDeathCounter();
    }


    private void OnCollisionEnter(Collision collision)
    {
        //Test si la voiture collisionne avec un obstacle
        if (collision.gameObject.layer == 6 && !isDead)
        {
            this.isDead = true;
            NotifyDeath();
            //Debug.Log(distance);
        }

        if (collision.gameObject.tag == "Car")
        {
            Physics.IgnoreCollision(this.GetComponent<BoxCollider>(), collision.collider);
        }
    }

}
