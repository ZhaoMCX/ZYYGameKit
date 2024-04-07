
using System;
using System.Collections.Generic;
namespace ZYYGameKit.Timer
{
    [Serializable]
    public class TimerConfig
    {
        public float Duration;
        public bool IsLooping;
    }
    
    
    public class ZyyTimer
    {
        public float Duration { get; private set; }
        public bool IsLooping { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsActive { get; private set; }

        private float timeRemaining;
        private Action onTimerComplete;

        public ZyyTimer(float duration, bool isLooping, Action onTimerComplete)
        {
            Duration = duration;
            IsLooping = isLooping;
            this.onTimerComplete = onTimerComplete;
            Reset();
        }

        public ZyyTimer(TimerConfig config, Action onTimerComplete)
        {
            Duration = config.Duration;
            IsLooping = config.IsLooping;
            this.onTimerComplete = onTimerComplete;
            Reset();
        }

        public void Update(float deltaTime)
        {
            if (!IsActive || IsCompleted) return;

            timeRemaining -= deltaTime;
            if (timeRemaining <= 0)
            {
                if (IsLooping)
                {
                    Reset();
                }
                else
                {
                    IsCompleted = true;
                    IsActive = false;
                }
                onTimerComplete?.Invoke();
            }
        }
        
        public void Trigger()
        {
            onTimerComplete?.Invoke();
        }

        public void Start()
        {
            IsActive = true;
            IsCompleted = false;
        }

        public void Stop()
        {
            IsActive = false;
        }

        public void Reset()
        {
            timeRemaining = Duration;
            IsActive = true;
            IsCompleted = false;
        }
    }
    
    public class TimerSystem : AbstractSystem
    {
        Dictionary<string, ZyyTimer> timerDict;

        public void RegisterTimer(string key, ZyyTimer timer)
        {
            timerDict.TryAdd(key, timer);
        }
        
        public void StartTimer(string key)
        {
            if (timerDict.TryGetValue(key, out ZyyTimer timer))
            {
                timer.Start();
            }
        }
        
        public void StopTimer(string key)
        {
            if (timerDict.TryGetValue(key, out ZyyTimer timer))
            {
                timer.Stop();
            }
        }
        
        public void ResetTimer(string key)
        {
            if (timerDict.TryGetValue(key, out ZyyTimer timer))
            {
                timer.Reset();
            }
        }

        public override void Init()
        {
            timerDict = new Dictionary<string, ZyyTimer>();
        }
        
        public override void Deinit()
        {
            timerDict.Clear();
            timerDict = null;
        }
    }
}