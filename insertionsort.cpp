#include <iostream>
#include <fstream>
#include <cstring>

// Define the structure to hold an attendance record
struct AttendanceRecord {
    char name[100];    // Name of the person
    int id;            // ID of the person
    char timestamp[100]; // Timestamp of the attendance
};

// Function to add a new attendance record to the array
void addAttendanceRecord(AttendanceRecord* records, int* size, const char* name, int id, const char* timestamp) {
    std::strcpy(records[*size].name, name);      // Copy the name into the record
    records[*size].id = id;                      // Set the ID for the record
    std::strcpy(records[*size].timestamp, timestamp); // Copy the timestamp into the record
    (*size)++;                                   // Increment the size to track number of records
}

// Insertion sort function to sort the records by name alphabetically
void insertionSortByName(AttendanceRecord* records, int size) {
    for (int i = 1; i < size; i++) {             // Start from the second element
        AttendanceRecord key = records[i];       // The record to be inserted
        int j = i - 1;
        
        // Shift elements to the right to make space for the key if they are greater than the key
        while (j >= 0 && std::strcmp(records[j].name, key.name) > 0) {
            records[j + 1] = records[j];         // Shift record to the right
            j--;                                 // Move leftward
        }
        records[j + 1] = key;                     // Place the key in the correct position
    }
}

// Function to read attendance data from an external file
void readAttendanceFromFile(const char* filename, AttendanceRecord* records, int* size) {
    std::ifstream file(filename);               // Open the file for reading
    if (!file.is_open()) {                       // If the file cannot be opened
        std::cerr << "Error: Unable to open file " << filename << '\n';
        return;
    }

    // Temporary variables to hold data for each record
    char name[100];
    int id;
    char timestamp[100];

    // Read each line of the file and store the data in the records array
    while (file >> name >> id >> timestamp) {    // Read name, ID, and timestamp from the file
        addAttendanceRecord(records, size, name, id, timestamp); // Add the record to the array
    }

    file.close();                               // Close the file after reading
}

// Function to display all attendance records
void displayRecords(const AttendanceRecord* records, int size) {
    if (size == 0) {                             // If there are no records
        std::cout << "No records to display.\n";
        return;
    }

    std::cout << "Attendance Records:\n";
    for (int i = 0; i < size; i++) {             // Loop through each record and display it
        std::cout << records[i].name << ", " << records[i].id << ", " << records[i].timestamp << '\n';
    }
}

int main() {
    AttendanceRecord records[100];                // Array to store attendance records (max 100)
    int size = 0;                                 // Keep track of the number of records

    const char* filename = "attendance.txt";      // External file to read attendance records from
    readAttendanceFromFile(filename, records, &size); // Read the records from the file

    // Display the records before sorting
    std::cout << "Before Sorting:\n";
    displayRecords(records, size);

    // Sort the records by name alphabetically
    insertionSortByName(records, size);

    // Display the records after sorting
    std::cout << "\nAfter Sorting by Name:\n";
    displayRecords(records, size);

    return 0; // End of the program
}
