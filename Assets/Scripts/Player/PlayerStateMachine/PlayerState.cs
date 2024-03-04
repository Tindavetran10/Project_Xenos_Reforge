using CoreSystem;
using CoreSystem.CoreComponents;
using Player.Data;
using UnityEngine;

namespace Player.PlayerStateMachine
{
     public class PlayerState
     {
          private readonly Core _core;

          protected readonly Player Player;
          protected readonly PlayerStateMachine StateMachine;
          protected readonly PlayerData PlayerData;
     
          protected bool IsAnimationFinished;
          protected bool IsAnimationCancel;
          protected bool IsExitingState;
          protected float StartTime;

          private readonly string _animBoolName;
          
          protected Movement Movement => _movement ? _movement : _core.GetCoreComponent(ref _movement);
          private Movement _movement;

          protected CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses 
               : _core.GetCoreComponent(ref _collisionSenses);
          private CollisionSenses _collisionSenses;
          

          // Create a constructor for player so we can access all the function like Update, Exit,... 
          // in different State class that inherited from PlayerState
          protected PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
          {
               Player = player;
               StateMachine = stateMachine;
               PlayerData = playerData;
               _animBoolName = animBoolName;
               _core = player.Core;
          }

          public virtual void Enter()
          {
               // Each state will have different checks
               // Like jump state will check for ground
               DoChecks();
               
               // Run the animation with the same name in the animator
               Player.Anim.SetBool(_animBoolName, true);
               
               // Save the time when the player enter a state 
               StartTime = Time.time;
               IsAnimationFinished = false;
               IsAnimationCancel = false;
               IsExitingState = false;
          }
     
          public virtual void Exit()
          {
               // Set the current animation to false so we can change into a new animation                                                                                                                                                                                    
               Player.Anim.SetBool(_animBoolName, false);
               IsExitingState = true;
          }

          public virtual void LogicUpdate(){}
     
          public void PhysicsUpdate() => DoChecks();
          protected virtual void DoChecks(){}
          
          #region Animation Function
          public virtual void AnimationTrigger() {}
          public virtual void AnimationFinishTrigger() => IsAnimationFinished = true;
          public virtual void AnimationCancelTrigger(){}
          
          public virtual void StartMovementTrigger(){}
          public virtual void StopMovementTrigger(){}

          public virtual void SetFlipActive(){}
          public virtual void SetFlipInactive(){}
          public virtual void AttackTrigger(){}
          
          #endregion
     }
}
