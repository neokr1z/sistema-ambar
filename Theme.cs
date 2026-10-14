using System.Drawing.Drawing2D;

namespace AMBAR.Semana4;

internal static class Theme
{
    public static readonly Color Blue = Color.FromArgb(6,55,99);
    public static readonly Color Gray = Color.FromArgb(61,107,134);
    public static readonly Color Light = Color.White;
    public static readonly Color Sky = Color.FromArgb(180,213,234);
    public static readonly Color Ink = Color.FromArgb(34,49,71);
    public static readonly Color Green = Color.FromArgb(53,151,128);

    public static void Setup(Form f, string title, int width=1100, int height=760)
    {
        f.Text = $"AMBAR  |  {title}";
        f.ClientSize = new Size(width,height);
        f.MinimumSize = new Size(Math.Min(width,900),Math.Min(height,650));
        f.BackColor = Light;
        f.Font = new Font("Segoe UI",10);
        f.StartPosition = FormStartPosition.CenterScreen;
    }

    public static Label Text(string value, float size=10, bool bold=false, Color? color=null)
        => new() { Text=value, AutoSize=true, Font=new Font("Segoe UI",size,bold ? FontStyle.Bold : FontStyle.Regular),
            ForeColor=color??Ink, Margin=new Padding(3,5,3,8) };

    public static Button Button(string label, bool primary=true)
        => new() { Text=label, FlatStyle=FlatStyle.Flat, BackColor=primary?Blue:Color.White,
            ForeColor=primary?Color.White:Blue, Padding=new Padding(12,7,12,7), AutoSize=true,
            Cursor=Cursors.Hand, Font=new Font("Segoe UI",10,FontStyle.Bold),
            Margin=new Padding(6), MinimumSize=new Size(115,42) };

    public static TextBox Input(int maxLength=120, bool password=false)
        => new() { Width=350, MaxLength=maxLength, UseSystemPasswordChar=password,
            Font=new Font("Segoe UI",11), Margin=new Padding(3,4,3,12) };

    public static ComboBox Combo(int width=350)
        => new() { Width=width, DropDownStyle=ComboBoxStyle.DropDownList,
            Font=new Font("Segoe UI",10), Margin=new Padding(3,4,3,12) };

    public static TableLayoutPanel Fields(int width=920)
        => new() { AutoSize=true, AutoSizeMode=AutoSizeMode.GrowAndShrink, ColumnCount=2,
            Width=width, Padding=new Padding(16), Dock=DockStyle.Top, BackColor=Color.White };

    public static T Row<T>(TableLayoutPanel table, string label, T value) where T:Control
    {
        table.RowCount++;
        table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        var title=Text(label,10,true,Gray);
        title.MaximumSize=new Size(280,60);
        table.Controls.Add(title,0,table.RowCount-1);
        table.Controls.Add(value,1,table.RowCount-1);
        return value;
    }

    public static PictureBox Logo(int width=285,int height=140)
    {
        var img=new PictureBox { Width=width, Height=height, SizeMode=PictureBoxSizeMode.Zoom,
            BackColor=Color.Transparent };
        var file=Path.Combine(AppContext.BaseDirectory,"Assets","tecnm_logo.png");
        if(File.Exists(file)) img.Image=Image.FromFile(file);
        return img;
    }

    public static Label Heading(string text)
        => new() { Text=text, Dock=DockStyle.Top, Height=60,
            Padding=new Padding(18,15,0,0), Font=new Font("Segoe UI",18,FontStyle.Bold), ForeColor=Blue };

    public static Label Message(string text)
        => new() { Text=text, AutoSize=false, Dock=DockStyle.Fill, TextAlign=ContentAlignment.MiddleCenter,
            Font=new Font("Segoe UI",13), ForeColor=Gray, Padding=new Padding(30) };
}

internal sealed record Choice(int Id, string Name)
{
    public override string ToString() => Name;
}
