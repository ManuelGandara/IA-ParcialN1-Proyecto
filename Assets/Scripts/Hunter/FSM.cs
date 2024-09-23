using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    public enum HunterStates
    {
        Idle,
        Patrol,
        Chase
    }

    private class State
    {
        public HunterStates StateType { get; private set; }
        public IState StateInstance { get; private set; }

        public State(HunterStates stateType, IState instance)
        {
            StateType = stateType;
            StateInstance = instance;
        }
    }

    private List<State> _states = new List<State>();
    private IState _currentState = null;

    public void CreateState(HunterStates newState, IState state)
    {
        if (!_states.Exists(s => s.StateType == newState))
        {
            _states.Add(new State(newState, state));
        }
    }

    public void ChangeState(HunterStates state)
    {
        var nextState = _states.Find(s => s.StateType == state);
        if (nextState != null)
        {
            if (_currentState != null)
            {
                Debug.Log("Saliendo del estado actual...");
                _currentState.OnExit();
            }

            _currentState = nextState.StateInstance;
            Debug.Log("Entrando en nuevo estado...");
            _currentState.OnEnter();
        }
    }

    public void ArtificialUpdate()
    {
        if (_currentState != null)
        {
            _currentState.OnUpdate();
        }
    }
}