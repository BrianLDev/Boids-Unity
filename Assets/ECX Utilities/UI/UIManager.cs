using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EcxUtilities;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
  [SerializeField] private BoidSettings boidsSettings;
  [SerializeField] private Canvas simulationCanvas;
  [SerializeField] private Canvas optionsMenuCanvas;
  // HUD
  [SerializeField] private Button debugLinesButton;   // Button is hidden when not in editor mode
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
  // OPTIONS MENU
  [SerializeField] private TextMeshProUGUI boidCount;
  [SerializeField] private TextMeshProUGUI simMethod;
  [SerializeField] private Slider countSlider;
  [SerializeField] private Text countSliderValue;
  [SerializeField] private TMP_Dropdown methodDropdown;

  private List<Canvas> canvasList;
  private Canvas currentCanvas;
  private AudioManager audioManager;
  private bool isOptionsUIVisible = false;
  private SimMethod nextSimMethod;

  private void Start()
  {
    // Populate canvasList with all avaialble Canvases
    if (canvasList == null)
      canvasList = new List<Canvas>();
    if (simulationCanvas) canvasList.Add(simulationCanvas);
    if (optionsMenuCanvas) canvasList.Add(optionsMenuCanvas);
    
    if (SceneManager.GetActiveScene().name == "Boids")
      ActivateCanvas(simulationCanvas);

    // Set up listeners on UI sliders
    // HUD
    sepSlider.onValueChanged.AddListener((v) => { sepValue.text = v.ToString("0.00"); });
    alignSlider.onValueChanged.AddListener((v) => { alignValue.text = v.ToString("0.00"); });
    cohSlider.onValueChanged.AddListener((v) => { cohValue.text = v.ToString("0.00"); });
    spdSlider.onValueChanged.AddListener((v) => { spdValue.text = v.ToString("0.00"); });
    forceSlider.onValueChanged.AddListener((v) => { forceValue.text = v.ToString("0.00"); });
    perceptSlider.onValueChanged.AddListener((v) => { perceptValue.text = v.ToString("0.00"); });
    // OPTIONS MENU
    countSlider.onValueChanged.AddListener((v) => { countSliderValue.text = v.ToString(); });
  }

  private void OnEnable()
  {
    audioManager = AudioManager.Instance;
    RefreshUI();
    // Hide Debug lines button if not running in editor
    #if UNITY_EDITOR
      debugLinesButton.gameObject.SetActive(true);
    #else
      debugLinesButton.gameObject.SetActive(false);
    #endif
  }

  public void LoadGame()
  {
    ActivateCanvas(simulationCanvas);
  }

  private IEnumerator ShowUIWithDelay(object[] parameters)
  {
    yield return new WaitForSeconds((float)parameters[1]);
    ActivateCanvas((Canvas)parameters[0]);
    yield return null;
  }

  public void ToggleOptionsMenu()
  {
    RefreshUI();
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

  public void RefreshUI()
  {
    // HUD
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
    // OPTIONS MENU
    boidCount.text = boidsSettings.boidCount.ToString();
    simMethod.text = boidsSettings.simMethod.ToString();
    countSlider.value = boidsSettings.boidCount;
    countSliderValue.text = boidsSettings.boidCount.ToString("0.00");
  }

  public void UpdateDropdownSimMethod(int option) {
    Debug.Log("setting nextSimMethod to " + (SimMethod)option);
    boidsSettings.nextSimMethod = (SimMethod)option;
    Debug.Log(boidsSettings.nextSimMethod.ToString());
  }
}