
Const adModeUnknown = 0
Const adModeRead = 1
Const adModeWrite = 2
Const adModeReadWrite = 3
Const adModeShareDenyRead = 4
Const adModeShareDenyWrite = 8
Const adModeShareExclusive = 12
Const adModeShareDenyNone = 16
Const adCreateOverwrite = 67108864
Const adOpenIfExists = 33554432
Const adCreateCollection = 8192
const adTypeBinary = 1

Dim objADOStream
Dim objCDOKnowledgeDocument
Dim objCDOKnowledgeFolder
Dim Doc
Dim SubFolder
Dim SubPath
Dim nArray
Dim ReturnArrayDocs
Dim ReturnArraySubFolders
Dim DataSourceName
Dim DataSourceSubName
Dim DownLoadPath

set oArgs=Wscript.Arguments

if oArgs.Count < 3 Then
	Wscript.Echo "Usage :  ExportDocs.vbs <DownloadPath> <HostName> <WorkSpace> {optional <WorkSpace_Subfolder>}"
	Wscript.Echo "Usage Example :  ExportDocs.vbs c:\datastore\myworkspacedocs\ myserver myworkspace /mySubfolder/"
	Wscript.Echo "Usage Note 1: DownloadPath must end with '\'"
	Wscript.Echo "Usage Note 2: The <WorkSpace_SubFolder> must exist under the 'Documents' folder"
	Wscript.Quit
Else
' test, whether the host is CScript.exe
	If (Not IsCScript()) Then
		Dim i
		for i = 0 to (oArgs.Count-1)
			strArgs = strArgs & " " & oArgs(i)
		next 
		Set WshShell = WScript.CreateObject("WScript.Shell")
		WshShell.Run "Cscript.exe " & WScript.ScriptFullName & strArgs 
		WScript.Quit            ' terminate this script
	End If
	DataSourceName = "http://" & oArgs(1) & "/" & oArgs(2) & "/shared%20documents/"
	DownLoadPath = oArgs(0)
	If oArgs.Count > 3 Then
		DataSourceName = Left(DataSourceName,Len(DataSourceName)-1) & oArgs(3)
	End If
	'Wscript.Echo DataSourceName
	
	chkFolder(DownLoadPath)
	CreateFolders(DataSourceName)
	ProcessFolder(DataSourceName)
	
	Wscript.Echo "Export Completed"
End If

Function ProcessFolder(DataSource)
	DownloadFolderContents(DataSource)
	DownloadSubFolders(DataSource)
End Function

Function DownloadFolderContents(DSName)	
	wscript.echo "Processing Folder: " & DSName
	ReturnArrayDocs = GetDocsArray(DSName)
		
	For Each Doc in ReturnArrayDocs
		DownloadFile Doc, DSName
	Next
		CloseMetaFile(DSName)
End Function 

