﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float _rcsThrust = 200f;
    [SerializeField] float _mainThrust = 100f;
    [SerializeField] float _LevelLoadDelay = 1f;

    [SerializeField] AudioClip _mainEngine;
    [SerializeField] AudioClip _rocketExplosion;
    [SerializeField] AudioClip _levelCompleteChime;

    [SerializeField] ParticleSystem _mainEngineParticles;
    [SerializeField] ParticleSystem _rocketExplosionParticles;
    [SerializeField] ParticleSystem _levelCompleteChimeParticles;

    Rigidbody _rigidBody;
    AudioSource _audioSource;
    bool _isCollisionEnabled = true;
    int _currentSceneIndex = 0;

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
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            if(Debug.isDebugBuild)
            {
                HandleDebugKeys();
            }

            RespondToThrustInput();
            Rotate();
        }
    }

    private void HandleDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _isCollisionEnabled = !_isCollisionEnabled;
        }
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if(state != State.Alive || _isCollisionEnabled == false) { return; }

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transending;
                PlayLevelCompleteChime();
                PlayLevelCompleteParticles();
                Invoke("LoadNextScene", _LevelLoadDelay);
                break;
            default:
                state = State.Dying;
                PlayRocketExplosion();
                PlayRocketExplosionParticles();
                StopRocketThrustParticles();
                Invoke("ReloadCurrentScene", _LevelLoadDelay);
                break;
        }
    }

    private void LoadNextScene()
    {
        if(_currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(_currentSceneIndex + 1);
        }
    }
    private void ReloadCurrentScene()
    {
        SceneManager.LoadScene(_currentSceneIndex);
    }
    private void PlayLevelCompleteChime()
    {
        if(_levelCompleteChime != null)
        {
            _audioSource.PlayOneShot(_levelCompleteChime);
        }
    }
    private void PlayLevelCompleteParticles()
    {
        _levelCompleteChimeParticles.Play();
    }
    private void PlayRocketExplosion()
    {
        if(_rocketExplosion != null)
        {
            _audioSource.PlayOneShot(_rocketExplosion, 0.5f);
        }
    }
    private void PlayRocketExplosionParticles()
    {
        _rocketExplosionParticles.Play();
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
            PlayRocketThrustSound();
            PlayRocketThrustParticles();
        }
        else
        {
            StopRocketThrustSound();
            StopRocketThrustParticles();
        }
    }
    private void ApplyThrust()
    {
        _rigidBody.AddRelativeForce(Vector3.up * _mainThrust * Time.deltaTime);
    }
    private void PlayRocketThrustSound()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(_mainEngine);
        }
    }
    private void StopRocketThrustSound()
    {
        if(_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }

    private void PlayRocketThrustParticles()
    {
        if(_mainEngineParticles.isPlaying == false)
        {
            _mainEngineParticles.Play();
        }
    }
    private void StopRocketThrustParticles()
    {
        _mainEngineParticles.Stop();
    }

    private void Rotate()
    {
        float rotationThisFrame = _rcsThrust * Time.deltaTime;

        if(Input.GetKey(KeyCode.A))
        {
            RotateLeft(rotationThisFrame);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            RotateRight(rotationThisFrame);
        }
    }
    private void RotateLeft(float rotationThisFrame)
    {
        _rigidBody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame);
        _rigidBody.freezeRotation = false;
    }
    private void RotateRight(float rotationThisFrame)
    {
        _rigidBody.freezeRotation = true;
        transform.Rotate(-Vector3.forward * rotationThisFrame);
        _rigidBody.freezeRotation = false;
    }
}
