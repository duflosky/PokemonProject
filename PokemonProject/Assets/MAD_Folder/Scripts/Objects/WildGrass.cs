using System;
using Manager;
using SO;
using UnityEngine;
using Random = UnityEngine.Random;

public class WildGrass : MonoBehaviour
{
    [SerializeField] private PokemonSO[] pokemons;

    private CharacterController player;

    private void Start()
    {
        player = GameManager.Instance.player;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        player.onInteractionMovement += LaunchFightMovement;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        player.onInteractionMovement -= LaunchFightMovement;
    }

    private void LaunchFightMovement(Vector2 direction)
    {
        var random = Random.Range(0, 100);
        if (random > 25) return;
        player.onInteractionMovement -= LaunchFightMovement;
        PokemonInstance[] enemyPokemons = new PokemonInstance[pokemons.Length];
        for (int index = 0; index < pokemons.Length; index++)
        {
            enemyPokemons[index] = new PokemonInstance(pokemons[index], Random.Range(1, 5));
        }

        var pokemon = enemyPokemons[Random.Range(0, enemyPokemons.Length)];
        FightManager.Instance.LaunchFight(pokemon);
    }
}