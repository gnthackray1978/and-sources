
$RootPath = "C:\Users\george\Documents\Visual Studio 2010\Projects\and-source\and-source\Genealogy\TancWebApp"
$DateToCompare = (Get-date).AddDays(-15)

Get-Childitem –recurse |
 where-object {$_.lastwritetime –gt $DateToCompare -and ($_.extension -eq ".js" -or $_.extension -eq ".css" -or $_.extension -eq ".html" -or $_.extension -eq ".dll")} |
 Format-Table @{Expression={$_.FullName.Replace($RootPath,"")}}  > test.txt