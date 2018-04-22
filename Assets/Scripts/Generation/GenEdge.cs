using System;
using UnityEngine;

public class GenEdge {
    public GenVertex r1, r2;
    public double dist;

    public GenEdge(GenVertex r1, GenVertex r2) {
        this.r1 = r1;
        this.r2 = r2;
        dist = r1.r.Distance(r2.r);
    }
}

