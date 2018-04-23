using Assets.Scripts.Entities.Attack;
using UnityEngine;

public abstract class Mob : Entity {
    protected readonly int maxHP;
    protected int currentHP;
    protected bool isDead;
    protected IAttack attack;

    /// <summary>
    /// Creates a new Mob instance with the given HP.
    /// </summary>
    /// <param name="maxHP"></param>
    public Mob(int maxHP) {
        this.maxHP = maxHP;
        currentHP = maxHP;
        isDead = false;
    }

    public void SetAttack(IAttack attack) {
        this.attack = attack;
    }

    /// <summary>
    /// Inflicts damage to this mob.
    /// </summary>
    /// <param name="damage"></param>
    public void InflictDamage(int damage) {
        Debug.Log(tag + " received " + damage + " Damage");
        currentHP -= damage;
        if ( !isDead && currentHP <= 0 ) {
            isDead = true;
            Death();
		} else if (! isDead) {
			GameController.instance.GetAudioControl().SfxPlay(AudioControl.Sfx.mobattack);
		}
    }

    /// <summary>
    /// This is called when a mob dies.
    /// </summary>
    protected virtual void Death() {
        if ( objective != null )
			objective.RemoveEntity(this);
		GameController.instance.GetAudioControl().SfxPlay(AudioControl.Sfx.explosion);
        ParticleSystem[] pss = GetComponentsInChildren<ParticleSystem>(true);
        foreach ( ParticleSystem ps in pss ) {
            if ( ps.gameObject.name == "despawn" ) {
                GameObject tmp = Instantiate(ps.gameObject);
                tmp.transform.position = transform.position;
                tmp.SetActive(true);
            }
        }
  
        Destroy(gameObject);
    }

    /// <summary>
    /// Heals the mob.
    /// </summary>
    /// <param name="healAmount"></param>
    public void Heal(int healAmount) {
        if ( !isDead )
            currentHP = ( currentHP + healAmount > currentHP ) ? maxHP : currentHP + healAmount;
    }

    /// <summary>
    /// Gets the current HP.
    /// </summary>
    /// <returns></returns>
    public int GetHealth() {
        return currentHP;
    }

    /// <summary>
    /// Gets the maximum HP.
    /// </summary>
    /// <returns></returns>
    public int GetMaxHealth() {
        return maxHP;
    }
}
