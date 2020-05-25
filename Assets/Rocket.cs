using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsRotateThrust = 100f;
    [SerializeField] float rcsEnguineThrust = 250f;

    Rigidbody rigidBody;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }


    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;
            case "Fuel":
                print("You got some fuel!");
                break;
            case "Finish":
                print("Level complete!");
                break;
            default:
                print("You are fucking die!");
                //Make Rocket explode
                break;
        }
    }
    private void Thrust()
    {

        //rigidbody Thrust

        float RocketSpeed = rcsEnguineThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space)) //can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * RocketSpeed);
        }

        //audio Thrust

        bool isThrusting = Input.GetKey(KeyCode.Space); //thrusting
        bool stopThrusting = Input.GetKeyUp(KeyCode.Space);
        bool isPlaying = audioSource.isPlaying;

        if (isThrusting && !isPlaying)
        {
            StartCoroutine(FadeAudioSource.StartFade(audioSource, 0.3f, 1));
        }
        else if (isThrusting && isPlaying)
        { 
        }
        else if (stopThrusting)
        {
            StartCoroutine(FadeAudioSource.StartFade(audioSource, 0.3f, 0));
        }
    }

    private void Rotate()
    {

        rigidBody.freezeRotation = false;

        
        float rotationSpeed = rcsRotateThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        rigidBody.freezeRotation = true;
    }

    public static class FadeAudioSource
    {

        public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = audioSource.volume;

            if (audioSource.volume == 0)
            {
                audioSource.Play();
            }

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                if (audioSource.volume == 0)
                {
                    audioSource.Stop();
                }
                yield return null;
            }
            yield break;


        }
    }

}
