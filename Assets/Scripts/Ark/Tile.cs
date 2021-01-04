using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public bool isEnterable = true;
    InputManager inputMng;

    private void Start()
    {
        inputMng = GameObject.FindWithTag("InputManager").GetComponent<InputManager>();
    }
    public void DragEnd_PUTFRAME()
    {

    }

}
