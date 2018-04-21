using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective {
	protected Room objectiveCaller;

	// Constructor
	public Objective(Room objectiveCaller)
	{
		this.objectiveCaller = objectiveCaller;
	}

	// Activates the objective to start progresstracking
	public virtual void Activate(){}
}
