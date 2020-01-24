using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody _rigidBody;
    AudioSource _audioSource;

    [SerializeField]float _rcsThrust = 200f;
    [SerializeField]float _mainThrust = 100f;

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

    private void OnCollisionEnter(Collision collision) 
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                SceneManager.LoadScene(1);
                break;
            default:
                SceneManager.LoadScene(0);
                break;
        }
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
