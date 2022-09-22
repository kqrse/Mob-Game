using System;
using System.Collections;
using System.Collections.Generic;
using Globals;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour {
    public List<BaseMinion> minionsList = new();
    protected const float MovementTolerance = 0.05f;
}