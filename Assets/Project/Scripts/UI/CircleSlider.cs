using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Control deslizante circular (knob) para parámetros de AudioMixer.
/// Soporta rotación mediante drag, reset a valores iniciales y feedback visual.
/// </summary>
public class CircleSlider : MonoBehaviour
{
    #region Serialized Fields

    [Header("Knob Rotation")]
    [SerializeField]
    [Tooltip("Transform del control rotatorio (handle del knob)")]
    private Transform handle;

    [SerializeField]
    [Tooltip("Imagen que representa el relleno visual del control")]
    private Image fill;

    [SerializeField]
    [Tooltip("Texto que muestra el valor actual del parámetro")]
    private Text valtxt;

    [Header("Audio Mixer Settings")]
    [SerializeField]
    [Tooltip("AudioMixer que contiene el parámetro a controlar")]
    protected AudioMixer myAudioMixer;

    [Tooltip("Nombre del parámetro expuesto en el AudioMixer")]
    public string parameterName;

    [Header("Value Range")]
    [Tooltip("Valor mínimo del parámetro")]
    public float minValue = -30f;

    [Tooltip("Valor máximo del parámetro")]
    public float maxValue = 30f;

    [Tooltip("Si es control de frecuencia en lugar de ganancia")]
    public bool isFrequencyControl = false;

    [Header("Initial Knob Parameters")]
    [Tooltip("Rotación inicial del knob en grados")]
    public float RotateKnob = 0f;

    [Tooltip("Cantidad de relleno inicial (0.375 = punto medio)")]
    public float FillAmount = 0.375f;

    [Header("Display Settings")]
    [Tooltip("Texto que se muestra antes del valor (LOW, MID, HIGH, etc)")]
    public string labelText = "";

    [Tooltip("Unidad que se muestra después del valor")]
    public string unit = "dB";

    [Header("Reset on Click")]
    [Tooltip("Permitir reset al hacer click en el centro")]
    public bool enableClickReset = true;

    [Tooltip("Radio en pixels para detectar click de reset")]
    public float resetClickRadius = 30f;

    [Header("Color Feedback")]
    [Tooltip("Activar cambio de color según el valor")]
    public bool useColorFeedback = true;

    [Tooltip("Color para valores positivos (boost)")]
    public Color positiveColor = new Color(0f, 1f, 0.5f);

    [Tooltip("Color para valor neutral (0)")]
    public Color neutralColor = Color.yellow;

    [Tooltip("Color para valores negativos (cut)")]
    public Color negativeColor = Color.red;

    #endregion

    #region Private Fields

    /// <summary>
    /// Posición actual del mouse durante el drag.
    /// </summary>
    private Vector3 mousePos;

    /// <summary>
    /// Valor normalizado actual (0 a 1).
    /// </summary>
    private float currentNormalizedValue;

    /// <summary>
    /// Rotación inicial configurada en el Inspector.
    /// </summary>
    private float initialRotateKnob;

    /// <summary>
    /// Cantidad de relleno inicial configurada en el Inspector.
    /// </summary>
    private float initialFillAmount;

    #endregion

    #region Unity Lifecycle Methods

    /// <summary>
    /// Inicializa el knob con los valores configurados en el Inspector
    /// y aplica el valor inicial al AudioMixer.
    /// </summary>
    private void Start()
    {
        initialRotateKnob = RotateKnob;
        initialFillAmount = FillAmount;

        handle.Rotate(0f, 0f, RotateKnob);
        fill.fillAmount = FillAmount;

        currentNormalizedValue = FillAmount / 0.75f;
        float initialValue = Mathf.Lerp(minValue, maxValue, currentNormalizedValue);

        myAudioMixer.SetFloat(parameterName, initialValue);
        UpdateDisplay();
    }

    #endregion

    #region Drag Control

    /// <summary>
    /// Maneja el evento de arrastre del knob.
    /// Calcula el ángulo basándose en la posición del mouse y actualiza
    /// la rotación, el relleno y el valor del parámetro en el AudioMixer.
    /// </summary>
    public void onHandleDrag()
    {
        mousePos = Input.mousePosition;
        Vector2 vector = mousePos - handle.position;
        float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
        angle = ((angle <= 0f) ? (360f + angle) : angle);

        if (angle <= 225f || angle >= 315f)
        {
            Quaternion rotation = Quaternion.AngleAxis(angle + 135f, Vector3.forward);
            handle.rotation = rotation;

            angle = ((angle >= 315f) ? (angle - 360f) : angle) + 45f;
            fill.fillAmount = 0.75f - angle / 360f;

            currentNormalizedValue = fill.fillAmount / 0.75f;

            float finalValue = Mathf.Lerp(minValue, maxValue, currentNormalizedValue);

            myAudioMixer.SetFloat(parameterName, finalValue);

            UpdateDisplay();
        }
    }

    /// <summary>
    /// Maneja el evento de liberación del knob.
    /// </summary>
    public void OnHandleRelease()
    {
    }

    #endregion

    #region Reset Functionality

    /// <summary>
    /// Resetea el knob a los valores iniciales configurados en el Inspector.
    /// Restaura rotación, relleno y valor del parámetro en el AudioMixer.
    /// </summary>
    public void ResetToNeutral()
    {
        handle.rotation = Quaternion.Euler(0f, 0f, initialRotateKnob);
        fill.fillAmount = initialFillAmount;

        currentNormalizedValue = initialFillAmount / 0.75f;

        float resetValue = Mathf.Lerp(minValue, maxValue, currentNormalizedValue);

        myAudioMixer.SetFloat(parameterName, resetValue);
        UpdateDisplay();
    }

    #endregion

    #region Display Update

    /// <summary>
    /// Actualiza la visualización del texto y el color del relleno
    /// basándose en el valor actual del parámetro.
    /// </summary>
    private void UpdateDisplay()
    {
        if (valtxt == null) return;

        float displayValue = Mathf.Lerp(minValue, maxValue, currentNormalizedValue);

        if (isFrequencyControl)
        {
            if (displayValue >= 1000f)
            {
                valtxt.text = labelText + "\n" + (displayValue / 1000f).ToString("F1") + " kHz";
            }
            else if (displayValue < 10f)
            {
                valtxt.text = labelText + "\n" + displayValue.ToString("F2") + unit;
            }
            else
            {
                valtxt.text = labelText + "\n" + displayValue.ToString("F0") + " " + unit;
            }
        }
        else
        {
            if (Mathf.Approximately(displayValue, 0f))
            {
                valtxt.text = labelText + "\n0 " + unit;
            }
            else if (displayValue > 0f)
            {
                valtxt.text = labelText + "\n+" + displayValue.ToString("F0") + " " + unit;
            }
            else
            {
                valtxt.text = labelText + "\n" + displayValue.ToString("F0") + " " + unit;
            }
        }

        if (useColorFeedback && fill != null)
        {
            if (currentNormalizedValue > 0.5f)
            {
                float t = (currentNormalizedValue - 0.5f) * 2f;
                fill.color = Color.Lerp(neutralColor, positiveColor, t);
            }
            else if (currentNormalizedValue < 0.5f)
            {
                float t = currentNormalizedValue * 2f;
                fill.color = Color.Lerp(negativeColor, neutralColor, t);
            }
            else
            {
                fill.color = neutralColor;
            }
        }
    }

    #endregion
}