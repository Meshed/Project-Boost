using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody _rigidBody;
    AudioSource _audioSource;

    enum RocketRotation
    {
        Left,
        Right
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            _rigidBody.AddRelativeForce(Vector3.up);

            PlayRocketThrustSound();
        }
        else
        {
            _audioSource.Stop();
        }

        if(Input.GetKey(KeyCode.A))
        {
            RotateRocket(RocketRotation.Left);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            RotateRocket(RocketRotation.Right);
        }
    }

    private void RotateRocket(RocketRotation rocketRotation)
    {
        switch(rocketRotation)
        {
            case RocketRotation.Left:
                transform.Rotate(Vector3.forward);
                break;
            case RocketRotation.Right:
                transform.Rotate(-Vector3.forward);
                break;
        }
    }

    private void PlayRocketThrustSound()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
}
