namespace OnboardingApp.Models;

public static class EmployeeGrades
{
    public const string Associate = "Associate";
    public const string SeniorAssociate = "Senior Associate";
    public const string Manager = "Manager";
    public const string SeniorManager = "Senior Manager";
    public const string TechLead = "Tech Lead";
    public const string Architect = "Architect";
    public const string SeniorArchitect = "Senior Architect";
    public const string VicePresident = "Vice President";
    public const string President = "President";

    public static readonly List<string> All = new()
    {
        Associate,
        SeniorAssociate,
        Manager,
        SeniorManager,
        TechLead,
        Architect,
        SeniorArchitect,
        VicePresident,
        President
    };
}
