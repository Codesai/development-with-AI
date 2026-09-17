using InterestApi.Domain;
var line = RegistrationFormatter.ToStorageLine(new Registration
{
    Name = "  Ada  ",
    Email = " ADA@EXAMPLE.COM ",
    Course = " Advanced "
});
var expected = "Ada\tada@example.com\tAdvanced";
if (line != expected) { Console.Error.WriteLine($"CI CONTRACT FAILURE: expected normalized storage line '{expected}', got '{line}'"); return 1; }
Console.WriteLine("CI registration contract: green"); return 0;
