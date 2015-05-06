[OutputType([void])]
param(
	[Parameter()]
	$apiKey,
	[Parameter()]
	$version = "1.0.0"
)

& nuget push -ApiKey "$apiKey" .\Dataline.Dguv.Validator.$version.nupkg
