using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public enum TileType {
        GROUND, WALL, DOOR, ROCK
    }

    Vector2Int center;

    List<Door> doors;
    List<Transform> spawnpoints;

    [SerializeField]
    GameObject doorsRootObject;

    [SerializeField]
    GameObject spawnpointRootObject;

    private Objective objective;

    // Use this for initialization
    void Awake() {
        doors = new List<Door>();
        if ( doorsRootObject != null ) {
            foreach ( Door d in doorsRootObject.GetComponentsInChildren<Door>() ) {
                doors.Add(d);
            }
            //Debug.Log("[ROOMS] Doors: " + doors.Count);
        }
        
        spawnpoints = new List<Transform>();
        if ( spawnpointRootObject != null ) {
            foreach ( Transform t in spawnpointRootObject.GetComponentsInChildren<Transform>() ) {
                if ( t.gameObject != spawnpointRootObject ) {
                    spawnpoints.Add(t);
                }
            }
            //Debug.Log("[ROOMS] Spawnpoints: " + spawnpoints.Count);
        }
        Unlock();
    }

    /// <summary>
    /// Center point in Generation phase.
    /// </summary>
    /// <param name="v"></param>
    public void SetCenter(Vector2Int v) {
        center = v;
    }

    /// <summary>
    /// Returns the Center as global from the generation.  TODO change this 
    /// Is available after Generation and after Start Phase.
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetCenter() {
        return center;
    }

    /// <summary>
    /// Reloads the spawnpoints and doors.
    /// </summary>
    public void Reload() {
        doors = new List<Door>();
        if ( doorsRootObject != null ) {
            foreach ( Door d in doorsRootObject.GetComponentsInChildren<Door>() ) {
                doors.Add(d);
            }
            //Debug.Log("[ROOMS] Doors: " + doors.Count);
        }

        spawnpoints = new List<Transform>();
        if ( spawnpointRootObject != null ) {
            foreach ( Transform t in spawnpointRootObject.GetComponentsInChildren<Transform>() ) {
                if ( t.gameObject != spawnpointRootObject ) {
                    spawnpoints.Add(t);
                }
            }
            //Debug.Log("[ROOMS] Spawnpoints: " + spawnpoints.Count);
        }
        Unlock(); // Ok to do so?
    }

    /// <summary>
    /// Sets the doors root object.
    /// </summary>
    /// <param name="go"></param>
    public void SetDoorsRootObject(GameObject go) {
        doorsRootObject = go;
    }

    /// <summary>
    /// Sets the spawnpoints root object.
    /// </summary>
    /// <param name="go"></param>
    public void SetSpawnPointsRootObject(GameObject go) {
        spawnpointRootObject = go;
    }

    /// <summary>
    /// Locks all doors associated with this room.
    /// </summary>
    public void Lock() {
        foreach ( Door d in doors ) {
            d.Lock();
        }
        Debug.Log("[ROOMS] Locked all doors...");
    }

    /// <summary>
    /// Unlocks all doors associated with this room.
    /// </summary>
    public void Unlock() {
        foreach ( Door d in doors ) {
            d.Unlock();
        }
        Debug.Log("[ROOMS] Unlocked all doors...");
    }

    /// <summary>
    /// Sets the rooms Objective.
    /// </summary>
    /// <param name="obj">Objective</param>
    public void SetObjective(Objective obj) {
        objective = obj;
    }

    /// <summary>
    /// Informs the objective / activates it and ensures that a cleared room is not going to be activated again.
    /// </summary>
    /// <param name="player"></param>
    public void OnPlayerEnter(Player player) {
        if(objective == null) {
            Debug.Log("[ROOMS] This Room has no objective!");
            return;
        }
        if ( objective.GetFinished() ) {
            Debug.Log("[ROOMS] This room has been cleared already.");
            return;
        }
        Debug.Log("[ROOMS] Player activated Objective");
        objective.ActivateGoal(player);
    }

    /// <summary>
    /// Returns the Spawnpoints a room has.
    /// </summary>
    /// <returns></returns>
    public List<Transform> GetSpawnpoints() {
        return spawnpoints;
    }

}
