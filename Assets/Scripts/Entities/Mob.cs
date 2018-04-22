using Assets.Scripts.Entities.Attack;

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
        currentHP -= damage;
        if ( !isDead && currentHP <= 0 ) {
            isDead = true;
            Death();
        }
    }

    /// <summary>
    /// This is called when a mob dies.
    /// </summary>
    protected virtual void Death() {
        if ( objective != null )
            objective.RemoveEntity(this);
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
