using System.ComponentModel;
namespace AMBAR.Semana4;

public class BotonRedondeado : Button
{
    private int radius=18;
    [DefaultValue(18)]
    public int Radius { get => radius; set {radius=Math.Max(1,value);UpdateShape();} }
    protected override void OnSizeChanged(EventArgs e) {base.OnSizeChanged(e);UpdateShape();}
    protected override void OnHandleCreated(EventArgs e) {base.OnHandleCreated(e);UpdateShape();}
    private void UpdateShape()
    {
        if(Width<3 || Height<3)return;
        using var path=ComponentesVisuales.Path(new Rectangle(0,0,Width,Height),Radius);
        var old=Region;Region=new Region(path);old?.Dispose();
    }
}