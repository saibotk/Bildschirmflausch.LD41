using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
    public enum Entities {
        SCORPION,
        BUG,
        COIN,
        SPIDER
    }

    protected EntityObjective objective;

    /// <summary>
    /// Sets the Objective this Entity is associated with.
    /// </summary>
    /// <param name="obj">Objective</param>
    public void SetObjective(EntityObjective obj) {
        objective = obj;
    }
}
