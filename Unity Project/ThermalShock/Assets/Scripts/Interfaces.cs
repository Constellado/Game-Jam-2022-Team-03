using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Attackable
{
    void Hit(float heat, bool isMelee);
}
