namespace Automata
{
    public abstract class State
    {
        protected readonly PushDownAutomata pda;

        protected State(PushDownAutomata pda)
        {
            this.pda = pda;
        }

        public abstract void Enter();
        public virtual void Enable() { }
        public abstract void Update();
        public virtual void Disable() { }
        public abstract void Exit();
    }
}