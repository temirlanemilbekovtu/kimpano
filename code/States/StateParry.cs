namespace Godot;

[GlobalClass]
public partial class StateParry : State
{
    // TODO: add animation
    
    [ExportGroup("Timing")]
    [Export] private float _duration = 0.1f;
    
    [ExportGroup("Transitions")] 
    [Export] private StateIdle        _idleState;
    [Export] private StateBlock       _blockState;
    [Export] private StateContrAttack _contrAttackState;

    private Timer _timer = new();
    private bool  _isQuitting;
    
    public override void _Ready() {
        _timer.Connect("timeout", new Callable(this, "OnTimerTimeout"));
    }

    public override void HandleInput(MoveInputInfo moveInputInfo) {
        if (!moveInputInfo.IsPressed && moveInputInfo.MoveKey == MoveKey.Block) {
            _isQuitting = true;
        }
    }

    public override void HandleDamage() {
        MyStateMachine.SwitchState(_contrAttackState, new string[] { });
    }

    public override void StateEntry(string[] args) {
        _timer.WaitTime = _duration;
        _timer.Start();
    }

    public override void StateExit() {
        _isQuitting = false;
        _timer.Stop();
    }

    private void OnTimerTimeout() {
        if (_isQuitting) {
            MyStateMachine.SwitchState(_idleState, new string[] { });
        } else {
            MyStateMachine.SwitchState(_blockState, new string[] { });
        }
    }
}