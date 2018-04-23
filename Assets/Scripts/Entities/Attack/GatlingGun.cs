using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Entities.Attack {
    class GatlingGun : SingleShot {
        public GatlingGun(GameObject owner) : base(owner) {
            damage = 5;
            cooldown = 0.1f;
            speed = 20;
        }

    }
}
