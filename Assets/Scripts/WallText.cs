using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WallText : MonoBehaviour
{
    [SerializeField] TMP_Text climbtext;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("WallText"))
            climbtext.gameObject.SetActive(true);
        else
        {
            climbtext.gameObject.SetActive(false);
        }
    }
}
