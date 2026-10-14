namespace AMBAR.Semana4;

// Interfaces de la semana 4; no escriben ni simulan registros académicos.
internal sealed class GradesForm : Form
{
    public GradesForm(Session session,bool history)
    {
        if(!session.IsTeacher)throw new UnauthorizedAccessException("Acceso exclusivo para docentes.");
        Theme.Setup(this,history?"Historial de calificaciones":"Captura de calificaciones",900,680);
        MinimumSize=Size;
        var frame=new RoundedPanel {Dock=DockStyle.Fill,Radius=32,BorderColor=Theme.Gray,Padding=new Padding(16)};
        Controls.Add(frame);
        var logo=Theme.Logo(196,82);logo.Location=new Point(62,12);frame.Controls.Add(logo);
        var menu=new Button {Text="☰",Bounds=new Rectangle(20,28,40,40),FlatStyle=FlatStyle.Flat,Font=new Font("Segoe UI",20)};
        menu.FlatAppearance.BorderSize=0;menu.Click+=(_,_)=>Close();frame.Controls.Add(menu);
        var title=ReferenceUi.Label(frame,"DOCENTE - "+(history?"HISTORIAL DE CALIFICACIONES":"CAPTURA DE CALIFICACIONES"),310,28,565,44,15,true,ContentAlignment.MiddleRight);
        title.ForeColor=Theme.Blue;
        if(history)History(frame);else Capture(frame);
    }
    private static ComboBox Filter(Control parent,string title,string value,int x,int y,int width)
    {
        ReferenceUi.Label(parent,title,x,y,width,32,15,true);
        var combo=Theme.Combo(width);combo.Location=new Point(x,y+42);combo.Items.Add(value);combo.SelectedIndex=0;
        combo.Enabled=false;parent.Controls.Add(combo);return combo;
    }
    private static Button ActionButton(Control parent,string title,int x,int y,int width)
    {
        var button=Theme.Button(title);button.AutoSize=false;button.Bounds=new Rectangle(x,y,width,34);
        button.MinimumSize=Size.Empty;button.Padding=Padding.Empty;ReferenceUi.Round(button);parent.Controls.Add(button);return button;
    }
    private void Capture(Control parent)
    {
        Filter(parent,"Materia","Sin materias cargadas",50,118,232);
        Filter(parent,"Grupo","—",305,118,90);
        Filter(parent,"Periodo","—",420,118,155);
        var rule=new Panel {Bounds=new Rectangle(50,154,720,2),BackColor=Theme.Blue};parent.Controls.Add(rule);rule.BringToFront();
        var units=new List<Button>();
        for(int i=1;i<=4;i++)
        {
            var button=ActionButton(parent,$"Unidad {i}",50+(i-1)*115,220,90);
            button.BackColor=i==2?Theme.Gray:Theme.Sky;button.ForeColor=i==2?Color.White:Theme.Gray;
            units.Add(button);
            button.Click+=(_,_)=> {foreach(var unit in units){unit.BackColor=Theme.Sky;unit.ForeColor=Theme.Gray;}
                button.BackColor=Theme.Gray;button.ForeColor=Color.White;};
        }
        var grid=ReferenceUi.Grid("Número de control","Alumno","Calificación","Estado");
        grid.Bounds=new Rectangle(130,280,650,282);parent.Controls.Add(grid);
        ReferenceUi.Label(parent,"Sin alumnos cargados",290,382,350,40,13,false,ContentAlignment.MiddleCenter).BringToFront();
        ReferenceUi.Label(parent,"Capturadas: —       Promedio del grupo: —",50,600,510,32,10,true);
        var cancel=ActionButton(parent,"Cancelar",590,600,120);cancel.Click+=(_,_)=>Close();
        var save=ActionButton(parent,"Guardar",735,600,120);save.Enabled=false;
        ReferenceUi.Label(parent,"Captura de calificaciones disponible en una próxima entrega.",50,642,800,25,10);
    }
    private static void History(Control parent)
    {
        Filter(parent,"Periodo","—",50,137,145);
        Filter(parent,"Materia","Todas",210,137,238);
        Filter(parent,"Grupo","Todos",470,137,90);
        var unit=Filter(parent,"Unidad","Todas",580,137,90);
        unit.Items.AddRange(new object[]{"1","2","3","4"});unit.Enabled=true;
        var search=ActionButton(parent,"⌕ Buscar",728,181,120);search.Enabled=false;
        ReferenceUi.Stat(parent,"Capturas registradas",80,285,225,65);
        ReferenceUi.Stat(parent,"Promedio general",330,285,225,65);
        ReferenceUi.Stat(parent,"Alumnos aprobados",580,285,225,65);
        var grid=ReferenceUi.Grid("Fecha","Materia","Grupo","Unidad","Alumnos","Promedio");
        var detail=new DataGridViewLinkColumn {Name="Detalle",HeaderText="",Text="Ver detalle",UseColumnTextForLinkValue=true,LinkColor=Theme.Gray};
        grid.Columns.Add(detail);grid.Bounds=new Rectangle(30,413,840,178);parent.Controls.Add(grid);
        ReferenceUi.Label(parent,"Sin capturas registradas para mostrar",185,486,520,42,13,false,ContentAlignment.MiddleCenter).BringToFront();
        ReferenceUi.Label(parent,"Historial de calificaciones disponible en una próxima entrega.",50,628,800,28,10);
    }
}
