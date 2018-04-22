public abstract class Objective {
    protected Room room;
    protected Player player;
    bool activated;
    bool finished;

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
    }

    /// <summary>
    /// Returns wether the goal was reached or not.
    /// </summary>
    /// <returns></returns>
    public bool GetFinished() {
        return finished;
    }


}
