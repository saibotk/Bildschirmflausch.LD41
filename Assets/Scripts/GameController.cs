using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    

    private Room start;
    private Room finish;

    // Generation Settings
    [Header("Tile Prefabs")]
    [SerializeField]
    GameObject BorderOuter;
    [SerializeField]
    GameObject BorderInner;
    [SerializeField]
    GameObject BorderSingle;
    [SerializeField]
    GameObject Ground;
    [SerializeField]
    GameObject Door;
    [SerializeField]
    GameObject Rock;
    [SerializeField]
    GameObject RockL;
    [SerializeField]
    GameObject RockU;
    [SerializeField]
    GameObject RockR;
    [SerializeField]
    GameObject RockD;
    [SerializeField]
    GameObject RockLU;
    [SerializeField]
    GameObject RockLR;
    [SerializeField]
    GameObject RockLD;
    [SerializeField]
    GameObject RockUR;
    [SerializeField]
    GameObject RockUD;
    [SerializeField]
    GameObject RockRD;
    [SerializeField]
    GameObject RockLURD;
    [SerializeField]
    GameObject RockLUD;
    [SerializeField]
    GameObject RockLUR;
    [SerializeField]
    GameObject RockURD;
    [SerializeField]
    GameObject RockLRD;

    private Dictionary<GenerationProcessor.ExtendedTileType, GameObject> genPrefabs;
    [Space(10)]
    [Header("References")]
    [SerializeField]
    private GameObject ui;

    [SerializeField]
    private GameObject cam;

    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    GameObject mapRoot;

    private bool engineInitDone;
    private Player player;
    public static GameController instance;
    public GameController() {
        instance = this;
    }

    public enum GameState { UNSET, INIT, STARTING, RUNNING, ENDED };

    private GameState state = GameState.UNSET;

	// Use this for initialization
	void Start () {
        genPrefabs = new Dictionary<GenerationProcessor.ExtendedTileType, GameObject> {
            { GenerationProcessor.ExtendedTileType.BorderOuter, BorderOuter },
            { GenerationProcessor.ExtendedTileType.BorderInner, BorderInner },
            { GenerationProcessor.ExtendedTileType.BorderSingle, BorderSingle },
            { GenerationProcessor.ExtendedTileType.Rock, Rock },
            { GenerationProcessor.ExtendedTileType.RockL, RockL },
            { GenerationProcessor.ExtendedTileType.RockU, RockU },
            { GenerationProcessor.ExtendedTileType.RockR, RockR },
            { GenerationProcessor.ExtendedTileType.RockD, RockD },
            { GenerationProcessor.ExtendedTileType.RockLU, RockLU },
            { GenerationProcessor.ExtendedTileType.RockLR, RockLR },
            { GenerationProcessor.ExtendedTileType.RockLD, RockLD },
            { GenerationProcessor.ExtendedTileType.RockLURD, RockLURD },
            { GenerationProcessor.ExtendedTileType.RockRD, RockRD },
            { GenerationProcessor.ExtendedTileType.RockUR, RockUR },
            { GenerationProcessor.ExtendedTileType.RockUD, RockUD },
            { GenerationProcessor.ExtendedTileType.RockLUD, RockLUD },
            { GenerationProcessor.ExtendedTileType.RockLUR, RockLUR },
            { GenerationProcessor.ExtendedTileType.RockURD, RockURD },
            { GenerationProcessor.ExtendedTileType.RockLRD, RockLRD },
            { GenerationProcessor.ExtendedTileType.Ground, Ground },
            { GenerationProcessor.ExtendedTileType.Door, Door }
        };

    }

    // Update is called once per frame
    void Update() {
        if ( !engineInitDone ) {
            engineInitDone = true;
            Debug.Log("First Frame");
            ChangeState(GameState.INIT);
        }
    }

    public void ChangeState(GameState nextState) {
        if ( nextState != state ) {
            state = nextState;
            StateLogic(nextState);
        }
    }

    void StateLogic(GameState nstate) {
        switch ( nstate ) {
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

    private void Init() {
        // Generation
        DungeonGenerator dg = new DungeonGenerator();
        GenerationProcessor gp = new GenerationProcessor(genPrefabs);
        dg.Generate();
        GameObject goStart = gp.ProcessRoom(dg.start.tiles);
        start = goStart.AddComponent<Room>();
        start.transform.SetParent(mapRoot.transform);
        GameObject goFinish = gp.ProcessRoom(dg.end.tiles);
        finish = goFinish.AddComponent<Room>();
        finish.transform.SetParent(mapRoot.transform);
        foreach (GenRoom gr in dg.rooms) {
            GameObject groom = gp.ProcessRoom(gr.tiles);
            groom.AddComponent<Room>();
            groom.transform.SetParent(mapRoot.transform);
        }
    }

    private void Starting() {
        StartObjective goal = new StartObjective(start, playerPrefab);
        start.SetObjective(goal);
        start.OnPlayerEnter(player);
        player = goal.GetPlayer();
        cam.GetComponent<CameraControl>().SetFollow(player.gameObject);
    }

    private void Running() {

    }

    private void Ended() {
        Debug.Log("Game ended");
        //Time.timeScale = 0;
        if ( ui != null ) {
            Debug.Log("show gameover UI");
            ui.GetComponent<UIController>().ShowGameOverUI();
        } else {
            Debug.Log("No ui specified");
        }
    }

}
