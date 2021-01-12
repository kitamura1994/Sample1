using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Commons.HABITAT habitatType = Commons.HABITAT.ROAD;
    [SerializeField] GameObject standingFrame;

    public bool CheckPuttable()
    {
        if (standingFrame == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public Commons.HABITAT GetHabitatType()
    {
        return habitatType;
    }

    public void SetStandingFrame(GameObject frame)
    {
        standingFrame = frame;
    }

}


