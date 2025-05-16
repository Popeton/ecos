using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CountdownSystem : MonoBehaviour
{
    [Header("Configuración UI")]
    [SerializeField] private TextMeshProUGUI[] countdownNumberIndicators; // Cuatro textos
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private float countdownDuration = 20f; // Se ajusta para que dure 20s

    [Header("Eventos")]
    public UnityEngine.Events.UnityEvent OnCountdownComplete;

    private void Start()
    {
        //if (countdownNumberIndicators.Length != 4)
        //{
        //    Debug.LogError("Debe haber exactamente 4 indicadores en countdownNumberIndicators.");
        //    return;
        //}

        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        mainText.gameObject.SetActive(true);
        foreach (var indicator in countdownNumberIndicators)
        {
            indicator.gameObject.SetActive(true);
        }

        mainText.text = "La experiencia está a punto de iniciar. Te invitamos a explorar cada detalle del espacio mientras esperas.";
        float timer = countdownDuration;

        while (timer >= 0)
        {
            int remaining = Mathf.CeilToInt(timer);

            // Asignar el mismo número a los cuatro textos de manera sincronizada
            for (int i = 0; i < countdownNumberIndicators.Length; i++)
            {
                countdownNumberIndicators[i].text = remaining.ToString();
            }

            yield return new WaitForSeconds(1f);
            timer--;
        }

        // Ocultar los textos después de la cuenta regresiva
        mainText.gameObject.SetActive(false);
        foreach (var indicator in countdownNumberIndicators)
        {
            indicator.gameObject.SetActive(false);
        }

        OnCountdownComplete?.Invoke();
    }
}
