public abstract class Mob : Entity {
    readonly int maxHP;
    int currentHP;
    bool isDead;
    int damage;

    /// <summary>
    /// Creates a new Mob instance with the given HP.
    /// </summary>
    /// <param name="maxHP"></param>
    public Mob(int maxHP) {
        this.maxHP = maxHP;
        currentHP = maxHP;
        isDead = false;
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
    /// Sets the damage value of a mobs attack.
    /// </summary>
    /// <param name="dmg"></param>
    public void SetDamage(int dmg) {
        damage = dmg;
    }

    /// <summary>
    /// Gets the damage value og a mobs attack.
    /// </summary>
    /// <returns></returns>
    public int GetDamage() {
        return damage;
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