Function DownloadFile(DocName, DSName)
	'on error resume next
	WScript.echo "Exporting File: " & DocName
	Dim filename
	Dim dwnLoadPath
	Dim CurrentFolder
	
	If DataSourceName = DSName then
		dwnLoadPath = DownLoadPath
	Else
		CurrentFolder = Replace(DSName,DataSourceName,"")
		CurrentFolder = Replace(CurrentFolder,"/","\")
		dwnLoadPath = DownLoadPath & CurrentFolder & SubFolderName
	End If 
	
	Set objCDOKnowledgeDocument = CreateObject("CDO.KnowledgeDocument")
	subpath = subfolder
	filename = DSName & DocName
	
	objCDOKnowledgeDocument.DataSource.Open filename , , 3, 0
	
	getMetaData objCDOKnowledgeDocument,dwnLoadPath
	
	Set objADOStream = objCDOKnowledgeDocument.OpenStream
	objADOStream.Type = 1 ' adTypeBinary
	objADOStream.SaveToFile dwnLoadPath & DocName', adCreateOverwrite
	Set objADOStream = nothing
	Set objCDOKnowledgeDocument = nothing	
End Function


Function GetDocsArray(DocDataSourceName)
	Dim oFolder
	Dim RS
	Dim DocArray()
	Dim Count 

	Count = 0 
	Set oFolder = CreateObject("CDO.KnowledgeFolder")
	oFolder.DataSource.Open DocDataSourceName
	set RS = CreateObject("ADODB.Recordset")
	set RS = oFolder.Items
	
	WHILE NOT RS.EOF
		' expand the array
        Redim preserve DocArray(Count)
		DocArray(Count) = RS.Fields ("RESOURCE_PARSENAME") 
		Count = Count + 1
	
	RS.MOVENEXT
	WEND
	GetDocsArray = DocArray
End Function


Function DownloadSubFolders(DSName)
	WScript.echo "Processing Subfolders Of:" & DSName 
	on error resume next
	Dim oFolder
	Dim RS
	set oFolder = CreateObject("CDO.KnowledgeFolder")
	set RS = CreateObject("ADODB.Recordset")
	
	oFolder.DataSource.Open DSName
	set RS = oFolder.Subfolders
	
	
	if err.number = 0 then
		WHILE NOT RS.EOF
			SubFolder = RS.Fields ("RESOURCE_PARSENAME")
			SubPath = ""
			'wscript.echo "Loop - Found Subfolder:" & DSName  & RS.Fields ("RESOURCE_PARSENAME") &"/"
			CreateFolders(DSName  & RS.Fields ("RESOURCE_PARSENAME") &"/")
			ProcessFolder(DSName  & RS.Fields ("RESOURCE_PARSENAME") &"/")
			RS.MOVENEXT
		WEND
	end if
End Function

Function CreateFolders(DSName)
	Dim oFolder
	Dim RS
	Dim SubFolderName
	Dim CurrentFolder
	
	
	set oFolder = CreateObject("CDO.KnowledgeFolder")
	set RS = CreateObject("ADODB.Recordset")
	
	oFolder.DataSource.Open DSName
	set RS = oFolder.Subfolders
	
	if err.number = 0 then	
		WHILE NOT RS.EOF
			SubFolderName = RS.Fields ("RESOURCE_PARSENAME")
			If DataSourceName = DSName then
				chkFolder(DownLoadPath & SubFolderName)
			Else
				CurrentFolder = Replace(DSName,DataSourceName,"")
				CurrentFolder = Replace(CurrentFolder,"/","\")
				chkFolder(DownLoadPath & CurrentFolder & SubFolderName)
			End If 
			'wscript.echo "Loop - Found Subfolder:" & DSName  & RS.Fields ("RESOURCE_PARSENAME") &"/"
			
			RS.MOVENEXT
		WEND
	end if
	
End Function

Function chkFolder(fldr)
	Dim fso
	Set fso = CreateObject("Scripting.FileSystemObject")
	
	If (fso.FolderExists(fldr)) Then
		WScript.echo "DownloadPath already exists - attempting to delete..."
		fso.DeleteFolder Left(fldr,Len(fldr)-1), TRUE
	End If
	
	WScript.echo "Creating Folder: " & fldr
	fso.CreateFolder(fldr)	
	
	CreateMetaFile(fldr & "\")
	Set fso = Nothing
End Function

Function chkFile(file)
	Dim fso, f
	Set fso = CreateObject("Scripting.FileSystemObject")
	If (fso.FileExists(file)) Then
		fso.DeleteFile file,TRUE
	End If
	Set fso = Nothing	
End Function

Function IsCScript()
	If (Instr(UCase(WScript.FullName), "CSCRIPT") <> 0) Then
		IsCScript = true
	Else
		IsCScript = false
	End if
End function

Function CreateMetaFile(Fldr)
   Dim fso, mf
   Set fso = CreateObject("Scripting.FileSystemObject")
   Set mf = fso.CreateTextFile(Fldr & "MetaFile.xml", True)
   mf.WriteLine("<?xml version=""1.0"" encoding=""ISO-8859-1""?>")
   mf.WriteLine("<Files>")
   mf.Close
   Set mf = Nothing	
   Set fso = Nothing	
End Function

Function CloseMetaFile(DSName)
	Dim fso, mf
	Dim dwnLoadPath
	Dim CurrentFolder
	
	If DataSourceName = DSName then
		dwnLoadPath = DownLoadPath
	Else
		CurrentFolder = Replace(DSName,DataSourceName,"")
		CurrentFolder = Replace(CurrentFolder,"/","\")
		dwnLoadPath = DownLoadPath & CurrentFolder & SubFolderName
	End If 
	WScript.echo "Closing MetaFile: " & dwnLoadPath & "MetaFile.xml"
	Set fso = CreateObject("Scripting.FileSystemObject")
	Set mf = fso.OpenTextFile(dwnLoadPath & "MetaFile.xml", 8, True)

	mf.WriteLine("</Files>")
	mf.Close
	Set mf = Nothing	
	Set fso = Nothing	
End Function

Function getMetaData(objCDOKDoc, currFldr)	
	on error resume next
	Dim fso, mf, strFName, strTitle, strAuthor, strDescription
	WScript.echo "Getting MetaData..."
	
	strFName = objCDOKDoc.Property("DAV:displayname")
	strTitle = objCDOKDoc.Property("urn:schemas-microsoft-com:office:office#Title")
	strAuthor = objCDOKDoc.Property("urn:schemas-microsoft-com:office:office#Author") 
	strDescription = objCDOKDoc.Property("urn:schemas-microsoft-com:office:office#Description")
	
	WScript.echo "Updating MetaFile: " & currFldr & "MetaFile.xml"
	Set fso = CreateObject("Scripting.FileSystemObject")
	Set mf = fso.OpenTextFile(currFldr & "MetaFile.xml", 8, True)
	
	mf.WriteLine(Chr(9) & "<File>")
	mf.WriteLine(Chr(9) & Chr(9) & "<Name>" & Replace(strFName,"&","&amp;") & "</Name>")
	mf.WriteLine(Chr(9) & Chr(9) & "<Title>" & Replace(strTitle,"&","&amp;") & "</Title>")
	mf.WriteLine(Chr(9) & Chr(9) & "<Author>" & Replace(strAuthor,"&","&amp;") &  "</Author>")
	mf.WriteLine(Chr(9) & Chr(9) & "<Description>" & Replace(strDescription,"&","&amp;") & "</Description>")
	mf.WriteLine(Chr(9) & "</File>")
	mf.Close
	
	strFName = Nothing
	strTitle = Nothing
	strAuthor = Nothing
	strDescription = Nothing
	Set mf = Nothing	
	Set fso = Nothing
	
End Function