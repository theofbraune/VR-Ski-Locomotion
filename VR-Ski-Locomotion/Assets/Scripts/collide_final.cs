using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collide_final : MonoBehaviour
{
    public bool finished;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (finished)
        {
            Vector3 up = new Vector3(0.0f, 10.0f, 0.0f);
            this.transform.position = this.transform.position + up;
        }
    }
}
