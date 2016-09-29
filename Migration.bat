@echo off

set arg1=%1
set arg2=%2

set migrateComm=packages\FluentMigrator.1.1.2.1\tools\
set migrateComm1=packages\FluentMigrator.1.1.2.1\tools\migrate -a 
set migrateComm2=Fuel\MITD.Fuel.Data.EF\bin\Debug\MITD.Fuel.Data.EF.dll 
set migrateComm3=-configPath packages\FluentMigrator.1.1.2.1\tools\Migrate.exe.config -db SqlServer2008 -connectionString 

attrib -R %migrateComm%\Migrate.exe.config
copy /Y Migrate.exe.config %migrateComm%\Migrate.exe.config


set ReleaseConn="DataContainer"

set DebugConn="DataContainer"


IF "%arg1%" == "release" ( 
	
	IF "%arg2%"=="u" (
	
	   GOTO ReleaseMigrateUp
	
	) ELSE IF "%arg2%"=="d" (
	
		GOTO ReleaseMigrateDown 
	
	) ELSE GOTO ReleaseCustomArgs

) ELSE IF "%arg1%" == "debug" (
	  
	IF "%arg2%" == "u" (
	
		GOTO DebugMigrateUp 
    
	) ELSE IF "%arg2%"=="d" (
	
		GOTO DebugMigrateDown  
	
	) ELSE GOTO DebugCustomArgs
    
) ElSE IF "%arg1%" == "both" (
	 
	IF "%arg2%" == "u" ( 
	       
		GOTO BothMigrateUp 

    ) ELSE IF "%arg2%"=="d" (
	
		GOTO BothMigrateDown 
	
	) ELSE GOTO BothCustomArgs
) ELSE (
	echo "Invalid Args"
	GOTO End
)



:ReleaseMigrateUp
echo "ReleaseMigrateUp"

IF [%3]==[] (
	%migrateComm1% %migrateComm2%  %migrateComm3% %ReleaseConn%  -profile=%arg1%
) ELSE (
	%migrateComm1% %migrateComm2%  %migrateComm3% %ReleaseConn%  -profile=%arg1%  --version %3
)
GOTO End

:ReleaseMigrateDown
echo "ReleaseMigrateDown"

IF [%3]==[] (
	%migrateComm1% %migrateComm2%  %migrateComm3%  %ReleaseConn% -t migrate:down 
) ELSE ( 
	%migrateComm1% %migrateComm2%  %migrateComm3%  %ReleaseConn% -t rollback:toversion --version %3 
)
GOTO End



:DebugMigrateUp
echo "DebugMigrateUp"

IF [%3]==[] (
	%migrateComm1% %migrateComm2%  %migrateComm3% %DebugConn% -profile=%arg1%
) ELSE ( 
	%migrateComm1% %migrateComm2%  %migrateComm3% %DebugConn% -profile=%arg1%  --version %3
)
GOTO End


:DebugMigrateDown
echo "DebugMigrateDown"
IF [%3]==[] (
	%migrateComm1% %migrateComm2%  %migrateComm3% %DebugConn%  -t migrate:down 
) ELSE ( 
	%migrateComm1% %migrateComm2%  %migrateComm3%  %DebugConn% -t rollback:toversion --version %3 
)
GOTO End



:BothMigrateUp
echo "BothMigrateUp"

IF [%3]==[] (
	%migrateComm1% %migrateComm2%  %migrateComm3%  %ReleaseConn%  -profile=%arg1%
	%migrateComm1% %migrateComm2%  %migrateComm3%  %DebugConn%    -profile=%arg1%
) ELSE (
	%migrateComm1% %migrateComm2%  %migrateComm3%  %ReleaseConn%  -profile=%arg1%  --version %3 
	%migrateComm1% %migrateComm2%  %migrateComm3%  %DebugConn%    -profile=%arg1%  --version %3 
)
GOTO End

:BothMigrateDown
echo "BothMigrateDown"
IF [%3]==[] (
	%migrateComm1% %migrateComm2%  %migrateComm3%  %ReleaseConn%  -t migrate:down 
	%migrateComm1% %migrateComm2%  %migrateComm3%  %DebugConn%    -t migrate:down 
) ELSE ( 
	%migrateComm1% %migrateComm2%  %migrateComm3%  %ReleaseConn%  -t rollback:toversion --version %3
	%migrateComm1% %migrateComm2%  %migrateComm3%  %DebugConn%    -t rollback:toversion --version %3
)
GOTO End




:DebugCustomArgs
echo "DebugCustomArgs"
%migrateComm1% %migrateComm2%  %migrateComm3%  %DebugConn%      %2 %3 %4 %5 %6 %7 %8
GOTO End

:ReleaseCustomArgs
echo "ReleaseCustomArgs"
%migrateComm1% %migrateComm2%  %migrateComm3%  %ReleaseConn%    %2 %3 %4 %5 %6 %7 %8
GOTO End

:BothCustomArgs
echo "BothCustomArgs"
%migrateComm1% %migrateComm2%  %migrateComm3%  %DebugConn%      %2 %3 %4 %5 %6 %7 %8
%migrateComm1% %migrateComm2%  %migrateComm3%  %ReleaseConn%    %2 %3 %4 %5 %6 %7 %8
GOTO End


:End