namespace Godot;

[GlobalClass]
public partial class StateAttack : State
{
	[ExportGroup("Collision")]
	[Export(PropertyHint.Layers2DPhysics)] private uint _hitBoxLayer;
	[Export(PropertyHint.Layers2DPhysics)] private uint _hurtBoxLayer;
	
	[ExportGroup("Transitions")]
	[Export] private StateIdle     _idle;
	[Export] private StateCooldown _cooldown;

	private Area2D           _hitBoxArea2D;
	private CollisionShape2D _collisionShape2D;
	private Combo            _currentCombo;
	
	public override void _Ready() {
		_hitBoxArea2D = new Area2D() {
			CollisionLayer = _hitBoxLayer,
			CollisionMask = _hurtBoxLayer
		};

		_collisionShape2D = new CollisionShape2D() {
			Disabled = true,
		};
		
		AddChild(_hitBoxArea2D);
		AddChild(_collisionShape2D);
	}

	public override void StateEntry(string[] args) {
		_currentCombo = args.Length == 0 ? null : MyComboManager.FindComboOrNull(args[0]);
		if (_currentCombo is null) {
			MyStateMachine.SwitchState(_idle, new string[] { });
			return;
		}

		_collisionShape2D.Disabled = false;
		_collisionShape2D.Shape = new RectangleShape2D() { Size = _currentCombo.HitBox.Size };
		_collisionShape2D.Position = GetOwner<Node2D>().ToGlobal(_currentCombo.HitBox.Position);
		
		MyAnimationPlayer.Play(_currentCombo.AttackAnimName);
		MyAnimationPlayer.AnimationFinished -= OnAnimationFinished;
		MyAnimationPlayer.AnimationFinished += OnAnimationFinished;
	}

	public override void StateExit() {
		MyAnimationPlayer.Stop();
		MyAnimationPlayer.AnimationFinished -= OnAnimationFinished;
		_collisionShape2D.Disabled = true;
	}

	private void OnAnimationFinished(StringName animName) {
		MyStateMachine.SwitchState(_cooldown, new string[] { _currentCombo.Name });
	}
}