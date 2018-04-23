using Assets.Scripts.Entities.Attack;
using UnityEngine;

public class Player : Mob {

    Rigidbody2D body;
    
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    Transform bulletSpawn;
    //[SerializeField]
    //private int carDamage = 5;

    private SingleShot singleShot;
    private GatlingGun ggun;

    private float nextAttackTime;

    public Player() : base(100) { }

    private void Start() {
        SingleShot s = new SingleShot(this.gameObject);
        s.SetPrefab(bulletPrefab);
        s.SetSpawn(bulletSpawn);
        singleShot = s;
        GatlingGun g = new GatlingGun(this.gameObject);
        g.SetPrefab(bulletPrefab);
        g.SetSpawn(bulletSpawn);
        ggun = g;
        body = GetComponent<Rigidbody2D>();
        SetAttack(s);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.G)) {
            if(attack != ggun) {
                attack = ggun;
                Debug.Log("Switched to GatlingGun");
            } else {
                attack = singleShot;
                Debug.Log("Switched to SingleShot");
            }
            
        }
        if ( Time.timeSinceLevelLoad >= nextAttackTime && attack != null) {
            if ( Input.GetAxis("Fire") > 0 ) {
                attack.Attack();
                nextAttackTime = Time.timeSinceLevelLoad + attack.GetCooldownTime(); // Todo put in attack()
            }
        }
        // scale particle emissions by speed
        float velocity = body.velocity.magnitude;
        ParticleSystem.EmissionModule emission = GetComponentInChildren<ParticleSystem>().emission;
        emission.rateOverTime = velocity * (velocity / 2) * 20 + 20;
    }


    /// <summary>
    /// Collision checking. Player is going to die on any collision with a wall.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Collision");
        if ( collision.collider.tag == "wall" ) {
            Death();
        } else if ( collision.collider.tag == "Enemy" ) {
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
