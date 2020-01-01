using UnityEngine;

namespace Assets.Scripts
{
    public abstract class OrderedMonoBehaviour : MonoBehaviour
    {
        protected abstract int Order { get; }

        protected bool Initialized { get; private set; } = false;

        void Start()
        {
            TryInitialize();
        }

        void Update()
        {
            if (!Initialized)
            {
                TryInitialize();
            }
            UpdateAction();
        }

        private void TryInitialize()
        {
            if (ClassManager.WaitingToLaunch >= Order)
            {
                Initialize();
                if (ClassManager.WaitingToLaunch == Order) ClassManager.WaitingToLaunch++;
                Initialized = true;
            }
        }

        protected abstract void Initialize();

        protected abstract void UpdateAction();
    }
}
