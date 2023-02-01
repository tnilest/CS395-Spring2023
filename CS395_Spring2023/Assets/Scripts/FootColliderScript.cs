using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootColliderScript : MonoBehaviour
{
    public Component playerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = this.GetComponentInParent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Environement"){
            this.GetComponentInParent<playerController>().canJump = true; 
        }
        else if (other.gameObject.tag == "Ramp"){
            this.GetComponentInParent<Rigidbody>().AddForce(new Vector3(0,1,0), ForceMode.Impulse);
        }
        
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "Environement"){
            this.GetComponentInParent<playerController>().canJump = true; 
        }
        if (other.gameObject.tag == "Ramp"){
            this.GetComponentInParent<Rigidbody>().AddForce(new Vector3(0,1,0), ForceMode.Impulse);
        } 
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Environement"){
            this.GetComponentInParent<playerController>().canJump = false; 
        }
    }
}