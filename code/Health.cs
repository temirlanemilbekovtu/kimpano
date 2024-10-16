namespace Godot;

[GlobalClass]
public partial class Health : Area2D
{
    public delegate void DamageHandler();
    public event DamageHandler Damaged;

    public override void _Ready() {
        Connect("area_entered", new Callable(this, "OnHitBoxAreaEntered"));
    }

    private void OnHitBoxAreaEntered(Area2D area) {
        if (area.GetOwner() != GetOwner()) {
            Damaged?.Invoke();
        }
    }
}