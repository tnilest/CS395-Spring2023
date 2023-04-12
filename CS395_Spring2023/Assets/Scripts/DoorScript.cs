using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    [SerializeField]
    GameObject door;
    [SerializeField]
    bool isUnlocked;
    [SerializeField]
    public Material ActivatedDoorMaterial;

    [SerializeField]
    private AudioClip activeDoor;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = activeDoor;
        
    }

    void LateUpdate()
    {
        if (isUnlocked)
        {
            door.GetComponent<MeshRenderer>().material = ActivatedDoorMaterial;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
                
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Metal")
        {
            isUnlocked = true;
            col.gameObject.SetActive(false);
            
        }

        if(col.tag == "Player" && isUnlocked)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }
}
