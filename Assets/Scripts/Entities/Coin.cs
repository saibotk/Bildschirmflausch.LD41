using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.Scripts.Entities {
    [RequireComponent(typeof(Collider2D))]
    public class Coin : Entity {

        void OnTriggerEnter2D(Collider2D bumper) {
            if ( objective == null )
                return;
            Player ply = objective.GetPlayer();
            if ( ply != null && ply.gameObject != null && bumper.gameObject.Equals(ply.gameObject) ) {
                Debug.Log("Collected coin...");
                objective.RemoveEntity(this);
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
