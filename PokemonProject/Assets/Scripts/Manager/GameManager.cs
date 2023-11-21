using System;
using SO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;


        public PokemonSO[] debugStartPokemon;

        public PokemonInstance[] team { get; set; }= new PokemonInstance[6];

        private void Awake()
        {
            if (Instance != null) DestroyImmediate(gameObject);
            else Instance = this;
        }

        private void Start()
        {
            for (int i = 0; i < debugStartPokemon.Length; i++)
            {
                team[i] = new PokemonInstance(debugStartPokemon[i], Random.Range(1, 11));
            }
        }
    }
}