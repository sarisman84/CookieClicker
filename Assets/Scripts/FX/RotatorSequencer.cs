using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RotatorSequencer : MonoBehaviour
{
    public List<Vector3> rotationPath;
    public float rotationSpeed;

    private int currentPath;
    private float curTime;
    private float curTimeLimit;

    // Start is called before the first frame update
    void Start()
    {
        currentPath = 0;
        curTime = 0;
        curTimeLimit = Random.Range(3, 6);
    }

    private void OnEnable()
    {
        currentPath = 0;
        curTime = 0;
        curTimeLimit = Random.Range(3, 6);
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime >= curTimeLimit)
        {
            currentPath = Random.Range(0, rotationPath.Count);
            curTimeLimit = Random.Range(6, 12);
            curTime = 0;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotationPath[currentPath]),
            rotationSpeed * Time.deltaTime);
    }
}