using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Attack {
    [Serializable]
    class MeleeAttack : IAttack {

        int damage = 10;
        float cooldown = 1;
        float range = 2f;
        GameObject owner;

        public MeleeAttack(GameObject owner) {
            this.owner = owner;
        }

        public void Attack() {
            RaycastHit2D[] hits = Physics2D.RaycastAll(owner.transform.position, owner.transform.localRotation * Vector3.up, range);
            List<RaycastHit2D> rh = new List<RaycastHit2D>(hits);
            RaycastHit2D hit = rh.Find(x => x.fraction != 0);
            Mob m = null;
            if (hit.collider != null && hit.collider.gameObject != null) {
                m = hit.collider.gameObject.GetComponent(typeof(Mob)) as Mob;
            }
            Debug.Log(m.tag);
            if ( m != null && m.tag != owner.tag) {
                m.InflictDamage(damage);
            }
            
            // Todo animation?
        }

        public float GetCooldownTime() {
            return cooldown;
        }

        public float GetRange() {
            return range;
        }
    }
}
