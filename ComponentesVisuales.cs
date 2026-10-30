using System.Drawing.Drawing2D;

namespace AMBAR.Semana4;

internal static class ComponentesVisuales
{
    public static GraphicsPath Path(Rectangle r,int radius)
    {
        int d=Math.Max(2,Math.Min(radius*2,Math.Min(r.Width,r.Height)));
        var path=new GraphicsPath();
        path.AddArc(r.X,r.Y,d,d,180,90);path.AddArc(r.Right-d,r.Y,d,d,270,90);
        path.AddArc(r.Right-d,r.Bottom-d,d,d,0,90);path.AddArc(r.X,r.Bottom-d,d,d,90,90);
        path.CloseFigure();return path;
    }
    public static void Round(Control c,int radius=18)
    {
        void Update()
        {
            if(c.Width<3 || c.Height<3)return;
            using var path=Path(new Rectangle(0,0,c.Width,c.Height),radius);
            var previous=c.Region;c.Region=new Region(path);previous?.Dispose();
        }
        c.SizeChanged+=(_,_)=>Update();Update();
    }
    public static Label Label(Control parent,string text,int x,int y,int w,int h,float size=12,
        bool bold=false,ContentAlignment align=ContentAlignment.MiddleLeft)
    {
        var label=new Label { Text=text,Bounds=new Rectangle(x,y,w,h),ForeColor=Theme.Gray,
            BackColor=Color.Transparent,Font=new Font("Segoe UI",size,bold?FontStyle.Bold:FontStyle.Regular),TextAlign=align };
        parent.Controls.Add(label);return label;
    }
    public static void Input(Control parent,TextBox input,int x,int y,int width)
    {
        var border=new PanelRedondeado { Bounds=new Rectangle(x,y,width,48),Radius=16,BorderColor=Theme.Blue };
        input.BorderStyle=BorderStyle.None;input.Location=new Point(13,13);input.Width=width-26;
        border.Controls.Add(input);parent.Controls.Add(border);
    }
    public static DataGridView Grid(params string[] columns)
    {
        var grid=new DataGridView {BackgroundColor=Color.White,BorderStyle=BorderStyle.None,
            AllowUserToAddRows=false,AllowUserToDeleteRows=false,ReadOnly=true,RowHeadersVisible=false,
            AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill,EnableHeadersVisualStyles=false,
            ColumnHeadersHeight=36,ColumnHeadersHeightSizeMode=DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            GridColor=Color.FromArgb(218,218,218),SelectionMode=DataGridViewSelectionMode.FullRowSelect,
            MultiSelect=false,AutoGenerateColumns=false};
        grid.ColumnHeadersDefaultCellStyle=new DataGridViewCellStyle {BackColor=Color.FromArgb(9,76,115),ForeColor=Color.White,
            Font=new Font("Segoe UI",10,FontStyle.Bold),SelectionBackColor=Color.FromArgb(9,76,115)};
        grid.DefaultCellStyle=new DataGridViewCellStyle {Font=new Font("Segoe UI",10),ForeColor=Color.Black,
            BackColor=Color.White,SelectionBackColor=Theme.Sky,SelectionForeColor=Color.Black,Padding=new Padding(5)};
        grid.BackgroundColor = Color.White;
        grid.BorderStyle = BorderStyle.None;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.EnableHeadersVisualStyles = false;
        grid.GridColor = Color.FromArgb(222, 233, 241);
        grid.RowHeadersVisible = false;
        grid.ColumnHeadersHeight = 48;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(6, 55, 99);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(6, 55, 99);
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 6, 10, 6);
        grid.DefaultCellStyle.BackColor = Color.White;
        grid.DefaultCellStyle.ForeColor = Color.FromArgb(34, 49, 71);
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(215, 234, 247);
        grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(6, 55, 99);
        grid.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
        grid.DefaultCellStyle.Padding = new Padding(10, 6, 10, 6);
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 246, 250);
        grid.RowTemplate.Height = 42;
        grid.RowTemplate.Height=42;
        foreach(var col in columns)grid.Columns.Add(col,col);
        return grid;
    }
    public static Label Stat(Control parent,string title,int x,int y,int width,int height=104)
    {
        var panel=new PanelRedondeado {Bounds=new Rectangle(x,y,width,height),BackColor=Theme.Gray,BorderColor=Theme.Green,Radius=20};
        Round(panel);parent.Controls.Add(panel);
        var caption=Label(panel,title,8,6,width-16,height-45,13,true,ContentAlignment.MiddleCenter);caption.ForeColor=Color.White;
        var value=Label(panel,"—",8,height-40,width-16,32,16,true,ContentAlignment.MiddleCenter);value.ForeColor=Color.White;
        return value;
    }
}