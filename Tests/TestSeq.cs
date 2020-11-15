using System;
using InvOp = System.InvalidOperationException;
using NUnit.Framework;
using Active.Core; using static Active.Status;
using T = Active.Core.Seq;

public class TestSeq : TestBase{

    T x; [SetUp] public void Setup() => x = new T();

    [Test] public void Self_afterEnter(){
        x.Step(); o(T.self, x);
    }

    [Test] public void InitialState(){
        o(x.key.complete);
        o(x.index, 0);
        o(x.ι, 0);
    }

    [Test] public void Step([Range(-1, 1)]        int  val,
                            [Values(true, false)] bool repeat){
        var s = status.@unchecked(val);
        x.index = 3;
        x.state = s;
        o(x.Step(repeat), x);
        o(Comp.stack.Peek(), x);
        o(x.index, repeat && !s.running ? 0      : 3);
        o(x.state, repeat && !s.running ? done() : s);
    }

    [Test] public void Task_atIndex(){
        x.Step();
        o( T.task, x);
        o( x.ι, 1);
    }

    [Test] public void Task_beforeIndex(){
        x.Step();
        x.index = 3;
        o( T.task, null);
        o( x.ι, 1);
    }

    [Test] public void Task_failing(){
        x.Step();
        x.state = fail();
        o( T.task, null);
    }

    [Test] public void Indexer([Range(-1, 1)] int val){
        var s = status.@unchecked(val);
        var z = x[ s ];
        o( z, x );
        o( x.index, s.complete ? 1 : 0);
    }

    [Test] public void AddOp() => o( x + (Comp)null, x);

    [Test] public void ModOp() => o( x % (Comp)null, x + (Comp)null);

}
