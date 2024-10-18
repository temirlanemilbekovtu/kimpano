using System;

namespace Godot;

[GlobalClass]
public partial class StateMove : State
{   
    // TODO: add animation
    
    [ExportGroup("Players")] 
    [Export] private Node2D _selfNode;
    [Export] private Node2D _opponentNode;

    [ExportGroup("Physics")] 
    [Export] private CharacterBody2D _characterBody;
    [Export] private float           _speed;

    [ExportGroup("Transitions")] 
    [Export] private StateIdle  _idleState;
    [Export] private StateSwing _swingState;
    [Export] private StateParry _parryState;
    [Export] private StateDeath _deathState;

    private Direction _faceDirection;
    private Direction _moveDirection;

    public override void _Ready() {
        if (_selfNode is null) {
            GD.Print("Self property is null. Free queued.");
            QueueFree();
            return;
        }
        
        if (_opponentNode is null) {
            GD.Print("Opponent property is null. Free queued.");
            QueueFree();
            return;
        }
        
        if (_characterBody == null) {
            GD.Print("CharacterBody is null. Free queued.");
            QueueFree();
            return;
        }

        _faceDirection = _selfNode.Position > _opponentNode.Position ? Direction.Backward : Direction.Forward;
    }

    public override void HandleInput(MoveInputInfo moveInputInfo) {
        MoveKey moveKey = moveInputInfo.MoveKey;

        switch (moveKey) {
            case MoveKey.Back:
            case MoveKey.Front:
                if (moveInputInfo.IsPressed) {
                    _moveDirection = InputHandler.GetOppositeDirection(_moveDirection);
                } else if (_moveDirection == InputHandler.MoveKeyToDirection(moveKey)) {
                    MyStateMachine.SwitchState(_idleState, new string[] { });
                }
                break;
            case MoveKey.Block:
                MyStateMachine.SwitchState(_swingState, new string[] { });
                break;
        }
    }

    public override void HandleCombo(Combo combo) {
        MyStateMachine.SwitchState(_swingState, new[] { combo.Name.ToString() });
    }

    public override void HandleDamage() {
        MyStateMachine.SwitchState(_deathState, new string[] { });
    }

    public override void StatePhysicsProcess(double delta) {
        _characterBody.Velocity = Vector2.Right * _speed * (int) _faceDirection * (int) _moveDirection;
        _characterBody.MoveAndSlide();
    }

    public override void StateEntry(string[] args) {
        if (args.Length == 0 || Enum.TryParse(args[0], out _moveDirection)) {
            MyStateMachine.SwitchState(_idleState, new string[] { });
        }
    }
}
