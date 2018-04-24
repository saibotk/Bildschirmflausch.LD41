using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartObjective : Objective {
    GameObject playerPrefab;

    /// <summary>
    /// Creates a new StartObjective instance.
    /// </summary>
    /// <param name="room"></param>
    /// <param name="playerPrefab"></param>
    public StartObjective(Room room, GameObject playerPrefab) : base(room) {
        this.playerPrefab = playerPrefab;
    }

    // TODO not as good as possible. Should Objectives ActivateGoal method require a player obj?
    /// <summary>
    /// Handle activation code for a goal.
    /// </summary>
    /// <param name="ply">Player is ignored </param>
    public override void ActivateGoal(Player player) {
        if ( activated )
            return;
        if ( room.GetSpawnpoints().Count > 0 ) {
            GameObject ply = GameObject.Instantiate(playerPrefab);
            ply.transform.position = room.GetSpawnpoints()[0].position;
            player = ply.GetComponent<Player>();
            base.ActivateGoal(player);
            UpdateGoal();
        }
    }

    /// <summary>
    /// Code executed if the goal is reached eg. opening doors.
    /// </summary>
    public override void UpdateGoal() {
        ReachedGoal();
    }
}
