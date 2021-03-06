﻿using Assets.Scripts.Entities.Attack;
using UnityEngine;

public class Player : Mob {

    Rigidbody2D body;
    
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    Transform bulletSpawn;
    //[SerializeField]
    //private int carDamage = 5;

    private float nextAttackTime;

    public Player() : base(100) { }

    private void Start() {
        SingleShot s = new SingleShot(this.gameObject, 6, 0.2f, 20);
        s.SetPrefab(bulletPrefab);
        s.SetSpawn(bulletSpawn);
        body = GetComponent<Rigidbody2D>();
        SetAttack(s);
    }

	void Update() {
        if (Input.GetAxis("Reset") > 0)
        {
			GameController.instance.GetUI().Restart();
        }
        if ( Time.timeSinceLevelLoad >= nextAttackTime && attack != null) {
            if ( Input.GetAxis("Fire") > 0 ) {
                attack.Attack();
                nextAttackTime = Time.timeSinceLevelLoad + attack.GetCooldownTime(); // Todo put in attack()
            }
        }
        // scale particle emissions by speed
        float velocity = body.velocity.magnitude;
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles) {
            ParticleSystem.EmissionModule emission = particle.emission;
			emission.rateOverTime = velocity * (velocity / 4) * 20 + 10;
        }
    }


    /// <summary>
    /// Collision checking. Player is going to die on any collision with a wall.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Collision");
        if ( collision.collider.tag == "wall" ) {
			//InflictDamage(maxHP / 2);
			Death();
		} else if (collision.collider.tag == "rock")
        {
            InflictDamage(maxHP / 10);
            //Death();
        }
        else if ( collision.collider.tag == "Enemy" ) {
            Mob m = collision.collider.GetComponent(typeof(Mob)) as Mob;
            if ( m != null ) {
                //m.InflictDamage(carDamage);
                //InflictDamage(carDamage); // TODO
            }
        }
    }

    /// <summary>
    /// This is called when a Player died.
    /// </summary>
    protected override void Death() {
        Debug.Log("Player died...");
		Destroy(this.gameObject);
		GameController.instance.GetAudioControl().SfxStop(AudioControl.Sfx.slowdriving);
		GameController.instance.GetAudioControl().SfxStop(AudioControl.Sfx.driving);
		GameController.instance.GetAudioControl().SfxPlay(AudioControl.Sfx.explosion);
		GameController.instance.EndGame(GameController.EndedCause.DIED);
    }
}
