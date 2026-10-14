namespace AMBAR.Semana4;

internal sealed class StudentData
{
    public string Control { get; set; }="";
    public int AspirantId { get; set; }
    public int PlanId { get; set; }
    public int GroupId { get; set; }
    public string Curp { get; set; }="";
    public string First { get; set; }="";
    public string Paternal { get; set; }="";
    public string Maternal { get; set; }="";
    public DateTime Birth { get; set; }=new DateTime(2005,1,1);
    public string Nss { get; set; }="";
    public string Phone { get; set; }="";
    public string Emergency { get; set; }="";
    public string Allergies { get; set; }="";
    public string Email { get; set; }="";
    public string PersonalEmail { get; set; }="";
    public string Street { get; set; }="";
    public string Exterior { get; set; }="";
    public string Interior { get; set; }="";
    public string Neighborhood { get; set; }="";
    public string Locality { get; set; }="";
    public string Municipality { get; set; }="";
    public string State { get; set; }="";
    public string Postal { get; set; }="";
    public string Career { get; set; }="";
    public string Plan { get; set; }="";
    public string Group { get; set; }="";
    public string Period { get; set; }="";
    public string Status { get; set; }="";
    public string ApplicantNumber { get; set; }="";
    public DateTime? EnrollmentDate { get; set; }

    public string FullName=>$"{First} {Paternal} {Maternal}".Trim();
    public void Validate()
    {
        First=Validation.Mandatory(First,"nombre(s)");
        Paternal=Validation.Mandatory(Paternal,"apellido paterno");
        Maternal=Validation.Mandatory(Maternal,"apellido materno");
        Curp=Validation.Mandatory(Curp,"CURP").ToUpperInvariant();
        if(!Validation.Curp(Curp)) throw new ArgumentException("CURP inválida: verifica sus 18 caracteres y formato.");
        if(Birth.Date>=DateTime.Today || Birth.Date<DateTime.Today.AddYears(-100))
            throw new ArgumentException("La fecha de nacimiento no es válida.");
        if(Phone.Length>0 && !Validation.Phone(Phone))throw new ArgumentException("Teléfono personal: introduce 10 dígitos.");
        if(!Validation.Phone(Emergency))throw new ArgumentException("El teléfono de emergencia debe tener 10 dígitos.");
        Email=Validation.Mandatory(Email,"correo institucional");
        if(!Validation.Email(Email))throw new ArgumentException("El correo institucional no es válido.");
        if(PersonalEmail.Length>0 && !Validation.Email(PersonalEmail))throw new ArgumentException("El correo personal no es válido.");
        Street=Validation.Mandatory(Street,"calle");
        Exterior=Validation.Mandatory(Exterior,"número exterior");
        Neighborhood=Validation.Mandatory(Neighborhood,"colonia");
        Locality=Validation.Mandatory(Locality,"localidad");
        Municipality=Validation.Mandatory(Municipality,"municipio");
        State=Validation.Mandatory(State,"estado");
        if(!Validation.Postal(Postal))throw new ArgumentException("Código postal inválido: introduce 5 dígitos.");
        if(Nss.Length>0 && (!System.Text.RegularExpressions.Regex.IsMatch(Nss,@"^\d{11}$")))
            throw new ArgumentException("El NSS debe tener 11 dígitos o quedar vacío hasta su verificación.");
    }
}

internal sealed record AspirantInfo(int Id,string Curp,string Result,string Status);
