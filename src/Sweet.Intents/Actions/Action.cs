namespace Sweet.Intents.Actions;

public unsafe struct Action
{
    public ActionState* State;
    public byte ClauseIndex;

    public bool IsDowned;

    public Action(ActionState* state, byte clauseIndex)
    {
        State = state;
        ClauseIndex = clauseIndex;
    }
}