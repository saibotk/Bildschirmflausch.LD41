using System;
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
            RaycastHit2D hit = Physics2D.Raycast(owner.transform.position, owner.transform.localRotation * Vector3.up, range);
            Mob m = hit.collider.gameObject.GetComponent(typeof(Mob)) as Mob;
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
