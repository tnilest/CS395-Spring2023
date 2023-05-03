using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootColliderScript : MonoBehaviour //script to manage collision detection with ground/metal. Primarily matters for telling if character can jump.
{
    public Component playerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = this.GetComponentInParent<playerController>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Environement" | other.gameObject.tag == "Metal")
        {
            this.GetComponentInParent<playerController>().canJump = true;
            this.GetComponentInParent<playerController>().movementAudioSource.clip = this.GetComponentInParent<playerController>().landSound;
            this.GetComponentInParent<playerController>().movementAudioSource.volume = 0.1f;
            this.GetComponentInParent<playerController>().movementAudioSource.Play();
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