Import-Module "$PSScriptRoot\bin\Debug\\net472\ScreenSettingsLib.dll"

$cs = [ScreenSettingsLib.MouseCursorSettings]::GetCursorSize()

if (-not $cs.IsConsistent)
{
	Write-Host "CursorSize not Consistent!"
	$userResponse = Read-Host "Ignore? (y/n)"
	if ($userResponse -ne "y")
	{
		return
	}
}


$wantedCursorSize = if ($cs.CursorSizeGrade -gt 2) { 2 } else { 3 }
[ScreenSettingsLib.MouseCursorSettings]::SetCursorSizeGrade($wantedCursorSize)
