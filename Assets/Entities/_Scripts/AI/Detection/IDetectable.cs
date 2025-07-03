using System;

namespace Automata
{
    public interface IDetectable
    {
        event Action<float> OnValueChanged;
        float GetValue();
    }
}