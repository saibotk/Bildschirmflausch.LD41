using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    [SerializeField]
    private GameObject victim;
    [SerializeField]
    private GameObject agressor;
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private float rotaionSpeedWIP = 1;

    /*
	 * Die destructive dumme deutsche Dungeon Diktator Drifter DLC Debakel Distribution Dokumentations - Druck Datei
	 * */

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if ( victim == null ) {
            //victim = GameController.instance.GetPlayer().gameObject; // TODO testing purpose only!
            return;
        }
        // movement
        agressor.transform.Translate(Vector3.Scale(( victim.transform.position - agressor.transform.position ).normalized, new Vector2(speed, speed)));

        // rotation
        agressor.transform.Rotate(agressor.transform.forward, Vector3.Angle(victim.transform.position - agressor.transform.position, agressor.transform.rotation * new Vector3(1, 1, 1)));
    }

    public void SetVictim(GameObject g) {
        victim = g;
    }
}
