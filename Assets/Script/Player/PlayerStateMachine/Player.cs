using System;
using Script.Player.Data;
using Script.Player.PlayerStates;
using UnityEngine;

namespace Script.Player.PlayerStateMachine
{
    public class Player : MonoBehaviour
    {
        #region State Variables
        // By declaring the state machine, we can access all the function
        // including changing or initializing different states
        private PlayerStateMachine StateMachine { get; set; }
        
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }

        [SerializeField] private PlayerData playerData;
        #endregion
        
        
        private void Awake()
        {
            StateMachine = new PlayerStateMachine();

            IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
            MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        }

        private void Start() => StateMachine.Initialize(IdleState);
        private void Update() => StateMachine.CurrentState.LogicUpdate();
    }
}