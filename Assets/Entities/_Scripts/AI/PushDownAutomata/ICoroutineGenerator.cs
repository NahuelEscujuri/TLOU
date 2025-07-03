using System.Collections;
using UnityEngine;

namespace Automata
{
    public interface ICoroutineGenerator
    {
        public Coroutine NewCorrutine(IEnumerator enumerator);
        public void EndCorrutine(Coroutine coroutine);
    }
}