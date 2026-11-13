namespace AMBAR.Semana4;

// Botón Enter: avanza al siguiente campo, sin activar botones de guardar.
internal static class NavegacionEnter
{
    public static void Instalar(Form form)
    {
        form.KeyPreview = true;
        form.KeyDown += (sender, e) =>
        {
            if (e.KeyCode != Keys.Enter || e.Control || e.Alt || e.Shift) return;
            Control? current = form.ActiveControl;
            while (current is ContainerControl container && container.ActiveControl != null)
                current = container.ActiveControl;
            if (current is not (TextBox or ComboBox or DateTimePicker)) return;
            if (current is TextBox box && box.Multiline && box.AcceptsReturn) return;
            // Permite que el usuario confirme el elemento desplegado antes de avanzar.
            if (current is ComboBox combo && combo.DroppedDown) { combo.DroppedDown = false; return; }
            var parent = current.Parent;
            while (parent != null && parent is not TabPage) parent = parent.Parent;
            var scope = parent ?? (Control)form;
            scope.SelectNextControl(current, true, true, true, true);
            e.SuppressKeyPress = true;
            e.Handled = true;
        };
    }
}
