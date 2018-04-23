using Assets.Scripts.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objective, where the goal is to "remove" all Entitys, how is defined by the Entity itself.
/// </summary>
public class EntityObjective : Objective {
    List<GameObject> prefabList;
    List<GameObject> entityList;

    /// <summary>
    /// Creates a new instance of an EntityObjective.
    /// </summary>
    /// <param name="caller"></param>
    /// <param name="prefabList"></param>
    public EntityObjective(Room caller, List<GameObject> prefabList) : base(caller) {
        this.entityList = new List<GameObject>();
        this.prefabList = prefabList;
    }

    /// <summary>
    /// Handle activation code for a goal.
    /// </summary>
    /// <param name="ply">Player</param>
    public override void ActivateGoal(Player ply) {
        if ( activated )
            return;
        base.ActivateGoal(ply);

		List<Transform> spawnPointList = room.GetSpawnpoints();
        if (spawnPointList.Count == 0) {
            ReachedGoal();
            return;
        }

        foreach ( GameObject i in prefabList ) {
            Debug.Log("[ROOMS] Spawning Entity...");
            if(i == null || player == null) {
                Debug.Log("[ROOMS] Failed.. Entity not set in GameController!");
                return;
            }

            GameObject tempObject = UnityEngine.Object.Instantiate(i);
            tempObject.transform.position = spawnPointList[Random.Range(0, spawnPointList.Count)].position;
            tempObject.GetComponent<Enemy>().SetVictim(player.gameObject);
            tempObject.GetComponent<Enemy>().SetObjective(this);
            entityList.Add(tempObject);
        }

        room.Lock();
    }

    /// <summary>
    /// Removes an Entity from the list. And checks if the goal was reached.
    /// </summary>
    /// <param name="e">Entity</param>
    public void RemoveEntity(Entity e) {
        if ( e == null )
            return;
        entityList.Remove(e.gameObject);
        UpdateGoal();
    }

    /// <summary>
    /// Tracks / Updates the progess of a goal.
    /// </summary>
    public override void UpdateGoal() {
        // Goal:
        if ( entityList.Count == 0 )
            ReachedGoal();
    }
}