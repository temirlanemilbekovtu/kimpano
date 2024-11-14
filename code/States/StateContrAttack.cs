namespace Godot;

[GlobalClass]
public partial class StateContrAttack : State
{
    [Export] private float      _waitTime;
    [Export] private StringName _animationName;

    [ExportGroup("Transitions")]
    [Export] private StateIdle   _idle;
    [Export] private StateDeath  _death;
    [Export] private StateAttack _attack;

    private Timer _timer = new();

    public override void _Ready() {
	    _timer.Timeout += OnTimerTimeout;
    }

    public override void StateEntry(string[] args) {
	    MyAnimationPlayer.Play(_animationName);

	    _timer.WaitTime = _waitTime;
	    _timer.Start();
    }

    public override void HandleCombo(Combo combo) {
	    MyStateMachine.SwitchState(_attack, new[] { combo.Name.ToString() });
    }

    public override void HandleDamage() {
	    MyStateMachine.SwitchState(_death, new string[] { });
    }

    public override void StateExit() {
	    MyAnimationPlayer.Stop();
	    _timer.Stop();
    }

    private void OnTimerTimeout() {
	    MyStateMachine.SwitchState(_idle, new string[] { });
    }
}