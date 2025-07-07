using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum weaponType
    {
        Unkown,
        Sword,
        Arrow,
        Wand
    }
    public abstract bool Attack();
}
