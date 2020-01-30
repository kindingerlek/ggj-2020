using UnityEditor;
using UnityEngine;

namespace Tools.DebugDraw.Drawables
{
    public abstract class Drawable
    {
        public Drawable()
        {
            ResetState();
        }

        public Color Color { get; set; } = Color.white;
        public bool Active { get; private set; }

        private float _lifeTime;

        public float LifeTime
        {   
            get { return _lifeTime; }
            set
            {
                _lifeTime = value;
                ResetState(); 
            }
        }


        private float currentLifeTime;

        public void ResetState()
        {
            Active = true;
            currentLifeTime = LifeTime;
        }

        protected abstract void OnDraw();
        public void Draw()
        {
            if(!EditorApplication.isPaused)
                Active = Mathf.Clamp((currentLifeTime -= Time.deltaTime),0, LifeTime) > 0;

            Gizmos.color = Color;
            OnDraw();
        }
    }
}