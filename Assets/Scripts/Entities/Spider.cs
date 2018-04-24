using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Entities.Attack;

namespace Assets.Scripts.Entities {

    public class Spider : Enemy {
        [SerializeField]
        private Transform bulletSpawn;
        [SerializeField]
        private GameObject bullet;

        public Spider() : base(25) {

        }

        protected override void Start() {
            base.Start();
            SingleShot s = new SingleShot(this.gameObject);
            s.SetCooldown(5);
            s.SetPrefab(bullet);
            s.SetSpawn(bulletSpawn);
            SetAttack(s);
        }
    }
}
