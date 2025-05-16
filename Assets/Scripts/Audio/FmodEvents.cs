using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FmodEvents : MonoBehaviour
{
    public static FmodEvents instance { get; private set; }

    [field: Header("Sonido Ambiente")]
    [field: Tooltip("Sonido ambiental de fondo que acompaña la experiencia.")]
    [field: SerializeField] public EventReference Ambient { get; private set; }


    [field: Header("Sonidos específicos")]
    [field: Tooltip(" sonido de eventos específicos ej sonidos animales fichas.")]
    [field: SerializeField] public EventReference Sound1 { get; private set; }
    [field: Tooltip("Segundo sonido de evento específico.")]
    [field: SerializeField] public EventReference Sound2 { get; private set; }
    [field: Tooltip("Tercer sonido de evento específico.")]
    [field: SerializeField] public EventReference Sound3 { get; private set; }
    [field: Tooltip("Cuarto sonido de evento específico.")]
    [field: SerializeField] public EventReference Sound4 { get; private set; } 
    [field: Tooltip("Quinto sonido de evento específico.")]
    [field: SerializeField] public EventReference Sound5 { get; private set; }
    [field: Tooltip("Sexto sonido de evento específico.")]
    [field: SerializeField] public EventReference Sound6 { get; private set; }
    [field: Tooltip("Septimo sonido de evento específico.")]
    [field: SerializeField] public EventReference Sound7 { get; private set; }
    [field: Tooltip("Octavo sonido de evento específico.")]
    [field: SerializeField] public EventReference Sound8 { get; private set; }
    [field: SerializeField] public EventReference Sound9 { get; private set; }



    [field: Header("Voice")]
    [field: Tooltip("Aquí se pone el audio de bienvenida de la experiencia/zona.")]
    [field: SerializeField] public EventReference Voice { get; private set; }
    
    [field: Header("Guia")]
    [field: Tooltip("Aquí Va el sonido de la guia moviendose.")]
    [field: SerializeField] public EventReference Guia { get; private set; }



    [field: Header("Button SFX")]
    [field: Tooltip("Sonido cuando el usuario entra en un botón.")]
    [field: SerializeField] public EventReference buttonIn { get; private set; }
    [field: Tooltip("Sonido cuando el usuario sale de un botón.")]
    [field: SerializeField] public EventReference buttonOut { get; private set; }
    [field: Tooltip("Sonido de recompensa o feedback positivo.")]
    [field: SerializeField] public EventReference buttonReward { get; private set; }



    [field: Header("Interacción 1")]
    [field: Tooltip("Fase 1 de la interacción 1.")]
    [field: SerializeField] public EventReference Interaccion1_Fase1 { get; private set; }
    [field: Tooltip("Fase 2 de la interacción 1.")]
    [field: SerializeField] public EventReference Interaccion1_Fase2 { get; private set; }
    [field: Tooltip("Fase 3 de la interacción 1.")]
    [field: SerializeField] public EventReference Interaccion1_Fase3 { get; private set; }
    [field: Tooltip("Fase 4 de la interacción 1.")]
    [field: SerializeField] public EventReference Interaccion1_Fase4 { get; private set; }



    [field: Header("Interacción 2")]
    [field: Tooltip("Fase 1 de la interacción 2.")]
    [field: SerializeField] public EventReference Interaccion2_Fase1 { get; private set; }
    [field: Tooltip("Fase 2 de la interacción 2.")]
    [field: SerializeField] public EventReference Interaccion2_Fase2 { get; private set; }



    [field: Header("Interacción 3")]
    [field: Tooltip("Fase 1 de la interacción 3.")]
    [field: SerializeField] public EventReference Interaccion3_Fase1 { get; private set; }



    [field: Header("Interacción 4")]
    [field: Tooltip("Fase 1 de la interacción 4.")]
    [field: SerializeField] public EventReference Interaccion4_Fase1 { get; private set; }



    [field: Header("Interacción 5")]
    [field: Tooltip("Fase 1 de la interacción 5.")]
    [field: SerializeField] public EventReference Interaccion5_Fase1 { get; private set; }
    [field: Tooltip("Fase 2 de la interacción 5.")]
    [field: SerializeField] public EventReference Interaccion5_Fase2 { get; private set; }
    [field: Tooltip("Fase 3 de la interacción 5.")]
    [field: SerializeField] public EventReference Interaccion5_Fase3 { get; private set; }
    [field: Tooltip("Fase 4 de la interacción 5.")]
    [field: SerializeField] public EventReference Interaccion5_Fase4 { get; private set; }
    [field: Tooltip("Fase 5 de la interacción 5.")]
    [field: SerializeField] public EventReference Interaccion5_Fase5 { get; private set; }



    [field: Header("Interacción 6")]
    [field: Tooltip("Fase 1 de la interacción 6.")]
    [field: SerializeField] public EventReference Interaccion6_Fase1 { get; private set; }



    [field: Header("Interacción 7")]
    [field: Tooltip("Fase 1 de la interacción 7.")]
    [field: SerializeField] public EventReference Interaccion7_Fase1 { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance in the scene");
        }

        instance = this;
    }
}
