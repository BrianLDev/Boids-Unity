using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EcxUtilities;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
  [SerializeField] private BoidSettings boidsSettings;
  // [SerializeField] private Canvas mainMenuCanvas;
  [SerializeField] private Canvas simulationCanvas;
  [SerializeField] private Canvas optionsMenuCanvas;
  // [SerializeField] private Canvas gameOverScreenCanvas;
  [SerializeField] private Slider countSlider;
  [SerializeField] private Text countValue;
  [SerializeField] private Slider sepSlider;
  [SerializeField] private Text sepValue;
  [SerializeField] private Slider alignSlider;
  [SerializeField] private Text alignValue;
  [SerializeField] private Slider cohSlider;
  [SerializeField] private Text cohValue;
  [SerializeField] private Slider spdSlider;
  [SerializeField] private Text spdValue;
  [SerializeField] private Slider forceSlider;
  [SerializeField] private Text forceValue;
  [SerializeField] private Slider perceptSlider;
  [SerializeField] private Text perceptValue;
  private List<Canvas> canvasList;
  private Canvas currentCanvas;
  private AudioManager audioManager;
  private bool isOptionsUIVisible = false;

  private void Start()
  {
    // populate canvasList with all avaialble Canvases
    if (canvasList == null)
      canvasList = new List<Canvas>();
    // if (mainMenuCanvas) canvasList.Add(mainMenuCanvas);
    if (simulationCanvas) canvasList.Add(simulationCanvas);
    if (optionsMenuCanvas) canvasList.Add(optionsMenuCanvas);
    // if (gameOverScreenCanvas) canvasList.Add(gameOverScreenCanvas);
    
    if (SceneManager.GetActiveScene().name == "Boids")
      ActivateCanvas(simulationCanvas);

    // set up listeners on UI sliders
    countSlider.onValueChanged.AddListener((v) => { countValue.text = v.ToString(); });
    sepSlider.onValueChanged.AddListener((v) => { sepValue.text = v.ToString("0.00"); });
    alignSlider.onValueChanged.AddListener((v) => { alignValue.text = v.ToString("0.00"); });
    cohSlider.onValueChanged.AddListener((v) => { cohValue.text = v.ToString("0.00"); });
    spdSlider.onValueChanged.AddListener((v) => { spdValue.text = v.ToString("0.00"); });
    forceSlider.onValueChanged.AddListener((v) => { forceValue.text = v.ToString("0.00"); });
    perceptSlider.onValueChanged.AddListener((v) => { perceptValue.text = v.ToString("0.00"); });
  }

  private void OnEnable()
  {
    audioManager = AudioManager.Instance;
    UpdateUI();
  }

  public void LoadGame()
  {
    // SceneManager.LoadScene(1);
    ActivateCanvas(simulationCanvas);
  }

  // public void LoadGameOverUI() {
  //     Canvas canvas = gameOverScreenCanvas;
  //     float delay = 1.5f;
  //     object[] parameters = new object[2] {canvas, delay};
  //     StartCoroutine("ShowUIWithDelay", parameters);
  // }

  private IEnumerator ShowUIWithDelay(object[] parameters)
  {
    yield return new WaitForSeconds((float)parameters[1]);
    ActivateCanvas((Canvas)parameters[0]);
    yield return null;
  }

  public void ToggleOptionsMenu()
  {
    isOptionsUIVisible = !isOptionsUIVisible;
    optionsMenuCanvas.gameObject.SetActive(isOptionsUIVisible);
    currentCanvas.gameObject.SetActive(!isOptionsUIVisible);
  }

  private void DisableAllCanvases()
  {
    foreach (Canvas canvas in canvasList)
    {
      canvas.gameObject.SetActive(false);
    }
  }

  private void ActivateCanvas(Canvas canvas)
  {
    DisableAllCanvases();
    currentCanvas = canvas;
    currentCanvas.gameObject.SetActive(true);
  }

  public void ReloadScene()
  {
    Scene scene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(scene.name);
  }

  public void ExitGame()
  {
    // TODO: ADD AN "ARE YOU SURE" MESSAGE
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
  }

  // USE THE METHODS BELOW BY ATTACHING THIS SCRIPT TO AN EVENT TRIGGER ON YOUR UI BUTTONS
  public static void PlayMouseOver()
  {
    float pitchRangeUI = AudioManager.PitchRangeUI;
    float pitch = Random.Range(1 - pitchRangeUI / 2, 1 + pitchRangeUI / 2);
    AudioManager.Instance.PlayClip(AudioManager.Instance.UISfxManager.mouseOver, AudioCategory.UI, 1, pitch);
  }
  public static void PlayButtonClick()
  {
    float pitchRangeUI = AudioManager.PitchRangeUI;
    float pitch = Random.Range(1 - pitchRangeUI / 2, 1 + pitchRangeUI / 2);
    AudioManager.Instance.PlayClip(AudioManager.Instance.UISfxManager.buttonClick, AudioCategory.UI, 1, pitch);
  }
  public static void PlayErrorSound()
  {
    float pitchRangeUI = AudioManager.PitchRangeUI;
    float pitch = Random.Range(1 - pitchRangeUI / 2, 1 + pitchRangeUI / 2);
    AudioManager.Instance.PlayClip(AudioManager.Instance.UISfxManager.errorSound, AudioCategory.UI, 1, pitch);
  }

  public void UpdateUI()
  {
    countSlider.value = boidsSettings.totalBoids;
    countValue.text = boidsSettings.totalBoids.ToString("0.00");
    sepSlider.value = boidsSettings.separationStrength;
    sepValue.text = boidsSettings.separationStrength.ToString("0.00");
    alignSlider.value = boidsSettings.alignmentStrength;
    alignValue.text = boidsSettings.alignmentStrength.ToString("0.00");
    cohSlider.value = boidsSettings.cohesionStrength;
    cohValue.text = boidsSettings.cohesionStrength.ToString("0.00");
    spdSlider.value = boidsSettings.speed;
    spdValue.text = boidsSettings.speed.ToString("0.00");
    forceSlider.value = boidsSettings.maxAccel;
    forceValue.text = boidsSettings.maxAccel.ToString("0.00");
    perceptSlider.value = boidsSettings.perceptionRange;
    perceptValue.text = boidsSettings.perceptionRange.ToString("0.00");
  }
}