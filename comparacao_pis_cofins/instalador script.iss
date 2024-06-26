; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Comparação Sped"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "META"
#define MyAppExeName "comparacao_pis_cofins.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{9A874D49-5DFC-4885-88A2-9D6D8DB57BAE}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputDir=C:\Users\Ti03\Documents\Pugramas Ranyel
OutputBaseFilename=Comparação EFD install
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}";

[Files]
Source: "C:\Users\Ti03\source\repos\comparacao_pis_cofins\comparacao_pis_cofins\bin\Debug\net6.0-windows\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Ti03\source\repos\comparacao_pis_cofins\comparacao_pis_cofins\bin\Debug\net6.0-windows\ClosedXML.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Ti03\source\repos\comparacao_pis_cofins\comparacao_pis_cofins\bin\Debug\net6.0-windows\comparacao_pis_cofins.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Ti03\source\repos\comparacao_pis_cofins\comparacao_pis_cofins\bin\Debug\net6.0-windows\comparacao_pis_cofins.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Ti03\source\repos\comparacao_pis_cofins\comparacao_pis_cofins\bin\Debug\net6.0-windows\comparacao_pis_cofins.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Ti03\source\repos\comparacao_pis_cofins\comparacao_pis_cofins\bin\Debug\net6.0-windows\comparacao_pis_cofins.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Ti03\source\repos\comparacao_pis_cofins\comparacao_pis_cofins\bin\Debug\net6.0-windows\comparacao_pis_cofins.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Ti03\source\repos\comparacao_pis_cofins\comparacao_pis_cofins\bin\Debug\net6.0-windows\DocumentFormat.OpenXml.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Ti03\source\repos\comparacao_pis_cofins\comparacao_pis_cofins\bin\Debug\net6.0-windows\ExcelNumberFormat.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Ti03\source\repos\comparacao_pis_cofins\comparacao_pis_cofins\bin\Debug\net6.0-windows\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Ti03\source\repos\comparacao_pis_cofins\comparacao_pis_cofins\bin\Debug\net6.0-windows\query.sql"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Ti03\source\repos\comparacao_pis_cofins\comparacao_pis_cofins\bin\Debug\net6.0-windows\System.Data.Odbc.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Ti03\source\repos\comparacao_pis_cofins\comparacao_pis_cofins\bin\Debug\net6.0-windows\System.Text.Encoding.CodePages.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Ti03\source\repos\comparacao_pis_cofins\comparacao_pis_cofins\bin\Debug\net6.0-windows\runtimes\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

