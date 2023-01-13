using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamLook : MonoBehaviour
{
    //Creates adjustable sensitivity
    [SerializeField]
    public float sensitivity = 5.0f;

    //Creates adjustable smoothness 
    [SerializeField]
    public float smoothing = 2.0f;

    //player object
    public GameObject player;
    private Rigidbody rb;

    private Vector2 mouseLook;

    private Vector2 smoothV;

    // Start is called before the first frame update
    void Start()
    {
        player = this.transform.parent.gameObject;
        rb = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //the delta value for mouse movement
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);

        mouseLook += smoothV;

        Quaternion angle = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        if(angle.x < 0.5 && angle.x > -0.5){
            transform.localRotation = angle;
        }

        rb.rotation = Quaternion.AngleAxis(mouseLook.x, Vector3.up);
    }

    void FixedUpdate()
    {
        
    }

}
