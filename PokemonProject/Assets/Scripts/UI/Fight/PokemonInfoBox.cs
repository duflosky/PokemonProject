using System.Threading.Tasks;
using SO;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Fight
{
    public class PokemonInfoBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private Lifebar lifebar;
        [SerializeField] private ExperienceBar expBar;

        [FormerlySerializedAs("lifetickDuration")] [Space] [SerializeField]
        private float lifeTickDuration = 0.02f;

        [Space] 
        [SerializeField] private float jiggleForce;
        [SerializeField] private float jiggleSpeed;

        private bool jiggling;

        private PokemonInstance pokemon;

        public void InitBox(PokemonInstance _pokemon)
        {
            pokemon = _pokemon;
            nameText.text = pokemon.Name;
            levelText.text = $"{pokemon.level}";
            if (hpText) hpText.text = $"{pokemon.currentHp} / {pokemon.maxHp}";
            lifebar.InitBar(pokemon.currentHp, pokemon.maxHp);
            if (expBar) expBar.InitBar(pokemon.currentExp, pokemon.totalExpNeed);
        }

        public async Task UpdateLife()
        {
            var it = pokemon.currentHp >= lifebar.life ? 1 : -1;
            var startTime = Time.time;
            while (pokemon.currentHp != lifebar.life)
            {
                if (Time.time >= startTime + lifeTickDuration)
                {
                    startTime = Time.time;
                    lifebar.life += it;
                    if (hpText) hpText.text = $"{lifebar.life} / {pokemon.maxHp}";
                    lifebar.UpdateBar();
                }

                await Task.Yield();
            }
        }

        public async void StartJiggle()
        {
            jiggling = true;
            bool isUp = false;
            float timer = Time.time;
            while (jiggling)
            {
                if (Time.time >= timer + 10 / jiggleSpeed)
                {
                    transform.position += new Vector3(0, jiggleForce * (isUp ? -1 : 1), 0);
                    isUp = !isUp;
                    timer = Time.time;
                }
                await Task.Yield();
            }
            
            if(isUp)transform.position -= new Vector3(0, jiggleForce, 0);
        }

        public void StopJiggle() => jiggling = false; 


        public async Task UpdateExperience(int exp)
        {
            await expBar.UpdateExperience(exp);
        }
    }
}