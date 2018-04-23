using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Entities.Attack;

namespace Assets.Scripts.Entities {
    class Scorpion : Enemy {
        public Scorpion() : base(30) {
            
        }

        protected override void Start() {
            base.Start();
            SetAttack(new MeleeAttack(this.gameObject));
        }
    }
}
