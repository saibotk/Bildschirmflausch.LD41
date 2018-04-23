using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishObjective : Objective {

    public FinishObjective(Room caller) : base(caller) { }

    public override void ActivateGoal(Player player) {
        // Player reached the end => win
        base.ActivateGoal(player);
        UpdateGoal();
    }

    public override void UpdateGoal() {
		if ((room.GetCenter() - (Vector2)player.transform.position).magnitude < 2)
			ReachedGoal();
    }

    protected override void ReachedGoal() {
        base.ReachedGoal();
        GameController.instance.EndGame(GameController.EndedCause.WIN);
    }
}
