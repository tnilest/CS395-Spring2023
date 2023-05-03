using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour //script to manage exit door activation. 
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

    [SerializeField]
    private GameObject particles;

    public GameObject player;
    public Component playerScript;
    public Rigidbody rb;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = activeDoor;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        rb = player.GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        if (isUnlocked)
        {
            door.GetComponent<MeshRenderer>().material = ActivatedDoorMaterial;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                particles.SetActive(true);
            }      
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Metal") //unlocks exit door if metal object collides
        {
            isUnlocked = true;
            col.gameObject.SetActive(false);
        }

        if(col.tag == "Player" && isUnlocked)
        {
            player.GetComponent<playerController>().controlsEnabled = false; //disables keyboard controlls so player can be carried up exit.
            StartCoroutine(sleeper());
        }
    }

    IEnumerator sleeper() //used to allow player to be slowly carried up exit, then load next scene.
    {
        yield return new WaitForSeconds(4);
        if(SceneManager.GetActiveScene().buildIndex + 1 == 4)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
