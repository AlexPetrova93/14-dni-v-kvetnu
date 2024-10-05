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
        IntroScreen.alpha = 1;
        
        StartCoroutine(RunLevel());
    }
    
    // Level runner
    private IEnumerator RunLevel()
    {
        Fader.Hide(.5f);
        yield return new WaitForSeconds(.5f);
        yield return Utility.WaitForClick();
        IntroScreen.Hide(1f);
        TexturePainter.ResetTexture(); // make sure the canvas is blank at the beginning
        yield return new WaitForSeconds(1f);

        // INTRO
        yield return MonologueManager.RunMonologue(MonologueNodeName.Intro);
        
        // PHASE 1
        yield return RunPaintingPhase(0, MonologueNodeName.PaintingComplete1);
        yield return RunMapPhase(0, MonologueNodeName.MapIntro1);
        
        // PHASE 2
        yield return RunPaintingPhase(1, MonologueNodeName.PaintingComplete2);
        yield return RunMapPhase(1, MonologueNodeName.MapIntro2);
        
        // PHASE 3
        yield return RunPaintingPhase(2, MonologueNodeName.PaintingComplete3);
        yield return RunMapPhase(2, MonologueNodeName.MapIntro3);
        
        // FINAL PAINTING
        yield return RunPaintingPhase(3, MonologueNodeName.PaintingComplete4);
        TexturePainter.PauseGame();
        
        // OUTRO
        yield return MonologueManager.RunMonologue(MonologueNodeName.Outro);
        
        yield return RestartLevelRoutine();
    }

    private IEnumerator RunPaintingPhase(int phaseIndex, MonologueNodeName paintingDoneNode)
    {
        // start painting
        TexturePainter.StartGame(phaseIndex);

        // wait until most is painted
        yield return Utility.WaitUntilEvent(TexturePainter.OnPaintingComplete);
        
        // run dialogue, which will signify you can open the door in first 3 phases
        yield return MonologueManager.RunMonologue(paintingDoneNode);
    }
    
    private IEnumerator RunMapPhase(int phaseIndex, MonologueNodeName mapIntroNode)
    {
        MapViewer.StartPhase(phaseIndex); // start next phase before activating door handle
        
        // TODO activate door handle drag
        
        // TODO wait for door drag
        
        TexturePainter.PauseGame();
        yield return OpenDoor(); 
            
        TexturePainter.ResetTexture();
        MapViewer.Activate(); // allow map input

        yield return MonologueManager.RunMonologue(mapIntroNode);
        MapViewer.ActivateElementsMonologue(); // allow monologue after click on element
        
        yield return Utility.WaitUntilEvent(MapViewer.OnRequiredElementFound);
        
        MapViewer.Deactivate(.5f);
        TexturePainter.ResetTexture();
        yield return CloseDoor();
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
