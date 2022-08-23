public interface IHitable
{
    bool Absorb(float damageMultiplier);

    void StunDamage(float damage);

    float RemainingAbsorb();
}
