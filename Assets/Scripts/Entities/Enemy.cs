using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Entities.Attack;

namespace Assets.Scripts.Entities {
    class Enemy : Mob {

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

        void Update() {
            if ( victim == null || attack == null ) {
                return;
            }

            if ( Time.timeSinceLevelLoad >= nextAttackTime ) {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.position - victim.transform.position, attack.GetRange());
                if ( hit.collider.gameObject == victim ) {
                    attack.Attack();
                    nextAttackTime = Time.timeSinceLevelLoad + attack.GetCooldownTime();
                }
            }

            Vector3 distanceToEnemy = victim.transform.position - gameObject.transform.position;

            if(distanceToEnemy.magnitude < attack.GetRange()) {
                return;
            }

            // movement
            body.velocity = new Vector2(distanceToEnemy.normalized.x, distanceToEnemy.normalized.y) * speed;

            //rotation
            Vector3 localRotation = gameObject.transform.localRotation * Vector3.up;
            float angleToRotate = Mathf.Round(Vector3.SignedAngle(localRotation, distanceToEnemy.normalized, Vector3.forward));
            gameObject.transform.Rotate(0, 0, angleToRotate * rotationSpeed);
        }

        public void SetVictim(GameObject g) {
            victim = g;
        }
    }
}
