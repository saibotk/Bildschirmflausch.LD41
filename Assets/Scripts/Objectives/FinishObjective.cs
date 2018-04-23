using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishObjective : EntityObjective {

    public FinishObjective(Room caller, GameObject go) : base(caller, new List<GameObject> { go }) { }

    protected override void ReachedGoal() {
        base.ReachedGoal();
        GameController.instance.EndGame(GameController.EndedCause.WIN);
    }
}
