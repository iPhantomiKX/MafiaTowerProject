using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoInspect : Inspect {

	public int collectedAmmo;

	public override void inspect()
	{
		GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<Gun>().CollectedAmmo(collectedAmmo);
		Destroy (this.gameObject);
	}

}
