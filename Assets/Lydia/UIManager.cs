using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject soundMenu;

    public RectTransform playButton;
    public RectTransform optionsButton;
    public RectTransform quitButton;

    public RectTransform soundButton;
    public RectTransform backButton;

    public Slider soundSlider; 
    public AudioSource backgroundMusicSource;  
    public AudioSource buttonClickSoundSource; 
    public AudioMixer audioMixer;

    private float defaultVolume = 0.75f;
    public Button resetButton;

    void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        soundMenu.SetActive(false);

        // Button listeners
        playButton.GetComponent<Button>().onClick.AddListener(() => {
            PlayButtonClickSound();
            PlayGame();
        });

        optionsButton.GetComponent<Button>().onClick.AddListener(() => {
            OpenOptionsMenu();
            PlayButtonClickSound();
        });

        quitButton.GetComponent<Button>().onClick.AddListener(() => {
            PlayButtonClickSound();
            QuitGame();
        });

        soundButton.GetComponent<Button>().onClick.AddListener(() => {
            PlayButtonClickSound();
        });

        backButton.GetComponent<Button>().onClick.AddListener(() => {
            PlayButtonClickSound();
            BackToMainMenu();
        });

        resetButton.onClick.AddListener(() => {
            PlayButtonClickSound();
            ResetSoundSettings();
        });

        
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", defaultVolume);
        soundSlider.value = savedVolume;
        AdjustBackgroundMusicVolume(savedVolume);

        soundSlider.onValueChanged.AddListener(AdjustBackgroundMusicVolume);

        backgroundMusicSource.Play();

  
        AnimateButtons();
    }

    
    void PlayButtonClickSound()
    {
        buttonClickSoundSource.Play();
    }

    
    public void PlayGame()
    {
        SceneManager.LoadScene("CutScene");
    }

  
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

 
    public void OpenOptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        DOVirtual.DelayedCall(0.1f, AnimateOptionsMenuButtons);
    }

   
    public void BackToMainMenu()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }


    void AnimateButtons()
    {
        float initialScale = 0.5f;  
        float dropDuration = 0.3f; 

      
        float playButtonTargetY = -283f;
        float optionsButtonTargetY = -35f;
        float quitButtonTargetY = 210f;

  
        playButton.localScale = new Vector3(initialScale, initialScale, 1);  
        playButton.DOAnchorPosY(playButtonTargetY, dropDuration).SetEase(Ease.OutBounce).OnComplete(() => {
            playButton.DOScale(1f, 0.2f); 
        });

       
        optionsButton.localScale = new Vector3(initialScale, initialScale, 1); 
        optionsButton.DOAnchorPosY(optionsButtonTargetY, dropDuration + 0.2f).SetEase(Ease.OutBounce).OnComplete(() => {
            optionsButton.DOScale(1f, 0.2f); 
        });

       
        quitButton.localScale = new Vector3(initialScale, initialScale, 1); 
        quitButton.DOAnchorPosY(quitButtonTargetY, dropDuration + 0.4f).SetEase(Ease.OutBounce).OnComplete(() => {
            quitButton.DOScale(1f, 0.2f); 
        });
    }

    
    void AnimateOptionsMenuButtons()
    {
        float initialScale = 0.5f;
        float dropDuration = 0.3f;

        float soundButtonTargetY = -150f;
        float backButtonTargetY = -250f;

        soundButton.localScale = new Vector3(initialScale, initialScale, 1);
        soundButton.anchoredPosition = new Vector2(soundButton.anchoredPosition.x, 800);  
        soundButton.DOAnchorPosY(soundButtonTargetY, dropDuration).SetEase(Ease.OutBounce).OnComplete(() => {
            soundButton.DOScale(1f, 0.2f);  
        });

        backButton.localScale = new Vector3(initialScale, initialScale, 1);
        backButton.anchoredPosition = new Vector2(backButton.anchoredPosition.x, 800);  
        backButton.DOAnchorPosY(backButtonTargetY, dropDuration + 0.2f).SetEase(Ease.OutBounce).OnComplete(() => {
            backButton.DOScale(1f, 0.2f);  
        });

        AnimateSoundSlider();
    }

    
    void AnimateSoundSlider()
    {
        soundSlider.transform.localScale = Vector3.zero;
        soundSlider.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            Debug.Log("Sound Slider animation complete.");
        });
    }

   
    public void AdjustBackgroundMusicVolume(float volume)
    {
        backgroundMusicSource.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }

    
    public void ResetSoundSettings()
    {
        soundSlider.value = defaultVolume;
        AdjustBackgroundMusicVolume(defaultVolume);
        PlayerPrefs.SetFloat("MasterVolume", defaultVolume);
        PlayerPrefs.Save();
        Debug.Log("Sound settings reset to default.");
    }
}
