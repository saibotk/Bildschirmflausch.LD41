using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Attack {
    class SingleShot : IAttack {

        int damage = 12;
        float cooldown = 1;
        int range = 4;
        GameObject owner;
        GameObject bulletPrefab;
        Transform spawn;

        public SingleShot(GameObject owner) {
            this.owner = owner;
        }

        public void SetSpawn(Transform t) {
            spawn = t;
        }

        public void SetPrefab(GameObject bullet) {
            this.bulletPrefab = bullet;
        }

        public void Attack() {
            if ( bulletPrefab == null )
                return;
            Debug.Log("Instantiate Bullet");
            GameObject b = GameObject.Instantiate(bulletPrefab);
            b.transform.rotation = spawn.rotation;
            b.transform.position = spawn.position;
            Bullet bu = b.GetComponent<Bullet>();
            bu.SetDamage(damage);
            bu.SetOwner(owner);
        }

        public float GetCooldownTime() {
            return cooldown;
        }

        public float GetRange() {
            return range;
        }
    }
}
