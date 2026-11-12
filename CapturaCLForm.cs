using System.Data;
using System.Globalization;

namespace AMBAR.Semana4;

// Captura secuencial 5, 9, 13, 17; cierre al guardar semana 17.
public partial class CapturaCLForm : Form
{
    private Sesion actor=null!;
    private FiltrosDocente? filters;
    private int initial,loadedAssignment,loadedWeek;
    private bool loading,dirty;
    private readonly Button imprimirFinal=new();
    private readonly Dictionary<string,decimal?> originals=new();

    public CapturaCLForm()
    {
        InitializeComponent();
        FormClosing+=CheckClosing;
        unit.Visible=false;
        label3.Visible=false;
        label4.Text="Semana de evaluación";
        label4.Width=280;
        window.Width=505;
        heading.Text="Calificaciones por semanas";
        note.Width=620;
        imprimirFinal.Text="Imprimir finales (PDF)";
        imprimirFinal.Size=new Size(214,50);
        imprimirFinal.Location=new Point(680,844);
        imprimirFinal.Anchor=AnchorStyles.Bottom|AnchorStyles.Right;
        imprimirFinal.BackColor=Color.FromArgb(6,55,99);
        imprimirFinal.ForeColor=Color.White;
        imprimirFinal.Font=new Font("Segoe UI",10F,FontStyle.Bold);
        imprimirFinal.FlatStyle=FlatStyle.Flat;
        imprimirFinal.FlatAppearance.BorderSize=0;
        imprimirFinal.Enabled=false;
        imprimirFinal.Click+=ImprimirFinalClick;
        Controls.Add(imprimirFinal);
    }

    internal CapturaCLForm(Sesion session,int assignment=0):this()
    {
        actor=session;
        initial=assignment;
        if(!actor.IsTeacher)throw new UnauthorizedAccessException("Acceso exclusivo para docentes.");
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if(DesignMode || System.ComponentModel.LicenseManager.UsageMode==
            System.ComponentModel.LicenseUsageMode.Designtime)return;
        if(actor is null){Close();return;}
        try
        {
            filters=new FiltrosDocente(actor,period,subject,group,AssignmentChanged,initial);
            AssignmentChanged();
            LoadRoster();
        }
        catch(Exception ex){Problems.Show(ex);}
    }

    private void AssignmentChanged()
    {
        if(filters is null)return;
        loading=true;window.Items.Clear();grid.DataSource=null;
        save.Enabled=false;imprimirFinal.Enabled=false;grid.ReadOnly=true;dirty=false;originals.Clear();
        try
        {
            if(filters.Selected is Asignacion a)
            {
                // La semana 17 es el último corte capturable.
                window.Items.Add(new Choice(5,"Semana 5 · Primera evaluación"));
                window.Items.Add(new Choice(9,"Semana 9 · Segunda evaluación"));
                window.Items.Add(new Choice(13,"Semana 13 · Tercera evaluación"));
                window.Items.Add(new Choice(17,"Semana 17 · Cuarta evaluación y cierre"));
                int recommended=5;
                foreach(int corte in new[]{5,9,13,17})
                {
                    var estado=RepositorioCortesDocente.Estado(actor,a.Id,corte);
                    if(estado.Habilitada)recommended=corte;
                    else break;
                }
                if(RepositorioCortesDocente.EstaCerrada(actor,a.Id))recommended=17;
                for(int i=0;i<window.Items.Count;i++)
                    if((window.Items[i] as Choice)?.Id==recommended)
                    {window.SelectedIndex=i;break;}
                note.Text="Captura en orden 5 → 9 → 13 → 17. La semana 17 cierra definitivamente la materia.";
            }
            else note.Text="No hay materias o grupos asignados a esta cuenta.";
        }
        catch(Exception ex){Problems.Show(ex);}
        finally{loading=false;}
    }

    private void SelectionChanged(object? sender,EventArgs e)
    {
        if(!loading)
        {
            save.Enabled=false;imprimirFinal.Enabled=false;grid.ReadOnly=true;
            note.Text="Pulsa Cargar alumnos para aplicar la selección.";
        }
    }

