
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace AMBAR.Semana4;

public class PanelRedondeado : Panel
{
    private int radius = 20;
    private Color borderColor = Color.FromArgb(61, 107, 134);

    [DefaultValue(20)]
    public int Radius
    {
        get => radius;
        set
        {
            radius = Math.Max(1, value);
            UpdateShape();
            Invalidate();
        }
    }

    [DefaultValue(typeof(Color), "61, 107, 134")]
    public Color BorderColor
    {
        get => borderColor;
        set
        {
            borderColor = value;
            Invalidate();
        }
    }

    public PanelRedondeado()
    {
        DoubleBuffered = true;
        Padding = new Padding(2);
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        UpdateShape();
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        UpdateShape();
    }

    private void UpdateShape()
    {
        if (Width < 4 || Height < 4)
            return;

        using var path = ComponentesVisuales.Path(
            new Rectangle(0, 0, Width - 1, Height - 1),
            radius
        );

        Region? previous = Region;
        Region = new Region(path);
        previous?.Dispose();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (Width < 4 || Height < 4)
            return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        using var path = ComponentesVisuales.Path(
            new Rectangle(1, 1, Width - 3, Height - 3),
            radius
        );

        using var pen = new Pen(borderColor, 1.7f);
        e.Graphics.DrawPath(pen, path);
    }
}