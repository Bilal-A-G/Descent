using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pages : MonoBehaviour
{
    public GameObject text;
    private GameObject page;
    private bool inReach;
    private bool isComplete;
    public string pageID;
    public string description;

    public PageManager pageManager;
    public EventObject madeLoudNoise;

    InputActions actions;
    void Start()
    {
        text.SetActive(false);
        inReach = false;
        page = this.gameObject;
        PageManager.Instance.AddPage(this);

    }

    void Awake()
    {
        actions = new InputActions();
        actions.Enable();
        actions.Default.Interact.performed += e => Interact();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
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
        if (inReach)
        {
            text.SetActive(false );
            inReach = false;
            isComplete = true;
        }
    }

    private void CompleteObjective()
    {
        
        // Notify the ObjectiveManager that this objective is complete
        PageManager.Instance.PagesComplete(pageID);

        // Debug statement to notify completion (you can customize this message)
        Debug.Log($"Objective '{description}' is complete!");

        pageManager.pageCount--;
        pageManager.pageCountUpdate++;
        madeLoudNoise.Invoke(true);

        // Optionally trigger additional events or actions upon completion
        Destroy(gameObject); // Destroy the objective GameObject once completed
    }
    void Update()
    {
        if (isComplete)
        {
            CompleteObjective();
        }
    }
}
