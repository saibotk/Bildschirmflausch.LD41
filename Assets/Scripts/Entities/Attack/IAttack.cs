namespace Assets.Scripts.Entities.Attack {
    public interface IAttack {
        void Attack();
        float GetRange();
        float GetCooldownTime();
    }
}
