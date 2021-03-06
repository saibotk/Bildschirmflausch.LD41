﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Entities.Attack;

namespace Assets.Scripts.Entities {
    public class Enemy : Mob {

        [SerializeField]
        private float speed = 1;
        [SerializeField]
        private float rotationSpeed = 1;
        [SerializeField]
        protected GameObject victim;
        [SerializeField]
        private Rigidbody2D body;
        private float nextAttackTime;

        public Enemy(int mHP) : base(mHP) {
            
        }

        protected virtual void Start() {
            ParticleSystem[] pss = GetComponentsInChildren<ParticleSystem>();
            foreach(ParticleSystem ps in pss) {
                if ( ps.gameObject.name == "spawn" ) {
                    ps.Play();
                }
            }
            if ( objective != null ) {
                Player ply = objective.GetPlayer();
                if ( ply != null )
                    victim = ply.gameObject;
            }
            
            if (attack != null)
				nextAttackTime = Time.timeSinceLevelLoad + attack.GetCooldownTime() * (UnityEngine.Random.value + 0.5f);
        }

        void Update() {
            if ( victim == null || attack == null ) {
                if ( objective != null ) {
                    Player ply = objective.GetPlayer();
                    if ( ply != null )
                        victim = ply.gameObject;
                }
                return;
            }

            if ( Time.timeSinceLevelLoad >= nextAttackTime ) {
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.localRotation * Vector3.up, attack.GetRange());
                List<RaycastHit2D> rh = new List<RaycastHit2D>(hits);
                RaycastHit2D hit = rh.Find(x => x.fraction != 0);
                if ( hit.collider != null && hit.collider.gameObject == victim ) {
                    Debug.Log("Attacking Player!!!");
					attack.Attack();
                    nextAttackTime = Time.timeSinceLevelLoad + attack.GetCooldownTime() * (UnityEngine.Random.value + 0.25f);
                }
            }

            Vector3 distanceToEnemy = victim.transform.position - gameObject.transform.position;
            //rotation
            Vector3 localRotation = gameObject.transform.localRotation * Vector3.up;
            float angleToRotate = Mathf.Round(Vector3.SignedAngle(localRotation, distanceToEnemy.normalized, Vector3.forward));
            gameObject.transform.Rotate(0, 0, angleToRotate * rotationSpeed);

            if ( distanceToEnemy.magnitude < attack.GetRange() ) {
                return;
            }
            // movement
            body.velocity = new Vector2(distanceToEnemy.normalized.x, distanceToEnemy.normalized.y) * speed;

            
        }
    }
}
