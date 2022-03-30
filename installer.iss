; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Stream Overlay"
#define MyAppVersion "0.2.1"
#define MyAppPublisher "VladTheJunior"
#define MyAppExeName "StreamOverlayUpdater.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{E93628EF-15F0-479E-9055-F841240936FE}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
PrivilegesRequired=admin
OutputBaseFilename=Stream Overlay
SetupIconFile=StreamOverlay\Icon.ico
UninstallDisplayIcon=StreamOverlay\Icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"
Name: "portuguese"; MessagesFile: "compiler:Languages\Portuguese.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "publish\Release\net6.0-windows\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Release\net6.0-windows\ColorPicker.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Release\net6.0-windows\LibVLCSharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Release\net6.0-windows\LibVLCSharp.WPF.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Release\net6.0-windows\Microsoft.Xaml.Behaviors.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Release\net6.0-windows\StreamOverlay.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Release\net6.0-windows\StreamOverlay.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Release\net6.0-windows\StreamOverlay.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Release\net6.0-windows\StreamOverlay.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Release\net6.0-windows\StreamOverlay.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Release\net6.0-windows\StreamOverlayUpdater.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Release\net6.0-windows\StreamOverlayUpdater.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Release\net6.0-windows\StreamOverlayUpdater.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Release\net6.0-windows\StreamOverlayUpdater.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Release\net6.0-windows\data\*"; DestDir: "{app}\data"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "publish\Release\net6.0-windows\libvlc\*"; DestDir: "{app}\libvlc"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "publish\netcorecheck_x64.exe"; DestDir: "{tmp}"
Source: "publish\netcorecheck.exe"; DestDir: "{tmp}"
Source: "publish\windowsdesktop-runtime-6.0.3-win-x64.exe"; DestDir: "{tmp}"
Source: "publish\windowsdesktop-runtime-6.0.3-win-x86.exe"; DestDir: "{tmp}"
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{tmp}\windowsdesktop-runtime-6.0.3-win-x86.exe"; Flags: runascurrentuser skipifdoesntexist; Check: (not IsWin64) and NotIsNetCoreInstalled86('Microsoft.NETCore.App 6.0.3')
Filename: "{tmp}\windowsdesktop-runtime-6.0.3-win-x64.exe"; Flags: runascurrentuser skipifdoesntexist; Check: IsWin64 and NotIsNetCoreInstalled64('Microsoft.NETCore.App 6.0.3')
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: runascurrentuser nowait postinstall skipifsilent

[Code]
function NotIsNetCoreInstalled86(const Version: String): Boolean;
var
  ResultCode: Integer;
begin
  if not FileExists(ExpandConstant('{tmp}{\}') + 'netcorecheck.exe') then begin
    ExtractTemporaryFile('netcorecheck.exe');
  end;
  Result := ShellExec('', ExpandConstant('{tmp}{\}') + 'netcorecheck.exe', Version, '', SW_HIDE, ewWaitUntilTerminated, ResultCode) and (ResultCode = 0);
  Result := not Result;
 end;

function NotIsNetCoreInstalled64(const Version: String): Boolean;
var
  ResultCode: Integer;
begin
  if not FileExists(ExpandConstant('{tmp}{\}') + 'netcorecheck_x64.exe') then begin
    ExtractTemporaryFile('netcorecheck_x64.exe');
  end;
  Result := ShellExec('', ExpandConstant('{tmp}{\}') + 'netcorecheck_x64.exe', Version, '', SW_HIDE, ewWaitUntilTerminated, ResultCode) and (ResultCode = 0);
  Result := not Result;
 end;