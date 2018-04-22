using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    private Room start;
    private Room finish;

    [SerializeField]
    private GameObject ui;

    [SerializeField]
    private GameObject cam;

    private bool engineInitDone;
    private Player player;
    public static GameController instance;
    public GameController()
    {
        instance = this;
    }

    public enum GameState { UNSET, INIT, STARTING, RUNNING, ENDED };

    private GameState state = GameState.UNSET;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (!engineInitDone)
        {
            engineInitDone = true;
            Debug.Log("First Frame");
            ChangeState(GameState.INIT);
        }
    }

    public void ChangeState(GameState nextState) {
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
                ChangeState(GameState.STARTING);
                break;
            case GameState.STARTING:
                Starting();
                ChangeState(GameState.RUNNING);
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
        List<GameObject> tmp = new List<GameObject>
        {
            playerPrefab
        };
        start.SetObjective(new EntityObjective(start, tmp));
        start.OnPlayerEnter();
        player = ((EntityObjective) start.GetObjective()).GetEntities()[0].GetComponent<Player>();
        cam.GetComponent<CameraControl>().SetFollow(player.gameObject);
        ((EntityObjective)start.GetObjective()).Remove(player.gameObject);
    }

    private void Starting()
    {
        
    }

    private void Running()
    {
        
    }

    private void Ended()
    {
        Debug.Log("Game ended");
        //Time.timeScale = 0;
        if (ui != null) {
            Debug.Log("show gameover UI");
            ui.GetComponent<UIController>().ShowGameOverUI();
        } else {
            Debug.Log("No ui specified");
        }
    }

    public Player GetPlayer() {
        return player;
    }

}
