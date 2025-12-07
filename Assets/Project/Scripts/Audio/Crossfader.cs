using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Controla el crossfade entre dos pistas de audio (Disco_01 y Disco_02)
/// mediante un slider que ajusta ambos volúmenes inversamente.
/// </summary>
public class Crossfader : MonoBehaviour
{
    #region Serialized Fields

    [Header("Audio Mixer")]
    [SerializeField]
    [Tooltip("AudioMixer utilizado para controlar los volúmenes de ambos discos")]
    private AudioMixer audioMixer;

    [Header("Crossfade Settings")]
    [Tooltip("Slider que controla el crossfade (0 = solo Disco_01, 1 = solo Disco_02, 0.5 = ambos al máximo)")]
    [SerializeField]
    private Slider crossfadeSlider;

    [Tooltip("Tipo de curva para el crossfade")]
    [SerializeField]
    private CrossfadeMode crossfadeMode = CrossfadeMode.EqualPower;

    [Header("Volume Parameters")]
    [Tooltip("Nombre del parámetro de volumen para Disco_01 en el AudioMixer")]
    [SerializeField]
    private string disco01VolumeParam = "Disco_01_Volume";

    [Tooltip("Nombre del parámetro de volumen para Disco_02 en el AudioMixer")]
    [SerializeField]
    private string disco02VolumeParam = "Disco_02_Volume";

    #endregion

    #region Enums

    /// <summary>
    /// Define los modos disponibles para realizar el crossfade entre pistas.
    /// </summary>
    public enum CrossfadeMode
    {
        /// <summary>
        /// Crossfade lineal simple. Transición directa entre valores.
        /// </summary>
        Linear,

        /// <summary>
        /// Curva de potencia igual. Transición más suave, típica en mezcladores DJ profesionales.
        /// Mantiene la potencia acústica constante durante la transición.
        /// </summary>
        EqualPower,

        /// <summary>
        /// Curva logarítmica. Transición más natural al oído humano.
        /// </summary>
        Logarithmic
    }

    #endregion

    #region Unity Lifecycle Methods

    /// <summary>
    /// Inicializa el crossfader, valida referencias y establece el estado inicial.
    /// </summary>
    private void Start()
    {
        if (audioMixer == null)
        {
            Debug.LogError("AudioMixer no está asignado en el Crossfader!");
            return;
        }

        if (crossfadeSlider == null)
        {
            Debug.LogError("Crossfade Slider no está asignado en el Crossfader!");
            return;
        }

        crossfadeSlider.value = 0.5f;
        crossfadeSlider.onValueChanged.AddListener(OnCrossfadeChanged);

        OnCrossfadeChanged(0.5f);
    }

    /// <summary>
    /// Limpia los listeners al destruir el objeto para evitar memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        if (crossfadeSlider != null)
        {
            crossfadeSlider.onValueChanged.RemoveListener(OnCrossfadeChanged);
        }
    }

    #endregion

    #region Crossfade Control

    /// <summary>
    /// Callback ejecutado cuando cambia el valor del slider de crossfade.
    /// Calcula y aplica los volúmenes correspondientes a ambos discos.
    /// </summary>
    /// <param name="value">Valor del slider entre 0 y 1</param>
    public void OnCrossfadeChanged(float value)
    {
        if (audioMixer == null) return;

        float volume01, volume02;
        CalculateVolumes(value, out volume01, out volume02);

        float dB01 = LinearToDecibel(volume01);
        float dB02 = LinearToDecibel(volume02);

        bool success01 = audioMixer.SetFloat(disco01VolumeParam, dB01);
        bool success02 = audioMixer.SetFloat(disco02VolumeParam, dB02);

        if (!success01)
        {
            Debug.LogWarning($"No se pudo establecer el parámetro '{disco01VolumeParam}'. Verifica que esté expuesto en el AudioMixer.");
        }
        if (!success02)
        {
            Debug.LogWarning($"No se pudo establecer el parámetro '{disco02VolumeParam}'. Verifica que esté expuesto en el AudioMixer.");
        }
    }

    /// <summary>
    /// Calcula los niveles de volumen para ambos discos según la posición del crossfader
    /// y el modo de crossfade seleccionado.
    /// </summary>
    /// <param name="crossfadeValue">Posición del crossfader entre 0 y 1</param>
    /// <param name="volume01">Nivel de volumen resultante para Disco_01</param>
    /// <param name="volume02">Nivel de volumen resultante para Disco_02</param>
    private void CalculateVolumes(float crossfadeValue, out float volume01, out float volume02)
    {
        crossfadeValue = Mathf.Clamp01(crossfadeValue);

        switch (crossfadeMode)
        {
            case CrossfadeMode.Linear:
                volume01 = crossfadeValue;
                volume02 = 1f - crossfadeValue;
                break;

            case CrossfadeMode.EqualPower:
                float angle = crossfadeValue * Mathf.PI * 0.5f;
                volume01 = Mathf.Sin(angle);
                volume02 = Mathf.Cos(angle);
                break;

            case CrossfadeMode.Logarithmic:
                volume01 = Mathf.Pow(crossfadeValue, 2f);
                volume02 = Mathf.Pow(1f - crossfadeValue, 2f);
                break;

            default:
                volume01 = crossfadeValue;
                volume02 = 1f - crossfadeValue;
                break;
        }
    }

    /// <summary>
    /// Convierte un valor lineal de volumen (0-1) a decibelios.
    /// </summary>
    /// <param name="linear">Valor lineal entre 0 y 1</param>
    /// <returns>Valor en decibelios (dB). Retorna -80 dB para valores muy bajos (silencio)</returns>
    private float LinearToDecibel(float linear)
    {
        if (linear <= 0.0001f)
        {
            return -80f;
        }

        return Mathf.Log10(linear) * 20f;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Establece el valor del crossfade programáticamente.
    /// </summary>
    /// <param name="value">Valor entre 0 y 1 para la posición del crossfader</param>
    public void SetCrossfade(float value)
    {
        if (crossfadeSlider != null)
        {
            crossfadeSlider.value = value;
        }
        else
        {
            OnCrossfadeChanged(value);
        }
    }

    /// <summary>
    /// Cambia el modo de crossfade y reaplica la configuración actual.
    /// </summary>
    /// <param name="mode">Nuevo modo de crossfade a utilizar</param>
    public void SetCrossfadeMode(CrossfadeMode mode)
    {
        crossfadeMode = mode;

        if (crossfadeSlider != null)
        {
            OnCrossfadeChanged(crossfadeSlider.value);
        }
    }

    #endregion
}