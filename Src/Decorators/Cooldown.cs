// Doc/Reference/Decorators.md
using static Active.Core.status;
using Tag = System.Runtime.CompilerServices.CallerLineNumberAttribute;

namespace Active.Core{
[System.Serializable]
public class Cooldown: Decorator, Decorator.OptionalArguments{

	static int uid; internal static int id => uid = ID(uid);

	public float duration = 1f;
	float stamp = System.Single.MinValue;

	public Cooldown(){}
	public Cooldown(float duration){ this.duration = duration; }

	public static implicit operator Cooldown(float val) => new Cooldown(val);

	public Gate? pass => this[duration];

	override public void OnStatus(status s)
	=> stamp = s.running ? stamp : time;

	public Gate? this[float s]
	=> (time >= stamp + s) ? done(log && $"[{s:0.0}]")
						   : fail(log && $"[{s + stamp - time:0.0}]");

	override public action Reset(){ stamp = 0; return @void(); }

}

#if !AL_BEST_PERF
partial class Task{
	public Decorator.Gate? Cooldown(float duration, [Tag] int key = -1)
	=> store.Decorator<Cooldown>(key, Active.Core.Cooldown.id)[duration];
}
#endif

}  // Active.Core
