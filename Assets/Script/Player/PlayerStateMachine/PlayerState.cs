using Script.Player.Data;

namespace Script.Player.PlayerStateMachine
{
    public class PlayerState
    {
        protected Player Player;
        protected PlayerStateMachine StateMachine;
        private string _animBoolName;
        private PlayerData PlayerData;

        public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
        {
            Player = player;
            StateMachine = stateMachine;
            PlayerData = playerData;
            _animBoolName = animBoolName;
        }

        public virtual void Enter()
        {
            
        }

        //Update
        public virtual void LogicUpdate()
        {
            
        }
        
        //Fixed Update
        public virtual void PhysicsUpdate()
        {
            
        }

        public virtual void Exit()
        {
            
        }
    }
}