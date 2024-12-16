Imports System.IO ' Required for file operations
Imports System.Collections.Generic ' Required for List

Module AttendanceModule
		' Define a structure to store attendance records
		Structure AttendanceRecord
				Dim Name As String       ' Stores the name of the person
				Dim ID As Integer        ' Stores the ID of the person
				Dim Timestamp As String  ' Stores the timestamp of the attendance record
		End Structure

		' Function to read attendance records from a file
		Function ReadAttendanceRecords(ByVal filename As String) As List(Of AttendanceRecord)
				Dim records As New List(Of AttendanceRecord)() ' List to store attendance records

				' Ensure the file exists before attempting to read it
				If Not File.Exists(filename) Then
						Console.WriteLine($"Error: File '{filename}' not found.")
						Return records ' Return an empty list if the file does not exist
				End If

				Try
						' Read all lines from the file
						Dim lines() As String = File.ReadAllLines(filename)

						' Loop through the lines, assuming each record spans three lines (ID, Name, Timestamp)
						For i As Integer = 0 To lines.Length - 1 Step 3
								' Ensure there are at least 3 lines remaining to form a valid record
								If i + 2 < lines.Length Then
										Dim record As New AttendanceRecord()

										' Parse the ID (line 1 of the record)
										If Integer.TryParse(lines(i), record.ID) Then
												' Parse the Name (line 2 of the record)
												record.Name = lines(i + 1).Trim()

												' Parse the Timestamp (line 3 of the record)
												record.Timestamp = lines(i + 2).Trim()

												records.Add(record) ' Add the record to the list
										Else
												Console.WriteLine($"Error: Invalid ID format on line {i + 1}.")
										End If
								End If
						Next
				Catch ex As Exception
						Console.WriteLine($"Error reading file: {ex.Message}")
				End Try

				Return records ' Return the list of records
		End Function

		' Subroutine to sort attendance records alphabetically by name using insertion sort
		Sub InsertionSortByName(ByRef records As List(Of AttendanceRecord))
				For i As Integer = 1 To records.Count - 1
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
		Sub DisplayAttendanceRecords(ByVal records As List(Of AttendanceRecord))
				Console.WriteLine("Attendance Records:") ' Display a header
				For Each record In records
						' Print each record's details
						Console.WriteLine($"ID: {record.ID}, Name: {record.Name}, Timestamp: {record.Timestamp}")
				Next
		End Sub

		' Main entry point of the program
		Sub Main()
				' Define the filename to read attendance data from
				Dim filename As String = "attendance.txt"

				' Read attendance records from the file
				Dim records As List(Of AttendanceRecord) = ReadAttendanceRecords(filename)

				' Check if any records were successfully read
				If records.Count = 0 Then
						Console.WriteLine("No data found or error reading file.")
						Return ' Exit the program if no records were read
				End If

				' Display the records before sorting
				Console.WriteLine("Before Sorting:")
				DisplayAttendanceRecords(records)

				' Sort the records alphabetically by name
				InsertionSortByName(records)

				' Display the records after sorting
				Console.WriteLine(vbCrLf & "After Sorting:")
				DisplayAttendanceRecords(records)

				' Wait for the user to press a key before exiting
				Console.ReadLine()
		End Sub
End Module
