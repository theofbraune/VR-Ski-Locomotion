using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionTechnique : MonoBehaviour
{
    // Please implement your locomotion technique in this script. 
    public OVRInput.Controller leftController;
    public OVRInput.Controller rightController;
    [Range(0, 10)] public float translationGain = 0.5f;
    public GameObject hmd;
    [SerializeField] private float leftTriggerValue;    
    [SerializeField] private float rightTriggerValue;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isIndexTriggerDown;


    private float brake;

    public Rigidbody rbdy;
    private Vector3 direction;
    

    public float speed;
    public float steering;
    public Vector3 force;

    private float min_height;

    private bool downhill;
    private bool uphill;

    //for the locomotion with the cubes

    public GameObject cube_left_up;
    public GameObject cube_right_up;
    public GameObject cube_left_down;
    public GameObject cube_right_down;

    public bool in_up_left = false;
    private bool in_up_right = false;
    public bool in_down_left = false;
    private bool in_down_right = false;

    //variables that detact whether a stroke between the cubes is finished.
    public bool started_left = false;
    public bool arrived_left = false;
    private bool started_right = false;
    private bool arrived_right = false;

    //scripts for the collision detection with the handle and the box
    //Collision detection with the handle of the box.
    public ColliderLeft collider_left;
    public ColliderRight collider_right;

    //measure the time
    private float t1_left;
    private float t2_left;
    private float t1_right;
    private float t2_right;

    //
    private bool trigger_haptic_left;
    private bool trigger_haptic_right;
    private float start_trigger_left;
    private float delta_t_left;

    private float start_trigger_right;
    private float delta_t_right;




    /////////////////////////////////////////////////////////
    // These are for the game mechanism.
    public ParkourCounter parkourCounter;
    public string stage;
    public collide_final coll;

    void Start()
    {
        
    }

    void Update()
    {

        
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // Please implement your LOCOMOTION TECHNIQUE in this script :D.
        leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, leftController); 
        rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, rightController); 

        

        
        ////////////////////////////////////////////////////////////////////////////////
        // These are for the game mechanism.
        if (OVRInput.Get(OVRInput.Button.Two) || OVRInput.Get(OVRInput.Button.Four))
        {
            if (parkourCounter.parkourStart)
            {
                this.transform.position = parkourCounter.currentRespawnPos;
            }
        }
        
    }

    private void FixedUpdate()
    {
        rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, rightController);
        brake = rightTriggerValue;
        rbdy.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        Vector2 rightjoystickval = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, rightController);
        speed = rightjoystickval[1];
        steering = rightjoystickval[0];
        //first try to move with the joystick
        Vector3 leftdir = new Vector3(-1.0f, 0.0f, 0.0f);
        Vector3 rightdir = new Vector3(1.0f, 0.0f, 0.0f);
        Vector3 strdir = new Vector3(0.0f, 0.0f, 1.0f);
        Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
        Vector3 forward_bef = (hmd.transform.forward).normalized;

        Vector3 forward = new Vector3(forward_bef[0], 0.0f, forward_bef[2]);
        leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, leftController);
        min_height = 0.5f;


        rbdy = this.GetComponent<Rigidbody>();


        
        



        //check whether we need to go downhill
        if ((this.transform.position.x > 74.0f) & (this.transform.position.x < 104.0f))
        {
            if ((this.transform.position.z < 70.0f) & (this.transform.position.z > 9.5f))
            {

                this.GetComponent<Collider>().isTrigger = true;
                float m = 11.6f / 60.5f;
                float n = -1.321f;
                float y_new = m * this.transform.position.z + n;
                min_height = y_new;
                this.transform.position = new Vector3(this.transform.position.x, y_new, this.transform.position.z);
                downhill = true;
            }
            else
            {
                downhill = false;
            }
        }
        else
        {
            downhill = false;
        }

        if ((this.transform.position.x > 74.0f) & (this.transform.position.x < 104.0f))
        {
            if ((this.transform.position.z < 9.5))
            {

                this.GetComponent<Collider>().isTrigger = true;
                min_height = 0.5f;
                this.transform.position = new Vector3(this.transform.position.x, 0.5f, this.transform.position.z);
            }
        }

        //check whether we need to go uphill
        if ((this.transform.position.x > 74.0f) & (this.transform.position.x < 104.0f))
        {
            if ((this.transform.position.z < 162.0f) & (this.transform.position.z > 102.0f))
            {

                this.GetComponent<Collider>().isTrigger = true;
                float m = -11.6f / 60.0f;
                float n = 31.82f;
                float y_new = m * this.transform.position.z + n;
                min_height = y_new;
                this.transform.position = new Vector3(this.transform.position.x, y_new, this.transform.position.z);
                uphill = true;
            }
            else
            {
                uphill = false;
            }
        }
        else
        {
            uphill = false;
        }

        //being on the bridge
        if ((this.transform.position.x > 74.0f) & (this.transform.position.x < 104.0f))
        {
            if ((this.transform.position.z < 100.0f) & (this.transform.position.z > 70.0f))
            {

                this.GetComponent<Collider>().isTrigger = true;
                min_height = 12.1f;
                this.transform.position = new Vector3(this.transform.position.x, 12.1f, this.transform.position.z);
            }
        }


        in_down_left = collider_left.in_left_down;
        in_up_left = collider_left.in_left_up;
        in_down_right = collider_right.in_right_down;
        in_up_right = collider_right.in_right_up;

        //define the brake value
        if (brake > 0.1f)
        {
            rbdy.drag = 2 * brake;
        }
        if ((in_up_left) & (!in_down_left))
        {
            started_left = true;
            t1_left = Time.time;
        }
        //check whether we are in the lower box. Measure the time for the first appearence.
        if (((started_left) & (in_down_left)))
        {
            if (arrived_left == false)
            {
                t2_left = Time.time;
                arrived_left = true;
            }

        }

        if (arrived_left)
        {
            Vector3 force = forward.normalized;

            started_left = false;
            arrived_left = false;

            if (uphill)
            {
                rbdy.drag = 0.8f;
            }
            else if (downhill)
            {
                rbdy.drag = -0.01f;
            }
            else
            {
                rbdy.drag = 0.4f;
            }
            if (brake > 0.1f)
            {
                rbdy.drag = brake * 2;
            }
            collider_left.triggered_left = true;
            OVRInput.SetControllerVibration(1, 1, leftController);
            trigger_haptic_left = true;
            start_trigger_left = Time.time;
            delta_t_left = t2_left - t1_left;
            //we see 0.1 is a good indicator for the speed, thus if we are faster the push is bigger
            rbdy.AddForce((150) * (10 * delta_t_left) * force);


        }
        if (trigger_haptic_left)
        {
            //check whether we need to trigger the haptics
            if ((Time.time - start_trigger_left) > 0.1f)
            {
                OVRInput.SetControllerVibration(0, 0, leftController);
                trigger_haptic_left = false;
            }

        }

        //same for the right controller
        if ((in_up_right) & (!in_down_right))
        {
            started_right = true;
            t1_right = Time.time;
        }

        if (started_right & in_down_right)
        {
            if (arrived_right == false)
            {
                t2_right = Time.time;
                arrived_right = true;
            }

        }
        if (arrived_right)
        {
            Vector3 force = forward.normalized;

            started_right = false;
            arrived_right = false;
            if (uphill)
            {
                rbdy.drag = 0.8f;
            }
            else if (downhill)
            {
                rbdy.drag = -0.01f;
            }
            else
            {
                rbdy.drag = 0.4f;
            }
            if (brake > 0.1f)
            {
                rbdy.drag = brake * 2;
            }
            collider_right.triggered_right = true;

            trigger_haptic_right = true;
            start_trigger_right = Time.time;
            OVRInput.SetControllerVibration(1, 1, rightController);
            delta_t_right = t2_right - t1_right;
            rbdy.AddForce((150) * (10 * delta_t_right) * force);



        }
        if (trigger_haptic_right)
        {
            //check whether we need to trigger the haptics
            if ((Time.time - start_trigger_right) > 0.1f)
            {
                OVRInput.SetControllerVibration(0, 0, rightController);
                trigger_haptic_right = false;
            }

        }
    }

    void OnTriggerEnter(Collider other)
    {

        // These are for the game mechanism.
        if (other.CompareTag("banner") || other.CompareTag("banner2"))
        {
            stage = other.gameObject.name;
            parkourCounter.isStageChange = true;
        }
        else if (other.CompareTag("coin"))
        {
            parkourCounter.coinCount += 1;
            this.GetComponent<AudioSource>().Play();
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("banner2"))
        {
            coll.finished = true;
        }
        // These are for the game mechanism.

    }
}