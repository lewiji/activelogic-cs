using NUnit.Framework;
using Active.Core;
using static Active.Status;

public class TestLoggingSemantics : TestBase{

    [Test] public void Standard(){
        o( done().complete  );
        o( fail().failing   );
        o( cont().running   );
        o( (status)@void(),  status._done );
        o( (status)@false(), status._fail );
        o( @forever().ever,  status._cont );
        // No short forms for pending/impending
        // ...
        // ...
        // ...
        // ...
    }

    // ==============================================================

    [Test] public void LongForm(){
        o( status.done().complete  );
        o( status.fail().failing   );
        o( status.cont().running   );
        o( (status)action.done(),  status._done );
        o( (status)failure.fail(), status._fail );
        o( loop.cont().ever,  status._cont );
        //
        o( pending.cont().running);
        o( pending.done().complete);
        o( impending.cont().running);
        o( impending.fail().failing);
    }

    [Test] public void Eval_([Range(-1, 1)] int val){
        var s0 = status.@unchecked(val);
        o( s0, Eval(s0) );
    }

}
