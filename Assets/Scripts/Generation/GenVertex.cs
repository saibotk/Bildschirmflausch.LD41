using System;
using UnityEngine;

public class GenVertex {

    public GenRoom r;
    public float value;

    public GenVertex(GenRoom r) {
        this.r = r;
        value = float.PositiveInfinity;
    }
}
