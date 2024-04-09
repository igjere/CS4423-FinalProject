using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CreatureSO")]
public class CreatureSO : ScriptableObject
{

   private void OnEnable() => hideFlags = HideFlags.DontUnloadUnusedAsset;
   // public float health = 0;
   public float currentHealth = 0;
   public float maxHealth = 0;
   public float stamina = 0;
}
