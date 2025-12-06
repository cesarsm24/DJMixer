using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Maneja la interacción del usuario con un control deslizante radial (knob).
/// Detecta eventos de puntero para arrastre y reset mediante click.
/// </summary>
public class RadialSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    #region Private Fields

    /// <summary>
    /// Indica si el puntero está presionado sobre el control.
    /// </summary>
    private bool isPointerDown;

    /// <summary>
    /// Indica si el usuario ha arrastrado el control después de presionarlo.
    /// </summary>
    private bool hasDragged;

    /// <summary>
    /// Referencia al componente CircleSlider asociado.
    /// </summary>
    private CircleSlider circleSlider;

    /// <summary>
    /// Posición inicial del puntero cuando se presiona.
    /// </summary>
    private Vector2 pointerDownPosition;

    /// <summary>
    /// Distancia mínima en pixels para considerar que hay arrastre.
    /// </summary>
    private float dragThreshold = 5f;

    #endregion

    #region Unity Lifecycle Methods

    /// <summary>
    /// Obtiene la referencia al componente CircleSlider en el mismo GameObject.
    /// </summary>
    private void Start()
    {
        circleSlider = GetComponent<CircleSlider>();
    }

    #endregion

    #region Pointer Event Handlers

    /// <summary>
    /// Se invoca cuando el puntero entra en el área del control.
    /// Inicia el seguimiento continuo del puntero.
    /// </summary>
    /// <param name="eventData">Datos del evento del puntero</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(TrackPointer());
    }

    /// <summary>
    /// Se invoca cuando el puntero sale del área del control.
    /// Detiene el seguimiento del puntero.
    /// </summary>
    /// <param name="eventData">Datos del evento del puntero</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(TrackPointer());
    }

    /// <summary>
    /// Se invoca cuando se presiona el puntero sobre el control.
    /// Registra la posición inicial para detectar arrastre.
    /// </summary>
    /// <param name="eventData">Datos del evento del puntero</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        hasDragged = false;
        pointerDownPosition = eventData.position;
    }

    /// <summary>
    /// Se invoca cuando se libera el puntero sobre el control.
    /// Si no hubo arrastre, ejecuta el reset del control.
    /// </summary>
    /// <param name="eventData">Datos del evento del puntero</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;

        if (circleSlider != null)
        {
            circleSlider.OnHandleRelease();

            if (!hasDragged)
            {
                circleSlider.ResetToNeutral();
            }
        }
    }

    #endregion

    #region Pointer Tracking

    /// <summary>
    /// Corrutina que rastrea continuamente la posición del puntero.
    /// Detecta si hay arrastre basándose en la distancia recorrida
    /// y actualiza el CircleSlider si el puntero está presionado.
    /// </summary>
    /// <returns>Enumerador para la corrutina</returns>
    private IEnumerator TrackPointer()
    {
        while (Application.isPlaying)
        {
            if (isPointerDown)
            {
                float distance = Vector2.Distance(Input.mousePosition, pointerDownPosition);
                if (distance > dragThreshold)
                {
                    hasDragged = true;
                }

                if (circleSlider != null)
                {
                    circleSlider.onHandleDrag();
                }
            }
            yield return null;
        }
    }

    #endregion
}