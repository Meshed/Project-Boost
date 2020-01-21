using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody _rigidBody;
    AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _rigidBody.AddRelativeForce(Vector3.up);

            PlayRocketThrustSound();
        }
        else
        {
            _audioSource.Stop();
        }
    }
    private void PlayRocketThrustSound()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    private void Rotate()
    {
        _rigidBody.freezeRotation = true;

        if(Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if(Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }

        _rigidBody.freezeRotation = false;
    }
    private void RotateLeft()
    {
        transform.Rotate(Vector3.forward);
    }
    private void RotateRight()
    {

        transform.Rotate(-Vector3.forward);
    }
}
