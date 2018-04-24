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

        public Spider() : base(15) {

        }

        protected override void Start() {
            SingleShot s = new SingleShot(this.gameObject, 25, 4, 2);
			s.SetRange(10);
            s.SetPrefab(bullet);
            s.SetSpawn(bulletSpawn);
			SetAttack(s);
            base.Start();
        }
    }
}
