set UNITY_ASSET_PATH=..\Assets\ZYYGameKit\LuBan\
set LUBAN_DLL=
set CONF_ROOT=

dotnet %LUBAN_DLL%Tools\Luban\Luban.dll ^
    -t all ^
	-c cs-simple-json ^
    -d json ^
	-d text-list ^
    --conf %CONF_ROOT%luban.conf ^
	-x outputCodeDir=%UNITY_ASSET_PATH%GenCode ^
    -x outputDataDir=%UNITY_ASSET_PATH%GenData ^
	-x l10n.provider=default ^
	-x l10n.textFile.path=%CONF_ROOT%Datas\Language.xlsx ^
	-x l10n.textFile.keyFieldName=Key ^
	-x l10n.textListFile=usedLauguageText.txt
	REM -x l10n.textFile.languageFieldName=zh ^
	REM -x l10n.convertTextKeyToValue=1
pause