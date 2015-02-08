.nuget\nuget pack "src\Plasma.Core\Plasma.Core.csproj" -OutputDirectory artifacts -Build -IncludeReferencedProjects -NonInteractive
.nuget\nuget pack "src\Plasma.Http\Plasma.Http.csproj" -OutputDirectory artifacts -Build -IncludeReferencedProjects -NonInteractive
.nuget\nuget pack "src\Plasma.HttpClient\Plasma.HttpClient.csproj" -OutputDirectory artifacts -Build -IncludeReferencedProjects -NonInteractive
.nuget\nuget pack "src\Plasma.WebDriver\Plasma.WebDriver.csproj" -OutputDirectory artifacts -Build -IncludeReferencedProjects -NonInteractive