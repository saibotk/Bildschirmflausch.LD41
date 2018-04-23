using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Entities.Attack;

namespace Assets.Scripts.Entities {

    public class Bug : Enemy {
        [SerializeField]
        private Transform bulletSpawn;
        [SerializeField]
        private GameObject bullet;

        public Bug() : base(5) {

        }

        protected override void Start() {
            base.Start();
            SingleShot s = new SingleShot(this.gameObject);
            s.SetCooldown(3);
            s.SetPrefab(bullet);
            s.SetSpawn(bulletSpawn);
            SetAttack(s);
        }
    }
}
