#include "AttendanceLib.h"
#include <cstring>

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
