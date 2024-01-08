using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Timeline;

namespace Objects
{
    public class PokemonSprite : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer { get; private set; }
        private Animation animation;
        [SerializeField] private float jiggleForce;
        [SerializeField] private float jiggleSpeed;
        
        private bool jiggling;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animation = GetComponent<Animation>();
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
                    transform.position += new Vector3(0, jiggleForce * (isUp ? 1 : -1), 0);
                    isUp = !isUp;
                    timer = Time.time;
                }
                await Task.Yield();
            }
            
            if(isUp)transform.position += new Vector3(0, jiggleForce, 0);
        }
        public void StopJiggle() => jiggling = false;

        public async Task Attack()
        {
            animation.clip = animation.GetClip("Attack");
            animation.Play();
            while (animation.isPlaying)
            {
                await Task.Yield();
            }
        }
        
        public async Task UseEffect()
        {
            animation.clip = animation.GetClip("Effect");
            animation.Play();
            while (animation.isPlaying)
            {
                await Task.Yield();
            }
        }
    }
}