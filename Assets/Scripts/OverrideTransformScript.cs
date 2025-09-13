using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public class OverrideTransformScript : NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
