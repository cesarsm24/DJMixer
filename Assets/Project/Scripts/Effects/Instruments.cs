using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Gestiona la reproducción de samples e instrumentos musicales.
/// Soporta pads de batería, loops, one-shots y atajos de teclado.
/// </summary>
public class Instruments : MonoBehaviour
{
    #region Nested Classes

    /// <summary>
    /// Representa un sample individual de instrumento con sus propiedades de reproducción.
    /// </summary>
    [System.Serializable]
    public class InstrumentSample
    {
        [Tooltip("Nombre descriptivo del sample")]
        public string name = "Sample";

        [Tooltip("Clip de audio del sample")]
        public AudioClip clip;

        [Tooltip("Volumen del sample (0-1)")]
        [Range(0f, 1f)]
        public float volume = 1f;

        [Tooltip("Si el sample hace loop automáticamente")]
        public bool loop = false;

        [Tooltip("Tecla rápida para activar el sample")]
        public KeyCode hotkey = KeyCode.None;
    }

    #endregion

    #region Serialized Fields

    [Header("Sample Library")]
    [Tooltip("Array de samples/instrumentos disponibles")]
    public InstrumentSample[] samples;

    [Header("Audio Configuration")]
    [Tooltip("AudioSource dedicado para instrumentos")]
    public AudioSource audioSource;

    [Header("Mixer Routing")]
    [Tooltip("Grupo 'Instruments' del AudioMixer")]
    [SerializeField]
    private AudioMixerGroup instrumentsGroup;

    [Header("Playback Settings")]
    [Tooltip("Permitir múltiples samples simultáneos (PlayOneShot)")]
    [SerializeField]
    private bool allowMultipleSamples = true;

    [Tooltip("Detener sample anterior al reproducir uno nuevo")]
    [SerializeField]
    private bool stopPreviousOnNew = false;

    #endregion

    #region Private Fields

    /// <summary>
    /// Índice del sample actualmente en reproducción. -1 si no hay ninguno.
    /// </summary>
    private int currentSample = -1;

    #endregion

    #region Unity Lifecycle Methods

    /// <summary>
    /// Inicialización temprana del AudioSource.
    /// </summary>
    private void Awake()
    {
        ConfigureAudioSource();
    }

    /// <summary>
    /// Configuración adicional al iniciar el componente.
    /// </summary>
    private void Start()
    {
        ConfigureAudioSource();
    }

    /// <summary>
    /// Procesa los atajos de teclado configurados para cada sample.
    /// </summary>
    private void Update()
    {
        if (samples == null) return;

        for (int i = 0; i < samples.Length; i++)
        {
            if (samples[i].hotkey != KeyCode.None && Input.GetKeyDown(samples[i].hotkey))
            {
                PlayInstrument(i);
            }
        }
    }

    #endregion

    #region Audio Source Configuration

    /// <summary>
    /// Configura los parámetros del AudioSource para reproducción de instrumentos.
    /// Establece el ruteo al grupo del AudioMixer si está asignado.
    /// </summary>
    private void ConfigureAudioSource()
    {
        if (audioSource == null)
        {
            Debug.LogError("No hay AudioSource asignado en Instruments!");
            return;
        }

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.clip = null;
        audioSource.Stop();
        audioSource.time = 0f;

        if (instrumentsGroup != null)
        {
            audioSource.outputAudioMixerGroup = instrumentsGroup;
        }
        else
        {
            Debug.LogWarning("InstrumentsGroup no asignado. Arrastra el grupo 'Instruments' del AudioMixer.");
        }
    }

    #endregion

    #region Playback Control

