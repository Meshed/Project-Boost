﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody _rigidBody;
    AudioSource _audioSource;

    [SerializeField]float _rcsThrust = 200f;
    [SerializeField]float _mainThrust = 100f;

    enum State
    {
        Alive,
        Dying,
        Transending
    }
    State state;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();

        state = State.Alive;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if(state != State.Alive) { return; }

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transending;
                Invoke("LoadNextScene", 1f);
                break;
            default:
                state = State.Dying;
                StopRocketThrustSound();
                Invoke("LoadFirstScene", 1f);
                break;
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _rigidBody.AddRelativeForce(Vector3.up * _mainThrust);

            PlayRocketThrustSound();
        }
        else
        {
            StopRocketThrustSound();
        }
    }
    private void PlayRocketThrustSound()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
    private void StopRocketThrustSound()
    {
        if(_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }

    private void Rotate()
    {
        float rotationThisFrame = _rcsThrust * Time.deltaTime;

        _rigidBody.freezeRotation = true;

        if(Input.GetKey(KeyCode.A))
        {
            RotateLeft(rotationThisFrame);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            RotateRight(rotationThisFrame);
        }

        _rigidBody.freezeRotation = false;
    }
    private void RotateLeft(float rotationThisFrame)
    {
        transform.Rotate(Vector3.forward * rotationThisFrame);
    }
    private void RotateRight(float rotationThisFrame)
    {
        transform.Rotate(-Vector3.forward * rotationThisFrame);
    }
}
