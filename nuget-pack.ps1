[OutputType([void])]
param(
	[Parameter()]
	$version = "1.0.0",
	[Parameter()]
	$config = "Release"
)

& nuget pack .\NuGet\Dguv.Validator.nuspec -Properties "Configuration=$config;clientversion=$version" -Version $version
