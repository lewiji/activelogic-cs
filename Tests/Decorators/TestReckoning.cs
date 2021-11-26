#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System;
using InvOp = System.InvalidOperationException;
using NUnit.Framework;
using Active.Core;
using Active.Core.Details;

public class TestReckoning : TestBase{

    Reckoning x;
    ReCon stack;

    [SetUp] public void Setup() => stack = new ReCon();

    [Test] public void Constructor([Values(true, false)] bool arg){
        x = new Reckoning(arg, stack);
        o(x.context != null, arg);
    }

    #if !AL_OPTIMIZE
    [Test] public void Constructor_withCallInfo(
                                      [Values(true, false)] bool arg){
        x = new Reckoning(arg, stack, new CallInfo("path", "member", -1));
        o(x.context != null, arg);
    }
    #endif

    [Test] public void Indexer([Range(-1, 1)]        int val,
                               [Values(true, false)] bool arg){
        var dec = new Dec();
                                          // init. with condition
        x = new Reckoning(arg, stack);
        x.context?.Traverse( dec );       // traverse a decorator
        var s0 = status.@unchecked(val);
        var s1 = x[ s0 ];                 // exit the context
        o( dec.didReset, arg );           // did reset?
        o( s0, s1 );                      // neutrality
    }

}
