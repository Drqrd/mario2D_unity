using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyInterface
{
    public void Enable();
    public void Disable();
    public IEnumerator DeathTimer(string slainBy = "NaN");
}
