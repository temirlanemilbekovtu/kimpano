using Godot.Collections;

namespace Godot;

[GlobalClass]
public partial class Combo : Resource
{
    [Export] public StringName Name                 { get; private set; } = "Combo";
    [Export] public StringName AnimationName        { get; private set; }
    [Export] public Rect2 HitBox                    { get; private set; }
    [Export] public Array<MoveKey> MoveList         { get; private set; } = new() { MoveKey.None };

    /*
    public bool Inherits(Combo combo) {
        if (combo == this || combo.MoveList.Count >= MoveList.Count) {
            return false;
        }

        int length = combo.MoveList.Count;
        return MoveList.ToList().GetRange(0, length).Equals(combo.MoveList.ToList());
    }
    */
}