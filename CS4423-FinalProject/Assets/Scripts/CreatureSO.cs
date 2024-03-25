using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CreatureSO")]
public class CreatureSO : ScriptableObject
{

   private void OnEnable() => hideFlags = HideFlags.DontUnloadUnusedAsset;
   public int health = 0;
   public int stamina = 0;
}
