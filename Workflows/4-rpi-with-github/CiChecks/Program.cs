using InterestApi;
var line = RegistrationFormatter.ToStorageLine(new Registration("  Ada  ", " ADA@EXAMPLE.COM ", " Advanced "));
var expected = "Ada\tada@example.com\tAdvanced";
if (line != expected) { Console.Error.WriteLine($"CI CONTRACT FAILURE: expected normalized storage line '{expected}', got '{line}'"); return 1; }
Console.WriteLine("CI registration contract: green"); return 0;
