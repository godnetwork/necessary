#ifdef ATTENDANCELIB_EXPORTS
#define ATTENDANCELIB_API __declspec(dllexport)
#else
#define ATTENDANCELIB_API __declspec(dllimport)
#endif

extern "C" {
    struct AttendanceRecord {
        char name[50];
        int id;
        char timestamp[50];
    };

    ATTENDANCELIB_API void addAttendanceRecord(AttendanceRecord* records, int* size, const char* name, int id, const char* timestamp);
    ATTENDANCELIB_API void insertionSortByName(AttendanceRecord* records, int size);
}
