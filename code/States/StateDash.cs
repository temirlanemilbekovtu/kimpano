namespace Godot;

[GlobalClass]
public partial class StateDash : State
{
    [ExportGroup("Players")]
    [Export] private Node2D _self;
    [Export] private Node2D _opponent;

    [ExportGroup("Physics")]
    [Export] private CharacterBody2D    _characterBody;
    [Export] private float              _speed;
    [Export] private float              _duration;

    [ExportGroup("Transitions")]
    [Export] private StateIdle          _idleState;
    [Export] private StateSwing         _swingState;
    [Export] private StateParry         _parryState;
    [Export] private StateDeath         _deathState;

    private int     _faceDirection;
    private int     _moveDirection;
    private Timer   _timer;

    public override void _Ready() {
        _timer.Timeout += OnTimerTimeout;
    }

    public override void HandleInput(MoveInputInfo moveInputInfo) {
        if (moveInputInfo.IsPressed && moveInputInfo.MoveKey == MoveKey.Block) {
            MyStateMachine.SwitchState(_parryState, new string[] { });
        }
    }

    public override void HandleCombo(Combo combo) {
        bool ok = false;
        foreach (MoveKey moveKey in combo.MoveList) {
            if (moveKey == (_moveDirection == 1 ? MoveKey.Front : MoveKey.Back)) {
                if (ok) {
                    break;
                }
                ok = true;
            } else {
                ok = false;
            }
        }

        if (ok) {
            MyStateMachine.SwitchState(_swingState, new[] { combo.Name.ToString() });
        }
    }

    public override void HandleDamage() {
        MyStateMachine.SwitchState(_deathState, new string[] { });
    }

    public override void StatePhysicsProcess(double delta) {
        _characterBody.Velocity = Vector2.Right * _speed * _faceDirection * _moveDirection;
        _characterBody.MoveAndSlide();
    }

    public override void StateEntry(string[] args) {
        if (args.Length == 0 || !int.TryParse(args[0], out _moveDirection)) {
            MyStateMachine.SwitchState(_idleState, new string[] { });
            return;
        }

        _timer.WaitTime = _duration;
        _timer.Start();
    }

    public override void StateExit() {
        _timer.Stop();
    }

    private void OnTimerTimeout() {
        MyStateMachine.SwitchState(_idleState, new string[] { });
    }
}