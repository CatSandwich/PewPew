using UnityEngine;

namespace Attack.Interfaces
{
    public interface ICooldownAttack : IAttack
    {
        float Cooldown { get; }
        int Id { get; }
    }
}
