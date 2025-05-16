using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDisolve : MonoBehaviour
{
    [SerializeField] Animator ballDisolver;
    [SerializeField] GameObject waterShader;
    private Collider M_collider;
    private Material water;

    void Start()
    {
        ballDisolver.GetComponent<Animator>();
        water =  waterShader.GetComponent<Renderer>().material;
        M_collider = GetComponent<Collider>();
    }


    
    public void Disolve()
    {
      //  AudioManager.instance.PlayOneShot(FmodEvents.instance.buttonselected, this.transform.position);   
        StartCoroutine(CorutineDisolveWater());
        Invoke("Restore", 10f);
    }

    public IEnumerator CorutineDisolveWater()
    {


        if (water.HasProperty("_Disolve"))
        {
            float dissolveValue = 0f;
            float dissolveSpeed = 0.5f; 

            while (dissolveValue < 1)
            {
                dissolveValue += Time.deltaTime * dissolveSpeed;
                water.SetFloat("_Disolve", Mathf.Clamp01(dissolveValue)); 
                M_collider.enabled = false;
                yield return null; 
            }
            water.SetFloat("_Disolve", 1f);
        }
        else
        {
            Debug.LogWarning("El material no tiene el parámetro _Dissolve.");
        }
    }

    public IEnumerator CorutineRestoreWater()
    {
        if (water.HasProperty("_Disolve"))
        {
            float dissolveValue = 1f;
            float dissolveSpeed = 0.5f;

          
            while (dissolveValue > 0)
            {
                dissolveValue -= Time.deltaTime * dissolveSpeed;
                water.SetFloat("_Disolve", dissolveValue);
                M_collider.enabled = true;
                yield return null; 
            }

           
            water.SetFloat("_Disolve", 0f);

        }
        else
        {
            Debug.LogWarning("El material no tiene el parámetro _Dissolve.");
        }
    }
    void Restore()
    {
        StartCoroutine(CorutineRestoreWater());
    }
}
