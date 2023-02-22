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
        if(other.gameObject.tag == "Environement" | other.gameObject.tag == "Metal")
        {
            this.GetComponentInParent<playerController>().canJump = true; 
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "Environement" | other.gameObject.tag == "Metal")
        {
            this.GetComponentInParent<playerController>().canJump = true; 
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Environement" | other.gameObject.tag == "Metal")
        {
            this.GetComponentInParent<playerController>().canJump = false; 
        }
    }
}