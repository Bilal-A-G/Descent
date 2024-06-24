using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

public class Kill : NodeBase
{
    public MonsterVariables variables;
    public Transform newPlayerCamPos;
    public Animator monsterAnimator;
    public string flashlightName;
    public float lerpSpeed;
    public float newFlashlightIntensity;
    public string killTrigger;
    public string deathSceneName;

    private Transform m_player;
    private Transform m_playerCamera;
    private Light m_playerFlashlight;
    private Animator m_cameraAnimator;
    private bool m_isPlaying;

    private bool m_setTrigger;
    private bool m_setCameraTrigger;
    private float m_lerp;

    public override void OnTransition()
    {
        m_player = variables.player;
        m_playerCamera = variables.playerCamera;
        m_playerFlashlight = GameObject.Find(flashlightName).GetComponent<Light>();
        m_cameraAnimator = m_playerCamera.GetComponent<Animator>();

        m_player.GetComponent<CharacterController>().enabled = false;
        m_isPlaying = true;
    }

    private void Update()
    {
        if (!m_isPlaying)
            return;

        m_playerCamera.position = Vector3.Lerp(m_playerCamera.position,
            newPlayerCamPos.position,
            lerpSpeed * Time.deltaTime);

        m_playerCamera.rotation = Quaternion.Lerp(m_playerCamera.rotation, 
            newPlayerCamPos.rotation, 
            lerpSpeed * Time.deltaTime);

        m_playerFlashlight.intensity = Mathf.Lerp(m_playerFlashlight.intensity, newFlashlightIntensity, Time.deltaTime * lerpSpeed);
        variables.lookAtPos = newPlayerCamPos.position;
        m_lerp += Time.deltaTime * lerpSpeed;

        m_player.transform.forward = -variables.spider.forward;

        if (!m_setTrigger)
        {
            m_setTrigger = true;
            monsterAnimator.SetTrigger(killTrigger);
        }

        if (m_lerp >= 1.0f && !m_setCameraTrigger) 
        {
            m_setCameraTrigger = true;
            m_cameraAnimator.enabled = true;
        }
    }

    public void CutToDeathScreen() 
    {
        SceneManager.LoadScene(deathSceneName);
    }
}
