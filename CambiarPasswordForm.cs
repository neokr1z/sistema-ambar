namespace AMBAR.Semana4;

public partial class CambiarPasswordForm : Form
{
    private Sesion user = null!;
    private bool mandatory;
    public CambiarPasswordForm() { InitializeComponent(); }
    internal CambiarPasswordForm(Sesion session, bool required) : this() { user=session; mandatory=required; }
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
        if (user is null) { Close(); return; }
    }
    private void PasswordClosing(object? sender, FormClosingEventArgs e)
    {
        if (user != null && mandatory && DialogResult != DialogResult.OK)
            MessageBox.Show("No podrás acceder a los módulos sin cambiar la contraseña temporal.", "AMBAR");
    }

    private void Save(object? sender,EventArgs args)
    {
        try
        {
            if(!Validation.Password(newer.Text))
                throw new ArgumentException("Contraseña nueva inválida: requiere al menos 10 caracteres, letras y números.");
            if(newer.Text!=confirm.Text)throw new ArgumentException("La confirmación no coincide.");
            using var c=Db.Connect();using var tx=c.BeginTransaction();
            string hash;
            using(var get=Db.Sql(c,"SELECT password_hash FROM Usuario WHERE Id_usuario=@id FOR UPDATE",tx))
            {Db.Param(get,"@id",user.Id);hash=Convert.ToString(get.ExecuteScalar())??"";}
            if(!PasswordHasher.Verify(previous.Text,hash))throw new ArgumentException("La contraseña actual es incorrecta.");
            if(PasswordHasher.Verify(newer.Text,hash))throw new ArgumentException("Utiliza una contraseña diferente a la anterior.");
            using(var update=Db.Sql(c,"""
                UPDATE Usuario SET password_hash=@hash,debe_cambiar_password=0 WHERE Id_usuario=@id
                """,tx))
            {Db.Param(update,"@hash",PasswordHasher.Hash(newer.Text));Db.Param(update,"@id",user.Id);update.ExecuteNonQuery();}
            Db.Audit(c,tx,user.Id,"CAMBIO_PASSWORD","USUARIOS","Cambio de contraseña del usuario",user.Username);
            tx.Commit();
            MessageBox.Show("Contraseña actualizada.","AMBAR");
            DialogResult=DialogResult.OK;
            Close();
        }
        catch(Exception ex){Problems.Show(ex);}
    }
}