using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controla la rotación visual de un disco de vinilo y simula el efecto scratch.
/// Sincroniza la velocidad de rotación con el pitch del audio y permite manipulación manual.
/// </summary>
public class RotateDisk : MonoBehaviour
{
    #region Serialized Fields

    [Header("Referencias")]
    [Tooltip("Referencia al controlador del disco")]
    public ControlDisco controlDisco;

    [Tooltip("Slider que controla el pitch/velocidad")]
    public Slider _slider;

    [Header("Velocidad Base del Disco")]
    [Range(50f, 300f)]
    [Tooltip("Velocidad visual base del disco (simula 33 RPM o 45 RPM)")]
    public float baseRotationSpeed = 180f;

    [Header("Configuración de Scratch")]
    [Range(10f, 500f)]
    [Tooltip("Sensibilidad del efecto scratch (mayor = más sensible)")]
    public float scratchSensitivity = 100f;

    #endregion

    #region Private Fields

    /// <summary>
    /// Referencia a la cámara principal.
    /// </summary>
    private Camera myCam;

    /// <summary>
    /// Posición del disco en coordenadas de pantalla.
    /// </summary>
    private Vector3 screenPos;

    /// <summary>
    /// Offset angular inicial al comenzar el arrastre.
    /// </summary>
    private float angleOffset;

    /// <summary>
    /// Indica si el mouse está activo sobre el disco.
    /// </summary>
    private bool MouseActive;

    /// <summary>
    /// Velocidad actual de rotación basada en el pitch.
    /// </summary>
    protected float speedRotation;

    /// <summary>
    /// Ángulo previo del disco, usado para calcular el delta en scratch.
    /// </summary>
    private float previousAngle;

    /// <summary>
    /// Referencia al AudioSource del disco.
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// Indica si el audio estaba reproduciéndose antes de iniciar el scratch.
    /// </summary>
    private bool wasPlayingBeforeDrag;

    /// <summary>
    /// Timestamp del último frame de arrastre.
    /// </summary>
    private float lastDragTime;

    #endregion

    #region Unity Lifecycle Methods

    /// <summary>
    /// Inicializa las referencias a cámara, slider y AudioSource.
    /// </summary>
    private void Start()
    {
        myCam = Camera.main;
        speedRotation = _slider.GetComponent<Slider>().value;
        audioSource = controlDisco.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Actualiza la rotación del disco y maneja la interacción de scratch.
    /// </summary>
    private void Update()
    {
        HandleScratchInput();
        HandleAutoRotation();
    }

    #endregion

    #region Scratch Input Handling

    /// <summary>
    /// Maneja los eventos de entrada del mouse para el efecto scratch.
    /// </summary>
    private void HandleScratchInput()
    {
        if (Input.GetMouseButtonDown(0) && MouseActive)
        {
            InitializeScratch();
        }

        if (Input.GetMouseButton(0) && MouseActive)
        {
            PerformScratch();
        }

        if (Input.GetMouseButtonUp(0) && MouseActive)
        {
            EndScratch();
        }
    }

    /// <summary>
    /// Inicializa las variables necesarias al comenzar el scratch.
    /// </summary>
    private void InitializeScratch()
    {
        screenPos = myCam.WorldToScreenPoint(base.transform.position);
        Vector3 vector = Input.mousePosition - screenPos;
        angleOffset = (Mathf.Atan2(base.transform.right.y, base.transform.right.x) - Mathf.Atan2(vector.y, vector.x)) * 57.29578f;

        previousAngle = base.transform.eulerAngles.z;
        wasPlayingBeforeDrag = audioSource.isPlaying;
        lastDragTime = Time.time;

        if (controlDisco != null)
        {
            controlDisco.RefreshUIState();
        }
    }

    /// <summary>
    /// Ejecuta el scratch mientras el mouse está presionado.
    /// Calcula la rotación del disco y ajusta la posición del audio.
    /// </summary>
    private void PerformScratch()
    {
        Vector3 vector2 = Input.mousePosition - screenPos;
        float num = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
        float newAngle = num + angleOffset;
        base.transform.eulerAngles = new Vector3(0f, 0f, newAngle);

        float currentAngle = base.transform.eulerAngles.z;
        float rotationDelta = Mathf.DeltaAngle(previousAngle, currentAngle);

        if (Time.time - lastDragTime > 0.001f)
        {
            ScratchAudio(-rotationDelta);
            lastDragTime = Time.time;
        }

        previousAngle = currentAngle;
    }

    /// <summary>
    /// Finaliza el scratch y restaura la reproducción si era necesario.
    /// </summary>
    private void EndScratch()
    {
        if (wasPlayingBeforeDrag && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        if (controlDisco != null)
        {
            controlDisco.RefreshUIState();
        }
    }

    /// <summary>
    /// Aplica el efecto scratch al audio modificando su posición temporal
    /// basándose en la rotación manual del disco.
    /// </summary>
    /// <param name="angleDelta">Delta angular de rotación en grados</param>
    private void ScratchAudio(float angleDelta)
    {
        if (audioSource.clip == null) return;

        float revolutionsPerSecond = scratchSensitivity / 360f;
        float timeDelta = (angleDelta / 360f) / revolutionsPerSecond;

        float currentTime = audioSource.time;
        float newTime = currentTime + timeDelta;

        newTime = Mathf.Clamp(newTime, 0f, audioSource.clip.length);

        audioSource.time = newTime;

        if (wasPlayingBeforeDrag && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    #endregion

    #region Auto Rotation

    /// <summary>
    /// Maneja la rotación automática del disco cuando está reproduciendo
    /// y no hay interacción manual. Sincroniza con el pitch actual.
    /// </summary>
    private void HandleAutoRotation()
    {
        if (audioSource.isPlaying && !MouseActive)
        {
            float adjustedSpeed = baseRotationSpeed * speedRotation;
            base.transform.Rotate(0f, 0f, -adjustedSpeed * Time.deltaTime);
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Ajusta la velocidad de rotación del disco basándose en el valor del pitch.
    /// </summary>
    /// <param name="newSpeed">Nuevo valor de velocidad (típicamente entre 0.5 y 2.0)</param>
    public void AdjustSpeedRotation(float newSpeed)
    {
        speedRotation = newSpeed;
    }

    /// <summary>
    /// Activa la interacción del mouse con el disco.
    /// Llamado cuando el mouse entra en el área del disco.
    /// </summary>
    public void OnMouseDown()
    {
        MouseActive = true;
    }

    /// <summary>
    /// Desactiva la interacción del mouse con el disco.
    /// Llamado cuando el mouse sale del área del disco.
    /// </summary>
    public void OnMouseUp()
    {
        MouseActive = false;
    }

    #endregion
}