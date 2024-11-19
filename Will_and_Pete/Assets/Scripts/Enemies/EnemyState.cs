namespace Assets.Scripts.Enemies
{
    public abstract class EnemyState
    {

        public enum States { NOTHING, GroundPatrol, GroundChase, GroundAttack }
        public States stateName;

        public abstract void Enter();
        public abstract void UpdateState();
        public abstract void FixedUpdateState();
        public abstract void Exit();
        public abstract States CheckExitConditions();

    }
}
