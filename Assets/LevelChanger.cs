using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator anim;

    public string switchLevel;

    
    

    public void FadeToLevel(string levelname)
    {
        switchLevel = levelname;    
        anim.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
      if (switchLevel == "Win") 
        { 
            SceneManager.LoadScene("Win"); 
        }
       if(switchLevel == "MainMenu")
        {
            SceneManager.LoadScene("MainMenu");
        }

    }

    public void FadeToMainMenu()
    {
        switchLevel = "MainMenu";
        anim.SetTrigger("FadeOut");
    }

    void Update()
    {
        if (!SceneManager.GetSceneByName("Game").isLoaded)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


}
