using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_SCRIPT : MonoBehaviour
{
    public SO_InputReader inputReader;

    private void Start()
    {
        inputReader.PrimaryMoveEvent += MoveEvent;

    }

    private void MoveEvent(Vector2 vector)
    {
        Debug.Log(vector);
    }
}
