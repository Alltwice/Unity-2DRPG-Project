using UnityEngine;

public abstract class PlayerStateMachine
{
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnExit();
}
