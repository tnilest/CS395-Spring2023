using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField]
    public float speed = 10.0f;
    private float translation;
    private float straffe;
    // [SerializeField]
    // public float jump_Height = 5.0f;
    private Rigidbody rb;
    // private Collider collider;
    private Vector3 curr_pos;

    // [SerializeField]
    // public GameObject ground;
    // private bool grounded;


    // Start is called before the first frame update
    void Start()
    {
        //used to turn off mouse cursor during execution.
        Cursor.lockState = CursorLockMode.Locked;
        
        rb = gameObject.GetComponent<Rigidbody>();
        // collider = gameObject.GetComponent<Collider>();

        // ground = GameObject.Find("floor");
        
        // grounded = true;
    }

    // Update is called once per frame
    void Update()
    {

        float curr_speed = speed;
        if (Input.GetKey(KeyCode.LeftShift)){
            curr_speed = 2 * speed;
        }

        //detects forward backward directional input and moves proportional to speed value.
        translation = Input.GetAxis("Vertical") * curr_speed * Time.deltaTime;

        //detects left/right keyboard input
        straffe = Input.GetAxis("Horizontal") * curr_speed * Time.deltaTime;

        // if (Input.GetKey(KeyCode.Space) && grounded){
        //     rb.AddForce(transform.up * 1000.0f);
        //     grounded = false;
        // }

        curr_pos = rb.position;
        Vector3 input = new Vector3(straffe, 0, translation);
        input = rb.rotation * input;
        //rb.velocity = input;
        rb.MovePosition(curr_pos + input);
        //transform.Translate(straffe, 0, translation);

        

    }

    // bool isGrounded(){
    //     return Physics.Raycast(transform.position, -Vector3.up, getDistanceToGround() + 1);
    // }

    // float getDistanceToGround(){
    //     return collider.bounds.extents.y;
    // }
}
