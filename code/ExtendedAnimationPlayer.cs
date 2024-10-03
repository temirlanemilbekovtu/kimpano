using System.Collections.Generic;
using System.Linq;

namespace Godot;

[GlobalClass]
public partial class ExtendedAnimationPlayer : AnimationPlayer
{
    public delegate void GetAnimationTag(string tag);
    public event GetAnimationTag AnimationEvent;
    
    [Export] public string[] Tags {
        get => _tags.ToArray();
        set => _tags = value.ToHashSet();
    }

    private HashSet<string> _tags;

    public void CallAnimationEvent(string tag) {
        if (_tags.Contains(tag)) {
            AnimationEvent?.Invoke(tag);
        }
    }
}
