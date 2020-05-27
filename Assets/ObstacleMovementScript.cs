using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleMovementScript : MonoBehaviour
{

    [SerializeField] Vector3 movementVector;
    [Range(0, 1)] [SerializeField] float movementFactor;
    [SerializeField] float period;

    Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;
    }

    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }
        float cycle = Time.time / period; //медленно растет от 0 до бесконечности. Чем больше period, тем медленней растет.
        float rawSinWave = Mathf.Sin(cycle); // меняется [-1; 1]
        movementFactor = rawSinWave / 2f + 0.5f; //делаем так, чтобы работало в значениях [0; 1] а не [-1; 1]
        transform.position = startingPos + (movementFactor * movementVector);
    }
}
