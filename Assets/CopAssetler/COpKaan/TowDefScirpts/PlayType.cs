using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayType : MonoBehaviour
{
    public enum playType { towDef, drone,fps,driving }

    public static playType type;

    private void Awake()
    {
        type = playType.towDef;
    }

}
