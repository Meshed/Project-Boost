using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 _movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float _period = 2f;

    [Range(0,1)]
    [SerializeField] float _movementFactor = 0f; // 0 for not moved, 1 for fully moved
    
    Vector3 _startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        const float tau = Mathf.PI * 2;
        float cycles = Time.time / _period;
        float rawSinWave = Mathf.Sin(cycles * tau);

        _movementFactor = rawSinWave / 2f + 0.5f;

        Vector3 offset = _movementVector * _movementFactor;

        transform.position = _startingPosition + offset;
    }
}
