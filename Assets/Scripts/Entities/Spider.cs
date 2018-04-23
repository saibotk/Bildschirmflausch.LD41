using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Entities.Attack;

namespace Assets.Scripts.Entities
{
    class Spider : Enemy
    {
        public Spider() : base(45)
        {

        }

        private void Start()
        {
            SetAttack(new SingleShot(this.gameObject));
        }
    }
}
