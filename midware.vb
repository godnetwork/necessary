Imports System.Runtime.InteropServices

Module AttendanceInterop
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
    Public Structure AttendanceRecord
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=50)>
        Public name As String
        Public id As Integer
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=50)>
        Public timestamp As String
    End Structure

    <DllImport("AttendanceLib.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Sub addAttendanceRecord(ByRef records() As AttendanceRecord, ByRef size As Integer, name As String, id As Integer, timestamp As String)
    End Sub

    <DllImport("AttendanceLib.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Sub insertionSortByName(ByRef records() As AttendanceRecord, size As Integer)
    Private attendanceRecords(99) As AttendanceRecord
    Private recordCount As Integer = 0

    ' Handle Electron requests (JSON via file communication)
    Private Sub ProcessRequest(requestFile As String, responseFile As String)
        Dim requestData As String = File.ReadAllText(requestFile)
        Dim request = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(requestData)

        Select Case request("action").ToString()
            Case "add"
                Dim name = request("name").ToString()
                Dim id = Integer.Parse(request("id").ToString())
                Dim timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                addAttendanceRecord(attendanceRecords, recordCount, name, id, timestamp)
                File.WriteAllText(responseFile, "Record added.")
            Case "sort"
                insertionSortByName(attendanceRecords, recordCount)
                File.WriteAllText(responseFile, "Records sorted.")
            Case "export"
                Dim filename = request("filename").ToString()
                ExportToCSV(filename)
                File.WriteAllText(responseFile, $"Data exported to {filename}.")
            Case Else
                File.WriteAllText(responseFile, "Invalid action.")
        End Select
    End Sub

    ' Export logic
    Private Sub ExportToCSV(filename As String)
        Using writer As New StreamWriter(filename)
            writer.WriteLine("Name,ID,Timestamp")
            For i As Integer = 0 To recordCount - 1
                writer.WriteLine($"{attendanceRecords(i).name},{attendanceRecords(i).id},{attendanceRecords(i).timestamp}")
            Next
        End Using
    End Sub
End Class