    private bool Discard()=>!dirty ||
        MessageBox.Show("Hay cambios sin guardar. ¿Descartarlos?","Calificaciones",
            MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes;

    private void LoadClick(object? sender,EventArgs e)=>LoadRoster();

    private void LoadRoster()
    {
        if(grid.IsCurrentCellInEditMode && !grid.EndEdit())return;
        if(!Discard())return;
        loading=true;save.Enabled=false;imprimirFinal.Enabled=false;grid.DataSource=null;
        originals.Clear();dirty=false;
        try
        {
            if(filters?.Selected is not Asignacion a ||
                window.SelectedItem is not Choice selected)
            {
                note.Text="Selecciona una materia, grupo y semana.";
                return;
            }
            DataTable data=RepositorioCortesDocente.Obtener(actor,a.Id,selected.Id);
            foreach(DataRow row in data.Rows)
                originals[(string)row["Control"]]=row.IsNull("Nota")?null:
                    Convert.ToDecimal(row["Nota"]);
            grid.DataSource=data;
            foreach(DataGridViewColumn col in grid.Columns)col.ReadOnly=col.Name!="Nota";
            grid.Columns["Nota"].HeaderText=selected.Id==17?
                "Semana 17 · cierre":"Calificación (0 o 70–100)";
            grid.Columns["Nota"].DefaultCellStyle.Format="0.##";
            grid.Columns["Alumno"].FillWeight=200;
            loadedAssignment=a.Id;loadedWeek=selected.Id;
            var estado=RepositorioCortesDocente.Estado(actor,a.Id,selected.Id);
            bool cerrada=RepositorioCortesDocente.EstaCerrada(actor,a.Id);
            save.Enabled=estado.Habilitada && data.Rows.Count>0;
            imprimirFinal.Enabled=cerrada;
            grid.ReadOnly=!save.Enabled;
            note.Text=estado.Aviso;
            if(cerrada && selected.Id==17)
            {
                var finales=RepositorioCortesDocente.Finales(actor,a.Id);
                grid.DataSource=finales;
                grid.ReadOnly=true;
                save.Enabled=false;
                summary.Text=$"Materia cerrada · {finales.Rows.Count} alumnos · Lista definitiva disponible para PDF.";
                return;
            }
            UpdateSummary();
        }
        catch(Exception ex){Problems.Show(ex);}
        finally{loading=false;}
    }

    private void DirtyChanged(object? sender,EventArgs e)
    {
        if(grid.IsCurrentCellDirty)grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }
    private void ValueChanged(object? sender,DataGridViewCellEventArgs e)
    {
        if(!loading && e.RowIndex>=0){dirty=true;UpdateSummary();}
    }

    private void UpdateSummary()
    {
        if(grid.DataSource is not DataTable data){summary.Text="Sin alumnos.";return;}
        var values=data.Rows.Cast<DataRow>()
            .Where(r=>!r.IsNull("Nota"))
            .Select(r=>Convert.ToDecimal(r["Nota"])).ToArray();
        summary.Text=$"Alumnos: {data.Rows.Count} · Capturadas: {values.Length}"+
            " · Promedio: "+(values.Length==0?"—":
                decimal.Truncate(values.Average()*100m)/100m+"");
    }

    private void SaveClick(object? sender,EventArgs e)
    {
        try
        {
            if(filters?.Selected?.Id!=loadedAssignment ||
                (window.SelectedItem as Choice)?.Id!=loadedWeek)
                throw new ArgumentException("La selección cambió. Recarga la lista.");
            if(!grid.EndEdit())
                throw new ArgumentException("Corrige el valor antes de guardar.");
            var changes=new List<NotaCambio>();
            foreach(DataGridViewRow row in grid.Rows)
            {
                string control=Convert.ToString(row.Cells["Control"].Value)!;
                object? raw=row.Cells["Nota"].Value;
                if(raw is null || raw==DBNull.Value ||
                    string.IsNullOrWhiteSpace(Convert.ToString(raw)))
                {
                    if(originals[control].HasValue)
                        throw new ArgumentException("No puedes borrar una calificación. Escribe el valor corregido.");
                    continue;
                }
                if(!decimal.TryParse(Convert.ToString(raw),
                    NumberStyles.AllowDecimalPoint,CultureInfo.CurrentCulture,out decimal value))
                    throw new ArgumentException("La calificación debe ser un número válido.");
                if(value<0 || value>100 || decimal.Truncate(value*100)!=value*100)
                    throw new ArgumentException("El rango permitido es 0 a 100 con hasta dos decimales.");
                decimal stored=value<70?0:value;
                if(originals[control]!=stored)
                    changes.Add(new NotaCambio(control,value,originals[control]));
            }
            if(loadedWeek==17)
            {
                if(grid.Rows.Count!=changes.Count)
                    throw new ArgumentException("La semana 17 requiere capturar a TODOS los alumnos.");
                if(MessageBox.Show(this,
                    "¿Cerrar definitivamente esta materia? Una vez guardada la semana 17 NO se podrá modificar ninguna calificación anterior ni posterior.",
                    "CIERRE DEFINITIVO",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)!=DialogResult.Yes)
                    return;
            }
            save.Enabled=false;
            RepositorioCortesDocente.Guardar(actor,loadedAssignment,loadedWeek,changes);
            dirty=false;
            MessageBox.Show(loadedWeek==17?
                "Materia cerrada definitivamente. Ya puedes imprimir la lista final.":
                "Calificaciones guardadas. Los valores menores de 70 se registraron como 0.",
                "AMBAR",MessageBoxButtons.OK,MessageBoxIcon.Information);
            LoadRoster();
        }
        catch(Exception ex)
        {
            Problems.Show(ex);
            try {save.Enabled=loadedAssignment>0 && RepositorioCortesDocente.Estado(actor,loadedAssignment,loadedWeek).Habilitada;}
            catch {save.Enabled=false;}
        }
    }

    private void ImprimirFinalClick(object? sender,EventArgs e)
    {
        try
        {
            if(filters?.Selected is not Asignacion a)
                throw new ArgumentException("Selecciona una materia y grupo.");
            var tabla=RepositorioCortesDocente.Finales(actor,a.Id);
            using var dialog=new SaveFileDialog
            {
                Filter="PDF (*.pdf)|*.pdf",
                FileName=$"AMBAR_Finales_{a.Grupo}.pdf"
            };
            if(dialog.ShowDialog(this)!=DialogResult.OK)return;
            CalificacionesFinalesPdf.Create(a.ToString(),tabla,dialog.FileName);
            MessageBox.Show(this,"Lista definitiva exportada a PDF.","AMBAR");
        }
        catch(Exception ex){Problems.Show(ex);}
    }

    private void CheckClosing(object? sender,FormClosingEventArgs e)
    {
        if(grid.IsCurrentCellInEditMode && !grid.EndEdit()){e.Cancel=true;return;}
        if(!Discard())e.Cancel=true;
    }

    private void CloseClick(object? sender,EventArgs e)=>Close();
    private void GridError(object? sender,DataGridViewDataErrorEventArgs e)
    {
        e.ThrowException=false;e.Cancel=true;
        MessageBox.Show("Escribe una calificación numérica válida entre 0 y 100.",
            "Calificación inválida");
    }
}
