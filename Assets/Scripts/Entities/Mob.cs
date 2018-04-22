using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mob : Entity {
	readonly int maxHP;
	int currentHP;
	bool isDead;
    int damage;

	// Constructor
	public Mob(EntityObjective referringObjective, int maxHP) : base(referringObjective)
	{
		this.maxHP = maxHP;
		currentHP = maxHP;

		isDead = false;
	}

	// inflicts damage to this mob
	public void InflictDamage(int damage)
	{
		currentHP -= damage;
		if (!isDead && currentHP <= 0) 
		{
			base.Kill ();
			isDead = true;
		}
	}

	// Heals the mob
	public void Heal(int healAmount)
	{
		if (!isDead)
			currentHP = (currentHP + healAmount > currentHP) ? maxHP : currentHP + healAmount;
	}

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    public int GetDamage()
    {
        return damage;
    }
}
