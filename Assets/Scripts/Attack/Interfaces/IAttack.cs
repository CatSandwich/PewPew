using Enemy;

namespace Attack.Interfaces
{
    public interface IAttack
    {
        float Damage { get; }
        bool ValidateHit(AbstractEnemyScript enemy);
        void OnHit(AbstractEnemyScript enemy);
    }
}
