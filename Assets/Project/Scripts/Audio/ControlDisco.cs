using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Controla la reproducción de audio de un disco virtual, incluyendo gestión de clips,
/// controles de reproducción, pitch, volumen y ecualización.
/// </summary>
public class ControlDisco : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    [Tooltip("Mixer de audio utilizado para controlar pitch, volumen y ecualización")]
    protected AudioMixer myAudioMixer;

    [SerializeField]
    [Tooltip("Componente que controla la rotación visual del disco")]
    protected RotateDisk rotateDisk;

    [Tooltip("Lista de clips de audio disponibles para reproducción")]
    public AudioClip[] clips;

    [Tooltip("Renderer del sprite para mostrar la carátula del disco")]
    public SpriteRenderer spriteRenderer;

    [Tooltip("Sprites correspondientes a cada clip de audio")]
    public Sprite[] sprites;

    [Tooltip("Slider utilizado para controlar el pitch")]
    public Slider _slider;

    [Header("UI References")]
    [SerializeField]
    [Tooltip("Botón de play que se muestra cuando el audio está detenido")]
    private GameObject btn_play;

    [SerializeField]
    [Tooltip("Botón de play que se muestra cuando el audio está reproduciendo")]
    private GameObject btn_play_mus;

    [SerializeField]
    [Tooltip("Botón para reiniciar el clip actual")]
    private Button btn_reiniciar;

    [SerializeField]
    [Tooltip("Botón para avanzar al siguiente clip")]
    private Button btn_next;

    [Header("Playback Settings")]
    [Tooltip("Si está marcado, la música no se reproduce automáticamente al iniciar")]
    [SerializeField]
    private bool startPaused = true;

    #endregion

    #region Private Fields

    /// <summary>
    /// Índice del clip actual en el array de clips
    /// </summary>
    private int clipOrder;

    /// <summary>
    /// Transform del texto que muestra el nombre de la canción
    /// </summary>
    protected Transform textSong;

    /// <summary>
    /// Componente AudioSource asociado a este GameObject
    /// </summary>
    protected AudioSource myAudioSource;

    #endregion

    #region Properties

    /// <summary>
    /// Obtiene un clip aleatorio del array de clips disponibles
    /// </summary>
    private AudioClip RandomClip => clips[Random.Range(0, clips.Length)];

    #endregion

    #region Unity Lifecycle Methods

    /// <summary>
    /// Inicialización temprana. Detiene el AudioSource inmediatamente para evitar
    /// reproducción automática no deseada.
    /// </summary>
    private void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
        textSong = base.transform.GetChild(0);

        if (myAudioSource != null)
        {
            myAudioSource.playOnAwake = false;
            myAudioSource.Stop();
            myAudioSource.time = 0f;
        }
    }

    /// <summary>
    /// Configuración inicial del componente. Establece el clip inicial,
    /// configura los parámetros del AudioSource y prepara la interfaz de usuario.
    /// </summary>
    private void Start()
    {
        if (clips == null || clips.Length == 0)
        {
            return;
        }

        clipOrder = 0;

        myAudioSource.playOnAwake = false;
        myAudioSource.loop = true;
        myAudioSource.clip = clips[0];
        myAudioSource.Stop();
        myAudioSource.time = 0f;

        if (sprites != null && sprites.Length > 0 && spriteRenderer != null)
        {
            spriteRenderer.sprite = sprites[0];
        }

        if (textSong != null)
        {
            textSong.GetComponent<Text>().text = clips[clipOrder].name;
        }

        if (startPaused)
        {
            myAudioSource.Stop();
            myAudioSource.time = 0f;
        }

        if (myAudioSource.isPlaying)
        {
            myAudioSource.Stop();
            myAudioSource.time = 0f;
        }

        if (gameObject.name.Contains("Primer"))
        {
            myAudioMixer.SetFloat("UowIlpN", 1f);
        }

        InitializeEQ();
        UpdateUIState();
    }

    /// <summary>
    /// Actualización por frame. Mantiene sincronizado el estado de la interfaz de usuario
    /// con el estado del AudioSource.
    /// </summary>
    private void Update()
    {
        UpdateUIState();

        if (!myAudioSource.isPlaying && myAudioSource.time >= myAudioSource.clip.length - 0.1f && !myAudioSource.loop)
        {
            UpdateUIState();
        }
    }

    #endregion

    #region Audio Mixer Configuration

    /// <summary>
    /// Inicializa los parámetros de ecualización del AudioMixer a sus valores predeterminados.
    /// Configura los valores de frecuencias bajas, medias y altas para ambos discos.
    /// </summary>
    private void InitializeEQ()
    {
        myAudioMixer.SetFloat("Disco_01_Low", 1.0f);
        myAudioMixer.SetFloat("Disco_01_Mid", 1.0f);
        myAudioMixer.SetFloat("Disco_01_High", 1.0f);

        myAudioMixer.SetFloat("Disco_02_Low", 1.0f);
        myAudioMixer.SetFloat("Disco_02_Mid", 1.0f);
        myAudioMixer.SetFloat("Disco_02_High", 1.0f);
    }

    /// <summary>
    /// Establece el pitch del disco 01 basándose en el valor del slider.
    /// Actualiza también la velocidad de rotación visual del disco.
    /// </summary>
    /// <param name="sliderValue">Valor entre 0.5 y 2.0 que representa el pitch</param>
    public void SetPitch01(float sliderValue)
    {
        myAudioMixer.SetFloat("UowIlpN", Mathf.Clamp(sliderValue, 0.5f, 2f));
        if (rotateDisk != null)
        {
            rotateDisk.AdjustSpeedRotation(sliderValue);
        }
    }

    /// <summary>
    /// Establece el pitch del disco 02 basándose en el valor del slider.
    /// Actualiza también la velocidad de rotación visual del disco.
    /// </summary>
    /// <param name="sliderValue">Valor entre 0.5 y 2.0 que representa el pitch</param>
    public void SetPitch02(float sliderValue)
    {
        if (rotateDisk != null)
        {
            rotateDisk.AdjustSpeedRotation(sliderValue);
        }
    }

    /// <summary>
    /// Establece el volumen del disco 01 convirtiendo el valor lineal a decibelios.
    /// </summary>
    /// <param name="sliderValue">Valor lineal entre 0 y 1</param>
    public void SetVolume01(float sliderValue)
    {
        float dB = (sliderValue > 0) ? Mathf.Log10(sliderValue) * 20f : -80f;
        myAudioMixer.SetFloat("tssPsQN", dB);
    }

    /// <summary>
    /// Establece el volumen del disco 02 convirtiendo el valor lineal a decibelios.
    /// </summary>
    /// <param name="sliderValue">Valor lineal entre 0 y 1</param>
    public void SetVolume02(float sliderValue)
    {
        float dB = (sliderValue > 0) ? Mathf.Log10(sliderValue) * 20f : -80f;
        myAudioMixer.SetFloat("vQTuTmL", dB);
    }

    #endregion

    #region UI Management

    /// <summary>
    /// Actualiza el estado visual de los elementos de interfaz de usuario
    /// basándose en el estado actual del AudioSource.
    /// </summary>
    private void UpdateUIState()
    {
        if (btn_play == null || btn_play_mus == null) return;

        bool hasClipLoaded = myAudioSource.clip != null;
        bool isPlaying = myAudioSource.isPlaying;

        if (isPlaying)
        {
            btn_play.SetActive(false);
            btn_play_mus.SetActive(true);
        }
        else
        {
            btn_play.SetActive(true);
            btn_play_mus.SetActive(false);
        }

        if (btn_reiniciar != null)
        {
            btn_reiniciar.interactable = hasClipLoaded;
        }

        if (btn_next != null)
        {
            btn_next.interactable = hasClipLoaded && clips.Length > 1;
        }
    }

    /// <summary>
    /// Fuerza una actualización del estado de la interfaz de usuario.
    /// Método público para uso externo.
    /// </summary>
    public void RefreshUIState()
    {
        UpdateUIState();
    }

    #endregion

    #region Playback Controls

    /// <summary>
    /// Inicia la reproducción del clip actual y ajusta la velocidad de rotación del disco.
    /// </summary>
    public void Play()
    {
        myAudioSource.Play();
        if (rotateDisk != null && _slider != null)
        {
            rotateDisk.AdjustSpeedRotation(_slider.value);
        }
        UpdateUIState();
    }

    /// <summary>
    /// Reinicia el clip actual desde el principio y comienza su reproducción.
    /// Actualiza el sprite correspondiente si está disponible.
    /// </summary>
    public void Reset()
    {
        myAudioSource.clip = clips[clipOrder];
        if (spriteRenderer != null && sprites != null && clipOrder < sprites.Length)
        {
            spriteRenderer.sprite = sprites[clipOrder];
        }
        myAudioSource.time = 0f;
        if (rotateDisk != null && _slider != null)
        {
            rotateDisk.AdjustSpeedRotation(_slider.value);
        }
        myAudioSource.Play();
        UpdateUIState();
    }

    /// <summary>
    /// Pausa la reproducción del audio actual sin reiniciar su posición.
    /// </summary>
    public void PauseMusic()
    {
        if (myAudioSource != null)
        {
            myAudioSource.Pause();
            UpdateUIState();
        }
    }

    /// <summary>
    /// Reanuda la reproducción del audio pausado desde su posición actual.
    /// </summary>
    public void UnPauseMusic()
    {
        if (myAudioSource != null)
        {
            myAudioSource.Play();
            if (rotateDisk != null && _slider != null)
            {
                rotateDisk.AdjustSpeedRotation(_slider.value);
            }
            UpdateUIState();
        }
    }

    /// <summary>
    /// Avanza al siguiente clip en la lista. Si se alcanza el final,
    /// vuelve al primer clip. Actualiza el sprite y el texto correspondientes.
    /// </summary>
    public void NextClip()
    {
        if (rotateDisk != null && _slider != null)
        {
            rotateDisk.AdjustSpeedRotation(_slider.value);
        }

        clipOrder++;
        if (clipOrder >= clips.Length)
        {
            clipOrder = 0;
        }

        myAudioSource.clip = clips[clipOrder];

        if (spriteRenderer != null && sprites != null && clipOrder < sprites.Length)
        {
            spriteRenderer.sprite = sprites[clipOrder];
        }

        if (textSong != null)
        {
            textSong.GetComponent<Text>().text = clips[clipOrder].name;
        }

        myAudioSource.Play();
        UpdateUIState();
    }

    #endregion

    #region Application Management

    /// <summary>
    /// Cierra la aplicación. En el editor, esto no tendrá efecto.
    /// </summary>
    public void ExitApk()
    {
        Application.Quit();
    }

    #endregion
}