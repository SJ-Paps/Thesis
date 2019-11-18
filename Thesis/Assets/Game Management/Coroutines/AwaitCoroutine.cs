using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.Coroutines
{
    public class AwaitCoroutine : CustomYieldInstruction
    {
        public override bool keepWaiting
        {
            get
            {
                return coroutine.MoveNext();
            }
        }

        private IEnumerator coroutine;

        public AwaitCoroutine(IEnumerator coroutine)
        {
            this.coroutine = coroutine;
        }
    }
}


