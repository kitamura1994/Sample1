using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCheckStanding : MonoBehaviour
{
    [SerializeField]Tile tileScript;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Friend"))
        {
            tileScript.SetStandingFrame( other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Friend"))
        {
            tileScript.SetStandingFrame(null);
        }
    }
}
