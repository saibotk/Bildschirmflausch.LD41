using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    
    public enum GameState { UNSET, INIT, STARTING, RUNNING, ENDED };

    private GameState state = GameState.UNSET;

	// Use this for initialization
	void Start () {
        ChangeState(GameState.INIT);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ChangeState(GameState nextState) {
        if(nextState != state) {
            state = nextState;
            StateLogic(nextState);
        }
    }

    void StateLogic(GameState nstate) {
        switch (nstate)
        {
            case GameState.INIT:
                Init();
                break;
            case GameState.STARTING:
                Starting();
                break;
            case GameState.RUNNING:
                Running();
                break;
            case GameState.ENDED:
                Ended();
                break;
            default:
                print("Wrong GameState for StateLogic() call!");
                break;
        }

    }

    private void Init()
    {
        ChangeState(GameState.STARTING);
    }

    private void Starting()
    {
        ChangeState(GameState.RUNNING);
    }

    private void Running()
    {
        
    }

    private void Ended()
    {

    }

}
