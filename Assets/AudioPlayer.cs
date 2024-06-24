using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public List<AudioClip> footstepSounds;
    public AudioClip crouchSound;
    public AudioClip landedSound;

    public AudioSource source;

    public void PlayFootstepSound() => source.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Count)]);
    public void PlayCrouch() => source.PlayOneShot(crouchSound);
    public void PlayLanded() => source.PlayOneShot(landedSound);
}
