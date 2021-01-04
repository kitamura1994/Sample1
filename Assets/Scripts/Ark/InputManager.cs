using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public bool isTile = false;

    public GameObject currentRayHitObject { get; private set; }
    void Update()
    {
        if (isTile) RayCastForTile();
    }

    private void RayCastForTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask(new string[] { "Tile" });
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            currentRayHitObject = hit.collider.gameObject;
        }
    }

    public void RemoveCurrentRayHitObject()
    {
        currentRayHitObject = null;
    }
}


