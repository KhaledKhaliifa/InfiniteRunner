using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioClip jump,slow,wall,refill,burn,invalid,fall,boost;

    public AudioSource audioSource;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void playJump()
    {
        audioSource.clip = jump;
        audioSource.Play();
    }
    public void playSlow()
    {
        audioSource.clip = slow;
        audioSource.Play();
    }
    public void playRefill()
    {
        audioSource.clip = refill;
        audioSource.Play();
    }
    public void playWall()
    {
        audioSource.clip = wall;
        audioSource.Play();
    }
    public void playBoost() {
        audioSource.clip = boost;
        audioSource.Play();
    }
    public void playFall()
    {
        audioSource.clip = fall;
        audioSource.Play();

    }
    public void playInvalid()
    {
        audioSource.clip = invalid;
        audioSource.Play();
    }
    public void playBurn()
    {
        audioSource.clip = burn;
        audioSource.Play();
    }
}
