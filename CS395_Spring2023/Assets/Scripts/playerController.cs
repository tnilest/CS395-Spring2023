using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField]
    public float speed = 6.0f;
    [SerializeField]
    public float max_speed = 15f;
    private float translation;
    private float straffe;
    private Rigidbody rb;
    private Vector3 curr_pos;

    // Start is called before the first frame update
    void Start()
    {
        //used to turn off mouse cursor during execution.
        Cursor.lockState = CursorLockMode.Locked;
        
        rb = gameObject.GetComponent<Rigidbody>();
        // transform = gameObject.GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate() {
        if(Vector3.Magnitude(rb.velocity) <= max_speed){
            float curr_speed = speed;

            //detects forward backward directional input and moves proportional to speed value.
            translation = Input.GetAxis("Vertical") * curr_speed * Time.deltaTime;

            //detects left/right keyboard input
            straffe = Input.GetAxis("Horizontal") * curr_speed * Time.deltaTime;

            curr_pos = rb.position;
            Vector3 input = new Vector3(straffe, 0, translation);
            input = rb.rotation * input;

            rb.AddForce(input, ForceMode.Impulse);
            //rb.MovePosition(curr_pos + input);
        }
        elseif(Vector3.Magnitude(rb.velocity) > max_speed){
            float reduceSpeed = 
        }
        
    }

}
