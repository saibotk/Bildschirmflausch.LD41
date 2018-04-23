using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {



    private Room start;
    private Room finish;

    public enum EndedCause {
        WIN, DIED
    }

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
    GameObject DoorInner;
    [SerializeField]
    GameObject DoorOuter;
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
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject ui;

    [SerializeField]
    private GameObject cam;

    [SerializeField]
    private GameObject mapRoot;

    private bool engineInitDone;
    private Player player;

    public static GameController instance;
    public GameController() {
        instance = this;
    }

    public enum GameState { UNSET, INIT, STARTING, RUNNING, ENDED };
    private EndedCause endCause = EndedCause.DIED;
    private GameState state = GameState.UNSET;

	// Use this for initialization
	void Start () {
        genPrefabs = new Dictionary<GenerationProcessor.ExtendedTileType, GameObject>();
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.BorderOuter, BorderOuter);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.BorderInner, BorderInner);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.BorderSingle, BorderSingle);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.Rock, Rock);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockL, RockL);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockU, RockU);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockR, RockR);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockD, RockD);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockLU, RockLU);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockLR, RockLR);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockLD, RockLD);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockLURD, RockLURD);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockRD, RockRD);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockUR, RockUR);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockUD, RockUD);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockLUD, RockLUD);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockLUR, RockLUR);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockURD, RockURD);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockLRD, RockLRD);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.Ground, Ground);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.DoorInner, DoorInner);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.DoorOuter, DoorOuter);
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

        // Start room
        GameObject goStart = gp.ProcessRoom(dg.start.tiles);
        goStart.name = "Start";
        List<Transform> lt = new List<Transform>(goStart.GetComponentsInChildren<Transform>());
        GameObject doorRoot = new GameObject();
        doorRoot.name = "Doors";
        doorRoot.transform.SetParent(goStart.transform);
        start = goStart.AddComponent<Room>();
        lt = lt.FindAll(x => x.tag == "door");
        lt.ForEach(x => {
            x.SetParent(doorRoot.transform);
            x.gameObject.GetComponent<Door>().SetParent(start);
            });
        start.SetDoorsRootObject(doorRoot);

        // WIP
        GameObject spawnpointRoot = new GameObject();
        spawnpointRoot.name = "Spawnpoints";
        spawnpointRoot.transform.SetParent(goStart.transform);
        spawnpointRoot.transform.position = new Vector3(dg.start.roomPosition.x, dg.start.roomPosition.y, 0);
        GameObject spawn = new GameObject();
        spawn.transform.SetParent(spawnpointRoot.transform);
        spawn.transform.position = new Vector3(dg.start.GetCenter().x, dg.start.GetCenter().y, 0);
        start.SetSpawnPointsRootObject(spawnpointRoot);

        start.Reload();
        start.transform.SetParent(mapRoot.transform);

        // Finish room
        GameObject goFinish = gp.ProcessRoom(dg.end.tiles);
        goFinish.name = "Finish";
        List<Transform> ltf = new List<Transform>(goFinish.GetComponentsInChildren<Transform>());
        GameObject doorRootf = new GameObject();
        doorRootf.name = "Doors";
        doorRootf.transform.SetParent(goFinish.transform);
        ltf = ltf.FindAll(x => x.tag == "door");
        finish = goFinish.AddComponent<Room>();
        ltf.ForEach(x => {
            x.SetParent(doorRootf.transform);
            x.gameObject.GetComponent<Door>().SetParent(finish);
        });
        finish.SetDoorsRootObject(doorRootf);
        finish.Reload();
        finish.transform.SetParent(mapRoot.transform);

        // Other Rooms
        foreach (GenRoom gr in dg.rooms) {
            GameObject groom = gp.ProcessRoom(gr.tiles);
            List<Transform> ltg = new List<Transform>(groom.GetComponentsInChildren<Transform>());
            GameObject doorRootg = new GameObject();
            doorRootg.name = "Doors";
            doorRootg.transform.SetParent(groom.transform);
            Room grom = groom.AddComponent<Room>();
            ltg = ltg.FindAll(x => x.tag == "door");
            ltg.ForEach(x => {
                x.SetParent(doorRootg.transform);
                x.gameObject.GetComponent<Door>().SetParent(grom);
                });
            
            grom.SetDoorsRootObject(doorRootg);
            grom.Reload();
            groom.transform.SetParent(mapRoot.transform);
        }

        // Hallways
        GameObject goHallways = gp.ProcessRoom(dg.path.tiles);
        goHallways.name = "Hallways";
        goHallways.AddComponent<Room>();
        goHallways.transform.SetParent(mapRoot.transform);
    }

    private void Starting() {
        
        StartObjective goal = new StartObjective(start, playerPrefab);
        start.SetObjective(goal);
        start.OnPlayerEnter(player);
        player = goal.GetPlayer();
        if ( player != null ) {
            cam.GetComponent<CameraControl>().SetFollow(player.gameObject);
            GetUI().InitHealthController(player);
        } else {
            Debug.Log("No Player spawned!");
        }
        finish.SetObjective(new FinishObjective(finish));
        cam.GetComponent<AudioControl>().LevelBgm();
    }

    private void Running() {

    }

    private void Ended() {
        Debug.Log("Game ended");
        //Time.timeScale = 0;
        if ( ui != null ) {
            Debug.Log("show end UI");
            if(endCause == EndedCause.DIED) {
                cam.GetComponent<AudioControl>().GameOverBgm();
                ui.GetComponent<UIController>().ShowGameOverUI();
            } else if(endCause == EndedCause.WIN) {
                //cam.GetComponent<AudioControl>().SfxPlay(2);
                ui.GetComponent<UIController>().ShowWinUI();
            }
        } else {
            Debug.Log("No UI specified");
        }
    }

    public AudioControl GetAudioControl() {
        return cam.GetComponent<AudioControl>();
    }

    public UIController GetUI() {
        return ui.GetComponent<UIController>();
    }

    public bool GameEnded() {
        return state == GameState.ENDED;
    }

    public void EndGame(EndedCause cause) {
        endCause = cause;
        ChangeState(GameState.ENDED);
    }
}
