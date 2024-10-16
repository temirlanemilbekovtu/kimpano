using Godot.Collections;

namespace Godot;

[GlobalClass]
public partial class Combo : Resource
{
    [Export] public StringName      Name            { get; private set; } = "Combo";
    [Export] public StringName      AnimationName   { get; private set; }
    [Export] public Rect2           HitBox          { get; private set; }
    [Export] public Array<MoveKey>  MoveList        { get; private set; } = new() { MoveKey.None };
}