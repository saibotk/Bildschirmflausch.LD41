﻿using UnityEngine;

public abstract class Objective {
    protected Room room;
    protected Player player;
    protected bool activated;
    protected bool finished;

    /// <summary>
    /// Constructs a new Objective instance.
    /// </summary>
    /// <param name="caller"></param>
    public Objective(Room caller) {
        this.room = caller;
    }

    /// <summary>
    /// Handle activation code for a goal.
    /// </summary>
    /// <param name="ply">Player</param>
    public virtual void ActivateGoal(Player ply) {
        if ( activated )
            return;
        activated = true;
        player = ply;
    }

    /// <summary>
    /// Tracks / Updates the progess of a goal.
    /// </summary>
    public abstract void UpdateGoal();

    /// <summary>
    /// Code executed if the goal is reached eg. opening doors.
    /// </summary>
    protected virtual void ReachedGoal() {
        finished = true;
        room.Unlock();
        Debug.Log("[ROOM] Goal reached. Doors will open.");
    }

    /// <summary>
    /// Returns wether the goal was reached or not.
    /// </summary>
    /// <returns></returns>
    public bool GetFinished() {
        return finished;
    }

    public Player GetPlayer() {
        return player;
    }


}
