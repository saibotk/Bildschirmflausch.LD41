using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Entities.Attack;

namespace Assets.Scripts.Entities {

    public class Bug : Enemy {

        public Bug() : base(15) {

        }

        private void Start() {
            SetAttack(new MeleeAttack(this.gameObject));
        }
    }
}
