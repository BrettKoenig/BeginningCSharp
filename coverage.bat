.\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user "-target:.\packages\xunit.runner.console.2.1.0\tools\xunit.console.exe" -targetargs:".\BadExample.Test\bin\Debug\BadExample.Test.dll" -searchdirs:".\BadExample.Test\bin\Debug" -excludebyattribute:*.ExcludeFromCodeCoverage* "-filter:+[BadExample.Service]* -[*Test]*"

.\packages\ReportGenerator.2.4.4.0\tools\ReportGenerator.exe "-reports:results.xml" "-targetdir:.\coverage"

pause