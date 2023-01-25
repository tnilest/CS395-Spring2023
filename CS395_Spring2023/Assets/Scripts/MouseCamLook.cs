using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamLook : MonoBehaviour
{

    // private GameObject player;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     // player = this.transform.parent.gameObject;
    //     // rb = player.GetComponent<Rigidbody>();
    // }

    // private void FixedUpdate(){
    //     var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
    //     Quaternion angle = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
    //     if(angle.x < 0.5 && angle.x > -0.5){
    //         transform.localRotation = angle;
    //     }
    // }

    // private void LateUpdate() {
    //     // //the delta value for mouse movement
    //     // var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
    //     // md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
    //     // smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
    //     // smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);

    //     // mouseLook += smoothV;

    //     // Quaternion angle = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
    //     // if(angle.x < 0.5 && angle.x > -0.5){
    //     //     transform.localRotation = angle;
    //     // }

    //     // rb.MoveRotation(rb.rotation * Quaternion.AngleAxis(mouseLook.x, Vector3.up));
    //     this.transform.forward = player.transform.forward;
    // }

}
