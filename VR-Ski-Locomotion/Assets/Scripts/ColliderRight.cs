using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderRight : MonoBehaviour
{
    public bool in_right_up;
    public bool in_right_down;
    public bool triggered_right = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (triggered_right)
        {
            this.GetComponent<AudioSource>().Play();
            triggered_right = false;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        
        Debug.Log("Detected object" + other.name);
        if (other.CompareTag("cube_right_up"))
        {
            in_right_up = true;
            //this.GetComponent<AudioSource>().Play();
            Debug.Log("This is collider: " + in_right_up);

        }
        else if (other.CompareTag("cube_right_down"))
        {
            //this.GetComponent<AudioSource>().Play();
            in_right_down = true;
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("cube_right_up"))
        {
            in_right_up = false;
            //this.GetComponent<AudioSource>().Play();
            Debug.Log("This is collider: " + in_right_up);

        }
        else if (other.CompareTag("cube_right_down"))
        {
            //this.GetComponent<AudioSource>().Play();
            in_right_down = false;
        }

    }
}
