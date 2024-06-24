using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    public static PageManager Instance;

    public List<Pages> pages = new List<Pages>();

    public int pageCount = 5;
    public int pageCountUpdate = 0;

    public TextMeshProUGUI pageCountText;

   

    public LevelChanger levelChanger;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPage(Pages page)
    {
        pages.Add(page);
    }

    // Method to handle when an objective is complete
    public void PagesComplete(string pageID)
    {
        Pages page = pages.Find(obj => obj.pageID == pageID);
        if (pages != null)
        {
            // Handle any logic related to completing the objective
            Debug.Log($"Objective Completed: {page.description}");
        }
    }

    void Update()
    {
        pageCountText.text = "Collect Pages:" + pageCountUpdate + "/5";
        
        if (pageCount == 0)
        {
            Debug.Log("all pages collected");
            
            levelChanger.FadeToLevel("Win");
        }
    }
}
