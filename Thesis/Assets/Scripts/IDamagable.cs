public enum DamageType
{
    Sharp,
}

public interface IDamagable : IMortal
{
    void TakeDamage(float damage, DamageType damageType);
}
