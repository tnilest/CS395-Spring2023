using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField]
    public float speed = 6.0f;
    [SerializeField]
    public float max_speed = 15f;
    private float vert;
    private float horz;
    private Rigidbody rb;
    private Vector3 curr_pos;
    //mouseCameraLook variables
    [SerializeField]
    public float sensitivity = 1.0f;
    public float smoothing = 2.0f;
    private Vector2 mouseLook;
    private Vector2 smoothV;
    private GameObject camera;
    private float maxLookAngle = 50.0f;
    private float minLookAngle = -50.0f;

    public bool canJump;
    public float jumpHeight = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //used to turn off mouse cursor during execution.
        rb = gameObject.GetComponent<Rigidbody>(); // collects player's rididbody
        camera = this.transform.GetChild(0).gameObject; // collects camera object, assuming it is the first child of player
        canJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && canJump)
        {
            rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
            canJump = false;
        }
    }

    void FixedUpdate()
    {
        float curr_speed = speed;

        vert = Input.GetAxis("Vertical") * curr_speed * Time.deltaTime; //detects forward backward directional input and moves proportional to speed value.
        horz = Input.GetAxis("Horizontal") * curr_speed * Time.deltaTime; //detects left/right keyboard input

        curr_pos = rb.position;
        Vector3 input = new Vector3(horz, 0, vert);
        input = rb.rotation * input;
        rb.AddForce(input, ForceMode.Impulse);

        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        mouseLook += smoothV;

        rb.rotation = Quaternion.AngleAxis(mouseLook.x, Vector3.up);

        print(rb.velocity.magnitude);
        if (rb.velocity.magnitude > max_speed)
        {
            rb.velocity = rb.velocity.normalized * max_speed;
        }
    }

    void LateUpdate()
    {
        mouseLook.y = Mathf.Clamp(mouseLook.y, minLookAngle, maxLookAngle);
        Quaternion lookAngle = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        camera.transform.localRotation = lookAngle;
    }

}