    /// <summary>
    /// Reproduce un instrumento o sample por su índice.
    /// Utilizable desde botones de UI o código.
    /// </summary>
    /// <param name="index">Índice del sample en el array</param>
    public void PlayInstrument(int index)
    {
        if (samples == null || index < 0 || index >= samples.Length)
        {
            Debug.LogWarning($"Sample {index} fuera de rango!");
            return;
        }

        InstrumentSample sample = samples[index];

        if (sample.clip == null)
        {
            Debug.LogWarning($"El sample {index} ({sample.name}) está vacío!");
            return;
        }

        if (stopPreviousOnNew && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (allowMultipleSamples)
        {
            audioSource.PlayOneShot(sample.clip, sample.volume);
        }
        else
        {
            audioSource.clip = sample.clip;
            audioSource.volume = sample.volume;
            audioSource.loop = sample.loop;
            audioSource.Play();
        }

        currentSample = index;
    }

    /// <summary>
    /// Reproduce un sample en modo loop continuo.
    /// Útil para pads de loop o elementos rítmicos repetitivos.
    /// </summary>
    /// <param name="index">Índice del sample en el array</param>
    public void PlayInstrumentLoop(int index)
    {
        if (samples == null || index < 0 || index >= samples.Length)
        {
            return;
        }

        InstrumentSample sample = samples[index];

        if (sample.clip == null) return;

        audioSource.Stop();
        audioSource.clip = sample.clip;
        audioSource.volume = sample.volume;
        audioSource.loop = true;
        audioSource.Play();

        currentSample = index;
    }

    /// <summary>
    /// Reproduce un sample con un volumen personalizado.
    /// Útil para botones de UI con control dinámico de volumen.
    /// </summary>
    /// <param name="index">Índice del sample en el array</param>
    /// <param name="customVolume">Volumen personalizado entre 0 y 1</param>
    public void PlayWithVolume(int index, float customVolume)
    {
        if (samples == null || index < 0 || index >= samples.Length)
        {
            return;
        }

        InstrumentSample sample = samples[index];

        if (sample.clip == null) return;

        audioSource.PlayOneShot(sample.clip, Mathf.Clamp01(customVolume));
    }

    /// <summary>
    /// Detiene el sample actualmente en reproducción.
    /// </summary>
    public void StopCurrentSample()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = null;
            currentSample = -1;
        }
    }

    #endregion

    #region Toggle Controls

    /// <summary>
    /// Alterna entre reproducir y detener un sample específico.
    /// Si el sample está sonando, lo detiene. Si está detenido, lo reproduce.
    /// </summary>
    /// <param name="index">Índice del sample en el array</param>
    public void ToggleSample(int index)
    {
        if (currentSample == index && audioSource.isPlaying)
        {
            StopCurrentSample();
        }
        else
        {
            PlayInstrument(index);
        }
    }

    /// <summary>
    /// Alterna entre reproducir y detener un loop específico.
    /// Funcionalidad similar a ToggleSample pero específica para loops.
    /// </summary>
    /// <param name="index">Índice del sample en el array</param>
    public void ToggleLoop(int index)
    {
        if (currentSample == index && audioSource.isPlaying)
        {
            StopCurrentSample();
        }
        else
        {
            PlayInstrumentLoop(index);
        }
    }

    #endregion

    #region State Queries

    /// <summary>
    /// Verifica si actualmente hay algún sample reproduciéndose.
    /// </summary>
    /// <returns>True si hay un sample en reproducción, false en caso contrario</returns>
    public bool IsPlaying()
    {
        return audioSource != null && audioSource.isPlaying;
    }

    /// <summary>
    /// Obtiene el índice del sample actualmente en reproducción.
    /// </summary>
    /// <returns>Índice del sample actual, o -1 si no hay ninguno reproduciéndose</returns>
    public int GetCurrentSampleIndex()
    {
        return currentSample;
    }

    #endregion

    #region Volume Control

    /// <summary>
    /// Establece el volumen global para todos los samples.
    /// </summary>
    /// <param name="volume">Valor de volumen entre 0 y 1</param>
    public void SetGlobalVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = Mathf.Clamp01(volume);
        }
    }

    #endregion
}