//using System;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsRotateThrust = 100f;
    [SerializeField] float rcsEnguineThrust = 250f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip levelCompete;
    [SerializeField] AudioClip destroy;


    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;
            case "Fuel":
                print("You got some fuel!");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        print("You are fucking die!");
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(destroy, 0.5f);
        //StartCoroutine(FadeAudioSource.StartFade(audioSource, 0.3f, 0, mainEngine)); заглушаем звук после смерти
        Invoke("DeadLevel", 1.5f);
    }

    private void StartSuccessSequence()
    {
        print("Level complete!");
        Invoke("LoadNewLevel", 1.5f); //todo работает только для 2 уровней
        audioSource.Stop();
        audioSource.PlayOneShot(levelCompete);
        state = State.Transcending;
    }

    private void RespondToThrustInput()
    {

        //rigidbody Thrust

        float RocketSpeed = rcsEnguineThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space)) //can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * RocketSpeed);
        }

        ThrustSound();
    }

    private void ThrustSound()
    {
        bool isThrusting = Input.GetKey(KeyCode.Space); //thrusting
        bool isPlaying = audioSource.isPlaying;

        if (isThrusting && !isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
            //StartCoroutine(FadeAudioSource.StartFade(audioSource, 0.3f, 1)); включаем звук двигателя
        }
        else if (isThrusting && isPlaying)
        {
        }
        else
        {
            audioSource.Stop();
            //StartFade(mainEngine, 0.3f, 0); заглушаем звук двигателя
        }
    }

    private void RespondToRotateInput()
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

    private void DeadLevel()
    {
        SceneManager.LoadScene(0);
        state = State.Alive;
    }

    private void LoadNewLevel()
    {
        SceneManager.LoadScene(1);
        state = State.Alive;
    }

    /* метод для заглушения звука
    private void StartFade(AudioSource audio, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audio.volume;

        if (audio.volume == 0)
        {
            audio.Play();
        }

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audio.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            if (audio.volume == 0)
            {
                audio.Stop();
            }
        }
    }  
    */
}
