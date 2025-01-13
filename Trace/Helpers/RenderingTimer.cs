using System;
using System.Diagnostics;
using System.Windows.Media;

public class RenderingTimer
{
    private Stopwatch stopwatch;

    public RenderingTimer()
    {
        stopwatch = new Stopwatch();
        CompositionTarget.Rendering += OnRendering;
    }

    private void OnRendering(object sender, EventArgs e)
    {
        if (!stopwatch.IsRunning)
        {
            stopwatch.Start();
        }
        else
        {
            stopwatch.Stop();
            Debug.WriteLine($"Rendering time: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Reset();
        }
    }
}
