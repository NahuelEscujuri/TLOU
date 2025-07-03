namespace Automata
{
    public interface IFaction
    {
        bool IsSameFaction(IFaction otherFaction);
        float ObtainSympathy(IFaction otherFaction);
    }
}