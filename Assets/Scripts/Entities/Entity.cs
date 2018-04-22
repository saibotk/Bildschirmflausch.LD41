﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
    protected EntityObjective objective;

    /// <summary>
    /// Sets the Objective this Entity is associated with.
    /// </summary>
    /// <param name="obj">Objective</param>
    public void SetObjective(EntityObjective obj) {
        objective = obj;
    }
}
