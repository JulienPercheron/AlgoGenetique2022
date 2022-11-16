using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    public float speed = 5.0f;
    public Vector3 rotation;
    public LayerMask layerMask;
    Rigidbody carRigidbody;


    // Start is called before the first frame update
    void Start()
    {
        carRigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        Ray ray1 = new Ray(this.transform.position, this.transform.forward + this.transform.right);
        Ray ray2 = new Ray(this.transform.position, this.transform.forward);
        Ray ray3 = new Ray(this.transform.position, this.transform.forward - this.transform.right);
        
        RaycastHit hit1;
        RaycastHit hit2;
        RaycastHit hit3;


        Debug.DrawLine(this.transform.position, this.transform.position + 5 * (this.transform.forward + this.transform.right), Color.red);
        Debug.DrawLine(this.transform.position, this.transform.position + 10 * (this.transform.forward), Color.red);
        Debug.DrawLine(this.transform.position, this.transform.position + 5 * (this.transform.forward - this.transform.right), Color.red);


        if (Physics.Raycast(ray1, out hit1, 5, layerMask))
        {
            //Debug.Log(hit1.transform.gameObject.name);
            this.transform.Rotate(-rotation * Time.deltaTime);

        }

        if (Physics.Raycast(ray2, out hit2, 10, layerMask))
        {
            //Debug.Log(hit1.transform.gameObject.name);
            //this.transform.Rotate(rotation * Time.deltaTime);
            carRigidbody.velocity = transform.forward * speed * 0.5f;
        }
        else
        {
            carRigidbody.velocity = transform.forward * speed;
        }

        if (Physics.Raycast(ray3, out hit3, 5, layerMask))
        {
            //Debug.Log(hit1.transform.gameObject.name);
            this.transform.Rotate(rotation * Time.deltaTime);
        }

    }

        
}
