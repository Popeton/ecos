using UnityEngine;
using DG.Tweening;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;
using EPOOutline;
using UnityEngine.SceneManagement;

public class RuneActivationSystem : MonoBehaviour
{
    [System.Serializable]
    public class Rune
    {
        public GameObject runeObject;
        public XRSimpleInteractable interactable;
        public Outlinable outlinable;
        public GameObject crackEffect;
        public bool isRequired;
        public int targetSceneIndex;
        [HideInInspector] public bool isActivated;
    }

    [Header("Configuración")]
    [SerializeField] private Rune[] runes;
    [SerializeField] private float raiseDuration = 2f;

    private int activatedCount;
    private bool interactionsEnabled;
    private bool lastRuneActivated = false;

   // public delegate void LastRuneActivatedHandler();
   // public event LastRuneActivatedHandler OnLastRuneActivated;

    void Start()
    {
        InitializeRunes();
        SetupInteractions();
        string name = GetSceneNameByIndex(1);
        print(name);
    }

    private void InitializeRunes()
    {
        foreach (var rune in runes)
        {
           // StartCoroutine(LowerRune(rune));
            ResetRune(rune);
            
        }
    }

    private void ResetRune(Rune rune)
    {
        rune.interactable.enabled = false;
        rune.outlinable.enabled = false;
        rune.crackEffect.SetActive(false);
        rune.isActivated = false;
       
    }

    private void SetupInteractions()
    {
        foreach (var rune in runes)
        {
            rune.interactable.selectEntered.AddListener(_ => OnRuneSelected(rune));
        }
    }

    public void ActivateRune(int index)
    {
        if (IsValidRune(index) && !runes[index].isActivated)
        {
            StartCoroutine(ActivationProcess(runes[index]));
        }
    }

    private IEnumerator ActivationProcess(Rune rune)
    {
        rune.crackEffect.SetActive(true);
        
        yield return rune.runeObject.transform.DOLocalMoveY(67.21f, raiseDuration)
            .SetEase(Ease.OutQuad)
            .WaitForCompletion();
        AudioManager.instance.PlayOneShot(FmodEvents.instance.Sound5);
        rune.outlinable.enabled = true;
        rune.interactable.enabled = true;
        rune.crackEffect.SetActive(false);
        rune.isActivated = true;
        activatedCount++;

        // Si esta es la última runa activada, espera a que termine el audio antes de continuar
        if (activatedCount == runes.Length)
        {
            lastRuneActivated = true;
           // OnLastRuneActivated?.Invoke();
        }
    }

    public void EnableRequiredInteractions()
    {
        if (!lastRuneActivated) return;

        foreach (var rune in runes)
        {
            if (rune.isRequired)
            {
                rune.interactable.enabled = true;
                rune.outlinable.enabled = true;
            }
        }

        interactionsEnabled = true;
    }
    public void LowerNonRequiredRunes()
    {
        foreach (var rune in runes)
        {
            if (!rune.isRequired) // Solo baja las runas que no son requeridas
            {
                StartCoroutine(LowerRune(rune));
            }
        }
    }

    private IEnumerator LowerRune(Rune rune)
    {
        AudioManager.instance.PlayOneShot(FmodEvents.instance.Sound5);
        yield return rune.runeObject.transform.DOLocalMoveY(65.25f, raiseDuration)
            .SetEase(Ease.InQuad)
            .WaitForCompletion();
        
        rune.outlinable.enabled = false;
        rune.interactable.enabled = false;
        rune.isActivated = false;
    }


    private void OnRuneSelected(Rune selectedRune)
    {
        if (interactionsEnabled && selectedRune.isRequired)
        {
            string sceneName = GetSceneNameByIndex(selectedRune.targetSceneIndex);
            Initiate.Fade(sceneName, Color.black, 1f);
           // LoadScene(selectedRune.targetSceneIndex);
        }
    }

    private string GetSceneNameByIndex(int index)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(index);
        if (!string.IsNullOrEmpty(path))
        {
            int lastSlash = path.LastIndexOf('/');
            int lastDot = path.LastIndexOf('.');
            if (lastSlash >= 0 && lastDot >= 0)
            {
                return path.Substring(lastSlash + 1, lastDot - lastSlash - 1);
            }
        }
        return null; // Retorna null si el índice es inválido
    }

    private void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private bool IsValidRune(int index) => index >= 0 && index < runes.Length;
}
