namespace AMBAR.Semana4;
// Enlaza controles definidos en Designer.cs; no construye interfaces en ejecución.
internal sealed class FiltrosDocente
{
    private readonly ComboBox period,subject,group;
    private readonly List<Asignacion> all;
    private readonly Action changed;
    private bool loading;
    public Asignacion? Selected=>group.SelectedItem as Asignacion;
    public FiltrosDocente(Sesion actor,ComboBox period,ComboBox subject,ComboBox group,Action changed,int initial=0)
    {
        this.period=period;this.subject=subject;this.group=group;this.changed=changed;
        all=RepositorioDocente.Assignments(actor);
        period.SelectedIndexChanged+=(_,_)=>Subjects();subject.SelectedIndexChanged+=(_,_)=>Groups();
        group.DisplayMember=nameof(Asignacion.Grupo);
        group.SelectedIndexChanged+=(_,_)=>{if(!loading)changed();};
        loading=true;
        foreach(var x in all.DistinctBy(x=>x.PeriodoId))period.Items.Add(new Choice(x.PeriodoId,x.Periodo));
        var chosen=all.FirstOrDefault(x=>x.Id==initial);
        period.SelectedIndex=chosen is null?(period.Items.Count>0?0:-1):all.DistinctBy(x=>x.PeriodoId).ToList().FindIndex(x=>x.PeriodoId==chosen.PeriodoId);
        loading=false;Subjects();
        if(chosen is not null)
        {
            subject.SelectedItem=subject.Items.Cast<Choice>().FirstOrDefault(x=>x.Id==chosen.MateriaId);
            group.SelectedItem=group.Items.Cast<Asignacion>().FirstOrDefault(x=>x.Id==chosen.Id);
        }
    }
    private void Subjects()
    {
        if(loading)return;loading=true;subject.Items.Clear();
        if(period.SelectedItem is Choice p)
            foreach(var x in all.Where(x=>x.PeriodoId==p.Id).DistinctBy(x=>x.MateriaId))subject.Items.Add(new Choice(x.MateriaId,x.Materia));
        if(subject.Items.Count>0)subject.SelectedIndex=0;
        loading=false;Groups();
    }
    private void Groups()
    {
        if(loading)return;loading=true;group.Items.Clear();
        if(period.SelectedItem is Choice p && subject.SelectedItem is Choice m)
            foreach(var x in all.Where(x=>x.PeriodoId==p.Id && x.MateriaId==m.Id))group.Items.Add(x);
        if(group.Items.Count>0)group.SelectedIndex=0;
        loading=false;changed();
    }
}
