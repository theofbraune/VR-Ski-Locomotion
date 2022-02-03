using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderLeft : MonoBehaviour
{
    public bool in_left_up = false;
    public bool in_left_down = false;
    public bool triggered_left = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (triggered_left)
        {
            triggered_left = false;
            this.GetComponent<AudioSource>().Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        print("Detected object" + other.name);
        if (other.CompareTag("cube_left_up"))
        {
            in_left_up = true;


        }
        else if (other.CompareTag("cube_left_down"))
        {

            in_left_down = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("cube_left_up"))
        {
            in_left_up = false;

        }
        else if (other.CompareTag("cube_left_down"))
        {
            in_left_down = false;
        }

    }
}
