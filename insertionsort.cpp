#include <iostream>
#include <fstream>
#include <cstring>

struct AttendanceRecord {
    char name[100];
    int id;
    char timestamp[100];
};

void addAttendanceRecord(AttendanceRecord* records, int* size, const char* name, int id, const char* timestamp) {
    std::strcpy(records[*size].name, name);
    records[*size].id = id;
    std::strcpy(records[*size].timestamp, timestamp);
    (*size)++;
}

void insertionSortByName(AttendanceRecord* records, int size) {
    for (int i = 1; i < size; i++) {
        AttendanceRecord key = records[i];
        int j = i - 1;
        while (j >= 0 && std::strcmp(records[j].name, key.name) > 0) {
            records[j + 1] = records[j];
            j--;
        }
        records[j + 1] = key;
    }
}

void readAttendanceFromFile(const char* filename, AttendanceRecord* records, int* size) {
    std::ifstream file(filename);
    if (!file.is_open()) {
        std::cerr << "Looking for attendance.txt" << filename << '\n';
        return;
    }

    char name[100];
    int id;
    char timestamp[100];

    while (file >> name >> id >> timestamp) {  // Reads data line by line
        addAttendanceRecord(records, size, name, id, timestamp);
    }

    file.close();
}

void displayRecords(const AttendanceRecord* records, int size) {
    if (size == 0) {
        std::cout << "No records to display.\n";
        return;
    }

    std::cout << "Attendance Records:\n";
    for (int i = 0; i < size; i++) {
        std::cout << records[i].name << ", " << records[i].id << ", " << records[i].timestamp << '\n';
    }
}

int main() {
    AttendanceRecord records[100];
    int size = 0;

    const char* filename = "attendance.txt";  // External file name
    readAttendanceFromFile(filename, records, &size);

    std::cout << "Before Sorting:\n";
    displayRecords(records, size);

    insertionSortByName(records, size);

    std::cout << "\nAfter Sorting by Name:\n";
    displayRecords(records, size);

    return 0;
}
