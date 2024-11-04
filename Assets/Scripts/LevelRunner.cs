using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRunner : MonoBehaviour
{
    public SoundManager SoundManager;
    public MonologueManager MonologueManager;
    public TexturePainter TexturePainter;
    public MapViewer MapViewer;

    public Camera Camera;

    public PauseMenu PauseMenu;
    public CanvasGroup IntroScreen;
    public CanvasGroup Fader;
    
    public Transform DoorLeft;
    public DraggableDoor DoorRight;

    public TutorialArrow TutorialArrow;
    
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
        _doorRightClosedX = DoorRight.transform.position.x;
        SoundManager.Init();
        MonologueManager.Init();
        TexturePainter.Init(Camera);
        MapViewer.Init(SoundManager, MonologueManager, Camera);

        DoorRight.Init(SoundManager, Camera);
        Fader.alpha = 1;
        IntroScreen.alpha = 1;
        
        StartCoroutine(RunLevel());
    }
    
    // Level runner
    private IEnumerator RunLevel()
    {
        SoundManager.PlayMusic(AudioRef.Music);
        Fader.Hide(.5f);
        yield return new WaitForSeconds(.5f);
        
        // set up
        var transform1 = DoorLeft.transform;
        var pos = transform1.position;
        pos.x = DoorLeftOpenX;
        transform1.position = pos;
        DoorRight.OpenFully(0);
        TexturePainter.ResetTexture(); // make sure the canvas is blank at the beginning
        
        yield return Utility.WaitForClick();
        IntroScreen.Hide(1f);
        SoundManager.DoPlayMusic(AudioRef.Ambient, 1f);
        yield return new WaitForSeconds(1f);

        MapViewer.Activate(); // allow map input

        yield return MonologueManager.RunMonologue(MonologueNodeName.MapIntro1);
        MapViewer.ActivateElementsMonologue(); // allow monologue after click on element
        
        yield return Utility.WaitUntilEvent(MapViewer.OnRequiredElementFound);
        
        MapViewer.Deactivate(.5f);
        SoundManager.DoPauseMusic(AudioRef.Ambient, 1f);
        yield return CloseDoor();
        
        // PHASE 1
        yield return RunPaintingPhase(0, MonologueNodeName.PaintingComplete1);
        
        // tutorial arrow here
        TutorialArrow.Show();
        yield return WaitForDoorDrag(1);
        TutorialArrow.Hide();
        
        // PHASE 2
        yield return RunMapPhase(MonologueNodeName.MapIntro2);
        yield return RunPaintingPhase(1, MonologueNodeName.PaintingComplete2);
        yield return WaitForDoorDrag(2);
        
        // PHASE 3
        yield return RunMapPhase(MonologueNodeName.MapIntro3);
        yield return RunPaintingPhase(2, MonologueNodeName.PaintingComplete3);
        yield return WaitForDoorDrag(3);
        
        // FINAL PAINTING
        yield return RunMapPhase(MonologueNodeName.MapIntro4);
        yield return RunPaintingPhase(3, MonologueNodeName.PaintingComplete4);
        TexturePainter.PauseGame();
        
        // OUTRO
        yield return MonologueManager.RunMonologue(MonologueNodeName.Outro);
        
        yield return RestartLevelRoutine();
    }
    
    private IEnumerator RunMapPhase(MonologueNodeName mapIntroNode)
    {
        SoundManager.DoPlayMusic(AudioRef.Ambient, 1f);
        DoorRight.OpenFully();
        SoundManager.PlaySound(AudioRef.Door);

        TexturePainter.PauseGame();
        yield return OpenDoor(); 
            
        TexturePainter.ResetTexture();
        MapViewer.Activate(); // allow map input

        yield return MonologueManager.RunMonologue(mapIntroNode);
        MapViewer.ActivateElementsMonologue(); // allow monologue after click on element
        
        yield return Utility.WaitUntilEvent(MapViewer.OnRequiredElementFound);
        
        MapViewer.Deactivate(.5f);
        //TexturePainter.ResetTexture();
        SoundManager.DoPauseMusic(AudioRef.Ambient, 1f);
        yield return CloseDoor();
    }

    private IEnumerator RunPaintingPhase(int phaseIndex, MonologueNodeName paintingDoneNode)
    {
        // start painting
        TexturePainter.StartGame(phaseIndex);

        // wait until most is painted
        yield return Utility.WaitUntilEvent(TexturePainter.OnPaintingComplete);
        
        SoundManager.PlaySound(AudioRef.Chime);
        
        // run dialogue, which will signify you can open the door in first 3 phases
        yield return MonologueManager.RunMonologue(paintingDoneNode);
    }
    
    private IEnumerator WaitForDoorDrag(int phaseIndex)
    {
        MapViewer.StartPhase(phaseIndex); // start next phase before activating door handle
        
        // activate door handle drag
        DoorRight.Activate();

        yield return Utility.WaitUntilEvent(DoorRight.OnDoorOpen);
        DoorRight.Deactivate();
    }


    private IEnumerator OpenDoor(float animTime = .5f)
    {
        DoorLeft.DOMoveX(DoorLeftOpenX, animTime);
        DoorRight.transform.DOMoveX(DoorRightOpenX, animTime);
        yield return new WaitForSeconds(animTime);
    }

    private IEnumerator CloseDoor(float animTime = .5f)
    {
        DoorLeft.DOMoveX(_doorLeftClosedX, animTime);
        DoorRight.transform.DOMoveX(_doorRightClosedX, animTime);
        yield return new WaitForSeconds(animTime);
    }
}
