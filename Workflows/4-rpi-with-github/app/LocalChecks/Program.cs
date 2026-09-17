using InterestApi.Domain;
var line = RegistrationFormatter.ToStorageLine(new Registration
{
    Name = " Ada ",
    Email = " ada@example.com ",
    Course = " Advanced "
});
if (line != "Ada\tada@example.com\tAdvanced") { Console.Error.WriteLine("LOCAL FAILURE: storage format changed"); return 1; }
Console.WriteLine("local registration checks: green"); return 0;
