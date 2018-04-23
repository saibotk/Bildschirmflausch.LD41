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

        public Bug() : base(15) {

        }

        private void Start() {
            SingleShot s = new SingleShot(this.gameObject);
            s.SetPrefab(bullet);
            s.SetSpawn(bulletSpawn);
            SetAttack(s);
        }
    }
}
