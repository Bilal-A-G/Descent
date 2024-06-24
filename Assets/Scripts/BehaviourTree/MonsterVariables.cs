using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterVariables : MonoBehaviour
{
    public float alertness;
    public string playerName;
    public string playerCameraName;
    public Transform player;
    public Transform playerCamera;
    public Transform spider;
    public Transform spiderEyes;

    public Vector3 lookAtPos;
    public Vector3 playerLastKnownPos;
    public float alertnessDecreaseRate;

    private void Start()
    { 
        player = GameObject.Find(playerName).transform;
        playerCamera = player.GetChild(0).Find(playerCameraName);
    }

    private void Update()
    {
        alertness -= alertnessDecreaseRate * Time.deltaTime;
        alertness = Mathf.Clamp(alertness, 0.0f, 1.0f);
    }

}
