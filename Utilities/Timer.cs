public class Timer{
    private float targetTime;
    private float time = 0f;
    public bool finished {get{return time == targetTime;}}

    public Timer(float targetTimeGiven)
    {
        targetTime = targetTimeGiven;
    }

    public void UpdateTime(float timeToUpadate)
    {
        
        if (time < targetTime)
        {
            time += timeToUpadate;
        }
        else
        {
            time = targetTime;
        }
    }

    public void Reset()
    {
        time = 0f;
    }
}