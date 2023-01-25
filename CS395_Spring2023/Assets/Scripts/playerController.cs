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
    //mouseCameraLook variables
    [SerializeField]
    public float sensitivity = 1.0f;
    private Vector2 mouseLook;
    

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
        float curr_speed = speed;

        //detects forward backward directional input and moves proportional to speed value.
        translation = Input.GetAxis("Vertical") * curr_speed * Time.deltaTime;

        //detects left/right keyboard input
        straffe = Input.GetAxis("Horizontal") * curr_speed * Time.deltaTime;

        curr_pos = rb.position;
        Vector3 input = new Vector3(straffe, 0, translation);
        input = rb.rotation * input;
        rb.AddForce(input, ForceMode.Impulse); 


        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(sensitivity, sensitivity));
       
        mouseLook += md;

        // Quaternion angle = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        // if(angle.x < 0.5 && angle.x > -0.5){
        //      transform.localRotation = angle;
        // }
        rb.rotation = Quaternion.AngleAxis(mouseLook.x, Vector3.up);
        //rb.MoveRotation(rb.rotation * Quaternion.AngleAxis(mouseLook.x, Vector3.up));

           
    }

}
