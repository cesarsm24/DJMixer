using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Gestiona la reproducción de efectos de sonido mediante AudioClips.
/// Los efectos se reproducen de forma no bloqueante utilizando PlayOneShot.
/// </summary>
public class Effects : MonoBehaviour
{
    #region Serialized Fields

    [Header("Effect Clips")]
    [Tooltip("Array de clips de audio disponibles para efectos de sonido")]
    public AudioClip[] _effects;

    [Header("Audio Configuration")]
    [Tooltip("AudioSource utilizado para reproducir los efectos")]
    public AudioSource _myAudioSource;

    [Header("Mixer Routing")]
    [SerializeField]
    [Tooltip("Grupo del AudioMixer al que se rutean los efectos")]
    private AudioMixerGroup effectsGroup;

    [Header("Playback Settings")]
    [Range(0f, 1f)]
    [SerializeField]
    [Tooltip("Volumen de reproducción de los efectos (0 = silencio, 1 = volumen máximo)")]
    private float effectVolume = 1f;

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

    #endregion

    #region Audio Source Configuration

    /// <summary>
    /// Configura los parámetros del AudioSource para reproducción de efectos.
    /// Establece el ruteo al grupo del AudioMixer si está asignado.
    /// </summary>
    private void ConfigureAudioSource()
    {
        if (_myAudioSource == null)
        {
            Debug.LogError("No hay AudioSource asignado en Effects!");
            return;
        }

        _myAudioSource.playOnAwake = false;
        _myAudioSource.loop = false;
        _myAudioSource.clip = null;
        _myAudioSource.Stop();
        _myAudioSource.volume = effectVolume;

        if (effectsGroup != null)
        {
            _myAudioSource.outputAudioMixerGroup = effectsGroup;
        }
        else
        {
            Debug.LogWarning("EffectsGroup no asignado. Arrastra el grupo 'Effects' del AudioMixer.");
        }
    }

    #endregion

    #region Effect Playback

    /// <summary>
    /// Reproduce un efecto de sonido específico del array de efectos.
    /// Utiliza PlayOneShot para permitir múltiples efectos simultáneos.
    /// </summary>
    /// <param name="numberEffect">Índice del efecto en el array _effects</param>
    public void PlaySong(int numberEffect)
    {
        if (_effects == null || numberEffect < 0 || numberEffect >= _effects.Length)
        {
            Debug.LogWarning($"Efecto {numberEffect} fuera de rango!");
            return;
        }

        if (_effects[numberEffect] == null)
        {
            Debug.LogWarning($"El efecto {numberEffect} está vacío!");
            return;
        }

        _myAudioSource.PlayOneShot(_effects[numberEffect], effectVolume);
    }

    /// <summary>
    /// Detiene todos los efectos en reproducción.
    /// </summary>
    public void StopAllEffects()
    {
        if (_myAudioSource != null)
        {
            _myAudioSource.Stop();
        }
    }

    #endregion

    #region Volume Control

    /// <summary>
    /// Establece el volumen de reproducción de los efectos.
    /// </summary>
    /// <param name="volume">Valor de volumen entre 0 (silencio) y 1 (máximo)</param>
    public void SetEffectVolume(float volume)
    {
        effectVolume = Mathf.Clamp01(volume);
        if (_myAudioSource != null)
        {
            _myAudioSource.volume = effectVolume;
        }
    }

    /// <summary>
    /// Obtiene el volumen actual de los efectos.
    /// </summary>
    /// <returns>Valor de volumen entre 0 y 1</returns>
    public float GetEffectVolume()
    {
        return effectVolume;
    }

    #endregion
}