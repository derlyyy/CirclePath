using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public GameObject trap;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCheck"))
        {
            trap.GetComponent<Trap>().Activate();
        }
    }
}
