using Assets.Scripts.Entities.Attack;
using UnityEngine;

public class Player : Mob {
    
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    Transform bulletSpawn;
    [SerializeField]
    private int carDamage = 5;

    private float nextAttackTime;

    public Player() : base(100) { }

    private void Start() {
        SingleShot s = new SingleShot(this.gameObject);
        s.SetPrefab(bulletPrefab);
        s.SetSpawn(bulletSpawn);
        SetAttack(s);
    }

    void Update() {
        if ( Time.timeSinceLevelLoad >= nextAttackTime && attack != null) {
            if ( Input.GetAxis("Fire") > 0 ) {
                Debug.Log("Attack pressed!");
                attack.Attack();
                nextAttackTime = Time.timeSinceLevelLoad + attack.GetCooldownTime();
            }
        }
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
		GameController.instance.EndGame(GameController.EndedCause.DIED);
    }
}
