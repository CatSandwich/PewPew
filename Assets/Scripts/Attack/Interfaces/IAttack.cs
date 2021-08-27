using Enemy;

namespace Attack.Interfaces
{
    public interface IAttack
    {
        float Damage { get; }
        bool ValidateHit(EnemyScript enemy);
        void OnHit(EnemyScript enemy);
    }
}
