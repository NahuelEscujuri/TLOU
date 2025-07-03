using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyDirection : BodyDirection
{
    protected override void Awake()
    {
        base.Awake();
        ChangeRedirectTransform(Camera.main.transform);
    }
}
