using System;
using System.Collections.Generic;

namespace Automata
{
    public class PushDownAutomata: IDisposable
    {
        public event Action<State> OnCurrentStateChanged;
        public State CurrentState => stateStack.Count > 0 ? stateStack.Peek() : null;
        public Context Context => context;
        readonly Context context;
        readonly Stack<State> stateStack = new();
        public PushDownAutomata(Context context)
        {
            this.context = context;
        }
        public void Dispose()
        {
            while (stateStack.Count > 0)
                PopState();
        }

        public void Update() => CurrentState?.Update();
        public void PushState(State newState)
        {
            UnityEngine.Debug.Log($"Estado pusheado: {newState}");
            CurrentState?.Disable();
            stateStack.Push(newState);
            newState.Enter();
            newState.Enable();
            OnCurrentStateChanged?.Invoke(newState);
        }
        public void PopState()
        {
            if (stateStack.Count == 0) return;
            CurrentState.Disable();
            CurrentState.Exit();
            stateStack.Pop();
            CurrentState?.Enable();
            OnCurrentStateChanged?.Invoke(CurrentState);
        }
    }
}