using Sweet.Collections.Unsafe.List;

namespace Sweet.Intents.Actions;

public struct ActionState : IDisposable
{
    public UnsafeList<Clause> Clauses;

    public byte SatisfiedClauses;
    public readonly bool IsDown => SatisfiedClauses == Clauses.Length;

    public ActionState(byte groupCapacity)
    {
        Clauses = new UnsafeList<Clause>(groupCapacity);
    }

    public void Dispose()
    {
        Clauses.Dispose();
    }
}