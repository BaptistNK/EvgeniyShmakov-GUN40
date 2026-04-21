using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    GameObject ball;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(ball);
    }
}
