using System;
using System.Collections.Generic;
using SO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public PokemonInstance[] team { get; set; }= new PokemonInstance[6];
        public Dictionary<Enums.ObjectType, List<ItemStack>> inventory = new();

        [SerializeField]private PokemonSO[] debugStartPokemon;
        [SerializeField] private ItemSO[] debugStartItem;
        private void Awake()
        {
            if (Instance != null) DestroyImmediate(gameObject);
            else Instance = this;
        }

        private void Start()
        {
            foreach (var objectType in Enum.GetValues(typeof(Enums.ObjectType)))
            {
                inventory.Add((Enums.ObjectType)objectType, new());
            }

            foreach (var item in debugStartItem)
            {
                inventory[item.type].Add(new ItemStack(item, Random.Range(1,15)));
            }
            
            for (int i = 0; i < debugStartPokemon.Length; i++)
            {
                team[i] = new PokemonInstance(debugStartPokemon[i], Random.Range(1, 11));
            }
        }
    }
}