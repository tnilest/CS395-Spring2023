using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    [SerializeField]
    public float speed = 25.0f; //player's movespeed
    private float vert; //stores keyboard input for vertical movement
    private float horz; //storese keyboard input for horizontal movement
    private Rigidbody rb; //player's rigidbody
    private Vector3 curr_pos; //current position
    //mouseCameraLook variables
    [SerializeField]
    public float sensitivity = 1.0f; //camera sensitivity
    public float smoothing = 2.0f; //value that helps smooth camera movement
    private Vector2 mouseLook; //gets axis of mouse movement
    private Vector2 smoothV; //smoothed verision of mouse movement
    new private GameObject camera; //camera object
    private float maxLookAngle = 90.0f; //limits how high player can look
    private float minLookAngle = -90.0f; //limits how low player can look

    public bool canJump;
    public float jumpHeight = 12.0f; //sets jump height

    public LayerMask PlayerMask;

    //stores the materials for interactable items
    public Material itemColor; //stores item's default material color
    public Material highlightedItemColor; //stores item's highlighted color

    public GameObject lastHighlightedObject; //keeps track of the previously highlighted object

    public float pushPower = 100.0f; //total push/pull power that is distributed between player & object

    private bool interacting; //a boolean to track if the player is currently interacting with (pushing or pulling) an object
    
    [SerializeField]
    private AudioClip pushSound; //push sound effect
    [SerializeField]
    private AudioClip pullSound; //pull sound effect

    [SerializeField]
    private AudioClip runSound;  //run sound effect

    [SerializeField]
    private AudioClip jumpSound; //jump sound effect

    [SerializeField]
    public AudioClip landSound; //landing sound effect

    private AudioSource audioSource; //player-centered audio source. Used for pushing,pulling audio.

    [SerializeField]
    public AudioSource movementAudioSource; //audio source centered on foot-collider. used to play movement based audio clips.

    public bool controlsEnabled; //boolean to prevent certain keyboard inputs from being used. Currently only applied for transition between scenes.


    // Start is called before the first frame update
    void Start()
    {
        //initialization of values here. also locks camera during operation
        Cursor.lockState = CursorLockMode.Locked; //used to turn off mouse cursor during execution.
        rb = gameObject.GetComponent<Rigidbody>(); // collects player's rididbody
        camera = this.transform.GetChild(0).gameObject; // collects camera object, assuming it is the first child of player
        //canJump = true;
        interacting = false;

        camera.AddComponent(typeof(AudioListener)); //initialize audio listner on scene load. 

        audioSource = GetComponent<AudioSource>();

        movementAudioSource.clip = runSound;

        controlsEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && canJump && controlsEnabled) //limits jumping
        {
            rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);

            //play jump sound effect
            movementAudioSource.Stop();
            movementAudioSource.clip = jumpSound;
            movementAudioSource.volume = 0.6f;
            movementAudioSource.Play();

        }

        if (Input.GetKey("escape") && controlsEnabled) //escape key returns to main menu
        {
            Cursor.lockState = CursorLockMode.None; //used to turn on mouse cursor during execution.
            SceneManager.LoadScene(0); //return to main menu
        }

        if (Input.GetKey(KeyCode.R)) //reloads the current scene
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void FixedUpdate()
    {
        //begin code for push/pull objects

        if (interacting) //if player is currently pushing/pulling on an object.
        {
            if(Vector3.Distance(lastHighlightedObject.GetComponent<Rigidbody>().position, rb.position) <= 60) 
            {
                float itemMass = lastHighlightedObject.GetComponent<Rigidbody>().mass; //interacted item's mass
                float playerMass = rb.mass; //player's mass
                float netMass = playerMass + itemMass; //combined mass in system.

                if (Input.GetMouseButton(1) && controlsEnabled)
                { //right click pulls object
                    //add force to item proportional to itemMass, playerMass, and pushPower

                    if(Vector3.Distance(lastHighlightedObject.GetComponent<Rigidbody>().position, rb.position) <= 5 && itemMass < playerMass) //If lighter object is pulled close, direct it in front of player so that they can manipulate it more consistently
                    {
                        lastHighlightedObject.GetComponent<Rigidbody>().velocity = lastHighlightedObject.GetComponent<Rigidbody>().velocity * 0.95f; //slows down velocity for better manipulation.
                        lastHighlightedObject.GetComponent<Rigidbody>().AddForce(-1 * pushPower * (itemMass / netMass) * (lastHighlightedObject.transform.position - (this.transform.position + camera.transform.forward*1.5f)).normalized, ForceMode.Force);
                    }
                    else
                    {
                        lastHighlightedObject.GetComponent<Rigidbody>().AddForce(-1 * pushPower * (playerMass / netMass) * (lastHighlightedObject.transform.position - this.transform.position).normalized, ForceMode.Force);
                        rb.AddForce(-1 * pushPower * (itemMass / netMass) * (this.transform.position - lastHighlightedObject.transform.position).normalized, ForceMode.Force);
                    }

                    interacting = true;

                    if(audioSource.clip == pushSound)
                    {
                        audioSource.clip = pullSound;
                        audioSource.Play();
                    }
                 }

                if (Input.GetMouseButton(0) && controlsEnabled)
                 { //left click pushes object
                    //applies propotional forces to both object at player. 
                    lastHighlightedObject.GetComponent<Rigidbody>().AddForce(-1 * pushPower * (playerMass / netMass) * (this.transform.position - lastHighlightedObject.transform.position).normalized, ForceMode.Force);
                    rb.AddForce(-1 * pushPower * (itemMass / netMass) * (lastHighlightedObject.transform.position - this.transform.position).normalized, ForceMode.Force);
                    interacting = true;
                    if (audioSource.clip == pullSound)
                    {
                        audioSource.clip = pushSound;
                        audioSource.Play();
                    }
                }

                if (!Input.GetMouseButton(1) && !Input.GetMouseButton(0))
                 {
                    interacting = false;
                    audioSource.Stop();
                 }
            }
            else
            {
                interacting = false;
                audioSource.Stop();
            }  
        }

        else if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit,60)) 
        { 
           if(hit.collider.gameObject.tag == "Metal"){ //if spherecast hits a metal object, then highlight it and can push/pull it
                hit.collider.gameObject.GetComponent<MeshRenderer>().material = highlightedItemColor; //highlight interactable object you are looking at
                if(hit.collider.gameObject != lastHighlightedObject){ //make sure that only 1 object is highlighted at a time
                    
                    if(lastHighlightedObject != null){
                        lastHighlightedObject.GetComponent<MeshRenderer>().material = itemColor;
                    }
                    lastHighlightedObject = hit.collider.gameObject;
                }
                
                float itemMass = hit.collider.gameObject.GetComponent<Rigidbody>().mass;
                float playerMass = rb.mass;
                float netMass = playerMass + itemMass;

                if(Input.GetMouseButton(1) && controlsEnabled){ //right click pulls object
                    //add force to item proportional to itemMass, playerMass, and pushPower
                    interacting = true;
                    hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(-1 * pushPower * (playerMass / netMass) * (hit.collider.gameObject.transform.position - this.transform.position).normalized, ForceMode.Force);
                    rb.AddForce(-1 * pushPower * (itemMass / netMass) * (this.transform.position - hit.collider.gameObject.transform.position).normalized, ForceMode.Force);
                    audioSource.clip = pullSound;
                    audioSource.Play();
                }

                if(Input.GetMouseButton(0) && controlsEnabled)
                { //left click pushes object
                    interacting = true;
                    //applies propotional forces to both object at player. 
                    hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(-1 * pushPower * (playerMass / netMass) * (this.transform.position - hit.collider.gameObject.transform.position).normalized, ForceMode.Force);
                    rb.AddForce(-1 * pushPower * (itemMass / netMass) * (hit.collider.gameObject.transform.position - this.transform.position).normalized, ForceMode.Force);
                    audioSource.clip = pushSound;
                    audioSource.Play();
                }
           }
           else if (hit.collider.gameObject.tag != "Metal" && lastHighlightedObject != null && !interacting){ //if not looking at a metal object, then remove previous highlight
                lastHighlightedObject.GetComponent<MeshRenderer>().material = itemColor;
           }
        }
        else
        {
            if(lastHighlightedObject != null)
            {
                lastHighlightedObject.GetComponent<MeshRenderer>().material = itemColor;
            }
        }
        //end code for push/pull

        //begin basic movement code
        if (controlsEnabled)
        {
            float curr_speed = speed;

            vert = Input.GetAxis("Vertical") * curr_speed * Time.deltaTime; //detects forward backward directional input and moves proportional to speed value.
            horz = Input.GetAxis("Horizontal") * curr_speed * Time.deltaTime; //detects left/right keyboard input


            curr_pos = rb.position;
            Vector3 input = new Vector3(horz, 0.1f, vert);
            input = rb.rotation * input;
            rb.AddForce(input, ForceMode.VelocityChange);

            if (canJump && rb.velocity.magnitude > 0.2f)
            {
                if (!movementAudioSource.isPlaying)
                {
                    movementAudioSource.clip = runSound;
                    movementAudioSource.volume = 1.2f;
                    movementAudioSource.Play();
                }
            }
            else
            {
                if(movementAudioSource.clip != jumpSound && movementAudioSource.clip != landSound)
                {
                    movementAudioSource.Stop();
                }
            }
         }
        
        if (!controlsEnabled)
        {
            movementAudioSource.Stop();
            rb.velocity = rb.velocity * 0.9f;
            rb.AddForce(new Vector3(0.0f, 0.5f, 0.0f), ForceMode.VelocityChange);
        }

        //use mouse movement to look around. base input is jittery so use smoothing.
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        mouseLook += smoothV;

        rb.rotation = Quaternion.AngleAxis(mouseLook.x, Vector3.up);
        //end basic movement code
    }

    void LateUpdate()
    {
        //begin camera movement
        mouseLook.y = Mathf.Clamp(mouseLook.y, minLookAngle, maxLookAngle);
        Quaternion lookAngle = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        camera.transform.localRotation = lookAngle;
        //end camera movement
    }
}
