﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Attack {
    class SingleShot : IAttack {

        protected int damage = 12;
        protected float cooldown = 1;
        protected int range = 4;
        protected float speed = 10;
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
            if ( bulletPrefab == null ) {
                Debug.Log("SingleShot: No Prefab defined for Bullet!");
                return;
            }
            GameObject b = UnityEngine.Object.Instantiate(bulletPrefab);
            b.transform.rotation = spawn.rotation;
            b.transform.position = spawn.position;
            Bullet bu = b.GetComponent<Bullet>();
            bu.SetDamage(damage);
            bu.SetSpeed(speed);
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
