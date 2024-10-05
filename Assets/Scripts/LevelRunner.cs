using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRunner : MonoBehaviour
{
    public MonologueManager MonologueManager;
    public TexturePainter TexturePainter;
    public MapViewer MapViewer;

    public Camera Camera;

    public PauseMenu PauseMenu;
    public CanvasGroup IntroScreen;
    public CanvasGroup Fader;
    
    public Transform DoorLeft;
    public Transform DoorRight;
    
    private float _doorLeftClosedX;
    private float _doorRightClosedX;
    
    private const float DoorLeftOpenX = -18.5f;
    private const float DoorRightOpenX = 21.75f;

    public void RestartLevel()
    {
        StartCoroutine(RestartLevelRoutine());
    }

    public void PauseGame()
    {
        MapViewer.PauseInput();
        TexturePainter.PauseGame();
        PauseMenu.Show();
        PauseMenu.OnClickedOutside.AddListener(UnpauseGame);
    }

    public void UnpauseGame()
    {
        MapViewer.Activate();
        TexturePainter.ResumeGame();
        PauseMenu.Hide();
        PauseMenu.OnClickedOutside.RemoveListener(UnpauseGame);
    }
    
    private IEnumerator RestartLevelRoutine()
    {
        MapViewer.PauseInput();
        TexturePainter.PauseGame();
        Fader.Show(.5f);
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void Start()
    {
        _doorLeftClosedX = DoorLeft.position.x;
        _doorRightClosedX = DoorRight.position.x;
        MonologueManager.Init();
        TexturePainter.Init(Camera);
        MapViewer.Init(MonologueManager, Camera);

        Fader.alpha = 1;
        
        StartCoroutine(RunLevel());
    }
    
    // Level runner
    private IEnumerator RunLevel()
    {
        Fader.Hide(.5f);
        IntroScreen.alpha = 1;
        yield return new WaitForSeconds(.5f);
        yield return Utility.WaitForClick();
        MapViewer.StartPhase(0);
        yield return OpenDoor(0);
        IntroScreen.Hide(1f);
        yield return new WaitForSeconds(1f);

        // redo - first painting, ten map
        yield return RunPhase(0, MonologueNodeName.Intro1, MonologueNodeName.PaintingComplete1);
        yield return RunPhaseEnd(1);
        
        yield return RunPhase(1, MonologueNodeName.Intro2, MonologueNodeName.PaintingComplete2);
        yield return RunPhaseEnd(2);
        
        yield return RunPhase(2, MonologueNodeName.Intro3, MonologueNodeName.PaintingComplete3);
        
        // TODO Outro
        yield return MonologueManager.RunMonologue(MonologueNodeName.Outro);
        
        yield return RestartLevelRoutine();
    }

    // Phase:
    // 1. Run map monologue
    // 2. Open Map
    // 3. Wait for player to find mountains
    // 4. Start painting
    // 5. Wait for painting finished
    private IEnumerator RunPhase(int phaseIndex, MonologueNodeName introNode, MonologueNodeName paintingDoneNode)
    {
        yield return MonologueManager.RunMonologue(introNode);
        MapViewer.Activate();

        yield return Utility.WaitUntilEvent(MapViewer.OnRequiredElementFound);
        MapViewer.Deactivate(.5f);
        TexturePainter.ResetTexture();
        yield return CloseDoor();

        // start painting
        TexturePainter.StartGame(phaseIndex);

        yield return Utility.WaitUntilEvent(TexturePainter.OnPaintingComplete);
        
        Debug.Log("Painting finished!!!");
        yield return MonologueManager.RunMonologue(paintingDoneNode);
    }
    
    // PhaseEnd:
    // 1. Activate door handle
    // 2. Wait for player to open the door
    // 3. End painting game
    // 4. Show the map for next phase
    private IEnumerator RunPhaseEnd(int nextPhaseIndex)
    {
            MapViewer.StartPhase(nextPhaseIndex); // start next phase before activating door handle
        
            // TODO activate door handle drag
        
            // TODO wait for door drag
        
            TexturePainter.PauseGame();
            yield return OpenDoor(); 
            
            TexturePainter.ResetTexture();
    }

    private IEnumerator OpenDoor(float animTime = .5f)
    {
        DoorLeft.DOMoveX(DoorLeftOpenX, animTime);
        DoorRight.DOMoveX(DoorRightOpenX, animTime);
        yield return new WaitForSeconds(animTime);
    }

    private IEnumerator CloseDoor(float animTime = .5f)
    {
        DoorLeft.DOMoveX(_doorLeftClosedX, animTime);
        DoorRight.DOMoveX(_doorRightClosedX, animTime);
        yield return new WaitForSeconds(animTime);
    }
}
