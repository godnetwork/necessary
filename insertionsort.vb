Imports System.IO ' Required for file operations (reading and writing files)

Module AttendanceModule
    ' Define a structure to store attendance records
    Structure AttendanceRecord
        Dim Name As String       ' Stores the name of the person
        Dim ID As Integer        ' Stores the ID of the person
        Dim Timestamp As String  ' Stores the timestamp of the attendance record
    End Structure

    ' Function to read attendance records from a file
    ' Parameters:
    ' - filename: The name of the file to read from
    ' - records: An array to store the attendance records
    ' Returns: The number of records successfully read
    Function ReadAttendanceRecords(ByVal filename As String, ByRef records() As AttendanceRecord) As Integer
        Dim size As Integer = 0 ' Counter to track the number of records

        ' Ensure the file exists before attempting to read it
        If Not File.Exists(filename) Then
            Console.WriteLine($"Error: File '{filename}' not found.")
            Return 0 ' Return 0 if the file does not exist
        End If

        ' Read all lines from the file
        Dim lines() As String = File.ReadAllLines(filename)

        ' Loop through the lines, assuming each record spans three lines (ID, Name, Timestamp)
        For i As Integer = 0 To lines.Length - 1 Step 3
            ' Ensure there are at least 3 lines remaining to form a valid record
            If i + 2 < lines.Length Then
                ' Parse the ID (line 1 of the record)
                records(size).ID = CInt(lines(i))

                ' Parse the Name (line 2 of the record)
                records(size).Name = lines(i + 1).Trim()

                ' Parse the Timestamp (line 3 of the record)
                records(size).Timestamp = lines(i + 2).Trim()

                size += 1 ' Increment the record count
            End If
        Next

        Return size ' Return the total number of records read
    End Function

    ' Subroutine to sort attendance records alphabetically by name using insertion sort
    ' Parameters:
    ' - records: The array of attendance records to sort
    ' - size: The number of records in the array
    Sub InsertionSortByName(ByRef records() As AttendanceRecord, size As Integer)
        For i As Integer = 1 To size - 1
            Dim key As AttendanceRecord = records(i) ' Temporarily store the current record
            Dim j As Integer = i - 1 ' Pointer to the last sorted element

            ' Compare names and shift larger elements to the right
            While j >= 0 AndAlso String.Compare(records(j).Name, key.Name) > 0
                records(j + 1) = records(j) ' Shift the larger record to the right
                j -= 1 ' Move to the previous element
            End While

            records(j + 1) = key ' Insert the key record at the correct position
        Next
    End Sub

    ' Subroutine to display the attendance records in the console
    ' Parameters:
    ' - records: The array of attendance records to display
    ' - size: The number of records to display
    Sub DisplayAttendanceRecords(ByVal records() As AttendanceRecord, size As Integer)
        Console.WriteLine("Attendance Records:") ' Display a header
        For i As Integer = 0 To size - 1
            ' Print each record's details
            Console.WriteLine($"ID: {records(i).ID}, Name: {records(i).Name}, Timestamp: {records(i).Timestamp}")
        Next
    End Sub

    ' Main entry point of the program
    Sub Main()
        ' Declare an array to hold up to 100 attendance records
        Dim records(99) As AttendanceRecord

        ' Define the filename to read attendance data from
        Dim filename As String = "attendance.txt"

        ' Read attendance records from the file
        Dim size As Integer = ReadAttendanceRecords(filename, records)

        ' Check if any records were successfully read
        If size = 0 Then
            Console.WriteLine("No data found or error reading file.")
            Return ' Exit the program if no records were read
        End If

        ' Display the records before sorting
        Console.WriteLine("Before Sorting:")
        DisplayAttendanceRecords(records, size)

        ' Sort the records alphabetically by name
        InsertionSortByName(records, size)

        ' Display the records after sorting
        Console.WriteLine(vbCrLf & "After Sorting:")
        DisplayAttendanceRecords(records, size)

        ' Wait for the user to press a key before exiting
        Console.ReadLine()
    End Sub
End Module
