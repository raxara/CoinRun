using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController INSTANCE;

    private void Awake()
    {
        INSTANCE = this;
    }

}
