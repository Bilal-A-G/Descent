using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickaxe : MonoBehaviour
{
    public GameObject text;
    private bool inReach;
    private bool isPickedUp;
    public GameObject hands;

    InputActions actions;

    void Start()
    {
        text.SetActive(false);
        inReach = false;
        isPickedUp = false;
    }

    void Awake()
    {
        actions = new InputActions();
        actions.Enable();
        actions.Default.Interact.performed += e => Interact();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inReach = true;
            text.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inReach = false;
            text.SetActive(false);
        }
    }

    void Interact()
    {
        if (inReach && !isPickedUp)
        {
            // Enable or activate the picked up object here
            gameObject.SetActive(false); // Disable the pickaxe GameObject
            isPickedUp = true;
            // Enable or activate other objects as needed
        }
    }
    void Update()
    {
        
    }
}
