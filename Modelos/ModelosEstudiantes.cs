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
        First=Validation.Name(First,"nombre(s)",60);
        Paternal=Validation.Name(Paternal,"apellido paterno");
        Maternal=Validation.Name(Maternal,"apellido materno");
        Curp=Validation.Mandatory(Curp,"CURP").ToUpperInvariant();
        if(!Validation.Curp(Curp)) throw new ArgumentException("CURP inválida: verifica sus 18 caracteres y formato.");
        if(Birth.Date>=DateTime.Today || Birth.Date<Validation.EarliestBirth)
            throw new ArgumentException("La fecha de nacimiento debe estar entre el 01/01/1926 y ayer.");
        Validation.CurpIdentity(Curp,First,Paternal,Maternal,Birth);
        Phone=Contacto.Phone(Phone,"Teléfono personal",optional:true);
        Emergency=Contacto.Phone(Emergency,"Teléfono de emergencia");
        Email=Contacto.Email(Email,"Correo institucional",institutional:true);
        PersonalEmail=Contacto.Email(PersonalEmail,"Correo personal",optional:true);
        Street=Validation.Mandatory(Street,"calle");
        Exterior=Validation.Mandatory(Exterior,"número exterior");
        Neighborhood=Validation.Mandatory(Neighborhood,"colonia");
        Locality=Validation.Mandatory(Locality,"localidad");
        Municipality=Validation.Mandatory(Municipality,"municipio");
        State=Validation.Mandatory(State,"estado");
        if(!Validation.Postal(Postal))throw new ArgumentException("Código postal inválido: introduce 5 dígitos.");
        Nss=SeguridadSocial.Validate(Nss);
    }
}

internal sealed record AspirantInfo(int Id,string Curp,string Result,string Status, int? PeriodId);
