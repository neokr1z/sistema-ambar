using MySqlConnector;
using QuestPDF.Infrastructure;

namespace AMBAR.Semana4;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        QuestPDF.Settings.License = LicenseType.Community;
        Application.Run(new LoginForm());
    }
}

internal static class Problems
{
    public static void Show(Exception ex)
    {
        var message = ex is MySqlException m && (m.Number == 1062)
            ? "Ya existe un registro con alguno de los datos únicos (número de control, CURP, correo, etc.)."
            : ex is MySqlException
                ? "No fue posible completar la operación en MySQL. Verifica la conexión, las tablas y los datos.\n\n" + ex.Message
                : ex.Message;
        MessageBox.Show(message,"AMBAR · Verificación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
    }
}