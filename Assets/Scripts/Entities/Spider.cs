﻿using System;
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

        protected override void Start()
        {
            SetAttack(new SingleShot(this.gameObject));
        }
    }
}
