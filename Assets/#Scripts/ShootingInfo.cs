using System;
using UnityEngine;

[Serializable]
public struct ShootingInfo
{
    public float Damage;
    public float FireRate;
    public float Range;
    public GameObject ProjectilePrefab;
    public GameObject ShootParticle;
    public Transform[] ProjectileExitPoints;
}