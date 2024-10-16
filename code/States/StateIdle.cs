namespace Godot;

[GlobalClass]
public partial class StateIdle : State
{
    // TODO: add animation
    
    [ExportGroup("Physics")] 
    [Export] private CharacterBody2D    _characterBody;

    [ExportGroup("Transitions")]
    [Export] private StateMove          _moveState;
    [Export] private StateDash          _dashState;
    [Export] private StateSwing         _swingState;
    [Export] private StateParry         _parryState;
    [Export] private StateDeath         _deathState;
    
    private Direction _previousDirection;

    public override void _Ready() {
        if (_characterBody == null) {
            GD.Print("CharacterBody is null. Free queued.");
            QueueFree();
        }
    }

    public override void HandleInput(MoveInputInfo moveInputInfo) {
        MoveKey inputMoveKey = moveInputInfo.MoveKey;

        switch (inputMoveKey) {
            case MoveKey.Back:
            case MoveKey.Front:
                if (!moveInputInfo.IsPressed) {
                    break;
                }
                var direction = inputMoveKey == MoveKey.Back ? Direction.Backward : Direction.Forward;
                var directionStr = ((int) direction).ToString();
                if (_previousDirection == direction) {
                    MyStateMachine.SwitchState(_dashState, new[] { directionStr });
                    _previousDirection = Direction.None;
                } else {
                    MyStateMachine.SwitchState(_moveState, new[] { directionStr });
                    _previousDirection = direction;
                }
                break;
            case MoveKey.Block:
                MyStateMachine.SwitchState(_parryState, new string[] { });
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
        _characterBody.Velocity = Vector2.Up * _characterBody.Velocity;
        _characterBody.MoveAndSlide();
    }
}