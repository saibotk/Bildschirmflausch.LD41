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
            GameObject b = UnityEngine.Object.Instantiate(bulletPrefab);
            b.transform.rotation = spawn.rotation;
            b.transform.position = spawn.position;
            Bullet bu = b.GetComponent<Bullet>();
            bu.SetDamage(damage);
            bu.SetOwner(owner);
			GameController.instance.GetAudioControl().SfxPlay(AudioControl.Sfx.shoot);
        }

        public float GetCooldownTime() {
            return cooldown;
        }

        public float GetRange() {
            return range;
        }
    }
}
