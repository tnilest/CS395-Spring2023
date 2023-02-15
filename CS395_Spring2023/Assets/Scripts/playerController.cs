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
    new private GameObject camera;
    private float maxLookAngle = 50.0f;
    private float minLookAngle = -50.0f;

    public bool canJump;
    public float jumpHeight = 5.0f;

    //stores the materials for interactable items
    public Material itemColor;
    public Material highlightedItemColor;

    public GameObject lastHighlightedObject;

    public float pushPower = 1.0f;

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
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        if (Physics.SphereCast(camera.transform.position, 1.0f, camera.transform.forward, out RaycastHit hit)) 
        { 
           if(hit.collider.gameObject.tag == "Metal"){ //if spherecast hits a metal object, then highlight it and can push/pull it
                if(hit.collider.gameObject != lastHighlightedObject){
                    hit.collider.gameObject.GetComponent<MeshRenderer>().material = highlightedItemColor;
                    if(lastHighlightedObject != null){
                        lastHighlightedObject.GetComponent<MeshRenderer>().material = itemColor;
                    }
                    lastHighlightedObject = hit.collider.gameObject;
                }
                
                float itemMass = hit.collider.gameObject.GetComponent<Rigidbody>().mass;
                float playerMass = rb.mass;

                if(Input.GetMouseButton(1)){ //right click pulls object
                //add force to item proportional to itemMass, playerMass, and pushPower
                    if (itemMass > playerMass) {
                        hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(-1 * (1 - (playerMass / itemMass)) * (hit.collider.gameObject.transform.position - this.transform.position).normalized, ForceMode.Impulse);
                        rb.AddForce(-1 * ((playerMass / itemMass)) * (this.transform.position - hit.collider.gameObject.transform.position).normalized, ForceMode.Impulse);
                    }
                    else {
                        hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(-1 * ((playerMass / itemMass)) * (hit.collider.gameObject.transform.position - this.transform.position).normalized, ForceMode.Impulse);
                        rb.AddForce(1 * (1 - (playerMass / itemMass)) * (this.transform.position - hit.collider.gameObject.transform.position).normalized, ForceMode.Impulse);
                    }
                  
                print("pulling");
                }

                if(Input.GetMouseButton(0)){ //left click pushes object
                    if (itemMass > playerMass) {
                        hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(-1 * (1 - (playerMass / itemMass)) * (this.transform.position - hit.collider.gameObject.transform.position).normalized, ForceMode.Impulse);
                        rb.AddForce(-1 * ((playerMass / itemMass)) * (hit.collider.gameObject.transform.position - this.transform.position).normalized, ForceMode.Impulse);
                    }
                    else {
                        hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(-1 * ((playerMass / itemMass)) * (this.transform.position - hit.collider.gameObject.transform.position).normalized, ForceMode.Impulse);
                        rb.AddForce(1 * (1 - (playerMass / itemMass)) * (hit.collider.gameObject.transform.position - this.transform.position).normalized, ForceMode.Impulse);
                    }
                    //hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(1 * camera.transform.forward, ForceMode.Impulse);
                    print("pushing");
                }
           }
           else if (hit.collider.gameObject.tag != "Metal" && lastHighlightedObject != null){ //if not looking at a metal object, then remove previous highlight
                lastHighlightedObject.GetComponent<MeshRenderer>().material = itemColor;
           }
        }

        ///////////////////////////////////////// above is new work for preliminary metal interactions
        float curr_speed = speed;

        vert = Input.GetAxis("Vertical") * curr_speed * Time.deltaTime; //detects forward backward directional input and moves proportional to speed value.
        horz = Input.GetAxis("Horizontal") * curr_speed * Time.deltaTime; //detects left/right keyboard input

        curr_pos = rb.position;
        Vector3 input = new Vector3(horz, 0.2f, vert);
        input = rb.rotation * input;
        rb.AddForce(input, ForceMode.Impulse);
        //rb.AddForce(new Vector3(0, 4, 0), ForceMode.Acceleration);

        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        mouseLook += smoothV;

        rb.rotation = Quaternion.AngleAxis(mouseLook.x, Vector3.up);

        // print(rb.velocity.magnitude);
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
