namespace Godot;

[GlobalClass]
public partial class StateSwing : State
{
    [ExportGroup("Physics")] 
    [Export] private CharacterBody2D _body;

    [ExportGroup("Transitions")] 
    [Export] private StateIdle   _idle;
    [Export] private StateSwing  _swing;
    [Export] private StateAttack _attack;
    [Export] private StateDeath  _death;

    private Combo _currentCombo;
    
    public override void _Ready() {
        if (_body == null) {
            GD.Print("CharacterBody is null. Free queued.");
            QueueFree();
        }
        
        if (MyComboManager is null) {
            GD.Print("ComboManager is missing. Free queued.");
            QueueFree();
        }
    }

    public override void HandleCombo(Combo combo) {
        MyStateMachine.SwitchState(_swing, new[] { combo.Name.ToString() });
    }

    public override void HandleDamage() {
        MyStateMachine.SwitchState(_death, new string[] { });
    }

    public override void StateEntry(string[] args) {
        if (args.Length == 0) {
            return;
        }
        
        _currentCombo = MyComboManager.FindComboOrNull(args[0]);
        if (_currentCombo is null) {
            MyStateMachine.SwitchState(_idle, new string[] { });
            return;
        }
        
        MyAnimationPlayer.AnimationEvent += OnAnimEvent;
        MyAnimationPlayer.Play(_currentCombo.AnimationName);
        
        _body.Velocity = Vector2.Zero;
        _body.MoveAndSlide();
    }

    public override void StateExit() {
        MyAnimationPlayer.AnimationEvent -= OnAnimEvent;
    }

    private void OnAnimEvent(string tag) {
        MyStateMachine.SwitchState(_attack, new string[] { _currentCombo.Name });
    }
}