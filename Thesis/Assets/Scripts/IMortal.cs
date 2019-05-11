using System;

public interface IMortal
{
    event Action onDead;
}

public interface IDamagable : IMortal
{
    void TakeDamage(float damage, DamageType damageType);
}
